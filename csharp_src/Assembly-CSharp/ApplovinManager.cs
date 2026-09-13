using System;

public class ApplovinManager
{
	private static ApplovinManager _instance;

	private Action<string, string> _callback;

	public static ApplovinManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ApplovinManager();
			}
			return _instance;
		}
	}

	public void SetCallBack(Action<string, string> action)
	{
		_callback = action;
	}

	public void OnNativeCallback(string funcName, string data)
	{
		if (_callback != null)
		{
			_callback(funcName, data);
		}
	}
}
