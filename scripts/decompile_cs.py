#!/usr/bin/env python3
"""Decompile all decrypted .NET assemblies to C# source (per-type project layout).

Strategy:
- Whole-assembly `-p --nested-directories` decompile per DLL with subprocess timeout.
- If a whole-assembly run times out (big DLL), fall back to batched `-t` runs
  (N types per invocation) with a resume state file.
"""
import json
import os
import subprocess
import sys
import time

DOTNET = "/home/z/.dotnet/dotnet"
ILSPY = "/home/z/my-project/scripts/dotnet_tmp/ilspy/ilspycmd.dll"
DLL_DIR = "/home/z/my-project/apk_analysis/re/dlls"
OUT_DIR = "/home/z/my-project/apk_analysis/github_repo/csharp_src"
STATE = "/home/z/my-project/scripts/decomp_state.json"
ENV = dict(os.environ, DOTNET_ROOT="/home/z/.dotnet", DOTNET_gcServer="0")
REFS = ["-r", DLL_DIR]
WHOLE_TIMEOUT = 560          # seconds per whole-assembly attempt
BATCH = 200                  # types per invocation in fallback mode
BATCH_TIMEOUT = 560

def run(args, timeout):
    p = subprocess.run(
        [DOTNET, ILSPY, "--disable-updatecheck"] + args,
        env=ENV, stdout=subprocess.PIPE, stderr=subprocess.STDOUT, timeout=timeout,
    )
    return p.returncode, p.stdout.decode("utf-8", "replace")[-4000:]

def list_types(dll):
    """All type full names (classes, interfaces, structs, delegates, enums)."""
    names = []
    for et in ("c", "i", "s", "d", "e"):
        rc, out = run(["-l", et, dll], 120)
        if rc == 0:
            names.extend(l for l in out.splitlines() if l.strip())
    # dedupe preserving order
    seen, uniq = set(), []
    for n in names:
        if n not in seen:
            seen.add(n)
            uniq.append(n)
    return uniq

def whole(dll, outdir):
    os.makedirs(outdir, exist_ok=True)
    return run(["-p", "--nested-directories"] + REFS + ["-o", outdir, dll], WHOLE_TIMEOUT)

def batched(dll, outdir, name):
    os.makedirs(outdir, exist_ok=True)
    st = json.load(open(STATE))
    bl = st.setdefault("batches", {}).setdefault(name, 0)
    types = list_types(dll)
    total = len(types)
    print(f"  {name}: {total} types, resume from batch {bl}", flush=True)
    if not total:
        return "no-types"
    while bl * BATCH < total:
        chunk = types[bl * BATCH:(bl + 1) * BATCH]
        args = ["-p", "--nested-directories"] + REFS + ["-o", outdir, dll]
        for t in chunk:
            args += ["-t", t]
        try:
            rc, out = run(args, BATCH_TIMEOUT)
        except subprocess.TimeoutExpired:
            return f"batch-timeout@{bl}"
        if rc != 0:
            return f"batch-error@{bl}: {out[-300:]}"
        bl += 1
        st["batches"][name] = bl
        json.dump(st, open(STATE, "w"))
        print(f"  batch {bl}/{(total + BATCH - 1)//BATCH} done", flush=True)
    return "ok"

def main():
    st = json.load(open(STATE)) if os.path.exists(STATE) else {"done": [], "batches": {}}
    json.dump(st, open(STATE, "w"))
    dlls = sorted(f for f in os.listdir(DLL_DIR) if f.endswith(".dll"))
    # small first, Assembly-CSharp (biggest) last
    dlls.sort(key=lambda f: os.path.getsize(os.path.join(DLL_DIR, f)))
    for f in dlls:
        name = f[:-4]
        if name in st["done"]:
            continue
        outdir = os.path.join(OUT_DIR, name)
        t0 = time.time()
        print(f"[{name}] {os.path.getsize(os.path.join(DLL_DIR, f))//1024} KB", flush=True)
        status = "ok"
        try:
            rc, out = whole(os.path.join(DLL_DIR, f), outdir)
            if rc != 0:
                status = f"whole-error: {out[-300:]}"
                print("  " + status, flush=True)
        except subprocess.TimeoutExpired:
            status = "whole-timeout"
            print("  whole-assembly timeout -> batched fallback", flush=True)
        if status not in ("ok",):
            status = batched(os.path.join(DLL_DIR, f), outdir, name)
        if status == "ok":
            n = sum(len(fs) for _, _, fs in os.walk(outdir))
            st["done"].append(name)
            json.dump(st, open(STATE, "w"))
            print(f"  OK {name}: {n} files in {time.time()-t0:.0f}s", flush=True)
        else:
            print(f"  FAIL {name}: {status}", flush=True)
    print("ALL DONE", flush=True)

if __name__ == "__main__":
    main()
