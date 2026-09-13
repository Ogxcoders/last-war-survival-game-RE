using System.Diagnostics.Tracing;
using System.Threading;
using System.Threading.Tasks;

namespace System.Linq.Parallel;

[EventSource(Name = "System.Linq.Parallel.PlinqEventSource", Guid = "159eeeec-4a14-4418-a8fe-faabcd987887")]
internal sealed class PlinqEtwProvider : EventSource
{
	public class Tasks
	{
		public const EventTask Query = (EventTask)1;

		public const EventTask ForkJoin = (EventTask)2;
	}

	internal static PlinqEtwProvider Log = new PlinqEtwProvider();

	private static readonly int s_defaultSchedulerId = TaskScheduler.Default.Id;

	private static int s_queryId = 0;

	private const EventKeywords ALL_KEYWORDS = EventKeywords.All;

	private const int PARALLELQUERYBEGIN_EVENTID = 1;

	private const int PARALLELQUERYEND_EVENTID = 2;

	private const int PARALLELQUERYFORK_EVENTID = 3;

	private const int PARALLELQUERYJOIN_EVENTID = 4;

	private PlinqEtwProvider()
	{
	}

	[NonEvent]
	internal static int NextQueryId()
	{
		return Interlocked.Increment(ref s_queryId);
	}

	[NonEvent]
	internal void ParallelQueryBegin(int queryId)
	{
		if (IsEnabled(EventLevel.Informational, EventKeywords.All))
		{
			int valueOrDefault = Task.CurrentId.GetValueOrDefault();
			ParallelQueryBegin(s_defaultSchedulerId, valueOrDefault, queryId);
		}
	}

	[Event(1, Level = EventLevel.Informational, Task = (EventTask)1, Opcode = EventOpcode.Start)]
	private void ParallelQueryBegin(int taskSchedulerId, int taskId, int queryId)
	{
		WriteEvent(1, taskSchedulerId, taskId, queryId);
	}

	[NonEvent]
	internal void ParallelQueryEnd(int queryId)
	{
		if (IsEnabled(EventLevel.Informational, EventKeywords.All))
		{
			int valueOrDefault = Task.CurrentId.GetValueOrDefault();
			ParallelQueryEnd(s_defaultSchedulerId, valueOrDefault, queryId);
		}
	}

	[Event(2, Level = EventLevel.Informational, Task = (EventTask)1, Opcode = EventOpcode.Stop)]
	private void ParallelQueryEnd(int taskSchedulerId, int taskId, int queryId)
	{
		WriteEvent(2, taskSchedulerId, taskId, queryId);
	}

	[NonEvent]
	internal void ParallelQueryFork(int queryId)
	{
		if (IsEnabled(EventLevel.Verbose, EventKeywords.All))
		{
			int valueOrDefault = Task.CurrentId.GetValueOrDefault();
			ParallelQueryFork(s_defaultSchedulerId, valueOrDefault, queryId);
		}
	}

	[Event(3, Level = EventLevel.Verbose, Task = (EventTask)2, Opcode = EventOpcode.Start)]
	private void ParallelQueryFork(int taskSchedulerId, int taskId, int queryId)
	{
		WriteEvent(3, taskSchedulerId, taskId, queryId);
	}

	[NonEvent]
	internal void ParallelQueryJoin(int queryId)
	{
		if (IsEnabled(EventLevel.Verbose, EventKeywords.All))
		{
			int valueOrDefault = Task.CurrentId.GetValueOrDefault();
			ParallelQueryJoin(s_defaultSchedulerId, valueOrDefault, queryId);
		}
	}

	[Event(4, Level = EventLevel.Verbose, Task = (EventTask)2, Opcode = EventOpcode.Stop)]
	private void ParallelQueryJoin(int taskSchedulerId, int taskId, int queryId)
	{
		WriteEvent(4, taskSchedulerId, taskId, queryId);
	}
}
