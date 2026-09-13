#!/usr/bin/env python3
"""Parse BundleOffsetTable.bytes (u8len+name+u64 offset per entry) and slice
BundleFragment0.bytes into individual .bundle files.

Usage: python3 split_bundles.py <AssetBundles_dir> <out_dir>
"""
import os, sys, struct, json

def main():
    ab_dir, out_dir = sys.argv[1], sys.argv[2]
    frag_path = os.path.join(ab_dir, "BundleFragment0.bytes")
    tbl_path = os.path.join(ab_dir, "BundleOffsetTable.bytes")
    frag_size = os.path.getsize(frag_path)
    d = open(tbl_path, "rb").read()

    p = 0
    frag_count = struct.unpack_from("<I", d, p)[0]; p += 4
    print(f"fragments: {frag_count}")
    # only BundleFragment0 expected
    nl = d[p]; p += 1
    frag_name = d[p:p+nl].decode(); p += nl
    v1 = struct.unpack_from("<I", d, p)[0]; p += 4
    cnt = struct.unpack_from("<I", d, p)[0]; p += 4
    print(f"fragment '{frag_name}' v1={v1} bundles={cnt} entries@{p}")

    entries = []
    for i in range(cnt):
        # ULEB128 length (names >=128 use 2+ bytes)
        nl = 0; shift = 0
        while True:
            b = d[p]; p += 1
            nl |= (b & 0x7F) << shift
            shift += 7
            if not (b & 0x80):
                break
        nm = d[p:p+nl].decode("utf-8", "replace"); p += nl
        off = struct.unpack_from("<Q", d, p)[0]; p += 8
        entries.append((nm, off))
    print(f"parsed {len(entries)} entries, consumed {p}/{len(d)} bytes")

    # sizes from deltas
    recs = []
    for i, (nm, off) in enumerate(entries):
        nxt = entries[i+1][1] if i+1 < len(entries) else frag_size
        size = nxt - off
        if 0 < off < frag_size and 0 < size <= frag_size - off:
            recs.append((nm, off, size))
    print(f"usable: {len(recs)}")
    # spot check UnityFS signature
    f = open(frag_path, "rb")
    ok = 0
    for nm, off, sz in recs[:20] + recs[-20:]:
        f.seek(off)
        if f.read(7) == b"UnityFS":
            ok += 1
    print(f"signature check: {ok}/40 are UnityFS")

    os.makedirs(out_dir, exist_ok=True)
    total = 0
    manifest = []
    for nm, off, sz in recs:
        f.seek(off)
        data = f.read(sz)
        if len(data) != sz:
            continue
        name = nm[:-7] if nm.endswith(".bundle") else nm
        with open(os.path.join(out_dir, name + ".bundle"), "wb") as w:
            w.write(data)
        manifest.append([name, off, sz])
        total += 1
        if total % 1000 == 0:
            print(f"  {total}...", flush=True)
    with open(os.path.join(out_dir, "_manifest.json"), "w") as w:
        json.dump(manifest, w)
    print(f"SPLIT DONE: {total} bundles -> {out_dir}", flush=True)

if __name__ == "__main__":
    main()
