using System;
using MiniGame.Core;

namespace MiniGame.Biubiu.Client;

public class EditorResourceHolder : IResourceHolder
{
	public object Data { get; set; }

	public bool IsDone { get; set; }

	public bool IsError { get; set; }

	public bool IsValid { get; set; }

	public float Progress { get; set; }

	public string ErrorMessage { get; set; }

	public Action<IResourceHolder> OnDone { get; set; }

	public T As<T>()
	{
		return (T)Data;
	}

	public void Dispose()
	{
		OnDone = null;
		Data = null;
	}
}
