using System;
using GameFramework;

public class CrossServerAfterCross : FsmBaseState
{
	public override int id => 6;

	public CrossServerAfterCross(CrossServerFsmManager mgr)
		: base(mgr)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_mgr.crossing = false;
		if (SceneManager.IsInWorld())
		{
			try
			{
				GameEntry.NetworkCross.Shutdown();
			}
			catch (Exception ex)
			{
				Log.Error("CrossServerAfterCross Shutdown NetworkCross error: {0} \n stack:{1}", ex.Message, ex.StackTrace);
			}
			SceneManager.World.SetFirstViewRequestFlag(isFirstTime: true);
			SceneManager.World.UpdateViewRequest(isForce: true);
			GameEntry.Lua.Call("GoToUtil.GotoMainBuildPos");
			GameEntry.Lua.Call("GoToUtil.CloseAllWindows");
			GameEntry.Event.Fire(EventId.ShowCrossServerTip);
			GameEntry.Event.Fire(EventId.CrossChatServer);
		}
		GotoLoadingError(0);
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}
}
