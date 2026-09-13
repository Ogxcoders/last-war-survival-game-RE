using System;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class TextAssetHolder<T> : ISerializerTextHolder, IResourceHolder
{
	private int _refCount;

	private IResourceHolder ResourceHolder;

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

	public TextAssetHolder(IResourceHolder resourceHolder, string name, IGameSerializer serializer)
	{
		ResourceHolder = resourceHolder;
		IsValid = TrySerializer(serializer);
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
			if (ResourceHolder != null)
			{
				ResourceHolder.Dispose();
			}
			ResourceHolder = null;
		}
	}

	private bool TrySerializer(IGameSerializer serializer)
	{
		try
		{
			if (ResourceHolder.IsError || !ResourceHolder.IsDone)
			{
				return false;
			}
			TextAsset textAsset = ResourceHolder.Data as TextAsset;
			if (textAsset != null)
			{
				Data = serializer.FromJson<T>(textAsset.text);
			}
		}
		catch (Exception ex)
		{
			ErrorMessage = ex.ToString();
			IsError = true;
		}
		return !IsError;
	}
}
