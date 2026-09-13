using System.Runtime.InteropServices;

namespace Joker;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ActorId
{
	public Address Address;

	public long InstanceId;

	public int Process
	{
		get
		{
			return Address.Process;
		}
		set
		{
			Address.Process = value;
		}
	}

	public int Coroutine
	{
		get
		{
			return Address.Coroutine;
		}
		set
		{
			Address.Coroutine = value;
		}
	}

	public bool Equals(ActorId other)
	{
		if (Address == other.Address)
		{
			return InstanceId == other.InstanceId;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is ActorId other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return UtilHashCode.GetHashCode(Address, InstanceId);
	}

	public ActorId(int process, int fiber)
	{
		Address = new Address(process, fiber);
		InstanceId = 1L;
	}

	public ActorId(int process, int fiber, long instanceId)
	{
		Address = new Address(process, fiber);
		InstanceId = instanceId;
	}

	public ActorId(Address address)
		: this(address, 1L)
	{
	}

	public ActorId(Address address, long instanceId)
	{
		Address = address;
		InstanceId = instanceId;
	}

	public static bool operator ==(ActorId left, ActorId right)
	{
		if (left.InstanceId == right.InstanceId)
		{
			return left.Address == right.Address;
		}
		return false;
	}

	public static bool operator !=(ActorId left, ActorId right)
	{
		return !(left == right);
	}

	public override string ToString()
	{
		return $"{Process}:{Coroutine}:{InstanceId}";
	}
}
