#!/usr/bin/env python3
"""Convert build-format AnimatorController typetree JSONs -> editor .controller YAML
(Unity 2019.4 schema) wired to the generated .anim files by deterministic GUID.

Semantics mirror AssetRipper's VirtualAnimationFactory / AnimatorStateMachineContext:
  - leaf blend-tree node: m_ClipID = index into m_AnimationClips
  - state names: m_TOS[m_NameID] (plaintext debug table)
  - transitions: m_DestinationState = index into state array (30000 = exit -> skip)
  - conditions: m_EventID = crc32(param name) resolved via m_TOS corpus
  - selector (Entry/Exit) transitions are implicit -> omitted
Our dataset: 444 controllers with 1 SM, 3 empty, 1 synced 2nd layer, 1 real blend tree.
"""
import glob
import json
import os
import sys

sys.path.insert(0, "/home/z/my-project/scripts")
from unity_guids import guid_anim, guid_ctrl, write_meta, sanitize, CTRL_MAIN_ID

W = "/home/z/my-project/apk_analysis/work"
REPO = "/home/z/my-project/apk_analysis/github_repo/Animations"
OUT = "/home/z/my-project/apk_analysis/work/ctrl_yml"
CTRLS = "/home/z/my-project/apk_analysis/github_repo/Animations/Animators"

PPTR_CLIP = lambda g: "{fileID: 7400000, guid: %s, type: 2}" % g

def ystr(s):
    return json.dumps(s, ensure_ascii=False)

def ynum(v):
    if isinstance(v, float):
        return ("%g" % v) if v != int(v) or abs(v) > 1e15 else str(int(v))
    return str(v)

def load_all():
    aext = json.load(open(f"{W}/animator_externals.json"))     # bundle -> {cab: [ext]}
    c2b = json.load(open(f"{W}/cab2bundle.json"))              # cab -> bundle
    clipmap = json.load(open(f"{W}/clipmap.json"))             # cab:pid -> clip name
    ctrlmap = json.load(open(f"{W}/ctrlmap.json"))             # cab:pid -> ctrl name
    return aext, c2b, clipmap, ctrlmap

def build_clip_registry(clipmap, c2b):
    groups = {}
    for k, nm in clipmap.items():
        cab, pid = k.rsplit(":", 1)
        b = c2b.get(cab)
        if b:
            groups.setdefault((b, sanitize(nm) or "unnamed"), []).append((cab, int(pid)))
    reg = {}
    for (b, stem), lst in groups.items():
        # all same-name duplicates share the single shipped .anim file
        for cab, pid in lst:
            reg[f"{cab}:{pid}"] = (b, stem)
    return reg

def build_crc_names(controllers, clipmap):
    # collect candidate strings, then map crc32(string) -> string
    names = set()
    for d, _ in controllers:
        for h, s in d.get("m_TOS") or []:
            names.add(s)
            if " -> " in s:
                for part in s.split(" -> "):
                    names.add(part)
                    if "." in part:
                        names.add(part.rsplit(".", 1)[-1])
            elif "." in s:
                names.add(s.rsplit(".", 1)[-1])
    for nm in clipmap.values():
        if nm:
            names.add(nm)
    import zlib
    table = {}
    for s in names:
        try:
            table[zlib.crc32(s.encode("utf-8")) & 0xFFFFFFFF] = s
        except Exception:
            pass
    return table

class Ctx:
    def __init__(self, aext, c2b, clipmap, clipreg, crcnames):
        self.aext = aext
        self.c2b = c2b
        self.clipmap = clipmap
        self.clipreg = clipreg
        self.crcnames = crcnames
        self.stats = {"ctrl": 0, "states": 0, "trans": 0, "any": 0, "bt": 0,
                      "clip_unresolved": 0, "exit_trans": 0, "param_named": 0, "param_crc": 0}

    def clip_pptr(self, cab, pid):
        nm = self.clipmap.get(f"{cab}:{pid}")
        if nm is None:
            self.stats["clip_unresolved"] += 1
            return None
        ent = self.clipreg.get(f"{cab}:{pid}")
        if ent is None:
            self.stats["clip_unresolved"] += 1
            return None
        b, stem = ent
        return PPTR_CLIP(guid_anim(b, stem))

    def param_name(self, crcid):
        if not crcid:
            return "", False
        nm = self.crcnames.get(crcid)
        if nm:
            self.stats["param_named"] += 1
            return nm, True
        self.stats["param_crc"] += 1
        return "Param_%08x" % crcid, False

