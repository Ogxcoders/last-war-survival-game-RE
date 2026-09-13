using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BattleEffectReason : IMessage<BattleEffectReason>, IMessage, IEquatable<BattleEffectReason>, IDeepCloneable<BattleEffectReason>
{
	private static readonly MessageParser<BattleEffectReason> _parser = new MessageParser<BattleEffectReason>(() => new BattleEffectReason());

	private UnknownFieldSet _unknownFields;

	public const int ValueFieldNumber = 1;

	private float value_;

	public const int ReasonFieldNumber = 2;

	private int reason_;

	[DebuggerNonUserCode]
	public static MessageParser<BattleEffectReason> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[34];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public BattleEffectReason()
	{
	}

	[DebuggerNonUserCode]
	public BattleEffectReason(BattleEffectReason other)
		: this()
	{
		value_ = other.value_;
		reason_ = other.reason_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleEffectReason Clone()
	{
		return new BattleEffectReason(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleEffectReason);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleEffectReason other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Value, other.Value))
		{
			return false;
		}
		if (Reason != other.Reason)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Value != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Value);
		}
		if (Reason != 0)
		{
			num ^= Reason.GetHashCode();
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
		if (Value != 0f)
		{
			output.WriteRawTag(13);
			output.WriteFloat(Value);
		}
		if (Reason != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Reason);
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
		if (Value != 0f)
		{
			num += 5;
		}
		if (Reason != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Reason);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleEffectReason other)
	{
		if (other != null)
		{
			if (other.Value != 0f)
			{
				Value = other.Value;
			}
			if (other.Reason != 0)
			{
				Reason = other.Reason;
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
			case 13u:
				Value = input.ReadFloat();
				break;
			case 16u:
				Reason = input.ReadInt32();
				break;
			}
		}
	}
}
