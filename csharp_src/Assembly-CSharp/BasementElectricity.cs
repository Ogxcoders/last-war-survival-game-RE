public class BasementElectricity : CityBuilding
{
	protected internal override void CSInit(object userData)
	{
		base.CSInit(userData);
		if (IsSelf())
		{
			GameEntry.Event.Subscribe(EventId.UPDATE_BUILD_DATA, UpdateBuildDataSignal);
			CheckDoAnim();
		}
	}

	protected internal override void CSUninit()
	{
		base.CSUninit();
		GameEntry.Event.Unsubscribe(EventId.UPDATE_BUILD_DATA, UpdateBuildDataSignal);
	}

	private void UpdateBuildDataSignal(object userData)
	{
		if ((long)userData == base.Uuid)
		{
			CheckDoAnim();
		}
	}

	private void CheckDoAnim()
	{
		if (_level > 0 && !IsRuins())
		{
			if (GameEntry.Lua.CallWithReturn<bool, long>("CSharpCallLuaInterface.IsBuildWork", base.Uuid))
			{
				PlayAnim("working");
			}
			else
			{
				PlayAnim("idle");
			}
		}
	}

	protected override bool IsSelfControlWorkAnim()
	{
		return true;
	}
}
