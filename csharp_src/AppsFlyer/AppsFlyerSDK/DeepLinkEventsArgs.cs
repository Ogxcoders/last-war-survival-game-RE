using System;
using System.Collections.Generic;

namespace AppsFlyerSDK;

public class DeepLinkEventsArgs : EventArgs
{
	public Dictionary<string, object> deepLink;

	public DeepLinkStatus status { get; }

	public DeepLinkError error { get; }

	public string getMatchType()
	{
		return getDeepLinkParameter("match_type");
	}

	public string getDeepLinkValue()
	{
		return getDeepLinkParameter("deep_link_value");
	}

	public string getClickHttpReferrer()
	{
		return getDeepLinkParameter("click_http_referrer");
	}

	public string getMediaSource()
	{
		return getDeepLinkParameter("media_source");
	}

	public string getCampaign()
	{
		return getDeepLinkParameter("campaign");
	}

	public string getCampaignId()
	{
		return getDeepLinkParameter("campaign_id");
	}

	public string getAfSub1()
	{
		return getDeepLinkParameter("af_sub1");
	}

	public string getAfSub2()
	{
		return getDeepLinkParameter("af_sub2");
	}

	public string getAfSub3()
	{
		return getDeepLinkParameter("af_sub3");
	}

	public string getAfSub4()
	{
		return getDeepLinkParameter("af_sub4");
	}

	public string getAfSub5()
	{
		return getDeepLinkParameter("af_sub5");
	}

	public bool isDeferred()
	{
		if (deepLink != null && deepLink.ContainsKey("is_deferred"))
		{
			try
			{
				return (bool)deepLink["is_deferred"];
			}
			catch (Exception arg)
			{
				AppsFlyer.AFLog("DeepLinkEventsArgs.isDeferred", $"{arg} Exception caught.");
			}
		}
		return false;
	}

	public Dictionary<string, object> getDeepLinkDictionary()
	{
		return deepLink;
	}

	public DeepLinkEventsArgs(string str)
	{
		try
		{
			Dictionary<string, object> dictionary = AppsFlyer.CallbackStringToDictionary(str);
			string text = "";
			string text2 = "";
			if (dictionary.ContainsKey("status") && dictionary["status"] != null)
			{
				text = dictionary["status"].ToString();
			}
			if (dictionary.ContainsKey("error") && dictionary["error"] != null)
			{
				text2 = dictionary["error"].ToString();
			}
			if (dictionary.ContainsKey("deepLink") && dictionary["deepLink"] != null)
			{
				deepLink = AppsFlyer.CallbackStringToDictionary(dictionary["deepLink"].ToString());
			}
			if (!(text == "FOUND"))
			{
				if (text == "NOT_FOUND")
				{
					status = DeepLinkStatus.NOT_FOUND;
				}
				else
				{
					status = DeepLinkStatus.ERROR;
				}
			}
			else
			{
				status = DeepLinkStatus.FOUND;
			}
			switch (text2)
			{
			case "TIMEOUT":
				error = DeepLinkError.TIMEOUT;
				break;
			case "NETWORK":
				error = DeepLinkError.NETWORK;
				break;
			case "HTTP_STATUS_CODE":
				error = DeepLinkError.HTTP_STATUS_CODE;
				break;
			default:
				error = DeepLinkError.UNEXPECTED;
				break;
			}
		}
		catch (Exception arg)
		{
			AppsFlyer.AFLog("DeepLinkEventsArgs.parseDeepLink", $"{arg} Exception caught.");
		}
	}

	private string getDeepLinkParameter(string name)
	{
		if (deepLink != null && deepLink.ContainsKey(name) && deepLink[name] != null)
		{
			return deepLink[name].ToString();
		}
		return null;
	}
}
