using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VEngine;

public class Scene : Loadable, IEnumerator
{
	internal static readonly List<Scene> Unused = new List<Scene>();

	public static Action<Scene> onSceneUnloaded;

	public static Action<Scene> onSceneLoaded;

	internal readonly List<Scene> additives = new List<Scene>();

	internal readonly List<SceneObject> objects = new List<SceneObject>();

	public Action<Scene> completed;

	protected string sceneName;

	public AsyncOperation operation { get; protected set; }

	public static Scene main { get; private set; }

	public static Scene current { get; private set; }

	protected internal LoadSceneMode loadSceneMode { get; set; }

	public object Current => null;

	public bool MoveNext()
	{
		return !base.isDone;
	}

	public void Reset()
	{
	}

	public static Scene LoadAsync(string assetPath, Action<Scene> completed = null, bool additive = false)
	{
		if (string.IsNullOrEmpty(assetPath))
		{
			throw new ArgumentNullException("assetPath");
		}
		Scene scene = Versions.CreateScene(assetPath, additive);
		if (completed != null)
		{
			scene.completed = (Action<Scene>)Delegate.Combine(scene.completed, completed);
		}
		current = scene;
		scene.Load();
		return scene;
	}

	public static Scene LoadAdditiveAsync(string assetPath, Action<Scene> completed = null)
	{
		return LoadAsync(assetPath, completed, additive: true);
	}

	protected override void OnUpdate()
	{
		if (base.status == LoadableStatus.Loading)
		{
			UpdateLoading();
		}
	}

	protected void UpdateLoading()
	{
		if (operation == null)
		{
			Finish("operation == null");
			return;
		}
		base.progress = 0.5f + operation.progress * 0.5f;
		if (operation.allowSceneActivation)
		{
			if (!operation.isDone)
			{
				return;
			}
		}
		else if (operation.progress < 0.9f)
		{
			return;
		}
		Finish();
	}

	protected override void OnLoad()
	{
		PrepareToLoad();
		operation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
	}

	protected void PrepareToLoad()
	{
		sceneName = Path.GetFileNameWithoutExtension(base.pathOrURL);
		if (loadSceneMode == LoadSceneMode.Single)
		{
			if (main != null)
			{
				main.Release();
				main = null;
			}
			main = this;
		}
		else if (main != null)
		{
			main.additives.Add(this);
		}
	}

	protected override void OnUnused()
	{
		completed = null;
		Unused.Add(this);
	}

	protected override void OnUnload()
	{
		foreach (SceneObject @object in objects)
		{
			@object.Release();
		}
		objects.Clear();
		if (loadSceneMode == LoadSceneMode.Additive)
		{
			if (main != null)
			{
				main.additives.Remove(this);
			}
			if (string.IsNullOrEmpty(base.error))
			{
				SceneManager.UnloadSceneAsync(sceneName);
			}
		}
		else
		{
			foreach (Scene additive in additives)
			{
				additive.Release();
			}
			additives.Clear();
		}
		if (onSceneUnloaded != null)
		{
			onSceneUnloaded(this);
		}
	}

	protected override void OnComplete()
	{
		if (onSceneLoaded != null)
		{
			onSceneLoaded(this);
		}
		if (completed != null)
		{
			Action<Scene> value = completed;
			if (completed != null)
			{
				completed(this);
			}
			completed = (Action<Scene>)Delegate.Remove(completed, value);
		}
	}

	public static void UpdateScenes()
	{
		if (current == null || !current.isDone)
		{
			return;
		}
		for (int i = 0; i < Unused.Count; i++)
		{
			Scene scene = Unused[i];
			if (!Updater.busy)
			{
				if (scene.isDone)
				{
					Unused.RemoveAt(i);
					i--;
					scene.Unload();
				}
				continue;
			}
			break;
		}
	}
}
