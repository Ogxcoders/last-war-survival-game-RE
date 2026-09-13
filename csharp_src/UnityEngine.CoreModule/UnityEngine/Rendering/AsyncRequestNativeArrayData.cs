using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering;

[NativeHeader("Runtime/Graphics/AsyncGPUReadbackManaged.h")]
[UsedByNativeCode]
internal struct AsyncRequestNativeArrayData
{
	public unsafe void* nativeArrayBuffer;

	public long lengthInBytes;
}
