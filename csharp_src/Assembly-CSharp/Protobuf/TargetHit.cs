using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class TargetHit : IMessage<TargetHit>, IMessage, IEquatable<TargetHit>, IDeepCloneable<TargetHit>
{
	private static readonly MessageParser<TargetHit> _parser = new MessageParser<TargetHit>(() => new TargetHit());

	private UnknownFieldSet _unknownFields;

	public const int IndexFieldNumber = 1;

	private int index_;

	public const int MissFieldNumber = 2;

	private int miss_;

	public const int CritFieldNumber = 3;

	private int crit_;

	public const int DamageFieldNumber = 4;

	private int damage_;

	public const int EffectChangesFieldNumber = 5;

	private static readonly FieldCodec<BattleEffectInfo> _repeated_effectChanges_codec = FieldCodec.ForMessage(42u, BattleEffectInfo.Parser);

	private readonly RepeatedField<BattleEffectInfo> effectChanges_ = new RepeatedField<BattleEffectInfo>();

	public const int BuffChangesFieldNumber = 6;

	private static readonly FieldCodec<UnitBuffChange> _repeated_buffChanges_codec = FieldCodec.ForMessage(50u, UnitBuffChange.Parser);

	private readonly RepeatedField<UnitBuffChange> buffChanges_ = new RepeatedField<UnitBuffChange>();

	public const int DamageDoubleFieldNumber = 7;

	private double damageDouble_;

	public const int ShieldChangeFieldNumber = 8;

	private float shieldChange_;

	public const int ShieldTotalFieldNumber = 9;

	private float shieldTotal_;

	public const int DotDamageOnceFieldNumber = 10;

	private int dotDamageOnce_;

	public const int DotDamageDetailFieldNumber = 11;

	private static readonly FieldCodec<DotDamageDetail> _repeated_dotDamageDetail_codec = FieldCodec.ForMessage(90u, Protobuf.DotDamageDetail.Parser);

	private readonly RepeatedField<DotDamageDetail> dotDamageDetail_ = new RepeatedField<DotDamageDetail>();

	public const int DefenseFieldNumber = 12;

	private bool defense_;

	public const int AttackBackFieldNumber = 13;

	private bool attackBack_;

	public const int DebugInfoFieldNumber = 15;

	private string debugInfo_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<TargetHit> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[4];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public int Miss
	{
		get
		{
			return miss_;
		}
		set
		{
			miss_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Crit
	{
		get
		{
			return crit_;
		}
		set
		{
			crit_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Damage
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
	public RepeatedField<BattleEffectInfo> EffectChanges => effectChanges_;

	[DebuggerNonUserCode]
	public RepeatedField<UnitBuffChange> BuffChanges => buffChanges_;

	[DebuggerNonUserCode]
	public double DamageDouble
	{
		get
		{
			return damageDouble_;
		}
		set
		{
			damageDouble_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float ShieldChange
	{
		get
		{
			return shieldChange_;
		}
		set
		{
			shieldChange_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float ShieldTotal
	{
		get
		{
			return shieldTotal_;
		}
		set
		{
			shieldTotal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DotDamageOnce
	{
		get
		{
			return dotDamageOnce_;
		}
		set
		{
			dotDamageOnce_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<DotDamageDetail> DotDamageDetail => dotDamageDetail_;

	[DebuggerNonUserCode]
	public bool Defense
	{
		get
		{
			return defense_;
		}
		set
		{
			defense_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool AttackBack
	{
		get
		{
			return attackBack_;
		}
		set
		{
			attackBack_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string DebugInfo
	{
		get
		{
			return debugInfo_;
		}
		set
		{
			debugInfo_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public TargetHit()
	{
	}

	[DebuggerNonUserCode]
	public TargetHit(TargetHit other)
		: this()
	{
		index_ = other.index_;
		miss_ = other.miss_;
		crit_ = other.crit_;
		damage_ = other.damage_;
		effectChanges_ = other.effectChanges_.Clone();
		buffChanges_ = other.buffChanges_.Clone();
		damageDouble_ = other.damageDouble_;
		shieldChange_ = other.shieldChange_;
		shieldTotal_ = other.shieldTotal_;
		dotDamageOnce_ = other.dotDamageOnce_;
		dotDamageDetail_ = other.dotDamageDetail_.Clone();
		defense_ = other.defense_;
		attackBack_ = other.attackBack_;
		debugInfo_ = other.debugInfo_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TargetHit Clone()
	{
		return new TargetHit(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TargetHit);
	}

	[DebuggerNonUserCode]
	public bool Equals(TargetHit other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Index != other.Index)
		{
			return false;
		}
		if (Miss != other.Miss)
		{
			return false;
		}
		if (Crit != other.Crit)
		{
			return false;
		}
		if (Damage != other.Damage)
		{
			return false;
		}
		if (!effectChanges_.Equals(other.effectChanges_))
		{
			return false;
		}
		if (!buffChanges_.Equals(other.buffChanges_))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(DamageDouble, other.DamageDouble))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(ShieldChange, other.ShieldChange))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(ShieldTotal, other.ShieldTotal))
		{
			return false;
		}
		if (DotDamageOnce != other.DotDamageOnce)
		{
			return false;
		}
		if (!dotDamageDetail_.Equals(other.dotDamageDetail_))
		{
			return false;
		}
		if (Defense != other.Defense)
		{
			return false;
		}
		if (AttackBack != other.AttackBack)
		{
			return false;
		}
		if (DebugInfo != other.DebugInfo)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Index != 0)
		{
			num ^= Index.GetHashCode();
		}
		if (Miss != 0)
		{
			num ^= Miss.GetHashCode();
		}
		if (Crit != 0)
		{
			num ^= Crit.GetHashCode();
		}
		if (Damage != 0)
		{
			num ^= Damage.GetHashCode();
		}
		num ^= effectChanges_.GetHashCode();
		num ^= buffChanges_.GetHashCode();
		if (DamageDouble != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(DamageDouble);
		}
		if (ShieldChange != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(ShieldChange);
		}
		if (ShieldTotal != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(ShieldTotal);
		}
		if (DotDamageOnce != 0)
		{
			num ^= DotDamageOnce.GetHashCode();
		}
		num ^= dotDamageDetail_.GetHashCode();
		if (Defense)
		{
			num ^= Defense.GetHashCode();
		}
		if (AttackBack)
		{
			num ^= AttackBack.GetHashCode();
		}
		if (DebugInfo.Length != 0)
		{
			num ^= DebugInfo.GetHashCode();
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
		if (Index != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Index);
		}
		if (Miss != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Miss);
		}
		if (Crit != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Crit);
		}
		if (Damage != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Damage);
		}
		effectChanges_.WriteTo(output, _repeated_effectChanges_codec);
		buffChanges_.WriteTo(output, _repeated_buffChanges_codec);
		if (DamageDouble != 0.0)
		{
			output.WriteRawTag(57);
			output.WriteDouble(DamageDouble);
		}
		if (ShieldChange != 0f)
		{
			output.WriteRawTag(69);
			output.WriteFloat(ShieldChange);
		}
		if (ShieldTotal != 0f)
		{
			output.WriteRawTag(77);
			output.WriteFloat(ShieldTotal);
		}
		if (DotDamageOnce != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(DotDamageOnce);
		}
		dotDamageDetail_.WriteTo(output, _repeated_dotDamageDetail_codec);
		if (Defense)
		{
			output.WriteRawTag(96);
			output.WriteBool(Defense);
		}
		if (AttackBack)
		{
			output.WriteRawTag(104);
			output.WriteBool(AttackBack);
		}
		if (DebugInfo.Length != 0)
		{
			output.WriteRawTag(122);
			output.WriteString(DebugInfo);
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
		if (Index != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Index);
		}
		if (Miss != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Miss);
		}
		if (Crit != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Crit);
		}
		if (Damage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Damage);
		}
		num += effectChanges_.CalculateSize(_repeated_effectChanges_codec);
		num += buffChanges_.CalculateSize(_repeated_buffChanges_codec);
		if (DamageDouble != 0.0)
		{
			num += 9;
		}
		if (ShieldChange != 0f)
		{
			num += 5;
		}
		if (ShieldTotal != 0f)
		{
			num += 5;
		}
		if (DotDamageOnce != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DotDamageOnce);
		}
		num += dotDamageDetail_.CalculateSize(_repeated_dotDamageDetail_codec);
		if (Defense)
		{
			num += 2;
		}
		if (AttackBack)
		{
			num += 2;
		}
		if (DebugInfo.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(DebugInfo);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TargetHit other)
	{
		if (other != null)
		{
			if (other.Index != 0)
			{
				Index = other.Index;
			}
			if (other.Miss != 0)
			{
				Miss = other.Miss;
			}
			if (other.Crit != 0)
			{
				Crit = other.Crit;
			}
			if (other.Damage != 0)
			{
				Damage = other.Damage;
			}
			effectChanges_.Add(other.effectChanges_);
			buffChanges_.Add(other.buffChanges_);
			if (other.DamageDouble != 0.0)
			{
				DamageDouble = other.DamageDouble;
			}
			if (other.ShieldChange != 0f)
			{
				ShieldChange = other.ShieldChange;
			}
			if (other.ShieldTotal != 0f)
			{
				ShieldTotal = other.ShieldTotal;
			}
			if (other.DotDamageOnce != 0)
			{
				DotDamageOnce = other.DotDamageOnce;
			}
			dotDamageDetail_.Add(other.dotDamageDetail_);
			if (other.Defense)
			{
				Defense = other.Defense;
			}
			if (other.AttackBack)
			{
				AttackBack = other.AttackBack;
			}
			if (other.DebugInfo.Length != 0)
			{
				DebugInfo = other.DebugInfo;
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
			case 8u:
				Index = input.ReadInt32();
				break;
			case 16u:
				Miss = input.ReadInt32();
				break;
			case 24u:
				Crit = input.ReadInt32();
				break;
			case 32u:
				Damage = input.ReadInt32();
				break;
			case 42u:
				effectChanges_.AddEntriesFrom(input, _repeated_effectChanges_codec);
				break;
			case 50u:
				buffChanges_.AddEntriesFrom(input, _repeated_buffChanges_codec);
				break;
			case 57u:
				DamageDouble = input.ReadDouble();
				break;
			case 69u:
				ShieldChange = input.ReadFloat();
				break;
			case 77u:
				ShieldTotal = input.ReadFloat();
				break;
			case 80u:
				DotDamageOnce = input.ReadInt32();
				break;
			case 90u:
				dotDamageDetail_.AddEntriesFrom(input, _repeated_dotDamageDetail_codec);
				break;
			case 96u:
				Defense = input.ReadBool();
				break;
			case 104u:
				AttackBack = input.ReadBool();
				break;
			case 122u:
				DebugInfo = input.ReadString();
				break;
			}
		}
	}
}
