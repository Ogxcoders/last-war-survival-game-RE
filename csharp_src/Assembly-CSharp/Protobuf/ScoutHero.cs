using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutHero : IMessage<ScoutHero>, IMessage, IEquatable<ScoutHero>, IDeepCloneable<ScoutHero>
{
	private static readonly MessageParser<ScoutHero> _parser = new MessageParser<ScoutHero>(() => new ScoutHero());

	private UnknownFieldSet _unknownFields;

	public const int HeroIdFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_heroId_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? heroId_;

	public const int HeroLevelFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_heroLevel_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? heroLevel_;

	public const int HeroQualityFieldNumber = 3;

	private static readonly FieldCodec<int?> _single_heroQuality_codec = FieldCodec.ForStructWrapper<int>(26u);

	private int? heroQuality_;

	public const int HeroRankLevelFieldNumber = 4;

	private static readonly FieldCodec<int?> _single_heroRankLevel_codec = FieldCodec.ForStructWrapper<int>(34u);

	private int? heroRankLevel_;

	public const int HeroIndexFieldNumber = 5;

	private static readonly FieldCodec<int?> _single_heroIndex_codec = FieldCodec.ForStructWrapper<int>(42u);

	private int? heroIndex_;

	public const int MaxHpFieldNumber = 6;

	private static readonly FieldCodec<int?> _single_maxHp_codec = FieldCodec.ForStructWrapper<int>(50u);

	private int? maxHp_;

	public const int CurHpFieldNumber = 7;

	private static readonly FieldCodec<int?> _single_curHp_codec = FieldCodec.ForStructWrapper<int>(58u);

	private int? curHp_;

	public const int WeaponLevelFieldNumber = 8;

	private static readonly FieldCodec<int?> _single_weaponLevel_codec = FieldCodec.ForStructWrapper<int>(66u);

	private int? weaponLevel_;

	public const int EquipInfosFieldNumber = 9;

	private static readonly FieldCodec<HeroEquipInfoProto> _repeated_equipInfos_codec = FieldCodec.ForMessage(74u, HeroEquipInfoProto.Parser);

	private readonly RepeatedField<HeroEquipInfoProto> equipInfos_ = new RepeatedField<HeroEquipInfoProto>();

	public const int SkillInfosFieldNumber = 10;

	private static readonly FieldCodec<HeroSkillInfoProto> _repeated_skillInfos_codec = FieldCodec.ForMessage(82u, HeroSkillInfoProto.Parser);

	private readonly RepeatedField<HeroSkillInfoProto> skillInfos_ = new RepeatedField<HeroSkillInfoProto>();

	public const int EffectsFieldNumber = 11;

	private static readonly FieldCodec<BattleEffectInfo> _repeated_effects_codec = FieldCodec.ForMessage(90u, BattleEffectInfo.Parser);

	private readonly RepeatedField<BattleEffectInfo> effects_ = new RepeatedField<BattleEffectInfo>();

	public const int DominatorFieldNumber = 12;

	private ScoutDominator dominator_;

	public const int WeaponStrengthenFieldNumber = 13;

	private static readonly FieldCodec<WeaponStrengthen> _repeated_weaponStrengthen_codec = FieldCodec.ForMessage(106u, Protobuf.WeaponStrengthen.Parser);

	private readonly RepeatedField<WeaponStrengthen> weaponStrengthen_ = new RepeatedField<WeaponStrengthen>();

	[DebuggerNonUserCode]
	public static MessageParser<ScoutHero> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[17];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? HeroId
	{
		get
		{
			return heroId_;
		}
		set
		{
			heroId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? HeroLevel
	{
		get
		{
			return heroLevel_;
		}
		set
		{
			heroLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? HeroQuality
	{
		get
		{
			return heroQuality_;
		}
		set
		{
			heroQuality_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? HeroRankLevel
	{
		get
		{
			return heroRankLevel_;
		}
		set
		{
			heroRankLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? HeroIndex
	{
		get
		{
			return heroIndex_;
		}
		set
		{
			heroIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? MaxHp
	{
		get
		{
			return maxHp_;
		}
		set
		{
			maxHp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? CurHp
	{
		get
		{
			return curHp_;
		}
		set
		{
			curHp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? WeaponLevel
	{
		get
		{
			return weaponLevel_;
		}
		set
		{
			weaponLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<HeroEquipInfoProto> EquipInfos => equipInfos_;

	[DebuggerNonUserCode]
	public RepeatedField<HeroSkillInfoProto> SkillInfos => skillInfos_;

	[DebuggerNonUserCode]
	public RepeatedField<BattleEffectInfo> Effects => effects_;

	[DebuggerNonUserCode]
	public ScoutDominator Dominator
	{
		get
		{
			return dominator_;
		}
		set
		{
			dominator_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<WeaponStrengthen> WeaponStrengthen => weaponStrengthen_;

	[DebuggerNonUserCode]
	public ScoutHero()
	{
	}

	[DebuggerNonUserCode]
	public ScoutHero(ScoutHero other)
		: this()
	{
		HeroId = other.HeroId;
		HeroLevel = other.HeroLevel;
		HeroQuality = other.HeroQuality;
		HeroRankLevel = other.HeroRankLevel;
		HeroIndex = other.HeroIndex;
		MaxHp = other.MaxHp;
		CurHp = other.CurHp;
		WeaponLevel = other.WeaponLevel;
		equipInfos_ = other.equipInfos_.Clone();
		skillInfos_ = other.skillInfos_.Clone();
		effects_ = other.effects_.Clone();
		dominator_ = ((other.dominator_ != null) ? other.dominator_.Clone() : null);
		weaponStrengthen_ = other.weaponStrengthen_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutHero Clone()
	{
		return new ScoutHero(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutHero);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutHero other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (HeroId != other.HeroId)
		{
			return false;
		}
		if (HeroLevel != other.HeroLevel)
		{
			return false;
		}
		if (HeroQuality != other.HeroQuality)
		{
			return false;
		}
		if (HeroRankLevel != other.HeroRankLevel)
		{
			return false;
		}
		if (HeroIndex != other.HeroIndex)
		{
			return false;
		}
		if (MaxHp != other.MaxHp)
		{
			return false;
		}
		if (CurHp != other.CurHp)
		{
			return false;
		}
		if (WeaponLevel != other.WeaponLevel)
		{
			return false;
		}
		if (!equipInfos_.Equals(other.equipInfos_))
		{
			return false;
		}
		if (!skillInfos_.Equals(other.skillInfos_))
		{
			return false;
		}
		if (!effects_.Equals(other.effects_))
		{
			return false;
		}
		if (!object.Equals(Dominator, other.Dominator))
		{
			return false;
		}
		if (!weaponStrengthen_.Equals(other.weaponStrengthen_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (heroId_.HasValue)
		{
			num ^= HeroId.GetHashCode();
		}
		if (heroLevel_.HasValue)
		{
			num ^= HeroLevel.GetHashCode();
		}
		if (heroQuality_.HasValue)
		{
			num ^= HeroQuality.GetHashCode();
		}
		if (heroRankLevel_.HasValue)
		{
			num ^= HeroRankLevel.GetHashCode();
		}
		if (heroIndex_.HasValue)
		{
			num ^= HeroIndex.GetHashCode();
		}
		if (maxHp_.HasValue)
		{
			num ^= MaxHp.GetHashCode();
		}
		if (curHp_.HasValue)
		{
			num ^= CurHp.GetHashCode();
		}
		if (weaponLevel_.HasValue)
		{
			num ^= WeaponLevel.GetHashCode();
		}
		num ^= equipInfos_.GetHashCode();
		num ^= skillInfos_.GetHashCode();
		num ^= effects_.GetHashCode();
		if (dominator_ != null)
		{
			num ^= Dominator.GetHashCode();
		}
		num ^= weaponStrengthen_.GetHashCode();
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
		if (heroId_.HasValue)
		{
			_single_heroId_codec.WriteTagAndValue(output, HeroId);
		}
		if (heroLevel_.HasValue)
		{
			_single_heroLevel_codec.WriteTagAndValue(output, HeroLevel);
		}
		if (heroQuality_.HasValue)
		{
			_single_heroQuality_codec.WriteTagAndValue(output, HeroQuality);
		}
		if (heroRankLevel_.HasValue)
		{
			_single_heroRankLevel_codec.WriteTagAndValue(output, HeroRankLevel);
		}
		if (heroIndex_.HasValue)
		{
			_single_heroIndex_codec.WriteTagAndValue(output, HeroIndex);
		}
		if (maxHp_.HasValue)
		{
			_single_maxHp_codec.WriteTagAndValue(output, MaxHp);
		}
		if (curHp_.HasValue)
		{
			_single_curHp_codec.WriteTagAndValue(output, CurHp);
		}
		if (weaponLevel_.HasValue)
		{
			_single_weaponLevel_codec.WriteTagAndValue(output, WeaponLevel);
		}
		equipInfos_.WriteTo(output, _repeated_equipInfos_codec);
		skillInfos_.WriteTo(output, _repeated_skillInfos_codec);
		effects_.WriteTo(output, _repeated_effects_codec);
		if (dominator_ != null)
		{
			output.WriteRawTag(98);
			output.WriteMessage(Dominator);
		}
		weaponStrengthen_.WriteTo(output, _repeated_weaponStrengthen_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (heroId_.HasValue)
		{
			num += _single_heroId_codec.CalculateSizeWithTag(HeroId);
		}
		if (heroLevel_.HasValue)
		{
			num += _single_heroLevel_codec.CalculateSizeWithTag(HeroLevel);
		}
		if (heroQuality_.HasValue)
		{
			num += _single_heroQuality_codec.CalculateSizeWithTag(HeroQuality);
		}
		if (heroRankLevel_.HasValue)
		{
			num += _single_heroRankLevel_codec.CalculateSizeWithTag(HeroRankLevel);
		}
		if (heroIndex_.HasValue)
		{
			num += _single_heroIndex_codec.CalculateSizeWithTag(HeroIndex);
		}
		if (maxHp_.HasValue)
		{
			num += _single_maxHp_codec.CalculateSizeWithTag(MaxHp);
		}
		if (curHp_.HasValue)
		{
			num += _single_curHp_codec.CalculateSizeWithTag(CurHp);
		}
		if (weaponLevel_.HasValue)
		{
			num += _single_weaponLevel_codec.CalculateSizeWithTag(WeaponLevel);
		}
		num += equipInfos_.CalculateSize(_repeated_equipInfos_codec);
		num += skillInfos_.CalculateSize(_repeated_skillInfos_codec);
		num += effects_.CalculateSize(_repeated_effects_codec);
		if (dominator_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Dominator);
		}
		num += weaponStrengthen_.CalculateSize(_repeated_weaponStrengthen_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutHero other)
	{
		if (other == null)
		{
			return;
		}
		if (other.heroId_.HasValue && (!heroId_.HasValue || other.HeroId != 0))
		{
			HeroId = other.HeroId;
		}
		if (other.heroLevel_.HasValue && (!heroLevel_.HasValue || other.HeroLevel != 0))
		{
			HeroLevel = other.HeroLevel;
		}
		if (other.heroQuality_.HasValue && (!heroQuality_.HasValue || other.HeroQuality != 0))
		{
			HeroQuality = other.HeroQuality;
		}
		if (other.heroRankLevel_.HasValue && (!heroRankLevel_.HasValue || other.HeroRankLevel != 0))
		{
			HeroRankLevel = other.HeroRankLevel;
		}
		if (other.heroIndex_.HasValue && (!heroIndex_.HasValue || other.HeroIndex != 0))
		{
			HeroIndex = other.HeroIndex;
		}
		if (other.maxHp_.HasValue && (!maxHp_.HasValue || other.MaxHp != 0))
		{
			MaxHp = other.MaxHp;
		}
		if (other.curHp_.HasValue && (!curHp_.HasValue || other.CurHp != 0))
		{
			CurHp = other.CurHp;
		}
		if (other.weaponLevel_.HasValue && (!weaponLevel_.HasValue || other.WeaponLevel != 0))
		{
			WeaponLevel = other.WeaponLevel;
		}
		equipInfos_.Add(other.equipInfos_);
		skillInfos_.Add(other.skillInfos_);
		effects_.Add(other.effects_);
		if (other.dominator_ != null)
		{
			if (dominator_ == null)
			{
				Dominator = new ScoutDominator();
			}
			Dominator.MergeFrom(other.Dominator);
		}
		weaponStrengthen_.Add(other.weaponStrengthen_);
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
				int? num4 = _single_heroId_codec.Read(input);
				if (!heroId_.HasValue || num4 != 0)
				{
					HeroId = num4;
				}
				break;
			}
			case 18u:
			{
				int? num2 = _single_heroLevel_codec.Read(input);
				if (!heroLevel_.HasValue || num2 != 0)
				{
					HeroLevel = num2;
				}
				break;
			}
			case 26u:
			{
				int? num8 = _single_heroQuality_codec.Read(input);
				if (!heroQuality_.HasValue || num8 != 0)
				{
					HeroQuality = num8;
				}
				break;
			}
			case 34u:
			{
				int? num7 = _single_heroRankLevel_codec.Read(input);
				if (!heroRankLevel_.HasValue || num7 != 0)
				{
					HeroRankLevel = num7;
				}
				break;
			}
			case 42u:
			{
				int? num5 = _single_heroIndex_codec.Read(input);
				if (!heroIndex_.HasValue || num5 != 0)
				{
					HeroIndex = num5;
				}
				break;
			}
			case 50u:
			{
				int? num9 = _single_maxHp_codec.Read(input);
				if (!maxHp_.HasValue || num9 != 0)
				{
					MaxHp = num9;
				}
				break;
			}
			case 58u:
			{
				int? num6 = _single_curHp_codec.Read(input);
				if (!curHp_.HasValue || num6 != 0)
				{
					CurHp = num6;
				}
				break;
			}
			case 66u:
			{
				int? num3 = _single_weaponLevel_codec.Read(input);
				if (!weaponLevel_.HasValue || num3 != 0)
				{
					WeaponLevel = num3;
				}
				break;
			}
			case 74u:
				equipInfos_.AddEntriesFrom(input, _repeated_equipInfos_codec);
				break;
			case 82u:
				skillInfos_.AddEntriesFrom(input, _repeated_skillInfos_codec);
				break;
			case 90u:
				effects_.AddEntriesFrom(input, _repeated_effects_codec);
				break;
			case 98u:
				if (dominator_ == null)
				{
					Dominator = new ScoutDominator();
				}
				input.ReadMessage(Dominator);
				break;
			case 106u:
				weaponStrengthen_.AddEntriesFrom(input, _repeated_weaponStrengthen_codec);
				break;
			}
		}
	}
}
