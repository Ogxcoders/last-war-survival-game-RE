#!/usr/bin/env python3
"""Decrypt Last War .mdl assemblies - CORRECT algorithm.

Scheme (reverse-engineered):
  key = data[0] ^ 0x4D ('M'); verified by data[1] ^ key == 0x5A ('Z')
  encrypted region = data[0:key]  (the length of the region IS the key value)
  the rest of the file is plaintext
  e_lfanew (0x3c) may be byte-swapped as anti-RE -> force to 0x80 (PE sig always at 0x80)
"""
import os, sys, struct

SRC = '/home/z/my-project/apk_analysis/re/apk_parts/assets/Assemblies'
DST = '/home/z/my-project/apk_analysis/re/dlls'
os.makedirs(DST, exist_ok=True)

ok, fail = 0, []
for f in sorted(os.listdir(SRC)):
    src = os.path.join(SRC, f)
    if not os.path.isfile(src):
        continue
    data = bytearray(open(src, 'rb').read())
    if not f.endswith('.mdl'):
        open(os.path.join(DST, f), 'wb').write(data)
        continue
    key = data[0] ^ 0x4D
    if data[1] ^ key != 0x5A:
        fail.append((f, 'key check failed'))
        continue
    for i in range(key):
        data[i] ^= key
    # force e_lfanew = 0x80 (PE signature location, verified across files)
    struct.pack_into('<I', data, 0x3c, 0x80)
    if bytes(data[:2]) != b'MZ':
        fail.append((f, 'bad MZ'))
        continue
    open(os.path.join(DST, f[:-4] + '.dll'), 'wb').write(bytes(data))
    ok += 1

print(f'decrypted OK: {ok}, failed: {len(fail)}')
for f, why in fail:
    print(' FAIL', f, why)
