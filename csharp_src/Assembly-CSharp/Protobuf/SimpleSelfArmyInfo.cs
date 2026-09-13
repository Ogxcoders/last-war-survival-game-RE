using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class SimpleSelfArmyInfo : IMessage<SimpleSelfArmyInfo>, IMessage, IEquatable<SimpleSelfArmyInfo>, IDeepCloneable<SimpleSelfArmyInfo>
{
	private static readonly MessageParser<SimpleSelfArmyInfo> _parser = new MessageParser<SimpleSelfArmyInfo>(() => new SimpleSelfArmyInfo());

	private UnknownFieldSet _unknownFields;

	public const int ArmyInfoFieldNumber = 1;

	private SimpleCombatUnitPushObj armyInfo_;

	public const int TargetInfoFieldNumber = 2;

	private SimpleCombatUnitPushObj targetInfo_;

	public const int HurtFieldNumber = 3;

	private int hurt_;

	public const int HealFieldNumber = 4;

	private int heal_;

	public const int ShieldHurtFieldNumber = 5;

	private int shieldHurt_;

	public const int ShieldFieldNumber = 6;

	private int shield_;

	public const int AngerFieldNumber = 7;

	private int anger_;

	public const int StatusInfoFieldNumber = 8;

	private static readonly FieldCodec<RoundStatusInfo> _repeated_statusInfo_codec = FieldCodec.ForMessage(66u, RoundStatusInfo.Parser);

	private readonly RepeatedField<RoundStatusInfo> statusInfo_ = new RepeatedField<RoundStatusInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<SimpleSelfArmyInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleRoundPushReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public SimpleCombatUnitPushObj ArmyInfo
	{
		get
		{
			return armyInfo_;
		}
		set
		{
			armyInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public SimpleCombatUnitPushObj TargetInfo
	{
		get
		{
			return targetInfo_;
		}
		set
		{
			targetInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Hurt
	{
		get
		{
			return hurt_;
		}
		set
		{
			hurt_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Heal
	{
		get
		{
			return heal_;
		}
		set
		{
			heal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ShieldHurt
	{
		get
		{
			return shieldHurt_;
		}
		set
		{
			shieldHurt_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Shield
	{
		get
		{
			return shield_;
		}
		set
		{
			shield_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Anger
	{
		get
		{
			return anger_;
		}
		set
		{
			anger_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<RoundStatusInfo> StatusInfo => statusInfo_;

	[DebuggerNonUserCode]
	public SimpleSelfArmyInfo()
	{
	}

	[DebuggerNonUserCode]
	public SimpleSelfArmyInfo(SimpleSelfArmyInfo other)
		: this()
	{
		armyInfo_ = ((other.armyInfo_ != null) ? other.armyInfo_.Clone() : null);
		targetInfo_ = ((other.targetInfo_ != null) ? other.targetInfo_.Clone() : null);
		hurt_ = other.hurt_;
		heal_ = other.heal_;
		shieldHurt_ = other.shieldHurt_;
		shield_ = other.shield_;
		anger_ = other.anger_;
		statusInfo_ = other.statusInfo_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SimpleSelfArmyInfo Clone()
	{
		return new SimpleSelfArmyInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SimpleSelfArmyInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(SimpleSelfArmyInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(ArmyInfo, other.ArmyInfo))
		{
			return false;
		}
		if (!object.Equals(TargetInfo, other.TargetInfo))
		{
			return false;
		}
		if (Hurt != other.Hurt)
		{
			return false;
		}
		if (Heal != other.Heal)
		{
			return false;
		}
		if (ShieldHurt != other.ShieldHurt)
		{
			return false;
		}
		if (Shield != other.Shield)
		{
			return false;
		}
		if (Anger != other.Anger)
		{
			return false;
		}
		if (!statusInfo_.Equals(other.statusInfo_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (armyInfo_ != null)
		{
			num ^= ArmyInfo.GetHashCode();
		}
		if (targetInfo_ != null)
		{
			num ^= TargetInfo.GetHashCode();
		}
		if (Hurt != 0)
		{
			num ^= Hurt.GetHashCode();
		}
		if (Heal != 0)
		{
			num ^= Heal.GetHashCode();
		}
		if (ShieldHurt != 0)
		{
			num ^= ShieldHurt.GetHashCode();
		}
		if (Shield != 0)
		{
			num ^= Shield.GetHashCode();
		}
		if (Anger != 0)
		{
			num ^= Anger.GetHashCode();
		}
		num ^= statusInfo_.GetHashCode();
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
		if (armyInfo_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(ArmyInfo);
		}
		if (targetInfo_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(TargetInfo);
		}
		if (Hurt != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Hurt);
		}
		if (Heal != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Heal);
		}
		if (ShieldHurt != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(ShieldHurt);
		}
		if (Shield != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(Shield);
		}
		if (Anger != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Anger);
		}
		statusInfo_.WriteTo(output, _repeated_statusInfo_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (armyInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ArmyInfo);
		}
		if (targetInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(TargetInfo);
		}
		if (Hurt != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Hurt);
		}
		if (Heal != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Heal);
		}
		if (ShieldHurt != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ShieldHurt);
		}
		if (Shield != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Shield);
		}
		if (Anger != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Anger);
		}
		num += statusInfo_.CalculateSize(_repeated_statusInfo_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SimpleSelfArmyInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.armyInfo_ != null)
		{
			if (armyInfo_ == null)
			{
				ArmyInfo = new SimpleCombatUnitPushObj();
			}
			ArmyInfo.MergeFrom(other.ArmyInfo);
		}
		if (other.targetInfo_ != null)
		{
			if (targetInfo_ == null)
			{
				TargetInfo = new SimpleCombatUnitPushObj();
			}
			TargetInfo.MergeFrom(other.TargetInfo);
		}
		if (other.Hurt != 0)
		{
			Hurt = other.Hurt;
		}
		if (other.Heal != 0)
		{
			Heal = other.Heal;
		}
		if (other.ShieldHurt != 0)
		{
			ShieldHurt = other.ShieldHurt;
		}
		if (other.Shield != 0)
		{
			Shield = other.Shield;
		}
		if (other.Anger != 0)
		{
			Anger = other.Anger;
		}
		statusInfo_.Add(other.statusInfo_);
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
				if (armyInfo_ == null)
				{
					ArmyInfo = new SimpleCombatUnitPushObj();
				}
				input.ReadMessage(ArmyInfo);
				break;
			case 18u:
				if (targetInfo_ == null)
				{
					TargetInfo = new SimpleCombatUnitPushObj();
				}
				input.ReadMessage(TargetInfo);
				break;
			case 24u:
				Hurt = input.ReadInt32();
				break;
			case 32u:
				Heal = input.ReadInt32();
				break;
			case 40u:
				ShieldHurt = input.ReadInt32();
				break;
			case 48u:
				Shield = input.ReadInt32();
				break;
			case 56u:
				Anger = input.ReadInt32();
				break;
			case 66u:
				statusInfo_.AddEntriesFrom(input, _repeated_statusInfo_codec);
				break;
			}
		}
	}
}
