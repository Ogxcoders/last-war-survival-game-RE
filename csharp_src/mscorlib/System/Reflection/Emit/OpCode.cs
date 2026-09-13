using System.Runtime.InteropServices;

namespace System.Reflection.Emit;

[ComVisible(true)]
public readonly struct OpCode : IEquatable<OpCode>
{
	internal readonly byte op1;

	internal readonly byte op2;

	private readonly byte push;

	private readonly byte pop;

	private readonly byte size;

	private readonly byte type;

	private readonly byte args;

	private readonly byte flow;

	public string Name
	{
		get
		{
			if (op1 == byte.MaxValue)
			{
				return OpCodeNames.names[op2];
			}
			return OpCodeNames.names[256 + op2];
		}
	}

	public int Size => size;

	public OpCodeType OpCodeType => (OpCodeType)type;

	public OperandType OperandType => (OperandType)args;

	public FlowControl FlowControl => (FlowControl)flow;

	public StackBehaviour StackBehaviourPop => (StackBehaviour)pop;

	public StackBehaviour StackBehaviourPush => (StackBehaviour)push;

	public short Value
	{
		get
		{
			if (size == 1)
			{
				return op2;
			}
			return (short)((op1 << 8) | op2);
		}
	}

	internal OpCode(int p, int q)
	{
		op1 = (byte)(p & 0xFF);
		op2 = (byte)((p >> 8) & 0xFF);
		push = (byte)((p >> 16) & 0xFF);
		pop = (byte)((p >> 24) & 0xFF);
		size = (byte)(q & 0xFF);
		type = (byte)((q >> 8) & 0xFF);
		args = (byte)((q >> 16) & 0xFF);
		flow = (byte)((q >> 24) & 0xFF);
	}

	public override int GetHashCode()
	{
		return Name.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj == null || !(obj is OpCode opCode))
		{
			return false;
		}
		if (opCode.op1 == op1)
		{
			return opCode.op2 == op2;
		}
		return false;
	}

	public bool Equals(OpCode obj)
	{
		if (obj.op1 == op1)
		{
			return obj.op2 == op2;
		}
		return false;
	}

	public override string ToString()
	{
		return Name;
	}

	public static bool operator ==(OpCode a, OpCode b)
	{
		if (a.op1 == b.op1)
		{
			return a.op2 == b.op2;
		}
		return false;
	}

	public static bool operator !=(OpCode a, OpCode b)
	{
		if (a.op1 == b.op1)
		{
			return a.op2 != b.op2;
		}
		return true;
	}
}
