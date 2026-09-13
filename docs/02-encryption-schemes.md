# Encryption & Obfuscation Schemes — Last War: Survival Game 1.0.328 (MOD)

Reverse-engineered from the APK. Every layer documented below was broken and is
handled by the tooling in `scripts/`.

---

## 1. `.mdl` C# Assemblies (Mono) — XOR + length trick

**Files:** `assets/Assemblies/*.mdl` (118 assemblies, `Assembly-CSharp.mdl` = 14.2 MB)

The game ships its .NET assemblies as `.mdl` instead of `.dll`. Two layers of
anti-tamper are applied:

### 1.1 Key derivation

```text
key = data[0] XOR 0x4D      # 0x4D = 'M' of the "MZ" magic
verify: data[1] XOR key == 0x5A   # 'Z'
```

Every file uses its own single-byte key (Assembly-CSharp: `0x13`, BaseUtils:
`0x0D`, BestHttp: `0x0C`, ...).

### 1.2 Encrypted region length == the key

Only the **first `key` bytes** of the file are XOR-encrypted. The rest of the
file is plaintext. (key = 0x13 → 19 bytes, key = 0x0D → 13 bytes, ...)

### 1.3 e_lfanew anti-RE

Some files byte-swap the low two bytes of `e_lfanew` (PE header offset at
`0x3C`). The PE signature is always at offset `0x80`, so the fix is:

```python
data[i] ^= key            for i in range(key)
struct.pack_into('<I', data, 0x3C, 0x80)
```

**Tool:** `scripts/decrypt_mdl.py` — all 118 assemblies decrypt to valid PE/CLR
images (`machine = 0x14C i386`, classic Mono).

---

## 2. Unity AssetBundles — BundleFragment + offset table

**Files:** `assets/AssetBundles/BundleFragment0.bytes` (~495 MB),
`BundleOffsetTable.bytes` (767 KB), `AliasOffsetTable.bytes` (563 KB)

The ~7,037 individual `.bundle` files are packed into one big fragment file.
`BundleOffsetTable.bytes` describes the layout:

```text
u32      fragment count (1)
uleb128  fragment name ("BundleFragment0")
u32      v1
u32      entry count
entries × { uleb128 nameLen, utf8 name, u64 offset }   # sizes = next.offset - this.offset
```

Bundle names look like `gameres_<description>_<md5>.bundle`. Unity version is
stripped from bundle headers — `UnityPy.config.FALLBACK_UNITY_VERSION =
"2019.4.41f1"` is required.

**Tool:** `scripts/split_bundles.py` → 7,036 UnityFS bundles.

---

## 3. Custom Lua 5.3 bytecode (`assets/table/*.data`, 1,061 files)

See **`docs/03-lua-bytecode-format.md`** for the full spec. Summary:

- Standard Lua 5.3 opcode set and instruction encoding
- Header: `format = 1`, **four** size bytes (standard 5.3 has three — the
  `sizeof(int)` byte is dropped), LUAC_INT/LUAC_NUM sentinels intact
- Function dump: `lastlinedefined` removed
- Strings: real Lua 5.3 `[len+1 byte][bytes]` encoding with `0xFF` escape
  (escape size is 4 bytes instead of size_t)
- VM: modified `libxlua.so` (symbols intact!) — `luaV_execute`, `luaU_undump`,
  `LoadFunction`, `LoadString` all recoverable via capstone

**Tools:** `scripts/lua53_normalizer.py` (custom → standard .luac),
`scripts/table2json.py` (mini-VM executing table chunks → JSON).

Result: **1,061 decompiled `.lua` sources + 1,061 JSON data files, 0 failures.**

---

## 4. Locale archives (`assets/locale/<ver>/<lang>.bin`)

gzip-wrapped. Inner format:

```text
u32      version (2)
entries × { uleb128 keyLen, utf8 key, uleb128 valLen, utf8 value }
```

⚠️ `valLen` counts **UTF-8 code points**, not bytes. 19 languages,
**906,575 strings** total.

---

## 5. Misc

| Item | Finding |
|------|---------|
| `assets/config.db` | SQLite 3, contains a single empty `sample` table (placeholder/template) |
| `assets/AssetBundles/datatable` | INI-style version marker (`1295/294/1768181`) |
| `assets/table/version` | `31264,13758616,...` — table bundle versioning |
| `libanogs.so` | Tencent ANti-cheat (ANogs) |
| `libLITEAPKS.COM.so` | The mod menu injection library (2,140 symbols, obfuscated payload) |
| Assembly loading | `XLuaRuntime`, `XAssetProRuntime` — assemblies are memory-loaded by the custom Mono host |
