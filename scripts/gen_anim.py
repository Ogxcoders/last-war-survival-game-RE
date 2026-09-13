#!/usr/bin/env python3
"""Generate Unity-importable .anim YAML files from the typetree JSON dumps.

Implements AssetRipper's AnimationClipConverter logic:
  streamed (channels 0..S-1) -> dense (S..S+D-1) -> constant (S+D..end)
  streamed key = [index i32, coefX f32, coefY f32, coefZ f32, value f32]
  Transform bindings: attr 1=Translation(3) 2=Rotation(4) 3=Scaling(3) 4=Euler(3)
  Paths = crc32(path) resolved via hierarchy scan; attributes via crc32 dict.
"""
import glob
import json
import os
import struct
import sys
import zlib
from multiprocessing import Pool

CLIPS = "/home/z/my-project/apk_analysis/github_repo/Animations/UnityClips"
OUT = "/home/z/my-project/apk_analysis/work/anim_yml"
PATHS = "/home/z/my-project/apk_analysis/work/paths.json"
ATTRS = "/home/z/my-project/apk_analysis/work/attr_table.json"

TT_DIM = {1: 3, 2: 4, 3: 3, 4: 3}          # Translation Rotation Scaling Euler
TT_NAME = {1: "m_LocalPosition", 2: "m_LocalRotation", 3: "m_LocalScale", 4: "m_LocalEulerHint"}

f32 = lambda u: struct.unpack("<f", struct.pack("<I", u & 0xFFFFFFFF))[0]
u32 = lambda f: struct.unpack("<I", struct.pack("<f", f))[0]

def g(x, nd=7):
    return ("%.*g" % (nd, x))

PATHS_MAP, ATTR_MAP = {}, {}

def init():
    global PATHS_MAP, ATTR_MAP
    PATHS_MAP = json.load(open(PATHS))
    ATTR_MAP = json.load(open(ATTRS))

def path_of(h):
    return PATHS_MAP.get(str(h)) or ("UnknownPath_%08x" % h)

def attr_of(h, is_script):
    if h in (1, 2, 3, 4):
        return TT_NAME[h]
    r = ATTR_MAP.get(str(h))
    if r:
        return r
    return ("ScriptProperty_%08x" % h) if is_script else ("UnknownProperty_%08x" % h)

def parse_streamed(words):
    """-> list of frames {time, keys: {index: (cx,cy,cz,val)}} (+ trailing dummy)."""
    frames = []
    c = 0
    n = len(words)
    while c < n:
        t = f32(words[c]); c += 1
        cnt = words[c]; c += 1
        keys = {}
        last = None
        for _ in range(cnt):
            idx = words[c]  # int32 (can be big unsigned bit pattern? index fits)
            cx = f32(words[c + 1]); cy = f32(words[c + 2]); cz = f32(words[c + 3])
            v = f32(words[c + 4])
            c += 5
            keys[idx] = (cx, cy, cz, v)  # dedupe: keep last
        frames.append((t, keys))
    return frames

class Curve:
    __slots__ = ("keys",)
    def __init__(self):
        self.keys = []  # (time, [vals], [ins], [outs])

