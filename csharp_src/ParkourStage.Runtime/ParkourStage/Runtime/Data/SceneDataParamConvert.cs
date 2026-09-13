using System.Collections.Generic;
using System.Text;

namespace ParkourStage.Runtime.Data;

public static class SceneDataParamConvert
{
	private const string SCENE_PREFAB_PATH = "Assets/Main/Prefabs/PVELevel/{0}/scene.prefab";

	private static StringBuilder ms_StringBuilder = new StringBuilder();

	public static string ConvertSceneListToParam(List<SceneEditData> sceneEditDataList)
	{
		if (sceneEditDataList.Count < 2)
		{
			return string.Empty;
		}
		ms_StringBuilder.Clear();
		for (int i = 0; i < sceneEditDataList.Count - 1; i++)
		{
			SceneEditData sceneEditData = sceneEditDataList[i];
			ms_StringBuilder.Append(sceneEditData.SceneId);
			if (i != sceneEditDataList.Count - 2)
			{
				ms_StringBuilder.Append(",");
			}
		}
		return ms_StringBuilder.ToString();
	}

	public static void ConvertByRowDatas(in SceneData sceneData, List<RowData> rowDatas)
	{
		foreach (RowData rowData in rowDatas)
		{
			if (string.Equals(rowData.ColumnName, "id"))
			{
				sceneData.SceneId = rowData.ColumnValue;
			}
			if (string.Equals(rowData.ColumnName, "scene_size"))
			{
				float.TryParse(rowData.ColumnValue, out var result);
				sceneData.Size = result;
			}
			if (string.Equals(rowData.ColumnName, "asset"))
			{
				string scenePrefabPath = $"Assets/Main/Prefabs/PVELevel/{rowData.ColumnValue}/scene.prefab";
				sceneData.ScenePrefabPath = scenePrefabPath;
			}
		}
	}
}
