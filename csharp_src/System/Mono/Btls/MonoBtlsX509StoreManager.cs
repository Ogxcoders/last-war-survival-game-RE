using System;

namespace Mono.Btls;

internal static class MonoBtlsX509StoreManager
{
	private static bool initialized;

	private static void Initialize()
	{
		if (initialized)
		{
			return;
		}
		try
		{
			DoInitialize();
		}
		catch (Exception arg)
		{
			Console.Error.WriteLine("MonoBtlsX509StoreManager.Initialize() threw exception: {0}", arg);
		}
		finally
		{
			initialized = true;
		}
	}

	private static void DoInitialize()
	{
	}

	public static bool HasStore(MonoBtlsX509StoreType type)
	{
		return false;
	}

	public static string GetStorePath(MonoBtlsX509StoreType type)
	{
		throw new NotSupportedException();
	}
}
