using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ArmyAttrInfo : IMessage<ArmyAttrInfo>, IMessage, IEquatable<ArmyAttrInfo>, IDeepCloneable<ArmyAttrInfo>
{
	private static readonly MessageParser<ArmyAttrInfo> _parser = new MessageParser<ArmyAttrInfo>(() => new ArmyAttrInfo());

	private UnknownFieldSet _unknownFields;

	public const int ReasonFieldNumber = 1;

	private int reason_;

	public const int ValueFieldNumber = 2;

	private float value_;

	[DebuggerNonUserCode]
	public static MessageParser<ArmyAttrInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[24];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Reason
	{
		get
		{
			return reason_;
		}
		set
		{
			reason_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float Value
	{
		get
		{
			return value_;
		}
		set
		{
			value_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyAttrInfo()
	{
	}

	[DebuggerNonUserCode]
	public ArmyAttrInfo(ArmyAttrInfo other)
		: this()
	{
		reason_ = other.reason_;
		value_ = other.value_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ArmyAttrInfo Clone()
	{
		return new ArmyAttrInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ArmyAttrInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ArmyAttrInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Reason != other.Reason)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Value, other.Value))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Reason != 0)
		{
			num ^= Reason.GetHashCode();
		}
		if (Value != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Value);
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
		if (Reason != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Reason);
		}
		if (Value != 0f)
		{
			output.WriteRawTag(21);
			output.WriteFloat(Value);
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
		if (Reason != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Reason);
		}
		if (Value != 0f)
		{
			num += 5;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ArmyAttrInfo other)
	{
		if (other != null)
		{
			if (other.Reason != 0)
			{
				Reason = other.Reason;
			}
			if (other.Value != 0f)
			{
				Value = other.Value;
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
				Reason = input.ReadInt32();
				break;
			case 21u:
				Value = input.ReadFloat();
				break;
			}
		}
	}
}
