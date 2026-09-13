using System;

namespace VEngine;

public class SceneObjectAction : Operation
{
	public Action<SceneObject> func;

	public string key;

	public SceneObject sceneObject;

	protected override void Update()
	{
		if (sceneObject.isDone && !base.isDone)
		{
			if (sceneObject.status == LoadableStatus.FailedToLoad)
			{
				Finish("view not exists.");
				return;
			}
			sceneObject.RemoveAction(key);
			func(sceneObject);
			Finish();
		}
	}
}
