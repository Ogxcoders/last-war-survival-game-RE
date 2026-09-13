using LuaScriptInterface;

public class BasementHospital : CityBuilding
{
	private int _queueType;

	protected internal override void CSInit(object userData)
	{
		base.CSInit(userData);
		if (IsSelf())
		{
			_queueType = 3;
			GameEntry.Event.Subscribe(EventId.QUEUE_TIME_END, QueueTimeEndSignal);
			GameEntry.Event.Subscribe(EventId.HospitaiStart, HospitalStartSignal);
			RefreshAnim();
		}
	}

	protected internal override void CSUninit()
	{
		base.CSUninit();
		GameEntry.Event.Unsubscribe(EventId.QUEUE_TIME_END, QueueTimeEndSignal);
		GameEntry.Event.Unsubscribe(EventId.HospitaiStart, HospitalStartSignal);
	}

	private void QueueTimeEndSignal(object userData)
	{
		if (userData.ToInt() == _queueType)
		{
			PlayAnim("self_working");
		}
	}

	protected override bool IsSelfControlWorkAnim()
	{
		if (IsSelf())
		{
			RefreshAnim();
			return true;
		}
		return false;
	}

	private void RefreshAnim()
	{
		if (_level <= 0 || IsRuins())
		{
			return;
		}
		QueueData queueDataByType = GameEntry.Lua.GetQueueDataByType(_queueType);
		if (queueDataByType != null)
		{
			switch (queueDataByType.GetQueueState())
			{
			case 0:
			case 1:
			case 3:
				PlayAnim("idle");
				break;
			case 2:
				PlayAnim("working");
				break;
			}
		}
	}

	private void HospitalStartSignal(object userData)
	{
		RefreshAnim();
	}
}
