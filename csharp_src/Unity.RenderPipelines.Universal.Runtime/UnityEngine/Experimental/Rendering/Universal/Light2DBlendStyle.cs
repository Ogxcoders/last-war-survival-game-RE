using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Experimental.Rendering.Universal;

[Serializable]
[MovedFrom("UnityEngine.Experimental.Rendering.LWRP")]
public struct Light2DBlendStyle
{
	internal enum TextureChannel
	{
		None,
		R,
		G,
		B,
		A,
		OneMinusR,
		OneMinusG,
		OneMinusB,
		OneMinusA
	}

	internal struct MaskChannelFilter
	{
		public Vector4 mask { get; private set; }

		public Vector4 inverted { get; private set; }

		public MaskChannelFilter(Vector4 m, Vector4 i)
		{
			mask = m;
			inverted = i;
		}
	}

	internal enum BlendMode
	{
		Additive = 0,
		Multiply = 1,
		Subtractive = 2,
		Custom = 99
	}

	[Serializable]
	internal struct BlendFactors
	{
		public float multiplicative;

		public float additive;
	}

	[Obsolete("This field is obsolete. Blend Styles are now automatically enabled/disabled.")]
	public bool enabled;

	public string name;

	[SerializeField]
	internal TextureChannel maskTextureChannel;

	[SerializeField]
	[Range(0.01f, 1f)]
	internal float renderTextureScale;

	[SerializeField]
	internal BlendMode blendMode;

	[SerializeField]
	internal BlendFactors customBlendFactors;

	internal Vector2 blendFactors
	{
		get
		{
			Vector2 result = default(Vector2);
			switch (blendMode)
			{
			case BlendMode.Additive:
				result.x = 0f;
				result.y = 1f;
				break;
			case BlendMode.Multiply:
				result.x = 1f;
				result.y = 0f;
				break;
			case BlendMode.Subtractive:
				result.x = 0f;
				result.y = -1f;
				break;
			case BlendMode.Custom:
				result.x = customBlendFactors.multiplicative;
				result.y = customBlendFactors.additive;
				break;
			default:
				return Vector2.zero;
			}
			return result;
		}
	}

	internal MaskChannelFilter maskTextureChannelFilter => maskTextureChannel switch
	{
		TextureChannel.R => new MaskChannelFilter(new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, 0f, 0f, 0f)), 
		TextureChannel.OneMinusR => new MaskChannelFilter(new Vector4(1f, 0f, 0f, 0f), new Vector4(1f, 0f, 0f, 0f)), 
		TextureChannel.G => new MaskChannelFilter(new Vector4(0f, 1f, 0f, 0f), new Vector4(0f, 0f, 0f, 0f)), 
		TextureChannel.OneMinusG => new MaskChannelFilter(new Vector4(0f, 1f, 0f, 0f), new Vector4(0f, 1f, 0f, 0f)), 
		TextureChannel.B => new MaskChannelFilter(new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 0f, 0f)), 
		TextureChannel.OneMinusB => new MaskChannelFilter(new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 1f, 0f)), 
		TextureChannel.A => new MaskChannelFilter(new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 0f)), 
		TextureChannel.OneMinusA => new MaskChannelFilter(new Vector4(0f, 0f, 0f, 1f), new Vector4(0f, 0f, 0f, 1f)), 
		_ => new MaskChannelFilter(Vector4.zero, Vector4.zero), 
	};
}
