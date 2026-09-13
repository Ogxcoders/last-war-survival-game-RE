using System;
using System.Collections.Generic;
using System.Globalization;

namespace ThinkingAnalytics;

public class TDPresetProperties
{
	public string AppVersion => (string)(mPresetProperties.ContainsKey("#app_version") ? mPresetProperties["#app_version"] : "");

	public string BundleId => (string)(mPresetProperties.ContainsKey("#bundle_id") ? mPresetProperties["#bundle_id"] : "");

	public string Carrier => (string)(mPresetProperties.ContainsKey("#carrier") ? mPresetProperties["#carrier"] : "");

	public string DeviceId => (string)(mPresetProperties.ContainsKey("#device_id") ? mPresetProperties["#device_id"] : "");

	public string DeviceModel => (string)(mPresetProperties.ContainsKey("#device_model") ? mPresetProperties["#device_model"] : "");

	public string Manufacturer => (string)(mPresetProperties.ContainsKey("#manufacturer") ? mPresetProperties["#manufacturer"] : "");

	public string NetworkType => (string)(mPresetProperties.ContainsKey("#network_type") ? mPresetProperties["#network_type"] : "");

	public string OS => (string)(mPresetProperties.ContainsKey("#os") ? mPresetProperties["#os"] : "");

	public string OSVersion => (string)(mPresetProperties.ContainsKey("#os_version") ? mPresetProperties["#os_version"] : "");

	public double ScreenHeight => Convert.ToDouble(mPresetProperties.ContainsKey("#screen_height") ? mPresetProperties["#screen_height"] : ((object)0));

	public double ScreenWidth => Convert.ToDouble(mPresetProperties.ContainsKey("#screen_width") ? mPresetProperties["#screen_width"] : ((object)0));

	public string SystemLanguage => (string)(mPresetProperties.ContainsKey("#system_language") ? mPresetProperties["#system_language"] : "");

	public double ZoneOffset => Convert.ToDouble(mPresetProperties.ContainsKey("#zone_offset") ? mPresetProperties["#zone_offset"] : ((object)0));

	public string InstallTime => (string)(mPresetProperties.ContainsKey("#install_time") ? mPresetProperties["#install_time"] : "");

	public string Disk => (string)(mPresetProperties.ContainsKey("#disk") ? mPresetProperties["#disk"] : "");

	public string Ram => (string)(mPresetProperties.ContainsKey("#ram") ? mPresetProperties["#ram"] : "");

	public double Fps => Convert.ToDouble(mPresetProperties.ContainsKey("#fps") ? mPresetProperties["#fps"] : ((object)0));

	public bool Simulator => (bool)(mPresetProperties.ContainsKey("#simulator") ? mPresetProperties["#simulator"] : ((object)false));

	private Dictionary<string, object> mPresetProperties { get; set; }

	public TDPresetProperties(Dictionary<string, object> properties)
	{
		properties = TDEncodeDate(properties);
		mPresetProperties = properties;
	}

	public Dictionary<string, object> ToEventPresetProperties()
	{
		return mPresetProperties;
	}

	private Dictionary<string, object> TDEncodeDate(Dictionary<string, object> properties)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (KeyValuePair<string, object> property in properties)
		{
			if (property.Value is DateTime)
			{
				DateTime dateTime = (DateTime)property.Value;
				dictionary.Add(property.Key, dateTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
			}
			else
			{
				dictionary.Add(property.Key, property.Value);
			}
		}
		return dictionary;
	}
}
