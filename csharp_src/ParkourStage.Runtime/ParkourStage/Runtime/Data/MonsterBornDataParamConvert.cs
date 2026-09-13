using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ParkourStage.Runtime.Data;

public static class MonsterBornDataParamConvert
{
	private static readonly List<RowData> ms_TmpRowDataList = new List<RowData>();

	private static StringBuilder ms_StringBuilder = new StringBuilder();

	public static void ConvertByRowDatas(in MonsterBornData monsterBornData, in List<RowData> rowDatas, IDataLoader monsterDataLoader, IDataLoader triggerDataLoader, bool checkValid)
	{
		if (rowDatas == null)
		{
			return;
		}
		string value = string.Empty;
		string value2 = string.Empty;
		string value3 = string.Empty;
		string value4 = string.Empty;
		string value5 = string.Empty;
		for (int i = 0; i < rowDatas.Count; i++)
		{
			RowData rowData = rowDatas[i];
			if (string.Equals(rowData.ColumnName, "id", StringComparison.CurrentCultureIgnoreCase))
			{
				monsterBornData.MonsterBornId = rowData.ColumnValue;
			}
			else if (string.Equals(rowData.ColumnName, "coord"))
			{
				monsterBornData.Coord = ConvertCoord(rowData.ColumnValue);
			}
			else if (string.Equals(rowData.ColumnName, "type"))
			{
				monsterBornData.BornType = ConvertBornType(rowData.ColumnValue);
			}
			else if (string.Equals(rowData.ColumnName, "para"))
			{
				value = rowData.ColumnValue;
			}
			else if (string.Equals(rowData.ColumnName, "monster"))
			{
				value2 = rowData.ColumnValue;
			}
			else if (string.Equals(rowData.ColumnName, "buff_item"))
			{
				value3 = rowData.ColumnValue;
			}
			else if (string.Equals(rowData.ColumnName, "view_Param"))
			{
				value4 = rowData.ColumnValue;
			}
			else if (string.Equals(rowData.ColumnName, "move_Param"))
			{
				value5 = rowData.ColumnValue;
			}
			else if (string.IsNullOrEmpty(rowData.ColumnName) && i == 1)
			{
				monsterBornData.Desc = rowData.ColumnValue;
			}
		}
		if (!string.IsNullOrEmpty(value) && !ConvertPara(in monsterBornData, value, out var msg))
		{
			Debug.LogError(msg);
		}
		if (!string.IsNullOrEmpty(value2) && !ConvertMonsterParam(in monsterBornData, value2, monsterDataLoader, triggerDataLoader, checkValid, out var msg2))
		{
			Debug.LogError(msg2);
		}
		if (!string.IsNullOrEmpty(value3) && !ConvertBuffParam(in monsterBornData, value3, triggerDataLoader, checkValid, out var msg3))
		{
			Debug.LogError(msg3);
		}
		if (!string.IsNullOrEmpty(value4) && !ConvertViewParam(in monsterBornData, value4, out var msg4))
		{
			Debug.LogError(msg4);
		}
		if (!string.IsNullOrEmpty(value5) && !ConvertMoveParam(in monsterBornData, value5, out var msg5))
		{
			Debug.LogError(msg5);
		}
	}

	public static bool ConvertPara(in MonsterBornData monsterBornData, string value, out string msg)
	{
		msg = string.Empty;
		if (string.IsNullOrEmpty(value))
		{
			msg = "参数为空!";
			return false;
		}
		switch (monsterBornData.BornType)
		{
		case MonsterBornType.TriggerLineArea:
		{
			string[] array2 = value.Split(new char[1] { '|' });
			for (int j = 0; j < array2.Length; j++)
			{
				string value3 = array2[j];
				switch (j)
				{
				case 0:
					monsterBornData.GenZ = ConvertGenZ(value3);
					break;
				case 1:
					monsterBornData.GenNumber = ConvertGenNumber(value3);
					break;
				case 2:
					monsterBornData.GenDelta = ConvertGenDelta(value3);
					break;
				case 3:
					monsterBornData.R1 = ConvertR(value3);
					break;
				case 4:
					monsterBornData.R2 = ConvertR(value3);
					break;
				}
			}
			break;
		}
		case MonsterBornType.TriggerLinePoint:
		{
			string[] array = value.Split(new char[1] { '|' });
			for (int i = 0; i < array.Length; i++)
			{
				string value2 = array[i];
				switch (i)
				{
				case 0:
					monsterBornData.GenZ = ConvertGenZ(value2);
					break;
				case 1:
					monsterBornData.GenNumber = ConvertGenNumber(value2);
					break;
				case 2:
					monsterBornData.GenDelta = ConvertGenDelta(value2);
					break;
				}
			}
			break;
		}
		}
		monsterBornData.ParaParams = value;
		return true;
	}

