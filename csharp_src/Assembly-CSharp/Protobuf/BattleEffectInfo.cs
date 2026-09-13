using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BattleEffectInfo : IMessage<BattleEffectInfo>, IMessage, IEquatable<BattleEffectInfo>, IDeepCloneable<BattleEffectInfo>
{
	private static readonly MessageParser<BattleEffectInfo> _parser = new MessageParser<BattleEffectInfo>(() => new BattleEffectInfo());

	private UnknownFieldSet _unknownFields;

	public const int EffectIdFieldNumber = 1;

	private int effectId_;

	public const int ValueFieldNumber = 2;

	private float value_;

	public const int ReasonsFieldNumber = 3;

	private static readonly FieldCodec<BattleEffectReason> _repeated_reasons_codec = FieldCodec.ForMessage(26u, BattleEffectReason.Parser);

	private readonly RepeatedField<BattleEffectReason> reasons_ = new RepeatedField<BattleEffectReason>();

	[DebuggerNonUserCode]
	public static MessageParser<BattleEffectInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[35];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int EffectId
	{
		get
		{
			return effectId_;
		}
		set
		{
			effectId_ = value;
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
	public RepeatedField<BattleEffectReason> Reasons => reasons_;

	[DebuggerNonUserCode]
	public BattleEffectInfo()
	{
	}

	[DebuggerNonUserCode]
	public BattleEffectInfo(BattleEffectInfo other)
		: this()
	{
		effectId_ = other.effectId_;
		value_ = other.value_;
		reasons_ = other.reasons_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleEffectInfo Clone()
	{
		return new BattleEffectInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleEffectInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleEffectInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (EffectId != other.EffectId)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Value, other.Value))
		{
			return false;
		}
		if (!reasons_.Equals(other.reasons_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (EffectId != 0)
		{
			num ^= EffectId.GetHashCode();
		}
		if (Value != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Value);
		}
		num ^= reasons_.GetHashCode();
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
		if (EffectId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(EffectId);
		}
		if (Value != 0f)
		{
			output.WriteRawTag(21);
			output.WriteFloat(Value);
		}
		reasons_.WriteTo(output, _repeated_reasons_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (EffectId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EffectId);
		}
		if (Value != 0f)
		{
			num += 5;
		}
		num += reasons_.CalculateSize(_repeated_reasons_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleEffectInfo other)
	{
		if (other != null)
		{
			if (other.EffectId != 0)
			{
				EffectId = other.EffectId;
			}
			if (other.Value != 0f)
			{
				Value = other.Value;
			}
			reasons_.Add(other.reasons_);
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
				EffectId = input.ReadInt32();
				break;
			case 21u:
				Value = input.ReadFloat();
				break;
			case 26u:
				reasons_.AddEntriesFrom(input, _repeated_reasons_codec);
				break;
			}
		}
	}
}
