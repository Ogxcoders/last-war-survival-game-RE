using System;
using UnityEngine;
using XLua;

public class LuaUIFormLogic : MonoBehaviour
{
	[Serializable]
	public class Injection
	{
		public string name;

		public GameObject value;
	}

	public TextAsset luaScript;

	public Injection[] injections;

	internal static LuaEnv luaEnv = new LuaEnv();

	internal static float lastGCTime = 0f;

	internal const float GCInterval = 1f;

	public OneObjDelegate luaInit;

	public OneObjDelegate luaOpen;

	public TwoFloatDelegate luaUpdate;

	public OneObjDelegate luaClose;

	public Action luaCover;

	public Action luaResume;

	public Action luaPause;

	public OneObjDelegate luaRefocus;

	public Action luaReveal;

	private LuaTable scriptEnv;

	public bool DoLuaFunction(string name, out object[] res, params object[] args)
	{
		LuaFunction luaFunction = scriptEnv.Get<LuaFunction>(name);
		if (luaFunction == null)
		{
			res = null;
			return false;
		}
		res = luaFunction.Call(args);
		return true;
	}

	public void LuaInit(object userData)
	{
		scriptEnv = luaEnv.NewTable();
		LuaTable luaTable = luaEnv.NewTable();
		luaTable.Set("__index", luaEnv.Global);
		scriptEnv.SetMetaTable(luaTable);
		luaTable.Dispose();
		scriptEnv.Set("self", this);
		Injection[] array = injections;
		foreach (Injection injection in array)
		{
			scriptEnv.Set(injection.name, injection.value);
		}
		luaEnv.DoString(luaScript.text, base.gameObject.name, scriptEnv);
		luaInit = scriptEnv.Get<OneObjDelegate>("init");
		luaOpen = scriptEnv.Get<OneObjDelegate>("open");
		luaUpdate = scriptEnv.Get<TwoFloatDelegate>("update");
		luaClose = scriptEnv.Get<OneObjDelegate>("close");
		luaCover = scriptEnv.Get<Action>("cover");
		luaResume = scriptEnv.Get<Action>("resume");
		luaPause = scriptEnv.Get<Action>("pause");
		luaRefocus = scriptEnv.Get<OneObjDelegate>("refocus");
		luaReveal = scriptEnv.Get<Action>("reveal");
		if (luaInit != null)
		{
			luaInit(userData);
		}
	}

	public void LuaOpen(object userData)
	{
		if (luaOpen != null)
		{
			luaOpen(userData);
		}
	}

	public void LuaUpdate(float elapseSeconds, float realElapseSeconds)
	{
		if (luaUpdate != null)
		{
			luaUpdate(elapseSeconds, realElapseSeconds);
		}
		if (Time.time - lastGCTime > 1f)
		{
			luaEnv.Tick();
			lastGCTime = Time.time;
		}
	}

	public void LuaClose(object userData)
	{
		if (luaClose != null)
		{
			luaClose(userData);
		}
	}

	public void LuaCover()
	{
		if (luaCover != null)
		{
			luaCover();
		}
	}

	public void LuaResume()
	{
		if (luaResume != null)
		{
			luaResume();
		}
	}

	public void LuaPause()
	{
		if (luaPause != null)
		{
			luaPause();
		}
	}

	public void LuaRefocus(object userData)
	{
		if (luaRefocus != null)
		{
			luaRefocus(userData);
		}
	}

	public void LuaReveal()
	{
		if (luaReveal != null)
		{
			luaReveal();
		}
	}

	private void OnDestroy()
	{
		luaClose = null;
		luaUpdate = null;
		luaOpen = null;
		luaCover = null;
		luaRefocus = null;
		luaReveal = null;
		luaPause = null;
		luaResume = null;
		scriptEnv.Dispose();
		injections = null;
	}
}
