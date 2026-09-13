using UnityEngine.SceneManagement;

namespace VEngine;

public class BundledScene : Scene
{
	protected Dependencies dependencies;

	protected override void OnUpdate()
	{
		switch (base.status)
		{
		case LoadableStatus.DependentLoading:
			UpdateDependencies();
			break;
		case LoadableStatus.Loading:
			UpdateLoading();
			break;
		}
	}

	private void UpdateDependencies()
	{
		if (dependencies == null)
		{
			Finish("dependencies == null");
			return;
		}
		base.progress = dependencies.progress * 0.5f;
		if (dependencies.isDone)
		{
			if (dependencies.assetBundle == null)
			{
				Finish("assetBundle == null");
				return;
			}
			base.operation = SceneManager.LoadSceneAsync(sceneName, base.loadSceneMode);
			base.status = LoadableStatus.Loading;
		}
	}

	protected override void OnUnload()
	{
		base.OnUnload();
		if (dependencies != null)
		{
			dependencies.Unload();
			dependencies = null;
		}
	}

	protected override void OnLoad()
	{
		PrepareToLoad();
		dependencies = new Dependencies
		{
			pathOrURL = base.pathOrURL
		};
		dependencies.Load();
		base.status = LoadableStatus.DependentLoading;
	}

	internal static Scene Create(string assetPath, bool additive = false)
	{
		if (Versions.GetAsset(ref assetPath) == null)
		{
			return new Scene
			{
				pathOrURL = assetPath,
				loadSceneMode = (additive ? LoadSceneMode.Additive : LoadSceneMode.Single)
			};
		}
		return new BundledScene
		{
			pathOrURL = assetPath,
			loadSceneMode = (additive ? LoadSceneMode.Additive : LoadSceneMode.Single)
		};
	}
}
