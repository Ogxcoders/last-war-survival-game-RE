using System;
using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SignalRCore.Messages;

public sealed class NegotiationResult
{
	public string ConnectionId { get; private set; }

	public List<SupportedTransport> SupportedTransports { get; private set; }

	public Uri Url { get; private set; }

	public string AccessToken { get; private set; }

	internal static NegotiationResult Parse(string json, out string error, HubConnection hub)
	{
		error = null;
		if (!(Json.Decode(json) is Dictionary<string, object> dictionary))
		{
			error = "Json decoding failed!";
			return null;
		}
		try
		{
			NegotiationResult negotiationResult = new NegotiationResult();
			if (dictionary.TryGetValue("connectionId", out var value))
			{
				negotiationResult.ConnectionId = value.ToString();
			}
			if (dictionary.TryGetValue("availableTransports", out value) && value is List<object> list)
			{
				List<SupportedTransport> list2 = new List<SupportedTransport>(list.Count);
				foreach (Dictionary<string, object> item in list)
				{
					string transportName = string.Empty;
					List<string> list3 = null;
					if (item.TryGetValue("transport", out value))
					{
						transportName = value.ToString();
					}
					if (item.TryGetValue("transferFormats", out value) && value is List<object> list4)
					{
						list3 = new List<string>(list4.Count);
						foreach (object item2 in list4)
						{
							list3.Add(item2.ToString());
						}
					}
					list2.Add(new SupportedTransport(transportName, list3));
				}
				negotiationResult.SupportedTransports = list2;
			}
			if (dictionary.TryGetValue("url", out value))
			{
				string text = value.ToString();
				if (!Uri.TryCreate(text, UriKind.RelativeOrAbsolute, out var result))
				{
					throw new Exception($"Couldn't parse url: '{text}'");
				}
				if (!result.IsAbsoluteUri)
				{
					result = new UriBuilder(hub.Uri)
					{
						Path = text
					}.Uri;
				}
				negotiationResult.Url = result;
			}
			if (dictionary.TryGetValue("accessToken", out value))
			{
				negotiationResult.AccessToken = value.ToString();
			}
			else if (hub.NegotiationResult != null)
			{
				negotiationResult.AccessToken = hub.NegotiationResult.AccessToken;
			}
			return negotiationResult;
		}
		catch (Exception ex)
		{
			error = "Error while parsing result: " + ex.Message + " StackTrace: " + ex.StackTrace;
			return null;
		}
	}
}
