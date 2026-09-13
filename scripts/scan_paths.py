#!/usr/bin/env python3
"""Walk every GameObject hierarchy in every bundle (in-place from fragment).
Build global map: crc32(path) -> path. Also collect Mesh blendshape names.
Output: paths.json {hash_str: path}, shapes.json [names]"""
import io
import json
import os
import struct
import zlib
from multiprocessing import Pool

AB = "/home/z/my-project/apk_analysis/apk_parts/assets/AssetBundles"
OUT_P = "/home/z/my-project/apk_analysis/work/paths.json"
OUT_S = "/home/z/my-project/apk_analysis/work/shapes.json"
STATE = "/home/z/my-project/scripts/path_scan_state.txt"

import UnityPy
UnityPy.config.FALLBACK_UNITY_VERSION = "2019.4.41f1"

def load_table():
    d = open(os.path.join(AB, "BundleOffsetTable.bytes"), "rb").read()
    p = 0
    p += 4
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
    paths = set()
    shapes = set()
    try:
        with open(os.path.join(AB, "BundleFragment0.bytes"), "rb") as f:
            f.seek(off)
            data = f.read(size)
        env = UnityPy.load(io.BytesIO(data))
        # index objects by (file, path_id)
        gos, ts, meshes = {}, {}, {}
        for o in env.objects:
            t = o.type.name
            if t == "GameObject":
                gos[(o.assets_file, o.path_id)] = o
            elif t == "Transform":
                ts[(o.assets_file, o.path_id)] = o
            elif t == "Mesh":
                meshes[(o.assets_file, o.path_id)] = o
        # read transforms
        tdata = {}
        for key, o in ts.items():
            try:
                tt = o.read_typetree()
            except Exception:
                continue
            father = (tt.get("m_Father") or {})
            fkey = (o.assets_file, father.get("m_PathID")) if father.get("m_PathID") else None
            go_pptr = (tt.get("m_GameObject") or {})
            gkey = (o.assets_file, go_pptr.get("m_PathID"))
            try:
                goname = gos[gkey].read_typetree().get("m_Name", "?")
            except Exception:
                goname = "?"
            children = [(o.assets_file, c.get("m_PathID"))
                        for c in (tt.get("m_Children") or []) if c.get("m_PathID")]
            tdata[key] = (fkey, goname, children)
        # roots: father missing from tdata
        def dfs(key, prefix):
            if key not in tdata:
                return
            fkey, goname, children = tdata[key]
            path = f"{prefix}/{goname}" if prefix else goname
            paths.add(path)
            for ckey in children:
                dfs(ckey, path)
        for key in tdata:
            fkey, _, _ = tdata[key]
            if fkey not in tdata:
                dfs(key, "")
        # blendshape names
        for o in meshes.values():
            try:
                tt = o.read_typetree()
                sh = tt.get("m_Shapes") or {}
                for s in sh.get("m_Shapes", []):
                    n = s.get("name") or s.get("shapeName")
                    if n:
                        shapes.add(str(n))
            except Exception:
                pass
    except Exception as e:
        return name, set(), set(), str(e)[:80]
    return name, paths, shapes, None

def main():
    recs = load_table()
    print(f"{len(recs)} bundles", flush=True)
    done = set()
    if os.path.exists(STATE):
        done = set(open(STATE).read().split())
    todo = [r for r in recs if r[0] not in done]
    print(f"{len(done)} done, {len(todo)} to go", flush=True)
    all_paths, all_shapes = set(), set()
    nerr = 0
    with Pool(6) as pool:
        for name, paths, shapes, err in pool.imap_unordered(one, todo, chunksize=8):
            if err:
                nerr += 1
            all_paths |= paths
            all_shapes |= shapes
            with open(STATE, "a") as sf:
                sf.write(name + "\n")
    json.dump({str(zlib.crc32(p.encode())): p for p in all_paths}, open(OUT_P, "w"))
    json.dump(sorted(all_shapes), open(OUT_S, "w"))
    print(f"PATH SCAN DONE: {len(all_paths)} paths, {len(all_shapes)} blendshapes, {nerr} errs", flush=True)

if __name__ == "__main__":
    main()
