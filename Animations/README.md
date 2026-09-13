# Animations — Last War: Survival Game 1.0.328

Every animation asset recovered from the 7,036 AssetBundles plus all Spine
skeletal data.

| Directory | Contents |
|-----------|----------|
| `UnityClips/` | **2,481 Unity `AnimationClip` assets** — full serialized typetree as JSON (rotation/position/scale/euler/float/PPtr curves, `m_MuscleClip` dense data, bindings, sample rate). One folder per source bundle. |
| `Animators/` | **447 `AnimatorController` / `AnimatorOverrideController` assets** — state machines, layers, transitions, parameter tables (JSON). |
| `Spine/` | **86 Spine skeleton files** (`.skel` + `.atlas` as `.txt`, plus spine textures) — hero icons, NPCs, emoji/biaoqing, activity UI. |
| `_clips_index.csv` | Machine-readable index of every clip (JSON-lines): name, source bundle, legacy/compressed flags, sample rate, per-curve-type counts, file size. |

## Notes

- All 2,481 clips are **uncompressed** (`m_Compressed = false`): 2,446 Generic
  (humanoid/muscle) + 35 Legacy. Curve data is complete and directly parseable.
- Hero anim bundles follow the pattern
  `gameres_art_dir_cars_a_hero_<name>_animation_*` — 60+ heroes covered
  (farhad, hager, katyusha, kilian, lambo, maxwell, david, monica, doctor, …).
- Largest clips are the hero "show" cinematic animations
  (`David_01_show` 4.5 MB of curves).
- 3D unit motion is partly driven by **GPUSkinning** (see
  `csharp_src/GPUSkinning/` in the repo root) — those bake to textures rather
  than AnimationClips, so they live with the model textures, not here.
- JSON files are the raw Unity typetrees — load with any JSON parser; the
  schema matches Unity 2019.4's `AnimationClip` / `AnimatorController`
  definitions (fields documented in the Unity docs).
