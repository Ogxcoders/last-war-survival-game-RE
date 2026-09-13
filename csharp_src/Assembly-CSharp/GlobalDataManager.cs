using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;

public class GlobalDataManager
{
	public struct LoginServerInfo
	{
		public int country;

		public int recommandCountry;
	}

	public string download_video_url = "";

	public string download_video_url2 = "";

	public string downloadurlcdn = "";

	public string downloadurl = "";

	public int eu_state;

	public string fblikeutil = "";

	public int force_merge;

	public int force_use_downloadxml;

	private bool isCN;

	public string lua = "";

	public string luaCode = "";

	public string luaCode_v3 = "";

	public int luaSize;

	public string luaVersion = "";

	public string luaVersion_v3 = "";

	public int luazipSize;

	public int randKey;

	public int reduce_init_data;

	public string serverVersion = "";

	public int updateType;

	public string upload_video_url = "";

	public string xmlVersion = "";

	public LoginServerInfo loginServerInfo;

	public string gcmRegisterId = "";

	public string referrer = "";

	public string deeplinkParams = "";

	public string AndroidID = "";

	public string IMEI = "";

	public string analyticID = "";

	public bool s_isGooglePlayAvailable;

	public string platformUID = "";

	public string parseRegisterId = "";

	public string fromCountry = "US";

	public string gaid = "";

	public bool isTodayFirstLogin;

	public bool isNewServer;

	public string version;

	public string uuid;

	public string gaidCache;

	public bool isUploadPic;

	public bool isOpenElvaChat;

	public bool isFAQVoteResp;

	public int cityTileCountry;

	public int serverType;

	public int serverMax;

	public int nowGameCnt;

	public int freeSpdT;

	public int TeleportLimitTime = 72;

	public bool TransResForbiddenSwith;

	public int NewTransKingdomLevel = 6;

	public bool IsCityMoved = true;

	public bool pushOffWithQuitGame;

	public bool isInBackGround;

	public HashSet<string> gameLineBlackList = new HashSet<string>();

	private bool _hasRequestAllProducts;

	public bool LoginServerError;

	private Dictionary<string, object> gGlobalValue = new Dictionary<string, object>();

	public bool HasRequestAllProducts
	{
		get
		{
			return _hasRequestAllProducts;
		}
		set
		{
			_hasRequestAllProducts = value;
		}
	}

	public GlobalDataManager()
	{
		version = Application.version;
	}

	public void Init(ISFSObject dict)
	{
		if (dict.ContainsKey("gaid"))
		{
			gaid = dict.GetUtfString("gaid");
		}
	}

	public void SetAnalyticID()
	{
		analyticID = GameEntry.Sdk.GetPublishRegion();
	}

	public void SetCnFlagFromServer(bool isCn)
	{
		isCN = isCn;
	}

	public void Reset()
	{
		gGlobalValue.Clear();
	}

	public object GetGlobalValue(string key)
	{
		List<string> list = key.ToStrList('/');
		if (list == null || list.Count == 0)
		{
			return null;
		}
		int index = list.Count - 1;
		_ = list[index];
		list.RemoveAt(index);
		if (gGlobalValue.ContainsKey(key))
		{
			return gGlobalValue[key];
		}
		return new object();
	}

	public bool SetGlobalValue(string key, object value)
	{
		List<string> list = key.ToStrList('/');
		if (list == null || list.Count == 0)
		{
			return false;
		}
		int index = list.Count - 1;
		_ = list[index];
		list.RemoveAt(index);
		gGlobalValue[key] = value;
		return true;
	}

	public void recordGaid()
	{
		if (gaid.IsNullOrEmpty())
		{
			if (!gaidCache.IsNullOrEmpty() && !gaidCache.Equals("missed"))
			{
				gaid = gaidCache;
				UserBindGaidMessage.Instance.Send(new UserBindGaidMessage.Request
				{
					gaid = gaid
				});
			}
			else
			{
				gaidCache = "missed";
			}
		}
	}

	public bool isGoogle()
	{
		if (analyticID == "market_global")
		{
			return true;
		}
		return false;
	}

	public bool isGoogleOnlyCheckAnalyticID()
	{
		return false;
	}

	public bool isChina()
	{
		return isCN;
	}

	public bool isMiddleEast()
	{
		return false;
	}

	public bool isTencent()
	{
		return false;
	}

	public bool isAmazon()
	{
		return false;
	}

	public bool isMol()
	{
		return false;
	}

	public bool isMycard()
	{
		return false;
	}

	public bool isOnestore()
	{
		return false;
	}
}
