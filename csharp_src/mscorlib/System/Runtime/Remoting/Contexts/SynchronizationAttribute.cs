using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace System.Runtime.Remoting.Contexts;

[Serializable]
[AttributeUsage(AttributeTargets.Class)]
[ComVisible(true)]
public class SynchronizationAttribute : ContextAttribute, IContributeClientContextSink, IContributeServerContextSink
{
	public const int NOT_SUPPORTED = 1;

	public const int SUPPORTED = 2;

	public const int REQUIRED = 4;

	public const int REQUIRES_NEW = 8;

	private bool _bReEntrant;

	private int _flavor;

	[NonSerialized]
	private int _lockCount;

	[NonSerialized]
	private Mutex _mutex = new Mutex(initiallyOwned: false);

	[NonSerialized]
	private Thread _ownerThread;

	public virtual bool IsReEntrant => _bReEntrant;

	public virtual bool Locked
	{
		get
		{
			return _lockCount > 0;
		}
		set
		{
			if (value)
			{
				AcquireLock();
				lock (this)
				{
					if (_lockCount > 1)
					{
						ReleaseLock();
					}
					return;
				}
			}
			lock (this)
			{
				while (_lockCount > 0 && _ownerThread == Thread.CurrentThread)
				{
					ReleaseLock();
				}
			}
		}
	}

	public SynchronizationAttribute()
		: this(8, reEntrant: false)
	{
	}

	public SynchronizationAttribute(bool reEntrant)
		: this(8, reEntrant)
	{
	}

	public SynchronizationAttribute(int flag)
		: this(flag, reEntrant: false)
	{
	}

	public SynchronizationAttribute(int flag, bool reEntrant)
		: base("Synchronization")
	{
		if (flag != 1 && flag != 4 && flag != 8 && flag != 2)
		{
			throw new ArgumentException("flag");
		}
		_bReEntrant = reEntrant;
		_flavor = flag;
	}

	internal void AcquireLock()
	{
		_mutex.WaitOne();
		lock (this)
		{
			_ownerThread = Thread.CurrentThread;
			_lockCount++;
		}
	}

	internal void ReleaseLock()
	{
		lock (this)
		{
			if (_lockCount > 0 && _ownerThread == Thread.CurrentThread)
			{
				_lockCount--;
				_mutex.ReleaseMutex();
				if (_lockCount == 0)
				{
					_ownerThread = null;
				}
			}
		}
	}

	[ComVisible(true)]
	public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
	{
		if (_flavor != 1)
		{
			ctorMsg.ContextProperties.Add(this);
		}
	}

	public virtual IMessageSink GetClientContextSink(IMessageSink nextSink)
	{
		return new SynchronizedClientContextSink(nextSink, this);
	}

	public virtual IMessageSink GetServerContextSink(IMessageSink nextSink)
	{
		return new SynchronizedServerContextSink(nextSink, this);
	}

	[ComVisible(true)]
	public override bool IsContextOK(Context ctx, IConstructionCallMessage msg)
	{
		SynchronizationAttribute synchronizationAttribute = ctx.GetProperty("Synchronization") as SynchronizationAttribute;
		return _flavor switch
		{
			1 => synchronizationAttribute == null, 
			4 => synchronizationAttribute != null, 
			8 => false, 
			2 => true, 
			_ => false, 
		};
	}

	internal static void ExitContext()
	{
		if (!Thread.CurrentContext.IsDefaultContext && Thread.CurrentContext.GetProperty("Synchronization") is SynchronizationAttribute synchronizationAttribute)
		{
			synchronizationAttribute.Locked = false;
		}
	}

	internal static void EnterContext()
	{
		if (!Thread.CurrentContext.IsDefaultContext && Thread.CurrentContext.GetProperty("Synchronization") is SynchronizationAttribute synchronizationAttribute)
		{
			synchronizationAttribute.Locked = true;
		}
	}
}
