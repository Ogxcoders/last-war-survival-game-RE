using System;

namespace Joker;

public interface IScheduler : IDisposable
{
	int Id { get; }

	event Action Updater;

	event Action LateUpdater;

	void Post(Action action);

	void Start();
}
