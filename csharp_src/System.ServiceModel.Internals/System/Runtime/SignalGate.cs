using System.Threading;

namespace System.Runtime;

internal class SignalGate
{
	private static class GateState
	{
		public const int Locked = 0;

		public const int SignalPending = 1;

		public const int Unlocked = 2;

		public const int Signalled = 3;
	}

	private int state;

	internal bool IsLocked => state == 0;

	internal bool IsSignalled => state == 3;

	public bool Signal()
	{
		int num = state;
		if (num == 0)
		{
			num = Interlocked.CompareExchange(ref state, 1, 0);
		}
		switch (num)
		{
		case 2:
			state = 3;
			return true;
		default:
			ThrowInvalidSignalGateState();
			break;
		case 0:
			break;
		}
		return false;
	}

	public bool Unlock()
	{
		int num = state;
		if (num == 0)
		{
			num = Interlocked.CompareExchange(ref state, 2, 0);
		}
		switch (num)
		{
		case 1:
			state = 3;
			return true;
		default:
			ThrowInvalidSignalGateState();
			break;
		case 0:
			break;
		}
		return false;
	}

	private void ThrowInvalidSignalGateState()
	{
		throw Fx.Exception.AsError(new InvalidOperationException("Invalid Semaphore Exit"));
	}
}
internal class SignalGate<T> : SignalGate
{
	private T result;

	public bool Signal(T result)
	{
		this.result = result;
		return Signal();
	}

	public bool Unlock(out T result)
	{
		if (Unlock())
		{
			result = this.result;
			return true;
		}
		result = default(T);
		return false;
	}
}
