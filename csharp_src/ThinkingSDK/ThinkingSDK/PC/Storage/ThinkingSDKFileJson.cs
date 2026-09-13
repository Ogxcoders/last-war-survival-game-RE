using System;
using System.Collections.Generic;
using ThinkingSDK.PC.Utils;
using UnityEngine;

namespace ThinkingSDK.PC.Storage;

public class ThinkingSDKFileJson
{
	internal static int EnqueueTrackingData(Dictionary<string, object> data, string prefix)
	{
		string key = (string)(data["id"] = prefix + "Event" + EventAutoIncrementingID(prefix));
		PlayerPrefs.SetString(key, ThinkingSDKJSON.Serialize(data));
		IncreaseTrackingDataID(prefix);
		return EventAutoIncrementingID(prefix) - EventIndexID(prefix);
	}

	internal static int EventAutoIncrementingID(string prefix)
	{
		string key = prefix + "EventAutoIncrementingID";
		if (!PlayerPrefs.HasKey(key))
		{
			return 0;
		}
		return PlayerPrefs.GetInt(key);
	}

	private static void IncreaseTrackingDataID(string prefix)
	{
		int num = EventAutoIncrementingID(prefix);
		num++;
		PlayerPrefs.SetInt(prefix + "EventAutoIncrementingID", num);
	}

	internal static int EventIndexID(string prefix)
	{
		string key = prefix + "EventIndexID";
		if (!PlayerPrefs.HasKey(key))
		{
			return 0;
		}
		return PlayerPrefs.GetInt(key);
	}

	private static void SaveEventIndexID(int indexID, string prefix)
	{
		PlayerPrefs.SetInt(prefix + "EventIndexID", indexID);
	}

	internal static List<Dictionary<string, object>> DequeueBatchTrackingData(int batchSize, string prefix)
	{
		List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		int num = EventIndexID(prefix);
		int num2 = EventAutoIncrementingID(prefix) - 1;
		while (list.Count < batchSize && num <= num2)
		{
			string text = prefix + "Event" + num;
			if (PlayerPrefs.HasKey(text))
			{
				try
				{
					Dictionary<string, object> dictionary = ThinkingSDKJSON.Deserialize(PlayerPrefs.GetString(text));
					dictionary.Remove("id");
					list.Add(dictionary);
				}
				catch (Exception ex)
				{
					ThinkingSDKLogger.Print("There was an error processing " + text + " from the internal object pool: " + ex);
					PlayerPrefs.DeleteKey(text);
				}
			}
			num++;
		}
		return list;
	}

	internal static int DeleteBatchTrackingData(int batchSize, string prefix)
	{
		int num = 0;
		int num2 = EventIndexID(prefix);
		int num3 = EventAutoIncrementingID(prefix) - 1;
		while (num < batchSize && num2 <= num3)
		{
			string key = prefix + "Event" + num2;
			if (PlayerPrefs.HasKey(key))
			{
				PlayerPrefs.DeleteKey(key);
				num++;
			}
			num2++;
		}
		SaveEventIndexID(num2, prefix);
		return EventAutoIncrementingID(prefix) - EventIndexID(prefix);
	}

	internal static int DeleteAllTrackingData(string prefix)
	{
		DeleteBatchTrackingData(int.MaxValue, prefix);
		SaveEventIndexID(0, prefix);
		return 0;
	}
}
