using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutSoldierEleven : IMessage<ScoutSoldierEleven>, IMessage, IEquatable<ScoutSoldierEleven>, IDeepCloneable<ScoutSoldierEleven>
{
	private static readonly MessageParser<ScoutSoldierEleven> _parser = new MessageParser<ScoutSoldierEleven>(() => new ScoutSoldierEleven());

	private UnknownFieldSet _unknownFields;

	public const int StageFieldNumber = 1;

	private int stage_;

	public const int EffectsFieldNumber = 2;

	private static readonly FieldCodec<Effect> _repeated_effects_codec = FieldCodec.ForMessage(18u, Effect.Parser);

	private readonly RepeatedField<Effect> effects_ = new RepeatedField<Effect>();

	[DebuggerNonUserCode]
	public static MessageParser<ScoutSoldierEleven> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[19];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public ScoutSoldierEleven()
	{
	}

	[DebuggerNonUserCode]
	public ScoutSoldierEleven(ScoutSoldierEleven other)
		: this()
	{
		stage_ = other.stage_;
		effects_ = other.effects_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutSoldierEleven Clone()
	{
		return new ScoutSoldierEleven(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutSoldierEleven);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutSoldierEleven other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Stage != other.Stage)
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
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
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
		if (Stage != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Stage);
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
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		num += effects_.CalculateSize(_repeated_effects_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutSoldierEleven other)
	{
		if (other != null)
		{
			if (other.Stage != 0)
			{
				Stage = other.Stage;
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
				Stage = input.ReadInt32();
				break;
			case 18u:
				effects_.AddEntriesFrom(input, _repeated_effects_codec);
				break;
			}
		}
	}
}
