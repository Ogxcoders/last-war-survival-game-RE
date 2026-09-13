using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System;

[ComVisible(true)]
public struct ModuleHandle
{
	private IntPtr value;

	public static readonly ModuleHandle EmptyHandle = new ModuleHandle(IntPtr.Zero);

	internal IntPtr Value => value;

	public int MDStreamVersion
	{
		get
		{
			if (value == IntPtr.Zero)
			{
				throw new ArgumentNullException(string.Empty, "Invalid handle");
			}
			return RuntimeModule.GetMDStreamVersion(value);
		}
	}

	internal ModuleHandle(IntPtr v)
	{
		value = v;
	}

	internal void GetPEKind(out PortableExecutableKinds peKind, out ImageFileMachine machine)
	{
		if (value == IntPtr.Zero)
		{
			throw new ArgumentNullException(string.Empty, "Invalid handle");
		}
		RuntimeModule.GetPEKind(value, out peKind, out machine);
	}

	public RuntimeFieldHandle ResolveFieldHandle(int fieldToken)
	{
		return ResolveFieldHandle(fieldToken, null, null);
	}

	public RuntimeMethodHandle ResolveMethodHandle(int methodToken)
	{
		return ResolveMethodHandle(methodToken, null, null);
	}

	public RuntimeTypeHandle ResolveTypeHandle(int typeToken)
	{
		return ResolveTypeHandle(typeToken, null, null);
	}

	private IntPtr[] ptrs_from_handles(RuntimeTypeHandle[] handles)
	{
		if (handles == null)
		{
			return null;
		}
		IntPtr[] array = new IntPtr[handles.Length];
		for (int i = 0; i < handles.Length; i++)
		{
			array[i] = handles[i].Value;
		}
		return array;
	}

	public RuntimeTypeHandle ResolveTypeHandle(int typeToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
	{
		if (value == IntPtr.Zero)
		{
			throw new ArgumentNullException(string.Empty, "Invalid handle");
		}
		ResolveTokenError error;
		IntPtr intPtr = RuntimeModule.ResolveTypeToken(value, typeToken, ptrs_from_handles(typeInstantiationContext), ptrs_from_handles(methodInstantiationContext), out error);
		if (intPtr == IntPtr.Zero)
		{
			throw new TypeLoadException($"Could not load type '0x{typeToken:x}' from assembly '0x{value.ToInt64():x}'");
		}
		return new RuntimeTypeHandle(intPtr);
	}

	public RuntimeMethodHandle ResolveMethodHandle(int methodToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
	{
		if (value == IntPtr.Zero)
		{
			throw new ArgumentNullException(string.Empty, "Invalid handle");
		}
		ResolveTokenError error;
		IntPtr intPtr = RuntimeModule.ResolveMethodToken(value, methodToken, ptrs_from_handles(typeInstantiationContext), ptrs_from_handles(methodInstantiationContext), out error);
		if (intPtr == IntPtr.Zero)
		{
			throw new Exception($"Could not load method '0x{methodToken:x}' from assembly '0x{value.ToInt64():x}'");
		}
		return new RuntimeMethodHandle(intPtr);
	}

	public RuntimeFieldHandle ResolveFieldHandle(int fieldToken, RuntimeTypeHandle[] typeInstantiationContext, RuntimeTypeHandle[] methodInstantiationContext)
	{
		if (value == IntPtr.Zero)
		{
			throw new ArgumentNullException(string.Empty, "Invalid handle");
		}
		ResolveTokenError error;
		IntPtr intPtr = RuntimeModule.ResolveFieldToken(value, fieldToken, ptrs_from_handles(typeInstantiationContext), ptrs_from_handles(methodInstantiationContext), out error);
		if (intPtr == IntPtr.Zero)
		{
			throw new Exception($"Could not load field '0x{fieldToken:x}' from assembly '0x{value.ToInt64():x}'");
		}
		return new RuntimeFieldHandle(intPtr);
	}

	public RuntimeFieldHandle GetRuntimeFieldHandleFromMetadataToken(int fieldToken)
	{
		return ResolveFieldHandle(fieldToken);
	}

	public RuntimeMethodHandle GetRuntimeMethodHandleFromMetadataToken(int methodToken)
	{
		return ResolveMethodHandle(methodToken);
	}

	public RuntimeTypeHandle GetRuntimeTypeHandleFromMetadataToken(int typeToken)
	{
		return ResolveTypeHandle(typeToken);
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		return value == ((ModuleHandle)obj).Value;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public bool Equals(ModuleHandle handle)
	{
		return value == handle.Value;
	}

	public override int GetHashCode()
	{
		return value.GetHashCode();
	}

	public static bool operator ==(ModuleHandle left, ModuleHandle right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(ModuleHandle left, ModuleHandle right)
	{
		return !object.Equals(left, right);
	}
}
