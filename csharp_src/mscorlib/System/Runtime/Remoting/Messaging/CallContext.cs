using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Messaging;

[Serializable]
[SecurityCritical]
[ComVisible(true)]
public sealed class CallContext
{
	internal static IPrincipal Principal
	{
		[SecurityCritical]
		get
		{
			return Thread.CurrentThread.GetExecutionContextReader().LogicalCallContext.Principal;
		}
		[SecurityCritical]
		set
		{
			Thread.CurrentThread.GetMutableExecutionContext().LogicalCallContext.Principal = value;
		}
	}

	public static object HostContext
	{
		[SecurityCritical]
		get
		{
			ExecutionContext.Reader executionContextReader = Thread.CurrentThread.GetExecutionContextReader();
			object hostContext = executionContextReader.IllogicalCallContext.HostContext;
			if (hostContext == null)
			{
				hostContext = executionContextReader.LogicalCallContext.HostContext;
			}
			return hostContext;
		}
		[SecurityCritical]
		set
		{
			ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
			if (value is ILogicalThreadAffinative)
			{
				mutableExecutionContext.IllogicalCallContext.HostContext = null;
				mutableExecutionContext.LogicalCallContext.HostContext = value;
			}
			else
			{
				mutableExecutionContext.IllogicalCallContext.HostContext = value;
				mutableExecutionContext.LogicalCallContext.HostContext = null;
			}
		}
	}

	private CallContext()
	{
	}

	internal static object SetCurrentCallContext(LogicalCallContext ctx)
	{
		return null;
	}

	internal static LogicalCallContext SetLogicalCallContext(LogicalCallContext callCtx)
	{
		ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
		LogicalCallContext logicalCallContext = mutableExecutionContext.LogicalCallContext;
		mutableExecutionContext.LogicalCallContext = callCtx;
		return logicalCallContext;
	}

	[SecurityCritical]
	public static void FreeNamedDataSlot(string name)
	{
		ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
		mutableExecutionContext.LogicalCallContext.FreeNamedDataSlot(name);
		mutableExecutionContext.IllogicalCallContext.FreeNamedDataSlot(name);
	}

	[SecurityCritical]
	public static object LogicalGetData(string name)
	{
		return Thread.CurrentThread.GetExecutionContextReader().LogicalCallContext.GetData(name);
	}

	private static object IllogicalGetData(string name)
	{
		return Thread.CurrentThread.GetExecutionContextReader().IllogicalCallContext.GetData(name);
	}

	[SecurityCritical]
	public static object GetData(string name)
	{
		object obj = LogicalGetData(name);
		if (obj == null)
		{
			return IllogicalGetData(name);
		}
		return obj;
	}

	[SecurityCritical]
	public static void SetData(string name, object data)
	{
		if (data is ILogicalThreadAffinative)
		{
			LogicalSetData(name, data);
			return;
		}
		ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
		mutableExecutionContext.LogicalCallContext.FreeNamedDataSlot(name);
		mutableExecutionContext.IllogicalCallContext.SetData(name, data);
	}

	[SecurityCritical]
	public static void LogicalSetData(string name, object data)
	{
		ExecutionContext mutableExecutionContext = Thread.CurrentThread.GetMutableExecutionContext();
		mutableExecutionContext.IllogicalCallContext.FreeNamedDataSlot(name);
		mutableExecutionContext.LogicalCallContext.SetData(name, data);
	}

	[SecurityCritical]
	public static Header[] GetHeaders()
	{
		return Thread.CurrentThread.GetMutableExecutionContext().LogicalCallContext.InternalGetHeaders();
	}

	[SecurityCritical]
	public static void SetHeaders(Header[] headers)
	{
		Thread.CurrentThread.GetMutableExecutionContext().LogicalCallContext.InternalSetHeaders(headers);
	}
}