	public static bool ConvertMonsterParam(in MonsterBornData monsterBornData, string value, IDataLoader monsterDataLoader, IDataLoader triggerDataLoader, bool checkValid, out string msg)
	{
		msg = string.Empty;
		if (string.IsNullOrEmpty(value))
		{
			msg = "monsterParams 参数为空!";
			return false;
		}
		int num = 0;
		string text = string.Empty;
		string[] array = value.Split(new char[1] { ',' });
		for (int i = 0; i < array.Length; i++)
		{
			if (!ConvertSingleMonster(array[i], out var monsterId, out var weight, out msg))
			{
				return false;
			}
			num += weight;
			if (checkValid && monsterDataLoader.ContainsId(monsterId) == -1)
			{
				msg = "monsterParams MonsterId:" + monsterId + "不存在";
				return false;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = monsterId;
			}
		}
		if (checkValid && num != 10000)
		{
			msg = "monsterParams 权重之和不等于10000";
			return false;
		}
		if (monsterDataLoader.ReadRowAllDataById(text, in ms_TmpRowDataList, out var _))
		{
			if (monsterBornData.ShowedMonsterData == null)
			{
				monsterBornData.ShowedMonsterData = new MonsterData();
			}
			MonsterDataParamConvert.ConvertByRowDatas(in monsterBornData.ShowedMonsterData, in ms_TmpRowDataList, triggerDataLoader, checkValid: true);
		}
		monsterBornData.MonsterParams = value;
		return true;
	}

	public static bool ConvertBuffParam(in MonsterBornData monsterBornData, string value, IDataLoader triggerDataLoader, bool checkValid, out string msg)
	{
		msg = string.Empty;
		if (string.IsNullOrEmpty(value))
		{
			msg = "buffParams 参数为空!";
			return false;
		}
		int num = 0;
		string text = string.Empty;
		string[] array = value.Split(new char[1] { ',' });
		for (int i = 0; i < array.Length; i++)
		{
			if (!ConvertSingleBuff(array[i], out var buffId, out var weight, out msg))
			{
				return false;
			}
			num += weight;
			if (checkValid && triggerDataLoader.ContainsId(buffId) == -1)
			{
				msg = "buffParams triggerId:" + buffId + "不存在";
				return false;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = buffId;
			}
		}
		if (checkValid && num != 10000)
		{
			msg = "buffParams 权重之和不等于10000";
			return false;
		}
		if (triggerDataLoader.ReadRowAllDataById(text, in ms_TmpRowDataList, out var _))
		{
			foreach (RowData ms_TmpRowData in ms_TmpRowDataList)
			{
				if (string.Equals(ms_TmpRowData.ColumnName, "effect") && !string.IsNullOrEmpty(ms_TmpRowData.ColumnValue))
				{
					monsterBornData.ShowedBuffPath = ms_TmpRowData.ColumnValue;
				}
			}
		}
		monsterBornData.BuffParams = value;
		return true;
	}

	public static bool ConvertViewParam(in MonsterBornData monsterBornData, string value, out string msg)
	{
		msg = string.Empty;
		if (string.IsNullOrEmpty(value))
		{
			msg = "ViewParam 参数为空!";
			return false;
		}
		string[] array = value.Split(new char[1] { ',' });
		if (array.Length != 0)
		{
			monsterBornData.ViewType = ConvertViewRuleType(array[0]);
		}
		if (array.Length > 1)
		{
			monsterBornData.ViewOffset = ConvertViewZOffset(array[1]);
		}
		return true;
	}

