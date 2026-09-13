using System.Diagnostics;

namespace UnityEngine.Timeline;

internal static class TimelineUndo
{
	internal static bool undoEnabled => false;

	public static void PushDestroyUndo(TimelineAsset timeline, Object thingToDirty, Object objectToDestroy)
	{
		if (objectToDestroy != null)
		{
			Object.Destroy(objectToDestroy);
		}
	}

	[Conditional("UNITY_EDITOR")]
	public static void PushUndo(Object[] thingsToDirty, string operation)
	{
	}

	[Conditional("UNITY_EDITOR")]
	public static void PushUndo(Object thingToDirty, string operation)
	{
	}

	[Conditional("UNITY_EDITOR")]
	public static void RegisterCreatedObjectUndo(Object thingCreated, string operation)
	{
	}

	internal static string UndoName(string name)
	{
		return "Timeline " + name;
	}
}
