#!/usr/bin/env python3
"""Attach deterministic .meta files to every generated .anim / .controller."""
import os
import sys

sys.path.insert(0, "/home/z/my-project/scripts")
from unity_guids import add_meta_for_anim, add_meta_for_ctrl

ANIM = "/home/z/my-project/apk_analysis/work/anim_yml"
CTRL = "/home/z/my-project/apk_analysis/work/ctrl_yml"

def anim_stem(fname):
    """'Farhad_01_show.AnimationClip.anim' -> 'Farhad_01_show' ; 'X.anim' -> 'X'."""
    s = fname[: -len(".anim")]
    if s.endswith(".AnimationClip"):
        s = s[: -len(".AnimationClip")]
    return s

def main():
    n = m = 0
    for root, _dirs, files in os.walk(ANIM):
        bundle = os.path.relpath(root, ANIM)
        if bundle == ".":
            continue
        bname = os.path.basename(bundle)
        for f in files:
            if f.endswith(".anim"):
                stem = anim_stem(f)
                from unity_guids import guid_anim, write_meta
                write_meta(os.path.join(root, f) + ".meta", guid_anim(bname, stem), 7400000)
                n += 1
    for root, _dirs, files in os.walk(CTRL):
        bundle = os.path.relpath(root, CTRL)
        if bundle == ".":
            continue
        bname = os.path.basename(bundle)
        for f in files:
            if f.endswith(".controller"):
                add_meta_for_ctrl(os.path.join(root, f), bname)
                m += 1
    print(f"metas: {n} anims, {m} controllers")

if __name__ == "__main__":
    main()
