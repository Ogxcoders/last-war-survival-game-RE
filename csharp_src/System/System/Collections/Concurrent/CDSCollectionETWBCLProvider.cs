using System.Diagnostics.Tracing;

namespace System.Collections.Concurrent;

[EventSource(Name = "System.Collections.Concurrent.ConcurrentCollectionsEventSource", Guid = "35167F8E-49B2-4b96-AB86-435B59336B5E")]
internal sealed class CDSCollectionETWBCLProvider : EventSource
{
	public static CDSCollectionETWBCLProvider Log = new CDSCollectionETWBCLProvider();

	private const EventKeywords ALL_KEYWORDS = EventKeywords.All;

	private const int CONCURRENTSTACK_FASTPUSHFAILED_ID = 1;

	private const int CONCURRENTSTACK_FASTPOPFAILED_ID = 2;

	private const int CONCURRENTDICTIONARY_ACQUIRINGALLLOCKS_ID = 3;

	private const int CONCURRENTBAG_TRYTAKESTEALS_ID = 4;

	private const int CONCURRENTBAG_TRYPEEKSTEALS_ID = 5;

	private CDSCollectionETWBCLProvider()
	{
	}

	[Event(1, Level = EventLevel.Warning)]
	public void ConcurrentStack_FastPushFailed(int spinCount)
	{
		if (IsEnabled(EventLevel.Warning, EventKeywords.All))
		{
			WriteEvent(1, spinCount);
		}
	}

	[Event(2, Level = EventLevel.Warning)]
	public void ConcurrentStack_FastPopFailed(int spinCount)
	{
		if (IsEnabled(EventLevel.Warning, EventKeywords.All))
		{
			WriteEvent(2, spinCount);
		}
	}

	[Event(3, Level = EventLevel.Warning)]
	public void ConcurrentDictionary_AcquiringAllLocks(int numOfBuckets)
	{
		if (IsEnabled(EventLevel.Warning, EventKeywords.All))
		{
			WriteEvent(3, numOfBuckets);
		}
	}

	[Event(4, Level = EventLevel.Verbose)]
	public void ConcurrentBag_TryTakeSteals()
	{
		if (IsEnabled(EventLevel.Verbose, EventKeywords.All))
		{
			WriteEvent(4);
		}
	}

	[Event(5, Level = EventLevel.Verbose)]
	public void ConcurrentBag_TryPeekSteals()
	{
		if (IsEnabled(EventLevel.Verbose, EventKeywords.All))
		{
			WriteEvent(5);
		}
	}
}
