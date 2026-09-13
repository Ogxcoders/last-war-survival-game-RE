using System.Data.Common;
using System.Threading;

namespace System.Data.SqlClient;

internal sealed class TdsParserStaticMethods
{
	private const int NoProcessId = -1;

	private static int s_currentProcessId = -1;

	private static byte[] s_nicAddress = null;

	internal static byte[] ObfuscatePassword(string password)
	{
		byte[] array = new byte[password.Length << 1];
		for (int i = 0; i < password.Length; i++)
		{
			char num = password[i];
			byte b = (byte)(num & 0xFF);
			byte b2 = (byte)(((int)num >> 8) & 0xFF);
			array[i << 1] = (byte)((((b & 0xF) << 4) | (b >> 4)) ^ 0xA5);
			array[(i << 1) + 1] = (byte)((((b2 & 0xF) << 4) | (b2 >> 4)) ^ 0xA5);
		}
		return array;
	}

	internal static byte[] ObfuscatePassword(byte[] password)
	{
		for (int i = 0; i < password.Length; i++)
		{
			byte b = (byte)(password[i] & 0xF);
			byte b2 = (byte)(password[i] & 0xF0);
			password[i] = (byte)(((b2 >> 4) | (b << 4)) ^ 0xA5);
		}
		return password;
	}

	internal static int GetCurrentProcessIdForTdsLoginOnly()
	{
		if (s_currentProcessId == -1)
		{
			int value = new Random().Next();
			Interlocked.CompareExchange(ref s_currentProcessId, value, -1);
		}
		return s_currentProcessId;
	}

	internal static int GetCurrentThreadIdForTdsLoginOnly()
	{
		return Environment.CurrentManagedThreadId;
	}

	internal static byte[] GetNetworkPhysicalAddressForTdsLoginOnly()
	{
		if (s_nicAddress == null)
		{
			byte[] array = new byte[6];
			new Random().NextBytes(array);
			Interlocked.CompareExchange(ref s_nicAddress, array, null);
		}
		return s_nicAddress;
	}

	internal static int GetTimeoutMilliseconds(long timeoutTime)
	{
		if (long.MaxValue == timeoutTime)
		{
			return -1;
		}
		long num = ADP.TimerRemainingMilliseconds(timeoutTime);
		if (num < 0)
		{
			return 0;
		}
		if (num > int.MaxValue)
		{
			return int.MaxValue;
		}
		return (int)num;
	}

	internal static long GetTimeout(long timeoutMilliseconds)
	{
		if (timeoutMilliseconds <= 0)
		{
			return long.MaxValue;
		}
		try
		{
			return checked(ADP.TimerCurrent() + ADP.TimerFromMilliseconds(timeoutMilliseconds));
		}
		catch (OverflowException)
		{
			return long.MaxValue;
		}
	}

	internal static bool TimeoutHasExpired(long timeoutTime)
	{
		bool result = false;
		if (timeoutTime != 0L && long.MaxValue != timeoutTime)
		{
			result = ADP.TimerHasExpired(timeoutTime);
		}
		return result;
	}

	internal static int NullAwareStringLength(string str)
	{
		return str?.Length ?? 0;
	}

	internal static int GetRemainingTimeout(int timeout, long start)
	{
		if (timeout <= 0)
		{
			return timeout;
		}
		long num = ADP.TimerRemainingSeconds(start + ADP.TimerFromSeconds(timeout));
		if (num <= 0)
		{
			return 1;
		}
		return checked((int)num);
	}
}
