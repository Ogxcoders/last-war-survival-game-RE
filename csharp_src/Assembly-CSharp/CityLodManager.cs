using System.Collections.Generic;
using XLua;

public class CityLodManager : CityManagerBase
{
	private HashSet<AutoAdjustLod> adjusterSet;

	private HashSet<AutoAdjustLod> addingAdjusterSet;

	private HashSet<AutoAdjustLod> removingAdjusterSet;

	private int curLod;

	private static Dictionary<int, Dictionary<string, LodConfig>> lodConfigCache;

	public CityLodManager(CityScene scene)
		: base(scene)
	{
		adjusterSet = new HashSet<AutoAdjustLod>();
		addingAdjusterSet = new HashSet<AutoAdjustLod>();
		removingAdjusterSet = new HashSet<AutoAdjustLod>();
		lodConfigCache = new Dictionary<int, Dictionary<string, LodConfig>>();
	}

	public override void Init()
	{
		GameEntry.Event.Subscribe(EventId.ChangeCameraLod, OnLodChanged);
	}

	public override void UnInit()
	{
		GameEntry.Event.Unsubscribe(EventId.ChangeCameraLod, OnLodChanged);
	}

	public void AddLodAdjuster(AutoAdjustLod adjuster)
	{
		adjuster.UpdateLod(curLod);
		addingAdjusterSet.Add(adjuster);
	}

	public void RemoveLodAdjuster(AutoAdjustLod adjuster)
	{
		removingAdjusterSet.Add(adjuster);
	}

	public override void OnUpdate(float deltaTime)
	{
		base.OnUpdate(deltaTime);
	}

	private void OnLodChanged(object userdata)
	{
		curLod = (int)userdata;
		UpdateAutoAdjustLod();
		if (curLod >= 2)
		{
			if (GameEntry.Lua.UIManager.IsWindowOpen("UIWorldTileUI"))
			{
				GameEntry.Lua.UIManager.DestroyWindow("UIWorldTileUI");
			}
			GameEntry.Lua.Call("UIUtil.CloseWorldMarchTileUI");
		}
	}

	private void UpdateAutoAdjustLod()
	{
		foreach (AutoAdjustLod item in removingAdjusterSet)
		{
			adjusterSet.Remove(item);
		}
		removingAdjusterSet.Clear();
		foreach (AutoAdjustLod item2 in addingAdjusterSet)
		{
			adjusterSet.Add(item2);
		}
		addingAdjusterSet.Clear();
		foreach (AutoAdjustLod item3 in adjusterSet)
		{
			if (item3 != null)
			{
				item3.UpdateLod(curLod);
			}
		}
	}

	public Dictionary<string, LodConfig> GetLodConfigs(int lodType)
	{
		if (!lodConfigCache.ContainsKey(lodType))
		{
			Dictionary<string, LodConfig> dictionary = new Dictionary<string, LodConfig>();
			LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("CSharpCallLuaInterface.GetLodTemplates", lodType);
			for (int i = 1; i <= luaTable.Length; i++)
			{
				LodConfig lodConfig = new LodConfig();
				LuaTable template = luaTable.Get<LuaTable>(i);
				lodConfig.InitByTemplate(template);
				dictionary[lodConfig.path] = lodConfig;
			}
			lodConfigCache[lodType] = dictionary;
		}
		return lodConfigCache[lodType];
	}
}
