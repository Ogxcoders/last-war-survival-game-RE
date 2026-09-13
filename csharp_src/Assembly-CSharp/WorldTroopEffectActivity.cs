using System;
using UnityEngine;

public class WorldTroopEffectActivity : IDisposable
{
	private InstanceRequest instanceRequest;

	public static string GetEffectByActivity(int id, string prefabPath)
	{
		return GameEntry.Lua.CallWithReturn<string, int, string>("CSharpCallLuaInterface.GetActivityDropEffect", id, prefabPath);
	}

	public void BuildEffect(string effectPath, GameObject parent)
	{
		if (parent == null)
		{
			Dispose();
		}
		else if (!GameEntry.Data.Player.IsInSelfServer())
		{
			Dispose();
		}
		else
		{
			if (instanceRequest != null && effectPath == instanceRequest.PrefabPath)
			{
				return;
			}
			instanceRequest = GameEntry.Resource.InstantiateAsync(effectPath);
			instanceRequest.completed += delegate(InstanceRequest r)
			{
				if (parent != null)
				{
					r.gameObject.transform.SetParent(parent.transform);
					r.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
					r.gameObject.transform.localScale = Vector3.one;
					r.gameObject.transform.localRotation = Quaternion.identity;
				}
				else
				{
					Dispose();
				}
			};
		}
	}

	public void Dispose()
	{
		if (instanceRequest != null)
		{
			instanceRequest.Destroy();
			instanceRequest = null;
		}
	}
}
