using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class SkinProgress : IMessage<SkinProgress>, IMessage, IEquatable<SkinProgress>, IDeepCloneable<SkinProgress>
{
	private static readonly MessageParser<SkinProgress> _parser = new MessageParser<SkinProgress>(() => new SkinProgress());

	private UnknownFieldSet _unknownFields;

	public const int PowerFieldNumber = 1;

	private int power_;

	public const int SkinEffectsFieldNumber = 2;

	private static readonly FieldCodec<Effect> _repeated_skinEffects_codec = FieldCodec.ForMessage(18u, Effect.Parser);

	private readonly RepeatedField<Effect> skinEffects_ = new RepeatedField<Effect>();

	public const int SkinProgressSingleFieldNumber = 3;

	private static readonly FieldCodec<SkinProgressSingle> _repeated_skinProgressSingle_codec = FieldCodec.ForMessage(26u, Protobuf.SkinProgressSingle.Parser);

	private readonly RepeatedField<SkinProgressSingle> skinProgressSingle_ = new RepeatedField<SkinProgressSingle>();

	[DebuggerNonUserCode]
	public static MessageParser<SkinProgress> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[27];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Power
	{
		get
		{
			return power_;
		}
		set
		{
			power_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Effect> SkinEffects => skinEffects_;

	[DebuggerNonUserCode]
	public RepeatedField<SkinProgressSingle> SkinProgressSingle => skinProgressSingle_;

	[DebuggerNonUserCode]
	public SkinProgress()
	{
	}

	[DebuggerNonUserCode]
	public SkinProgress(SkinProgress other)
		: this()
	{
		power_ = other.power_;
		skinEffects_ = other.skinEffects_.Clone();
		skinProgressSingle_ = other.skinProgressSingle_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SkinProgress Clone()
	{
		return new SkinProgress(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SkinProgress);
	}

	[DebuggerNonUserCode]
	public bool Equals(SkinProgress other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Power != other.Power)
		{
			return false;
		}
		if (!skinEffects_.Equals(other.skinEffects_))
		{
			return false;
		}
		if (!skinProgressSingle_.Equals(other.skinProgressSingle_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Power != 0)
		{
			num ^= Power.GetHashCode();
		}
		num ^= skinEffects_.GetHashCode();
		num ^= skinProgressSingle_.GetHashCode();
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
		if (Power != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Power);
		}
		skinEffects_.WriteTo(output, _repeated_skinEffects_codec);
		skinProgressSingle_.WriteTo(output, _repeated_skinProgressSingle_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (Power != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Power);
		}
		num += skinEffects_.CalculateSize(_repeated_skinEffects_codec);
		num += skinProgressSingle_.CalculateSize(_repeated_skinProgressSingle_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SkinProgress other)
	{
		if (other != null)
		{
			if (other.Power != 0)
			{
				Power = other.Power;
			}
			skinEffects_.Add(other.skinEffects_);
			skinProgressSingle_.Add(other.skinProgressSingle_);
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
				Power = input.ReadInt32();
				break;
			case 18u:
				skinEffects_.AddEntriesFrom(input, _repeated_skinEffects_codec);
				break;
			case 26u:
				skinProgressSingle_.AddEntriesFrom(input, _repeated_skinProgressSingle_codec);
				break;
			}
		}
	}
}
