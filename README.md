# Last War: Survival Game 1.0.328 (MOD) — Reverse-Engineered Game Assets

Complete asset extraction from the Android APK
`Last-War-Survival-Game-1.0.328-mod.apk` (Unity 2019.4.41f1, Mono + xLua).

**Source**: 7,037 Unity AssetBundles inside `assets/AssetBundles/BundleFragment0.bytes`
(495 MB, split via the game's custom `BundleOffsetTable.bytes` — ULEB128-prefixed
names + u64 offsets), plus core boot data from `assets/bin/Data/`.

## Contents

| Folder | What's inside |
|---|---|
| `UI_Icons/` | 14,000+ UI sprites, icons, atlases, Spine `.skel`/`.atlas` data |
| `Activity_UI/` | Seasonal/event UI (Easter 2025, limited-time events) |
| `Audio/` | WAV audio — `Music/`, `SFX/`, `Voice/` (hero dubbing, plot lines), `Misc/` |
| `Fonts/` | Game fonts (TTF/OTF) |
| `Core_Data/` | Boot assets from `bin/Data` (unity default resources, sharedassets, globalgamemanagers, level0) |
| `Heroes/` | Per-hero `textures/` (diffuse/normal/shadow) + `mesh/*.obj` 3D models |
| `Zombies/` `Characters/` `Monsters/` | Enemy & unit textures + OBJ meshes |
| `World_Buildings/` | Building/environment textures + world edge data |
| `Models_Anim/` | Vehicle & shared model textures |
| `Effects/` | VFX textures (glow/smoke/noise/mask) + effect meshes |
| `Other/` | Auto-packaged misc assets |

`_manifest.csv` maps every file back to its source bundle
(`part,category,type,name,file,bundle`).

## Formats

- Textures/Sprites → **PNG**
- Audio → **WAV**
- 3D models → **Wavefront OBJ**
- Fonts → **TTF/OTF**
- Spine/data → **TXT**

## Also available

The same assets packaged as 4 downloadable zips:
`LastWar_1.0.328_Assets_Part1_UI_Icons.zip` (475 MB),
`Part2_Audio_Fonts.zip` (131 MB), `Part3_Models_World.zip` (328 MB),
`Part4_Effects_Other.zip` (87 MB).
