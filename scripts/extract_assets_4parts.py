#!/usr/bin/env python3
"""Extract all Last War assets into 4 part-staging dirs, then zip each part.

Phases:
  python3 extract_assets_4parts.py extract <bundle_dir> <stage_dir>
  python3 extract_assets_4parts.py core <binData_dir> <stage_dir>
  python3 extract_assets_4parts.py zip <stage_dir> <download_dir> <version>
"""
import warnings, os, sys, csv, re, glob, hashlib, json, shutil, traceback, subprocess
from multiprocessing import Pool
warnings.filterwarnings("ignore")
import UnityPy
UnityPy.config.FALLBACK_UNITY_VERSION = "2019.4.41f1"

STAGE = "/home/z/my-project/apk_analysis/stage"
MAN = "/home/z/my-project/scripts/_manifest_all.csv"

def h8(s): return hashlib.md5(s.encode()).hexdigest()[:8]
def safe(s): return re.sub(r'[^\w\-. ()]', '_', str(s))[:110].strip('._ ') or "unnamed"

def classify(desc):
    """desc = bundle filename minus _hash -> (part, category, sub)"""
    d = re.sub(r'^gameres_', '', desc)
    def has(*ks): return any(k in d for k in ks)
    if has('sound', 'music', 'dub', 'audio'):
        return ("Part2_Audio_Fonts", "Audio", None)
    if has('font'):
        return ("Part2_Audio_Fonts", "Fonts", "fonts")
    if has('effect'):
        return ("Part4_Effects_Other", "Effects", safe(d))
    if has('activity'):
        return ("Part1_UI_Icons", "Activity_UI", "sprites")
    m = re.search(r'cars_a_hero_(.+?)_(texture|mesh|animation|animator|material|prefab)', d)
    if m or has('herobody'):
        name = m.group(1) if m else safe(d)
        kind = "mesh" if has('mesh') else "textures"
        return ("Part3_Models_World", "Heroes", f"{safe(name)}/{kind}")
    if has('zombie'):
        return ("Part3_Models_World", "Zombies", "mesh" if has('mesh') else "textures")
    if has('characters'):
        return ("Part3_Models_World", "Characters", "mesh" if has('mesh') else "textures")
    if has('monster'):
        return ("Part3_Models_World", "Monsters", "mesh" if has('mesh') else "sprites")
    if has('environment', 'build', 'prop', 'plant', 'road', 'scene', 'terrain'):
        return ("Part3_Models_World", "World_Buildings", safe(d))
    if has('art_model', 'models', 'anim', 'vehicle', 'cars'):
        return ("Part3_Models_World", "Models_Anim", "mesh" if has('mesh') else "textures")
    if has('skel', 'atlas') and not has('ui', 'main'):
        return ("Part1_UI_Icons", "UI_Icons", "text")
    if has('ui', 'atlas', 'icon', 'main', 'newbie', 'biaoqing', 'spine'):
        t = "text" if has('skel', 'atlas') else "sprites"
        return ("Part1_UI_Icons", "UI_Icons", t)
    return ("Part4_Effects_Other", "Other", safe(d))

def audio_cat(name):
    n = name.lower()
    if n.startswith(("plot", "dub", "voice", "cv_")): return "Voice"
    if n.startswith(("bgm", "music", "song")): return "Music"
    return "SFX"

def man_row(row):
    with open(MAN, "a", newline="", encoding="utf-8") as f:
        csv.writer(f).writerow(row)

def out_path(part, category, sub, name, ext, pid):
    base = os.path.join(STAGE, part, category, sub) if sub else os.path.join(STAGE, part, category)
    os.makedirs(base, exist_ok=True)
    p = os.path.join(base, f"{safe(name)}.{ext}")
    if os.path.exists(p):
        p = os.path.join(base, f"{safe(name)}_{h8(str(pid))}.{ext}")
    return p

