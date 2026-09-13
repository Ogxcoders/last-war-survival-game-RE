using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutFormation : IMessage<ScoutFormation>, IMessage, IEquatable<ScoutFormation>, IDeepCloneable<ScoutFormation>
{
	private static readonly MessageParser<ScoutFormation> _parser = new MessageParser<ScoutFormation>(() => new ScoutFormation());

	private UnknownFieldSet _unknownFields;

	public const int SoldierTotalFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_soldierTotal_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? soldierTotal_;

	public const int HeroFieldNumber = 2;

	private static readonly FieldCodec<ScoutHero> _repeated_hero_codec = FieldCodec.ForMessage(18u, ScoutHero.Parser);

	private readonly RepeatedField<ScoutHero> hero_ = new RepeatedField<ScoutHero>();

	public const int SoldierFieldNumber = 3;

	private static readonly FieldCodec<ScoutSoldier> _repeated_soldier_codec = FieldCodec.ForMessage(26u, ScoutSoldier.Parser);

	private readonly RepeatedField<ScoutSoldier> soldier_ = new RepeatedField<ScoutSoldier>();

	public const int SpecialUnitTypeFieldNumber = 4;

	private static readonly FieldCodec<int?> _single_specialUnitType_codec = FieldCodec.ForStructWrapper<int>(34u);

	private int? specialUnitType_;

	public const int IndexFieldNumber = 5;

	private int index_;

	public const int UseChipGroupFieldNumber = 6;

	private int useChipGroup_;

	public const int PowerFieldNumber = 7;

	private long power_;

	public const int SoldierElevenFieldNumber = 8;

	private ScoutSoldierEleven soldierEleven_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutFormation> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[16];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? SoldierTotal
	{
		get
		{
			return soldierTotal_;
		}
		set
		{
			soldierTotal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<ScoutHero> Hero => hero_;

	[DebuggerNonUserCode]
	public RepeatedField<ScoutSoldier> Soldier => soldier_;

	[DebuggerNonUserCode]
	public int? SpecialUnitType
	{
		get
		{
			return specialUnitType_;
		}
		set
		{
			specialUnitType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Index
	{
		get
		{
			return index_;
		}
		set
		{
			index_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int UseChipGroup
	{
		get
		{
			return useChipGroup_;
		}
		set
		{
			useChipGroup_ = value;
		}
	}

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
	public ScoutSoldierEleven SoldierEleven
	{
		get
		{
			return soldierEleven_;
		}
		set
		{
			soldierEleven_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutFormation()
	{
	}

	[DebuggerNonUserCode]
	public ScoutFormation(ScoutFormation other)
		: this()
	{
		SoldierTotal = other.SoldierTotal;
		hero_ = other.hero_.Clone();
		soldier_ = other.soldier_.Clone();
		SpecialUnitType = other.SpecialUnitType;
		index_ = other.index_;
		useChipGroup_ = other.useChipGroup_;
		power_ = other.power_;
		soldierEleven_ = ((other.soldierEleven_ != null) ? other.soldierEleven_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutFormation Clone()
	{
		return new ScoutFormation(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutFormation);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutFormation other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SoldierTotal != other.SoldierTotal)
		{
			return false;
		}
		if (!hero_.Equals(other.hero_))
		{
			return false;
		}
		if (!soldier_.Equals(other.soldier_))
		{
			return false;
		}
		if (SpecialUnitType != other.SpecialUnitType)
		{
			return false;
		}
		if (Index != other.Index)
		{
			return false;
		}
		if (UseChipGroup != other.UseChipGroup)
		{
			return false;
		}
		if (Power != other.Power)
		{
			return false;
		}
		if (!object.Equals(SoldierEleven, other.SoldierEleven))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (soldierTotal_.HasValue)
		{
			num ^= SoldierTotal.GetHashCode();
		}
		num ^= hero_.GetHashCode();
		num ^= soldier_.GetHashCode();
		if (specialUnitType_.HasValue)
		{
			num ^= SpecialUnitType.GetHashCode();
		}
		if (Index != 0)
		{
			num ^= Index.GetHashCode();
		}
		if (UseChipGroup != 0)
		{
			num ^= UseChipGroup.GetHashCode();
		}
		if (Power != 0L)
		{
			num ^= Power.GetHashCode();
		}
		if (soldierEleven_ != null)
		{
			num ^= SoldierEleven.GetHashCode();
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
		if (soldierTotal_.HasValue)
		{
			_single_soldierTotal_codec.WriteTagAndValue(output, SoldierTotal);
		}
		hero_.WriteTo(output, _repeated_hero_codec);
		soldier_.WriteTo(output, _repeated_soldier_codec);
		if (specialUnitType_.HasValue)
		{
			_single_specialUnitType_codec.WriteTagAndValue(output, SpecialUnitType);
		}
		if (Index != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Index);
		}
		if (UseChipGroup != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(UseChipGroup);
		}
		if (Power != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(Power);
		}
		if (soldierEleven_ != null)
		{
			output.WriteRawTag(66);
			output.WriteMessage(SoldierEleven);
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
		if (soldierTotal_.HasValue)
		{
			num += _single_soldierTotal_codec.CalculateSizeWithTag(SoldierTotal);
		}
		num += hero_.CalculateSize(_repeated_hero_codec);
		num += soldier_.CalculateSize(_repeated_soldier_codec);
		if (specialUnitType_.HasValue)
		{
			num += _single_specialUnitType_codec.CalculateSizeWithTag(SpecialUnitType);
		}
		if (Index != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Index);
		}
		if (UseChipGroup != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(UseChipGroup);
		}
		if (Power != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Power);
		}
		if (soldierEleven_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SoldierEleven);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutFormation other)
	{
		if (other == null)
		{
			return;
		}
		if (other.soldierTotal_.HasValue && (!soldierTotal_.HasValue || other.SoldierTotal != 0))
		{
			SoldierTotal = other.SoldierTotal;
		}
		hero_.Add(other.hero_);
		soldier_.Add(other.soldier_);
		if (other.specialUnitType_.HasValue && (!specialUnitType_.HasValue || other.SpecialUnitType != 0))
		{
			SpecialUnitType = other.SpecialUnitType;
		}
		if (other.Index != 0)
		{
			Index = other.Index;
		}
		if (other.UseChipGroup != 0)
		{
			UseChipGroup = other.UseChipGroup;
		}
		if (other.Power != 0L)
		{
			Power = other.Power;
		}
		if (other.soldierEleven_ != null)
		{
			if (soldierEleven_ == null)
			{
				SoldierEleven = new ScoutSoldierEleven();
			}
			SoldierEleven.MergeFrom(other.SoldierEleven);
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
			{
				int? num3 = _single_soldierTotal_codec.Read(input);
				if (!soldierTotal_.HasValue || num3 != 0)
				{
					SoldierTotal = num3;
				}
				break;
			}
			case 18u:
				hero_.AddEntriesFrom(input, _repeated_hero_codec);
				break;
			case 26u:
				soldier_.AddEntriesFrom(input, _repeated_soldier_codec);
				break;
			case 34u:
			{
				int? num2 = _single_specialUnitType_codec.Read(input);
				if (!specialUnitType_.HasValue || num2 != 0)
				{
					SpecialUnitType = num2;
				}
				break;
			}
			case 40u:
				Index = input.ReadInt32();
				break;
			case 48u:
				UseChipGroup = input.ReadInt32();
				break;
			case 56u:
				Power = input.ReadInt64();
				break;
			case 66u:
				if (soldierEleven_ == null)
				{
					SoldierEleven = new ScoutSoldierEleven();
				}
				input.ReadMessage(SoldierEleven);
				break;
			}
		}
	}
}
