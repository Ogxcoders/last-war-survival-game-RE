using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.VFX;

[UsedByNativeCode]
[NativeHeader("Modules/VFX/Public/ScriptBindings/VisualEffectAssetBindings.h")]
public struct VFXExposedProperty
{
	public string name;

	public Type type;
}
