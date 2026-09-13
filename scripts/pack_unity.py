#!/usr/bin/env python3
"""Stream-build the Unity-ready animation pack zip (no on-disk duplication):
  UnityClips/<bundle>/<stem>.anim + .anim.meta   (clean names, .AnimationClip suffix stripped)
  Animators/<bundle>/<stem>.controller + .controller.meta
  README.md
"""
import os
import zipfile

ANIM = "/home/z/my-project/apk_analysis/work/anim_yml"
CTRL = "/home/z/my-project/apk_analysis/work/ctrl_yml"
OUT_ZIP = "/home/z/my-project/download/LastWar_1.0.328_anim_unity_pack.zip"

README = """# Last War 1.0.328 - Unity-ready Animation Pack

Directly importable into Unity (target pipeline version 2019.4.41f1; works in any newer editor).

## Contents
- `UnityClips/` - 2,409 AnimationClips as `.anim` YAML + `.meta` (deterministic GUIDs)
- `Animators/` - 447 AnimatorControllers as `.controller` YAML + `.meta`,
  state machines fully wired to the clips by GUID (0 unresolved references)

## Usage
1. Copy both folders (or any subset) anywhere under `Assets/`.
2. Unity imports the `.anim` / `.controller` assets immediately - the GUIDs in the
   `.meta` files already wire every controller state to its clip.
3. Drop a controller on any model via the `Animator` component.

## How it was made
- Clips were decoded from build-serialized muscle data (streamed/dense/constant
  curve blocks, exactly like Unity's own loader) and emitted as 2019.4 YAML.
- Controllers were reconstructed from the build-format controller constants:
  state names come from the engine's own `m_TOS` debug-name table; transitions,
  AnyState transitions, conditions (parameter names recovered by CRC32 from the
  `m_TOS` corpus) and the blend tree are preserved. Entry/Exit selector plumbing
  is implicit in Unity and therefore omitted.
- Parameters used with If/IfNot conditions are emitted as bool; rename them in the
  Animator window if your code expects triggers or floats.

## Known limitations
- ~6% of transform paths could not be resolved to bone names at extraction time and
  appear as `UnknownPath_xxxxxxxx` bindings (their rig hierarchy lived in bundles
  not present in 1.0.328).
- PPtr curves (sprite swaps) are not emitted; see `Animations/_clips_index.csv` in
  the main repo.
- Humanoid clips bind the original rig paths (`Bip001/...`) verbatim; retargeting to
  your own rigs requires matching bone names or a Generic rig.
- 3D unit motion in this game is partly baked to textures (GPUSkinning) at runtime;
  those are NOT AnimationClips and are not included here (see Animations/README.md).
- 72 duplicate clip instances (same name inside one bundle) share a single file.
"""

def main():
    os.makedirs(os.path.dirname(OUT_ZIP), exist_ok=True)
    if os.path.exists(OUT_ZIP):
        os.remove(OUT_ZIP)
    n = m = 0
    zf = zipfile.ZipFile(OUT_ZIP, "w", zipfile.ZIP_DEFLATED, compresslevel=6)
    zf.writestr("UnityAnimPack/README.md", README)
    for root, _dirs, files in os.walk(ANIM):
        rel = os.path.relpath(root, ANIM)
        if rel == ".":
            continue
        b = os.path.basename(rel)
        for f in sorted(files):
            if not f.endswith(".anim"):
                continue
            stem = f[: -len(".anim")]
            if stem.endswith(".AnimationClip"):
                stem = stem[: -len(".AnimationClip")]
            base = "UnityAnimPack/UnityClips/" + b + "/" + stem
            zf.write(os.path.join(root, f), base + ".anim")
            zf.write(os.path.join(root, f) + ".meta", base + ".anim.meta")
            n += 1
    for root, _dirs, files in os.walk(CTRL):
        rel = os.path.relpath(root, CTRL)
        if rel == ".":
            continue
        b = os.path.basename(rel)
        for f in sorted(files):
            if f.endswith(".controller") or f.endswith(".controller.meta"):
                zf.write(os.path.join(root, f), "UnityAnimPack/Animators/" + b + "/" + f)
                if f.endswith(".controller"):
                    m += 1
    zf.close()
    print(f"pack: {n} anims, {m} controllers -> {OUT_ZIP}")
    print("zip size: %.1f MB" % (os.path.getsize(OUT_ZIP) / 1e6))

if __name__ == "__main__":
    main()
