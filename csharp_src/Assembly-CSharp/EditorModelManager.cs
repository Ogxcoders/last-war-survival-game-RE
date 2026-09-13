using System.Collections.Generic;
using XLua;

public class EditorModelManager : WorldManagerBase
{
	private static List<(int id, string path)> modelPaths = new List<(int, string)>();

	private static List<(int id, string path)> effectPaths = new List<(int, string)>();

	private static List<(int id, string path)> labelPaths = new List<(int, string)>();

	private static List<(int id, string path)> colorfulPaths = new List<(int, string)>();

	public static List<(int id, string path)> ModelNames
	{
		get
		{
			if (modelPaths == null || modelPaths.Count == 0)
			{
				InitModels();
			}
			return modelPaths;
		}
	}

	public static List<(int id, string path)> EffectNames
	{
		get
		{
			if (effectPaths == null || effectPaths.Count == 0)
			{
				InitEffects();
			}
			return effectPaths;
		}
	}

	public static List<(int id, string path)> LabelNames
	{
		get
		{
			if (labelPaths == null || labelPaths.Count == 0)
			{
				InitLabels();
			}
			return labelPaths;
		}
	}

	public static List<(int id, string path)> ColorfulNames
	{
		get
		{
			if (colorfulPaths == null || colorfulPaths.Count == 0)
			{
				InitColorful();
			}
			return colorfulPaths;
		}
	}

	public EditorModelManager(WorldScene scene)
		: base(scene)
	{
	}

	public void CreateModel(int idx, int point)
	{
		string prefabPath = $"Assets/Main/Prefabs/Building/{modelPaths[idx]}.prefab";
		if (modelPaths[idx].path.StartsWith("Assets/"))
		{
			prefabPath = modelPaths[idx].path;
		}
		InstanceRequest request = GameEntry.Resource.InstantiateAsync(prefabPath);
		request.completed += delegate
		{
			request.gameObject.GetComponent<WorldBuilding>().CSInit(new WorldBuilding.Param
			{
				buildId = idx,
				buildUuid = 0L,
				point = point,
				BuildTopType = PlaceBuildType.None,
				noPutPoint = null,
				buildSceneType = WorldBuilding.BuildSceneType.Fake
			});
		};
	}

	public override void Init()
	{
		InitModels();
	}

	public override void OnUpdate(float deltaTime)
	{
	}

	public override void UnInit()
	{
	}

	private static void InitModels()
	{
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("DataCenter.DecorationTemplateManager:GetTypeDecorations", 1);
		for (int i = 1; i <= luaTable.Length; i++)
		{
			int num = luaTable.Get<int>(i);
			string text = string.Empty;
			if (text.IsNullOrEmpty())
			{
				text = GameEntry.ConfigCache.GetTemplateData("lw_decoration", num, "model_world_path");
			}
			if (text.IsNullOrEmpty())
			{
				text = GameEntry.ConfigCache.GetTemplateData("lw_decoration", num, "model_world");
			}
			if (!text.IsNullOrEmpty())
			{
				modelPaths.Add((num, text));
			}
		}
	}

	private static void InitEffects()
	{
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("DataCenter.DecorationTemplateManager:GetTypeDecorations", 4);
		for (int i = 1; i <= luaTable.Length; i++)
		{
			int num = luaTable.Get<int>(i);
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_decoration", num, "model_world");
			if (!templateData.IsNullOrEmpty())
			{
				effectPaths.Add((num, templateData));
			}
		}
	}

	private static void InitLabels()
	{
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable, int>("DataCenter.DecorationTemplateManager:GetTypeDecorations", 3);
		for (int i = 1; i <= luaTable.Length; i++)
		{
			int num = luaTable.Get<int>(i);
			string templateData = GameEntry.ConfigCache.GetTemplateData("lw_decoration", num, "image");
			if (!templateData.IsNullOrEmpty())
			{
				labelPaths.Add((num, templateData));
			}
		}
	}

	private static void InitColorful()
	{
		LuaTable luaTable = GameEntry.Lua.CallWithReturn<LuaTable>("DataCenter.DecorationDazzleManager:GetAllDazzleSkins");
		for (int i = 1; i < luaTable.Length; i++)
		{
			int num = luaTable.Get<int>(i);
			string templateData = GameEntry.ConfigCache.GetTemplateData("decoration_colorful_skin", num, "image");
			colorfulPaths.Add((num, templateData));
		}
	}
}
