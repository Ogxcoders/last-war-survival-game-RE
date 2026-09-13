using System;
using GameFramework;
using Protobuf;
using Sfs2X.Entities.Data;
using UnityEngine;

public class SandWormData : IDisposable
{
	public byte state;

	public long stateEndTime;

	public string stateTriggerInfo;

	public int monsterId;

	public long curHp;

	public long maxHp;

	public long expireTime;

	public string finderName;

	public string finderUid;

	public string finderAbbr;

	public int finderServerId;

	public void SetData(ISFSObject msg, string uid, string name, string abbr, int serverId)
	{
		state = (byte)msg.TryGetInt("state");
		curHp = msg.TryGetLong("curHp");
		maxHp = msg.TryGetLong("maxHp");
		if (maxHp == 0L)
		{
			maxHp = 1L;
		}
		monsterId = msg.TryGetInt("monsterId");
		expireTime = msg.TryGetLong("stateEndTime");
		SetFinder(uid, name, abbr, serverId);
	}

	public void SetData(Sandworm msg, string uid, string name, string abbr, int serverId)
	{
		state = (byte)msg.State;
		curHp = msg.CurHp;
		maxHp = msg.MaxHp;
		if (maxHp == 0L)
		{
			maxHp = 1L;
		}
		monsterId = msg.MonsterId;
		expireTime = msg.StateEndTime;
		SetFinder(uid, name, abbr, serverId);
	}

	public void SetData(MonsterInfo msg, string uid, string name, string abbr, int serverId)
	{
		state = (byte)msg.State;
		curHp = msg.CurHp;
		maxHp = msg.MaxHp;
		if (maxHp == 0L)
		{
			maxHp = 1L;
		}
		monsterId = msg.MonsterId;
		stateEndTime = msg.StateEndTime;
		expireTime = msg.ExpireTime;
		stateTriggerInfo = msg.StateTriggerInfo;
		SetFinder(uid, name, abbr, serverId);
	}

	public void SetFinder(string uid, string name, string abbr, int serverId)
	{
		finderUid = uid;
		finderName = name;
		finderAbbr = abbr;
		finderServerId = serverId;
	}

	public string GetPrefabPath()
	{
		string templateData = GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "model_name");
		if (templateData.IsNullOrEmpty())
		{
			Log.Error("cant find sandworm model_name! configId：" + monsterId);
		}
		return templateData;
	}

	public void Dispose()
	{
	}

	public bool IsBirthing()
	{
		if (long.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_world_monster", monsterId, "expire"), out var result))
		{
			result *= 60000;
			long num = expireTime - result;
			long serverTime = GameEntry.Timer.GetServerTime();
			if (Mathf.Abs(num - serverTime) < 3000f)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsStunning()
	{
		return GetState() == 1;
	}

	public byte GetState()
	{
		if (state == 0)
		{
			return 0;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		if (stateEndTime < serverTime)
		{
			state = 0;
			stateEndTime = 0L;
			return 0;
		}
		return 1;
	}
}
