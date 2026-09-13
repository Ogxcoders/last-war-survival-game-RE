using System.Reflection;
using System.Threading;

namespace System;

internal class AndroidPlatform
{
	private static readonly Func<SynchronizationContext> getDefaultSyncContext;

	private static readonly Func<string> getDefaultTimeZone;

	static AndroidPlatform()
	{
		Type type = Type.GetType("Android.Runtime.AndroidEnvironment, Mono.Android", throwOnError: true);
		getDefaultSyncContext = (Func<SynchronizationContext>)Delegate.CreateDelegate(typeof(Func<SynchronizationContext>), type.GetMethod("GetDefaultSyncContext", BindingFlags.Static | BindingFlags.NonPublic));
		getDefaultTimeZone = (Func<string>)Delegate.CreateDelegate(typeof(Func<string>), type.GetMethod("GetDefaultTimeZone", BindingFlags.Static | BindingFlags.NonPublic));
	}

	internal static SynchronizationContext GetDefaultSyncContext()
	{
		return getDefaultSyncContext();
	}

	internal static string GetDefaultTimeZone()
	{
		return getDefaultTimeZone();
	}
}
