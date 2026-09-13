using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngineInternal;

namespace UnityEngine;

[ExcludeFromPreset]
[NativeHeader("Modules/AssetBundle/Public/AssetBundleLoadAssetUtility.h")]
[NativeHeader("Modules/AssetBundle/Public/AssetBundleUtility.h")]
[NativeHeader("Modules/AssetBundle/Public/AssetBundleSaveAndLoadHelper.h")]
[NativeHeader("AssetBundleScriptingClasses.h")]
[NativeHeader("Runtime/Scripting/ScriptingUtility.h")]
[NativeHeader("Runtime/Scripting/ScriptingObjectWithIntPtrField.h")]
[NativeHeader("Runtime/Scripting/ScriptingExportUtility.h")]
[NativeHeader("Modules/AssetBundle/Public/AssetBundleLoadFromManagedStreamAsyncOperation.h")]
[NativeHeader("Modules/AssetBundle/Public/AssetBundleLoadFromMemoryAsyncOperation.h")]
[NativeHeader("Modules/AssetBundle/Public/AssetBundleLoadAssetOperation.h")]
[NativeHeader("Modules/AssetBundle/Public/AssetBundleLoadFromFileAsyncOperation.h")]
public class AssetBundle : Object
{
	[Obsolete("mainAsset has been made obsolete. Please use the new AssetBundle build system introduced in 5.0 and check BuildAssetBundles documentation for details.")]
	public Object mainAsset => returnMainAsset(this);

