using System;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.SafeHandles;

public sealed class SafePipeHandle : SafeHandleZeroOrMinusOneIsInvalid
{
	private const int DefaultInvalidHandle = -1;

	private Socket _namedPipeSocket;

	private SafeHandle _namedPipeSocketHandle;

	private static PropertyInfo s_safeHandleProperty;

	internal Socket NamedPipeSocket => _namedPipeSocket;

	internal SafeHandle NamedPipeSocketHandle => _namedPipeSocketHandle;

	public override bool IsInvalid
	{
		get
		{
			if ((long)handle < 0)
			{
				return _namedPipeSocket == null;
			}
			return false;
		}
	}

	internal SafePipeHandle(Socket namedPipeSocket)
		: base(ownsHandle: true)
	{
		_namedPipeSocket = namedPipeSocket;
		_namedPipeSocketHandle = (SafeHandle)((s_safeHandleProperty ?? (s_safeHandleProperty = typeof(Socket).GetTypeInfo().GetDeclaredProperty("SafeHandle")))?.GetValue(namedPipeSocket, null));
		bool success = false;
		_namedPipeSocketHandle.DangerousAddRef(ref success);
		SetHandle(_namedPipeSocketHandle.DangerousGetHandle());
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		if (disposing && _namedPipeSocket != null)
		{
			_namedPipeSocket.Dispose();
			_namedPipeSocket = null;
		}
	}

	protected override bool ReleaseHandle()
	{
		if (_namedPipeSocketHandle != null)
		{
			SetHandle(-1);
			_namedPipeSocketHandle.DangerousRelease();
			_namedPipeSocketHandle = null;
			return true;
		}
		if ((long)handle < 0)
		{
			return true;
		}
		return global::Interop.Sys.Close(handle) == 0;
	}

	internal SafePipeHandle()
		: this(new IntPtr(-1), ownsHandle: true)
	{
	}

	public SafePipeHandle(IntPtr preexistingHandle, bool ownsHandle)
		: base(ownsHandle)
	{
		SetHandle(preexistingHandle);
	}

	internal void SetHandle(int descriptor)
	{
		SetHandle((IntPtr)descriptor);
	}
}
