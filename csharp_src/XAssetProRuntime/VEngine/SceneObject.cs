using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VEngine;

public class SceneObject : Loadable, IEnumerator
{
	internal static readonly List<SceneObject> Unused = new List<SceneObject>();

	internal readonly Dictionary<string, SceneObjectAction> actions = new Dictionary<string, SceneObjectAction>();

	public readonly Assets assets = new Assets();

	protected readonly List<SceneObject> children = new List<SceneObject>();

	protected int actionTimes;

	private Asset asset;

	public Action<SceneObject> completed;

	protected Scene owner;

	protected SceneObject parent;

	protected Transform transformParent;

	protected bool transformWorldPositionStays;

	public GameObject gameObject { get; private set; }

	public object Current => null;

	public bool ActiveSelf { get; private set; }

	public bool MoveNext()
	{
		return !base.isDone;
	}

	public void Reset()
	{
	}

	public void SetTransformParent(Transform tranParent, bool worldPositionStays)
	{
		transformParent = tranParent;
		transformWorldPositionStays = worldPositionStays;
		if (gameObject != null)
		{
			gameObject.transform.SetParent(tranParent, worldPositionStays);
		}
	}

	public void SetActive(bool active)
	{
		if (ActiveSelf != active)
		{
			ActiveSelf = active;
			if (gameObject != null)
			{
				gameObject.SetActive(active);
			}
		}
	}

	public bool RemoveAction(string key)
	{
		return actions.Remove(key);
	}

	public SceneObjectAction RunAction(Action<SceneObject> func, string key = null)
	{
		if (string.IsNullOrEmpty(key))
		{
			key = $"actions_{actionTimes++}";
		}
		if (actions.TryGetValue(key, out var value))
		{
			value.func = func;
			return value;
		}
		SceneObjectAction sceneObjectAction = new SceneObjectAction
		{
			func = func,
			key = key,
			sceneObject = this
		};
		actions.Add(sceneObjectAction.key, sceneObjectAction);
		sceneObjectAction.Start();
		return sceneObjectAction;
	}

	public static SceneObject LoadAsync(string assetPath, Action<SceneObject> completed = null)
	{
		if (string.IsNullOrEmpty(assetPath))
		{
			throw new ArgumentNullException("assetPath");
		}
		SceneObject obj = new SceneObject
		{
			pathOrURL = assetPath
		};
		obj.completed = (Action<SceneObject>)Delegate.Combine(obj.completed, completed);
		obj.Load();
		return obj;
	}

	public void AddChild(SceneObject child)
	{
		if (child.parent != null)
		{
			child.parent.RemoveChild(child);
		}
		children.Add(child);
		child.parent = this;
	}

	public bool RemoveChild(SceneObject child)
	{
		return children.Remove(child);
	}

	protected override void OnUpdate()
	{
		LoadableStatus loadableStatus = base.status;
		if (loadableStatus != LoadableStatus.Loading)
		{
			return;
		}
		if (asset == null)
		{
			Finish("asset == null");
			return;
		}
		base.progress = 0.5f + asset.progress * 0.5f;
		if (!asset.isDone)
		{
			return;
		}
		GameObject gameObject = asset.Get<GameObject>();
		if (gameObject == null)
		{
			Finish("prefab == null");
			return;
		}
		this.gameObject = UnityEngine.Object.Instantiate(gameObject);
		this.gameObject.transform.SetParent(transformParent, transformWorldPositionStays);
		if (this.gameObject.activeSelf != ActiveSelf)
		{
			this.gameObject.SetActive(ActiveSelf);
		}
		Finish();
	}

	protected override void OnLoad()
	{
		asset = assets.Preload(base.pathOrURL, typeof(GameObject));
		owner = Scene.current;
		if (owner != null)
		{
			owner.objects.Add(this);
		}
	}

	protected override void OnUnused()
	{
		completed = null;
		Unused.Add(this);
	}

	protected override void OnUnload()
	{
		if (gameObject != null)
		{
			UnityEngine.Object.Destroy(gameObject);
			gameObject = null;
		}
		if (owner != null)
		{
			owner.objects.Remove(this);
			owner = null;
		}
		foreach (KeyValuePair<string, SceneObjectAction> action in actions)
		{
			action.Value.Cancel();
		}
		actions.Clear();
		foreach (SceneObject child in children)
		{
			child.Release();
		}
		children.Clear();
		assets.Clear();
	}

	protected override void OnComplete()
	{
		if (completed != null)
		{
			Action<SceneObject> value = completed;
			if (completed != null)
			{
				completed(this);
			}
			completed = (Action<SceneObject>)Delegate.Remove(completed, value);
		}
	}

	public static void UpdateObjects()
	{
		for (int i = 0; i < Unused.Count; i++)
		{
			SceneObject sceneObject = Unused[i];
			if (!Updater.busy)
			{
				if (sceneObject.isDone)
				{
					Unused.RemoveAt(i);
					i--;
					sceneObject.Unload();
				}
				continue;
			}
			break;
		}
	}
}
