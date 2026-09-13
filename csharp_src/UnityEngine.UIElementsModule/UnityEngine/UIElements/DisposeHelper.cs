using System;
using System.Diagnostics;

namespace UnityEngine.UIElements;

internal class DisposeHelper
{
	[Conditional("UNITY_UIELEMENTS_DEBUG_DISPOSE")]
	public static void NotifyMissingDispose(IDisposable disposable)
	{
		if (disposable != null)
		{
			Debug.LogError("An IDisposable instance of type '" + disposable.GetType().FullName + "' has not been disposed.");
		}
	}

	public static void NotifyDisposedUsed(IDisposable disposable)
	{
		Debug.LogError("An instance of type '" + disposable.GetType().FullName + "' is being used although it has been disposed.");
	}
}
