#!/usr/bin/env python3
"""
Mini Lua 5.3 VM for Last War data-table chunks -> JSON.

Executes the top-level proto of each custom-format chunk (parsed by
lua53_normalizer.parse) with the subset of opcodes used by table-constructor
code, then serializes the resulting table to JSON.
"""
import struct, os, sys, json, math
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from lua53_normalizer import parse, Proto

MAXARG_C = 255

class LuaTable:
    __slots__ = ('arr', 'hash')
    def __init__(self):
        self.arr = {}      # 1-based int keys -> value (sparse-safe)
        self.hash = {}
    def set(self, key, val):
        if isinstance(key, float) and key.is_integer():
            key = int(key)
        if isinstance(key, int) and key >= 1:
            self.arr[key] = val
        else:
            if isinstance(key, LuaTable):
                key = ('@table', id(key))
            self.hash[key] = val
    def get(self, key):
        if isinstance(key, float) and key.is_integer():
            key = int(key)
        if isinstance(key, int) and key in self.arr:
            return self.arr[key]
        if isinstance(key, LuaTable):
            key = ('@table', id(key))
        return self.hash.get(key)

def kvalue(k):
    t, v = k
    if t == 0: return None
    if t == 1: return bool(v)
    return v

def rk(x, regs, consts):
    if x < 256:
        return regs[x]
    return kvalue(consts[x - 256])

def truthy(v):
    return v is not None and v is not False

def execute(proto, maxsteps=8_000_000):
    p = proto
    nregs = max(p.maxstack + 2, 8)
    regs = [None] * 300
    open_tables = []  # keep references alive for id() keys
    pc = 0
    steps = 0
    ncode = len(p.code)
    while True:
        steps += 1
        if steps > maxsteps:
            raise RuntimeError('step limit')
        if pc >= ncode:
            return None  # fell off end
        i = p.code[pc]
        op = i & 0x3F
        A = (i >> 6) & 0xFF
        C = (i >> 14) & 0x1FF
        B = (i >> 23) & 0x1FF
        pc += 1
        if op == 11:      # NEWTABLE
            regs[A] = LuaTable()
            open_tables.append(regs[A])
        elif op == 1:     # LOADK
            Bx = i >> 14
            regs[A] = kvalue(p.k[Bx]) if Bx < len(p.k) else None
        elif op == 2:     # LOADKX
            ea = p.code[pc]; pc += 1
            Ax = (ea >> 6) & 0x3FFFFFF
            regs[A] = kvalue(p.k[Ax]) if Ax < len(p.k) else None
        elif op == 3:     # LOADBOOL
            regs[A] = bool(B)
            if C: pc += 1
        elif op == 4:     # LOADNIL
            for j in range(A, A + B + 1):
                regs[j] = None
        elif op == 0:     # MOVE
            regs[A] = regs[B]
        elif op == 10:    # SETTABLE R(A)[RK(B)] := RK(C)
            t = regs[A]
            key = rk(B, regs, p.k)
            val = rk(C, regs, p.k)
            if isinstance(t, LuaTable):
                t.set(key, val)
        elif op == 8:     # SETTABUP UpVal(A)[RK(B)] := RK(C)
            pass  # global writes ignored (data chunks don't need them)
        elif op == 43:    # SETLIST
            t = regs[A]
            if B == 0:
                B = 0
                while B < 64 and A + 1 + B < len(regs) and regs[A + 1 + B] is not None:
                    B += 1
            if C == 0:
                ea = p.code[pc]; pc += 1
                Ax = (ea >> 6) & 0x3FFFFFF
                base = Ax * (MAXARG_C + 1)
            else:
                base = (C - 1) * (MAXARG_C + 1)
            for j in range(1, B + 1):
                t.set(base + j, regs[A + j])
        elif op == 38:    # RETURN
            if B == 0:
                n = 0
                while n < 64 and A + n < len(regs) and regs[A + n] is not None:
                    n += 1
                vals = [regs[A + k] for k in range(n)]
            elif B == 1:
                vals = []
            else:
                vals = [regs[A + k] for k in range(B - 1)]
            return vals[0] if len(vals) == 1 else (vals if vals else None)
        elif op == 47:    # EXTRAARG (standalone: skip)
            pass
        elif op == 44:    # CLOSURE
            Bx = i >> 14
            regs[A] = ('closure', Bx)
        elif op == 36:    # CALL - data chunks may call nothing meaningful
            Bc = B; Cc = C
            if Bc == 0:
                Bc = 0
            # treat function calls as no-op producing nothing
            nres = Cc - 1 if Cc >= 1 else 0
            for j in range(nres):
                regs[A + j] = None
        elif op == 5:     # GETUPVAL
            regs[A] = None
        elif op == 6:     # GETTABUP R(A) := UpVal(B)[RK(C)]
            regs[A] = rk(C, regs, p.k)  # assume missing globals -> their key name? use None
            regs[A] = None
        elif op == 7:     # GETTABLE
            t = regs[B]
            key = rk(C, regs, p.k)
            regs[A] = t.get(key) if isinstance(t, LuaTable) else None
        elif op == 30:    # JMP
            sBx = (i >> 14) - 131071
            pc += sBx
        elif op in (31, 32, 33):  # EQ/LT/LE
            a = rk(A + 1 if False else B, regs, p.k)  # placeholder
            # standard: if ((RK(B) == RK(C)) ~= A) then pc++
            lb = rk(B, regs, p.k); lc = rk(C, regs, p.k)
            if op == 31: cond = (lb == lc)
            elif op == 32: cond = (lb < lc)
            else: cond = (lb <= lc)
            if truthy(cond) != bool(A & 1):
                pc += 1  # skip next JMP
            pc += 1  # always skip the JMP slot? no: skip if condition met
            pc -= 1
            # simplify: comparisons in data chunks are rare; treat conservatively
        elif op == 34:    # TEST
            if truthy(regs[A]) != bool(C):
                pc += 1
        elif op == 35:    # TESTSET
            if truthy(regs[B]) == bool(C):
                regs[A] = regs[B]
            else:
                pc += 1
        elif op == 13:    # ADD
            regs[A] = _arith(regs[B], rk(C, regs, p.k), 'add')
        elif op == 14:
            regs[A] = _arith(regs[B], rk(C, regs, p.k), 'sub')
        elif op == 15:
            regs[A] = _arith(regs[B], rk(C, regs, p.k), 'mul')
        elif op == 28:    # LEN
            v = regs[B]
            regs[A] = (max(v.arr) if v.arr else 0) if isinstance(v, LuaTable) else (len(v) if isinstance(v, (str, list)) else 0)
        elif op == 12:    # SELF
            t = regs[B]
            key = rk(C, regs, p.k)
            regs[A + 1] = t
            regs[A] = t.get(key) if isinstance(t, LuaTable) else None
        elif op == 45:    # VARARG
            pass
        else:
            raise RuntimeError(f'unhandled opcode {op} at pc {pc-1}')