	public extern bool isStreamedSceneAssetBundle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetIsStreamedSceneAssetBundle")]
		get;
	}

	public static uint memoryBudgetKB
	{
		get
		{
			return AssetBundleLoadingCache.memoryBudgetKB;
		}
		set
		{
			AssetBundleLoadingCache.memoryBudgetKB = value;
		}
	}

	private AssetBundle()
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("LoadMainObjectFromAssetBundle", true)]
	internal static extern Object returnMainAsset(AssetBundle bundle);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("UnloadAllAssetBundles")]
	public static extern void UnloadAllAssetBundles(bool unloadAllObjects);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("GetAllAssetBundles")]
	internal static extern AssetBundle[] GetAllLoadedAssetBundles_Native();

	public static IEnumerable<AssetBundle> GetAllLoadedAssetBundles()
	{
		return GetAllLoadedAssetBundles_Native();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("LoadFromFileAsync")]
	internal static extern AssetBundleCreateRequest LoadFromFileAsync_Internal(string path, uint crc, ulong offset);

	public static AssetBundleCreateRequest LoadFromFileAsync(string path)
	{
		return LoadFromFileAsync_Internal(path, 0u, 0uL);
	}

	public static AssetBundleCreateRequest LoadFromFileAsync(string path, uint crc)
	{
		return LoadFromFileAsync_Internal(path, crc, 0uL);
	}

	public static AssetBundleCreateRequest LoadFromFileAsync(string path, uint crc, ulong offset)
	{
		return LoadFromFileAsync_Internal(path, crc, offset);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("LoadFromFile")]
	internal static extern AssetBundle LoadFromFile_Internal(string path, uint crc, ulong offset);

	public static AssetBundle LoadFromFile(string path)
	{
		return LoadFromFile_Internal(path, 0u, 0uL);
	}

	public static AssetBundle LoadFromFile(string path, uint crc)
	{
		return LoadFromFile_Internal(path, crc, 0uL);
	}

	public static AssetBundle LoadFromFile(string path, uint crc, ulong offset)
	{
		return LoadFromFile_Internal(path, crc, offset);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("LoadFromMemoryAsync")]
	internal static extern AssetBundleCreateRequest LoadFromMemoryAsync_Internal(byte[] binary, uint crc);

	public static AssetBundleCreateRequest LoadFromMemoryAsync(byte[] binary)
	{
		return LoadFromMemoryAsync_Internal(binary, 0u);
	}

	public static AssetBundleCreateRequest LoadFromMemoryAsync(byte[] binary, uint crc)
	{
		return LoadFromMemoryAsync_Internal(binary, crc);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("LoadFromMemory")]
	internal static extern AssetBundle LoadFromMemory_Internal(byte[] binary, uint crc);

	public static AssetBundle LoadFromMemory(byte[] binary)
	{
		return LoadFromMemory_Internal(binary, 0u);
	}

	public static AssetBundle LoadFromMemory(byte[] binary, uint crc)
	{
		return LoadFromMemory_Internal(binary, crc);
	}

	internal static void ValidateLoadFromStream(Stream stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("ManagedStream object must be non-null", "stream");
		}
		if (!stream.CanRead)
		{
			throw new ArgumentException("ManagedStream object must be readable (stream.CanRead must return true)", "stream");
		}
		if (!stream.CanSeek)
		{
			throw new ArgumentException("ManagedStream object must be seekable (stream.CanSeek must return true)", "stream");
		}
	}

	public static AssetBundleCreateRequest LoadFromStreamAsync(Stream stream, uint crc, uint managedReadBufferSize)
	{
		ValidateLoadFromStream(stream);
		return LoadFromStreamAsyncInternal(stream, crc, managedReadBufferSize);
	}

	public static AssetBundleCreateRequest LoadFromStreamAsync(Stream stream, uint crc)
	{
		ValidateLoadFromStream(stream);
		return LoadFromStreamAsyncInternal(stream, crc, 0u);
	}

	public static AssetBundleCreateRequest LoadFromStreamAsync(Stream stream)
	{
		ValidateLoadFromStream(stream);
		return LoadFromStreamAsyncInternal(stream, 0u, 0u);
	}

	public static AssetBundle LoadFromStream(Stream stream, uint crc, uint managedReadBufferSize)
	{
		ValidateLoadFromStream(stream);
		return LoadFromStreamInternal(stream, crc, managedReadBufferSize);
	}

	public static AssetBundle LoadFromStream(Stream stream, uint crc)
	{
		ValidateLoadFromStream(stream);
		return LoadFromStreamInternal(stream, crc, 0u);
	}

	public static AssetBundle LoadFromStream(Stream stream)
	{
		ValidateLoadFromStream(stream);
		return LoadFromStreamInternal(stream, 0u, 0u);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("LoadFromStreamAsyncInternal")]
	internal static extern AssetBundleCreateRequest LoadFromStreamAsyncInternal(Stream stream, uint crc, uint managedReadBufferSize);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("LoadFromStreamInternal")]
	internal static extern AssetBundle LoadFromStreamInternal(Stream stream, uint crc, uint managedReadBufferSize);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("Contains")]
	public extern bool Contains(string name);

	[Obsolete("Method Load has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAsset instead and check the documentation for details.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Object Load(string name)
	{
		return null;
	}

	[Obsolete("Method Load has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAsset instead and check the documentation for details.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public Object Load<T>(string name)
	{
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Method Load has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAsset instead and check the documentation for details.", true)]
	private Object Load(string name, Type type)
	{
		return null;
	}

	[Obsolete("Method LoadAsync has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAssetAsync instead and check the documentation for details.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	private AssetBundleRequest LoadAsync(string name, Type type)
	{
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Method LoadAll has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAllAssets instead and check the documentation for details.", true)]
	private Object[] LoadAll(Type type)
	{
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Method LoadAll has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAllAssets instead and check the documentation for details.", true)]
	public Object[] LoadAll()
	{
		return null;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Method LoadAll has been deprecated. Script updater cannot update it as the loading behaviour has changed. Please use LoadAllAssets instead and check the documentation for details.", true)]
	public T[] LoadAll<T>() where T : Object
	{
		return null;
	}

	public Object LoadAsset(string name)
	{
		return LoadAsset(name, typeof(Object));
	}

	public T LoadAsset<T>(string name) where T : Object
	{
		return (T)LoadAsset(name, typeof(T));
	}

	[TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
	public Object LoadAsset(string name, Type type)
	{
		if (name == null)
		{
			throw new NullReferenceException("The input asset name cannot be null.");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("The input asset name cannot be empty.");
		}
		if ((object)type == null)
		{
			throw new NullReferenceException("The input type cannot be null.");
		}
		return LoadAsset_Internal(name, type);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("LoadAsset_Internal")]
	[NativeThrows]
	[TypeInferenceRule(TypeInferenceRules.TypeReferencedBySecondArgument)]
	private extern Object LoadAsset_Internal(string name, Type type);

	public AssetBundleRequest LoadAssetAsync(string name)
	{
		return LoadAssetAsync(name, typeof(Object));
	}

	public AssetBundleRequest LoadAssetAsync<T>(string name)
	{
		return LoadAssetAsync(name, typeof(T));
	}

	public AssetBundleRequest LoadAssetAsync(string name, Type type)
	{
		if (name == null)
		{
			throw new NullReferenceException("The input asset name cannot be null.");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("The input asset name cannot be empty.");
		}
		if ((object)type == null)
		{
			throw new NullReferenceException("The input type cannot be null.");
		}
		return LoadAssetAsync_Internal(name, type);
	}

	public Object[] LoadAssetWithSubAssets(string name)
	{
		return LoadAssetWithSubAssets(name, typeof(Object));
	}

	internal static T[] ConvertObjects<T>(Object[] rawObjects) where T : Object
	{
		if (rawObjects == null)
		{
			return null;
		}
		T[] array = new T[rawObjects.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = (T)rawObjects[i];
		}
		return array;
	}

	public T[] LoadAssetWithSubAssets<T>(string name) where T : Object
	{
		return ConvertObjects<T>(LoadAssetWithSubAssets(name, typeof(T)));
	}

	public Object[] LoadAssetWithSubAssets(string name, Type type)
	{
		if (name == null)
		{
			throw new NullReferenceException("The input asset name cannot be null.");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("The input asset name cannot be empty.");
		}
		if ((object)type == null)
		{
			throw new NullReferenceException("The input type cannot be null.");
		}
		return LoadAssetWithSubAssets_Internal(name, type);
	}

	public AssetBundleRequest LoadAssetWithSubAssetsAsync(string name)
	{
		return LoadAssetWithSubAssetsAsync(name, typeof(Object));
	}

	public AssetBundleRequest LoadAssetWithSubAssetsAsync<T>(string name)
	{
		return LoadAssetWithSubAssetsAsync(name, typeof(T));
	}

	public AssetBundleRequest LoadAssetWithSubAssetsAsync(string name, Type type)
	{
		if (name == null)
		{
			throw new NullReferenceException("The input asset name cannot be null.");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("The input asset name cannot be empty.");
		}
		if ((object)type == null)
		{
			throw new NullReferenceException("The input type cannot be null.");
		}
		return LoadAssetWithSubAssetsAsync_Internal(name, type);
	}

	public Object[] LoadAllAssets()
	{
		return LoadAllAssets(typeof(Object));
	}

	public T[] LoadAllAssets<T>() where T : Object
	{
		return ConvertObjects<T>(LoadAllAssets(typeof(T)));
	}

	public Object[] LoadAllAssets(Type type)
	{
		if ((object)type == null)
		{
			throw new NullReferenceException("The input type cannot be null.");
		}
		return LoadAssetWithSubAssets_Internal("", type);
	}

	public AssetBundleRequest LoadAllAssetsAsync()
	{
		return LoadAllAssetsAsync(typeof(Object));
	}

	public AssetBundleRequest LoadAllAssetsAsync<T>()
	{
		return LoadAllAssetsAsync(typeof(T));
	}

	public AssetBundleRequest LoadAllAssetsAsync(Type type)
	{
		if ((object)type == null)
		{
			throw new NullReferenceException("The input type cannot be null.");
		}
		return LoadAssetWithSubAssetsAsync_Internal("", type);
	}

	[Obsolete("This method is deprecated.Use GetAllAssetNames() instead.", false)]
	public string[] AllAssetNames()
	{
		return GetAllAssetNames();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	[NativeMethod("LoadAssetAsync_Internal")]
	private extern AssetBundleRequest LoadAssetAsync_Internal(string name, Type type);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("Unload")]
	public extern void Unload(bool unloadAllLoadedObjects);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetAllAssetNames")]
	public extern string[] GetAllAssetNames();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("GetAllScenePaths")]
	public extern string[] GetAllScenePaths();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("LoadAssetWithSubAssets_Internal")]
	[NativeThrows]
	internal extern Object[] LoadAssetWithSubAssets_Internal(string name, Type type);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	[NativeMethod("LoadAssetWithSubAssetsAsync_Internal")]
	private extern AssetBundleRequest LoadAssetWithSubAssetsAsync_Internal(string name, Type type);

	public static AssetBundleRecompressOperation RecompressAssetBundleAsync(string inputPath, string outputPath, BuildCompression method, uint expectedCRC = 0u, ThreadPriority priority = ThreadPriority.Low)
	{
		return RecompressAssetBundleAsync_Internal(inputPath, outputPath, method, expectedCRC, priority);
	}

	[NativeThrows]
	[FreeFunction("RecompressAssetBundleAsync_Internal")]
	internal static AssetBundleRecompressOperation RecompressAssetBundleAsync_Internal(string inputPath, string outputPath, BuildCompression method, uint expectedCRC, ThreadPriority priority)
	{
		return RecompressAssetBundleAsync_Internal_Injected(inputPath, outputPath, ref method, expectedCRC, priority);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern AssetBundleRecompressOperation RecompressAssetBundleAsync_Internal_Injected(string inputPath, string outputPath, ref BuildCompression method, uint expectedCRC, ThreadPriority priority);
}
