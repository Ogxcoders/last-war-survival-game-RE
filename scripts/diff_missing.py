#!/usr/bin/env python3
"""Diff scanned clip/controller universe vs repo JSON dumps.
- Groups clips by (bundle, sanitized_name); assigns unique stems
  (dup groups: first keeps name, others get name@pathID).
- Lists (bundle, cab, pathID, stem) jobs for MISSING clip JSONs.
- Lists (bundle, cab, pathID, stem) jobs for MISSING controller JSONs.
Outputs: work/missing_jobs.json, work/stem_registry.json (only referenced later)
"""
import json
import os

W = "/home/z/my-project/apk_analysis/work"
REPO = "/home/z/my-project/apk_analysis/github_repo/Animations"

def sanitize(n):
    return str(n).replace("/", "_").replace("\x00", "")

clipmap = json.load(open(f"{W}/clipmap.json"))          # "cab:pathID" -> name
cab2bundle = json.load(open(f"{W}/cab2bundle.json"))    # cab -> bundle

# group clips by (bundle, sanitized name)
groups = {}   # (bundle, stem) -> [(cab, pathID)]
for k, nm in clipmap.items():
    cab, pid = k.rsplit(":", 1)
    b = cab2bundle.get(cab)
    if not b:
        continue
    groups.setdefault((b, sanitize(nm) or "unnamed"), []).append((cab, int(pid)))

# existing repo clip files: (bundle, stem)
existing = set()
for d in os.listdir(f"{REPO}/UnityClips"):
    for f in os.listdir(f"{REPO}/UnityClips/{d}"):
        if f.endswith(".AnimationClip.json"):
            existing.add((d, f[: -len(".AnimationClip.json")]))

missing_clips = []
registry = {}   # "cab:pathID" -> {bundle, stem}
for (b, stem), lst in groups.items():
    lst.sort()
    for i, (cab, pid) in enumerate(lst):
        s = stem if i == 0 else f"{stem}@{pid}"
        registry[f"{cab}:{pid}"] = {"bundle": b, "stem": s}
        if (b, s) not in existing:
            missing_clips.append([b, cab, pid, s])

# controllers: enumerate from scan info is nameless; count groups via files vs scan later.
# We simply mark all *_animator_* bundles for a fresh controller pass in extractor.
anim_bundles = sorted({b for b, _ in groups if "_animator_" in b})
for d in os.listdir(f"{REPO}/Animators"):
    if d not in anim_bundles:
        anim_bundles.append(d)

json.dump(missing_clips, open(f"{W}/missing_clips.json", "w"))
json.dump({"animator_bundles": anim_bundles}, open(f"{W}/anim_bundles.json", "w"))
json.dump(registry, open(f"{W}/stem_registry.json", "w"))

dup_groups = sum(1 for v in groups.values() if len(v) > 1)
print(f"clip groups: {len(groups)}  dup groups: {dup_groups}")
print(f"existing clip jsons: {len(existing)}")
print(f"MISSING clip jsons to extract: {len(missing_clips)}")
print(f"animator bundles to re-extract controllers from: {len(anim_bundles)}")
