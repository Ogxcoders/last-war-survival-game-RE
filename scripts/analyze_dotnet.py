#!/usr/bin/env python3
""".NET assembly analyzer: dump types/methods/fields/enums/literals per assembly."""
import os, sys, json
import dnfile

DLL_DIR = '/home/z/my-project/apk_analysis/re/dlls'
OUT = '/home/z/my-project/apk_analysis/re/csharp_meta'
os.makedirs(OUT, exist_ok=True)

def analyze(path, outdir):
    name = os.path.basename(path)[:-4]
    pe = dnfile.dnPE(path)
    md = pe.net.mdtables
    trows = md.TypeDef.rows if md.TypeDef else []
    nmethods = len(md.MethodDef.rows) if md.MethodDef else 0
    nfields = len(md.Field.rows) if md.Field else 0

    # method/field ranges per typedef
    def list_start(lst, total):
        try:
            return lst[0].row_index - 1 if lst else total
        except Exception:
            return total
    mstarts, fstarts = [], []
    for t in trows:
        mstarts.append(list_start(t.MethodList, nmethods))
        fstarts.append(list_start(t.FieldList, nfields))
    # fix empty-list cases: start should be next type's start (walk backward)
    for arr, total in ((mstarts, nmethods), (fstarts, nfields)):
        nxt = total
        for i in range(len(arr) - 1, -1, -1):
            if arr[i] >= total:
                arr[i] = nxt
            else:
                nxt = arr[i]
    mranges, franges = mstarts, fstarts
    mranges.append(nmethods); franges.append(nfields)

    lines = []
    ntypes = nmeth = nfields_c = 0
    for i, t in enumerate(trows):
        ns = str(t.TypeNamespace or '')
        tn = str(t.TypeName or '')
        full = f'{ns}.{tn}' if ns else tn
        lines.append(f'\n===== {full} =====')
        try:
            if t.Extends and t.Extends.row:
                ext = t.Extends.row
                en = str(getattr(ext, "TypeName", "") or "")
                ens = str(getattr(ext, "TypeNamespace", "") or "")
                lines.append(f'  : {ens + "." + en if ens else en}')
        except Exception:
            pass
        try:
            if len(t.interface_list) if hasattr(t, "interface_list") else 0:
                pass
        except Exception:
            pass
        # fields
        for fri in range(franges[i], min(franges[i+1], nfields)):
            fr = md.Field.rows[fri]
            try: st = 'static ' if fr.Flags.Static else ''
            except Exception: st = ''
            lines.append(f'  field  {st}{fr.Name}')
            nfields_c += 1
        # enum values: field names of enums are the constants
        # methods
        for mri in range(mranges[i], min(mranges[i+1], nmethods)):
            mr = md.MethodDef.rows[mri]
            try:
                st = ('static ' if mr.Flags.Static else '') + ('public ' if mr.Flags.Public else '')
            except Exception:
                st = ''
            pn = []
            try:
                pn = [p.Name for p in mr.ParamList]
            except Exception:
                pass
            lines.append(f'  method {st}{mr.Name}({", ".join(map(str, pn))})')
            nmeth += 1
        ntypes += 1

    with open(os.path.join(outdir, f'{name}.types.txt'), 'w', encoding='utf-8') as f:
        f.write('\n'.join(lines))
    return name, {'types': ntypes, 'methods': nmeth, 'fields': nfields_c}

if __name__ == '__main__':
    dlls = sorted(f for f in os.listdir(DLL_DIR) if f.endswith('.dll'))
    print(f'{len(dlls)} assemblies', flush=True)
    results = {}
    for d in dlls:
        try:
            n, st = analyze(os.path.join(DLL_DIR, d), OUT)
        except Exception as e:
            n, st = d[:-4], {'error': str(e)[:120]}
        results[n] = st
        print(n, st, flush=True)
    json.dump(results, open(os.path.join(OUT, '_summary.json'), 'w'), indent=1)
