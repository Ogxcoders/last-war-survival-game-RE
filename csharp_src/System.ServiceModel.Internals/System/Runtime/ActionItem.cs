using System.Diagnostics;
using System.Runtime.Diagnostics;
using System.Security;
using System.Threading;

namespace System.Runtime;

internal abstract class ActionItem
{
	[SecurityCritical]
	private static class CallbackHelper
	{
		private static Action<object> invokeWithoutContextCallback;

		private static ContextCallback onContextAppliedCallback;

		public static Action<object> InvokeWithoutContextCallback
		{
			get
			{
				if (invokeWithoutContextCallback == null)
				{
					invokeWithoutContextCallback = InvokeWithoutContext;
				}
				return invokeWithoutContextCallback;
			}
		}

		public static ContextCallback OnContextAppliedCallback
		{
			get
			{
				if (onContextAppliedCallback == null)
				{
					onContextAppliedCallback = OnContextApplied;
				}
				return onContextAppliedCallback;
			}
		}

		private static void InvokeWithoutContext(object state)
		{
			((ActionItem)state).Invoke();
			((ActionItem)state).isScheduled = false;
		}

		private static void OnContextApplied(object o)
		{
			((ActionItem)o).Invoke();
			((ActionItem)o).isScheduled = false;
		}
	}

	private class DefaultActionItem : ActionItem
	{
		[SecurityCritical]
		private Action<object> callback;

		[SecurityCritical]
		private object state;

		private bool flowLegacyActivityId;

		private Guid activityId;

		private EventTraceActivity eventTraceActivity;

		[SecuritySafeCritical]
		public DefaultActionItem(Action<object> callback, object state, bool isLowPriority)
		{
			base.LowPriority = isLowPriority;
			this.callback = callback;
			this.state = state;
			if (WaitCallbackActionItem.ShouldUseActivity)
			{
				flowLegacyActivityId = true;
				activityId = DiagnosticTraceBase.ActivityId;
			}
			if (Fx.Trace.IsEnd2EndActivityTracingEnabled)
			{
				eventTraceActivity = EventTraceActivity.GetFromThreadOrCreate();
				if (TraceCore.ActionItemScheduledIsEnabled(Fx.Trace))
				{
					TraceCore.ActionItemScheduled(Fx.Trace, eventTraceActivity);
				}
			}
		}

		[SecurityCritical]
		protected override void Invoke()
		{
			if (flowLegacyActivityId || Fx.Trace.IsEnd2EndActivityTracingEnabled)
			{
				TraceAndInvoke();
			}
			else
			{
				callback(state);
			}
		}

		[SecurityCritical]
		private void TraceAndInvoke()
		{
			if (flowLegacyActivityId)
			{
				Guid guid = DiagnosticTraceBase.ActivityId;
				try
				{
					DiagnosticTraceBase.ActivityId = activityId;
					callback(state);
					return;
				}
				finally
				{
					DiagnosticTraceBase.ActivityId = guid;
				}
			}
			Guid empty = Guid.Empty;
			bool flag = false;
			try
			{
				if (eventTraceActivity != null)
				{
					empty = Trace.CorrelationManager.ActivityId;
					flag = true;
					Trace.CorrelationManager.ActivityId = eventTraceActivity.ActivityId;
					if (TraceCore.ActionItemCallbackInvokedIsEnabled(Fx.Trace))
					{
						TraceCore.ActionItemCallbackInvoked(Fx.Trace, eventTraceActivity);
					}
				}
				callback(state);
			}
			finally
			{
				if (flag)
				{
					Trace.CorrelationManager.ActivityId = empty;
				}
			}
		}
	}

	private bool isScheduled;

	private bool lowPriority;

	public bool LowPriority
	{
		get
		{
			return lowPriority;
		}
		protected set
		{
			lowPriority = value;
		}
	}

	public static void Schedule(Action<object> callback, object state)
	{
		Schedule(callback, state, lowPriority: false);
	}

	[SecuritySafeCritical]
	public static void Schedule(Action<object> callback, object state, bool lowPriority)
	{
		if (PartialTrustHelpers.ShouldFlowSecurityContext || WaitCallbackActionItem.ShouldUseActivity || Fx.Trace.IsEnd2EndActivityTracingEnabled)
		{
			new DefaultActionItem(callback, state, lowPriority).Schedule();
		}
		else
		{
			ScheduleCallback(callback, state, lowPriority);
		}
	}

	[SecurityCritical]
	protected abstract void Invoke();

	[SecurityCritical]
	protected void Schedule()
	{
		if (isScheduled)
		{
			throw Fx.Exception.AsError(new InvalidOperationException("Action Item Is Already Scheduled"));
		}
		isScheduled = true;
		ScheduleCallback(CallbackHelper.InvokeWithoutContextCallback);
	}

	[SecurityCritical]
	protected void ScheduleWithoutContext()
	{
		if (isScheduled)
		{
			throw Fx.Exception.AsError(new InvalidOperationException("Action Item Is Already Scheduled"));
		}
		isScheduled = true;
		ScheduleCallback(CallbackHelper.InvokeWithoutContextCallback);
	}

	[SecurityCritical]
	private static void ScheduleCallback(Action<object> callback, object state, bool lowPriority)
	{
		if (lowPriority)
		{
			IOThreadScheduler.ScheduleCallbackLowPriNoFlow(callback, state);
		}
		else
		{
			IOThreadScheduler.ScheduleCallbackNoFlow(callback, state);
		}
	}

	[SecurityCritical]
	private void ScheduleCallback(Action<object> callback)
	{
		ScheduleCallback(callback, this, lowPriority);
	}
}