def process_bundle(bundle_path):
    try:
        b = os.path.basename(bundle_path)
        desc = re.sub(r'_[0-9a-f]{32}$', '', b[:-len('.bundle')])
        part, category, sub = classify(desc)
        env = UnityPy.load(bundle_path)
        n = 0
        for o in env.objects:
            t = o.type.name
            pid = o.path_id
            try:
                d = o.read()
                name = getattr(d, "m_Name", "") or "unnamed"
                if t == "Texture2D":
                    p = out_path(part, category, sub or "textures", name, "png", pid)
                    d.image.save(p); n += 1
                    man_row([part, category, "Texture2D", name, os.path.relpath(p, STAGE), desc])
                elif t == "Sprite":
                    p = out_path(part, category, sub or "sprites", name, "png", pid)
                    d.image.save(p); n += 1
                    man_row([part, category, "Sprite", name, os.path.relpath(p, STAGE), desc])
                elif t == "TextAsset":
                    sc = d.m_Script
                    sc = sc if isinstance(sc, bytes) else str(sc).encode("utf-8", "replace")
                    p = out_path(part, category, sub or "text", name, "txt", pid)
                    open(p, "wb").write(sc); n += 1
                    man_row([part, category, "TextAsset", name, os.path.relpath(p, STAGE), desc])
                elif t == "Font":
                    fd = bytes(d.m_FontData) if d.m_FontData else b""
                    ext = ("otf" if fd[:4] == b"OTTO" else "ttf") if fd[:4] in (b"OTTO", b"\x00\x01\x00\x00", b"true") else "bin"
                    p = out_path(part, category, sub or "fonts", name, ext, pid)
                    open(p, "wb").write(fd); n += 1
                    man_row([part, category, "Font", name, os.path.relpath(p, STAGE), desc])
                elif t == "AudioClip":
                    for sname, sdata in (d.samples or {}).items():
                        cat = audio_cat(name)
                        p = out_path(part, "Audio", cat, name, "wav", pid)
                        open(p, "wb").write(sdata if isinstance(sdata, bytes) else bytes(sname))
                        man_row([part, "Audio/" + cat, "AudioClip", name, os.path.relpath(p, STAGE), desc])
                        n += 1
                    if not d.samples:
                        data = bytes(d.m_AudioData) if d.m_AudioData else b""
                        if data:
                            p = out_path(part, "Audio", "Misc", name, "fsb", pid)
                            open(p, "wb").write(data); n += 1
                            man_row([part, "Audio/Misc", "AudioClip", name, os.path.relpath(p, STAGE), desc])
                elif t == "Mesh":
                    txt = d.export()
                    p = out_path(part, category, sub or "mesh", name, "obj", pid)
                    with open(p, "w", encoding="utf-8", errors="replace") as f:
                        f.write(txt)
                    n += 1
                    man_row([part, category, "Mesh", name, os.path.relpath(p, STAGE), desc])
            except Exception as e:
                man_row([part, category, t, name, "", f"ERR {e}"[:100]])
        return ("OK", b, n)
    except Exception as e:
        return ("ERR", os.path.basename(bundle_path), str(e)[:100])

STATE = "/home/z/my-project/scripts/_extract_state.txt"

def phase_extract(bundle_dir):
    if not os.path.exists(MAN):
        with open(MAN, "w", newline="", encoding="utf-8") as f:
            csv.writer(f).writerow(["part", "category", "type", "name", "file", "bundle"])
    done = set()
    if os.path.exists(STATE):
        with open(STATE) as f:
            done = set(x.strip() for x in f if x.strip())
    bundles = [b for b in sorted(glob.glob(os.path.join(bundle_dir, "*.bundle")))
               if os.path.basename(b) not in done]
    print(f"[extract] {len(bundles)} bundles to do ({len(done)} already done)", flush=True)
    stats = {"OK": 0, "ERR": 0, "n": 0}
    with Pool(6) as pool:
        for i, (st, b, n) in enumerate(pool.imap_unordered(process_bundle, bundles, chunksize=2)):
            stats[st] += 1; stats["n"] += n
            with open(STATE, "a") as sf:
                sf.write(b + "\n")
            if (i + 1) % 100 == 0:
                print(f"[extract] {i+1}/{len(bundles)} files={stats['n']} errBundles={stats['ERR']}", flush=True)
    print("[extract] FINAL", stats, flush=True)