def conv(clip_json_path):
    rel = os.path.relpath(clip_json_path, CLIPS)
    outp = os.path.join(OUT, rel[:-5] + ".anim")  # replace .json
    os.makedirs(os.path.dirname(outp), exist_ok=True)
    try:
        d = json.load(open(clip_json_path))
    except Exception:
        return ("unreadable", 0)
    name = d.get("m_Name", "clip")
    legacy = bool(d.get("m_Legacy"))
    rot_c, pos_c, scl_c, euler_c, float_c = {}, {}, {}, {}, {}
    npptr = 0

    def add(cdict, key, dim, t, vals, ins=None, outs=None):
        c = cdict.setdefault(key, Curve())
        c.keys.append((t, list(vals), list(ins or [0] * dim), list(outs or [0] * dim)))

    if legacy:
        # plaintext curves already in typetree
        for rc in d.get("m_RotationCurves") or []:
            for k in rc["curve"]["m_Curve"]:
                add(rot_c, rc["path"], 4, k["time"], [k["value"]["x"], k["value"]["y"], k["value"]["z"], k["value"]["w"]])
        for pc in d.get("m_PositionCurves") or []:
            for k in pc["curve"]["m_Curve"]:
                add(pos_c, pc["path"], 3, k["time"], [k["value"]["x"], k["value"]["y"], k["value"]["z"]])
        for sc in d.get("m_ScaleCurves") or []:
            for k in sc["curve"]["m_Curve"]:
                add(scl_c, sc["path"], 3, k["time"], [k["value"]["x"], k["value"]["y"], k["value"]["z"]])
        for fc in d.get("m_FloatCurves") or []:
            attr = fc.get("attribute") or "m_Float"
            for k in fc["curve"]["m_Curve"]:
                v = k.get("value")
                if isinstance(v, dict):
                    v = v.get("x", 0)
                add(float_c, (fc.get("path") or "", attr, fc.get("classID", 114)), 1, k["time"], [v])
    else:
        mc = (d.get("m_MuscleClip") or {}).get("m_Clip") or {}
        data = mc.get("data") or {}
        bc = d.get("m_ClipBindingConstant") or {}
        gb = bc.get("genericBindings") or []
        # binding dims
        dims, channels = [], 0
        for bnd in gb:
            if bnd.get("isPPtrCurve"):
                dims.append(0)
                continue
            if bnd.get("typeID") == 4 and bnd.get("attribute") in TT_DIM:
                dim = TT_DIM[bnd["attribute"]]
            else:
                dim = 1
            dims.append(dim)
            channels += dim
        streamed = data.get("m_StreamedClip") or {}
        dense = data.get("m_DenseClip") or {}
        constant = data.get("m_ConstantClip") or {}
        s_frames = parse_streamed(streamed.get("data") or [])
        S = int(streamed.get("curveCount") or 0)
        D = int(dense.get("m_CurveCount") or 0)
        F = int(dense.get("m_FrameCount") or 0)
        SR = float(dense.get("m_SampleRate") or 0)
        B = float(dense.get("m_BeginTime") or 0)
        cdat = constant.get("data") or []
        stop = float((d.get("m_MuscleClip") or {}).get("m_StopTime") or 0)

        # --- streamed (channels 0..S-1) ---
        if s_frames:
            real = len(s_frames) - 1  # last frame is dummy
            for fi in range(1, real):  # skip sentinel frame 0 and dummy last
                t, keys = s_frames[fi]
                nxt = s_frames[fi + 1][1]
                items = list(keys.items())
                i = 0
                while i < len(items):
                    cid = items[i][0]
                    if cid >= channels:
                        i += 1
                        continue
                    bidx = None
                    acc = 0
                    for bi, dm in enumerate(dims):
                        if acc <= cid < acc + dm or (dm == 0 and acc == cid):
                            bidx = bi
                            break
                        acc += dm
                    if bidx is None:
                        i += 1
                        continue
                    dim = dims[bidx]
                    bnd = gb[bidx]
                    if bnd.get("isPPtrCurve"):
                        npptr += 1
                        i += dim or 1
                        continue
                    # require complete consecutive block
                    block_ok = all(
                        i + o < len(items) and items[i + o][0] == cid + o
                        for o in range(dim)
                    )
                    if not block_ok:
                        i += 1
                        continue
                    vals = [items[i + o][1][3] for o in range(dim)]
                    ins, outs = [], []
                    for o in range(dim):
                        cx, cy, cz, v = items[i + o][1]
                        nk = nxt.get(cid + o)
                        if (cx, cy, cz) != (0, 0, 0) and nk is not None:
                            dt = s_frames[fi + 1][0] - t
                            ins.append(3 * cx * dt * dt + 2 * cy * dt + cz)
                            outs.append(cz)
                        else:
                            ins.append(0.0)
                            outs.append(0.0)
                    emit_transform_or_float(bnd, cid, path_of, t, vals, ins, outs,
                                            rot_c, pos_c, scl_c, euler_c, float_c, add, attr_of)
                    i += dim
        # --- dense (global channels S..S+D-1) ---
        if F and D:
            sa = dense["m_SampleArray"]
            for fr in range(F):
                t = fr / SR + B
                acc = 0
                for bi, dm in enumerate(dims):
                    bnd = gb[bi]
                    if dm == 0:
                        continue
                    lo, hi = acc, acc + dm          # global channel range of this binding
                    acc = hi
                    dlo, dhi = max(lo, S), min(hi, S + D)
                    if dlo != lo or dhi != hi:
                        continue   # binding straddles region boundary - skip, never pad
                    vals = []
                    for gc in range(dlo, dhi):
                        local = gc - S
                        vals.append(sa[fr * D + local] if fr * D + local < len(sa) else 0.0)
                    emit_transform_or_float(bnd, lo, path_of, t, vals, None, None,
                                            rot_c, pos_c, scl_c, euler_c, float_c, add, attr_of)
        # --- constant (global channels S+D..end) ---
        if cdat:
            pre = S + D
            times = [0.0] if (stop == 0) else [0.0, stop]
            for t in times:
                acc = 0
                for bi, dm in enumerate(dims):
                    bnd = gb[bi]
                    if dm == 0:
                        continue
                    lo, hi = acc, acc + dm          # global channel range
                    acc = hi
                    clo, chi = max(lo, pre), min(hi, pre + len(cdat))
                    if clo != lo or chi != hi:
                        continue   # straddles boundary - skip
                    if clo >= chi:
                        continue
                    vals = [cdat[gc - pre] for gc in range(clo, chi)]
                    emit_transform_or_float(bnd, lo, path_of, t, vals, None, None,
                                            rot_c, pos_c, scl_c, euler_c, float_c, add, attr_of)

    write_anim(outp, name, legacy, d, rot_c, pos_c, scl_c, euler_c, float_c)
    return ("ok", len(rot_c) + len(pos_c) + len(scl_c) + len(euler_c) + len(float_c))

