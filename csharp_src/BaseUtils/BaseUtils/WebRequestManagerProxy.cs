using System;
using UnityEngine.Networking;

namespace BaseUtils;

public class WebRequestManagerProxy
{
	private static WebRequestManagerProxy _instance;

	public Action<string, Action<UnityWebRequest, bool, object>, int, int, object> GetAction;

	public static WebRequestManagerProxy Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new WebRequestManagerProxy();
			}
			return _instance;
		}
	}

	public void Get(string uri, Action<UnityWebRequest, bool, object> callback, int priority = 0, int timeout = 0, object userdata = null)
	{
		GetAction(uri, callback, priority, timeout, userdata);
	}

	public void Dispose()
	{
	}
}
