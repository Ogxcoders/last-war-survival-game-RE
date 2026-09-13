using LuaScriptInterface;
using Sfs2X.Entities.Data;

public class BasementTrainingCamp : CityBuilding
{
	private int _queueType;

	protected internal override void CSInit(object userData)
	{
		base.CSInit(userData);
		if (IsSelf())
		{
			_queueType = GameEntry.Lua.CallWithReturn<int, int>("CSharpCallLuaInterface.GetTrainingTypeAndBuildingType", build_Id);
			GameEntry.Event.Subscribe(EventId.QUEUE_TIME_END, QueueTimeEndSignal);
			GameEntry.Event.Subscribe(EventId.TrainingArmy, TrainingArmySignal);
			GameEntry.Event.Subscribe(EventId.TrainingArmyFinish, TrainingArmyFinishSignal);
			GameEntry.Event.Subscribe(EventId.AddSpeedSuccess, AddSpeedSuccessSignal);
			RefreshAnim();
		}
	}

	protected internal override void CSUninit()
	{
		base.CSUninit();
		GameEntry.Event.Unsubscribe(EventId.QUEUE_TIME_END, QueueTimeEndSignal);
		GameEntry.Event.Unsubscribe(EventId.TrainingArmy, TrainingArmySignal);
		GameEntry.Event.Unsubscribe(EventId.TrainingArmyFinish, TrainingArmyFinishSignal);
		GameEntry.Event.Unsubscribe(EventId.AddSpeedSuccess, AddSpeedSuccessSignal);
	}

	private void QueueTimeEndSignal(object userData)
	{
		if (_level > 0 && !IsRuins() && userData.ToInt() == _queueType)
		{
			PlayAnim("end");
		}
	}

	private void TrainingArmySignal(object userData)
	{
		if (_level > 0 && !IsRuins() && userData is SFSObject sFSObject && sFSObject.ContainsKey("bUuid") && base.Uuid == sFSObject.GetLong("bUuid"))
		{
			PlayAnim("working");
		}
	}

	private void TrainingArmyFinishSignal(object userData)
	{
		if (_level > 0 && !IsRuins() && userData.ToInt() == _queueType)
		{
			PlayAnim("idle");
		}
	}

	private void AddSpeedSuccessSignal(object userData)
	{
		if (_level > 0 && !IsRuins() && userData.ToInt() == _queueType)
		{
			RefreshAnim();
		}
	}

	private void RefreshAnim()
	{
		QueueData queueDataByType = GameEntry.Lua.GetQueueDataByType(_queueType);
		if (queueDataByType != null)
		{
			switch (queueDataByType.GetQueueState())
			{
			case 0:
				PlayAnim("idle");
				break;
			case 2:
				PlayAnim("working");
				break;
			case 3:
				PlayAnim("end");
				break;
			case 1:
				break;
			}
		}
	}

	protected override bool IsSelfControlWorkAnim()
	{
		if (IsSelf())
		{
			return true;
		}
		return false;
	}
}
