using System.Collections.Generic;
using UnityEngine;

public class WorldTroopStateS1SeasonPreActBossAttackEachOther : WorldTroopStateBase
{
	private float _waitToAttack;

	private float _waitToSpeak;

	private string[] CirHitSpeakTalks;

	private string[] SpeakTalks;

	private float SpeakInterval;

	private float AttackIntervalDown;

	private float AttackIntervalTop;

	private bool AiStart;

	private bool Weak;

	public bool PlayCirHit;

	private string S1_SEASONPREBOSS_CIRHIT = "S1_SeasonPreBoss_CirHit";

	private static int LastSpeakMonsterId = -1;

	private WorldMarch WorldMarch;

	public WorldTroopStateS1SeasonPreActBossAttackEachOther(WorldTroop worldTroop, WorldTroopStateMachine stateMachine)
		: base(worldTroop, stateMachine)
	{
	}

	private float GetAttackInterval()
	{
		return Random.Range(AttackIntervalDown, AttackIntervalTop);
	}

	private string GetSpeakText()
	{
		if (SpeakTalks == null || SpeakTalks.Length == 0)
		{
			return string.Empty;
		}
		int num = Random.Range(0, SpeakTalks.Length);
		return SpeakTalks[num];
	}

	private string GetCirHitSpeakText()
	{
		if (CirHitSpeakTalks == null || CirHitSpeakTalks.Length == 0)
		{
			return string.Empty;
		}
		int num = Random.Range(0, CirHitSpeakTalks.Length);
		return CirHitSpeakTalks[num];
	}

	public override void OnStateEnter()
	{
		if (worldTroop == null)
		{
			return;
		}
		WorldMarch = worldTroop.GetMarchInfo();
		base.OnStateEnter();
		Weak = worldTroop.IsWeakS1SeasonPreBoss();
		if (Weak)
		{
			worldTroop.PlayAnim("weak");
			return;
		}
		AiStart = false;
		List<string> list = GameEntry.Lua.CallWithReturn<List<string>, int>("CSharpCallLuaInterface.GetS1SeasonPreBossAIConfig", WorldMarch.monsterId);
		if (list != null && list.Count != 0)
		{
			float.TryParse(list[0], out SpeakInterval);
			string[] array = list[1].Split(new char[1] { ';' });
			float.TryParse(array[0], out AttackIntervalDown);
			float.TryParse(array[1], out AttackIntervalTop);
			SpeakTalks = GameEntry.Lua.CallWithReturn<string[], int, int>("CSharpCallLuaInterface.GetS1SeasonPreBossSpeakByType", WorldMarch.monsterId, 1) ?? new string[0];
			CirHitSpeakTalks = GameEntry.Lua.CallWithReturn<string[], int, int>("CSharpCallLuaInterface.GetS1SeasonPreBossSpeakByType", WorldMarch.monsterId, 2) ?? new string[0];
			_waitToSpeak = SpeakInterval;
			_waitToAttack = GetAttackInterval();
			AiStart = true;
			if (LastSpeakMonsterId == -1)
			{
				LastSpeakMonsterId = WorldMarch.monsterId;
			}
		}
	}

	private bool CanToPlayWeakAnim()
	{
		if (!GameEntry.Lua.CallWithReturn<bool, string>("CSharpCallLuaInterface.GetTodayCanShowSecondConfirm", S1_SEASONPREBOSS_CIRHIT))
		{
			return false;
		}
		return GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.CheckCanToPlayerS1SeasonPreBossCirHit");
	}

	public override void OnStateUpdate(float deltaTime)
	{
		if (Weak && !CanToPlayWeakAnim())
		{
			LastSpeakMonsterId = WorldMarch.monsterId;
		}
		else
		{
			if (PlayCirHit)
			{
				return;
			}
			if (CanToPlayWeakAnim())
			{
				string empty = string.Empty;
				if (worldTroop.IsWeakS1SeasonPreBoss())
				{
					empty = "get_beat";
					worldTroop.PlayAnim(empty);
					worldTroop.PlayQueued("weak");
					Weak = true;
				}
				else
				{
					empty = "beat";
					worldTroop.PlayAnim(empty);
					worldTroop.PlayQueued("idle");
					_waitToAttack = GetAttackInterval();
					_waitToSpeak = SpeakInterval;
				}
				PlayCirHit = true;
				float num = worldTroop.TryGetAnimStateTimeLength(empty);
				if (num < 0f)
				{
					num = 4f;
				}
				if (empty == "beat")
				{
					worldTroop.TryShowWorldBossTipByText(GetCirHitSpeakText(), 2f);
				}
				YieldUtils.DelayActionWithOutContext(delegate
				{
					GameEntry.Lua.CallWithReturn<bool, string, bool>("CSharpCallLuaInterface.SetTodayNoShowSecondConfirm", S1_SEASONPREBOSS_CIRHIT, param2: false);
					PlayCirHit = false;
				}, num);
			}
			else
			{
				if (!AiStart)
				{
					return;
				}
				_waitToAttack -= deltaTime;
				if (_waitToAttack <= 0f)
				{
					_waitToAttack = GetAttackInterval();
					worldTroop.PlayQueued("fight_eachother");
					worldTroop.PlayQueued("idle");
				}
				_waitToSpeak -= deltaTime;
				if (!(_waitToSpeak <= 0f))
				{
					return;
				}
				_waitToSpeak = SpeakInterval;
				if (LastSpeakMonsterId != WorldMarch.monsterId)
				{
					worldTroop.TryShowWorldBossTipByText(GetSpeakText(), 2f, delegate
					{
						LastSpeakMonsterId = WorldMarch.monsterId;
					});
				}
			}
		}
	}
}
