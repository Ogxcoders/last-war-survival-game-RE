using System.Threading;

namespace System.Net;

internal sealed class Semaphore : WaitHandle
{
	internal Semaphore(int initialCount, int maxCount)
	{
		lock (this)
		{
			Handle = System.Threading.Semaphore.CreateSemaphore_internal(initialCount, maxCount, null, out var _);
		}
	}

	internal bool ReleaseSemaphore()
	{
		int previousCount;
		return System.Threading.Semaphore.ReleaseSemaphore_internal(Handle, 1, out previousCount);
	}
}
