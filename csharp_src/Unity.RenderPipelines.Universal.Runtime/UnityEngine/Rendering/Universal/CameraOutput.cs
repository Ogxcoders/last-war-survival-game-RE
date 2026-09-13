using System;
using System.ComponentModel;

namespace UnityEngine.Rendering.Universal;

[Obsolete("This enum is deprecated.")]
[EditorBrowsable(EditorBrowsableState.Never)]
public enum CameraOutput
{
	Screen = 0,
	Texture = 1,
	[Obsolete("Use CameraOutput.Screen instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	Camera = 0
}
