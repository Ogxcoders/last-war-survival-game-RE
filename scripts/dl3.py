#!/usr/bin/env python3
"""Block-based resumable downloader: 1MB blocks, 10 workers, state file."""
import os, sys, time, threading, queue, urllib.request, urllib.error, hashlib

URL = sys.argv[1] if len(sys.argv) > 1 else "https://gp4.liteapks.com/Last%20War:%20Survival%20Game/Last%20War:%20Survival%20Game-1.0.351.xapk"
DIR = "/home/z/my-project/apk_analysis"
OUT = sys.argv[2] if len(sys.argv) > 2 else os.path.join(DIR, "LastWar-1.0.351-mod.xapk")
TOTAL = int(sys.argv[3]) if len(sys.argv) > 3 else 817739360
UA = sys.argv[4] if len(sys.argv) > 4 else "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36"
REF = sys.argv[5] if len(sys.argv) > 5 else "https://liteapks.com/last-warsurvival-game.html"
BLOCK = 1 << 20
NBLOCKS = (TOTAL + BLOCK - 1) // BLOCK
WORKERS = int(sys.argv[6]) if len(sys.argv) > 6 else 10
STATE = OUT + ".state"

# init output file sparse
if not os.path.exists(OUT) or os.path.getsize(OUT) != TOTAL:
    with open(OUT, "wb") as f:
        f.truncate(TOTAL)

# load state (sorted list of done block ids)
done = set()
if os.path.exists(STATE):
    with open(STATE) as f:
        done = set(int(x) for x in f.read().split())
todo = queue.Queue()
for b in range(NBLOCKS):
    if b not in done:
        todo.put(b)
print(f"[dl3] {NBLOCKS} blocks, {len(done)} done, {todo.qsize()} to go", flush=True)

lock = threading.Lock()
statef = open(STATE, "a")
errs = 0

def worker():
    global errs
    out = open(OUT, "r+b", buffering=0)
    while True:
        try:
            b = todo.get_nowait()
        except queue.Empty:
            return
        start = b * BLOCK
        end = min(TOTAL, start + BLOCK) - 1
        want = end - start + 1
        ok = False
        for attempt in range(12):
            req = urllib.request.Request(URL, headers={
                "User-Agent": UA, "Referer": REF, "Range": f"bytes={start}-{end}"})
            try:
                with urllib.request.urlopen(req, timeout=45) as r:
                    data = r.read()
                if len(data) == want:
                    out.seek(start)
                    out.write(data)
                    with lock:
                        statef.write(f"{b}\n")
                        statef.flush()
                        done.add(b)
                    ok = True
                    break
                else:
                    errs += 1
            except Exception:
                errs += 1
                time.sleep(min(30, 1.5 * attempt))
        if not ok:
            todo.put(b)  # requeue for later
            time.sleep(2)

threads = [threading.Thread(target=worker, daemon=True) for _ in range(WORKERS)]
for t in threads: t.start()
last = -1
while any(t.is_alive() for t in threads):
    time.sleep(15)
    with lock:
        n = len(done)
    if n != last:
        print(f"progress {n*100//NBLOCKS}% ({n}/{NBLOCKS} blocks, {n*BLOCK//1048576} MB) errs={errs}", flush=True)
        last = n
for t in threads: t.join()
with lock:
    n = len(done)
if n == NBLOCKS:
    h = hashlib.md5()
    with open(OUT, "rb") as f:
        while True:
            c = f.read(1 << 24)
            if not c: break
            h.update(c)
    print(f"DOWNLOAD COMPLETE md5={h.hexdigest()}", flush=True)
    os.remove(STATE)
else:
    print(f"INCOMPLETE {n}/{NBLOCKS}", flush=True)
    sys.exit(1)
