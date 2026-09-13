using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwBattleUnitStat : IMessage<LwBattleUnitStat>, IMessage, IEquatable<LwBattleUnitStat>, IDeepCloneable<LwBattleUnitStat>
{
	private static readonly MessageParser<LwBattleUnitStat> _parser = new MessageParser<LwBattleUnitStat>(() => new LwBattleUnitStat());

	private UnknownFieldSet _unknownFields;

	public const int DamageFieldNumber = 1;

	private float damage_;

	public const int InjuredFieldNumber = 2;

	private float injured_;

	public const int EnhanceFieldNumber = 3;

	private float enhance_;

	public const int WeakenFieldNumber = 4;

	private float weaken_;

	public const int CritDamageFieldNumber = 5;

	private float critDamage_;

	public const int CritNumFieldNumber = 6;

	private int critNum_;

	public const int DizzyNumFieldNumber = 7;

	private int dizzyNum_;

	public const int PhysicalInjuredFieldNumber = 8;

	private float physicalInjured_;

	public const int MagicInjuredFieldNumber = 9;

	private float magicInjured_;

	public const int DotDamageFieldNumber = 10;

	private float dotDamage_;

	public const int SputterDamageFieldNumber = 11;

	private float sputterDamage_;

	public const int PhysicalDamageFieldNumber = 12;

	private float physicalDamage_;

	public const int MagicDamageFieldNumber = 13;

	private float magicDamage_;

	public const int DamageNumFieldNumber = 14;

	private int damageNum_;

	public const int DefenseNumFieldNumber = 15;

	private int defenseNum_;

	public const int DefenseDamageFieldNumber = 16;

	private float defenseDamage_;

	public const int SkillDamageMapFieldNumber = 17;

	private static readonly MapField<int, float>.Codec _map_skillDamageMap_codec = new MapField<int, float>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForFloat(21u, 0f), 138u);

	private readonly MapField<int, float> skillDamageMap_ = new MapField<int, float>();

	public const int SkillInjuredMapFieldNumber = 18;

	private static readonly MapField<int, float>.Codec _map_skillInjuredMap_codec = new MapField<int, float>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForFloat(21u, 0f), 146u);

	private readonly MapField<int, float> skillInjuredMap_ = new MapField<int, float>();

	public const int DispelNumFieldNumber = 19;

	private int dispelNum_;

	public const int DispelDebufNumFieldNumber = 20;

	private int dispelDebufNum_;

	public const int RealDamageFieldNumber = 21;

	private float realDamage_;

	public const int ShieldDamageFieldNumber = 22;

	private float shieldDamage_;

	public const int ImmuneBuffCountFieldNumber = 23;

	private int immuneBuffCount_;

	[DebuggerNonUserCode]
	public static MessageParser<LwBattleUnitStat> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public float Damage
	{
		get
		{
			return damage_;
		}
		set
		{
			damage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float Injured
	{
		get
		{
			return injured_;
		}
		set
		{
			injured_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float Enhance
	{
		get
		{
			return enhance_;
		}
		set
		{
			enhance_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float Weaken
	{
		get
		{
			return weaken_;
		}
		set
		{
			weaken_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float CritDamage
	{
		get
		{
			return critDamage_;
		}
		set
		{
			critDamage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CritNum
	{
		get
		{
			return critNum_;
		}
		set
		{
			critNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DizzyNum
	{
		get
		{
			return dizzyNum_;
		}
		set
		{
			dizzyNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float PhysicalInjured
	{
		get
		{
			return physicalInjured_;
		}
		set
		{
			physicalInjured_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float MagicInjured
	{
		get
		{
			return magicInjured_;
		}
		set
		{
			magicInjured_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float DotDamage
	{
		get
		{
			return dotDamage_;
		}
		set
		{
			dotDamage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float SputterDamage
	{
		get
		{
			return sputterDamage_;
		}
		set
		{
			sputterDamage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float PhysicalDamage
	{
		get
		{
			return physicalDamage_;
		}
		set
		{
			physicalDamage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float MagicDamage
	{
		get
		{
			return magicDamage_;
		}
		set
		{
			magicDamage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DamageNum
	{
		get
		{
			return damageNum_;
		}
		set
		{
			damageNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DefenseNum
	{
		get
		{
			return defenseNum_;
		}
		set
		{
			defenseNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float DefenseDamage
	{
		get
		{
			return defenseDamage_;
		}
		set
		{
			defenseDamage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MapField<int, float> SkillDamageMap => skillDamageMap_;

	[DebuggerNonUserCode]
	public MapField<int, float> SkillInjuredMap => skillInjuredMap_;

	[DebuggerNonUserCode]
	public int DispelNum
	{
		get
		{
			return dispelNum_;
		}
		set
		{
			dispelNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DispelDebufNum
	{
		get
		{
			return dispelDebufNum_;
		}
		set
		{
			dispelDebufNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float RealDamage
	{
		get
		{
			return realDamage_;
		}
		set
		{
			realDamage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float ShieldDamage
	{
		get
		{
			return shieldDamage_;
		}
		set
		{
			shieldDamage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ImmuneBuffCount
	{
		get
		{
			return immuneBuffCount_;
		}
		set
		{
			immuneBuffCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LwBattleUnitStat()
	{
	}

	[DebuggerNonUserCode]
	public LwBattleUnitStat(LwBattleUnitStat other)
		: this()
	{
		damage_ = other.damage_;
		injured_ = other.injured_;
		enhance_ = other.enhance_;
		weaken_ = other.weaken_;
		critDamage_ = other.critDamage_;
		critNum_ = other.critNum_;
		dizzyNum_ = other.dizzyNum_;
		physicalInjured_ = other.physicalInjured_;
		magicInjured_ = other.magicInjured_;
		dotDamage_ = other.dotDamage_;
		sputterDamage_ = other.sputterDamage_;
		physicalDamage_ = other.physicalDamage_;
		magicDamage_ = other.magicDamage_;
		damageNum_ = other.damageNum_;
		defenseNum_ = other.defenseNum_;
		defenseDamage_ = other.defenseDamage_;
		skillDamageMap_ = other.skillDamageMap_.Clone();
		skillInjuredMap_ = other.skillInjuredMap_.Clone();
		dispelNum_ = other.dispelNum_;
		dispelDebufNum_ = other.dispelDebufNum_;
		realDamage_ = other.realDamage_;
		shieldDamage_ = other.shieldDamage_;
		immuneBuffCount_ = other.immuneBuffCount_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwBattleUnitStat Clone()
	{
		return new LwBattleUnitStat(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwBattleUnitStat);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwBattleUnitStat other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Damage, other.Damage))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Injured, other.Injured))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Enhance, other.Enhance))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Weaken, other.Weaken))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(CritDamage, other.CritDamage))
		{
			return false;
		}
		if (CritNum != other.CritNum)
		{
			return false;
		}
		if (DizzyNum != other.DizzyNum)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(PhysicalInjured, other.PhysicalInjured))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(MagicInjured, other.MagicInjured))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(DotDamage, other.DotDamage))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(SputterDamage, other.SputterDamage))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(PhysicalDamage, other.PhysicalDamage))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(MagicDamage, other.MagicDamage))
		{
			return false;
		}
		if (DamageNum != other.DamageNum)
		{
			return false;
		}
		if (DefenseNum != other.DefenseNum)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(DefenseDamage, other.DefenseDamage))
		{
			return false;
		}
		if (!SkillDamageMap.Equals(other.SkillDamageMap))
		{
			return false;
		}
		if (!SkillInjuredMap.Equals(other.SkillInjuredMap))
		{
			return false;
		}
		if (DispelNum != other.DispelNum)
		{
			return false;
		}
		if (DispelDebufNum != other.DispelDebufNum)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(RealDamage, other.RealDamage))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(ShieldDamage, other.ShieldDamage))
		{
			return false;
		}
		if (ImmuneBuffCount != other.ImmuneBuffCount)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Damage != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Damage);
		}
		if (Injured != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Injured);
		}
		if (Enhance != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Enhance);
		}
		if (Weaken != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Weaken);
		}
		if (CritDamage != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(CritDamage);
		}
		if (CritNum != 0)
		{
			num ^= CritNum.GetHashCode();
		}
		if (DizzyNum != 0)
		{
			num ^= DizzyNum.GetHashCode();
		}
		if (PhysicalInjured != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(PhysicalInjured);
		}
		if (MagicInjured != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(MagicInjured);
		}
		if (DotDamage != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(DotDamage);
		}
		if (SputterDamage != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(SputterDamage);
		}
		if (PhysicalDamage != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(PhysicalDamage);
		}
		if (MagicDamage != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(MagicDamage);
		}
		if (DamageNum != 0)
		{
			num ^= DamageNum.GetHashCode();
		}
		if (DefenseNum != 0)
		{
			num ^= DefenseNum.GetHashCode();
		}
		if (DefenseDamage != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(DefenseDamage);
		}
		num ^= SkillDamageMap.GetHashCode();
		num ^= SkillInjuredMap.GetHashCode();
		if (DispelNum != 0)
		{
			num ^= DispelNum.GetHashCode();
		}
		if (DispelDebufNum != 0)
		{
			num ^= DispelDebufNum.GetHashCode();
		}
		if (RealDamage != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(RealDamage);
		}
		if (ShieldDamage != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(ShieldDamage);
		}
		if (ImmuneBuffCount != 0)
		{
			num ^= ImmuneBuffCount.GetHashCode();
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
		if (Damage != 0f)
		{
			output.WriteRawTag(13);
			output.WriteFloat(Damage);
		}
		if (Injured != 0f)
		{
			output.WriteRawTag(21);
			output.WriteFloat(Injured);
		}
		if (Enhance != 0f)
		{
			output.WriteRawTag(29);
			output.WriteFloat(Enhance);
		}
		if (Weaken != 0f)
		{
			output.WriteRawTag(37);
			output.WriteFloat(Weaken);
		}
		if (CritDamage != 0f)
		{
			output.WriteRawTag(45);
			output.WriteFloat(CritDamage);
		}
		if (CritNum != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(CritNum);
		}
		if (DizzyNum != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(DizzyNum);
		}
		if (PhysicalInjured != 0f)
		{
			output.WriteRawTag(69);
			output.WriteFloat(PhysicalInjured);
		}
		if (MagicInjured != 0f)
		{
			output.WriteRawTag(77);
			output.WriteFloat(MagicInjured);
		}
		if (DotDamage != 0f)
		{
			output.WriteRawTag(85);
			output.WriteFloat(DotDamage);
		}
		if (SputterDamage != 0f)
		{
			output.WriteRawTag(93);
			output.WriteFloat(SputterDamage);
		}
		if (PhysicalDamage != 0f)
		{
			output.WriteRawTag(101);
			output.WriteFloat(PhysicalDamage);
		}
		if (MagicDamage != 0f)
		{
			output.WriteRawTag(109);
			output.WriteFloat(MagicDamage);
		}
		if (DamageNum != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(DamageNum);
		}
		if (DefenseNum != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(DefenseNum);
		}
		if (DefenseDamage != 0f)
		{
			output.WriteRawTag(133, 1);
			output.WriteFloat(DefenseDamage);
		}
		skillDamageMap_.WriteTo(output, _map_skillDamageMap_codec);
		skillInjuredMap_.WriteTo(output, _map_skillInjuredMap_codec);
		if (DispelNum != 0)
		{
			output.WriteRawTag(152, 1);
			output.WriteInt32(DispelNum);
		}
		if (DispelDebufNum != 0)
		{
			output.WriteRawTag(160, 1);
			output.WriteInt32(DispelDebufNum);
		}
		if (RealDamage != 0f)
		{
			output.WriteRawTag(173, 1);
			output.WriteFloat(RealDamage);
		}
		if (ShieldDamage != 0f)
		{
			output.WriteRawTag(181, 1);
			output.WriteFloat(ShieldDamage);
		}
		if (ImmuneBuffCount != 0)
		{
			output.WriteRawTag(184, 1);
			output.WriteInt32(ImmuneBuffCount);
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
		if (Damage != 0f)
		{
			num += 5;
		}
		if (Injured != 0f)
		{
			num += 5;
		}
		if (Enhance != 0f)
		{
			num += 5;
		}
		if (Weaken != 0f)
		{
			num += 5;
		}
		if (CritDamage != 0f)
		{
			num += 5;
		}
		if (CritNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CritNum);
		}
		if (DizzyNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DizzyNum);
		}
		if (PhysicalInjured != 0f)
		{
			num += 5;
		}
		if (MagicInjured != 0f)
		{
			num += 5;
		}
		if (DotDamage != 0f)
		{
			num += 5;
		}
		if (SputterDamage != 0f)
		{
			num += 5;
		}
		if (PhysicalDamage != 0f)
		{
			num += 5;
		}
		if (MagicDamage != 0f)
		{
			num += 5;
		}
		if (DamageNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DamageNum);
		}
		if (DefenseNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DefenseNum);
		}
		if (DefenseDamage != 0f)
		{
			num += 6;
		}
		num += skillDamageMap_.CalculateSize(_map_skillDamageMap_codec);
		num += skillInjuredMap_.CalculateSize(_map_skillInjuredMap_codec);
		if (DispelNum != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(DispelNum);
		}
		if (DispelDebufNum != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(DispelDebufNum);
		}
		if (RealDamage != 0f)
		{
			num += 6;
		}
		if (ShieldDamage != 0f)
		{
			num += 6;
		}
		if (ImmuneBuffCount != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ImmuneBuffCount);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwBattleUnitStat other)
	{
		if (other != null)
		{
			if (other.Damage != 0f)
			{
				Damage = other.Damage;
			}
			if (other.Injured != 0f)
			{
				Injured = other.Injured;
			}
			if (other.Enhance != 0f)
			{
				Enhance = other.Enhance;
			}
			if (other.Weaken != 0f)
			{
				Weaken = other.Weaken;
			}
			if (other.CritDamage != 0f)
			{
				CritDamage = other.CritDamage;
			}
			if (other.CritNum != 0)
			{
				CritNum = other.CritNum;
			}
			if (other.DizzyNum != 0)
			{
				DizzyNum = other.DizzyNum;
			}
			if (other.PhysicalInjured != 0f)
			{
				PhysicalInjured = other.PhysicalInjured;
			}
			if (other.MagicInjured != 0f)
			{
				MagicInjured = other.MagicInjured;
			}
			if (other.DotDamage != 0f)
			{
				DotDamage = other.DotDamage;
			}
			if (other.SputterDamage != 0f)
			{
				SputterDamage = other.SputterDamage;
			}
			if (other.PhysicalDamage != 0f)
			{
				PhysicalDamage = other.PhysicalDamage;
			}
			if (other.MagicDamage != 0f)
			{
				MagicDamage = other.MagicDamage;
			}
			if (other.DamageNum != 0)
			{
				DamageNum = other.DamageNum;
			}
			if (other.DefenseNum != 0)
			{
				DefenseNum = other.DefenseNum;
			}
			if (other.DefenseDamage != 0f)
			{
				DefenseDamage = other.DefenseDamage;
			}
			skillDamageMap_.Add(other.skillDamageMap_);
			skillInjuredMap_.Add(other.skillInjuredMap_);
			if (other.DispelNum != 0)
			{
				DispelNum = other.DispelNum;
			}
			if (other.DispelDebufNum != 0)
			{
				DispelDebufNum = other.DispelDebufNum;
			}
			if (other.RealDamage != 0f)
			{
				RealDamage = other.RealDamage;
			}
			if (other.ShieldDamage != 0f)
			{
				ShieldDamage = other.ShieldDamage;
			}
			if (other.ImmuneBuffCount != 0)
			{
				ImmuneBuffCount = other.ImmuneBuffCount;
			}
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
			case 13u:
				Damage = input.ReadFloat();
				break;
			case 21u:
				Injured = input.ReadFloat();
				break;
			case 29u:
				Enhance = input.ReadFloat();
				break;
			case 37u:
				Weaken = input.ReadFloat();
				break;
			case 45u:
				CritDamage = input.ReadFloat();
				break;
			case 48u:
				CritNum = input.ReadInt32();
				break;
			case 56u:
				DizzyNum = input.ReadInt32();
				break;
			case 69u:
				PhysicalInjured = input.ReadFloat();
				break;
			case 77u:
				MagicInjured = input.ReadFloat();
				break;
			case 85u:
				DotDamage = input.ReadFloat();
				break;
			case 93u:
				SputterDamage = input.ReadFloat();
				break;
			case 101u:
				PhysicalDamage = input.ReadFloat();
				break;
			case 109u:
				MagicDamage = input.ReadFloat();
				break;
			case 112u:
				DamageNum = input.ReadInt32();
				break;
			case 120u:
				DefenseNum = input.ReadInt32();
				break;
			case 133u:
				DefenseDamage = input.ReadFloat();
				break;
			case 138u:
				skillDamageMap_.AddEntriesFrom(input, _map_skillDamageMap_codec);
				break;
			case 146u:
				skillInjuredMap_.AddEntriesFrom(input, _map_skillInjuredMap_codec);
				break;
			case 152u:
				DispelNum = input.ReadInt32();
				break;
			case 160u:
				DispelDebufNum = input.ReadInt32();
				break;
			case 173u:
				RealDamage = input.ReadFloat();
				break;
			case 181u:
				ShieldDamage = input.ReadFloat();
				break;
			case 184u:
				ImmuneBuffCount = input.ReadInt32();
				break;
			}
		}
	}
}
