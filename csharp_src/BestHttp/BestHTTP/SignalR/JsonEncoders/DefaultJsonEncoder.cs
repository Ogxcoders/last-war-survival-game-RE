using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SignalR.JsonEncoders;

public sealed class DefaultJsonEncoder : IJsonEncoder
{
	public string Encode(object obj)
	{
		return Json.Encode(obj);
	}

	public IDictionary<string, object> DecodeMessage(string json)
	{
		bool success = false;
		IDictionary<string, object> result = Json.Decode(json, ref success) as IDictionary<string, object>;
		if (!success)
		{
			return null;
		}
		return result;
	}
}
