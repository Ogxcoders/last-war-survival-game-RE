using System;
using System.ComponentModel;
using UnityEngine.Internal;

namespace UnityEngine;

[ExcludeFromPreset]
[ExcludeFromObjectFactory]
[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("GUIElement has been removed.", true)]
public sealed class GUIElement
{
	private static void FeatureRemoved()
	{
		throw new Exception("GUIElement has been removed from Unity.");
	}

	[Obsolete("GUIElement has been removed.", true)]
	public bool HitTest(Vector3 screenPosition)
	{
		FeatureRemoved();
		return false;
	}

	[Obsolete("GUIElement has been removed.", true)]
	public bool HitTest(Vector3 screenPosition, [UnityEngine.Internal.DefaultValue("null")] Camera camera)
	{
		FeatureRemoved();
		return false;
	}

	[Obsolete("GUIElement has been removed.", true)]
	public Rect GetScreenRect([UnityEngine.Internal.DefaultValue("null")] Camera camera)
	{
		FeatureRemoved();
		return new Rect(0f, 0f, 0f, 0f);
	}

	[Obsolete("GUIElement has been removed.", true)]
	public Rect GetScreenRect()
	{
		FeatureRemoved();
		return new Rect(0f, 0f, 0f, 0f);
	}
}
