using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ExtraEffectSource : IMessage<ExtraEffectSource>, IMessage, IEquatable<ExtraEffectSource>, IDeepCloneable<ExtraEffectSource>
{
	private static readonly MessageParser<ExtraEffectSource> _parser = new MessageParser<ExtraEffectSource>(() => new ExtraEffectSource());

	private UnknownFieldSet _unknownFields;

	public const int SourceTypeFieldNumber = 1;

	private int sourceType_;

	public const int EffectsFieldNumber = 2;

	private static readonly FieldCodec<Effect> _repeated_effects_codec = FieldCodec.ForMessage(18u, Effect.Parser);

	private readonly RepeatedField<Effect> effects_ = new RepeatedField<Effect>();

	[DebuggerNonUserCode]
	public static MessageParser<ExtraEffectSource> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[19];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int SourceType
	{
		get
		{
			return sourceType_;
		}
		set
		{
			sourceType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Effect> Effects => effects_;

	[DebuggerNonUserCode]
	public ExtraEffectSource()
	{
	}

	[DebuggerNonUserCode]
	public ExtraEffectSource(ExtraEffectSource other)
		: this()
	{
		sourceType_ = other.sourceType_;
		effects_ = other.effects_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ExtraEffectSource Clone()
	{
		return new ExtraEffectSource(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ExtraEffectSource);
	}

	[DebuggerNonUserCode]
	public bool Equals(ExtraEffectSource other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SourceType != other.SourceType)
		{
			return false;
		}
		if (!effects_.Equals(other.effects_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SourceType != 0)
		{
			num ^= SourceType.GetHashCode();
		}
		num ^= effects_.GetHashCode();
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
		if (SourceType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(SourceType);
		}
		effects_.WriteTo(output, _repeated_effects_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (SourceType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SourceType);
		}
		num += effects_.CalculateSize(_repeated_effects_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ExtraEffectSource other)
	{
		if (other != null)
		{
			if (other.SourceType != 0)
			{
				SourceType = other.SourceType;
			}
			effects_.Add(other.effects_);
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
				SourceType = input.ReadInt32();
				break;
			case 18u:
				effects_.AddEntriesFrom(input, _repeated_effects_codec);
				break;
			}
		}
	}
}
