using System;
using UnityEngine;

namespace RiverGame.PerformanceAnalysis;

public static class PerformanceMemoryMetrics
{
	public static long GetMemoryUsageLong()
	{
		long result = 0L;
		try
		{
			long num = 0L;
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.io.FileInputStream", "/proc/self/statm"))
			{
				try
				{
					using AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.util.Scanner", androidJavaObject);
					try
					{
						androidJavaObject2.Call<long>("nextLong", Array.Empty<object>());
						num = androidJavaObject2.Call<long>("nextLong", Array.Empty<object>()) << 12;
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
					finally
					{
						androidJavaObject2?.Call("close");
					}
				}
				catch (Exception exception2)
				{
					Debug.LogException(exception2);
				}
				finally
				{
					androidJavaObject?.Call("close");
				}
			}
			result = num;
		}
		catch (Exception exception3)
		{
			Debug.LogException(exception3);
		}
		finally
		{
		}
		return result;
	}
}
