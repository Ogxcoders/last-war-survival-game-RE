using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Threading;

namespace Mono.Interop;

[StructLayout(LayoutKind.Sequential)]
internal class ComInteropProxy : RealProxy, IRemotingTypeInfo
{
	private __ComObject com_object;

	private int ref_count = 1;

	private string type_name;

	public string TypeName
	{
		get
		{
			return type_name;
		}
		set
		{
			type_name = value;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void AddProxy(IntPtr pItf, ref ComInteropProxy proxy);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void FindProxy(IntPtr pItf, ref ComInteropProxy proxy);

	private ComInteropProxy(Type t)
		: base(t)
	{
		com_object = __ComObject.CreateRCW(t);
	}

	private void CacheProxy()
	{
		ComInteropProxy proxy = null;
		FindProxy(com_object.IUnknown, ref proxy);
		if (proxy == null)
		{
			ComInteropProxy proxy2 = this;
			AddProxy(com_object.IUnknown, ref proxy2);
		}
		else
		{
			Interlocked.Increment(ref ref_count);
		}
	}

	private ComInteropProxy(IntPtr pUnk)
		: this(pUnk, typeof(__ComObject))
	{
	}

	internal ComInteropProxy(IntPtr pUnk, Type t)
		: base(t)
	{
		com_object = new __ComObject(pUnk, this);
		CacheProxy();
	}

	internal static ComInteropProxy GetProxy(IntPtr pItf, Type t)
	{
		Guid iid = __ComObject.IID_IUnknown;
		Marshal.ThrowExceptionForHR(Marshal.QueryInterface(pItf, ref iid, out var ppv));
		ComInteropProxy proxy = null;
		FindProxy(ppv, ref proxy);
		if (proxy == null)
		{
			Marshal.Release(ppv);
			return new ComInteropProxy(ppv);
		}
		Marshal.Release(ppv);
		Interlocked.Increment(ref proxy.ref_count);
		return proxy;
	}

	internal static ComInteropProxy CreateProxy(Type t)
	{
		IntPtr intPtr = __ComObject.CreateIUnknown(t);
		ComInteropProxy proxy = null;
		FindProxy(intPtr, ref proxy);
		ComInteropProxy comInteropProxy;
		if (proxy != null)
		{
			Type type = proxy.com_object.GetType();
			if (type != t)
			{
				throw new InvalidCastException($"Unable to cast object of type '{type}' to type '{t}'.");
			}
			comInteropProxy = proxy;
			Marshal.Release(intPtr);
		}
		else
		{
			comInteropProxy = new ComInteropProxy(t);
			comInteropProxy.com_object.Initialize(intPtr, comInteropProxy);
		}
		return comInteropProxy;
	}

	public override IMessage Invoke(IMessage msg)
	{
		throw new Exception("The method or operation is not implemented.");
	}

	public bool CanCastTo(Type fromType, object o)
	{
		if (!(o is __ComObject _ComObject))
		{
			throw new NotSupportedException("Only RCWs are currently supported");
		}
		if ((fromType.Attributes & TypeAttributes.Import) == 0)
		{
			return false;
		}
		if (_ComObject.GetInterface(fromType, throwException: false) == IntPtr.Zero)
		{
			return false;
		}
		return true;
	}
}
