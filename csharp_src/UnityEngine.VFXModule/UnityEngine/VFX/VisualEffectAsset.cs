using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine.VFX;

[NativeHeader("Modules/VFX/Public/ScriptBindings/VisualEffectAssetBindings.h")]
[UsedByNativeCode]
[NativeHeader("Modules/VFX/Public/VisualEffectAsset.h")]
[NativeHeader("VFXScriptingClasses.h")]
public class VisualEffectAsset : VisualEffectObject
{
	public const string PlayEventName = "OnPlay";

	public const string StopEventName = "OnStop";

	public static readonly int PlayEventID = Shader.PropertyToID("OnPlay");

	public static readonly int StopEventID = Shader.PropertyToID("OnStop");

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectAssetBindings::GetTextureDimension", HasExplicitThis = true)]
	public extern TextureDimension GetTextureDimension(int nameID);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectAssetBindings::GetExposedProperties", HasExplicitThis = true)]
	public extern void GetExposedProperties([NotNull] List<VFXExposedProperty> exposedProperties);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction(Name = "VisualEffectAssetBindings::GetEvents", HasExplicitThis = true)]
	public extern void GetEvents([NotNull] List<string> names);

	public TextureDimension GetTextureDimension(string name)
	{
		return GetTextureDimension(Shader.PropertyToID(name));
	}
}
