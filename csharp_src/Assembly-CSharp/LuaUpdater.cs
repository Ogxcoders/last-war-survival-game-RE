using System;
using UnityEngine;
using XLua;

[Hotfix(HotfixFlag.Stateless)]
public class LuaUpdater : MonoBehaviour
{
	private Action<float, float> luaUpdate;

	private Action luaLateUpdate;

	public void OnInit(LuaEnv luaEnv)
	{
		Restart(luaEnv);
	}

	public void Restart(LuaEnv luaEnv)
	{
		luaUpdate = luaEnv.Global.Get<Action<float, float>>("Update");
		luaLateUpdate = luaEnv.Global.Get<Action>("LateUpdate");
	}

	private void Update()
	{
		if (luaUpdate != null)
		{
			try
			{
				luaUpdate(Time.deltaTime, Time.unscaledDeltaTime);
			}
			catch (Exception ex)
			{
				Debug.LogError("luaUpdate err : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
	}

	private void LateUpdate()
	{
		if (luaLateUpdate != null)
		{
			try
			{
				luaLateUpdate();
			}
			catch (Exception ex)
			{
				Debug.LogError("luaLateUpdate err : " + ex.Message + "\n" + ex.StackTrace);
			}
		}
	}

	public void Dispose()
	{
		luaUpdate = null;
		luaLateUpdate = null;
	}
}
