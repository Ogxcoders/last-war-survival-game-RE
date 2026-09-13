using System;

namespace MiniGame.Core;

public interface IResourceHolder
{
	object Data { get; }

	bool IsDone { get; }

	bool IsError { get; }

	bool IsValid { get; }

	float Progress { get; }

	string ErrorMessage { get; }

	Action<IResourceHolder> OnDone { get; set; }

	T As<T>();

	void Dispose();
}