def emit_transform_or_float(bnd, cid, path_of, t, vals, ins, outs,
                            rot_c, pos_c, scl_c, euler_c, float_c, add, attr_of):
    if bnd.get("typeID") == 4 and bnd.get("attribute") in TT_DIM:
        p = path_of(bnd.get("path", 0))
        att = bnd["attribute"]
        if att == 1:
            add(pos_c, p, 3, t, vals[:3], ins, outs)
        elif att == 2:
            add(rot_c, p, 4, t, vals[:4], ins, outs)
        elif att == 3:
            add(scl_c, p, 3, t, vals[:3], ins, outs)
        elif att == 4:
            add(euler_c, p, 3, t, vals[:3], ins, outs)
    else:
        p = path_of(bnd.get("path", 0))
        is_script = bnd.get("typeID") == 114
        a = attr_of(bnd.get("attribute", 0), is_script)
        key = (p, a, bnd.get("typeID", 114))
        add(float_c, key, 1, t, vals[:1], ins, outs)

def _pad(v, n):
    v = list(v or [])
    if len(v) < n:
        global PAD_COUNT
        PAD_COUNT += 1
        if PAD_COUNT < 6:
            import traceback
            traceback.print_stack()
            print("PAD-CHECK: short list", v, "need", n)
        v = v + [0.0] * (n - len(v))
    return v[:n]

PAD_COUNT = 0

def kv_scalar(t, v, i, o):
    return ("      - serializedVersion: 3\n"
            "        time: %s\n"
            "        value: %s\n"
            "        inSlope: %s\n"
            "        outSlope: %s\n"
            "        tangentMode: 0\n"
            "        weightedMode: 0\n"
            "        inWeight: 0.33333334\n"
            "        outWeight: 0.33333334\n" % (g(t), g(v), g(i), g(o)))

def kv_vec(t, v, i, o):
    v, i, o = _pad(v, 3), _pad(i, 3), _pad(o, 3)
    return ("      - serializedVersion: 3\n"
            "        time: %s\n"
            "        value: {x: %s, y: %s, z: %s}\n"
            "        inSlope: {x: %s, y: %s, z: %s}\n"
            "        outSlope: {x: %s, y: %s, z: %s}\n"
            "        tangentMode: 0\n"
            "        weightedMode: 0\n"
            "        inWeight: {x: 0.33333334, y: 0.33333334, z: 0.33333334}\n"
            "        outWeight: {x: 0.33333334, y: 0.33333334, z: 0.33333334}\n"
            % (g(t), g(v[0]), g(v[1]), g(v[2]), g(i[0]), g(i[1]), g(i[2]), g(o[0]), g(o[1]), g(o[2])))

def kv_quat(t, v, i, o):
    v, i, o = _pad(v, 4), _pad(i, 4), _pad(o, 4)
    return ("      - serializedVersion: 3\n"
            "        time: %s\n"
            "        value: {x: %s, y: %s, z: %s, w: %s}\n"
            "        inSlope: {x: %s, y: %s, z: %s, w: %s}\n"
            "        outSlope: {x: %s, y: %s, z: %s, w: %s}\n"
            "        tangentMode: 0\n"
            "        weightedMode: 0\n"
            "        inWeight: {x: 0.33333334, y: 0.33333334, z: 0.33333334, w: 0.33333334}\n"
            "        outWeight: {x: 0.33333334, y: 0.33333334, z: 0.33333334, w: 0.33333334}\n"
            % (g(t), g(v[0]), g(v[1]), g(v[2]), g(v[3]),
               g(i[0]), g(i[1]), g(i[2]), g(i[3]), g(o[0]), g(o[1]), g(o[2]), g(o[3])))

