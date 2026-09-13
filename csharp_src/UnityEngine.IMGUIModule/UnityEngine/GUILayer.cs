using System;
using System.ComponentModel;

namespace UnityEngine;

[ExcludeFromPreset]
[Obsolete("GUILayer has been removed.", true)]
[ExcludeFromObjectFactory]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class GUILayer
{
	[Obsolete("GUILayer has been removed.", true)]
	public GUIElement HitTest(Vector3 screenPosition)
	{
		throw new Exception("GUILayer has been removed from Unity.");
	}
}
