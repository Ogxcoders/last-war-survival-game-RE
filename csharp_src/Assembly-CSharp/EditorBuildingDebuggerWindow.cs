using System.Collections.Generic;
using System.Linq;
using GameKit.Base;
using UnityEngine;

public class EditorBuildingDebuggerWindow : DebuggerWindow.ScrollableDebuggerWindowBase
{
	private const int MAIN_CITY = 0;

	private const int EFFECT = 1;

	private const int LABEL = 2;

	private const int COLORFUL = 3;

	private GUIStyle buttonStyle;

	private GUIStyle labelStyle;

	private int selectedModel = 9999;

	private int selectedEffect = 9999;

	private int selectedLabel = 9999;

	private int selectedColorful = 9999;

	private string[] selectedModelOpts;

	private string[] selectedEffectOpts;

	private string[] selectedLabelOpts;

	private string[] selectedColorfulOpts;

	private int selectedTab;

	private static string[] selectedTabOpts = new string[4] { "基地", "特效", "铭牌", "炫彩" };

	private DebugLODStrategy debugLODStrategy;

	public override void Initialize(params object[] args)
	{
	}

	public override void OnEnter()
	{
	}

	public override void OnLeave()
	{
	}

	protected override void OnDrawScrollableWindow()
	{
		InitStyle();
		GUILayout.Label($"<b>Skin:{GetSkinID()} Effect:{GetEffectID()} Label:{GetLabelID()} Colorfull:{GetColoruflID()}</b>", labelStyle);
		GUILayout.BeginHorizontal("box");
		if (GUILayout.Button("单一铺满", buttonStyle))
		{
			PutBuildingFullScreenSingle();
		}
		if (GUILayout.Button("随机铺满", buttonStyle))
		{
			PutBuildingFullScreenRandom();
		}
		if (GUILayout.Button("单一建筑", buttonStyle))
		{
			PutBuildingOne();
		}
		if (GUILayout.Button("清理地图", buttonStyle))
		{
			ClearAllBuildings();
		}
		GUILayout.EndHorizontal();
		if (debugLODStrategy == null)
		{
			if (GUILayout.Button("特效 AutoLOD", buttonStyle))
			{
				SingletonBehaviour<SceneLODManager>.Instance.ClearStrategies(LODType.Effect);
				debugLODStrategy = new DebugLODStrategy();
				SingletonBehaviour<SceneLODManager>.Instance.AddStrategy(LODType.Effect, debugLODStrategy);
				debugLODStrategy.lod = 0;
			}
		}
		else if (debugLODStrategy.lod == 0)
		{
			if (GUILayout.Button("特效 LOD:0", buttonStyle))
			{
				debugLODStrategy.lod = 1;
			}
		}
		else if (debugLODStrategy.lod == 1)
		{
			if (GUILayout.Button("特效 LOD:1", buttonStyle))
			{
				debugLODStrategy.lod = 2;
			}
		}
		else if (debugLODStrategy.lod == 2)
		{
			if (GUILayout.Button("特效 LOD:2", buttonStyle))
			{
				debugLODStrategy.lod = 3;
			}
		}
		else if (debugLODStrategy.lod == 3)
		{
			if (GUILayout.Button("特效 LOD:3", buttonStyle))
			{
				debugLODStrategy.lod = 4;
			}
		}
		else if (debugLODStrategy.lod == 4 && GUILayout.Button("特效 LOD:4", buttonStyle))
		{
			debugLODStrategy = null;
			SingletonBehaviour<SceneLODManager>.Instance.ClearStrategies(LODType.Effect);
			GameEntry.LOD.Initialize();
		}
		GUILayout.BeginHorizontal("box");
		if (GUILayout.Button("LOD1", buttonStyle))
		{
			EditorSceneBuilding.Instance.Camera.Zoom = 150f;
		}
		if (GUILayout.Button("LOD2", buttonStyle))
		{
			EditorSceneBuilding.Instance.Camera.Zoom = 250f;
		}
		if (GUILayout.Button("LOD3", buttonStyle))
		{
			EditorSceneBuilding.Instance.Camera.Zoom = 400f;
		}
		if (GUILayout.Button("LOD4", buttonStyle))
		{
			EditorSceneBuilding.Instance.Camera.Zoom = 600f;
		}
		if (GUILayout.Button("LOD5", buttonStyle))
		{
			EditorSceneBuilding.Instance.Camera.Zoom = 1200f;
		}
		if (GUILayout.Button("LOD5", buttonStyle))
		{
			EditorSceneBuilding.Instance.Camera.Zoom = 2200f;
		}
		GUILayout.EndHorizontal();
		selectedTab = GUILayout.SelectionGrid(selectedTab, selectedTabOpts, 4, buttonStyle);
		if (selectedTab == 0)
		{
			selectedModel = SelectedGrid(selectedModel, ref selectedModelOpts, EditorModelManager.ModelNames);
		}
		else if (selectedTab == 1)
		{
			selectedEffect = SelectedGrid(selectedEffect, ref selectedEffectOpts, EditorModelManager.EffectNames);
		}
		else if (selectedTab == 2)
		{
			selectedLabel = SelectedGrid(selectedLabel, ref selectedLabelOpts, EditorModelManager.LabelNames);
		}
		else if (selectedTab == 3)
		{
			selectedColorful = SelectedGrid(selectedColorful, ref selectedColorfulOpts, EditorModelManager.ColorfulNames);
		}
	}

