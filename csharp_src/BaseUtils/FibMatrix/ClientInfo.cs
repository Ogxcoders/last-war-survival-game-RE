using System.Collections.Generic;

namespace FibMatrix;

public class ClientInfo
{
	public string guid;

	public string store;

	public string runtime;

	public string clientVer;

	public string packVer;

	public string device;

	public string model;

	public string deviceId;

	public string country;

	public string system;

	public void CopyToDictionary(Dictionary<string, object> dic)
	{
		dic["_guid_"] = guid;
		dic["store"] = store;
		dic["runtime"] = runtime;
		dic["clientVer"] = clientVer;
		dic["packVer"] = packVer;
		dic["device"] = device;
		dic["model"] = model;
		dic["deviceId"] = deviceId;
		dic["country"] = country;
		dic["system"] = system;
	}
}
