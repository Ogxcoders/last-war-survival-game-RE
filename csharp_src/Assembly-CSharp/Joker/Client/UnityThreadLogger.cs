using System;
using UnityEngine;

namespace Joker.Client;

public class UnityThreadLogger : ILogger
{
	public void Debug(string message)
	{
		Singleton<ThreadService>.Instance.PostMainThread(delegate
		{
			UnityEngine.Debug.Log(message);
		});
	}

	public void Info(string message)
	{
		Singleton<ThreadService>.Instance.PostMainThread(delegate
		{
			UnityEngine.Debug.Log(message);
		});
	}

	public void Warning(string message)
	{
		Singleton<ThreadService>.Instance.PostMainThread(delegate
		{
			UnityEngine.Debug.LogWarning(message);
		});
	}

	public void Error(string message)
	{
		Singleton<ThreadService>.Instance.PostMainThread(delegate
		{
			UnityEngine.Debug.LogError(message);
		});
	}

	public void Exception(Exception e)
	{
		Singleton<ThreadService>.Instance.PostMainThread(delegate
		{
			UnityEngine.Debug.LogException(e);
		});
	}

	public void Assert(bool condition, object message = null)
	{
		Singleton<ThreadService>.Instance.PostMainThread(delegate
		{
		});
	}
}
