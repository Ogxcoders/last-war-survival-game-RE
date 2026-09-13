using System.Diagnostics;
using UnityEngine;

public abstract class LittleSmartUpdater<THost, TSub> where THost : WorldManagerBase where TSub : LittleSmartUpdater<THost, TSub>, new()
{
	protected THost host;

	private LittleSmartGizmos gizmosHelper;

	public virtual bool IsLittleSmartModeEnable { get; private set; }

	protected abstract string LuaCheckEnableFuncName { get; }

	protected abstract EventId ChangeTroopModeEventId { get; }

	public static TSub Create(THost host)
	{
		TSub val = new TSub();
		val.host = host;
		val.Init();
		return val;
	}

	protected void Init()
	{
		GameEntry.Event.Subscribe(ChangeTroopModeEventId, RefreshLittleSmartMode);
		OnInit();
		RefreshLittleSmartMode();
		RefreshLittleSmartDebugState();
	}

	[Conditional("UNITY_EDITOR")]
	public void EditorLog(string log)
	{
	}

	public virtual string EditorDescription()
	{
		return typeof(THost).Name ?? "";
	}

	protected virtual void RefreshLittleSmartMode(object _ = null)
	{
		bool isLittleSmartModeEnable = IsLittleSmartModeEnable;
		IsLittleSmartModeEnable = GameEntry.Lua.CallWithReturn<bool>(LuaCheckEnableFuncName);
		if (isLittleSmartModeEnable != IsLittleSmartModeEnable)
		{
			OnLittleSmartModeChanged();
		}
	}

	public void Dispose()
	{
		if (gizmosHelper != null)
		{
			Object.Destroy(gizmosHelper.gameObject);
			gizmosHelper = null;
		}
		GameEntry.Event.Unsubscribe(ChangeTroopModeEventId, RefreshLittleSmartMode);
		OnDispose();
	}

	public void UpdateLittleSmart(float delteTime)
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		OnLittleSmartUpdate(serverTime, delteTime);
	}

	protected virtual void OnLittleSmartUpdate(long currentServerTime, float delteTime)
	{
	}

	protected virtual void OnLittleSmartModeChanged()
	{
	}

	protected virtual void OnInit()
	{
	}

	protected virtual void OnDispose()
	{
	}

	private void RefreshLittleSmartDebugState()
	{
	}

	protected virtual void OnDrawGizmos()
	{
	}

	protected virtual void OnDrawGizmosSelected()
	{
	}
}
