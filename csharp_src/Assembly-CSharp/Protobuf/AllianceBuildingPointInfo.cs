using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceBuildingPointInfo : IMessage<AllianceBuildingPointInfo>, IMessage, IEquatable<AllianceBuildingPointInfo>, IDeepCloneable<AllianceBuildingPointInfo>
{
	private static readonly MessageParser<AllianceBuildingPointInfo> _parser = new MessageParser<AllianceBuildingPointInfo>(() => new AllianceBuildingPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int BuildIdFieldNumber = 2;

	private int buildId_;

	public const int LevelFieldNumber = 3;

	private int level_;

	public const int StateFieldNumber = 4;

	private int state_;

	public const int AlAbbrFieldNumber = 5;

	private string alAbbr_ = "";

	public const int AllianceIdFieldNumber = 6;

	private string allianceId_ = "";

	public const int DurabilityFieldNumber = 7;

	private int durability_;

	public const int LastDurabilityTimeFieldNumber = 8;

	private long lastDurabilityTime_;

	public const int DurabilitySpeedFieldNumber = 9;

	private float durabilitySpeed_;

	public const int FireEndTimeFieldNumber = 10;

	private long fireEndTime_;

	public const int ZombieRushInfoFieldNumber = 11;

	private ZombieRushInfo zombieRushInfo_;

	public const int AllianceFurnaceInfoFieldNumber = 12;

	private AllianceFurnaceInfo allianceFurnaceInfo_;

	public const int FightStateFieldNumber = 13;

	private int fightState_;

	public const int PositionIdFieldNumber = 14;

	private int positionId_;

	public const int ShieldSkillInfoFieldNumber = 15;

	private ShieldSkillInfo shieldSkillInfo_;

	[DebuggerNonUserCode]
	public static MessageParser<AllianceBuildingPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[34];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long Uuid
	{
		get
		{
			return uuid_;
		}
		set
		{
			uuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BuildId
	{
		get
		{
			return buildId_;
		}
		set
		{
			buildId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int State
	{
		get
		{
			return state_;
		}
		set
		{
			state_ = value;
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
	public string AllianceId
	{
		get
		{
			return allianceId_;
		}
		set
		{
			allianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Durability
	{
		get
		{
			return durability_;
		}
		set
		{
			durability_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long LastDurabilityTime
	{
		get
		{
			return lastDurabilityTime_;
		}
		set
		{
			lastDurabilityTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float DurabilitySpeed
	{
		get
		{
			return durabilitySpeed_;
		}
		set
		{
			durabilitySpeed_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long FireEndTime
	{
		get
		{
			return fireEndTime_;
		}
		set
		{
			fireEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ZombieRushInfo ZombieRushInfo
	{
		get
		{
			return zombieRushInfo_;
		}
		set
		{
			zombieRushInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceFurnaceInfo AllianceFurnaceInfo
	{
		get
		{
			return allianceFurnaceInfo_;
		}
		set
		{
			allianceFurnaceInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FightState
	{
		get
		{
			return fightState_;
		}
		set
		{
			fightState_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PositionId
	{
		get
		{
			return positionId_;
		}
		set
		{
			positionId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ShieldSkillInfo ShieldSkillInfo
	{
		get
		{
			return shieldSkillInfo_;
		}
		set
		{
			shieldSkillInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceBuildingPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public AllianceBuildingPointInfo(AllianceBuildingPointInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		buildId_ = other.buildId_;
		level_ = other.level_;
		state_ = other.state_;
		alAbbr_ = other.alAbbr_;
		allianceId_ = other.allianceId_;
		durability_ = other.durability_;
		lastDurabilityTime_ = other.lastDurabilityTime_;
		durabilitySpeed_ = other.durabilitySpeed_;
		fireEndTime_ = other.fireEndTime_;
		zombieRushInfo_ = ((other.zombieRushInfo_ != null) ? other.zombieRushInfo_.Clone() : null);
		allianceFurnaceInfo_ = ((other.allianceFurnaceInfo_ != null) ? other.allianceFurnaceInfo_.Clone() : null);
		fightState_ = other.fightState_;
		positionId_ = other.positionId_;
		shieldSkillInfo_ = ((other.shieldSkillInfo_ != null) ? other.shieldSkillInfo_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceBuildingPointInfo Clone()
	{
		return new AllianceBuildingPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceBuildingPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceBuildingPointInfo other)
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
		if (BuildId != other.BuildId)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		if (AlAbbr != other.AlAbbr)
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (Durability != other.Durability)
		{
			return false;
		}
		if (LastDurabilityTime != other.LastDurabilityTime)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(DurabilitySpeed, other.DurabilitySpeed))
		{
			return false;
		}
		if (FireEndTime != other.FireEndTime)
		{
			return false;
		}
		if (!object.Equals(ZombieRushInfo, other.ZombieRushInfo))
		{
			return false;
		}
		if (!object.Equals(AllianceFurnaceInfo, other.AllianceFurnaceInfo))
		{
			return false;
		}
		if (FightState != other.FightState)
		{
			return false;
		}
		if (PositionId != other.PositionId)
		{
			return false;
		}
		if (!object.Equals(ShieldSkillInfo, other.ShieldSkillInfo))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (BuildId != 0)
		{
			num ^= BuildId.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (AlAbbr.Length != 0)
		{
			num ^= AlAbbr.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (Durability != 0)
		{
			num ^= Durability.GetHashCode();
		}
		if (LastDurabilityTime != 0L)
		{
			num ^= LastDurabilityTime.GetHashCode();
		}
		if (DurabilitySpeed != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(DurabilitySpeed);
		}
		if (FireEndTime != 0L)
		{
			num ^= FireEndTime.GetHashCode();
		}
		if (zombieRushInfo_ != null)
		{
			num ^= ZombieRushInfo.GetHashCode();
		}
		if (allianceFurnaceInfo_ != null)
		{
			num ^= AllianceFurnaceInfo.GetHashCode();
		}
		if (FightState != 0)
		{
			num ^= FightState.GetHashCode();
		}
		if (PositionId != 0)
		{
			num ^= PositionId.GetHashCode();
		}
		if (shieldSkillInfo_ != null)
		{
			num ^= ShieldSkillInfo.GetHashCode();
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
		if (Uuid != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(Uuid);
		}
		if (BuildId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(BuildId);
		}
		if (Level != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Level);
		}
		if (State != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(State);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(AllianceId);
		}
		if (Durability != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Durability);
		}
		if (LastDurabilityTime != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(LastDurabilityTime);
		}
		if (DurabilitySpeed != 0f)
		{
			output.WriteRawTag(77);
			output.WriteFloat(DurabilitySpeed);
		}
		if (FireEndTime != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(FireEndTime);
		}
		if (zombieRushInfo_ != null)
		{
			output.WriteRawTag(90);
			output.WriteMessage(ZombieRushInfo);
		}
		if (allianceFurnaceInfo_ != null)
		{
			output.WriteRawTag(98);
			output.WriteMessage(AllianceFurnaceInfo);
		}
		if (FightState != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(FightState);
		}
		if (PositionId != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(PositionId);
		}
		if (shieldSkillInfo_ != null)
		{
			output.WriteRawTag(122);
			output.WriteMessage(ShieldSkillInfo);
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
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (BuildId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildId);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (AlAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (Durability != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Durability);
		}
		if (LastDurabilityTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(LastDurabilityTime);
		}
		if (DurabilitySpeed != 0f)
		{
			num += 5;
		}
		if (FireEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FireEndTime);
		}
		if (zombieRushInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ZombieRushInfo);
		}
		if (allianceFurnaceInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(AllianceFurnaceInfo);
		}
		if (FightState != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FightState);
		}
		if (PositionId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PositionId);
		}
		if (shieldSkillInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ShieldSkillInfo);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceBuildingPointInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Uuid != 0L)
		{
			Uuid = other.Uuid;
		}
		if (other.BuildId != 0)
		{
			BuildId = other.BuildId;
		}
		if (other.Level != 0)
		{
			Level = other.Level;
		}
		if (other.State != 0)
		{
			State = other.State;
		}
		if (other.AlAbbr.Length != 0)
		{
			AlAbbr = other.AlAbbr;
		}
		if (other.AllianceId.Length != 0)
		{
			AllianceId = other.AllianceId;
		}
		if (other.Durability != 0)
		{
			Durability = other.Durability;
		}
		if (other.LastDurabilityTime != 0L)
		{
			LastDurabilityTime = other.LastDurabilityTime;
		}
		if (other.DurabilitySpeed != 0f)
		{
			DurabilitySpeed = other.DurabilitySpeed;
		}
		if (other.FireEndTime != 0L)
		{
			FireEndTime = other.FireEndTime;
		}
		if (other.zombieRushInfo_ != null)
		{
			if (zombieRushInfo_ == null)
			{
				ZombieRushInfo = new ZombieRushInfo();
			}
			ZombieRushInfo.MergeFrom(other.ZombieRushInfo);
		}
		if (other.allianceFurnaceInfo_ != null)
		{
			if (allianceFurnaceInfo_ == null)
			{
				AllianceFurnaceInfo = new AllianceFurnaceInfo();
			}
			AllianceFurnaceInfo.MergeFrom(other.AllianceFurnaceInfo);
		}
		if (other.FightState != 0)
		{
			FightState = other.FightState;
		}
		if (other.PositionId != 0)
		{
			PositionId = other.PositionId;
		}
		if (other.shieldSkillInfo_ != null)
		{
			if (shieldSkillInfo_ == null)
			{
				ShieldSkillInfo = new ShieldSkillInfo();
			}
			ShieldSkillInfo.MergeFrom(other.ShieldSkillInfo);
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
			case 8u:
				Uuid = input.ReadInt64();
				break;
			case 16u:
				BuildId = input.ReadInt32();
				break;
			case 24u:
				Level = input.ReadInt32();
				break;
			case 32u:
				State = input.ReadInt32();
				break;
			case 42u:
				AlAbbr = input.ReadString();
				break;
			case 50u:
				AllianceId = input.ReadString();
				break;
			case 56u:
				Durability = input.ReadInt32();
				break;
			case 64u:
				LastDurabilityTime = input.ReadInt64();
				break;
			case 77u:
				DurabilitySpeed = input.ReadFloat();
				break;
			case 80u:
				FireEndTime = input.ReadInt64();
				break;
			case 90u:
				if (zombieRushInfo_ == null)
				{
					ZombieRushInfo = new ZombieRushInfo();
				}
				input.ReadMessage(ZombieRushInfo);
				break;
			case 98u:
				if (allianceFurnaceInfo_ == null)
				{
					AllianceFurnaceInfo = new AllianceFurnaceInfo();
				}
				input.ReadMessage(AllianceFurnaceInfo);
				break;
			case 104u:
				FightState = input.ReadInt32();
				break;
			case 112u:
				PositionId = input.ReadInt32();
				break;
			case 122u:
				if (shieldSkillInfo_ == null)
				{
					ShieldSkillInfo = new ShieldSkillInfo();
				}
				input.ReadMessage(ShieldSkillInfo);
				break;
			}
		}
	}
}
