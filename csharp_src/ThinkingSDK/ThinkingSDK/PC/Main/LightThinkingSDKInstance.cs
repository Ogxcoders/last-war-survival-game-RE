using System.Collections.Generic;
using ThinkingSDK.PC.Config;
using ThinkingSDK.PC.Utils;
using UnityEngine;

namespace ThinkingSDK.PC.Main;

public class LightThinkingSDKInstance : ThinkingSDKInstance
{
	public LightThinkingSDKInstance(string appId, string server, ThinkingSDKConfig config, MonoBehaviour mono = null)
		: base(appId, server, null, config, mono)
	{
	}

	public override void Identifiy(string distinctID)
	{
		if (!IsPaused() && !string.IsNullOrEmpty(distinctID))
		{
			mDistinctID = distinctID;
		}
	}

	public override string DistinctId()
	{
		if (string.IsNullOrEmpty(mDistinctID))
		{
			mDistinctID = ThinkingSDKUtil.RandomID(persistent: false);
		}
		return mDistinctID;
	}

	public override void Login(string accountID)
	{
		if (!IsPaused() && !string.IsNullOrEmpty(accountID))
		{
			mAccountID = accountID;
		}
	}

	public override string AccountID()
	{
		return mAccountID;
	}

	public override void Logout()
	{
		if (!IsPaused())
		{
			mAccountID = "";
		}
	}

	public override void SetSuperProperties(Dictionary<string, object> superProperties)
	{
		if (!IsPaused())
		{
			ThinkingSDKUtil.AddDictionary(mSupperProperties, superProperties);
		}
	}

	public override void UnsetSuperProperty(string propertyKey)
	{
		if (!IsPaused() && mSupperProperties.ContainsKey(propertyKey))
		{
			mSupperProperties.Remove(propertyKey);
		}
	}

	public override Dictionary<string, object> SuperProperties()
	{
		return mSupperProperties;
	}

	public override void ClearSuperProperties()
	{
		if (!IsPaused())
		{
			mSupperProperties.Clear();
		}
	}

	public override void EnableAutoTrack(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties)
	{
	}

	public override void SetAutoTrackProperties(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties)
	{
	}

	public override void Flush()
	{
	}
}
