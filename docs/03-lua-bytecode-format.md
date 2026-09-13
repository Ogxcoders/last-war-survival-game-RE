# Last War custom Lua 5.3 bytecode format

Reverse-engineered by (a) statistical analysis of 1,061 sample files, (b)
disassembly of the modified `libxlua.so` (ARM64, symbols intact: `luaU_undump`
@0x59ce4, `LoadFunction` @0x5a004, `LoadString` @0x5a634, `luaV_execute`
@0x5c460), and (c) cross-validation via unluac decompilation of all files.

The format is **real Lua 5.3 bytecode** with four deliberate deviations.

---

## 1. Header (32 bytes vs standard 31)

```text
offset  size  field
0       4     signature  "\x1bLua"
4       1     version    0x53
5       1     format     0x01        <-- standard is 0x00
6       6     LUAC_DATA  19 93 0D 0A 1A 0A
12      4     sizes:     sizeof(Instruction)=4, X=4, sizeof(lua_Integer)=8, sizeof(lua_Number)=8
                        ^^^ FOUR bytes; standard 5.3 has THREE (Instruction, Integer, Number).
                        the extra second byte corresponds to the dropped sizeof(int).
16      8     LUAC_INT   0x5678 (int64 LE)
24      8     LUAC_NUM   370.5  (double LE)
32      1     nupvalues of main closure (usually 1 = _ENV)
```

Verified against `luaU_undump` disassembly: the loader checks `0x1b`, version
`0x53`, format `== 1`, LUAC_DATA, then four size checks `4,4,8,8`, then the
`0x5678` integer sentinel.

## 2. LoadString (0x5a634) — byte-prefixed with 0xFF escape

```text
byte n:
  n == 0     -> NULL string
  n == 0xFF  -> u32 N follows (little endian); N == 0 -> NULL, else len = N-1
  else       -> len = n - 1 (max 40), followed by len raw bytes
```

This is the same scheme as stock Lua 5.3.3+ **except** the 0xFF escape uses a
4-byte length instead of `size_t` (8 bytes on 64-bit).

## 3. Function dump (LoadFunction @0x5a004)

```text
source          LoadString
linedefined     i32
lastlinedefined i32        <-- REMOVED in the custom format (present in stock 5.3)
numparams       byte
is_vararg       byte
maxstacksize    byte
code            i32 count + count × u32 instructions
constants       i32 count + per constant: tag byte + payload
                tags: 0x00 nil | 0x01 bool+1B | 0x03 float+8B | 0x13 int+8B | 0x04/0x14 string
upvalues        i32 count + per: instack(1B), idx(1B)
protos          i32 count + recursive LoadFunction
lineinfo        i32 count + count × i32
locvars         i32 count + per: LoadString name, i32 startpc, i32 endpc
upvalnames      i32 count + per: LoadString
```

(When `lastlinedefined` is removed the parser must simply skip it; the
normalized output duplicates `linedefined` into that slot.)

## 4. Instructions — stock 5.3 encoding, VM quirks

Instruction words are **unmodified Lua 5.3 iABC** words:

```text
op(6)@0 | A(8)@6 | C(9)@14 | B(9)@23      Bx(18)@14, sBx@14 (offset 131071), Ax(26)@6
```

This was confirmed with capstone: `luaV_execute` extracts operands via
`ubfx #14,#9` and `lsr #23`, matching the stock 5.3 layout (`POS_C = 14`,
`POS_B = 23` — note 5.3 puts **C before B**).

One VM quirk found: the modified `OP_NEWTABLE` handler calls
`luaH_resize(L, t, f(F23), f(F14))` where `luaH_resize(L,t,array,hash)` —
i.e. the compiler stores `array size` in the field @23 and `hash log2` in the
field @14, the **reverse of stock 5.3**. Since NEWTABLE sizes are only
pre-allocation hints, this has no semantic effect on decompilation or execution.

## 5. Normalization pipeline

`scripts/lua53_normalizer.py` rewrites each chunk into a **stock Lua 5.3
format-0** bytecode:

1. header → format 0, sizes `int=4, size_t=8, Instruction=4, Integer=8, Number=8`
2. re-insert `lastlinedefined` (duplicate of `linedefined`)
3. strings re-encoded with 8-byte size_t escape for `0xFF` long strings
4. instructions copied verbatim

The output is consumed by unluac (rebuilt from source with ecj — see
`scripts/unluac_src/`) and by `scripts/table2json.py`, a mini-VM that executes
the table-constructor opcodes (NEWTABLE/SETTABLE/SETLIST/LOADK/LOADNIL/
LOADBOOL/CALL/RETURN/…) and serializes the result to JSON.

## 6. What the tables contain

Each `assets/table/<name>_<md5>.data` is a compiled Lua chunk returning one
table. Example `daily_reward`:

```lua
return {
  index = {
    id = {1, "number"},
    point = {2, "number"},
    reward = {3, "string", true},
    icon_show = {4, "string"},
    activity_reward = {5, "string"},
    builders_alliance_reward = {6, "string"},
    timing_reward = {7, "string"},
  },
  data = { ... }   -- row arrays
}
```

`index` = column schema (name → [ordinal, type, flags...]), `data` = rows.
All 1,061 tables decompiled with **0 failures**.
