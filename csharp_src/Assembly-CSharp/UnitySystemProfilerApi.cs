using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;

public class UnitySystemProfilerApi
{
	public delegate void BeginSample(IntPtr pointer);

	public delegate void EndSample();

	private static readonly Dictionary<long, string> _showNames = new Dictionary<long, string>(32);

	[DllImport("xlua", CallingConvention = CallingConvention.Cdecl)]
	public static extern void InitProfileDelegate(BeginSample begin, EndSample end);

	public static void InitUnityProfile_xLuaDll()
	{
		InitProfileDelegate(Unity_BeginSample, Unity_EndProfile);
	}

	[MonoPInvokeCallback(typeof(BeginSample))]
	public static void Unity_BeginSample(IntPtr pointer)
	{
		long key = pointer.ToInt64();
		if (!_showNames.TryGetValue(key, out var _))
		{
			string value2 = Marshal.PtrToStringAnsi(pointer);
			_showNames[key] = value2;
		}
	}

	[MonoPInvokeCallback(typeof(EndSample))]
	public static void Unity_EndProfile()
	{
	}
}
