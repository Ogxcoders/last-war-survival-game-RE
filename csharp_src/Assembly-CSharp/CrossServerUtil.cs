using System;

public class CrossServerUtil
{
	private static CrossServerFsmManager _mgr;

	public static void SilentLogin()
	{
		_mgr = _mgr ?? new CrossServerFsmManager();
		_mgr.SyncServerListResult();
		_mgr.Start();
	}

	public static void Update()
	{
		if (_mgr != null)
		{
			_mgr.Update();
		}
	}

	public static bool IsCrossing()
	{
		if (_mgr == null)
		{
			return false;
		}
		return _mgr.crossing;
	}

	public static void ShowCloud(string sid)
	{
		GameEntry.Lua.Call<Action, Func<bool>, string>("UIUtil.PlayCutSceneAnim", null, _mgr.IsCrossingOver, sid);
	}
}