def doc_header(cls, fid):
    return ["%YAML 1.1\n%TAG !u! tag:unity3d.com,2011:\n--- !u!" + str(cls) + " &" + str(fid) + "\n"]

def emit_state(ctx, st, idx, clips, tos, ids, docs, fid_state):
    sd = st["data"]
    L = doc_header(1101, fid_state)
    name = tos.get(sd["m_NameID"]) or tos.get(sd["m_FullPathID"])
    if name:
        if "." in name:
            name = name.rsplit(".", 1)[-1]
    else:
        name = "State_%08x" % sd["m_FullPathID"]
    L.append("AnimatorState:\n  serializedVersion: 5\n")
    L.append("  m_ObjectHideFlags: 1\n  m_CorrespondingSourceObject: {fileID: 0}\n")
    L.append("  m_PrefabInstance: {fileID: 0}\n  m_PrefabAsset: {fileID: 0}\n")
    L.append("  m_Name: %s\n" % ystr(name))
    L.append("  m_Speed: %s\n  m_CycleOffset: %s\n" % (ynum(sd["m_Speed"]), ynum(sd["m_CycleOffset"])))
    L.append("  m_Transitions:\n" if sd["m_TransitionConstantArray"] else "  m_Transitions: []\n")
    for t in sd["m_TransitionConstantArray"]:
        tid = ids.trans_map.get(id(t["data"]))
        if tid is not None:
            L.append("  - {fileID: %d}\n" % tid)
    L.append("  m_StateMachineBehaviours: []\n")
    x = 260 * (idx % 5)
    y = -100 * (idx // 5)
    L.append("  m_Position: {x: %d, y: %d, z: 0}\n" % (x, y))
    # motion
    bts = sd.get("m_BlendTreeConstantArray") or []
    idxs = sd.get("m_BlendTreeConstantIndexArray") or []
    motion = "{fileID: 0}"
    if bts and idxs and idxs[0] != 0xFFFFFFFF:
        node0 = bts[0]["data"]["m_NodeArray"][0]["data"]
        if not node0["m_ChildIndices"]:
            cid = node0["m_ClipID"]
            if cid != 0xFFFFFFFF and cid < len(clips):
                p = ctx.clip_pptr(clips[cid][0], clips[cid][1])
                motion = p or "{fileID: 0}"
        else:
            bt_fid = ids.bt_map.get(id(bts[0]["data"]))
            motion = "{fileID: %d}" % bt_fid
    L.append("  m_Motion: %s\n" % motion)
    L.append("  m_Tag: %s\n" % ystr(tos.get(sd["m_TagID"], "")))
    L.append("  m_SpeedParameterActive: %d\n" % (1 if sd["m_SpeedParamID"] else 0))
    L.append("  m_MirrorParameterActive: %d\n" % (1 if sd["m_MirrorParamID"] else 0))
    L.append("  m_CycleOffsetParameterActive: %d\n" % (1 if sd["m_CycleOffsetParamID"] else 0))
    L.append("  m_TimeParameterActive: %d\n" % (1 if sd["m_TimeParamID"] else 0))
    L.append("  m_IKOnFeet: %d\n" % (1 if sd["m_IKOnFeet"] else 0))
    L.append("  m_Mirror: %d\n" % (1 if sd["m_Mirror"] else 0))
    L.append("  m_WriteDefaultValues: %d\n" % (1 if sd["m_WriteDefaultValues"] else 0))
    L.append("  m_LoopTime: %d\n" % (1 if sd["m_Loop"] else 0))
    sp, _ = ctx.param_name(sd["m_SpeedParamID"])
    mp, _ = ctx.param_name(sd["m_MirrorParamID"])
    cp, _ = ctx.param_name(sd["m_CycleOffsetParamID"])
    tp, _ = ctx.param_name(sd["m_TimeParamID"])
    L.append("  m_SpeedParameter: %s\n  m_MirrorParameter: %s\n" % (ystr(sp), ystr(mp)))
    L.append("  m_CycleOffsetParameter: %s\n  m_TimeParameter: %s\n" % (ystr(cp), ystr(tp)))
    docs.append("".join(L))
    ctx.stats["states"] += 1
    return name

def emit_bt(ctx, btd, clips, tos, docs, fid_bt):
    """Emit a BlendTree doc for build BlendTreeConstant data."""
    L = doc_header(1011, fid_bt)
    L.append("BlendTree:\n")
    L.append("  m_ObjectHideFlags: 1\n  m_CorrespondingSourceObject: {fileID: 0}\n")
    L.append("  m_PrefabInstance: {fileID: 0}\n  m_PrefabAsset: {fileID: 0}\n")
    L.append("  m_Name: %s\n" % ystr("Blend Tree"))
    nodes = btd["m_NodeArray"]
    root = nodes[0]["data"]
    L.append("  m_Children:\n" if root["m_ChildIndices"] else "  m_Children: []\n")
    for ci in root["m_ChildIndices"]:
        child = nodes[ci]["data"]
        L.append("  - serializedVersion: 2\n")
        if child["m_ChildIndices"]:
            # nested blend tree - not present in this dataset; placeholder null
            L.append("    m_Motion: {fileID: 0}\n")
        else:
            cid = child["m_ClipID"]
            p = "{fileID: 0}"
            if cid != 0xFFFFFFFF and cid < len(clips):
                p = ctx.clip_pptr(clips[cid][0], clips[cid][1]) or "{fileID: 0}"
            L.append("    m_Motion: %s\n" % p)
        thr = root["m_Blend1dData"]["data"]["m_ChildThresholdArray"]
        threshold = thr[ci] if ci < len(thr) else 0.0
        pos2 = root["m_Blend2dData"]["data"]["m_ChildPositionArray"]
        px = pos2[2 * ci] if 2 * ci < len(pos2) else 0.0
        py = pos2[2 * ci + 1] if 2 * ci + 1 < len(pos2) else 0.0
        L.append("    m_Threshold: %s\n" % ynum(float(threshold)))
        L.append("    m_Position: {x: %s, y: %s}\n" % (ynum(float(px)), ynum(float(py))))
        L.append("    m_TimeScale: %s\n" % ynum(1.0 / child["m_Duration"] if child["m_Duration"] else 1.0))
        L.append("    m_CycleOffset: %s\n" % ynum(child["m_CycleOffset"]))
        L.append("    m_DirectBlendParameter: %s\n" % ystr(""))
        L.append("    m_Mirror: %d\n" % (1 if child["m_Mirror"] else 0))
    bp, _ = ctx.param_name(root["m_BlendEventID"] if root["m_BlendEventID"] != 0xFFFFFFFF else 0)
    bpy, _ = ctx.param_name(root["m_BlendEventYID"] if root["m_BlendEventYID"] != 0xFFFFFFFF else 0)
    L.append("  m_BlendParameter: %s\n  m_BlendParameterY: %s\n" % (ystr(bp), ystr(bpy)))
    L.append("  m_BlendType: %d\n" % root["m_BlendType"])
    thr = root["m_Blend1dData"]["data"]["m_ChildThresholdArray"]
    L.append("  m_MinThreshold: %s\n" % ynum(float(thr[0]) if thr else 0.0))
    L.append("  m_MaxThreshold: %s\n" % ynum(float(thr[-1]) if thr else 1.0))
    L.append("  m_UseAutomaticThresholds: 0\n")
    L.append("  m_NormalizedBlendValues: %d\n" % (1 if (root["m_BlendDirectData"]["data"].get("m_NormalizedBlendValues")) else 0))
    docs.append("".join(L))
    ctx.stats["bt"] += 1

def emit_trans(ctx, td, docs, fid_trans, dest_fid):
    L = doc_header(1102, fid_trans)
    L.append("AnimatorStateTransition:\n  serializedVersion: 3\n")
    L.append("  m_ObjectHideFlags: 1\n  m_CorrespondingSourceObject: {fileID: 0}\n")
    L.append("  m_PrefabInstance: {fileID: 0}\n  m_PrefabAsset: {fileID: 0}\n")
    L.append("  m_Name: %s\n" % ystr(""))
    conds = [c["data"] for c in td["m_ConditionConstantArray"]]
    real = [c for c in conds if c["m_ConditionMode"] != 7]
    L.append("  m_Conditions:\n" if real else "  m_Conditions: []\n")
    for c in real:
        pn, _ = ctx.param_name(c["m_EventID"])
        L.append("  - m_ConditionMode: %d\n" % c["m_ConditionMode"])
        L.append("    m_ConditionEvent: %s\n" % ystr(pn))
        L.append("    m_EventTreshold: %s\n" % ynum(float(c["m_EventThreshold"])))
    L.append("  m_Duration: %s\n" % ynum(float(td["m_TransitionDuration"])))
    L.append("  m_TransitionOffset: %s\n" % ynum(float(td["m_TransitionOffset"])))
    L.append("  m_ExitTime: %s\n" % ynum(float(td["m_ExitTime"])))
    L.append("  m_HasExitTime: %d\n" % (1 if td["m_HasExitTime"] else 0))
    L.append("  m_HasFixedDuration: %d\n" % (1 if td["m_HasFixedDuration"] else 0))
    L.append("  m_InterruptionSource: %d\n" % td["m_InterruptionSource"])
    L.append("  m_OrderedInterruption: %d\n" % (1 if td["m_OrderedInterruption"] else 0))
    L.append("  m_CanTransitionToSelf: %d\n" % (1 if td["m_CanTransitionToSelf"] else 0))
    L.append("  m_DestinationState: {fileID: %d}\n" % dest_fid)
    docs.append("".join(L))

class Ids:
    pass

def convert(ctx, path, outdir):
    d = json.load(open(path))
    bundle = os.path.basename(os.path.dirname(path))
    stem = os.path.basename(path)[: -len(".AnimatorController.json")]
    c = d["m_Controller"]
    tos = {t[0]: t[1] for t in d.get("m_TOS") or []}
    sfx = ctx.aext.get(bundle) or {}
    cabs = list(sfx.keys())
    own = cabs[0] if cabs else ""
    exts = sfx.get(own) or []

    # resolve clip PPtrs -> (cab, pid) pairs
    clips = []
    for pp in d.get("m_AnimationClips") or []:
        fid, pid = pp.get("m_FileID", 0), pp.get("m_PathID")
        if fid == 0:
            tcab = own
        elif fid > 0 and fid - 1 < len(exts):
            tcab = exts[fid - 1]
        else:
            tcab = None
        clips.append((tcab, pid) if tcab else (None, pid))

    docs = []
    ids = Ids()
    ids.trans_map = {}
    ids.bt_map = {}

    sms = c.get("m_StateMachineArray") or []
    layers = c.get("m_LayerArray") or []
    state_fid = 1101000000
    trans_fid = 1102000000
    bt_fid = 1011000000

    # ---- pre-allocate state fids per (sm, state idx)
    sm_state_fids = []
    for sm in sms:
        fids = [state_fid + i for i in range(len(sm["data"]["m_StateConstantArray"]))]
        sm_state_fids.append(fids)
        state_fid += len(fids)

    # ---- transitions (state-level), allocate + emit
    for smi, sm in enumerate(sms):
        s = sm["data"]
        for sti, st in enumerate(s["m_StateConstantArray"]):
            for t in st["data"]["m_TransitionConstantArray"]:
                td = t["data"]
                dest = td["m_DestinationState"]
                if dest >= 30000:
                    ctx.stats["exit_trans"] += 1
                    continue
                if dest >= len(sm_state_fids[smi]):
                    continue
                ids.trans_map[id(td)] = trans_fid
                emit_trans(ctx, td, docs, trans_fid, sm_state_fids[smi][dest])
                trans_fid += 1
                ctx.stats["trans"] += 1
    # ---- any-state transitions
    any_fids = []
    for smi, sm in enumerate(sms):
        s = sm["data"]
        for t in s.get("m_AnyStateTransitionConstantArray") or []:
            td = t["data"]
            dest = td["m_DestinationState"]
            if dest >= 30000 or dest >= len(sm_state_fids[smi]):
                continue
            ids.trans_map[id(td)] = trans_fid
            emit_trans(ctx, td, docs, trans_fid, sm_state_fids[smi][dest])
            any_fids.append(trans_fid)
            trans_fid += 1
            ctx.stats["any"] += 1
    # ---- blend trees
    for smi, sm in enumerate(sms):
        s = sm["data"]
        for st in s["m_StateConstantArray"]:
            bts = st["data"].get("m_BlendTreeConstantArray") or []
            idxs = st["data"].get("m_BlendTreeConstantIndexArray") or []
            if bts and idxs and idxs[0] != 0xFFFFFFFF:
                node0 = bts[0]["data"]["m_NodeArray"][0]["data"]
                if node0["m_ChildIndices"]:
                    ids.bt_map[id(bts[0]["data"])] = bt_fid
                    emit_bt(ctx, bts[0]["data"], clips, tos, docs, bt_fid)
                    bt_fid += 1
    # ---- states
    names = []
    for smi, sm in enumerate(sms):
        s = sm["data"]
        for sti, st in enumerate(s["m_StateConstantArray"]):
            names.append(emit_state(ctx, st, sti, clips, tos, ids, docs, sm_state_fids[smi][sti]))

    # ---- state machine docs + layers
    layer_docs = []
    used_sm_fids = set()
    for li, layer in enumerate(layers):
        ld = layer["data"]
        smi = ld.get("m_StateMachineIndex", 0)
        synced = ld.get("m_StateMachineSynchronizedLayerIndex", 0)
        blend = ld.get("(int&)m_LayerBlendingMode", 0)
        if synced:
            sm_name = "Base Layer" if li == 0 else "Layer %d" % li
            sm_ref = "{fileID: 0}"
        elif smi < len(sms):
            s = sms[smi]["data"]
            sel = s.get("m_SelectorStateConstantArray") or []
            sm_name = None
            if sel:
                sm_name = tos.get(sel[0]["data"].get("m_FullPathID"))
            if not sm_name:
                sm_name = "Base Layer" if li == 0 else "Layer %d" % li
            sm_fid = 1107000000 + smi
            used_sm_fids.add(sm_fid)
            sm_ref = "{fileID: %d}" % sm_fid
        else:
            sm_name = "Base Layer" if li == 0 else "Layer %d" % li
            sm_ref = "{fileID: 0}"
        L = ["  - serializedVersion: 5\n"]
        L.append("    m_Name: %s\n" % ystr(sm_name))
        L.append("    m_StateMachine: %s\n" % sm_ref)
        L.append("    m_Mask: {fileID: 0}\n    m_Motion: {fileID: 0}\n")
        L.append("    m_BlendingMode: %d\n" % blend)
        L.append("    m_SyncedLayerIndex: %d\n" % (synced - 1 if synced else -1))
        L.append("    m_DefaultWeight: %s\n" % ynum(float(ld.get("m_DefaultWeight", 0.0))))
        L.append("    m_IKPass: %d\n" % (1 if ld.get("m_IKPass") else 0))
        L.append("    m_SyncedLayerAffectsTiming: %d\n" % (1 if ld.get("m_SyncedLayerAffectsTiming") else 0))
        L.append("    m_StateMachineBehaviours: []\n")
        layer_docs.append("".join(L))
        if not synced and smi < len(sms):
            s = sms[smi]["data"]
            SL = doc_header(1107, 1107000000 + smi)
            SL.append("AnimatorStateMachine:\n  serializedVersion: 6\n")
            SL.append("  m_ObjectHideFlags: 1\n  m_CorrespondingSourceObject: {fileID: 0}\n")
            SL.append("  m_PrefabInstance: {fileID: 0}\n  m_PrefabAsset: {fileID: 0}\n")
            SL.append("  m_Name: %s\n" % ystr(sm_name))
            SL.append("  m_ChildStates:\n" if names else "  m_ChildStates: []\n")
            for sti in range(len(s["m_StateConstantArray"])):
                x = 260 * (sti % 5)
                y = -100 * (sti // 5)
                SL.append("  - m_State: {fileID: %d}\n" % sm_state_fids[smi][sti])
                SL.append("    m_Position: {x: %d, y: %d, z: 0}\n" % (x, y))
            SL.append("  m_ChildStateMachines: []\n")
            SL.append("  m_AnyStateTransitions:\n" if any_fids else "  m_AnyStateTransitions: []\n")
            for af in any_fids:
                SL.append("  - {fileID: %d}\n" % af)
            SL.append("  m_EntryTransitions: []\n")
            SL.append("  m_StateMachineTransitions: {}\n")
            SL.append("  m_StateMachineBehaviours: []\n")
            SL.append("  m_AnyStatePosition: {x: 50, y: 20, z: 0}\n")
            SL.append("  m_EntryPosition: {x: 50, y: 120, z: 0}\n")
            SL.append("  m_ExitPosition: {x: 800, y: 120, z: 0}\n")
            SL.append("  m_ParentStateMachinePosition: {x: 800, y: 20, z: 0}\n")
            dsi = s["m_DefaultState"]
            fids_i = sm_state_fids[smi]
            if dsi >= len(fids_i):
                dsi = 0
            SL.append("  m_DefaultState: {fileID: %d}\n" % fids_i[dsi] if fids_i else "  m_DefaultState: {fileID: 0}\n")
            docs.append("".join(SL))

    # ensure at least one layer
    if not layer_docs:
        layer_docs = ["  - serializedVersion: 5\n    m_Name: Base Layer\n    m_StateMachine: {fileID: 0}\n"
                      "    m_Mask: {fileID: 0}\n    m_Motion: {fileID: 0}\n    m_BlendingMode: 0\n"
                      "    m_SyncedLayerIndex: -1\n    m_DefaultWeight: 0\n    m_IKPass: 0\n"
                      "    m_SyncedLayerAffectsTiming: 0\n    m_StateMachineBehaviours: []\n"]

    # ---- parameters
    params = []
    seen = set()
    for sm in sms:
        s = sm["data"]
        for t in s.get("m_AnyStateTransitionConstantArray") or []:
            for cd in t["data"]["m_ConditionConstantArray"]:
                params.append((cd["data"]["m_EventID"], cd["data"]["m_ConditionMode"], float(cd["data"]["m_EventThreshold"])))
        for st in s["m_StateConstantArray"]:
            for t in st["data"]["m_TransitionConstantArray"]:
                for cd in t["data"]["m_ConditionConstantArray"]:
                    params.append((cd["data"]["m_EventID"], cd["data"]["m_ConditionMode"], float(cd["data"]["m_EventThreshold"])))
            for bt in st["data"].get("m_BlendTreeConstantArray") or []:
                for nd in bt["data"]["m_NodeArray"]:
                    nv = nd["data"]["m_BlendEventID"]
                    if nv and nv != 0xFFFFFFFF:
                        params.append((nv, 3, 0.0))
    plines = []
    for crcid, mode, thr in params:
        if crcid in seen or not crcid or crcid == 0xFFFFFFFF:
            continue
        seen.add(crcid)
        nm, _ = ctx.param_name(crcid)
        mtype = 4 if mode in (1, 2) else 1
        plines.append("  - m_Name: %s\n    m_Type: %d\n    m_DefaultFloat: 0\n    m_DefaultInt: 0\n    m_DefaultBool: 0\n" % (ystr(nm), mtype))

    # ---- controller doc
    L = doc_header(91, 9100000)
    L.append("AnimatorController:\n")
    L.append("  m_ObjectHideFlags: 0\n  m_CorrespondingSourceObject: {fileID: 0}\n")
    L.append("  m_PrefabInstance: {fileID: 0}\n  m_PrefabAsset: {fileID: 0}\n")
    L.append("  m_Name: %s\n" % ystr(d.get("m_Name", stem)))
    L.append("  serializedVersion: 5\n")
    L.append("  m_AnimatorParameters:\n" if plines else "  m_AnimatorParameters: []\n")
    for pl in plines:
        L.append(pl)
    L.append("  m_AnimatorLayers:\n" if layer_docs else "  m_AnimatorLayers: []\n")
    for ld in layer_docs:
        L.append(ld)
    docs.append("".join(L))

    outp = os.path.join(outdir, bundle, stem + ".controller")
    os.makedirs(os.path.dirname(outp), exist_ok=True)
    with open(outp, "w") as f:
        f.write("".join(docs))
    write_meta(outp + ".meta", guid_ctrl(bundle, stem), CTRL_MAIN_ID)
    ctx.stats["ctrl"] += 1

def main():
    aext, c2b, clipmap, ctrlmap = load_all()
    files = sorted(glob.glob(os.path.join(CTRLS, "*", "*.AnimatorController.json")))
    print(f"{len(files)} controllers", flush=True)
    clipreg = build_clip_registry(clipmap, c2b)
    ctx = Ctx(aext, c2b, clipmap, clipreg, {})
    # crc names need controllers loaded twice; load once here
    ctrls = [(json.load(open(f)), f) for f in files]
    ctx.crcnames = build_crc_names(ctrls, clipmap)
    for d, f in ctrls:
        convert(ctx, f, OUT)
    print("STATS:", json.dumps(ctx.stats), flush=True)
    print("CTRL DONE", flush=True)

if __name__ == "__main__":
    main()