def phase_core(bin_data):
    out = os.path.join(STAGE, "Part2_Audio_Fonts", "Core_Data")
    os.makedirs(out, exist_ok=True)
    work = "/home/z/my-project/apk_analysis/work/core"
    os.makedirs(work, exist_ok=True)
    jobs = {}
    for f in os.listdir(bin_data):
        if f in ("unity default resources", "unity_builtin_extra", "level0", "level1"):
            jobs[f] = os.path.join(bin_data, f)
    for stem, nparts in (("sharedassets0.assets", 2), ("globalgamemanagers.assets", 5)):
        s0 = os.path.join(bin_data, stem + ".split0")
        if os.path.exists(s0):
            tmp = os.path.join(work, stem)
            with open(tmp, "wb") as w:
                for i in range(nparts):
                    part = f"{bin_data}/{stem}.split{i}"
                    if os.path.exists(part):
                        with open(part, "rb") as r:
                            shutil.copyfileobj(r, w)
            jobs[stem] = tmp
    rows = []
    for label, path in jobs.items():
        try:
            env = UnityPy.load(path)
            for o in env.objects:
                t = o.type.name
                if t not in ("Texture2D", "Sprite", "Font", "TextAsset", "AudioClip"): continue
                try:
                    d = o.read()
                    name = safe(d.m_Name or "unnamed")
                    sub = {"Texture2D": "textures", "Sprite": "sprites", "Font": "fonts", "TextAsset": "text", "AudioClip": "audio"}[t]
                    os.makedirs(os.path.join(out, label, sub), exist_ok=True)
                    if t in ("Texture2D", "Sprite"):
                        p = os.path.join(out, label, sub, f"{name}.png"); d.image.save(p)
                    elif t == "Font":
                        fd = bytes(d.m_FontData) if d.m_FontData else b""
                        ext = ".otf" if fd[:4] == b"OTTO" else ".ttf"
                        p = os.path.join(out, label, sub, f"{name}{ext}"); open(p, "wb").write(fd)
                    elif t == "TextAsset":
                        sc = d.m_Script
                        sc = sc if isinstance(sc, bytes) else str(sc).encode()
                        p = os.path.join(out, label, sub, f"{name}.txt"); open(p, "wb").write(sc)
                    else:
                        p = os.path.join(out, label, sub, f"{name}.wav")
                        for sn, sd in (d.samples or {}).items():
                            open(p, "wb").write(sd)
                    rows.append([label, t, name, os.path.relpath(p, STAGE)])
                except Exception:
                    pass
        except Exception as e:
            print(f"[core] {label} fail {e}", flush=True)
    with open(os.path.join(out, "_index.csv"), "w", newline="") as f:
        w = csv.writer(f); w.writerow(["source", "type", "name", "file"]); w.writerows(rows)
    print(f"[core] FINAL {len(rows)}", flush=True)

def phase_zip(dl, version):
    parts = ["Part1_UI_Icons", "Part2_Audio_Fonts", "Part3_Models_World", "Part4_Effects_Other"]
    for p in parts:
        src = os.path.join(STAGE, p)
        if os.path.isdir(src):
            shutil.copy(MAN, os.path.join(src, "_manifest.csv"))
    for p in parts:
        src = os.path.join(STAGE, p)
        if not os.path.isdir(src):
            print(f"[zip] missing {p}"); continue
        out = os.path.join(os.path.abspath(dl), f"LastWar_{version}_Assets_{p}.zip")
        print(f"[zip] {out}", flush=True)
        r = subprocess.run(["zip", "-r", "-1", "-q", out, "."], cwd=src)
        print(f"[zip] rc={r.returncode} {os.path.getsize(out)/1048576:.0f} MB", flush=True)
        shutil.rmtree(src, ignore_errors=True)
    print("[zip] DONE", flush=True)

if __name__ == "__main__":
    ph = sys.argv[1]
    if ph == "extract":
        phase_extract(sys.argv[2])
    elif ph == "core":
        phase_core(sys.argv[2])
    elif ph == "zip":
        phase_zip(sys.argv[2], sys.argv[3])
