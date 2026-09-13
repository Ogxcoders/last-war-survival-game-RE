using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class HeroInfoProto : IMessage<HeroInfoProto>, IMessage, IEquatable<HeroInfoProto>, IDeepCloneable<HeroInfoProto>
{
	private static readonly MessageParser<HeroInfoProto> _parser = new MessageParser<HeroInfoProto>(() => new HeroInfoProto());

	private UnknownFieldSet _unknownFields;

	public const int HeroIdFieldNumber = 1;

	private int heroId_;

	public const int HeroLevelFieldNumber = 2;

	private int heroLevel_;

	public const int HeroQualityFieldNumber = 3;

	private int heroQuality_;

	public const int IndexFieldNumber = 4;

	private int index_;

	public const int SkillInfosFieldNumber = 5;

	private static readonly FieldCodec<HeroSkillInfoProto> _repeated_skillInfos_codec = FieldCodec.ForMessage(42u, HeroSkillInfoProto.Parser);

	private readonly RepeatedField<HeroSkillInfoProto> skillInfos_ = new RepeatedField<HeroSkillInfoProto>();

	public const int HeroUuidFieldNumber = 6;

	private long heroUuid_;

	public const int MaxLevelFieldNumber = 7;

	private int maxLevel_;

	public const int RankLvFieldNumber = 8;

	private int rankLv_;

	public const int StageFieldNumber = 9;

	private int stage_;

	public const int HeroIndexFieldNumber = 10;

	private int heroIndex_;

	public const int EffectInfosFieldNumber = 11;

	private static readonly FieldCodec<HeroEffectProto> _repeated_effectInfos_codec = FieldCodec.ForMessage(90u, HeroEffectProto.Parser);

	private readonly RepeatedField<HeroEffectProto> effectInfos_ = new RepeatedField<HeroEffectProto>();

	public const int MaxHealthFieldNumber = 12;

	private int maxHealth_;

	public const int CurHealthFieldNumber = 13;

	private int curHealth_;

	public const int EquipInfosFieldNumber = 14;

	private static readonly FieldCodec<HeroEquipInfoProto> _repeated_equipInfos_codec = FieldCodec.ForMessage(114u, HeroEquipInfoProto.Parser);

	private readonly RepeatedField<HeroEquipInfoProto> equipInfos_ = new RepeatedField<HeroEquipInfoProto>();

	public const int WeaponLevelFieldNumber = 15;

	private int weaponLevel_;

	public const int DominatorFieldNumber = 16;

	private DominatorProto dominator_;

	public const int WeaponStrengthenFieldNumber = 17;

	private static readonly FieldCodec<WeaponStrengthen> _repeated_weaponStrengthen_codec = FieldCodec.ForMessage(138u, Protobuf.WeaponStrengthen.Parser);

	private readonly RepeatedField<WeaponStrengthen> weaponStrengthen_ = new RepeatedField<WeaponStrengthen>();

	[DebuggerNonUserCode]
	public static MessageParser<HeroInfoProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[4];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int HeroId
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
	public int HeroLevel
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
	public int HeroQuality
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
	public RepeatedField<HeroSkillInfoProto> SkillInfos => skillInfos_;

	[DebuggerNonUserCode]
	public long HeroUuid
	{
		get
		{
			return heroUuid_;
		}
		set
		{
			heroUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxLevel
	{
		get
		{
			return maxLevel_;
		}
		set
		{
			maxLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RankLv
	{
		get
		{
			return rankLv_;
		}
		set
		{
			rankLv_ = value;
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
	public int HeroIndex
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
	public RepeatedField<HeroEffectProto> EffectInfos => effectInfos_;

	[DebuggerNonUserCode]
	public int MaxHealth
	{
		get
		{
			return maxHealth_;
		}
		set
		{
			maxHealth_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurHealth
	{
		get
		{
			return curHealth_;
		}
		set
		{
			curHealth_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<HeroEquipInfoProto> EquipInfos => equipInfos_;

	[DebuggerNonUserCode]
	public int WeaponLevel
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
	public DominatorProto Dominator
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
	public HeroInfoProto()
	{
	}

	[DebuggerNonUserCode]
	public HeroInfoProto(HeroInfoProto other)
		: this()
	{
		heroId_ = other.heroId_;
		heroLevel_ = other.heroLevel_;
		heroQuality_ = other.heroQuality_;
		index_ = other.index_;
		skillInfos_ = other.skillInfos_.Clone();
		heroUuid_ = other.heroUuid_;
		maxLevel_ = other.maxLevel_;
		rankLv_ = other.rankLv_;
		stage_ = other.stage_;
		heroIndex_ = other.heroIndex_;
		effectInfos_ = other.effectInfos_.Clone();
		maxHealth_ = other.maxHealth_;
		curHealth_ = other.curHealth_;
		equipInfos_ = other.equipInfos_.Clone();
		weaponLevel_ = other.weaponLevel_;
		dominator_ = ((other.dominator_ != null) ? other.dominator_.Clone() : null);
		weaponStrengthen_ = other.weaponStrengthen_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public HeroInfoProto Clone()
	{
		return new HeroInfoProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as HeroInfoProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(HeroInfoProto other)
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
		if (Index != other.Index)
		{
			return false;
		}
		if (!skillInfos_.Equals(other.skillInfos_))
		{
			return false;
		}
		if (HeroUuid != other.HeroUuid)
		{
			return false;
		}
		if (MaxLevel != other.MaxLevel)
		{
			return false;
		}
		if (RankLv != other.RankLv)
		{
			return false;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		if (HeroIndex != other.HeroIndex)
		{
			return false;
		}
		if (!effectInfos_.Equals(other.effectInfos_))
		{
			return false;
		}
		if (MaxHealth != other.MaxHealth)
		{
			return false;
		}
		if (CurHealth != other.CurHealth)
		{
			return false;
		}
		if (!equipInfos_.Equals(other.equipInfos_))
		{
			return false;
		}
		if (WeaponLevel != other.WeaponLevel)
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
		if (HeroId != 0)
		{
			num ^= HeroId.GetHashCode();
		}
		if (HeroLevel != 0)
		{
			num ^= HeroLevel.GetHashCode();
		}
		if (HeroQuality != 0)
		{
			num ^= HeroQuality.GetHashCode();
		}
		if (Index != 0)
		{
			num ^= Index.GetHashCode();
		}
		num ^= skillInfos_.GetHashCode();
		if (HeroUuid != 0L)
		{
			num ^= HeroUuid.GetHashCode();
		}
		if (MaxLevel != 0)
		{
			num ^= MaxLevel.GetHashCode();
		}
		if (RankLv != 0)
		{
			num ^= RankLv.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (HeroIndex != 0)
		{
			num ^= HeroIndex.GetHashCode();
		}
		num ^= effectInfos_.GetHashCode();
		if (MaxHealth != 0)
		{
			num ^= MaxHealth.GetHashCode();
		}
		if (CurHealth != 0)
		{
			num ^= CurHealth.GetHashCode();
		}
		num ^= equipInfos_.GetHashCode();
		if (WeaponLevel != 0)
		{
			num ^= WeaponLevel.GetHashCode();
		}
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
		if (HeroId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(HeroId);
		}
		if (HeroLevel != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(HeroLevel);
		}
		if (HeroQuality != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(HeroQuality);
		}
		if (Index != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Index);
		}
		skillInfos_.WriteTo(output, _repeated_skillInfos_codec);
		if (HeroUuid != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(HeroUuid);
		}
		if (MaxLevel != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(MaxLevel);
		}
		if (RankLv != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(RankLv);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Stage);
		}
		if (HeroIndex != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(HeroIndex);
		}
		effectInfos_.WriteTo(output, _repeated_effectInfos_codec);
		if (MaxHealth != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(MaxHealth);
		}
		if (CurHealth != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(CurHealth);
		}
		equipInfos_.WriteTo(output, _repeated_equipInfos_codec);
		if (WeaponLevel != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(WeaponLevel);
		}
		if (dominator_ != null)
		{
			output.WriteRawTag(130, 1);
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
		if (HeroId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeroId);
		}
		if (HeroLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeroLevel);
		}
		if (HeroQuality != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeroQuality);
		}
		if (Index != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Index);
		}
		num += skillInfos_.CalculateSize(_repeated_skillInfos_codec);
		if (HeroUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(HeroUuid);
		}
		if (MaxLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxLevel);
		}
		if (RankLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RankLv);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (HeroIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeroIndex);
		}
		num += effectInfos_.CalculateSize(_repeated_effectInfos_codec);
		if (MaxHealth != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxHealth);
		}
		if (CurHealth != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurHealth);
		}
		num += equipInfos_.CalculateSize(_repeated_equipInfos_codec);
		if (WeaponLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WeaponLevel);
		}
		if (dominator_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Dominator);
		}
		num += weaponStrengthen_.CalculateSize(_repeated_weaponStrengthen_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(HeroInfoProto other)
	{
		if (other == null)
		{
			return;
		}
		if (other.HeroId != 0)
		{
			HeroId = other.HeroId;
		}
		if (other.HeroLevel != 0)
		{
			HeroLevel = other.HeroLevel;
		}
		if (other.HeroQuality != 0)
		{
			HeroQuality = other.HeroQuality;
		}
		if (other.Index != 0)
		{
			Index = other.Index;
		}
		skillInfos_.Add(other.skillInfos_);
		if (other.HeroUuid != 0L)
		{
			HeroUuid = other.HeroUuid;
		}
		if (other.MaxLevel != 0)
		{
			MaxLevel = other.MaxLevel;
		}
		if (other.RankLv != 0)
		{
			RankLv = other.RankLv;
		}
		if (other.Stage != 0)
		{
			Stage = other.Stage;
		}
		if (other.HeroIndex != 0)
		{
			HeroIndex = other.HeroIndex;
		}
		effectInfos_.Add(other.effectInfos_);
		if (other.MaxHealth != 0)
		{
			MaxHealth = other.MaxHealth;
		}
		if (other.CurHealth != 0)
		{
			CurHealth = other.CurHealth;
		}
		equipInfos_.Add(other.equipInfos_);
		if (other.WeaponLevel != 0)
		{
			WeaponLevel = other.WeaponLevel;
		}
		if (other.dominator_ != null)
		{
			if (dominator_ == null)
			{
				Dominator = new DominatorProto();
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
			case 8u:
				HeroId = input.ReadInt32();
				break;
			case 16u:
				HeroLevel = input.ReadInt32();
				break;
			case 24u:
				HeroQuality = input.ReadInt32();
				break;
			case 32u:
				Index = input.ReadInt32();
				break;
			case 42u:
				skillInfos_.AddEntriesFrom(input, _repeated_skillInfos_codec);
				break;
			case 48u:
				HeroUuid = input.ReadInt64();
				break;
			case 56u:
				MaxLevel = input.ReadInt32();
				break;
			case 64u:
				RankLv = input.ReadInt32();
				break;
			case 72u:
				Stage = input.ReadInt32();
				break;
			case 80u:
				HeroIndex = input.ReadInt32();
				break;
			case 90u:
				effectInfos_.AddEntriesFrom(input, _repeated_effectInfos_codec);
				break;
			case 96u:
				MaxHealth = input.ReadInt32();
				break;
			case 104u:
				CurHealth = input.ReadInt32();
				break;
			case 114u:
				equipInfos_.AddEntriesFrom(input, _repeated_equipInfos_codec);
				break;
			case 120u:
				WeaponLevel = input.ReadInt32();
				break;
			case 130u:
				if (dominator_ == null)
				{
					Dominator = new DominatorProto();
				}
				input.ReadMessage(Dominator);
				break;
			case 138u:
				weaponStrengthen_.AddEntriesFrom(input, _repeated_weaponStrengthen_codec);
				break;
			}
		}
	}
}