	private void InitStyle()
	{
		if (buttonStyle == null)
		{
			buttonStyle = new GUIStyle(GUI.skin.button);
			buttonStyle.fontSize = 30;
			buttonStyle.fixedHeight = 50f;
			labelStyle = new GUIStyle(GUI.skin.label);
			labelStyle.fontSize = 25;
			labelStyle.fixedHeight = 40f;
		}
	}

	private void PutBuildingFullScreenSingle()
	{
		ClearAllBuildings();
		int num = 500500;
		for (int i = -10; i <= 10; i++)
		{
			for (int j = -10; j <= 10; j++)
			{
				int buildingIndex = num + 3 * i + 3 * j * 1000;
				BuildPointInfo building = CreateBuildPointInfo(buildingIndex);
				EditorSceneBuilding.Instance.EditorPoint.AddPlayerBuilding(building);
			}
		}
		EditorSceneBuilding.Instance.FocusWorldIndex(num);
	}

	private void PutBuildingFullScreenRandom()
	{
		ClearAllBuildings();
	}

	private void PutBuildingOne()
	{
		ClearAllBuildings();
		int num = 500500;
		BuildPointInfo building = CreateBuildPointInfo(num);
		EditorSceneBuilding.Instance.EditorPoint.AddPlayerBuilding(building);
		EditorSceneBuilding.Instance.FocusWorldIndex(num);
	}

	private void ClearAllBuildings()
	{
		EditorSceneBuilding.Instance.EditorPoint.ClearPlayerBuildings();
	}

	private BuildPointInfo CreateBuildPointInfo(int buildingIndex)
	{
		int skinID = GetSkinID();
		int effectID = GetEffectID();
		int coloruflID = GetColoruflID();
		int labelID = GetLabelID();
		return EditorMockData.CreateBuildPointInfo(buildingIndex, skinID, effectID, coloruflID, labelID);
	}

	private int GetSkinID()
	{
		return GetSkinIDImpl(selectedModel, EditorModelManager.ModelNames);
	}

	private int GetEffectID()
	{
		return GetSkinIDImpl(selectedEffect, EditorModelManager.EffectNames);
	}

	private int GetLabelID()
	{
		return GetSkinIDImpl(selectedLabel, EditorModelManager.LabelNames);
	}

	private int GetColoruflID()
	{
		return GetSkinIDImpl(selectedColorful, EditorModelManager.ColorfulNames);
	}

	private int GetSkinIDImpl(int selected, List<(int id, string path)> skins)
	{
		if (selected >= skins.Count)
		{
			return 0;
		}
		return skins[selected].id;
	}

	private int SelectedGrid(int selected, ref string[] opts, List<(int id, string path)> tables)
	{
		if (opts == null || opts.Length == 0)
		{
			List<string> list = new List<string>(tables.Select(((int id, string path) v) => $"[{v.id}]{v.path}"));
			list.Add("[0]None");
			opts = list.ToArray();
		}
		return GUILayout.SelectionGrid(selected, opts, 1, buttonStyle);
	}
}
