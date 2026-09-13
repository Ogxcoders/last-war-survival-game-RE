using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.TestTools;

[NativeType("Runtime/Scripting/ScriptingCoverage.h")]
[NativeClass("ScriptingCoverage")]
public static class Coverage
{
	public static extern bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ScriptingCoverageGetCoverageForMethodInfoObject", ThrowsException = true)]
	private static extern CoveredSequencePoint[] GetSequencePointsFor_Internal(MethodBase method);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ScriptingCoverageResetForMethodInfoObject", ThrowsException = true)]
	private static extern void ResetFor_Internal(MethodBase method);

	[FreeFunction("ScriptingCoverageGetStatsForMethodInfoObject", ThrowsException = true)]
	private static CoveredMethodStats GetStatsFor_Internal(MethodBase method)
	{
		GetStatsFor_Internal_Injected(method, out var ret);
		return ret;
	}

	public static CoveredSequencePoint[] GetSequencePointsFor(MethodBase method)
	{
		if ((object)method == null)
		{
			throw new ArgumentNullException("method");
		}
		return GetSequencePointsFor_Internal(method);
	}

	public static CoveredMethodStats GetStatsFor(MethodBase method)
	{
		if ((object)method == null)
		{
			throw new ArgumentNullException("method");
		}
		return GetStatsFor_Internal(method);
	}

	public static CoveredMethodStats[] GetStatsFor(MethodBase[] methods)
	{
		if (methods == null)
		{
			throw new ArgumentNullException("methods");
		}
		CoveredMethodStats[] array = new CoveredMethodStats[methods.Length];
		for (int i = 0; i < methods.Length; i++)
		{
			array[i] = GetStatsFor(methods[i]);
		}
		return array;
	}

	public static CoveredMethodStats[] GetStatsFor(Type type)
	{
		if ((object)type == null)
		{
			throw new ArgumentNullException("type");
		}
		return GetStatsFor(type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).OfType<MethodBase>().ToArray());
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ScriptingCoverageGetStatsForAllCoveredMethodsFromScripting", ThrowsException = true)]
	public static extern CoveredMethodStats[] GetStatsForAllCoveredMethods();

	public static void ResetFor(MethodBase method)
	{
		if ((object)method == null)
		{
			throw new ArgumentNullException("method");
		}
		ResetFor_Internal(method);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("ScriptingCoverageResetAllFromScripting", ThrowsException = true)]
	public static extern void ResetAll();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetStatsFor_Internal_Injected(MethodBase method, out CoveredMethodStats ret);
}
