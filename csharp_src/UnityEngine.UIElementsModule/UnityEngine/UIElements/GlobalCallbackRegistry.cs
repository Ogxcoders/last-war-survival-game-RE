using System;

namespace UnityEngine.UIElements;

internal static class GlobalCallbackRegistry
{
	public static void RegisterListeners<TEventType>(CallbackEventHandler ceh, Delegate callback, TrickleDown useTrickleDown)
	{
	}

	public static void UnregisterListeners<TEventType>(CallbackEventHandler ceh, Delegate callback)
	{
	}
}
