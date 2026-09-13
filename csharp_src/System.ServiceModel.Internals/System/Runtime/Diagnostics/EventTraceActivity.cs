using System.Diagnostics;
using System.Security;

namespace System.Runtime.Diagnostics;

internal class EventTraceActivity
{
	public Guid ActivityId;

	private static EventTraceActivity empty;

	public static EventTraceActivity Empty
	{
		get
		{
			if (empty == null)
			{
				empty = new EventTraceActivity(Guid.Empty);
			}
			return empty;
		}
	}

	public static string Name => "E2EActivity";

	public EventTraceActivity(bool setOnThread = false)
		: this(Guid.NewGuid(), setOnThread)
	{
	}

	public EventTraceActivity(Guid guid, bool setOnThread = false)
	{
		ActivityId = guid;
		if (setOnThread)
		{
			SetActivityIdOnThread();
		}
	}

	[SecuritySafeCritical]
	public static EventTraceActivity GetFromThreadOrCreate(bool clearIdOnThread = false)
	{
		Guid guid = Trace.CorrelationManager.ActivityId;
		if (guid == Guid.Empty)
		{
			guid = Guid.NewGuid();
		}
		else if (clearIdOnThread)
		{
			Trace.CorrelationManager.ActivityId = Guid.Empty;
		}
		return new EventTraceActivity(guid);
	}

	[SecuritySafeCritical]
	public static Guid GetActivityIdFromThread()
	{
		return Trace.CorrelationManager.ActivityId;
	}

	public void SetActivityId(Guid guid)
	{
		ActivityId = guid;
	}

	[SecuritySafeCritical]
	private void SetActivityIdOnThread()
	{
		Trace.CorrelationManager.ActivityId = ActivityId;
	}
}
