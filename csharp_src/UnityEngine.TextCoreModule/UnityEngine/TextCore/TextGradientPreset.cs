using System;

namespace UnityEngine.TextCore;

[Serializable]
internal class TextGradientPreset : ScriptableObject
{
	public ColorMode colorMode;

	public Color topLeft;

	public Color topRight;

	public Color bottomLeft;

	public Color bottomRight;

	public TextGradientPreset()
	{
		colorMode = ColorMode.FourCornersGradient;
		topLeft = Color.white;
		topRight = Color.white;
		bottomLeft = Color.white;
		bottomRight = Color.white;
	}

	public TextGradientPreset(Color color)
	{
		colorMode = ColorMode.FourCornersGradient;
		topLeft = color;
		topRight = color;
		bottomLeft = color;
		bottomRight = color;
	}

	public TextGradientPreset(Color color0, Color color1, Color color2, Color color3)
	{
		colorMode = ColorMode.FourCornersGradient;
		topLeft = color0;
		topRight = color1;
		bottomLeft = color2;
		bottomRight = color3;
	}
}
