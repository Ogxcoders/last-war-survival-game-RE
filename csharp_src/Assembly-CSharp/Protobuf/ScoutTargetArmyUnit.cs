using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutTargetArmyUnit : IMessage<ScoutTargetArmyUnit>, IMessage, IEquatable<ScoutTargetArmyUnit>, IDeepCloneable<ScoutTargetArmyUnit>
{
	private static readonly MessageParser<ScoutTargetArmyUnit> _parser = new MessageParser<ScoutTargetArmyUnit>(() => new ScoutTargetArmyUnit());

	private UnknownFieldSet _unknownFields;

	public const int FormationFieldNumber = 1;

	private static readonly FieldCodec<ScoutFormation> _repeated_formation_codec = FieldCodec.ForMessage(10u, ScoutFormation.Parser);

	private readonly RepeatedField<ScoutFormation> formation_ = new RepeatedField<ScoutFormation>();

	public const int VisibleFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_visible_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? visible_;

	public const int AllSoldierFieldNumber = 3;

	private static readonly FieldCodec<ScoutSoldier> _repeated_allSoldier_codec = FieldCodec.ForMessage(26u, ScoutSoldier.Parser);

	private readonly RepeatedField<ScoutSoldier> allSoldier_ = new RepeatedField<ScoutSoldier>();

	public const int SoldierPowerFieldNumber = 4;

	private int soldierPower_;

	public const int FreeSoldierFieldNumber = 5;

	private static readonly FieldCodec<ScoutSoldier> _repeated_freeSoldier_codec = FieldCodec.ForMessage(42u, ScoutSoldier.Parser);

	private readonly RepeatedField<ScoutSoldier> freeSoldier_ = new RepeatedField<ScoutSoldier>();

	public const int HospitalSoldierFieldNumber = 6;

	private static readonly FieldCodec<ScoutSoldier> _repeated_hospitalSoldier_codec = FieldCodec.ForMessage(50u, ScoutSoldier.Parser);

	private readonly RepeatedField<ScoutSoldier> hospitalSoldier_ = new RepeatedField<ScoutSoldier>();

	public const int EffectsFieldNumber = 7;

	private static readonly FieldCodec<BattleEffectInfo> _repeated_effects_codec = FieldCodec.ForMessage(58u, BattleEffectInfo.Parser);

	private readonly RepeatedField<BattleEffectInfo> effects_ = new RepeatedField<BattleEffectInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<ScoutTargetArmyUnit> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[12];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<ScoutFormation> Formation => formation_;

	[DebuggerNonUserCode]
	public int? Visible
	{
		get
		{
			return visible_;
		}
		set
		{
			visible_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<ScoutSoldier> AllSoldier => allSoldier_;

	[DebuggerNonUserCode]
	public int SoldierPower
	{
		get
		{
			return soldierPower_;
		}
		set
		{
			soldierPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<ScoutSoldier> FreeSoldier => freeSoldier_;

	[DebuggerNonUserCode]
	public RepeatedField<ScoutSoldier> HospitalSoldier => hospitalSoldier_;

	[DebuggerNonUserCode]
	public RepeatedField<BattleEffectInfo> Effects => effects_;

	[DebuggerNonUserCode]
	public ScoutTargetArmyUnit()
	{
	}

	[DebuggerNonUserCode]
	public ScoutTargetArmyUnit(ScoutTargetArmyUnit other)
		: this()
	{
		formation_ = other.formation_.Clone();
		Visible = other.Visible;
		allSoldier_ = other.allSoldier_.Clone();
		soldierPower_ = other.soldierPower_;
		freeSoldier_ = other.freeSoldier_.Clone();
		hospitalSoldier_ = other.hospitalSoldier_.Clone();
		effects_ = other.effects_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutTargetArmyUnit Clone()
	{
		return new ScoutTargetArmyUnit(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutTargetArmyUnit);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutTargetArmyUnit other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!formation_.Equals(other.formation_))
		{
			return false;
		}
		if (Visible != other.Visible)
		{
			return false;
		}
		if (!allSoldier_.Equals(other.allSoldier_))
		{
			return false;
		}
		if (SoldierPower != other.SoldierPower)
		{
			return false;
		}
		if (!freeSoldier_.Equals(other.freeSoldier_))
		{
			return false;
		}
		if (!hospitalSoldier_.Equals(other.hospitalSoldier_))
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
		num ^= formation_.GetHashCode();
		if (visible_.HasValue)
		{
			num ^= Visible.GetHashCode();
		}
		num ^= allSoldier_.GetHashCode();
		if (SoldierPower != 0)
		{
			num ^= SoldierPower.GetHashCode();
		}
		num ^= freeSoldier_.GetHashCode();
		num ^= hospitalSoldier_.GetHashCode();
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
		formation_.WriteTo(output, _repeated_formation_codec);
		if (visible_.HasValue)
		{
			_single_visible_codec.WriteTagAndValue(output, Visible);
		}
		allSoldier_.WriteTo(output, _repeated_allSoldier_codec);
		if (SoldierPower != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(SoldierPower);
		}
		freeSoldier_.WriteTo(output, _repeated_freeSoldier_codec);
		hospitalSoldier_.WriteTo(output, _repeated_hospitalSoldier_codec);
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
		num += formation_.CalculateSize(_repeated_formation_codec);
		if (visible_.HasValue)
		{
			num += _single_visible_codec.CalculateSizeWithTag(Visible);
		}
		num += allSoldier_.CalculateSize(_repeated_allSoldier_codec);
		if (SoldierPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SoldierPower);
		}
		num += freeSoldier_.CalculateSize(_repeated_freeSoldier_codec);
		num += hospitalSoldier_.CalculateSize(_repeated_hospitalSoldier_codec);
		num += effects_.CalculateSize(_repeated_effects_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutTargetArmyUnit other)
	{
		if (other != null)
		{
			formation_.Add(other.formation_);
			if (other.visible_.HasValue && (!visible_.HasValue || other.Visible != 0))
			{
				Visible = other.Visible;
			}
			allSoldier_.Add(other.allSoldier_);
			if (other.SoldierPower != 0)
			{
				SoldierPower = other.SoldierPower;
			}
			freeSoldier_.Add(other.freeSoldier_);
			hospitalSoldier_.Add(other.hospitalSoldier_);
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
			case 10u:
				formation_.AddEntriesFrom(input, _repeated_formation_codec);
				break;
			case 18u:
			{
				int? num2 = _single_visible_codec.Read(input);
				if (!visible_.HasValue || num2 != 0)
				{
					Visible = num2;
				}
				break;
			}
			case 26u:
				allSoldier_.AddEntriesFrom(input, _repeated_allSoldier_codec);
				break;
			case 32u:
				SoldierPower = input.ReadInt32();
				break;
			case 42u:
				freeSoldier_.AddEntriesFrom(input, _repeated_freeSoldier_codec);
				break;
			case 50u:
				hospitalSoldier_.AddEntriesFrom(input, _repeated_hospitalSoldier_codec);
				break;
			case 58u:
				effects_.AddEntriesFrom(input, _repeated_effects_codec);
				break;
			}
		}
	}
}