	public static bool ConvertMoveParam(in MonsterBornData monsterBornData, string value, out string msg)
	{
		msg = string.Empty;
		if (string.IsNullOrEmpty(value))
		{
			msg = "MoveParam 参数为空!";
			return false;
		}
		string[] array = value.Split(new char[1] { ';' });
		if (array.Length != 0)
		{
			monsterBornData.MoveType = ConvertMoveRuleType(array[0]);
		}
		if (array.Length > 1)
		{
			monsterBornData.MoveCenterOffset = ConvertMoveCenterOffset(array[1]);
		}
		if (array.Length > 2)
		{
			if (monsterBornData.MoveType == MoveRuleType.EllipseLike)
			{
				monsterBornData.MoveEllipseLikeAScaleFactor = CovertMoveEllipseLikeAScaleFactor(array[2]);
			}
			monsterBornData.MoveExtraParam = array[2];
		}
		if (array.Length > 3)
		{
			monsterBornData.MoveDirectionFactor = ConvertMoveDirection(array[3]);
		}
		return true;
	}

	public static Vector3 ConvertCoord(string value)
	{
		float result = 0f;
		float result2 = 0f;
		float result3 = 0f;
		if (!string.IsNullOrEmpty(value))
		{
			string[] array = value.Split(new char[1] { ',' });
			if (array.Length == 2)
			{
				float.TryParse(array[0], out result);
				float.TryParse(array[1], out result3);
			}
			else if (array.Length == 3)
			{
				float.TryParse(array[0], out result);
				float.TryParse(array[1], out result2);
				float.TryParse(array[2], out result3);
			}
		}
		return new Vector3(result, result2, result3);
	}

	public static MonsterBornType ConvertBornType(string value)
	{
		int.TryParse(value, out var result);
		return (MonsterBornType)result;
	}

	public static float ConvertGenZ(string value)
	{
		float.TryParse(value, out var result);
		return result;
	}

	public static int ConvertGenNumber(string value)
	{
		int.TryParse(value, out var result);
		return result;
	}

	public static int ConvertGenDelta(string value)
	{
		int.TryParse(value, out var result);
		return result;
	}

	public static float ConvertR(string value)
	{
		float.TryParse(value, out var result);
		return result;
	}

	public static int ConvertGenWeight(string value)
	{
		int.TryParse(value, out var result);
		return Mathf.Clamp(result, 0, 10000);
	}

	public static ViewRuleType ConvertViewRuleType(string value)
	{
		int.TryParse(value, out var result);
		return (ViewRuleType)result;
	}

	public static float ConvertViewZOffset(string value)
	{
		float.TryParse(value, out var result);
		return result;
	}

	public static MoveRuleType ConvertMoveRuleType(string value)
	{
		int.TryParse(value, out var result);
		return (MoveRuleType)result;
	}

	public static Vector2 ConvertMoveCenterOffset(string value)
	{
		string[] array = value.Split(new char[1] { ',' });
		float result = 0f;
		float result2 = 0f;
		if (array.Length != 0)
		{
			float.TryParse(array[0], out result);
		}
		if (array.Length > 1)
		{
			float.TryParse(array[1], out result2);
		}
		return new Vector2(result, result2);
	}

	public static float CovertMoveEllipseLikeAScaleFactor(string value)
	{
		float.TryParse(value, out var result);
		return result;
	}

	public static int ConvertMoveDirection(string value)
	{
		int.TryParse(value, out var result);
		return (result > 0) ? 1 : (-1);
	}

	public static bool ConvertSingleMonster(string value, out string monsterId, out int weight, out string msg)
	{
		monsterId = string.Empty;
		weight = 0;
		msg = string.Empty;
		string[] array = value.Split(new char[1] { '|' });
		if (array.Length != 2)
		{
			msg = "monsterParams 怪物权重格式错误,格式：怪物|权重,怪物|权重";
			return false;
		}
		if (!string.IsNullOrEmpty(array[0]))
		{
			monsterId = array[0];
		}
		if (!string.IsNullOrEmpty(array[1]))
		{
			weight = ConvertGenWeight(array[1]);
		}
		return true;
	}

