using System.Security;

namespace System.Threading;

public struct AsyncFlowControl : IDisposable
{
	private bool useEC;

	private ExecutionContext _ec;

	private Thread _thread;

	[SecurityCritical]
	internal void Setup()
	{
		useEC = true;
		Thread currentThread = Thread.CurrentThread;
		_ec = currentThread.GetMutableExecutionContext();
		_ec.isFlowSuppressed = true;
		_thread = currentThread;
	}

	public void Dispose()
	{
		Undo();
	}

	[SecuritySafeCritical]
	public void Undo()
	{
		if (_thread == null)
		{
			throw new InvalidOperationException(Environment.GetResourceString("AsyncFlowControl object can be used only once to call Undo()."));
		}
		if (_thread != Thread.CurrentThread)
		{
			throw new InvalidOperationException(Environment.GetResourceString("AsyncFlowControl object must be used on the thread where it was created."));
		}
		if (useEC)
		{
			if (Thread.CurrentThread.GetMutableExecutionContext() != _ec)
			{
				throw new InvalidOperationException(Environment.GetResourceString("AsyncFlowControl objects can be used to restore flow only on the Context that had its flow suppressed."));
			}
			ExecutionContext.RestoreFlow();
		}
		_thread = null;
	}

	public override int GetHashCode()
	{
		if (_thread != null)
		{
			return _thread.GetHashCode();
		}
		return ToString().GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj is AsyncFlowControl)
		{
			return Equals((AsyncFlowControl)obj);
		}
		return false;
	}

	public bool Equals(AsyncFlowControl obj)
	{
		if (obj.useEC == useEC && obj._ec == _ec)
		{
			return obj._thread == _thread;
		}
		return false;
	}

	public static bool operator ==(AsyncFlowControl a, AsyncFlowControl b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(AsyncFlowControl a, AsyncFlowControl b)
	{
		return !(a == b);
	}
}
