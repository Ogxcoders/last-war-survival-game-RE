using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BattleCardModule : IMessage<BattleCardModule>, IMessage, IEquatable<BattleCardModule>, IDeepCloneable<BattleCardModule>
{
	private static readonly MessageParser<BattleCardModule> _parser = new MessageParser<BattleCardModule>(() => new BattleCardModule());

	private UnknownFieldSet _unknownFields;

	public const int CardsFieldNumber = 1;

	private static readonly FieldCodec<BattleCardProto> _repeated_cards_codec = FieldCodec.ForMessage(10u, BattleCardProto.Parser);

	private readonly RepeatedField<BattleCardProto> cards_ = new RepeatedField<BattleCardProto>();

	public const int PowerFieldNumber = 2;

	private long power_;

	public const int EffectsFieldNumber = 3;

	private static readonly FieldCodec<Effect> _repeated_effects_codec = FieldCodec.ForMessage(26u, Effect.Parser);

	private readonly RepeatedField<Effect> effects_ = new RepeatedField<Effect>();

	public const int SkillEffectsFieldNumber = 4;

	private static readonly FieldCodec<Effect> _repeated_skillEffects_codec = FieldCodec.ForMessage(34u, Effect.Parser);

	private readonly RepeatedField<Effect> skillEffects_ = new RepeatedField<Effect>();

	public const int CheckResultsFieldNumber = 5;

	private static readonly FieldCodec<CardSkillCheckResult> _repeated_checkResults_codec = FieldCodec.ForMessage(42u, CardSkillCheckResult.Parser);

	private readonly RepeatedField<CardSkillCheckResult> checkResults_ = new RepeatedField<CardSkillCheckResult>();

	[DebuggerNonUserCode]
	public static MessageParser<BattleCardModule> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<BattleCardProto> Cards => cards_;

	[DebuggerNonUserCode]
	public long Power
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
	public RepeatedField<Effect> Effects => effects_;

	[DebuggerNonUserCode]
	public RepeatedField<Effect> SkillEffects => skillEffects_;

	[DebuggerNonUserCode]
	public RepeatedField<CardSkillCheckResult> CheckResults => checkResults_;

	[DebuggerNonUserCode]
	public BattleCardModule()
	{
	}

	[DebuggerNonUserCode]
	public BattleCardModule(BattleCardModule other)
		: this()
	{
		cards_ = other.cards_.Clone();
		power_ = other.power_;
		effects_ = other.effects_.Clone();
		skillEffects_ = other.skillEffects_.Clone();
		checkResults_ = other.checkResults_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleCardModule Clone()
	{
		return new BattleCardModule(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleCardModule);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleCardModule other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!cards_.Equals(other.cards_))
		{
			return false;
		}
		if (Power != other.Power)
		{
			return false;
		}
		if (!effects_.Equals(other.effects_))
		{
			return false;
		}
		if (!skillEffects_.Equals(other.skillEffects_))
		{
			return false;
		}
		if (!checkResults_.Equals(other.checkResults_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= cards_.GetHashCode();
		if (Power != 0L)
		{
			num ^= Power.GetHashCode();
		}
		num ^= effects_.GetHashCode();
		num ^= skillEffects_.GetHashCode();
		num ^= checkResults_.GetHashCode();
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
		cards_.WriteTo(output, _repeated_cards_codec);
		if (Power != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Power);
		}
		effects_.WriteTo(output, _repeated_effects_codec);
		skillEffects_.WriteTo(output, _repeated_skillEffects_codec);
		checkResults_.WriteTo(output, _repeated_checkResults_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += cards_.CalculateSize(_repeated_cards_codec);
		if (Power != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Power);
		}
		num += effects_.CalculateSize(_repeated_effects_codec);
		num += skillEffects_.CalculateSize(_repeated_skillEffects_codec);
		num += checkResults_.CalculateSize(_repeated_checkResults_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleCardModule other)
	{
		if (other != null)
		{
			cards_.Add(other.cards_);
			if (other.Power != 0L)
			{
				Power = other.Power;
			}
			effects_.Add(other.effects_);
			skillEffects_.Add(other.skillEffects_);
			checkResults_.Add(other.checkResults_);
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
			case 10u:
				cards_.AddEntriesFrom(input, _repeated_cards_codec);
				break;
			case 16u:
				Power = input.ReadInt64();
				break;
			case 26u:
				effects_.AddEntriesFrom(input, _repeated_effects_codec);
				break;
			case 34u:
				skillEffects_.AddEntriesFrom(input, _repeated_skillEffects_codec);
				break;
			case 42u:
				checkResults_.AddEntriesFrom(input, _repeated_checkResults_codec);
				break;
			}
		}
	}
}
