#!/usr/bin/env python3
"""Stream-extract AssetBundles members straight from the remote APK zip
via HTTP range requests + manual central-directory parsing (no full download)."""
import os
import sys
import time
import urllib.request
import zlib

URL = open("/home/z/my-project/scripts/dl_url.txt").read().strip()
UA = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36"
REF = "https://liteapks.com/last-warsurvival-game.html"
DEST = "/home/z/my-project/apk_analysis/apk_parts/assets/AssetBundles"
WANT = ("assets/AssetBundles/BundleFragment0.bytes",
        "assets/AssetBundles/BundleOffsetTable.bytes",
        "assets/AssetBundles/AliasOffsetTable.bytes")

def fetch(start, end, tries=8):
    for a in range(tries):
        try:
            req = urllib.request.Request(URL, headers={
                "User-Agent": UA, "Referer": REF,
                "Range": f"bytes={start}-{end}"})
            with urllib.request.urlopen(req, timeout=60) as r:
                return r.read()
        except Exception as e:
            if a == tries - 1:
                raise
            time.sleep(min(20, 2 * a))

# 1) EOCD (last 64KB)
tail = fetch(os.path.basename if False else None, None) if False else None
size_probe = fetch(0, 0)
total = int(size_probe and 0 or 0)
# get total size from probe
req = urllib.request.Request(URL, headers={"User-Agent": UA, "Referer": REF, "Range": "bytes=0-0"})
with urllib.request.urlopen(req, timeout=60) as r:
    cr = r.headers.get("Content-Range", "")
total = int(cr.split("/")[-1])
print("APK size:", total)

tail = fetch(total - 65536, total - 1)
eocd = tail.rfind(b"PK\x05\x06")
assert eocd >= 0, "no EOCD"
import struct
_, _, _, _, nent, cdsize, cdoff, _ = struct.unpack_from("<IHHHHIIH", tail, eocd + 4 - 4)
# fields: signature(4) disk(2) cddisk(2) disk_nent(2) nent(2) cdsize(4) cdoff(4) commentlen(2)
nent, cdsize, cdoff = struct.unpack_from("<HII", tail, eocd + 10)
print("entries:", nent, "cd:", cdsize, "@", cdoff)
cd = fetch(cdoff, cdoff + cdsize - 1)

# 2) parse central directory
p = 0
found = {}
while p < len(cd) and cd[p:p + 4] == b"PK\x01\x02":
    (sig, vmade, vneed, flags, method, mtime, mdate, crc, csize, usize,
     nlen, elen, clen, dnum, iattr, eattr, loff) = struct.unpack_from("<IHHHHHHIIIHHHHHII", cd, p)
    name = cd[p + 46:p + 46 + nlen].decode()
    if name in WANT:
        found[name] = (method, crc, csize, usize, loff, flags & 0x8 != 0)
    p += 46 + nlen + elen + clen
print("wanted found:", list(found))

# 3) local header size then stream member
for name in WANT:
    method, crc, csize, usize, loff, streamed = found[name]
    lh = fetch(loff, loff + 29)
    assert lh[:4] == b"PK\x03\x04"
    nlen2, elen2 = struct.unpack_from("<HH", lh, 26)
    data_start = loff + 30 + nlen2 + elen2
    out = os.path.join(DEST, os.path.basename(name))
    os.makedirs(DEST, exist_ok=True)
    crc32 = 0
    t0 = time.time()
    if method == 0:  # stored
        want = csize
        with open(out, "wb") as f:
            got = 0
            while got < csize:
                chunk = fetch(data_start + got, data_start + min(csize, got + (1 << 23)) - 1)
                f.write(chunk); got += len(chunk); crc32 = zlib.crc32(chunk, crc32)
                print(f"  {os.path.basename(out)} {got*100//max(csize,1)}% {got//1048576}MB", flush=True)
    else:  # deflate
        d = zlib.decompressobj(-15)
        with open(out, "wb") as f:
            got = 0
            while got < csize:
                n = min(1 << 23, csize - got)
                chunk = fetch(data_start + got, data_start + n - 1)
                outc = d.decompress(chunk)
                if outc:
                    f.write(outc); crc32 = zlib.crc32(outc, crc32)
                got += n
            outc = d.flush()
            if outc:
                f.write(outc); crc32 = zlib.crc32(outc, crc32)
                print(f"  flushed {len(outc)}", flush=True)
    print(f"{os.path.basename(out)}: {os.path.getsize(out)} bytes, crc {'OK' if crc32 == crc else 'FAIL %08x vs %08x' % (crc32, crc)}, {time.time()-t0:.0f}s", flush=True)
print("REMOTE EXTRACT DONE")
