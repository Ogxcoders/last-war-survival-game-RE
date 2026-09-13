using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ArmyUnitInfo : IMessage<ArmyUnitInfo>, IMessage, IEquatable<ArmyUnitInfo>, IDeepCloneable<ArmyUnitInfo>
{
	private static readonly MessageParser<ArmyUnitInfo> _parser = new MessageParser<ArmyUnitInfo>(() => new ArmyUnitInfo());

	private UnknownFieldSet _unknownFields;

	public const int SoldiersFieldNumber = 1;

	private static readonly FieldCodec<SoldierProto> _repeated_soldiers_codec = FieldCodec.ForMessage(10u, SoldierProto.Parser);

	private readonly RepeatedField<SoldierProto> soldiers_ = new RepeatedField<SoldierProto>();

	public const int HeroesFieldNumber = 2;

	private static readonly FieldCodec<HeroInfoProto> _repeated_heroes_codec = FieldCodec.ForMessage(18u, HeroInfoProto.Parser);

	private readonly RepeatedField<HeroInfoProto> heroes_ = new RepeatedField<HeroInfoProto>();

	public const int NameFieldNumber = 3;

	private string name_ = "";

	public const int AlAbbrFieldNumber = 4;

	private string alAbbr_ = "";

	public const int PicFieldNumber = 5;

	private string pic_ = "";

	public const int PicVerFieldNumber = 6;

	private int picVer_;

	public const int HeadFrameFieldNumber = 7;

	private int headFrame_;

	public const int CareerTypeFieldNumber = 8;

	private int careerType_;

	public const int CareerLvFieldNumber = 9;

	private int careerLv_;

	public const int UnitBuffsFieldNumber = 10;

	private static readonly FieldCodec<ArmyUnitBuff> _repeated_unitBuffs_codec = FieldCodec.ForMessage(82u, ArmyUnitBuff.Parser);

	private readonly RepeatedField<ArmyUnitBuff> unitBuffs_ = new RepeatedField<ArmyUnitBuff>();

	public const int HeadSkinIdFieldNumber = 11;

	private int headSkinId_;

	public const int WeaponFieldNumber = 12;

	private WeaponProto weapon_;

	public const int WeaponSkinIdFieldNumber = 13;

	private int weaponSkinId_;

	public const int HasSoldierFieldNumber = 14;

	private bool hasSoldier_;

	public const int HeroModuleFieldNumber = 15;

	private HeroModuleProto heroModule_;

	[DebuggerNonUserCode]
	public static MessageParser<ArmyUnitInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[15];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<SoldierProto> Soldiers => soldiers_;

	[DebuggerNonUserCode]
	public RepeatedField<HeroInfoProto> Heroes => heroes_;

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AlAbbr
	{
		get
		{
			return alAbbr_;
		}
		set
		{
			alAbbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Pic
	{
		get
		{
			return pic_;
		}
		set
		{
			pic_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int PicVer
	{
		get
		{
			return picVer_;
		}
		set
		{
			picVer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeadFrame
	{
		get
		{
			return headFrame_;
		}
		set
		{
			headFrame_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CareerType
	{
		get
		{
			return careerType_;
		}
		set
		{
			careerType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CareerLv
	{
		get
		{
			return careerLv_;
		}
		set
		{
			careerLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<ArmyUnitBuff> UnitBuffs => unitBuffs_;

	[DebuggerNonUserCode]
	public int HeadSkinId
	{
		get
		{
			return headSkinId_;
		}
		set
		{
			headSkinId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WeaponProto Weapon
	{
		get
		{
			return weapon_;
		}
		set
		{
			weapon_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WeaponSkinId
	{
		get
		{
			return weaponSkinId_;
		}
		set
		{
			weaponSkinId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasSoldier
	{
		get
		{
			return hasSoldier_;
		}
		set
		{
			hasSoldier_ = value;
		}
	}

	[DebuggerNonUserCode]
	public HeroModuleProto HeroModule
	{
		get
		{
			return heroModule_;
		}
		set
		{
			heroModule_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyUnitInfo()
	{
	}

	[DebuggerNonUserCode]
	public ArmyUnitInfo(ArmyUnitInfo other)
		: this()
	{
		soldiers_ = other.soldiers_.Clone();
		heroes_ = other.heroes_.Clone();
		name_ = other.name_;
		alAbbr_ = other.alAbbr_;
		pic_ = other.pic_;
		picVer_ = other.picVer_;
		headFrame_ = other.headFrame_;
		careerType_ = other.careerType_;
		careerLv_ = other.careerLv_;
		unitBuffs_ = other.unitBuffs_.Clone();
		headSkinId_ = other.headSkinId_;
		weapon_ = ((other.weapon_ != null) ? other.weapon_.Clone() : null);
		weaponSkinId_ = other.weaponSkinId_;
		hasSoldier_ = other.hasSoldier_;
		heroModule_ = ((other.heroModule_ != null) ? other.heroModule_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ArmyUnitInfo Clone()
	{
		return new ArmyUnitInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ArmyUnitInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ArmyUnitInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!soldiers_.Equals(other.soldiers_))
		{
			return false;
		}
		if (!heroes_.Equals(other.heroes_))
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (AlAbbr != other.AlAbbr)
		{
			return false;
		}
		if (Pic != other.Pic)
		{
			return false;
		}
		if (PicVer != other.PicVer)
		{
			return false;
		}
		if (HeadFrame != other.HeadFrame)
		{
			return false;
		}
		if (CareerType != other.CareerType)
		{
			return false;
		}
		if (CareerLv != other.CareerLv)
		{
			return false;
		}
		if (!unitBuffs_.Equals(other.unitBuffs_))
		{
			return false;
		}
		if (HeadSkinId != other.HeadSkinId)
		{
			return false;
		}
		if (!object.Equals(Weapon, other.Weapon))
		{
			return false;
		}
		if (WeaponSkinId != other.WeaponSkinId)
		{
			return false;
		}
		if (HasSoldier != other.HasSoldier)
		{
			return false;
		}
		if (!object.Equals(HeroModule, other.HeroModule))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= soldiers_.GetHashCode();
		num ^= heroes_.GetHashCode();
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (AlAbbr.Length != 0)
		{
			num ^= AlAbbr.GetHashCode();
		}
		if (Pic.Length != 0)
		{
			num ^= Pic.GetHashCode();
		}
		if (PicVer != 0)
		{
			num ^= PicVer.GetHashCode();
		}
		if (HeadFrame != 0)
		{
			num ^= HeadFrame.GetHashCode();
		}
		if (CareerType != 0)
		{
			num ^= CareerType.GetHashCode();
		}
		if (CareerLv != 0)
		{
			num ^= CareerLv.GetHashCode();
		}
		num ^= unitBuffs_.GetHashCode();
		if (HeadSkinId != 0)
		{
			num ^= HeadSkinId.GetHashCode();
		}
		if (weapon_ != null)
		{
			num ^= Weapon.GetHashCode();
		}
		if (WeaponSkinId != 0)
		{
			num ^= WeaponSkinId.GetHashCode();
		}
		if (HasSoldier)
		{
			num ^= HasSoldier.GetHashCode();
		}
		if (heroModule_ != null)
		{
			num ^= HeroModule.GetHashCode();
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
		soldiers_.WriteTo(output, _repeated_soldiers_codec);
		heroes_.WriteTo(output, _repeated_heroes_codec);
		if (Name.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Name);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(AlAbbr);
		}
		if (Pic.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(Pic);
		}
		if (PicVer != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(PicVer);
		}
		if (HeadFrame != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(HeadFrame);
		}
		if (CareerType != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(CareerType);
		}
		if (CareerLv != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(CareerLv);
		}
		unitBuffs_.WriteTo(output, _repeated_unitBuffs_codec);
		if (HeadSkinId != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(HeadSkinId);
		}
		if (weapon_ != null)
		{
			output.WriteRawTag(98);
			output.WriteMessage(Weapon);
		}
		if (WeaponSkinId != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(WeaponSkinId);
		}
		if (HasSoldier)
		{
			output.WriteRawTag(112);
			output.WriteBool(HasSoldier);
		}
		if (heroModule_ != null)
		{
			output.WriteRawTag(122);
			output.WriteMessage(HeroModule);
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
		num += soldiers_.CalculateSize(_repeated_soldiers_codec);
		num += heroes_.CalculateSize(_repeated_heroes_codec);
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (AlAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlAbbr);
		}
		if (Pic.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Pic);
		}
		if (PicVer != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PicVer);
		}
		if (HeadFrame != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadFrame);
		}
		if (CareerType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CareerType);
		}
		if (CareerLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CareerLv);
		}
		num += unitBuffs_.CalculateSize(_repeated_unitBuffs_codec);
		if (HeadSkinId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadSkinId);
		}
		if (weapon_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Weapon);
		}
		if (WeaponSkinId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WeaponSkinId);
		}
		if (HasSoldier)
		{
			num += 2;
		}
		if (heroModule_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(HeroModule);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ArmyUnitInfo other)
	{
		if (other == null)
		{
			return;
		}
		soldiers_.Add(other.soldiers_);
		heroes_.Add(other.heroes_);
		if (other.Name.Length != 0)
		{
			Name = other.Name;
		}
		if (other.AlAbbr.Length != 0)
		{
			AlAbbr = other.AlAbbr;
		}
		if (other.Pic.Length != 0)
		{
			Pic = other.Pic;
		}
		if (other.PicVer != 0)
		{
			PicVer = other.PicVer;
		}
		if (other.HeadFrame != 0)
		{
			HeadFrame = other.HeadFrame;
		}
		if (other.CareerType != 0)
		{
			CareerType = other.CareerType;
		}
		if (other.CareerLv != 0)
		{
			CareerLv = other.CareerLv;
		}
		unitBuffs_.Add(other.unitBuffs_);
		if (other.HeadSkinId != 0)
		{
			HeadSkinId = other.HeadSkinId;
		}
		if (other.weapon_ != null)
		{
			if (weapon_ == null)
			{
				Weapon = new WeaponProto();
			}
			Weapon.MergeFrom(other.Weapon);
		}
		if (other.WeaponSkinId != 0)
		{
			WeaponSkinId = other.WeaponSkinId;
		}
		if (other.HasSoldier)
		{
			HasSoldier = other.HasSoldier;
		}
		if (other.heroModule_ != null)
		{
			if (heroModule_ == null)
			{
				HeroModule = new HeroModuleProto();
			}
			HeroModule.MergeFrom(other.HeroModule);
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
				soldiers_.AddEntriesFrom(input, _repeated_soldiers_codec);
				break;
			case 18u:
				heroes_.AddEntriesFrom(input, _repeated_heroes_codec);
				break;
			case 26u:
				Name = input.ReadString();
				break;
			case 34u:
				AlAbbr = input.ReadString();
				break;
			case 42u:
				Pic = input.ReadString();
				break;
			case 48u:
				PicVer = input.ReadInt32();
				break;
			case 56u:
				HeadFrame = input.ReadInt32();
				break;
			case 64u:
				CareerType = input.ReadInt32();
				break;
			case 72u:
				CareerLv = input.ReadInt32();
				break;
			case 82u:
				unitBuffs_.AddEntriesFrom(input, _repeated_unitBuffs_codec);
				break;
			case 88u:
				HeadSkinId = input.ReadInt32();
				break;
			case 98u:
				if (weapon_ == null)
				{
					Weapon = new WeaponProto();
				}
				input.ReadMessage(Weapon);
				break;
			case 104u:
				WeaponSkinId = input.ReadInt32();
				break;
			case 112u:
				HasSoldier = input.ReadBool();
				break;
			case 122u:
				if (heroModule_ == null)
				{
					HeroModule = new HeroModuleProto();
				}
				input.ReadMessage(HeroModule);
				break;
			}
		}
	}
}
