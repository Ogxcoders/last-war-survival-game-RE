# APK Structure — Last War: Survival Game 1.0.328 (MOD)

APK: `LastWar-1.0.328-mod.apk` — 733 MB, 1,930 entries, MD5
`0b6dadb90a4887d331c33c9f62cb7972`.

## Identity

| Field | Value |
|-------|-------|
| package | com.harry.lastwar.gp (via resources) |
| versionName | 1.0.328 |
| versionCode | 15 |
| Unity | 2019.4.41f1 (Mono scripting, il2cpp also present but unused for game logic) |
| install GUID | 6911fe10-7bfd-49dd-9da9-0b3b0561e664 |
| signature | META-INF/TEST.SF / TEST.RSA (self-signed, typical for modded APK) |

## Top-level layout

| Path | Size | Contents |
|------|------|----------|
| `classes.dex` … `classes9.dex` | 41 MB | 36,352 Java/Kotlin classes (Firebase, ADS SDKs, Zendesk support, AIHelp, compose UI, SmartFox client, mod glue) |
| `assets/Assemblies/*.mdl` | ~76 MB total assets dir | 118 XOR-encrypted Mono assemblies — **the entire game logic in C#** (Assembly-CSharp = 14.2 MB, 5,023 types / 48,075 methods) |
| `assets/AssetBundles/` | 495 MB | `BundleFragment0.bytes` + `BundleOffsetTable.bytes` + `AliasOffsetTable.bytes` → 7,036 UnityFS bundles |
| `assets/bin/Data/` | — | Unity boot: `level0`, `sharedassets0.assets.split0-1`, `globalgamemanagers.assets.split0-4`, `unity default resources`, boot.config |
| `assets/table/` | 13.7 MB | 1,061 custom-Lua-5.3 game data tables + version marker |
| `assets/locale/22674/*.bin` | 21 MB | 19 languages × ~47.7k strings (gzip + LEB128 format) |
| `assets/config.db` | 8 KB | SQLite placeholder (`sample` table, empty) |
| `lib/arm64-v8a/*.so` | 56 MB | see below |
| `res/`, `resources.arsc` | 2.2 MB+ | Android resources |

## Native libraries (arm64-v8a)

| Library | Size | Role |
|---------|------|------|
| `libunity.so` | 23.4 MB | Unity 2019.4.41 engine |
| `libxlua.so` | 12.4 MB | xLua — **modified Lua 5.3 VM** running the 1,061 data tables; symbols intact |
| `libanogs.so` | 5.6 MB | Tencent ANti-cheat (ANogs) |
| `libzstd.so` | 4.9 MB | Zstandard compression (bundle decompression) |
| `libLITEAPKS.COM.so` | 1.4 MB | **The mod menu** injection library (liteapks.com) — 2,140 dynamic symbols |
| `libmonoboehm-2.0.so` | 3.7 MB | Boehm GC Mono runtime |
| `libil2cpp.so` | 1.8 MB | IL2CPP host (minimal; game logic is Mono) |
| `libsmsdk.so` | 0.8 MB | SDK helper |
| `libsqlite3.so` | 1.1 MB | SQLite |
| mono-* helpers | ~1.5 MB | Mono support libs |
| `libmain.so` | 19 KB | Unity entry shim |

## Android components (highlights)

- Launcher activity chain + Unity player; DeepLink `:newinst`, `:phoenix`
- Notifications: `com.sdkmanager.notify.*`, Unity notification manager
- Support SDKs: Zendesk, AIHelp (`net.aihelp.core`)
- Ads/analytics: Firebase, AppsFlyer, ThinkingAnalytics, play-services-*
- Permissions: INTERNET, network state, WiFi state, vibration, billings, etc.

Full decoded manifest: `AndroidManifest.decoded.txt`; class inventory:
`dex/all_classes.txt`.

## Mod analysis notes

`libLITEAPKS.COM.so` ships the menu UI/injection for the MOD (unlimited
resources etc.). Its payload is obfuscated; symbol table exported. The Java-side
glue references `com.liteapks.*` classes inside `classes7-9.dex`.
