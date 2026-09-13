using AIHelp;
using FM_Mono;
using GameFramework;
using UnityEngine;
using VEngine;

public class EditorLauncher : ApplicationLaunch
{
	private static string _previousScenePath = string.Empty;

	private const string EDITOR_SCENE = "Assets/Main/Scripts/EditorOnly/Editor/EditorApp.unity";

	private void Awake()
	{
		AnoProxy.SdkInitEx();
		if (Runtime.IsDllLocalDirLocked())
		{
			Log.Info("DllLocalDir is locked, try to unlock it");
			Versions.DeleteManifest();
			Runtime.UnlockDllLocalDir();
		}
		ApplicationLaunch._instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		AIHelpProxy.Init();
		ConfigBastHttp();
		GameEntry.Init();
		GameEntry.Resource.Loggable = GameEntry.Setting.GetBool("Setting.Resource.Logger", defaultValue: false);
		base.Loading = new EditorStartupLoading();
		Log.Info("Resource.Initialize.");
		GameEntry.Resource.Initialize(base.OnResourcesInitialized);
		AIHelpProxy.SetAIHelpDataList();
		RealTimer.Reset();
	}
}
