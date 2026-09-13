using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Networking;

[NativeHeader("Modules/UnityWebRequestAudio/Public/DownloadHandlerMovieTexture.h")]
[NativeHeader("Modules/UnityWebRequestAudio/Public/DownloadHandlerAudioClip.h")]
internal static class WebRequestWWW
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("UnityWebRequestCreateAudioClip")]
	internal static extern AudioClip InternalCreateAudioClipUsingDH(DownloadHandler dh, string url, bool stream, bool compressed, AudioType audioType);
}
