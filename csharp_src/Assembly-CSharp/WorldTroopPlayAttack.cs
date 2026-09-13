using UnityEngine;

public class WorldTroopPlayAttack : WorldTroopStateBase
{
	private long _nextPlayTime;

	private long _lastPlayTime;

	private bool _isPlayed;

	public WorldTroopPlayAttack(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	public override void OnStateEnter()
	{
		base.OnStateEnter();
		if (worldTroop != null)
		{
			WorldMarch marchInfo = worldTroop.GetMarchInfo();
			if (marchInfo != null)
			{
				_nextPlayTime = marchInfo.endTime;
				_isPlayed = false;
			}
			else
			{
				_isPlayed = true;
				_nextPlayTime = long.MaxValue;
			}
			worldTroop.PlayQueued("idle");
		}
	}

	public override void OnStateLeave()
	{
		if (worldTroop != null)
		{
			base.OnStateLeave();
		}
	}

	public override void OnStateUpdate(float deltaTime)
	{
		base.OnStateUpdate(deltaTime);
		if (worldTroop == null || worldTroop.GetMarchInfo() == null)
		{
			return;
		}
		if (worldTroop.GetMarchStatus() == MarchStatus.BEHEMOTH_ATTACK_CITY)
		{
			if (_isPlayed)
			{
				if (worldTroop.GetMarchInfo().endTime != _nextPlayTime)
				{
					_nextPlayTime = worldTroop.GetMarchInfo().endTime;
					_isPlayed = false;
				}
				return;
			}
			long serverTime = GameEntry.Timer.GetServerTime();
			if (serverTime > _nextPlayTime)
			{
				_isPlayed = true;
				_lastPlayTime = serverTime;
				worldTroop.PlayAnim("attack", rewind: true);
				worldTroop.PlayQueued("idle");
				Debug.Log("WorldTroopPlayAttack: play attack");
			}
		}
		else if (GameEntry.Timer.GetServerTime() - _lastPlayTime > 1000)
		{
			worldTroop.RefreshState();
		}
	}
}
