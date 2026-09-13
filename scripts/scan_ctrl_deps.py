#!/usr/bin/env python3
"""One-pass scan over all bundles IN THE FRAGMENT (no files written to disk):
- own CAB name per bundle
- (CAB, pathID) -> AnimationClip name          (clipmap.json)
- CAB -> bundle name                            (cab2bundle.json)
- externals CAB list for *_animator_* bundles   (animator_externals.json)
- census: clip count, override-controller count per bundle
Resumable via scripts/scan_deps_state.txt. Pool(6).
"""
import io
import json
import os
import struct
import sys
import warnings
import logging
from multiprocessing import Pool

warnings.filterwarnings("ignore")
logging.getLogger("UnityPy").setLevel(logging.ERROR)

import UnityPy
UnityPy.config.FALLBACK_UNITY_VERSION = "2019.4.41f1"

AB = "/home/z/my-project/apk_analysis/apk_parts/assets/AssetBundles"
STATE = "/home/z/my-project/scripts/scan_deps_state.txt"
OUT = "/home/z/my-project/apk_analysis/work"
PASS2 = len(sys.argv) > 1 and sys.argv[1] == "pass2"
if PASS2:
    STATE = "/home/z/my-project/scripts/scan_deps2_state.txt"

def parse_table():
    d = open(os.path.join(AB, "BundleOffsetTable.bytes"), "rb").read()
    p = 0
    p += 4                              # fragment count
    nl = d[p]; p += 1 + nl              # fragment name
    p += 4                              # version
    cnt = struct.unpack_from("<I", d, p)[0]; p += 4
    entries = []
    for _ in range(cnt):
        ln = 0; sh = 0
        while True:
            b = d[p]; p += 1
            ln |= (b & 0x7F) << sh; sh += 7
            if not (b & 0x80):
                break
        nm = d[p:p + ln].decode("utf-8", "replace"); p += ln
        off = struct.unpack_from("<Q", d, p)[0]; p += 8
        entries.append((nm, off))
    frag_size = os.path.getsize(os.path.join(AB, "BundleFragment0.bytes"))
    recs = []
    for i, (nm, off) in enumerate(entries):
        nxt = entries[i + 1][1] if i + 1 < len(entries) else frag_size
        sz = nxt - off
        if 0 < off < frag_size and 0 < sz <= frag_size - off:
            recs.append((nm[:-7] if nm.endswith(".bundle") else nm, off, sz))
    return recs

FRAG = None

def init():
    global FRAG
    FRAG = open(os.path.join(AB, "BundleFragment0.bytes"), "rb")

def one(job):
    name, off, sz = job
    try:
        FRAG.seek(off)
        data = FRAG.read(sz)
        env = UnityPy.load(io.BytesIO(data))
    except Exception as e:
        return name, {"err": str(e)[:80]}, {}
    cabs = []
    sfx = {}       # cab -> [external cabs]
    clipmap = {}
    ctrlmap = {}   # "cab:pathID" -> name (pass2)
    nclips = 0
    nover = 0
    nctrl = 0
    has_ctrl = False
    try:
        for bf in env.files.values():
            sfs = getattr(bf, "files", None) or {}
            for sf in sfs.values():
                cab = getattr(sf, "name", None) or ""
                cabs.append(cab)
                try:
                    sfx[cab] = [e.path.rsplit("/", 1)[-1] for e in (sf.externals or [])]
                except Exception:
                    sfx[cab] = []
                for obj in env.objects:
                    try:
                        t = obj.type.name
                    except Exception:
                        continue
                    if t == "AnimationClip":
                        if not PASS2:
                            try:
                                tt = obj.read_typetree()
                                clipmap["%s:%d" % (cab, obj.path_id)] = str(tt.get("m_Name", ""))
                                nclips += 1
                            except Exception:
                                pass
                        else:
                            nclips += 1
                    elif t == "AnimatorController":
                        nctrl += 1
                        has_ctrl = True
                        if PASS2:
                            try:
                                own = getattr(obj.assets_file, "name", "") or cab
                                tt = obj.read_typetree()
                                ctrlmap["%s:%d" % (own, obj.path_id)] = str(tt.get("m_Name", ""))
                            except Exception:
                                pass
                    elif t == "AnimatorOverrideController":
                        nover += 1
    except Exception as e:
        return name, {"err": "inner:" + str(e)[:70]}, {}
    info = {"cabs": cabs, "sfx": sfx if has_ctrl else None, "ctrlmap": ctrlmap if PASS2 else None,
            "nclips": nclips, "nctrl": nctrl, "nover": nover}
    return name, info, clipmap

def done_set():
    if os.path.exists(STATE):
        return set(open(STATE).read().split())
    return set()

def main():
    recs = parse_table()
    print(f"{len(recs)} bundles in fragment", flush=True)
    done = done_set()
    jobs = [r for r in recs if r[0] not in done]
    print(f"todo: {len(jobs)}", flush=True)
    st = open(STATE, "a")
    cab2bundle = {}
    animator_ext = {}
    clipmap = {}
    ctrlmap = {}
    stats = {"clips": 0, "ctrl": 0, "over": 0, "err": 0, "animator_bundles": 0, "cabs": 0}
    n_done = 0
    with Pool(6, initializer=init) as pool:
        for name, info, cm in pool.imap_unordered(one, jobs, chunksize=4):
            n_done += 1
            if "err" in info:
                stats["err"] += 1
                st.write(name + "\n"); st.flush()
                continue
            for cab in info.get("cabs") or []:
                if cab:
                    if cab not in cab2bundle:
                        stats["cabs"] += 1
                    cab2bundle[cab] = name
            if info.get("sfx") is not None:
                animator_ext[name] = info["sfx"]
                stats["animator_bundles"] += 1
            stats["clips"] += info.get("nclips", 0)
            stats["ctrl"] += info.get("nctrl", 0)
            stats["over"] += info.get("nover", 0)
            clipmap.update(cm)
            ctrlmap.update(info.get("ctrlmap") or {})
            st.write(name + "\n"); st.flush()
            if n_done % 500 == 0:
                print(f"  {n_done}/{len(jobs)} clips={stats['clips']} ctrl={stats['ctrl']} cabs={stats['cabs']}", flush=True)
    st.close()
    json.dump(cab2bundle, open(os.path.join(OUT, "cab2bundle.json"), "w"))
    json.dump(animator_ext, open(os.path.join(OUT, "animator_externals.json"), "w"))
    if not PASS2:
        json.dump(clipmap, open(os.path.join(OUT, "clipmap.json"), "w"))
    else:
        json.dump(ctrlmap, open(os.path.join(OUT, "ctrlmap.json"), "w"))
    print("STATS:", json.dumps(stats), flush=True)
    print(f"clipmap entries: {len(clipmap)} ctrlmap entries: {len(ctrlmap)}", flush=True)
    print("SCAN DONE", flush=True)

if __name__ == "__main__":
    main()