def _arith(a, b, how):
    try:
        if how == 'add': return a + b
        if how == 'sub': return a - b
        if how == 'mul': return a * b
    except Exception:
        return 0
    return 0

def to_jsonable(v, depth=0, seen=None):
    if seen is None: seen = set()
    if depth > 64: return '<deep>'
    if isinstance(v, LuaTable):
        key = id(v)
        if key in seen: return '<cycle>'
        seen.add(key)
        try:
            out = {}
            for idx, item in v.arr.items():
                if item is not None:
                    out[str(idx)] = to_jsonable(item, depth + 1, seen)
            for k, item in v.hash.items():
                if item is None: continue
                kk = k if isinstance(k, str) else str(k)
                out[kk] = to_jsonable(item, depth + 1, seen)
        finally:
            seen.discard(key)
        if out and v.hash == {} and all(k.isdigit() for k in out):
            keys = sorted(int(k) for k in out)
            if keys == list(range(1, len(keys) + 1)):
                return [out[str(k)] for k in keys]
        return out
    if isinstance(v, float):
        if v.is_integer() and abs(v) < 1e15:
            return int(v)
        return v
    if isinstance(v, (int, str, bool)) or v is None:
        return v
    if isinstance(v, tuple) and v and v[0] == 'closure':
        return f'<closure#{v[1]}>'
    return str(v)

def convert_file(path):
    data = open(path, 'rb').read()
    nup, f = parse(data)
    result = execute(f)
    return to_jsonable(result)

def _work(args):
    name, src, dst = args
    try:
        obj = convert_file(os.path.join(src, name))
        empty = (obj is None or obj == {})
        with open(os.path.join(dst, name + '.json'), 'w', encoding='utf-8') as fo:
            json.dump(obj, fo, ensure_ascii=False, separators=(',', ':'))
        return (name, None, empty)
    except Exception as e:
        return (name, f'{type(e).__name__}: {e}', False)

if __name__ == '__main__':
    from multiprocessing import Pool
    src, dst = sys.argv[1], sys.argv[2]
    os.makedirs(dst, exist_ok=True)
    files = sorted(os.listdir(src))
    ok, fail, empty = 0, [], 0
    with Pool(4, maxtasksperchild=1) as pool:
        for name, err, emp in pool.imap_unordered(_work, [(n, src, dst) for n in files], chunksize=1):
            if err:
                fail.append((name, err))
            else:
                ok += 1
                if emp: empty += 1
    print(f'converted: {ok}/{len(files)} (empty: {empty}), failed: {len(fail)}')
    for n, e in fail[:20]:
        print(' FAIL', n, e)
