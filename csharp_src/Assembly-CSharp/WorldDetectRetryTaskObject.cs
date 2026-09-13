using System.Collections.Generic;

public class WorldDetectRetryTaskObject : WorldDetectEventItemObject
{
	public WorldDetectRetryTaskObject(WorldScene worldScene, int pointIndex, int pType)
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
		if (world.GetPointInfo(pointIndex) is DetectRetryTaskPointInfo detectRetryTaskPointInfo)
		{
			return detectRetryTaskPointInfo.uuid;
		}
		return 0L;
	}

	protected override bool NeedShowTime()
	{
		return false;
	}

	protected override string GetEventId()
	{
		if (world.GetPointInfo(pointIndex) is DetectRetryTaskPointInfo detectRetryTaskPointInfo)
		{
			return detectRetryTaskPointInfo.eventId;
		}
		return "";
	}

	protected override string GetModePath()
	{
		string text = "";
		int key = eventId.ToInt();
		int key2 = 44;
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
