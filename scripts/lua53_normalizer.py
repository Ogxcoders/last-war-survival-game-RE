#!/usr/bin/env python3
"""
Last War custom Lua 5.3 bytecode -> standard Lua 5.3 (format 0) normalizer.

Reverse-engineered format (verified against libxlua.so disassembly):

HEADER:
  sig "\x1bLua" (4) | version 0x53 (1) | format 0x01 (1) | LUAC_DATA 19 93 0d 0a 1a 0a (6)
  | size bytes (4): Instruction=4, X=4, lua_Integer=8, lua_Number=8  [standard has 5; int-size dropped]
  | LUAC_INT 0x5678 (8) | LUAC_NUM 370.5 (8)
  | nupvalues (1)
  -> 32 bytes total

FUNCTION (LoadFunction @0x5a004):
  source:      LoadString
  linedefined: 4B int
  lastlinedefined: 4B int
  numparams, is_vararg, maxstacksize: 1 byte each
  code:        4B count + count x 4B instructions
  constants:   4B count + per: tag(1B): 0=nil, 1=bool+1B, 3=flt+8B, 0x13=int+8B, 4/0x14=string
  upvalues:    4B count + per (instack 1B, idx 1B)
  protos:      4B count + recursive LoadFunction
  lineinfo:    4B count + count x 4B int
  locvars:     4B count + per (LoadString name, 4B startpc, 4B endpc)
  upvalnames:  4B count + per LoadString

LoadString (0x5a634):
  1 byte n: 0 -> NULL; 0xFF -> next 4B uint32 N (0 -> NULL, else len=N-1); else len = n-1 (max 40)

INSTRUCTIONS: 4B LE, Lua 5.3 opcode numbering. Evidence-based field rule:
  - MOST iABC ops: B@23, C@14 (swapped vs standard 5.3)  [SETLIST count=7@23 test,
    SETTABLE key=K@23/value=R@14 test, RETURN nret+1@23 test]
  - NEWTABLE (op 11): B@14 (hash), C@23 (array) - standard positions
    [verified via luaH_resize(L,t,arr@w2,hash@w3) param roles + 2-key/14-key data]
  - iABx/iAsBx/iAx ops (LOADK, JMP, FORLOOP/FORPREP/TFORLOOP, CLOSURE, EXTRAARG):
    Bx/sBx/Ax at @14/@6, identical in both layouts.

OUTPUT: standard Lua 5.3 format-0 chunk (size_t=8) that unluac can decompile.
"""
import struct, os, sys

# instructions pass through UNCHANGED: game body is already real Lua 5.3 encoding

class Reader:
    def __init__(self, data):
        self.d = data
        self.p = 0
    def bytes(self, n):
        b = self.d[self.p:self.p+n]
        if len(b) < n:
            raise EOFError(f'want {n} at {self.p}, file len {len(self.d)}')
        self.p += n
        return b
    def byte(self):
        return self.bytes(1)[0]
    def int32(self):
        return struct.unpack('<i', self.bytes(4))[0]
    def uint32(self):
        return struct.unpack('<I', self.bytes(4))[0]
    def int64(self):
        return struct.unpack('<q', self.bytes(8))[0]
    def float64(self):
        return struct.unpack('<d', self.bytes(8))[0]
    def loadstring(self):
        n = self.byte()
        if n == 0:
            return None
        if n == 0xFF:
            N = self.uint32()
            if N == 0:
                return None
            n = N - 1
        else:
            n = n - 1
        return self.bytes(n)

class Proto:
    __slots__ = ('source','linedef','lastline','numparams','is_vararg','maxstack',
                 'code','k','upvals','protos','lineinfo','locvars','upvalnames')

def load_function(r):
    f = Proto()
    f.source = r.loadstring()
    f.linedef = r.int32()
    f.lastline = r.int32()
    f.numparams = r.byte()
    f.is_vararg = r.byte()
    f.maxstack = r.byte()
    n = r.int32()
    f.code = [struct.unpack('<I', r.bytes(4))[0] for _ in range(n)]
    n = r.int32()
    ks = []
    for _ in range(n):
        t = r.byte()
        if t == 0:      ks.append((0, None))
        elif t == 1:    ks.append((1, r.byte()))
        elif t == 3:    ks.append((3, r.float64()))
        elif t == 0x13: ks.append((0x13, r.int64()))
        elif t in (4, 0x14): ks.append((t, r.loadstring()))
        else: raise ValueError(f'bad const tag {t:#x} at {r.p}')
    f.k = ks
    n = r.int32()
    f.upvals = [(r.byte(), r.byte()) for _ in range(n)]
    n = r.int32()
    f.protos = [load_function(r) for _ in range(n)]
    n = r.int32()
    f.lineinfo = [r.int32() for _ in range(n)]
    n = r.int32()
    f.locvars = [(r.loadstring(), r.int32(), r.int32()) for _ in range(n)]
    n = r.int32()
    f.upvalnames = [r.loadstring() for _ in range(n)]
    return f

