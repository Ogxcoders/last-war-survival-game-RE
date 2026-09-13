using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Android;

[StaticAccessor("AndroidAssetPacksBindingsHelpers", StaticAccessorType.DoubleColon)]
[NativeHeader("Modules/AndroidJNI/Public/AndroidAssetPacksBindingsHelpers.h")]
public static class AndroidAssetPacks
{
	private class AssetPackManagerDownloadStatusCallback : AndroidJavaProxy
	{
		private Action<AndroidAssetPackInfo> m_Callback;

		private string[] m_AssetPacks;

		public AssetPackManagerDownloadStatusCallback(Action<AndroidAssetPackInfo> callback, string[] assetPacks)
			: base("com.unity3d.player.IAssetPackManagerDownloadStatusCallback")
		{
			m_Callback = callback;
			m_AssetPacks = assetPacks;
		}

		[Preserve]
		private void onStatusUpdate(string assetPackName, int assetPackStatus, long assetPackSize, long assetPackBytesDownloaded, int assetPackTransferProgress, int assetPackErrorCode)
		{
			if (m_AssetPacks.Contains(assetPackName))
			{
				AndroidAssetPackInfo obj = new AndroidAssetPackInfo(assetPackName, (AndroidAssetPackStatus)assetPackStatus, (ulong)assetPackSize, (ulong)assetPackBytesDownloaded, (float)assetPackTransferProgress / 100f, (AndroidAssetPackError)assetPackErrorCode);
				m_Callback?.Invoke(obj);
			}
		}
	}

	private class AssetPackManagerMobileDataConfirmationCallback : AndroidJavaProxy
	{
		private Action<AndroidAssetPackUseMobileDataRequestResult> m_Callback;

		public AssetPackManagerMobileDataConfirmationCallback(Action<AndroidAssetPackUseMobileDataRequestResult> callback)
			: base("com.unity3d.player.IAssetPackManagerMobileDataConfirmationCallback")
		{
			m_Callback = callback;
		}

		[Preserve]
		private void onMobileDataConfirmationResult(bool allowed)
		{
			m_Callback?.Invoke(new AndroidAssetPackUseMobileDataRequestResult(allowed));
		}
	}

	private class AssetPackManagerStatusQueryCallback : AndroidJavaProxy
	{
		private Action<ulong, AndroidAssetPackState[]> m_Callback;

		private List<string> m_AssetPackNames;

		private List<AndroidAssetPackState> m_States;

		private long m_Size;

		public AssetPackManagerStatusQueryCallback(Action<ulong, AndroidAssetPackState[]> callback, string[] assetPacks)
			: base("com.unity3d.player.IAssetPackManagerStatusQueryCallback")
		{
			m_Callback = callback;
			m_AssetPackNames = assetPacks.ToList();
			m_States = new List<AndroidAssetPackState>();
			m_Size = 0L;
		}

		[Preserve]
		private void onStatusResult(long totalBytes, string[] assetPackNames, int[] assetPackStatuses, int[] assetPackErrorCodes)
		{
			if (m_Callback != null)
			{
				int num = assetPackNames.Length;
				bool flag = num == m_AssetPackNames.Count;
				for (int i = 0; i < num; i++)
				{
					m_States.Add(new AndroidAssetPackState(assetPackNames[i], (AndroidAssetPackStatus)assetPackStatuses[i], (AndroidAssetPackError)assetPackErrorCodes[i]));
					m_AssetPackNames.Remove(assetPackNames[i]);
				}
				m_Size += totalBytes;
				if (flag)
				{
					m_Callback((ulong)m_Size, m_States.ToArray());
					return;
				}
				GetAssetPackManager().Call("getAssetPackStates", m_AssetPackNames.ToArray(), this);
			}
		}
	}

	private static AndroidJavaObject s_JavaPlayAssetDeliveryWrapper;

	private static bool s_ApiMissing;

	public static bool coreUnityAssetPacksDownloaded => CoreUnityAssetPacksDownloaded();

	internal static string dataPackName => GetDataPackName();

