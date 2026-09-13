using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class SoldierElevenProto : IMessage<SoldierElevenProto>, IMessage, IEquatable<SoldierElevenProto>, IDeepCloneable<SoldierElevenProto>
{
	private static readonly MessageParser<SoldierElevenProto> _parser = new MessageParser<SoldierElevenProto>(() => new SoldierElevenProto());

	private UnknownFieldSet _unknownFields;

	public const int ProgressIdFieldNumber = 1;

	private int progressId_;

	public const int StageFieldNumber = 2;

	private int stage_;

	public const int EffectsFieldNumber = 3;

	private static readonly FieldCodec<Effect> _repeated_effects_codec = FieldCodec.ForMessage(26u, Effect.Parser);

	private readonly RepeatedField<Effect> effects_ = new RepeatedField<Effect>();

	public const int SpecialEffectsFieldNumber = 4;

	private static readonly FieldCodec<Effect> _repeated_specialEffects_codec = FieldCodec.ForMessage(34u, Effect.Parser);

	private readonly RepeatedField<Effect> specialEffects_ = new RepeatedField<Effect>();

	[DebuggerNonUserCode]
	public static MessageParser<SoldierElevenProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[12];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int ProgressId
	{
		get
		{
			return progressId_;
		}
		set
		{
			progressId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Effect> Effects => effects_;

	[DebuggerNonUserCode]
	public RepeatedField<Effect> SpecialEffects => specialEffects_;

	[DebuggerNonUserCode]
	public SoldierElevenProto()
	{
	}

	[DebuggerNonUserCode]
	public SoldierElevenProto(SoldierElevenProto other)
		: this()
	{
		progressId_ = other.progressId_;
		stage_ = other.stage_;
		effects_ = other.effects_.Clone();
		specialEffects_ = other.specialEffects_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SoldierElevenProto Clone()
	{
		return new SoldierElevenProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SoldierElevenProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(SoldierElevenProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ProgressId != other.ProgressId)
		{
			return false;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		if (!effects_.Equals(other.effects_))
		{
			return false;
		}
		if (!specialEffects_.Equals(other.specialEffects_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ProgressId != 0)
		{
			num ^= ProgressId.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		num ^= effects_.GetHashCode();
		num ^= specialEffects_.GetHashCode();
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
		if (ProgressId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ProgressId);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Stage);
		}
		effects_.WriteTo(output, _repeated_effects_codec);
		specialEffects_.WriteTo(output, _repeated_specialEffects_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (ProgressId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ProgressId);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		num += effects_.CalculateSize(_repeated_effects_codec);
		num += specialEffects_.CalculateSize(_repeated_specialEffects_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SoldierElevenProto other)
	{
		if (other != null)
		{
			if (other.ProgressId != 0)
			{
				ProgressId = other.ProgressId;
			}
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			effects_.Add(other.effects_);
			specialEffects_.Add(other.specialEffects_);
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
				ProgressId = input.ReadInt32();
				break;
			case 16u:
				Stage = input.ReadInt32();
				break;
			case 26u:
				effects_.AddEntriesFrom(input, _repeated_effects_codec);
				break;
			case 34u:
				specialEffects_.AddEntriesFrom(input, _repeated_specialEffects_codec);
				break;
			}
		}
	}
}
