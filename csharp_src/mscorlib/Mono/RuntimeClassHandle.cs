using System;
using System.Runtime.CompilerServices;

namespace Mono;

internal struct RuntimeClassHandle
{
	private unsafe RuntimeStructs.MonoClass* value;

	internal unsafe RuntimeStructs.MonoClass* Value => value;

	internal unsafe RuntimeClassHandle(RuntimeStructs.MonoClass* value)
	{
		this.value = value;
	}

	internal unsafe RuntimeClassHandle(IntPtr ptr)
	{
		value = (RuntimeStructs.MonoClass*)(void*)ptr;
	}

	public unsafe override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		return value == ((RuntimeClassHandle)obj).Value;
	}

	public unsafe override int GetHashCode()
	{
		return ((IntPtr)value).GetHashCode();
	}

	public unsafe bool Equals(RuntimeClassHandle handle)
	{
		return value == handle.Value;
	}

	public static bool operator ==(RuntimeClassHandle left, object right)
	{
		if (right != null && right is RuntimeClassHandle handle)
		{
			return left.Equals(handle);
		}
		return false;
	}

	public static bool operator !=(RuntimeClassHandle left, object right)
	{
		return !(left == right);
	}

	public static bool operator ==(object left, RuntimeClassHandle right)
	{
		if (left != null && left is RuntimeClassHandle runtimeClassHandle)
		{
			return runtimeClassHandle.Equals(right);
		}
		return false;
	}

	public static bool operator !=(object left, RuntimeClassHandle right)
	{
		return !(left == right);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal unsafe static extern IntPtr GetTypeFromClass(RuntimeStructs.MonoClass* klass);

	internal unsafe RuntimeTypeHandle GetTypeHandle()
	{
		return new RuntimeTypeHandle(GetTypeFromClass(value));
	}
}
