using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ArmyUnitBuff : IMessage<ArmyUnitBuff>, IMessage, IEquatable<ArmyUnitBuff>, IDeepCloneable<ArmyUnitBuff>
{
	private static readonly MessageParser<ArmyUnitBuff> _parser = new MessageParser<ArmyUnitBuff>(() => new ArmyUnitBuff());

	private UnknownFieldSet _unknownFields;

	public const int BuffIdFieldNumber = 1;

	private int buffId_;

	public const int EffectIdFieldNumber = 2;

	private static readonly FieldCodec<int> _repeated_effectId_codec = FieldCodec.ForInt32(18u);

	private readonly RepeatedField<int> effectId_ = new RepeatedField<int>();

	public const int ValueFieldNumber = 3;

	private static readonly FieldCodec<float> _repeated_value_codec = FieldCodec.ForFloat(26u);

	private readonly RepeatedField<float> value_ = new RepeatedField<float>();

	public const int ExpireTimeFieldNumber = 4;

	private int expireTime_;

	[DebuggerNonUserCode]
	public static MessageParser<ArmyUnitBuff> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[21];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int BuffId
	{
		get
		{
			return buffId_;
		}
		set
		{
			buffId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<int> EffectId => effectId_;

	[DebuggerNonUserCode]
	public RepeatedField<float> Value => value_;

	[DebuggerNonUserCode]
	public int ExpireTime
	{
		get
		{
			return expireTime_;
		}
		set
		{
			expireTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyUnitBuff()
	{
	}

	[DebuggerNonUserCode]
	public ArmyUnitBuff(ArmyUnitBuff other)
		: this()
	{
		buffId_ = other.buffId_;
		effectId_ = other.effectId_.Clone();
		value_ = other.value_.Clone();
		expireTime_ = other.expireTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ArmyUnitBuff Clone()
	{
		return new ArmyUnitBuff(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ArmyUnitBuff);
	}

	[DebuggerNonUserCode]
	public bool Equals(ArmyUnitBuff other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BuffId != other.BuffId)
		{
			return false;
		}
		if (!effectId_.Equals(other.effectId_))
		{
			return false;
		}
		if (!value_.Equals(other.value_))
		{
			return false;
		}
		if (ExpireTime != other.ExpireTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (BuffId != 0)
		{
			num ^= BuffId.GetHashCode();
		}
		num ^= effectId_.GetHashCode();
		num ^= value_.GetHashCode();
		if (ExpireTime != 0)
		{
			num ^= ExpireTime.GetHashCode();
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
		if (BuffId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(BuffId);
		}
		effectId_.WriteTo(output, _repeated_effectId_codec);
		value_.WriteTo(output, _repeated_value_codec);
		if (ExpireTime != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(ExpireTime);
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
		if (BuffId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuffId);
		}
		num += effectId_.CalculateSize(_repeated_effectId_codec);
		num += value_.CalculateSize(_repeated_value_codec);
		if (ExpireTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ExpireTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ArmyUnitBuff other)
	{
		if (other != null)
		{
			if (other.BuffId != 0)
			{
				BuffId = other.BuffId;
			}
			effectId_.Add(other.effectId_);
			value_.Add(other.value_);
			if (other.ExpireTime != 0)
			{
				ExpireTime = other.ExpireTime;
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
				BuffId = input.ReadInt32();
				break;
			case 16u:
			case 18u:
				effectId_.AddEntriesFrom(input, _repeated_effectId_codec);
				break;
			case 26u:
			case 29u:
				value_.AddEntriesFrom(input, _repeated_value_codec);
				break;
			case 32u:
				ExpireTime = input.ReadInt32();
				break;
			}
		}
	}
}
