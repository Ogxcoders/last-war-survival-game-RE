namespace System.Reflection;

public static class EventInfoExtensions
{
	public static MethodInfo GetAddMethod(EventInfo eventInfo)
	{
		Requires.NotNull(eventInfo, "eventInfo");
		return eventInfo.GetAddMethod();
	}

	public static MethodInfo GetAddMethod(EventInfo eventInfo, bool nonPublic)
	{
		Requires.NotNull(eventInfo, "eventInfo");
		return eventInfo.GetAddMethod(nonPublic);
	}

	public static MethodInfo GetRaiseMethod(EventInfo eventInfo)
	{
		Requires.NotNull(eventInfo, "eventInfo");
		return eventInfo.GetRaiseMethod();
	}

	public static MethodInfo GetRaiseMethod(EventInfo eventInfo, bool nonPublic)
	{
		Requires.NotNull(eventInfo, "eventInfo");
		return eventInfo.GetRaiseMethod(nonPublic);
	}

	public static MethodInfo GetRemoveMethod(EventInfo eventInfo)
	{
		Requires.NotNull(eventInfo, "eventInfo");
		return eventInfo.GetRemoveMethod();
	}

	public static MethodInfo GetRemoveMethod(EventInfo eventInfo, bool nonPublic)
	{
		Requires.NotNull(eventInfo, "eventInfo");
		return eventInfo.GetRemoveMethod(nonPublic);
	}
}
