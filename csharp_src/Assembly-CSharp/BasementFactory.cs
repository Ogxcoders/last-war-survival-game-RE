public class BasementFactory : CityBuilding
{
	protected internal override void CSInit(object userData)
	{
		base.CSInit(userData);
		if (IsSelf())
		{
			GameEntry.Event.Subscribe(EventId.CanGetProduct, CanGetProductSignal);
			GameEntry.Event.Subscribe(EventId.AddFactoryProduct, CanGetProductSignal);
			GameEntry.Event.Subscribe(EventId.GetFactoryData, CanGetProductSignal);
			RefreshState();
		}
	}

	protected internal override void CSUninit()
	{
		base.CSUninit();
		GameEntry.Event.Unsubscribe(EventId.CanGetProduct, CanGetProductSignal);
		GameEntry.Event.Unsubscribe(EventId.AddFactoryProduct, CanGetProductSignal);
		GameEntry.Event.Unsubscribe(EventId.GetFactoryData, CanGetProductSignal);
	}

	private void CanGetProductSignal(object userData)
	{
		RefreshState();
	}

	private void RefreshState()
	{
		if (_level > 0 && !IsRuins())
		{
			switch (GameEntry.Lua.CallWithReturnInt("DataCenter.FactoryDataManager:GetFactoryStateByBuildUuid", base.Uuid))
			{
			case 0:
			case 2:
				PlayAnim("self_working");
				break;
			case 1:
				PlayAnim("working");
				break;
			case 3:
				PlayAnim("end");
				break;
			}
		}
	}

	protected override bool IsSelfControlWorkAnim()
	{
		return true;
	}
}