def write_anim(outp, name, legacy, d, rot_c, pos_c, scl_c, euler_c, float_c):
    b = d.get("m_Bounds") or {}
    L = []
    L.append("%YAML 1.1\n%TAG !u! tag:unity3d.com,2011:\n--- !u!74 &7400000\n")
    L.append("AnimationClip:\n  m_ObjectHideFlags: 0\n")
    L.append("  m_CorrespondingSourceObject: {fileID: 0}\n  m_PrefabInstance: {fileID: 0}\n  m_PrefabAsset: {fileID: 0}\n")
    L.append("  m_Name: %s\n  serializedVersion: 6\n" % json.dumps(name))
    L.append("  m_Legacy: %d\n  m_Compressed: 0\n  m_UseHighQualityCurve: 1\n" % (1 if legacy else 0))
    # rotations
    L.append("  m_RotationCurves:\n" if rot_c else "  m_RotationCurves: []\n")
    for p, c in rot_c.items():
        c.keys.sort(key=lambda k: k[0])
        L.append("  - curve:\n      serializedVersion: 2\n      m_Curve:\n")
        for t, v, i, o in c.keys:
            L.append(kv_quat(t, v, i, o))
        L.append("      m_PreInfinity: 2\n      m_PostInfinity: 2\n      m_RotationOrder: 4\n")
        L.append("    path: %s\n" % json.dumps(p))
    L.append("  m_CompressedRotationCurves: []\n")
    L.append("  m_EulerCurves:\n" if euler_c else "  m_EulerCurves: []\n")
    for p, c in euler_c.items():
        c.keys.sort(key=lambda k: k[0])
        L.append("  - curve:\n      serializedVersion: 2\n      m_Curve:\n")
        for t, v, i, o in c.keys:
            L.append(kv_vec(t, v, i, o))
        L.append("      m_PreInfinity: 2\n      m_PostInfinity: 2\n      m_RotationOrder: 4\n")
        L.append("    path: %s\n" % json.dumps(p))
    L.append("  m_PositionCurves:\n" if pos_c else "  m_PositionCurves: []\n")
    for p, c in pos_c.items():
        c.keys.sort(key=lambda k: k[0])
        L.append("  - curve:\n      serializedVersion: 2\n      m_Curve:\n")
        for t, v, i, o in c.keys:
            L.append(kv_vec(t, v, i, o))
        L.append("      m_PreInfinity: 2\n      m_PostInfinity: 2\n      m_RotationOrder: 4\n")
        L.append("    path: %s\n" % json.dumps(p))
    L.append("  m_ScaleCurves:\n" if scl_c else "  m_ScaleCurves: []\n")
    for p, c in scl_c.items():
        c.keys.sort(key=lambda k: k[0])
        L.append("  - curve:\n      serializedVersion: 2\n      m_Curve:\n")
        for t, v, i, o in c.keys:
            L.append(kv_vec(t, v, i, o))
        L.append("      m_PreInfinity: 2\n      m_PostInfinity: 2\n      m_RotationOrder: 4\n")
        L.append("    path: %s\n" % json.dumps(p))
    L.append("  m_FloatCurves:\n" if float_c else "  m_FloatCurves: []\n")
    for (p, a, cid), c in float_c.items():
        c.keys.sort(key=lambda k: k[0])
        L.append("  - curve:\n      serializedVersion: 2\n      m_Curve:\n")
        for t, v, i, o in c.keys:
            L.append(kv_scalar(t, v[0], i[0], o[0]))
        L.append("      m_PreInfinity: 2\n      m_PostInfinity: 2\n      m_RotationOrder: 4\n")
        L.append("    attribute: %s\n" % json.dumps(a))
        L.append("    path: %s\n" % json.dumps(p))
        L.append("    classID: %d\n" % cid)
        L.append("    script: {fileID: 0}\n")
    L.append("  m_PPtrCurves: []\n")
    L.append("  m_SampleRate: %s\n" % g(d.get("m_SampleRate") or 60))
    L.append("  m_WrapMode: %d\n" % (d.get("m_WrapMode") or 0))
    ce, ex = b.get("m_Center") or {}, b.get("m_Extent") or {}
    L.append("  m_Bounds:\n    m_Center: {x: %s, y: %s, z: %s}\n    m_Extent: {x: %s, y: %s, z: %s}\n"
             % (g(ce.get("x", 0)), g(ce.get("y", 0)), g(ce.get("z", 0)),
                g(ex.get("x", 0)), g(ex.get("y", 0)), g(ex.get("z", 0))))
    L.append("  m_ClipBindingConstant:\n    genericBindings: []\n    pptrCurveMapping: []\n")
    L.append("  m_Events: []\n")
    with open(outp, "w") as f:
        f.write("".join(L))

def main():
    os.makedirs(OUT, exist_ok=True)
    files = sorted(glob.glob(os.path.join(CLIPS, "*", "*.AnimationClip.json")))
    print(f"{len(files)} clips", flush=True)
    if len(sys.argv) > 1 and sys.argv[1] == "one":
        init()
        print(conv(files[0]))
        return
    nok = nerr = ncurves = 0
    with Pool(6, initializer=init) as pool:
        for status, n in pool.imap_unordered(conv, files, chunksize=8):
            if status == "ok":
                nok += 1
                ncurves += n
            else:
                nerr += 1
    print(f"GEN DONE: {nok} ok, {nerr} err, {ncurves} curves", flush=True)

if __name__ == "__main__":
    main()
