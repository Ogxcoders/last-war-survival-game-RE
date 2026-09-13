using System;
using UnityEngine;
using XLua;

public class KeyCodeListener : MonoBehaviour
{
	private Action<int> onKeyBoard;

	private LuaEnv luaEnv;

	private void Awake()
	{
	}

	private void Update()
	{
		OnCheck();
	}

	private void OnCheck()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			GameEntry.Event.Fire(EventId.OnKeyCodeEscape);
		}
	}

	private void OnGUI()
	{
		luaEnv = GameEntry.Lua.Env;
		if (luaEnv != null && Input.anyKeyDown)
		{
			Event current = Event.current;
			if (current.isKey)
			{
				onKeyBoard = luaEnv.Global.Get<Action<int>>("onKeyBoard");
				onKeyBoard?.Invoke((int)current.keyCode);
				onKeyBoard = null;
			}
		}
	}
}
