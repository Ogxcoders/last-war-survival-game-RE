using System.Collections.Generic;
using Newtonsoft.Json;

namespace RiverBISDK;

public class PackParams
{
	private Dictionary<string, object> _packData;

	public PackParams()
	{
		_packData = new Dictionary<string, object>();
	}

	public PackParams(string info)
	{
		_packData = new Dictionary<string, object>();
		UpdatePackParams(info);
	}

	public PackParams(string eventName, IDictionary<string, object> dataParam)
	{
		InitPackParams(eventName, dataParam, null);
	}

	public PackParams(string eventName, IDictionary<string, object> dataParam, IDictionary<string, object> extParam)
	{
		InitPackParams(eventName, dataParam, extParam);
	}

	public void InitPackParams(string eventName, IDictionary<string, object> dataParam, IDictionary<string, object> extParam)
	{
		_packData = new Dictionary<string, object>();
		if (dataParam != null)
		{
			foreach (KeyValuePair<string, object> item in dataParam)
			{
				_packData.Add(item.Key, item.Value);
			}
		}
		_packData["event"] = eventName;
		IDictionary<string, object> dictionary = new Dictionary<string, object>();
		if (extParam != null)
		{
			foreach (KeyValuePair<string, object> item2 in extParam)
			{
				dictionary.Add(item2.Key, item2.Value);
			}
		}
		dictionary["oltime"] = BIConfig.GetTimeStamp() - BIConfig.beginGameTime - BIConfig.totalHideGameTime;
		dictionary["country"] = BIConfig.country;
		dictionary["unionid"] = BIConfig.unionid;
		dictionary["channel"] = BIConfig.channel;
		dictionary["old_temp_id"] = BIConfig.oldTempId;
		dictionary["first_launch"] = BIConfig.firstLaunch;
		dictionary["pf_version"] = BIConfig.pfVersion;
		dictionary["host_process_id"] = BIConfig.hostProcessId;
		_packData["eventinfo"] = dictionary;
	}

	public void ClearPackParams()
	{
		_packData.Clear();
	}

	public void UpdatePackParams(string info)
	{
		if (!string.IsNullOrWhiteSpace(info))
		{
			_packData = JsonConvert.DeserializeObject<Dictionary<string, object>>(info);
		}
	}

	public IDictionary<string, object> GetSendMap()
	{
		return _packData;
	}

	public string DataDictionaryToString()
	{
		return JsonConvert.SerializeObject(GetSendMap());
	}
}
