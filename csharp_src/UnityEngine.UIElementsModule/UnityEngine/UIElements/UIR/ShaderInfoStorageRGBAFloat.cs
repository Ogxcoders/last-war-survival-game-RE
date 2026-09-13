using System;

namespace UnityEngine.UIElements.UIR;

internal class ShaderInfoStorageRGBAFloat : ShaderInfoStorage<Color>
{
	private static readonly Func<Color, Color> s_Convert = (Color c) => c;

	public ShaderInfoStorageRGBAFloat(int initialSize = 64, int maxSize = 4096)
		: base(TextureFormat.RGBAFloat, s_Convert, initialSize, maxSize)
	{
	}
}