	public static bool ConvertSingleBuff(string value, out string buffId, out int weight, out string msg)
	{
		buffId = string.Empty;
		weight = 0;
		msg = string.Empty;
		string[] array = value.Split(new char[1] { '|' });
		if (array.Length != 2)
		{
			msg = "buffParams 权重格式错误,格式：buff|权重,buff|权重";
			return false;
		}
		if (!string.IsNullOrEmpty(array[0]))
		{
			buffId = array[0];
		}
		if (!string.IsNullOrEmpty(array[1]))
		{
			weight = ConvertGenWeight(array[1]);
		}
		return true;
	}

	public static string ConvertMonsterBornListToParam(List<MonsterBornEditData> monsterBornEditDataList)
	{
		ms_StringBuilder.Clear();
		if (monsterBornEditDataList == null)
		{
			return string.Empty;
		}
		int count = monsterBornEditDataList.Count;
		for (int i = 0; i < count; i++)
		{
			MonsterBornEditData monsterBornEditData = monsterBornEditDataList[i];
			ms_StringBuilder.Append(monsterBornEditData.MonsterBornId);
			if (i != count - 1)
			{
				ms_StringBuilder.Append("|");
			}
		}
		return ms_StringBuilder.ToString();
	}

	public static void ConvertDataToRowData(in MonsterBornData data, in List<RowData> rowDataList, bool coordV3 = false)
	{
		for (int i = 0; i < rowDataList.Count; i++)
		{
			RowData value = rowDataList[i];
			if (string.Equals(value.ColumnName, "id"))
			{
				value.ColumnValue = data.MonsterBornId;
			}
			else if (string.Equals(value.ColumnName, "coord"))
			{
				string empty = string.Empty;
				empty = ((!coordV3) ? (data.Coord.x.ToString("#0.00") + "," + data.Coord.z.ToString("#0.00")) : (data.Coord.x.ToString("#0.00") + "," + data.Coord.y.ToString("#0.00") + "," + data.Coord.z.ToString("#0.00")));
				value.ColumnValue = empty;
			}
			else if (string.Equals(value.ColumnName, "type"))
			{
				value.ColumnValue = $"{(int)data.BornType}";
			}
			else if (string.Equals(value.ColumnName, "para"))
			{
				switch (data.BornType)
				{
				case MonsterBornType.SingleMonster:
				case MonsterBornType.BuffBall:
				case MonsterBornType.Resource:
				{
					int num = 0;
					string text = (string.IsNullOrEmpty(data.MonsterParams) ? data.BuffParams : data.MonsterParams);
					if (!string.IsNullOrEmpty(text))
					{
						string[] array = text.Split(new char[1] { ',' });
						foreach (string text2 in array)
						{
							if (!string.IsNullOrEmpty(text2))
							{
								string[] array2 = text2.Split(new char[1] { '|' });
								if (array2.Length >= 2)
								{
									int.TryParse(array2[1], out var result);
									num += result;
								}
							}
						}
					}
					value.ColumnValue = $"{num}";
					break;
				}
				case MonsterBornType.TriggerLinePoint:
					value.ColumnValue = $"{data.GenZ}|{data.GenNumber}|{data.GenDelta}";
					break;
				case MonsterBornType.TriggerLineArea:
					value.ColumnValue = $"{data.GenZ}|{data.GenNumber}|{data.GenDelta}|{data.R1}|{data.R2}";
					break;
				default:
					value.ColumnValue = data.ParaParams;
					break;
				}
			}
			else if (string.Equals(value.ColumnName, "monster"))
			{
				value.ColumnValue = data.MonsterParams;
			}
			else if (string.Equals(value.ColumnName, "buff_item"))
			{
				value.ColumnValue = data.BuffParams;
			}
			else if (string.Equals(value.ColumnName, "view_Param"))
			{
				value.ColumnValue = $"{(int)data.ViewType},{data.ViewOffset}";
			}
			else if (string.Equals(value.ColumnName, "move_Param"))
			{
				string text3 = data.MoveExtraParam;
				if (data.MoveType == MoveRuleType.EllipseLike)
				{
					text3 = $"{data.MoveEllipseLikeAScaleFactor}";
				}
				value.ColumnValue = $"{(int)data.MoveType};{data.MoveCenterOffset.x},{data.MoveCenterOffset.y};{text3};{data.MoveDirectionFactor}";
			}
			else if (string.IsNullOrEmpty(value.ColumnName) && i == 1)
			{
				value.ColumnValue = data.Desc;
			}
			rowDataList[i] = value;
		}
	}
}
