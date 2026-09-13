using Unity.Collections;

namespace UnityEngine.Profiling.Experimental;

public struct DebugScreenCapture
{
	public NativeArray<byte> rawImageDataReference { get; set; }

	public TextureFormat imageFormat { get; set; }

	public int width { get; set; }

	public int height { get; set; }
}
