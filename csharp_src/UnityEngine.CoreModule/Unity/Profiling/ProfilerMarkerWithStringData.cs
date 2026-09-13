using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.Profiling.LowLevel;
using Unity.Profiling.LowLevel.Unsafe;

namespace Unity.Profiling;

internal struct ProfilerMarkerWithStringData
{
	public struct AutoScope : IDisposable
	{
		private IntPtr _marker;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal AutoScope(IntPtr marker)
		{
			_marker = marker;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		[Pure]
		public void Dispose()
		{
			if (_marker != IntPtr.Zero)
			{
				ProfilerUnsafeUtility.EndSample(_marker);
			}
		}
	}

	private const MethodImplOptions AggressiveInlining = MethodImplOptions.AggressiveInlining;

	private IntPtr _marker;

	public static ProfilerMarkerWithStringData Create(string name, string parameterName)
	{
		IntPtr intPtr = ProfilerUnsafeUtility.CreateMarker(name, 16, MarkerFlags.Default, 1);
		ProfilerUnsafeUtility.SetMarkerMetadata(intPtr, 0, parameterName, 9, 0);
		return new ProfilerMarkerWithStringData
		{
			_marker = intPtr
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Pure]
	public AutoScope Auto(bool enabled, Func<string> parameterValue)
	{
		if (enabled)
		{
			return Auto(parameterValue());
		}
		return new AutoScope(IntPtr.Zero);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Pure]
	public unsafe AutoScope Auto(string value)
	{
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		fixed (char* ptr = value)
		{
			ProfilerMarkerData profilerMarkerData = new ProfilerMarkerData
			{
				Type = 9,
				Size = (uint)(value.Length * 2 + 2)
			};
			profilerMarkerData.Ptr = ptr;
			ProfilerUnsafeUtility.BeginSampleWithMetadata(_marker, 1, &profilerMarkerData);
		}
		return new AutoScope(_marker);
	}
}
