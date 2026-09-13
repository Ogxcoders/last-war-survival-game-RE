using System;
using System.Diagnostics;
using MiniGame.Core;

namespace MiniGame.Test.Client;

public class GameTestLevelHolder : IResourceHolder
{
	private string _path;

	private Stopwatch _sw;

	public object Data { get; private set; }

	public string ErrorMessage { get; private set; }

	public bool IsDone
	{
		get
		{
			UpdateData();
			if (Data != null)
			{
				return true;
			}
			return _sw.ElapsedMilliseconds > 1000;
		}
	}

	public bool IsError
	{
		get
		{
			UpdateData();
			if (_sw.ElapsedMilliseconds > 1000)
			{
				return Data == null;
			}
			return false;
		}
	}

	public bool IsValid => Data != null;

	public float Progress
	{
		get
		{
			if (!IsValid)
			{
				return (float)_sw.ElapsedMilliseconds / 1000f;
			}
			return 1f;
		}
	}

	public Action<IResourceHolder> OnDone { get; set; }

	public T As<T>()
	{
		UpdateData();
		return (T)Data;
	}

	public GameTestLevelHolder(string path)
	{
		_path = path;
		_sw = new Stopwatch();
		_sw.Start();
		Data = null;
	}

	public void Dispose()
	{
		_sw.Stop();
		_sw = null;
	}

	private void UpdateData()
	{
		if (Data == null && _sw.ElapsedMilliseconds >= 1000)
		{
			Data = new GameLevel();
		}
	}
}
