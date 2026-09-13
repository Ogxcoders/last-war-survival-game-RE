using System;

namespace UnityEngine.UIElements.UIR;

internal class ShaderInfoStorageRGBA32 : ShaderInfoStorage<Color32>
{
	private static readonly Func<Color, Color32> s_Convert = (Color c) => c;

	public ShaderInfoStorageRGBA32(int initialSize = 64, int maxSize = 4096)
		: base(TextureFormat.RGBA32, s_Convert, initialSize, maxSize)
	{
	}
}
