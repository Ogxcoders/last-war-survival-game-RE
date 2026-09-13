using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class UnitEffectReason : IMessage<UnitEffectReason>, IMessage, IEquatable<UnitEffectReason>, IDeepCloneable<UnitEffectReason>
{
	private static readonly MessageParser<UnitEffectReason> _parser = new MessageParser<UnitEffectReason>(() => new UnitEffectReason());

	private UnknownFieldSet _unknownFields;

	public const int EffectIdFieldNumber = 1;

	private int effectId_;

	public const int ReasonsFieldNumber = 2;

	private static readonly FieldCodec<EffectReason> _repeated_reasons_codec = FieldCodec.ForMessage(18u, EffectReason.Parser);

	private readonly RepeatedField<EffectReason> reasons_ = new RepeatedField<EffectReason>();

	[DebuggerNonUserCode]
	public static MessageParser<UnitEffectReason> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[20];

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
	public RepeatedField<EffectReason> Reasons => reasons_;

	[DebuggerNonUserCode]
	public UnitEffectReason()
	{
	}

	[DebuggerNonUserCode]
	public UnitEffectReason(UnitEffectReason other)
		: this()
	{
		effectId_ = other.effectId_;
		reasons_ = other.reasons_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UnitEffectReason Clone()
	{
		return new UnitEffectReason(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UnitEffectReason);
	}

	[DebuggerNonUserCode]
	public bool Equals(UnitEffectReason other)
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
		num += reasons_.CalculateSize(_repeated_reasons_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UnitEffectReason other)
	{
		if (other != null)
		{
			if (other.EffectId != 0)
			{
				EffectId = other.EffectId;
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
			case 18u:
				reasons_.AddEntriesFrom(input, _repeated_reasons_codec);
				break;
			}
		}
	}
}
