#!/usr/bin/env python3
"""Parse Android binary XML (AXML) without external deps."""
import struct, sys

def parse_axml(data):
    # AXML: ResChunk_header type=0x0003
    assert data[:4] == b'\x03\x00\x08\x00' or struct.unpack_from('<H', data)[0] == 3, 'not AXML'
    pos = 8
    out = []
    strings = []
    res_ids = []
    while pos < len(data):
        ctype, hsize, size = struct.unpack_from('<HHI', data, pos)
        if ctype == 0x0001:  # STRING_POOL
            strcount, stylecount, flags = struct.unpack_from('<III', data, pos + 8)
            strstart, stylestart = struct.unpack_from('<II', data, pos + 16)
            utf8 = bool(flags & (1 << 8))
            offsets = [struct.unpack_from('<I', data, pos + 28 + 4*i)[0] for i in range(strcount)]
            base = pos + strstart
            for off in offsets:
                p = base + off
                if utf8:
                    # u16len, u8len, bytes
                    if data[p] & 0x80:
                        u16len = ((data[p] & 0x7f) << 8) | data[p+1]; p += 2
                    else:
                        u16len = data[p]; p += 1
                    if data[p] & 0x80:
                        u8len = ((data[p] & 0x7f) << 8) | data[p+1]; p += 2
                    else:
                        u8len = data[p]; p += 1
                    s = data[p:p+u8len].decode('utf-8', 'replace')
                else:
                    n = data[p] | (data[p+1] << 8)
                    if n & 0x8000:
                        n2 = data[p+2] | (data[p+3] << 8)
                        n = ((n & 0x7fff) << 16) | n2
                        p += 4
                    else:
                        p += 2
                    s = data[p:p+2*n].decode('utf-16-le', 'replace')
                strings.append(s)
        elif ctype == 0x0180:  # RES_XML_RESOURCE_MAP
            n = (size - hsize) // 4
            res_ids = [struct.unpack_from('<I', data, pos + hsize + 4*i)[0] for i in range(n)]
        elif ctype == 0x0100:  # START_NAMESPACE
            line, comment = struct.unpack_from('<II', data, pos + 8)
            prefix, uri = struct.unpack_from('<iI', data, pos + 16)
            out.append(('ns', strings[prefix] if prefix >= 0 else '', strings[uri] if uri < len(strings) else '?'))
        elif ctype == 0x0101:  # END_NAMESPACE
            pass
        elif ctype == 0x0102:  # START_ELEMENT
            line, comment = struct.unpack_from('<II', data, pos + 8)
            ns, name, attrstart, attrsize, nattr = struct.unpack_from('<iIHHH', data, pos + 16)[:5]
            # attribute start offsets: header(16) + ns(4)+name(4)+flags(2)+attrstart... per spec:
            base_attr = pos + 16 + 20  # ns(4) name(4) attrstart(2) attrsize(2) nattr(2)? careful
            # layout: header(8) line(4) comment(4) ns(4) name(4) attrStart(2) attrSize(2) attrCount(2) idIdx(2) classIdx(2) styleIdx(2) = 36 header
            base_attr = pos + 36
            attrs = []
            for i in range(nattr):
                a_ns, a_name, a_raw, a_size = struct.unpack_from('<iIiH', data, base_attr + i * attrsize)
                a_type = data[base_attr + i * attrsize + 15]
                a_data = struct.unpack_from('<I', data, base_attr + i * attrsize + 16)[0]
                nm = strings[a_name] if 0 <= a_name < len(strings) else '?'
                if a_type == 0x03:  val = strings[a_data] if a_data < len(strings) else f'@str{a_data}'
                elif a_type == 0x02: val = f'?{a_data:08x}'
                elif a_type == 0x10: val = a_data  # int
                elif a_type == 0x12: val = bool(a_data)
                elif a_type == 0x01: val = f'@ref{a_data:08x}'
                elif a_type == 0x04: val = struct.unpack('<f', struct.pack('<I', a_data))[0]
                elif a_type == 0x11: val = f'0x{a_data:08x}'
                else: val = f't{a_type:02x}:{a_data}'
                attrs.append((nm, val))
            out.append(('elem', strings[name] if 0 <= name < len(strings) else '?', attrs))
        elif ctype == 0x0103:  # END_ELEMENT
            ns, name = struct.unpack_from('<iI', data, pos + 16)
            out.append(('end', strings[name] if 0 <= name < len(strings) else '?'))
        pos += size
    return out, res_ids

if __name__ == '__main__':
    data = open(sys.argv[1], 'rb').read()
    out, res_ids = parse_axml(data)
    depth = 0
    for item in out:
        if item[0] == 'ns':
            print(f'{item[2]}')
        elif item[0] == 'elem':
            name, attrs = item[1], item[2]
            print('  ' * depth + f'<{name}>')
            for k, v in attrs:
                print('  ' * (depth + 1) + f'{k} = {v}')
            depth += 1
        elif item[0] == 'end':
            depth = max(0, depth - 1)
