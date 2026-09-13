using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Networking;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityWebRequest/Public/UnityWebRequestAsyncOperation.h")]
[UsedByNativeCode]
[NativeHeader("UnityWebRequestScriptingClasses.h")]
public class UnityWebRequestAsyncOperation : AsyncOperation
{
	public UnityWebRequest webRequest { get; internal set; }
}
