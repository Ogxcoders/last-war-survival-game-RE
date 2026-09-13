namespace UnityEngine.Rendering;

internal class DebugUpdater : MonoBehaviour
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void RuntimeInit()
	{
		if (Debug.isDebugBuild && !(Object.FindObjectOfType<DebugUpdater>() != null))
		{
			GameObject obj = new GameObject();
			obj.name = "[Debug Updater]";
			obj.AddComponent<DebugUpdater>();
			Object.DontDestroyOnLoad(obj);
		}
	}

	private void Update()
	{
		DebugManager.instance.UpdateActions();
		if (DebugManager.instance.GetAction(DebugAction.EnableDebugMenu) != 0f)
		{
			DebugManager.instance.displayRuntimeUI = !DebugManager.instance.displayRuntimeUI;
		}
		if (DebugManager.instance.displayRuntimeUI && DebugManager.instance.GetAction(DebugAction.ResetAll) != 0f)
		{
			DebugManager.instance.Reset();
		}
	}
}