	internal static string streamingAssetsPackName => GetStreamingAssetsPackName();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("PLATFORM_ANDROID")]
	private static extern bool CoreUnityAssetPacksDownloaded();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern string GetDataPackName();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern string GetStreamingAssetsPackName();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern string[] GetCoreUnityAssetPackNames();

	public static void GetAssetPackStateAsync(string[] assetPackNames, Action<ulong, AndroidAssetPackState[]> callback)
	{
		GetAssetPackManager().Call("getAssetPackStates", assetPackNames, new AssetPackManagerStatusQueryCallback(callback, assetPackNames));
	}

	public static GetAssetPackStateAsyncOperation GetAssetPackStateAsync(string[] assetPackNames)
	{
		if (assetPackNames == null || assetPackNames.Length == 0)
		{
			return null;
		}
		GetAssetPackStateAsyncOperation getAssetPackStateAsyncOperation = new GetAssetPackStateAsyncOperation();
		GetAssetPackStateAsync(assetPackNames, getAssetPackStateAsyncOperation.OnResult);
		return getAssetPackStateAsyncOperation;
	}

	public static void DownloadAssetPackAsync(string[] assetPackNames, Action<AndroidAssetPackInfo> callback)
	{
		GetAssetPackManager().Call("downloadAssetPacks", assetPackNames, new AssetPackManagerDownloadStatusCallback(callback, assetPackNames));
	}

	public static DownloadAssetPackAsyncOperation DownloadAssetPackAsync(string[] assetPackNames)
	{
		if (assetPackNames == null || assetPackNames.Length == 0)
		{
			return null;
		}
		DownloadAssetPackAsyncOperation downloadAssetPackAsyncOperation = new DownloadAssetPackAsyncOperation(assetPackNames);
		DownloadAssetPackAsync(assetPackNames, downloadAssetPackAsyncOperation.OnUpdate);
		return downloadAssetPackAsyncOperation;
	}

	public static void RequestToUseMobileDataAsync(Action<AndroidAssetPackUseMobileDataRequestResult> callback)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		GetAssetPackManager().Call("requestToUseMobileData", androidJavaObject, new AssetPackManagerMobileDataConfirmationCallback(callback));
	}

	public static RequestToUseMobileDataAsyncOperation RequestToUseMobileDataAsync()
	{
		RequestToUseMobileDataAsyncOperation requestToUseMobileDataAsyncOperation = new RequestToUseMobileDataAsyncOperation();
		RequestToUseMobileDataAsync(requestToUseMobileDataAsyncOperation.OnResult);
		return requestToUseMobileDataAsyncOperation;
	}

	public static string GetAssetPackPath(string assetPackName)
	{
		return GetAssetPackManager().Call<string>("getAssetPackPath", new object[1] { assetPackName });
	}

	public static void CancelAssetPackDownload(string[] assetPackNames)
	{
		GetAssetPackManager().Call("cancelAssetPackDownloads", assetPackNames);
	}

	public static void RemoveAssetPack(string assetPackName)
	{
		if (assetPackName == dataPackName || assetPackName == streamingAssetsPackName)
		{
			Debug.LogWarning("Can't remove core Unity asset pack '" + assetPackName + "'");
			return;
		}
		GetAssetPackManager().Call("removeAssetPack", assetPackName);
	}

	private static AndroidJavaObject GetAssetPackManager()
	{
		if (s_JavaPlayAssetDeliveryWrapper == null)
		{
			using AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.PlayAssetDeliveryUnityWrapper");
			s_JavaPlayAssetDeliveryWrapper = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
			s_ApiMissing = s_JavaPlayAssetDeliveryWrapper.Call<bool>("playCoreApiMissing", new object[0]);
		}
		if (s_ApiMissing)
		{
			throw new InvalidOperationException("PlayCore AssetPackManager API is not available! Make sure your gradle project includes \"com.google.android.play:core\" dependency.");
		}
		return s_JavaPlayAssetDeliveryWrapper;
	}
}
