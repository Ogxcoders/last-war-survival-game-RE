using System;
using UnityEngine;

namespace Joker.Client;

public class UnityLogger : ILogger
{
	public void Debug(string message)
	{
		UnityEngine.Debug.Log(message);
	}

	public void Info(string message)
	{
		UnityEngine.Debug.Log(message);
		if (GameEntry.Sdk != null)
		{
			GameEntry.Sdk.SendDataToNative("[SmSdk]", message);
		}
	}

	public void Warning(string message)
	{
		UnityEngine.Debug.LogWarning(message);
	}

	public void Error(string message)
	{
		UnityEngine.Debug.LogError(message);
	}

	public void Exception(Exception e)
	{
		UnityEngine.Debug.LogException(e);
	}

	public void Assert(bool condition, object message = null)
	{
	}
}
