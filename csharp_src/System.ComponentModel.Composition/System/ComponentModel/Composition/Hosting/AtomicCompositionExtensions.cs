using Microsoft.Internal;

namespace System.ComponentModel.Composition.Hosting;

internal static class AtomicCompositionExtensions
{
	internal static T GetValueAllowNull<T>(this AtomicComposition atomicComposition, T defaultResultAndKey) where T : class
	{
		Assumes.NotNull(defaultResultAndKey);
		return atomicComposition.GetValueAllowNull(defaultResultAndKey, defaultResultAndKey);
	}

	internal static T GetValueAllowNull<T>(this AtomicComposition atomicComposition, object key, T defaultResult)
	{
		if (atomicComposition != null && atomicComposition.TryGetValue<T>(key, out var value))
		{
			return value;
		}
		return defaultResult;
	}

	internal static void AddRevertActionAllowNull(this AtomicComposition atomicComposition, Action action)
	{
		Assumes.NotNull(action);
		if (atomicComposition == null)
		{
			action();
		}
		else
		{
			atomicComposition.AddRevertAction(action);
		}
	}

	internal static void AddCompleteActionAllowNull(this AtomicComposition atomicComposition, Action action)
	{
		Assumes.NotNull(action);
		if (atomicComposition == null)
		{
			action();
		}
		else
		{
			atomicComposition.AddCompleteAction(action);
		}
	}
}
