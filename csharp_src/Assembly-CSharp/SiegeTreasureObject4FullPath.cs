using System.Collections.Generic;

public class SiegeTreasureObject4FullPath : SiegeTreasureObject
{
	public SiegeTreasureObject4FullPath(WorldScene worldScene, int pointIndex, int pType)
		: base(worldScene, pointIndex, pType)
	{
	}

	protected override string GetModePath()
	{
		string text = "";
		int key = eventId.ToInt();
		int key2 = 1001;
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
			text = GameEntry.ConfigCache.GetTemplateData("world_treasure", key, "fullPathModels");
			if (string.IsNullOrEmpty(text))
			{
				text = GameEntry.ConfigCache.GetTemplateData("world_treasure", key, "models");
				text = "Assets/Main/Prefabs/Garbage/" + text + ".prefab";
			}
			if (!WorldScene.ModelPathDic.ContainsKey(key))
			{
				WorldScene.ModelPathDic[key] = new Dictionary<int, string>();
			}
			WorldScene.ModelPathDic[key][key2] = text;
		}
		return text;
	}
}
