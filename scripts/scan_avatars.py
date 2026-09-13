#!/usr/bin/env python3
"""Scan every bundle (read in-place from BundleFragment0.bytes via offsets)
for Avatar assets; save m_TOS (path-hash -> path) + humanoid info.
Pool(6) workers each open the fragment independently. Zero extra disk."""
import io
import json
import os
import struct
import sys
from multiprocessing import Pool

AB = "/home/z/my-project/apk_analysis/apk_parts/assets/AssetBundles"
OUT = "/home/z/my-project/apk_analysis/work/avatars.json"
STATE = "/home/z/my-project/scripts/avatar_scan_state.txt"

import UnityPy
UnityPy.config.FALLBACK_UNITY_VERSION = "2019.4.41f1"

def load_table():
    d = open(os.path.join(AB, "BundleOffsetTable.bytes"), "rb").read()
    p = 0
    frag_count = struct.unpack_from("<I", d, p)[0]; p += 4
    nl = d[p]; p += 1
    p += nl
    p += 4
    cnt = struct.unpack_from("<I", d, p)[0]; p += 4
    entries = []
    for i in range(cnt):
        nlen = 0; shift = 0
        while True:
            b = d[p]; p += 1
            nlen |= (b & 0x7F) << shift
            shift += 7
            if not (b & 0x80):
                break
        nm = d[p:p + nlen].decode("utf-8", "replace"); p += nlen
        off = struct.unpack_from("<Q", d, p)[0]; p += 8
        entries.append((nm, off))
    frag_size = os.path.getsize(os.path.join(AB, "BundleFragment0.bytes"))
    recs = []
    for i, (nm, off) in enumerate(entries):
        nxt = entries[i + 1][1] if i + 1 < len(entries) else frag_size
        size = nxt - off
        if 0 < off < frag_size and 0 < size <= frag_size - off:
            recs.append((nm[:-7] if nm.endswith(".bundle") else nm, off, size))
    return recs

def one(rec):
    name, off, size = rec
    try:
        with open(os.path.join(AB, "BundleFragment0.bytes"), "rb") as f:
            f.seek(off)
            data = f.read(size)
        env = UnityPy.load(io.BytesIO(data))
        avatars = []
        for o in env.objects:
            if o.type.name != "Avatar":
                continue
            tt = o.read_typetree()
            tos = tt.get("m_TOS") or {}
            if isinstance(tos, list):  # list of [hash, path] pairs
                tos = {str(k): v for k, v in tos}
            avatars.append({
                "name": tt.get("m_Name"),
                "tos": {str(k): v for k, v in tos.items()},
                "human": bool((tt.get("m_HumanDescription") or {}).get("m_Human")),
                "humanBones": [h.get("m_BoneName") for h in (tt.get("m_HumanDescription") or {}).get("m_Human", [])],
            })
        return name, avatars, None
    except Exception as e:
        return name, [], str(e)[:100]

def main():
    recs = load_table()
    print(f"{len(recs)} bundles in table", flush=True)
    done = set()
    if os.path.exists(STATE):
        done = set(open(STATE).read().split())
    todo = [r for r in recs if r[0] not in done]
    print(f"{len(done)} done, {len(todo)} to go", flush=True)
    out = {}
    if os.path.exists(OUT):
        out = json.load(open(OUT))
    nav = 0
    with Pool(6) as pool:
        for name, avatars, err in pool.imap_unordered(one, todo, chunksize=8):
            if err:
                print(f"ERR {name}: {err}", flush=True)
            if avatars:
                out[name] = avatars
                nav += len(avatars)
                print(f"AVATAR {name}: {[a['name'] for a in avatars]}", flush=True)
            with open(STATE, "a") as sf:
                sf.write(name + "\n")
    json.dump(out, open(OUT, "w"))
    print(f"SCAN DONE: {nav} avatars in {len(out)} bundles", flush=True)

if __name__ == "__main__":
    main()
