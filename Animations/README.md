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

## Unity-ready `.anim` files (Release: `anim_unity_clips.zip`)

All 2,409 readable clips were converted to **Unity YAML `.anim` files**
(`scripts/gen_anim.py`), following AssetRipper's `AnimationClipConverter`
logic exactly:

- streamed / dense / constant curve streams decoded
  (streamed key = cubic-Hermite `[index, coefX, coefY, coefZ, value]`),
- transform bindings mapped by attribute enum
  (1 = Translation, 2 = Rotation, 3 = Scaling, 4 = EulerHint),
- bone paths recovered by CRC32-hashing every GameObject hierarchy in the
  game (166,811 paths — Unity's path ID is `crc32(path)`, verified 217/217
  against Avatar `m_TOS` maps),
- script/engine float properties resolved through a CRC32 dictionary built
  from all 118 assemblies' field names (91% coverage).

Validation: 55,354 sampled rotation keyframes, **0 non-unit quaternions**.
Drop the `.anim` files next to a matching rig (or re-path them) inside
Unity. ~6% of curves reference paths whose rig hierarchy ships outside the
animation bundles (`UnknownPath_*`); re-point those manually if needed.
PPtr (sprite-swap) curves are not converted.

## Unity import kit (`anim_unity_pack.zip` on the Release)

The one-stop download for using the animations directly in Unity:

- `UnityClips/` — the 2,409 `.anim` files (clean names, `.meta` included,
  deterministic GUIDs = `md5("LastWar.anim.<bundle>.<clip>")`)
- `Animators/` — all 447 `.controller` files with the **full state machine graph
  reconstructed from the build-format controller constants**:
  - states + names (from the engine's `m_TOS` debug-name table), speeds, loop flags,
  - state transitions, AnyState transitions (durations, exit times, interruption),
  - conditions with parameter names recovered by CRC32 (`attack`, `dead`,
    `walking`, `Blend`, ... — 218 of 219 references resolved),
  - the zombie 1D blend tree,
  - the additive synchronized layer of `ZhuanwuzahnshiController`,
  - every state's motion wired to its `.anim` by GUID — **0 unresolved references**.

Entry/Exit selector plumbing is implicit in Unity and omitted. Parameters used
with If/IfNot conditions are emitted as bools — rename to triggers in the
Animator window if your code expects them. Conversion logic mirrors AssetRipper's
`VirtualAnimationFactory` / `AnimatorStateMachineContext`; see
`scripts/gen_controllers.py` + `scripts/scan_ctrl_deps.py` in the repo.
