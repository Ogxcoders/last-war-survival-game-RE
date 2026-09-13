using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwSoldierCount : IMessage<LwSoldierCount>, IMessage, IEquatable<LwSoldierCount>, IDeepCloneable<LwSoldierCount>
{
	private static readonly MessageParser<LwSoldierCount> _parser = new MessageParser<LwSoldierCount>(() => new LwSoldierCount());

	private UnknownFieldSet _unknownFields;

	public const int SoldierIdFieldNumber = 1;

	private int soldierId_;

	public const int CountFieldNumber = 2;

	private int count_;

	[DebuggerNonUserCode]
	public static MessageParser<LwSoldierCount> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int SoldierId
	{
		get
		{
			return soldierId_;
		}
		set
		{
			soldierId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Count
	{
		get
		{
			return count_;
		}
		set
		{
			count_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LwSoldierCount()
	{
	}

	[DebuggerNonUserCode]
	public LwSoldierCount(LwSoldierCount other)
		: this()
	{
		soldierId_ = other.soldierId_;
		count_ = other.count_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwSoldierCount Clone()
	{
		return new LwSoldierCount(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwSoldierCount);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwSoldierCount other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SoldierId != other.SoldierId)
		{
			return false;
		}
		if (Count != other.Count)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SoldierId != 0)
		{
			num ^= SoldierId.GetHashCode();
		}
		if (Count != 0)
		{
			num ^= Count.GetHashCode();
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
		if (SoldierId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(SoldierId);
		}
		if (Count != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Count);
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
		if (SoldierId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SoldierId);
		}
		if (Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Count);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwSoldierCount other)
	{
		if (other != null)
		{
			if (other.SoldierId != 0)
			{
				SoldierId = other.SoldierId;
			}
			if (other.Count != 0)
			{
				Count = other.Count;
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
			switch (num)
			{
			default:
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				break;
			case 8u:
				SoldierId = input.ReadInt32();
				break;
			case 16u:
				Count = input.ReadInt32();
				break;
			}
		}
	}
}
