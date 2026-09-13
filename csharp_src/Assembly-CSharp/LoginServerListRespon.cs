using System;

[Serializable]
public class LoginServerListRespon
{
	public int code;

	public LoginServerInfo[] serverList;

	public int country;

	public string tcp;

	public string newDesertFlag;

	public int lastLoggedServer;

	public AccountServerInfo loginServer;

	public bool UseMQTT;

	public string MQTTIP;

	public int MQTTPort;

	public bool uploadLog;

	public string NetType;

	public string bin;

	public LoginToken at;

	public LoginToken rt;

	public LoginServerInfo GetLastLoggedServerInfo()
	{
		if (lastLoggedServer != 0)
		{
			for (int i = 0; i < serverList.Length; i++)
			{
				if (serverList[i].id == lastLoggedServer)
				{
					return serverList[i];
				}
			}
		}
		return null;
	}
}
