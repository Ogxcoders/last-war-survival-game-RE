using System;

namespace Mono;

internal struct RuntimeEventHandle
{
	private IntPtr value;

	public IntPtr Value => value;

	internal RuntimeEventHandle(IntPtr v)
	{
		value = v;
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		return value == ((RuntimeEventHandle)obj).Value;
	}

	public bool Equals(RuntimeEventHandle handle)
	{
		return value == handle.Value;
	}

	public override int GetHashCode()
	{
		return value.GetHashCode();
	}

	public static bool operator ==(RuntimeEventHandle left, RuntimeEventHandle right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(RuntimeEventHandle left, RuntimeEventHandle right)
	{
		return !left.Equals(right);
	}
}
