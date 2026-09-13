using System;
using System.IO;
using MiniGame.Core;

namespace MiniGame.Biubiu;

public class SerializerTextHolder<T> : ISerializerTextHolder, IResourceHolder
{
	private int _refCount;

	public object Data { get; private set; }

	public bool IsDone { get; private set; }

	public bool IsError { get; private set; }

	public bool IsValid { get; private set; }

	public string ErrorMessage { get; private set; }

	public float Progress { get; }

	public Action<IResourceHolder> OnDone { get; set; }

	public T As<T>()
	{
		return (T)Data;
	}

	public SerializerTextHolder(string name, IGameSerializer serializer)
	{
		IsValid = TryLoad(name, serializer);
		IsDone = true;
		Retain();
		if (IsError)
		{
			throw new Exception("Can not load resource " + name + ":\n" + ErrorMessage);
		}
	}

	public void Retain()
	{
		_refCount++;
	}

	public void Dispose()
	{
		_refCount--;
		if (_refCount <= 0)
		{
			Data = null;
		}
	}

	private bool TryLoad(string name, IGameSerializer serializer)
	{
		try
		{
			string json = File.ReadAllText(name);
			serializer.FromJson<object>(json);
			Data = serializer.FromJson<T>(json);
		}
		catch (Exception ex)
		{
			ErrorMessage = ex.ToString();
			IsError = true;
		}
		return !IsError;
	}
}
