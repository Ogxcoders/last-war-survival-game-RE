using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ThermalConductor : IMessage<ThermalConductor>, IMessage, IEquatable<ThermalConductor>, IDeepCloneable<ThermalConductor>
{
	private static readonly MessageParser<ThermalConductor> _parser = new MessageParser<ThermalConductor>(() => new ThermalConductor());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private string uuid_ = "";

	public const int OwnerIdFieldNumber = 2;

	private string ownerId_ = "";

	public const int PointFieldNumber = 3;

	private int point_;

	public const int TypeFieldNumber = 4;

	private int type_;

	public const int StartTimeFieldNumber = 5;

	private long startTime_;

	public const int CurFieldNumber = 6;

	private float cur_;

	public const int TargetFieldNumber = 7;

	private float target_;

	public const int EndTimeFieldNumber = 8;

	private long endTime_;

	public const int PhaseFieldNumber = 9;

	private int phase_;

	public const int NextPhaseFieldNumber = 10;

	private int nextPhase_;

	public const int NextPhaseEndTimeFieldNumber = 11;

	private long nextPhaseEndTime_;

	public const int ChangePhaseDurationFieldNumber = 12;

	private long changePhaseDuration_;

	public const int SpeedFieldNumber = 13;

	private float speed_;

	public const int HpFieldNumber = 14;

	private int hp_;

	public const int MaxHpFieldNumber = 15;

	private int maxHp_;

	public const int PhaseReasonFieldNumber = 16;

	private int phaseReason_;

	public const int NextPhaseReasonFieldNumber = 17;

	private int nextPhaseReason_;

	[DebuggerNonUserCode]
	public static MessageParser<ThermalConductor> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[56];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Uuid
	{
		get
		{
			return uuid_;
		}
		set
		{
			uuid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string OwnerId
	{
		get
		{
			return ownerId_;
		}
		set
		{
			ownerId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Point
	{
		get
		{
			return point_;
		}
		set
		{
			point_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StartTime
	{
		get
		{
			return startTime_;
		}
		set
		{
			startTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float Cur
	{
		get
		{
			return cur_;
		}
		set
		{
			cur_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float Target
	{
		get
		{
			return target_;
		}
		set
		{
			target_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long EndTime
	{
		get
		{
			return endTime_;
		}
		set
		{
			endTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Phase
	{
		get
		{
			return phase_;
		}
		set
		{
			phase_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int NextPhase
	{
		get
		{
			return nextPhase_;
		}
		set
		{
			nextPhase_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long NextPhaseEndTime
	{
		get
		{
			return nextPhaseEndTime_;
		}
		set
		{
			nextPhaseEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ChangePhaseDuration
	{
		get
		{
			return changePhaseDuration_;
		}
		set
		{
			changePhaseDuration_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float Speed
	{
		get
		{
			return speed_;
		}
		set
		{
			speed_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Hp
	{
		get
		{
			return hp_;
		}
		set
		{
			hp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxHp
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
	public int PhaseReason
	{
		get
		{
			return phaseReason_;
		}
		set
		{
			phaseReason_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int NextPhaseReason
	{
		get
		{
			return nextPhaseReason_;
		}
		set
		{
			nextPhaseReason_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ThermalConductor()
	{
	}

	[DebuggerNonUserCode]
	public ThermalConductor(ThermalConductor other)
		: this()
	{
		uuid_ = other.uuid_;
		ownerId_ = other.ownerId_;
		point_ = other.point_;
		type_ = other.type_;
		startTime_ = other.startTime_;
		cur_ = other.cur_;
		target_ = other.target_;
		endTime_ = other.endTime_;
		phase_ = other.phase_;
		nextPhase_ = other.nextPhase_;
		nextPhaseEndTime_ = other.nextPhaseEndTime_;
		changePhaseDuration_ = other.changePhaseDuration_;
		speed_ = other.speed_;
		hp_ = other.hp_;
		maxHp_ = other.maxHp_;
		phaseReason_ = other.phaseReason_;
		nextPhaseReason_ = other.nextPhaseReason_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ThermalConductor Clone()
	{
		return new ThermalConductor(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ThermalConductor);
	}

	[DebuggerNonUserCode]
	public bool Equals(ThermalConductor other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (OwnerId != other.OwnerId)
		{
			return false;
		}
		if (Point != other.Point)
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Cur, other.Cur))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Target, other.Target))
		{
			return false;
		}
		if (EndTime != other.EndTime)
		{
			return false;
		}
		if (Phase != other.Phase)
		{
			return false;
		}
		if (NextPhase != other.NextPhase)
		{
			return false;
		}
		if (NextPhaseEndTime != other.NextPhaseEndTime)
		{
			return false;
		}
		if (ChangePhaseDuration != other.ChangePhaseDuration)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Speed, other.Speed))
		{
			return false;
		}
		if (Hp != other.Hp)
		{
			return false;
		}
		if (MaxHp != other.MaxHp)
		{
			return false;
		}
		if (PhaseReason != other.PhaseReason)
		{
			return false;
		}
		if (NextPhaseReason != other.NextPhaseReason)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uuid.Length != 0)
		{
			num ^= Uuid.GetHashCode();
		}
		if (OwnerId.Length != 0)
		{
			num ^= OwnerId.GetHashCode();
		}
		if (Point != 0)
		{
			num ^= Point.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (Cur != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Cur);
		}
		if (Target != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Target);
		}
		if (EndTime != 0L)
		{
			num ^= EndTime.GetHashCode();
		}
		if (Phase != 0)
		{
			num ^= Phase.GetHashCode();
		}
		if (NextPhase != 0)
		{
			num ^= NextPhase.GetHashCode();
		}
		if (NextPhaseEndTime != 0L)
		{
			num ^= NextPhaseEndTime.GetHashCode();
		}
		if (ChangePhaseDuration != 0L)
		{
			num ^= ChangePhaseDuration.GetHashCode();
		}
		if (Speed != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Speed);
		}
		if (Hp != 0)
		{
			num ^= Hp.GetHashCode();
		}
		if (MaxHp != 0)
		{
			num ^= MaxHp.GetHashCode();
		}
		if (PhaseReason != 0)
		{
			num ^= PhaseReason.GetHashCode();
		}
		if (NextPhaseReason != 0)
		{
			num ^= NextPhaseReason.GetHashCode();
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
		if (Uuid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Uuid);
		}
		if (OwnerId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(OwnerId);
		}
		if (Point != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Point);
		}
		if (Type != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Type);
		}
		if (StartTime != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(StartTime);
		}
		if (Cur != 0f)
		{
			output.WriteRawTag(53);
			output.WriteFloat(Cur);
		}
		if (Target != 0f)
		{
			output.WriteRawTag(61);
			output.WriteFloat(Target);
		}
		if (EndTime != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(EndTime);
		}
		if (Phase != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Phase);
		}
		if (NextPhase != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(NextPhase);
		}
		if (NextPhaseEndTime != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(NextPhaseEndTime);
		}
		if (ChangePhaseDuration != 0L)
		{
			output.WriteRawTag(96);
			output.WriteInt64(ChangePhaseDuration);
		}
		if (Speed != 0f)
		{
			output.WriteRawTag(109);
			output.WriteFloat(Speed);
		}
		if (Hp != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(Hp);
		}
		if (MaxHp != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(MaxHp);
		}
		if (PhaseReason != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(PhaseReason);
		}
		if (NextPhaseReason != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(NextPhaseReason);
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
		if (Uuid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uuid);
		}
		if (OwnerId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(OwnerId);
		}
		if (Point != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Point);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (Cur != 0f)
		{
			num += 5;
		}
		if (Target != 0f)
		{
			num += 5;
		}
		if (EndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(EndTime);
		}
		if (Phase != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Phase);
		}
		if (NextPhase != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NextPhase);
		}
		if (NextPhaseEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(NextPhaseEndTime);
		}
		if (ChangePhaseDuration != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ChangePhaseDuration);
		}
		if (Speed != 0f)
		{
			num += 5;
		}
		if (Hp != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Hp);
		}
		if (MaxHp != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxHp);
		}
		if (PhaseReason != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(PhaseReason);
		}
		if (NextPhaseReason != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(NextPhaseReason);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ThermalConductor other)
	{
		if (other != null)
		{
			if (other.Uuid.Length != 0)
			{
				Uuid = other.Uuid;
			}
			if (other.OwnerId.Length != 0)
			{
				OwnerId = other.OwnerId;
			}
			if (other.Point != 0)
			{
				Point = other.Point;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.StartTime != 0L)
			{
				StartTime = other.StartTime;
			}
			if (other.Cur != 0f)
			{
				Cur = other.Cur;
			}
			if (other.Target != 0f)
			{
				Target = other.Target;
			}
			if (other.EndTime != 0L)
			{
				EndTime = other.EndTime;
			}
			if (other.Phase != 0)
			{
				Phase = other.Phase;
			}
			if (other.NextPhase != 0)
			{
				NextPhase = other.NextPhase;
			}
			if (other.NextPhaseEndTime != 0L)
			{
				NextPhaseEndTime = other.NextPhaseEndTime;
			}
			if (other.ChangePhaseDuration != 0L)
			{
				ChangePhaseDuration = other.ChangePhaseDuration;
			}
			if (other.Speed != 0f)
			{
				Speed = other.Speed;
			}
			if (other.Hp != 0)
			{
				Hp = other.Hp;
			}
			if (other.MaxHp != 0)
			{
				MaxHp = other.MaxHp;
			}
			if (other.PhaseReason != 0)
			{
				PhaseReason = other.PhaseReason;
			}
			if (other.NextPhaseReason != 0)
			{
				NextPhaseReason = other.NextPhaseReason;
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
			case 10u:
				Uuid = input.ReadString();
				break;
			case 18u:
				OwnerId = input.ReadString();
				break;
			case 24u:
				Point = input.ReadInt32();
				break;
			case 32u:
				Type = input.ReadInt32();
				break;
			case 40u:
				StartTime = input.ReadInt64();
				break;
			case 53u:
				Cur = input.ReadFloat();
				break;
			case 61u:
				Target = input.ReadFloat();
				break;
			case 64u:
				EndTime = input.ReadInt64();
				break;
			case 72u:
				Phase = input.ReadInt32();
				break;
			case 80u:
				NextPhase = input.ReadInt32();
				break;
			case 88u:
				NextPhaseEndTime = input.ReadInt64();
				break;
			case 96u:
				ChangePhaseDuration = input.ReadInt64();
				break;
			case 109u:
				Speed = input.ReadFloat();
				break;
			case 112u:
				Hp = input.ReadInt32();
				break;
			case 120u:
				MaxHp = input.ReadInt32();
				break;
			case 128u:
				PhaseReason = input.ReadInt32();
				break;
			case 136u:
				NextPhaseReason = input.ReadInt32();
				break;
			}
		}
	}
}
