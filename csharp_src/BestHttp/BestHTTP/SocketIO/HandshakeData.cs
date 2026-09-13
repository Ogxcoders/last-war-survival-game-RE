using System;
using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SocketIO;

public sealed class HandshakeData
{
	public string Sid { get; private set; }

	public List<string> Upgrades { get; private set; }

	public TimeSpan PingInterval { get; private set; }

	public TimeSpan PingTimeout { get; private set; }

	public bool Parse(string str)
	{
		bool success = false;
		Dictionary<string, object> dictionary = Json.Decode(str, ref success) as Dictionary<string, object>;
		if (!success)
		{
			return false;
		}
		try
		{
			Sid = GetString(dictionary, "sid");
			Upgrades = GetStringList(dictionary, "upgrades");
			PingInterval = TimeSpan.FromMilliseconds(GetInt(dictionary, "pingInterval"));
			PingTimeout = TimeSpan.FromMilliseconds(GetInt(dictionary, "pingTimeout"));
		}
		catch (Exception ex)
		{
			HTTPManager.Logger.Exception("HandshakeData", "Parse", ex);
			return false;
		}
		return true;
	}

	private static object Get(Dictionary<string, object> from, string key)
	{
		if (!from.TryGetValue(key, out var value))
		{
			throw new Exception($"Can't get {key} from Handshake data!");
		}
		return value;
	}

	private static string GetString(Dictionary<string, object> from, string key)
	{
		return Get(from, key) as string;
	}

	private static List<string> GetStringList(Dictionary<string, object> from, string key)
	{
		List<object> list = Get(from, key) as List<object>;
		List<string> list2 = new List<string>(list.Count);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] is string item)
			{
				list2.Add(item);
			}
		}
		return list2;
	}

	private static int GetInt(Dictionary<string, object> from, string key)
	{
		return (int)(double)Get(from, key);
	}
}
