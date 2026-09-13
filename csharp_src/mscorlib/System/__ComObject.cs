using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Mono.Interop;

namespace System;

[StructLayout(LayoutKind.Sequential)]
internal class __ComObject : MarshalByRefObject
{
	private IntPtr iunknown;

	private IntPtr hash_table;

	private SynchronizationContext synchronization_context;

	private ComInteropProxy proxy;

	internal IntPtr IUnknown
	{
		get
		{
			if (iunknown == IntPtr.Zero)
			{
				throw new InvalidComObjectException("COM object that has been separated from its underlying RCW cannot be used.");
			}
			return iunknown;
		}
	}

	internal IntPtr IDispatch
	{
		get
		{
			IntPtr intPtr = GetInterface(typeof(IDispatch));
			if (intPtr == IntPtr.Zero)
			{
				throw new InvalidComObjectException("COM object that has been separated from its underlying RCW cannot be used.");
			}
			return intPtr;
		}
	}

	internal static Guid IID_IUnknown => new Guid("00000000-0000-0000-C000-000000000046");

	internal static Guid IID_IDispatch => new Guid("00020400-0000-0000-C000-000000000046");

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern __ComObject CreateRCW(Type t);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private extern void ReleaseInterfaces();

	~__ComObject()
	{
		if (hash_table != IntPtr.Zero)
		{
			if (synchronization_context != null)
			{
				synchronization_context.Post(delegate
				{
					ReleaseInterfaces();
				}, this);
			}
			else
			{
				ReleaseInterfaces();
			}
		}
		proxy = null;
	}

	public __ComObject()
	{
		Initialize(GetType());
	}

	internal __ComObject(Type t)
	{
		Initialize(t);
	}

	internal __ComObject(IntPtr pItf, ComInteropProxy p)
	{
		proxy = p;
		InitializeApartmentDetails();
		Guid iid = IID_IUnknown;
		Marshal.ThrowExceptionForHR(Marshal.QueryInterface(pItf, ref iid, out iunknown));
	}

	internal void Initialize(IntPtr pUnk, ComInteropProxy p)
	{
		proxy = p;
		InitializeApartmentDetails();
		iunknown = pUnk;
	}

	internal void Initialize(Type t)
	{
		InitializeApartmentDetails();
		if (!(iunknown != IntPtr.Zero))
		{
			iunknown = CreateIUnknown(t);
		}
	}

	internal static IntPtr CreateIUnknown(Type t)
	{
		RuntimeHelpers.RunClassConstructor(t.TypeHandle);
		ObjectCreationDelegate objectCreationCallback = ExtensibleClassFactory.GetObjectCreationCallback(t);
		IntPtr pUnk;
		if (objectCreationCallback != null)
		{
			pUnk = objectCreationCallback(IntPtr.Zero);
			if (pUnk == IntPtr.Zero)
			{
				throw new COMException($"ObjectCreationDelegate for type {t} failed to return a valid COM object");
			}
		}
		else
		{
			Marshal.ThrowExceptionForHR(CoCreateInstance(GetCLSID(t), IntPtr.Zero, 21u, IID_IUnknown, out pUnk));
		}
		return pUnk;
	}

	private void InitializeApartmentDetails()
	{
		if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
		{
			synchronization_context = SynchronizationContext.Current;
			if (synchronization_context != null && synchronization_context.GetType() == typeof(SynchronizationContext))
			{
				synchronization_context = null;
			}
		}
	}

	private static Guid GetCLSID(Type t)
	{
		if (t.IsImport)
		{
			return t.GUID;
		}
		Type baseType = t.BaseType;
		while (baseType != typeof(object))
		{
			if (baseType.IsImport)
			{
				return baseType.GUID;
			}
			baseType = baseType.BaseType;
		}
		throw new COMException("Could not find base COM type for type " + t.ToString());
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern IntPtr GetInterfaceInternal(Type t, bool throwException);

	internal IntPtr GetInterface(Type t, bool throwException)
	{
		CheckIUnknown();
		return GetInterfaceInternal(t, throwException);
	}

	internal IntPtr GetInterface(Type t)
	{
		return GetInterface(t, throwException: true);
	}

	private void CheckIUnknown()
	{
		if (iunknown == IntPtr.Zero)
		{
			throw new InvalidComObjectException("COM object that has been separated from its underlying RCW cannot be used.");
		}
	}

	public override bool Equals(object obj)
	{
		CheckIUnknown();
		if (obj == null)
		{
			return false;
		}
		if (!(obj is __ComObject _ComObject))
		{
			return false;
		}
		return iunknown == _ComObject.IUnknown;
	}

	public override int GetHashCode()
	{
		CheckIUnknown();
		return iunknown.ToInt32();
	}

	[DllImport("ole32.dll", CallingConvention = CallingConvention.StdCall, ExactSpelling = true)]
	private static extern int CoCreateInstance([In][MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, IntPtr pUnkOuter, uint dwClsContext, [In][MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IntPtr pUnk);
}
