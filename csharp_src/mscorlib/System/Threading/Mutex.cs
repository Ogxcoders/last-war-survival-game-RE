using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace System.Threading;

[ComVisible(true)]
public sealed class Mutex : WaitHandle
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern IntPtr CreateMutex_icall(bool initiallyOwned, char* name, int name_length, out bool created);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern IntPtr OpenMutex_icall(char* name, int name_length, MutexRights rights, out MonoIOError error);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool ReleaseMutex_internal(IntPtr handle);

	private unsafe static IntPtr CreateMutex_internal(bool initiallyOwned, string name, out bool created)
	{
		fixed (char* name2 = name)
		{
			return CreateMutex_icall(initiallyOwned, name2, name?.Length ?? 0, out created);
		}
	}

	private unsafe static IntPtr OpenMutex_internal(string name, MutexRights rights, out MonoIOError error)
	{
		fixed (char* name2 = name)
		{
			return OpenMutex_icall(name2, name?.Length ?? 0, rights, out error);
		}
	}

	private Mutex(IntPtr handle)
	{
		Handle = handle;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public Mutex()
	{
		Handle = CreateMutex_internal(initiallyOwned: false, null, out var _);
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public Mutex(bool initiallyOwned)
	{
		Handle = CreateMutex_internal(initiallyOwned, null, out var _);
	}

	public Mutex(bool initiallyOwned, string name)
	{
		throw new NotSupportedException();
	}

	public Mutex(bool initiallyOwned, string name, out bool createdNew)
	{
		throw new NotSupportedException();
	}

	public Mutex(bool initiallyOwned, string name, out bool createdNew, MutexSecurity mutexSecurity)
	{
		throw new NotSupportedException();
	}

	public static Mutex OpenExisting(string name)
	{
		throw new NotSupportedException();
	}

	public static Mutex OpenExisting(string name, MutexRights rights)
	{
		throw new NotSupportedException();
	}

	public static bool TryOpenExisting(string name, out Mutex result)
	{
		throw new NotSupportedException();
	}

	public static bool TryOpenExisting(string name, MutexRights rights, out Mutex result)
	{
		throw new NotSupportedException();
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
	public void ReleaseMutex()
	{
		if (!ReleaseMutex_internal(Handle))
		{
			throw new ApplicationException("Mutex is not owned");
		}
	}
}
