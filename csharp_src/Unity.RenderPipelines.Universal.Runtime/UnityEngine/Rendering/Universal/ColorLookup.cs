using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal;

[Serializable]
[VolumeComponentMenu("Post-processing/Color Lookup")]
public sealed class ColorLookup : VolumeComponent, IPostProcessComponent
{
	[Tooltip("A custom 2D texture lookup table to apply.")]
	public TextureParameter texture = new TextureParameter(null);

	[Tooltip("How much of the lookup texture will contribute to the color grading effect.")]
	public ClampedFloatParameter contribution = new ClampedFloatParameter(1f, 0f, 1f);

	public bool IsActive()
	{
		if (contribution.value > 0f)
		{
			return ValidateLUT();
		}
		return false;
	}

	public bool IsTileCompatible()
	{
		return true;
	}

	public bool ValidateLUT()
	{
		UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
		if (asset == null || texture.value == null)
		{
			return false;
		}
		int colorGradingLutSize = asset.colorGradingLutSize;
		if (texture.value.height != colorGradingLutSize)
		{
			return false;
		}
		bool flag = false;
		Texture value = texture.value;
		if ((object)value != null)
		{
			if (!(value is Texture2D texture2D))
			{
				if (value is RenderTexture renderTexture)
				{
					RenderTexture renderTexture2 = renderTexture;
					flag |= renderTexture2.dimension == TextureDimension.Tex2D && renderTexture2.width == colorGradingLutSize * colorGradingLutSize && !renderTexture2.sRGB;
				}
			}
			else
			{
				Texture2D texture2D2 = texture2D;
				flag |= texture2D2.width == colorGradingLutSize * colorGradingLutSize && !GraphicsFormatUtility.IsSRGBFormat(texture2D2.graphicsFormat);
			}
		}
		return flag;
	}
}
