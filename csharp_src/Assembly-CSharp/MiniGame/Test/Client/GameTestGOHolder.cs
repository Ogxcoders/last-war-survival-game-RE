using System;
using MiniGame.Core;

namespace MiniGame.Test.Client;

public class GameTestGOHolder : IResourceHolder
{
	public object Data { get; }

	public bool IsDone { get; }

	public bool IsError { get; }

	public bool IsValid { get; }

	public float Progress { get; }

	public string ErrorMessage { get; private set; }

	public Action<IResourceHolder> OnDone { get; set; }

	public T As<T>()
	{
		throw new NotImplementedException();
	}

	public void Dispose()
	{
		throw new NotImplementedException();
	}
}
