using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParkourStage.Runtime.Data;

public static class TriggerDataParamConvert
{
	public static void ConvertByRowDatas(in TriggerData triggerData, in List<RowData> rowDatas, bool checkValid)
	{
		string value = string.Empty;
		foreach (RowData rowData in rowDatas)
		{
			if (string.Equals(rowData.ColumnName, "id", StringComparison.CurrentCultureIgnoreCase))
			{
				triggerData.TriggerId = rowData.ColumnValue;
			}
			else if (string.Equals(rowData.ColumnName, "type") && !string.IsNullOrEmpty(rowData.ColumnValue))
			{
				triggerData.TriggerType = ConvertTriggerEventType(rowData.ColumnValue);
			}
			else if (string.Equals(rowData.ColumnName, "para") && !string.IsNullOrEmpty(rowData.ColumnValue))
			{
				value = rowData.ColumnValue;
			}
		}
		if (!string.IsNullOrEmpty(value) && !ConvertPara(in triggerData, value, out var msg))
		{
			Debug.LogError(msg);
		}
	}

	public static TriggerEventType ConvertTriggerEventType(string value)
	{
		int.TryParse(value, out var result);
		return (TriggerEventType)result;
	}

	public static bool ConvertPara(in TriggerData triggerData, string value, out string msg)
	{
		msg = string.Empty;
		if (string.IsNullOrEmpty(value))
		{
			msg = "参数为空!";
			return false;
		}
		TriggerEventType triggerType = triggerData.TriggerType;
		if (triggerType == TriggerEventType.AddEnergy)
		{
			triggerData.AddEnergyIndex = ConvertAddEnergyIndex(value);
		}
		triggerData.Para = value;
		return true;
	}

	public static int ConvertAddEnergyIndex(string value)
	{
		int.TryParse(value, out var result);
		return result;
	}
}
