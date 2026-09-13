using System.Threading;

namespace Internal.Runtime.Augments;

internal sealed class RuntimeThread
{
	internal static readonly int OptimalMaxSpinWaitsPerSpinIteration = 64;

	private readonly Thread thread;

	public bool IsBackground
	{
		get
		{
			return thread.IsBackground;
		}
		set
		{
			thread.IsBackground = value;
		}
	}

	private RuntimeThread(Thread t)
	{
		thread = t;
	}

	public void ResetThreadPoolThread()
	{
	}

	public static RuntimeThread InitializeThreadPoolThread()
	{
		return new RuntimeThread(null);
	}

	public static RuntimeThread Create(ParameterizedThreadStart start, int maxStackSize)
	{
		return new RuntimeThread(new Thread(start, maxStackSize));
	}

	public void Start()
	{
		thread.Start();
	}

	public void Start(object state)
	{
		thread.Start(state);
	}

	public static void Sleep(int millisecondsTimeout)
	{
		Thread.Sleep(millisecondsTimeout);
	}

	public static bool Yield()
	{
		return Thread.Yield();
	}

	public static bool SpinWait(int iterations)
	{
		Thread.SpinWait(iterations);
		return true;
	}

	public static int GetCurrentProcessorId()
	{
		return 1;
	}
}
