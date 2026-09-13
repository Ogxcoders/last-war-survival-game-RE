using System.Diagnostics;
using System.Security.Permissions;

namespace System.ComponentModel;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
internal static class CompModSwitches
{
	private static volatile BooleanSwitch commonDesignerServices;

	private static volatile TraceSwitch eventLog;

	public static BooleanSwitch CommonDesignerServices
	{
		get
		{
			if (commonDesignerServices == null)
			{
				commonDesignerServices = new BooleanSwitch("CommonDesignerServices", "Assert if any common designer service is not found.");
			}
			return commonDesignerServices;
		}
	}

	public static TraceSwitch EventLog
	{
		get
		{
			if (eventLog == null)
			{
				eventLog = new TraceSwitch("EventLog", "Enable tracing for the EventLog component.");
			}
			return eventLog;
		}
	}
}
