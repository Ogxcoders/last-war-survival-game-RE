using System;
using System.Collections.Generic;

namespace ParkourStage.Runtime.Data;

public static class MonsterDataParamConvert
{
	private static readonly List<RowData> ms_TmpRowDataList = new List<RowData>();

	public static void ConvertByRowDatas(in MonsterData monsterData, in List<RowData> rowDatas, IDataLoader triggerDataLoader, bool checkValid)
	{
		foreach (RowData rowData in rowDatas)
		{
			if (string.Equals(rowData.ColumnName, "id", StringComparison.CurrentCultureIgnoreCase))
			{
				monsterData.MonsterId = rowData.ColumnValue;
			}
			else if (string.Equals(rowData.ColumnName, "asset") && !string.IsNullOrEmpty(rowData.ColumnValue))
			{
				monsterData.MonsterPath = rowData.ColumnValue;
			}
			else if (string.Equals(rowData.ColumnName, "monster_type") && !string.IsNullOrEmpty(rowData.ColumnValue))
			{
				monsterData.MonsterType = ConvertMonsterType(rowData.ColumnValue);
			}
			else if (string.Equals(rowData.ColumnName, "dynamic_resource") && !string.IsNullOrEmpty(rowData.ColumnValue))
			{
				monsterData.DynamicObjectPath = rowData.ColumnValue;
			}
			else if (string.Equals(rowData.ColumnName, "model_size") && !string.IsNullOrEmpty(rowData.ColumnValue))
			{
				monsterData.Size = ConvertMonsterModelSize(rowData.ColumnValue);
			}
			else if (string.Equals(rowData.ColumnName, "start_hp") && !string.IsNullOrEmpty(rowData.ColumnValue))
			{
				monsterData.DoorNumber = ConvertMonsterHp(rowData.ColumnValue);
			}
			else if (string.Equals(rowData.ColumnName, "property") && !string.IsNullOrEmpty(rowData.ColumnValue))
			{
				ConvertMonsterProperty(in monsterData, rowData.ColumnValue, out var _);
			}
			else if (string.Equals(rowData.ColumnName, "alert_range") && !string.IsNullOrEmpty(rowData.ColumnValue))
			{
				monsterData.AlertRange = ConvertMonsterAlertRange(rowData.ColumnValue);
			}
			else
			{
				if (!string.Equals(rowData.ColumnName, "death_trigger_item") || string.IsNullOrEmpty(rowData.ColumnValue))
				{
					continue;
				}
				string columnValue = rowData.ColumnValue;
				if (triggerDataLoader.ReadRowAllDataById(columnValue, in ms_TmpRowDataList, out var _))
				{
					if (monsterData.DeathTrigger == null)
					{
						monsterData.DeathTrigger = new TriggerData();
					}
					TriggerDataParamConvert.ConvertByRowDatas(in monsterData.DeathTrigger, in ms_TmpRowDataList, checkValid);
				}
			}
		}
	}

	public static MonsterType ConvertMonsterType(string value)
	{
		int.TryParse(value, out var result);
		return (MonsterType)result;
	}

	public static float ConvertMonsterModelSize(string value)
	{
		float.TryParse(value, out var result);
		return result;
	}

	public static int ConvertMonsterHp(string value)
	{
		float.TryParse(value, out var result);
		return (int)result;
	}

	public static float ConvertMonsterAlertRange(string value)
	{
		float.TryParse(value, out var result);
		return result;
	}

	public static bool ConvertMonsterProperty(in MonsterData monsterData, string value, out string msg)
	{
		msg = string.Empty;
		if (string.IsNullOrEmpty(value))
		{
			msg = "Monster Property 参数为空!";
			return false;
		}
		string[] array = value.Split(new char[1] { '|' });
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(new char[1] { ';' });
			if (array2.Length == 2 && string.Equals(array2[0], "50006"))
			{
				float.TryParse(array2[1], out var result);
				monsterData.PropertyHp = (int)result;
				break;
			}
		}
		return true;
	}
}
