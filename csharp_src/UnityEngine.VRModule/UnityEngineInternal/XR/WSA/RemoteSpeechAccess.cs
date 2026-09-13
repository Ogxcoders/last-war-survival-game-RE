using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.XR.WSA;

namespace UnityEngineInternal.XR.WSA;

[NativeConditional("ENABLE_HOLOLENS_MODULE")]
[NativeHeader("Modules/VR/HoloLens/PerceptionRemoting.h")]
public class RemoteSpeechAccess
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Support for built-in VR will be removed in Unity 2020.1. Please update to the new Unity XR Plugin System. More information about the new XR Plugin System can be found at https://docs.unity3d.com/2019.3/Documentation/Manual/XR.html.", false)]
	public static extern void EnableRemoteSpeech(RemoteDeviceVersion remoteDeviceVersion);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[Obsolete("Support for built-in VR will be removed in Unity 2020.1. Please update to the new Unity XR Plugin System. More information about the new XR Plugin System can be found at https://docs.unity3d.com/2019.3/Documentation/Manual/XR.html.", false)]
	public static extern void DisableRemoteSpeech();
}
