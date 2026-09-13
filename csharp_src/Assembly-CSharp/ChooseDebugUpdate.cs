using System;
using GameFramework;
using UnityEngine;
using UnityEngine.Networking;
using VEngine;

public class ChooseDebugUpdate
{
	private UIChooseLocalUpdate _uiChooseLocalUpdate;

	private UnityWebRequestAsyncOperation _tableRequest;

	private UnityWebRequestAsyncOperation _buildRequest;

	private bool _localUpdateConfigSet;

	public static string selectedTableEnv = string.Empty;

	public static string selectedBundleBuildId = string.Empty;

	public static string selectedLuaBuildId = string.Empty;

	public static bool selectedLocaleDev = false;

	public static bool SkipUpdate = true;

	public static bool inDevEnv = true;

	private Action _completedCallback;

	private AssetBundle _bundle;

	private GameObject LoadUI()
	{
		string uri = Application.streamingAssetsPath + "/LocalUpdateUI/localupdateui.bundle";
		string name = "UIChooseLocalUpdate.prefab";
		using UnityWebRequest unityWebRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri);
		unityWebRequest.SendWebRequest();
		while (!unityWebRequest.isDone)
		{
		}
		_bundle = DownloadHandlerAssetBundle.GetContent(unityWebRequest);
		return _bundle.LoadAsset<GameObject>(name);
	}

	public void Enter(Action completedCallback)
	{
		Updater.AddUpdateCallback(OnUpdate);
		_completedCallback = completedCallback;
		Versions.UseBundlePackageID = true;
		_localUpdateConfigSet = false;
		GameEntry.Event.Subscribe(EventId.LOCAL_UPDATE_CONFIG_SET, OnLocalUpdateConfigSet);
		OnPrefabInstComplete(LoadUI());
	}

	public void Exit()
	{
		if (_bundle != null)
		{
			_bundle.Unload(unloadAllLoadedObjects: true);
			_bundle = null;
		}
		Updater.RemoveUpdateCallback(OnUpdate);
		GameEntry.Event.Unsubscribe(EventId.LOCAL_UPDATE_CONFIG_SET, OnLocalUpdateConfigSet);
		_completedCallback?.Invoke();
	}

	private void OnLocalUpdateConfigSet(object obj)
	{
		_localUpdateConfigSet = true;
		inDevEnv = (bool)obj;
		if (!inDevEnv && NetworkURLConfig.URLGroupType == URLGroupType.Local)
		{
			NetworkURLConfig.SetURLGroupEnv("PressureTest");
		}
	}

	private void OnPrefabInstComplete(UnityEngine.Object asset)
	{
		try
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(asset as GameObject);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			RectTransform component2 = GameEntry.UIContainer.GetComponent<RectTransform>();
			component.SetParent(component2, worldPositionStays: false);
			component.localScale = Vector3.one;
			component.offsetMin = Vector3.zero;
			component.offsetMax = Vector3.zero;
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.pivot = new Vector2(0.5f, 0.5f);
			component.SetAsLastSibling();
			Canvas component3 = component.GetComponent<Canvas>();
			component3.overrideSorting = true;
			component3.sortingOrder = 32767;
			_uiChooseLocalUpdate = gameObject.GetComponent<UIChooseLocalUpdate>();
			_uiChooseLocalUpdate.Load();
		}
		catch (Exception message)
		{
			Log.Error(message);
			_localUpdateConfigSet = true;
		}
	}

	private void OnUpdate()
	{
		if (_localUpdateConfigSet)
		{
			UnityEngine.Object.Destroy(_uiChooseLocalUpdate.gameObject);
			_uiChooseLocalUpdate = null;
			Resources.UnloadUnusedAssets();
			Exit();
		}
	}
}
