#!/usr/bin/env python3
"""Validate generated .controller / .anim YAML + GUID wiring."""
import glob
import os
import re
import sys
import yaml

CTRL = "/home/z/my-project/apk_analysis/work/ctrl_yml"
ANIM = "/home/z/my-project/apk_analysis/work/anim_yml"

class LooseLoader(yaml.SafeLoader):
    pass

def unknown(loader, suffix, node):
    if isinstance(node, yaml.ScalarNode):
        return loader.construct_scalar(node)
    if isinstance(node, yaml.SequenceNode):
        return loader.construct_sequence(node)
    return loader.construct_mapping(node)

LooseLoader.add_multi_constructor("!", unknown)
LooseLoader.add_multi_constructor("tag:unity3d.com,2011:", unknown)

guid_re = re.compile(r"guid: ([0-9a-f]{32})")

def main():
    # collect anim guids from metas
    anim_guids = {}
    for p in glob.glob(ANIM + "/*/*.anim.meta"):
        b = os.path.basename(os.path.dirname(p))
        stem = os.path.basename(p)[:-len(".anim.meta")]
        txt = open(p).read()
        m = guid_re.search(txt)
        if m:
            anim_guids[m.group(1)] = f"{b}/{stem}.anim"
    print("anim metas with guid:", len(anim_guids))

    nctrl = 0
    ndocs = 0
    errs = []
    total_refs = 0
    missing = set()
    states_total = 0
    wired = 0
    for p in sorted(glob.glob(CTRL + "/*/*.controller")):
        nctrl += 1
        try:
            docs = list(yaml.load_all(open(p), Loader=LooseLoader))
        except Exception as e:
            errs.append((p, "parse: " + str(e)[:90]))
            continue
        ndocs += len(docs)
        for d in docs:
            if not isinstance(d, dict):
                errs.append((p, "doc not dict"))
                continue
            t = list(d.keys())[0]
            body = d[t]
            if t == "AnimatorState":
                states_total += 1
                mot = body.get("m_Motion") or {}
                if isinstance(mot, dict) and mot.get("guid"):
                    total_refs += 1
                    if mot["guid"] in anim_guids:
                        wired += 1
                    else:
                        missing.add(mot["guid"])
                elif mot == {"fileID": 0}:
                    pass
                else:
                    errs.append((p, "motion empty? " + repr(mot)[:40]))
            txt2 = None
        # guid references anywhere in file
        raw = open(p).read()
        for g in guid_re.findall(raw):
            total_refs += 1
            if g not in anim_guids:
                missing.add(g)
    print(f"controllers parsed: {nctrl}, yaml docs: {ndocs}")
    print(f"states: {states_total}, states with guid motion: {total_refs}, wired ok: {wired}")
    print(f"missing guids: {len(missing)}")
    for e in errs[:10]:
        print("ERR:", e)

    # spot check Farhad
    far = CTRL + "/gameres_art_dir_cars_a_hero_farhad_01_animator_a8ee676bef9f7d9a6f668f62c46cac3a/Farhad01_preview.controller"
    docs = list(yaml.load_all(open(far), Loader=LooseLoader))
    print("\n--- Farhad01_preview:", len(docs), "docs")
    for d in docs:
        t = list(d.keys())[0]
        if t == "AnimatorController":
            for l in d[t]["m_AnimatorLayers"]:
                print("  layer:", l["m_Name"], "sm:", l["m_StateMachine"])
        elif t == "AnimatorStateMachine":
            print("  SM:", d[t]["m_Name"], "children:", len(d[t]["m_ChildStates"]), "default:", d[t]["m_DefaultState"])
        elif t == "AnimatorState":
            print("  state:", d[t]["m_Name"], "loop:", d[t]["m_LoopTime"], "motion:", str(d[t]["m_Motion"])[:60], "->", anim_guids.get(d[t]["m_Motion"].get("guid", ""), "?")[:40] if isinstance(d[t]["m_Motion"], dict) else "")
    # sanity: anim yaml parses
    sample = sorted(glob.glob(ANIM + "/*/*.anim"))[0]
    yaml.load(open(sample), Loader=LooseLoader)
    print("\nsample .anim parses OK:", sample)

if __name__ == "__main__":
    main()
