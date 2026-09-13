using System;
using System.ComponentModel;

namespace UnityEngine;

[ExcludeFromPreset]
[EditorBrowsable(EditorBrowsableState.Never)]
[ExcludeFromObjectFactory]
[Obsolete("GUITexture has been removed. Use UI.Image instead.", true)]
public sealed class GUITexture
{
	[Obsolete("GUITexture has been removed. Use UI.Image instead.", true)]
	public Color color
	{
		get
		{
			FeatureRemoved();
			return new Color(0f, 0f, 0f);
		}
		set
		{
			FeatureRemoved();
		}
	}

	[Obsolete("GUITexture has been removed. Use UI.Image instead.", true)]
	public Texture texture
	{
		get
		{
			FeatureRemoved();
			return null;
		}
		set
		{
			FeatureRemoved();
		}
	}

	[Obsolete("GUITexture has been removed. Use UI.Image instead.", true)]
	public Rect pixelInset
	{
		get
		{
			FeatureRemoved();
			return default(Rect);
		}
		set
		{
			FeatureRemoved();
		}
	}

	[Obsolete("GUITexture has been removed. Use UI.Image instead.", true)]
	public RectOffset border
	{
		get
		{
			FeatureRemoved();
			return null;
		}
		set
		{
			FeatureRemoved();
		}
	}

	private static void FeatureRemoved()
	{
		throw new Exception("GUITexture has been removed from Unity. Use UI.Image instead.");
	}
}
