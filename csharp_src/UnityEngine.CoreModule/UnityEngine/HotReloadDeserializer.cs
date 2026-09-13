using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeConditional("HOT_RELOAD_AVAILABLE")]
[NativeType(Header = "Runtime/Export/HotReload/HotReload.bindings.h")]
internal static class HotReloadDeserializer
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("HotReload::Prepare")]
	internal static extern void PrepareHotReload();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("HotReload::Finish")]
	internal static extern void FinishHotReload(Type[] typesToReset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	[FreeFunction("HotReload::CreateEmptyAsset")]
	internal static extern Object CreateEmptyAsset(Type type);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("HotReload::DeserializeAsset")]
	[NativeThrows]
	internal static extern void DeserializeAsset(Object asset, byte[] data);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	[FreeFunction("HotReload::RemapInstanceIds")]
	private static extern void RemapInstanceIds(Object editorAsset, int[] editorToPlayerInstanceIdMapKeys, int[] editorToPlayerInstanceIdMapValues);

	internal static void RemapInstanceIds(Object editorAsset, Dictionary<int, int> editorToPlayerInstanceIdMap)
	{
		RemapInstanceIds(editorAsset, editorToPlayerInstanceIdMap.Keys.ToArray(), editorToPlayerInstanceIdMap.Values.ToArray());
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("HotReload::FinalizeAssetCreation")]
	internal static extern void FinalizeAssetCreation(Object asset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("HotReload::GetDependencies")]
	internal static extern Object[] GetDependencies(Object asset);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("HotReload::GetNullDependencies")]
	internal static extern int[] GetNullDependencies(Object asset);
}