def check_header(r):
    assert r.bytes(4) == b'\x1bLua', 'bad sig'
    ver = r.byte(); assert ver == 0x53, f'ver {ver:#x}'
    fmt = r.byte(); assert fmt == 1, f'fmt {fmt}'
    assert r.bytes(6) == b'\x19\x93\r\n\x1a\n', 'bad LUAC_DATA'
    sizes = r.bytes(4)
    assert sizes == bytes([4, 4, 8, 8]), f'sizes {sizes.hex()}'
    assert r.int64() == 0x5678, 'LUAC_INT'
    assert abs(r.float64() - 370.5) < 1e-9, 'LUAC_NUM'

def parse(data):
    r = Reader(data)
    check_header(r)
    nup = r.byte()
    f = load_function(r)
    return nup, f

def norm_function(f):
    pass  # no code rewriting needed

# ---- standard 5.3 serializer (size_t = 8) ----
class Writer:
    def __init__(self):
        self.buf = bytearray()
    def byte(self, b): self.buf.append(b & 0xFF)
    def raw(self, b): self.buf.extend(b)
    def int32(self, v): self.buf.extend(struct.pack('<i', v))
    def size(self, v): self.buf.extend(struct.pack('<Q', v))
    def wstring(self, s):
        if s is None:
            self.byte(0)
        else:
            # real Lua 5.3 (5.3.3+) format: [1 byte len+1][len bytes]
            # 0xFF escape -> [0xFF][size_t len+1][len bytes]
            n = len(s) + 1
            if n < 0xFF:
                self.byte(n)
            else:
                self.byte(0xFF)
                self.size(n)
            self.raw(s)

def dump_function(w, f):
    w.wstring(f.source)
    w.int32(f.linedef)
    w.int32(f.lastline)
    w.byte(f.numparams); w.byte(f.is_vararg); w.byte(f.maxstack)
    w.int32(len(f.code))
    for i in f.code:
        w.raw(struct.pack('<I', i))
    w.int32(len(f.k))
    for t, v in f.k:
        w.byte(t)
        if t == 1: w.byte(v)
        elif t == 3: w.raw(struct.pack('<d', v))
        elif t == 0x13: w.raw(struct.pack('<q', v))
        elif t in (4, 0x14): w.wstring(v)
    w.int32(len(f.upvals))
    for a, b in f.upvals:
        w.byte(a); w.byte(b)
    w.int32(len(f.protos))
    for p in f.protos:
        dump_function(w, p)
    w.int32(len(f.lineinfo))
    for l in f.lineinfo:
        w.int32(l)
    w.int32(len(f.locvars))
    for name, s, e in f.locvars:
        w.wstring(name); w.int32(s); w.int32(e)
    w.int32(len(f.upvalnames))
    for n in f.upvalnames:
        w.wstring(n)

def normalize(data):
    nup, f = parse(data)
    norm_function(f)
    w = Writer()
    w.raw(b'\x1bLua\x53\x00\x19\x93\r\n\x1a\x0a')
    w.byte(4); w.byte(8); w.byte(4); w.byte(8); w.byte(8)
    w.raw(struct.pack('<q', 0x5678))
    w.raw(struct.pack('<d', 370.5))
    w.byte(nup)
    dump_function(w, f)
    return bytes(w.buf)

def tree_stats(f):
    acc = [0, 0, 0]
    def rec(p):
        acc[0] += len(p.code); acc[1] += len(p.k); acc[2] += 1
        for q in p.protos: rec(q)
    rec(f)
    return acc

if __name__ == '__main__':
    src, dst = sys.argv[1], sys.argv[2]
    os.makedirs(dst, exist_ok=True)
    files = sorted(os.listdir(src))
    ok, fail = 0, []
    for name in files:
        try:
            data = open(os.path.join(src, name), 'rb').read()
            std = normalize(data)
            open(os.path.join(dst, name + '.luac'), 'wb').write(std)
            ok += 1
        except Exception as e:
            fail.append((name, f'{type(e).__name__}: {e}'))
    print(f'normalized: {ok}/{len(files)}, failed: {len(fail)}')
    for n, e in fail[:15]:
        print(' FAIL', n, e)
