using System.Threading;

namespace System.Net;

internal struct InterlockedGate
{
	private int m_State;

	internal const int Open = 0;

	internal const int Triggering = 1;

	internal const int Triggered = 2;

	internal const int Signaling = 3;

	internal const int Signaled = 4;

	internal const int Completed = 5;

	internal void Reset()
	{
		m_State = 0;
	}

	internal bool Trigger(bool exclusive)
	{
		int num = Interlocked.CompareExchange(ref m_State, 2, 0);
		if (exclusive && (num == 1 || num == 2))
		{
			throw new InternalException();
		}
		return num == 0;
	}

	internal bool StartTriggering(bool exclusive)
	{
		int num = Interlocked.CompareExchange(ref m_State, 1, 0);
		if (exclusive && (num == 1 || num == 2))
		{
			throw new InternalException();
		}
		return num == 0;
	}

	internal void FinishTriggering()
	{
		if (Interlocked.CompareExchange(ref m_State, 2, 1) != 1)
		{
			throw new InternalException();
		}
	}

	internal bool StartSignaling(bool exclusive)
	{
		int num = Interlocked.CompareExchange(ref m_State, 3, 2);
		if (exclusive && (num == 3 || num == 4))
		{
			throw new InternalException();
		}
		return num == 2;
	}

	internal void FinishSignaling()
	{
		if (Interlocked.CompareExchange(ref m_State, 4, 3) != 3)
		{
			throw new InternalException();
		}
	}

	internal bool Complete()
	{
		return Interlocked.CompareExchange(ref m_State, 5, 4) == 4;
	}
}
