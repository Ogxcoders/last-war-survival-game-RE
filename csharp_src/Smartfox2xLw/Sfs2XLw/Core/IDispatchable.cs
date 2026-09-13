namespace Sfs2XLw.Core;

public interface IDispatchable
{
	EventDispatcher Dispatcher { get; }

	void AddEventListener(string eventType, EventListenerDelegate listener);
}
