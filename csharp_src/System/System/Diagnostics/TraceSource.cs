using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Diagnostics;

public class TraceSource
{
	private static List<WeakReference> tracesources = new List<WeakReference>();

	private static int s_LastCollectionCount;

	private volatile SourceSwitch internalSwitch;

	private volatile TraceListenerCollection listeners;

	private StringDictionary attributes;

	private SourceLevels switchLevel;

	private volatile string sourceName;

	internal volatile bool _initCalled;

	public StringDictionary Attributes
	{
		get
		{
			Initialize();
			if (attributes == null)
			{
				attributes = new StringDictionary();
			}
			return attributes;
		}
	}

	public string Name => sourceName;

	public TraceListenerCollection Listeners
	{
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		get
		{
			Initialize();
			return listeners;
		}
	}

	public SourceSwitch Switch
	{
		get
		{
			Initialize();
			return internalSwitch;
		}
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("Switch");
			}
			Initialize();
			internalSwitch = value;
		}
	}

	public TraceSource(string name)
		: this(name, SourceLevels.Off)
	{
	}

	public TraceSource(string name, SourceLevels defaultLevel)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (name.Length == 0)
		{
			throw new ArgumentException("name");
		}
		sourceName = name;
		switchLevel = defaultLevel;
		lock (tracesources)
		{
			_pruneCachedTraceSources();
			tracesources.Add(new WeakReference(this));
		}
	}

	private static void _pruneCachedTraceSources()
	{
		lock (tracesources)
		{
			if (s_LastCollectionCount == GC.CollectionCount(2))
			{
				return;
			}
			List<WeakReference> list = new List<WeakReference>(tracesources.Count);
			for (int i = 0; i < tracesources.Count; i++)
			{
				if ((TraceSource)tracesources[i].Target != null)
				{
					list.Add(tracesources[i]);
				}
			}
			if (list.Count < tracesources.Count)
			{
				tracesources.Clear();
				tracesources.AddRange(list);
				tracesources.TrimExcess();
			}
			s_LastCollectionCount = GC.CollectionCount(2);
		}
	}

	private void Initialize()
	{
		if (_initCalled)
		{
			return;
		}
		lock (this)
		{
			if (!_initCalled)
			{
				NoConfigInit();
				_initCalled = true;
			}
		}
	}

	private void NoConfigInit()
	{
		internalSwitch = new SourceSwitch(sourceName, switchLevel.ToString());
		listeners = new TraceListenerCollection();
		listeners.Add(new DefaultTraceListener());
		attributes = null;
	}

	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public void Close()
	{
		if (listeners == null)
		{
			return;
		}
		lock (TraceInternal.critSec)
		{
			foreach (TraceListener listener in listeners)
			{
				listener.Close();
			}
		}
	}

	public void Flush()
	{
		if (listeners == null)
		{
			return;
		}
		if (TraceInternal.UseGlobalLock)
		{
			lock (TraceInternal.critSec)
			{
				foreach (TraceListener listener in listeners)
				{
					listener.Flush();
				}
				return;
			}
		}
		foreach (TraceListener listener2 in listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.Flush();
				}
			}
			else
			{
				listener2.Flush();
			}
		}
	}

	protected internal virtual string[] GetSupportedAttributes()
	{
		return null;
	}

	internal static void RefreshAll()
	{
		lock (tracesources)
		{
			_pruneCachedTraceSources();
			for (int i = 0; i < tracesources.Count; i++)
			{
				((TraceSource)tracesources[i].Target)?.Refresh();
			}
		}
	}

	internal void Refresh()
	{
		if (!_initCalled)
		{
			Initialize();
		}
	}

	[Conditional("TRACE")]
	public void TraceEvent(TraceEventType eventType, int id)
	{
		Initialize();
		TraceEventCache eventCache = new TraceEventCache();
		if (!internalSwitch.ShouldTrace(eventType) || listeners == null)
		{
			return;
		}
		if (TraceInternal.UseGlobalLock)
		{
			lock (TraceInternal.critSec)
			{
				for (int i = 0; i < listeners.Count; i++)
				{
					TraceListener traceListener = listeners[i];
					traceListener.TraceEvent(eventCache, Name, eventType, id);
					if (Trace.AutoFlush)
					{
						traceListener.Flush();
					}
				}
				return;
			}
		}
		for (int j = 0; j < listeners.Count; j++)
		{
			TraceListener traceListener2 = listeners[j];
			if (!traceListener2.IsThreadSafe)
			{
				lock (traceListener2)
				{
					traceListener2.TraceEvent(eventCache, Name, eventType, id);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
					}
				}
			}
			else
			{
				traceListener2.TraceEvent(eventCache, Name, eventType, id);
				if (Trace.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}
	}

	[Conditional("TRACE")]
	public void TraceEvent(TraceEventType eventType, int id, string message)
	{
		Initialize();
		TraceEventCache eventCache = new TraceEventCache();
		if (!internalSwitch.ShouldTrace(eventType) || listeners == null)
		{
			return;
		}
		if (TraceInternal.UseGlobalLock)
		{
			lock (TraceInternal.critSec)
			{
				for (int i = 0; i < listeners.Count; i++)
				{
					TraceListener traceListener = listeners[i];
					traceListener.TraceEvent(eventCache, Name, eventType, id, message);
					if (Trace.AutoFlush)
					{
						traceListener.Flush();
					}
				}
				return;
			}
		}
		for (int j = 0; j < listeners.Count; j++)
		{
			TraceListener traceListener2 = listeners[j];
			if (!traceListener2.IsThreadSafe)
			{
				lock (traceListener2)
				{
					traceListener2.TraceEvent(eventCache, Name, eventType, id, message);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
					}
				}
			}
			else
			{
				traceListener2.TraceEvent(eventCache, Name, eventType, id, message);
				if (Trace.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}
	}

	[Conditional("TRACE")]
	public void TraceEvent(TraceEventType eventType, int id, string format, params object[] args)
	{
		Initialize();
		TraceEventCache eventCache = new TraceEventCache();
		if (!internalSwitch.ShouldTrace(eventType) || listeners == null)
		{
			return;
		}
		if (TraceInternal.UseGlobalLock)
		{
			lock (TraceInternal.critSec)
			{
				for (int i = 0; i < listeners.Count; i++)
				{
					TraceListener traceListener = listeners[i];
					traceListener.TraceEvent(eventCache, Name, eventType, id, format, args);
					if (Trace.AutoFlush)
					{
						traceListener.Flush();
					}
				}
				return;
			}
		}
		for (int j = 0; j < listeners.Count; j++)
		{
			TraceListener traceListener2 = listeners[j];
			if (!traceListener2.IsThreadSafe)
			{
				lock (traceListener2)
				{
					traceListener2.TraceEvent(eventCache, Name, eventType, id, format, args);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
					}
				}
			}
			else
			{
				traceListener2.TraceEvent(eventCache, Name, eventType, id, format, args);
				if (Trace.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}
	}

	[Conditional("TRACE")]
	public void TraceData(TraceEventType eventType, int id, object data)
	{
		Initialize();
		TraceEventCache eventCache = new TraceEventCache();
		if (!internalSwitch.ShouldTrace(eventType) || listeners == null)
		{
			return;
		}
		if (TraceInternal.UseGlobalLock)
		{
			lock (TraceInternal.critSec)
			{
				for (int i = 0; i < listeners.Count; i++)
				{
					TraceListener traceListener = listeners[i];
					traceListener.TraceData(eventCache, Name, eventType, id, data);
					if (Trace.AutoFlush)
					{
						traceListener.Flush();
					}
				}
				return;
			}
		}
		for (int j = 0; j < listeners.Count; j++)
		{
			TraceListener traceListener2 = listeners[j];
			if (!traceListener2.IsThreadSafe)
			{
				lock (traceListener2)
				{
					traceListener2.TraceData(eventCache, Name, eventType, id, data);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
					}
				}
			}
			else
			{
				traceListener2.TraceData(eventCache, Name, eventType, id, data);
				if (Trace.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}
	}

	[Conditional("TRACE")]
	public void TraceData(TraceEventType eventType, int id, params object[] data)
	{
		Initialize();
		TraceEventCache eventCache = new TraceEventCache();
		if (!internalSwitch.ShouldTrace(eventType) || listeners == null)
		{
			return;
		}
		if (TraceInternal.UseGlobalLock)
		{
			lock (TraceInternal.critSec)
			{
				for (int i = 0; i < listeners.Count; i++)
				{
					TraceListener traceListener = listeners[i];
					traceListener.TraceData(eventCache, Name, eventType, id, data);
					if (Trace.AutoFlush)
					{
						traceListener.Flush();
					}
				}
				return;
			}
		}
		for (int j = 0; j < listeners.Count; j++)
		{
			TraceListener traceListener2 = listeners[j];
			if (!traceListener2.IsThreadSafe)
			{
				lock (traceListener2)
				{
					traceListener2.TraceData(eventCache, Name, eventType, id, data);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
					}
				}
			}
			else
			{
				traceListener2.TraceData(eventCache, Name, eventType, id, data);
				if (Trace.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}
	}

	[Conditional("TRACE")]
	public void TraceInformation(string message)
	{
	}

	[Conditional("TRACE")]
	public void TraceInformation(string format, params object[] args)
	{
	}

	[Conditional("TRACE")]
	public void TraceTransfer(int id, string message, Guid relatedActivityId)
	{
		Initialize();
		TraceEventCache eventCache = new TraceEventCache();
		if (!internalSwitch.ShouldTrace(TraceEventType.Transfer) || listeners == null)
		{
			return;
		}
		if (TraceInternal.UseGlobalLock)
		{
			lock (TraceInternal.critSec)
			{
				for (int i = 0; i < listeners.Count; i++)
				{
					TraceListener traceListener = listeners[i];
					traceListener.TraceTransfer(eventCache, Name, id, message, relatedActivityId);
					if (Trace.AutoFlush)
					{
						traceListener.Flush();
					}
				}
				return;
			}
		}
		for (int j = 0; j < listeners.Count; j++)
		{
			TraceListener traceListener2 = listeners[j];
			if (!traceListener2.IsThreadSafe)
			{
				lock (traceListener2)
				{
					traceListener2.TraceTransfer(eventCache, Name, id, message, relatedActivityId);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
					}
				}
			}
			else
			{
				traceListener2.TraceTransfer(eventCache, Name, id, message, relatedActivityId);
				if (Trace.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}
	}

	private void CreateSwitch(string typename, string name)
	{
		if (!string.IsNullOrEmpty(typename))
		{
			internalSwitch = (SourceSwitch)TraceUtils.GetRuntimeObject(typename, typeof(SourceSwitch), name);
		}
		else
		{
			internalSwitch = new SourceSwitch(name, switchLevel.ToString());
		}
	}
}
