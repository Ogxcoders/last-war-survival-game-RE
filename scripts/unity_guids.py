#!/usr/bin/env python3
"""Shared deterministic GUID scheme + Unity .meta writers for the Last War pack.

GUIDs:
  anim : md5(f"LastWar.anim.{bundle}.{stem}")
  ctrl : md5(f"LastWar.ctrl.{bundle}.{stem}")
Metas are NativeFormatImporter style (as Unity 2019.4 writes for .anim/.controller).
"""
import hashlib
import os

TIME_CREATED = 17000000000000000  # fixed deterministic 100-ns ticks

def guid_anim(bundle, stem):
    return hashlib.md5(f"LastWar.anim.{bundle}.{stem}".encode()).hexdigest()

def guid_ctrl(bundle, stem):
    return hashlib.md5(f"LastWar.ctrl.{bundle}.{stem}".encode()).hexdigest()

def meta_text(guid, main_object_file_id):
    return (
        "fileFormatVersion: 2\n"
        f"guid: {guid}\n"
        "NativeFormatImporter:\n"
        "  externalObjects: {}\n"
        f"  mainObjectFileID: {main_object_file_id}\n"
        "  userData: \n"
        "  assetBundleName: \n"
        "  assetBundleVariant: \n"
    )

def write_meta(path, guid, main_object_file_id):
    with open(path, "w") as f:
        f.write(meta_text(guid, main_object_file_id))

def sanitize(n):
    return str(n).replace("/", "_").replace("\x00", "")

ANIM_MAIN_ID = 7400000
CTRL_MAIN_ID = 9100000

def add_meta_for_anim(anim_path, bundle):
    stem = os.path.basename(anim_path)[:-len(".anim")]
    write_meta(anim_path + ".meta", guid_anim(bundle, stem), ANIM_MAIN_ID)

def add_meta_for_ctrl(ctrl_path, bundle):
    stem = os.path.basename(ctrl_path)[:-len(".controller")]
    write_meta(ctrl_path + ".meta", guid_ctrl(bundle, stem), CTRL_MAIN_ID)
