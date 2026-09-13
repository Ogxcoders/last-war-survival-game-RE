using System.Collections.Generic;

namespace BestHTTP.SignalR.JsonEncoders;

public interface IJsonEncoder
{
	string Encode(object obj);

	IDictionary<string, object> DecodeMessage(string json);
}
