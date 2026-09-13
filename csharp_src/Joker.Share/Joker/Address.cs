using System.Runtime.InteropServices;

namespace Joker;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Address
{
	public int Process;

	public int Coroutine;

	public bool Equals(Address other)
	{
		if (Process == other.Process)
		{
			return Coroutine == other.Coroutine;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is Address other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return UtilHashCode.GetHashCode(Process, Coroutine);
	}

	public Address(int process, int coroutine)
	{
		Process = process;
		Coroutine = coroutine;
	}

	public static bool operator ==(Address left, Address right)
	{
		if (left.Process == right.Process)
		{
			return left.Coroutine == right.Coroutine;
		}
		return false;
	}

	public static bool operator !=(Address left, Address right)
	{
		return !(left == right);
	}

	public override string ToString()
	{
		return $"{Process}:{Coroutine}";
	}
}
