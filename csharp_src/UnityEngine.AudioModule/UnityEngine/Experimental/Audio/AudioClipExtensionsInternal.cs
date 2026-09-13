using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.Audio;

[NativeHeader("Modules/Audio/Public/ScriptBindings/AudioClipExtensions.bindings.h")]
[NativeHeader("AudioScriptingClasses.h")]
[NativeHeader("Modules/Audio/Public/AudioClip.h")]
internal static class AudioClipExtensionsInternal
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsFreeFunction = true, ThrowsException = true)]
	public static extern uint Internal_CreateAudioClipSampleProvider(this AudioClip audioClip, ulong start, long end, bool loop, bool allowDrop);
}
