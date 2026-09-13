using System;
using System.ComponentModel;

namespace UnityEngine;

[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
[ExcludeFromPreset]
[ExcludeFromObjectFactory]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class GUIText
{
	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
	public bool text
	{
		get
		{
			FeatureRemoved();
			return false;
		}
		set
		{
			FeatureRemoved();
		}
	}

	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
	public Material material
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

	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
	public Font font
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

	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
	public TextAlignment alignment
	{
		get
		{
			FeatureRemoved();
			return TextAlignment.Left;
		}
		set
		{
			FeatureRemoved();
		}
	}

	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
	public TextAnchor anchor
	{
		get
		{
			FeatureRemoved();
			return TextAnchor.UpperLeft;
		}
		set
		{
			FeatureRemoved();
		}
	}

	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
	public float lineSpacing
	{
		get
		{
			FeatureRemoved();
			return 0f;
		}
		set
		{
			FeatureRemoved();
		}
	}

	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
	public float tabSize
	{
		get
		{
			FeatureRemoved();
			return 0f;
		}
		set
		{
			FeatureRemoved();
		}
	}

	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
	public int fontSize
	{
		get
		{
			FeatureRemoved();
			return 0;
		}
		set
		{
			FeatureRemoved();
		}
	}

	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
	public FontStyle fontStyle
	{
		get
		{
			FeatureRemoved();
			return FontStyle.Normal;
		}
		set
		{
			FeatureRemoved();
		}
	}

	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
	public bool richText
	{
		get
		{
			FeatureRemoved();
			return false;
		}
		set
		{
			FeatureRemoved();
		}
	}

	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
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

	[Obsolete("GUIText has been removed. Use UI.Text instead.", true)]
	public Vector2 pixelOffset
	{
		get
		{
			FeatureRemoved();
			return new Vector2(0f, 0f);
		}
		set
		{
			FeatureRemoved();
		}
	}

	private static void FeatureRemoved()
	{
		throw new Exception("GUIText has been removed from Unity. Use UI.Text instead.");
	}
}
