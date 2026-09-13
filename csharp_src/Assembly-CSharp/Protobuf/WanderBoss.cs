using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WanderBoss : IMessage<WanderBoss>, IMessage, IEquatable<WanderBoss>, IDeepCloneable<WanderBoss>
{
	private static readonly MessageParser<WanderBoss> _parser = new MessageParser<WanderBoss>(() => new WanderBoss());

	private UnknownFieldSet _unknownFields;

	public const int RallyNumFieldNumber = 1;

	private int rallyNum_;

	[DebuggerNonUserCode]
	public static MessageParser<WanderBoss> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[40];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int RallyNum
	{
		get
		{
			return rallyNum_;
		}
		set
		{
			rallyNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WanderBoss()
	{
	}

	[DebuggerNonUserCode]
	public WanderBoss(WanderBoss other)
		: this()
	{
		rallyNum_ = other.rallyNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WanderBoss Clone()
	{
		return new WanderBoss(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WanderBoss);
	}

	[DebuggerNonUserCode]
	public bool Equals(WanderBoss other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (RallyNum != other.RallyNum)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (RallyNum != 0)
		{
			num ^= RallyNum.GetHashCode();
		}
		if (_unknownFields != null)
		{
			num ^= _unknownFields.GetHashCode();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public override string ToString()
	{
		return JsonFormatter.ToDiagnosticString(this);
	}

	[DebuggerNonUserCode]
	public void WriteTo(CodedOutputStream output)
	{
		if (RallyNum != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(RallyNum);
		}
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (RallyNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RallyNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WanderBoss other)
	{
		if (other != null)
		{
			if (other.RallyNum != 0)
			{
				RallyNum = other.RallyNum;
			}
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				RallyNum = input.ReadInt32();
			}
		}
	}
}
