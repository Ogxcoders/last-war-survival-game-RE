using System;
using System.Collections.Generic;

namespace ParkourStage.Runtime.Data;

public static class StageDataParamConvert
{
	public static void ConvertByRowDatas(in StageData stageData, List<RowData> rowDatas)
	{
		if (stageData == null)
		{
			return;
		}
		for (int i = 0; i < rowDatas.Count; i++)
		{
			RowData rowData = rowDatas[i];
			if (string.Equals(rowData.ColumnName, "id"))
			{
				stageData.StageId = rowData.ColumnValue;
				long.TryParse(stageData.StageId, out var result);
				stageData.StageMonsterBornBasicId = result * 1000;
			}
			if (string.Equals(rowData.ColumnName, "scene"))
			{
				ConvertScene(rowData.ColumnValue, out var scenes);
				stageData.SceneIds = scenes;
			}
			if (string.Equals(rowData.ColumnName, "scene_tail"))
			{
				ConvertSceneTrail(rowData.ColumnValue, out var sceneTrail);
				stageData.SceneTrailId = sceneTrail;
			}
			if (string.Equals(rowData.ColumnName, "farm_monster") && !string.IsNullOrEmpty(rowData.ColumnValue))
			{
				ConvertFarmMonster(rowData.ColumnValue, out var farmMonsters);
				stageData.MonsterBornIds = farmMonsters;
			}
		}
	}

	public static void ConvertScene(string value, out string[] scenes)
	{
		if (string.IsNullOrEmpty(value))
		{
			scenes = null;
			return;
		}
		scenes = value.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
	}

	public static void ConvertSceneTrail(string value, out string sceneTrail)
	{
		sceneTrail = value;
	}

	public static void ConvertFarmMonster(string value, out string[] farmMonsters)
	{
		if (string.IsNullOrEmpty(value))
		{
			farmMonsters = null;
			return;
		}
		farmMonsters = value.Split(new char[1] { '|' }, StringSplitOptions.RemoveEmptyEntries);
	}

	public static void ConvertDataToRowData(string farmMonsterParam, string sceneParam, string sceneTrail, in List<RowData> rowDatas)
	{
		for (int i = 0; i < rowDatas.Count; i++)
		{
			RowData rowData = rowDatas[i];
			if (string.Equals(rowData.ColumnName, "farm_monster"))
			{
				rowDatas[i] = new RowData
				{
					ColumnName = rowData.ColumnName,
					ColumnValue = farmMonsterParam
				};
			}
			else if (string.Equals(rowData.ColumnName, "scene"))
			{
				rowDatas[i] = new RowData
				{
					ColumnName = rowData.ColumnName,
					ColumnValue = sceneParam
				};
			}
			else if (string.Equals(rowData.ColumnName, "scene_tail"))
			{
				rowDatas[i] = new RowData
				{
					ColumnName = rowData.ColumnName,
					ColumnValue = sceneTrail
				};
			}
		}
	}
}
