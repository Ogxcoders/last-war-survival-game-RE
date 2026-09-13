# Last War: Survival Game — Complete Reverse Engineering

Full reverse-engineering corpus for **Last War: Survival Game** (`1.0.328`, MOD APK),
recovered from the encrypted Unity/Mono build. Everything in this repository was
extracted or decoded with the tooling in [`scripts/`](scripts/) — no leaks, no
re-uploads of third-party dumps.

## What is here

| Directory | Contents |
|-----------|----------|
| [`Animations/`](Animations/) | **2,481 Unity AnimationClips (full curve JSON) + 447 AnimatorControllers + 86 Spine skeletons** |
| [`Heroes/`](Heroes/) `Zombies/` `Characters/` `Monsters/` `World_Buildings/` `Models_Anim/` | 62+ hero texture sets, OBJ meshes, zombie/monster/vehicle/building models |
| [`UI_Icons/`](UI_Icons/) `Activity_UI/` `Effects/` | 15k UI sprites & atlases, Spine skeleton data, VFX textures |
| [`Audio/`](Audio/) `Fonts/` | 562 WAV clips (music/SFX/voice), 24 TTF/OTF fonts |
| [`Core_Data/`](Core_Data/) | Unity boot data (`level0`, `sharedassets0`, `globalgamemanagers`, `unity default resources`) + bundle version markers |
| [`csharp_src/`](csharp_src/) | **All 118 Mono assemblies fully decompiled to C# source** (ILSpy 9.1) — **16,971 `.cs` files**, one per type, incl. complete `Assembly-CSharp` game logic |
| [`csharp_meta/`](csharp_meta/) | Type/method/field index of all 118 decrypted assemblies: **24,284 types / 224,551 methods / 120,939 fields** |
| [`tables_json/`](tables_json/) | **All 1,061 game data tables** (custom Lua 5.3 bytecode) converted to JSON via a purpose-built mini-VM |
| `tables_decompiled_lua.tar.gz` | All 1,061 tables decompiled to readable Lua source (unluac) |
| [`locale/`](locale/) | **906,575 localized strings** across 19 languages |
| [`dex/`](dex/) | 36,352 Java/Kotlin class inventory |
| [`native_libs/`](native_libs/) | ELF inventory of all 21 arm64 libraries |
| `AndroidManifest.decoded.txt` | Full decoded binary manifest |
| `_manifest_all.csv` | Master index of all 24,723 extracted asset files → source bundle |
| [`docs/`](docs/) | The reverse-engineering write-ups (encryption, formats) |
| [`scripts/`](scripts/) | Every tool built along the way |

## Downloads (Release `assets-1.0.328`)

| File | Size | Contents |
|------|------|----------|
| `Assets_Part1_UI_Icons.zip` | 498 MB | UI sprites, atlases, Spine, activity UI |
| `Assets_Part2_Audio_Fonts.zip` | 137 MB | 562 WAVs + 24 fonts |
| `Assets_Part3_Models_World.zip` | 343 MB | heroes/zombies/monsters/vehicles/buildings |
| `Assets_Part4_Effects_Other.zip` | 91 MB | VFX + misc |
| `csharp_src.zip` | 22 MB | all 118 assemblies → 16,971 `.cs` files |
| `tables_lua.zip` | 14 MB | 1,061 tables as readable Lua |
| `tables_json.zip` | 12 MB | 1,061 tables as JSON |
| `locale.zip` | 24 MB | 906k strings, 19 languages |
| `animations.zip` | 66 MB | all Unity clips + animators + Spine skeletons |
| `RE_core.zip` | 2 MB | docs + scripts + type index + inventories + manifest |

## Documentation

1. **[APK structure](docs/01-apk-structure.md)** — layout, native libraries, Android components, mod analysis
2. **[Encryption & obfuscation schemes](docs/02-encryption-schemes.md)** — `.mdl` XOR scheme, AssetBundle fragment/offset table, locale archives
3. **[Custom Lua 5.3 bytecode format](docs/03-lua-bytecode-format.md)** — the full reverse-engineered spec (header, LoadString, LoadFunction), verified by disassembling `libxlua.so`

## C# source quick links (`csharp_src/`)

| Path | What to look at |
|------|-----------------|
| `Assembly-CSharp/` | The entire game: 3,073 types — managers (`WorldTroop.cs`, `WorldMarchDataManager.cs`), PVE/season logic, UI, economy |
| `Assembly-CSharp/AES.cs`, `AesEncryptor.cs` | In-game crypto helpers |
| `BaseUtils/`, `DefinesRuntime/` | Core utilities & constant definitions |
| `SmartFox2X/`, `Smartfox2xLw/` | Network protocol client used for multiplayer |
| `BestHttp/`, `FM_MonoLib/` | HTTP stack & SDK glue |

Generated with `scripts/decompile_cs.py` (ilspycmd 9.1 under .NET 8 runtime): one `.cs` per type in nested namespace folders, plus a `.csproj` per assembly.

## Headline reversals

- **`.mdl` assemblies** — per-file XOR key (`data[0] ^ 'M'`), key value == encrypted
  region length, plus an `e_lfanew` byte-swap. All 118 decrypt to valid PE/CLR images.
- **AssetBundle packing** — 7,036 bundles reconstructed from one 495 MB fragment +
  ULEB128-based offset table.
- **Custom Lua 5.3** — `format=1` header, `sizeof(int)` header byte dropped,
  `lastlinedefined` removed, byte-prefixed strings. Proven by static analysis of
  the modified VM (`luaV_execute`, `luaU_undump`, `LoadFunction`, `LoadString`)
  and by decompiling all 1,061 data tables with zero failures.
- **Locale format** — gzip + `u32 version + LEB128-length-prefixed UTF-8 pairs`.

## Data table anatomy

```lua
return {
  index = {                       -- column schema
    id    = {1, "number"},
    point = {2, "number"},
    reward= {3, "string", true},
  },
  data = { ... }                  -- rows
}
```

Tables cover heroes, buildings, PVE triggers, seasons, monsters, skills,
economy, activities and more (`tables_json/` has one JSON per table).

## Network footprint

- Game protocol: SmartFox2X (`SmartFox2X.mdl` / `Smartfox2xLw.mdl`)
- Analytics: `https://shumei-api.lastwargame.com:19091/...` (deviceprofile / cloudconf)
- Anti-cheat: Tencent ANogs (`libanogs.so`)
- MOD: `libLITEAPKS.COM.so` injection library (liteapks.com menu)

## Reproduction pipeline

```
APK ─ unzip ─► assets/Assemblies/*.mdl ─ decrypt_mdl.py ─► .dll ─ decompile_cs.py ─► csharp_src/ (C# source)
                                              └─ analyze_dotnet.py ─► csharp_meta/
    └─────────► assets/AssetBundles ─ split_bundles.py ─► 7,036 bundles ─ extract_assets_4parts.py ─► assets/
                assets/table/*.data ─ lua53_normalizer.py ─► .luac ─ unluac ─► .lua
                                     └───────────── table2json.py ─► tables_json/
                assets/locale/*.bin ─ LEB128 parser ─► locale/*.json
```

## Legal

Game assets and code belong to their respective owners (Last War: Survival Game).
This repository is for **educational and research purposes only**.
