using System.Collections.Generic;
using UnityEngine;

public class WorldDetectAttackCityMonsterObject : WorldDetectEventItemObject
{
	public WorldDetectAttackCityMonsterObject(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	protected override bool NeedShowDetectEventIcon()
	{
		return true;
	}

	public override bool DoDisappear()
	{
		return false;
	}

	protected override long GetPointUid()
	{
		if (world.GetPointInfo(pointIndex) is DetectAttackCityS0TaskPointInfo detectAttackCityS0TaskPointInfo)
		{
			return detectAttackCityS0TaskPointInfo.uuid;
		}
		return 0L;
	}

	protected override bool NeedShowTime()
	{
		return false;
	}

	protected override string GetEventId()
	{
		if (world.GetPointInfo(pointIndex) is DetectAttackCityS0TaskPointInfo detectAttackCityS0TaskPointInfo)
		{
			return detectAttackCityS0TaskPointInfo.eventId;
		}
		return "";
	}

	protected override string GetModePath()
	{
		string text = "";
		if (string.IsNullOrEmpty(eventId))
		{
			Debug.LogError("eventId is invalid");
			return "Assets/Main/Prefabs/Monsters/WorldMonster01_secret.prefab";
		}
		int key = eventId.ToInt();
		int key2 = 50;
		if (WorldScene.ModelPathDic.ContainsKey(key))
		{
			Dictionary<int, string> dictionary = WorldScene.ModelPathDic[key];
			if (dictionary.ContainsKey(key2))
			{
				text = dictionary[key2];
			}
		}
		if (text.IsNullOrEmpty())
		{
			text = GameEntry.Lua.CallWithReturn<string, string>("CSharpCallLuaInterface.GetDetectRetryTaskObjectPath", eventId);
			if (!WorldScene.ModelPathDic.ContainsKey(key))
			{
				WorldScene.ModelPathDic[key] = new Dictionary<int, string>();
			}
			WorldScene.ModelPathDic[key][key2] = text;
		}
		return text;
	}
}
