using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityWebRequestAudio/Public/DownloadHandlerMovieTexture.h")]
[NativeHeader("Runtime/Video/MovieTexture.h")]
[Obsolete("MovieTexture is deprecated. Use VideoPlayer instead.", false)]
public sealed class DownloadHandlerMovieTexture : DownloadHandler
{
}
