using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Util;
using System.Threading;

namespace System;

internal sealed class SharedStatics
{
	private static readonly SharedStatics _sharedStatics = new SharedStatics();

	private volatile string _Remoting_Identity_IDGuid;

	private Tokenizer.StringMaker _maker;

	private int _Remoting_Identity_IDSeqNum;

	private long _memFailPointReservedMemory;

	public static string Remoting_Identity_IDGuid
	{
		[SecuritySafeCritical]
		get
		{
			if (_sharedStatics._Remoting_Identity_IDGuid == null)
			{
				bool lockTaken = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					Monitor.Enter(_sharedStatics, ref lockTaken);
					if (_sharedStatics._Remoting_Identity_IDGuid == null)
					{
						_sharedStatics._Remoting_Identity_IDGuid = Guid.NewGuid().ToString().Replace('-', '_');
					}
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(_sharedStatics);
					}
				}
			}
			return _sharedStatics._Remoting_Identity_IDGuid;
		}
	}

	internal static ulong MemoryFailPointReservedMemory => (ulong)Volatile.Read(ref _sharedStatics._memFailPointReservedMemory);

	private SharedStatics()
	{
	}

	[SecuritySafeCritical]
	public static Tokenizer.StringMaker GetSharedStringMaker()
	{
		Tokenizer.StringMaker stringMaker = null;
		bool lockTaken = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			Monitor.Enter(_sharedStatics, ref lockTaken);
			if (_sharedStatics._maker != null)
			{
				stringMaker = _sharedStatics._maker;
				_sharedStatics._maker = null;
			}
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(_sharedStatics);
			}
		}
		if (stringMaker == null)
		{
			stringMaker = new Tokenizer.StringMaker();
		}
		return stringMaker;
	}

	[SecuritySafeCritical]
	public static void ReleaseSharedStringMaker(ref Tokenizer.StringMaker maker)
	{
		bool lockTaken = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			Monitor.Enter(_sharedStatics, ref lockTaken);
			_sharedStatics._maker = maker;
			maker = null;
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(_sharedStatics);
			}
		}
	}

	internal static int Remoting_Identity_GetNextSeqNum()
	{
		return Interlocked.Increment(ref _sharedStatics._Remoting_Identity_IDSeqNum);
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal static long AddMemoryFailPointReservation(long size)
	{
		return Interlocked.Add(ref _sharedStatics._memFailPointReservedMemory, size);
	}
}
