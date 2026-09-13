using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class OutpostInfo : IMessage<OutpostInfo>, IMessage, IEquatable<OutpostInfo>, IDeepCloneable<OutpostInfo>
{
	private static readonly MessageParser<OutpostInfo> _parser = new MessageParser<OutpostInfo>(() => new OutpostInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int CityIdFieldNumber = 2;

	private int cityId_;

	public const int PointIdFieldNumber = 3;

	private int pointId_;

	public const int WorldIdFieldNumber = 4;

	private int worldId_;

	public const int StateFieldNumber = 5;

	private int state_;

	public const int OwnerAllianceIdFieldNumber = 6;

	private string ownerAllianceId_ = "";

	public const int OwnerServerIdFieldNumber = 7;

	private int ownerServerId_;

	public const int TmpOwnerServerIdFieldNumber = 8;

	private int tmpOwnerServerId_;

	public const int ProtectTimeFieldNumber = 9;

	private long protectTime_;

	public const int BattleStartTimeFieldNumber = 10;

	private long battleStartTime_;

	public const int FirstOccupyTimeFieldNumber = 11;

	private long firstOccupyTime_;

	public const int FirstOccupyAllianceIdFieldNumber = 12;

	private string firstOccupyAllianceId_ = "";

	public const int FirstOccupyAllianceNameFieldNumber = 13;

	private string firstOccupyAllianceName_ = "";

	public const int FirstOccupyAllianceAbbrFieldNumber = 14;

	private string firstOccupyAllianceAbbr_ = "";

	public const int FirstOccupyServerIdFieldNumber = 15;

	private int firstOccupyServerId_;

	public const int LastRepairTimeFieldNumber = 16;

	private long lastRepairTime_;

	public const int RepairScoreFieldNumber = 17;

	private int repairScore_;

	public const int ConnectSwitchFieldNumber = 18;

	private int connectSwitch_;

	[DebuggerNonUserCode]
	public static MessageParser<OutpostInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[65];

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
	public int CityId
	{
		get
		{
			return cityId_;
		}
		set
		{
			cityId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PointId
	{
		get
		{
			return pointId_;
		}
		set
		{
			pointId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WorldId
	{
		get
		{
			return worldId_;
		}
		set
		{
			worldId_ = value;
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
	public string OwnerAllianceId
	{
		get
		{
			return ownerAllianceId_;
		}
		set
		{
			ownerAllianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int OwnerServerId
	{
		get
		{
			return ownerServerId_;
		}
		set
		{
			ownerServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TmpOwnerServerId
	{
		get
		{
			return tmpOwnerServerId_;
		}
		set
		{
			tmpOwnerServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ProtectTime
	{
		get
		{
			return protectTime_;
		}
		set
		{
			protectTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long BattleStartTime
	{
		get
		{
			return battleStartTime_;
		}
		set
		{
			battleStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long FirstOccupyTime
	{
		get
		{
			return firstOccupyTime_;
		}
		set
		{
			firstOccupyTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string FirstOccupyAllianceId
	{
		get
		{
			return firstOccupyAllianceId_;
		}
		set
		{
			firstOccupyAllianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string FirstOccupyAllianceName
	{
		get
		{
			return firstOccupyAllianceName_;
		}
		set
		{
			firstOccupyAllianceName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string FirstOccupyAllianceAbbr
	{
		get
		{
			return firstOccupyAllianceAbbr_;
		}
		set
		{
			firstOccupyAllianceAbbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int FirstOccupyServerId
	{
		get
		{
			return firstOccupyServerId_;
		}
		set
		{
			firstOccupyServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long LastRepairTime
	{
		get
		{
			return lastRepairTime_;
		}
		set
		{
			lastRepairTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RepairScore
	{
		get
		{
			return repairScore_;
		}
		set
		{
			repairScore_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ConnectSwitch
	{
		get
		{
			return connectSwitch_;
		}
		set
		{
			connectSwitch_ = value;
		}
	}

	[DebuggerNonUserCode]
	public OutpostInfo()
	{
	}

	[DebuggerNonUserCode]
	public OutpostInfo(OutpostInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		cityId_ = other.cityId_;
		pointId_ = other.pointId_;
		worldId_ = other.worldId_;
		state_ = other.state_;
		ownerAllianceId_ = other.ownerAllianceId_;
		ownerServerId_ = other.ownerServerId_;
		tmpOwnerServerId_ = other.tmpOwnerServerId_;
		protectTime_ = other.protectTime_;
		battleStartTime_ = other.battleStartTime_;
		firstOccupyTime_ = other.firstOccupyTime_;
		firstOccupyAllianceId_ = other.firstOccupyAllianceId_;
		firstOccupyAllianceName_ = other.firstOccupyAllianceName_;
		firstOccupyAllianceAbbr_ = other.firstOccupyAllianceAbbr_;
		firstOccupyServerId_ = other.firstOccupyServerId_;
		lastRepairTime_ = other.lastRepairTime_;
		repairScore_ = other.repairScore_;
		connectSwitch_ = other.connectSwitch_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OutpostInfo Clone()
	{
		return new OutpostInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OutpostInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(OutpostInfo other)
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
		if (CityId != other.CityId)
		{
			return false;
		}
		if (PointId != other.PointId)
		{
			return false;
		}
		if (WorldId != other.WorldId)
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		if (OwnerAllianceId != other.OwnerAllianceId)
		{
			return false;
		}
		if (OwnerServerId != other.OwnerServerId)
		{
			return false;
		}
		if (TmpOwnerServerId != other.TmpOwnerServerId)
		{
			return false;
		}
		if (ProtectTime != other.ProtectTime)
		{
			return false;
		}
		if (BattleStartTime != other.BattleStartTime)
		{
			return false;
		}
		if (FirstOccupyTime != other.FirstOccupyTime)
		{
			return false;
		}
		if (FirstOccupyAllianceId != other.FirstOccupyAllianceId)
		{
			return false;
		}
		if (FirstOccupyAllianceName != other.FirstOccupyAllianceName)
		{
			return false;
		}
		if (FirstOccupyAllianceAbbr != other.FirstOccupyAllianceAbbr)
		{
			return false;
		}
		if (FirstOccupyServerId != other.FirstOccupyServerId)
		{
			return false;
		}
		if (LastRepairTime != other.LastRepairTime)
		{
			return false;
		}
		if (RepairScore != other.RepairScore)
		{
			return false;
		}
		if (ConnectSwitch != other.ConnectSwitch)
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
		if (CityId != 0)
		{
			num ^= CityId.GetHashCode();
		}
		if (PointId != 0)
		{
			num ^= PointId.GetHashCode();
		}
		if (WorldId != 0)
		{
			num ^= WorldId.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (OwnerAllianceId.Length != 0)
		{
			num ^= OwnerAllianceId.GetHashCode();
		}
		if (OwnerServerId != 0)
		{
			num ^= OwnerServerId.GetHashCode();
		}
		if (TmpOwnerServerId != 0)
		{
			num ^= TmpOwnerServerId.GetHashCode();
		}
		if (ProtectTime != 0L)
		{
			num ^= ProtectTime.GetHashCode();
		}
		if (BattleStartTime != 0L)
		{
			num ^= BattleStartTime.GetHashCode();
		}
		if (FirstOccupyTime != 0L)
		{
			num ^= FirstOccupyTime.GetHashCode();
		}
		if (FirstOccupyAllianceId.Length != 0)
		{
			num ^= FirstOccupyAllianceId.GetHashCode();
		}
		if (FirstOccupyAllianceName.Length != 0)
		{
			num ^= FirstOccupyAllianceName.GetHashCode();
		}
		if (FirstOccupyAllianceAbbr.Length != 0)
		{
			num ^= FirstOccupyAllianceAbbr.GetHashCode();
		}
		if (FirstOccupyServerId != 0)
		{
			num ^= FirstOccupyServerId.GetHashCode();
		}
		if (LastRepairTime != 0L)
		{
			num ^= LastRepairTime.GetHashCode();
		}
		if (RepairScore != 0)
		{
			num ^= RepairScore.GetHashCode();
		}
		if (ConnectSwitch != 0)
		{
			num ^= ConnectSwitch.GetHashCode();
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
		if (CityId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(CityId);
		}
		if (PointId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(PointId);
		}
		if (WorldId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(WorldId);
		}
		if (State != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(State);
		}
		if (OwnerAllianceId.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(OwnerAllianceId);
		}
		if (OwnerServerId != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(OwnerServerId);
		}
		if (TmpOwnerServerId != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(TmpOwnerServerId);
		}
		if (ProtectTime != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(ProtectTime);
		}
		if (BattleStartTime != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(BattleStartTime);
		}
		if (FirstOccupyTime != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(FirstOccupyTime);
		}
		if (FirstOccupyAllianceId.Length != 0)
		{
			output.WriteRawTag(98);
			output.WriteString(FirstOccupyAllianceId);
		}
		if (FirstOccupyAllianceName.Length != 0)
		{
			output.WriteRawTag(106);
			output.WriteString(FirstOccupyAllianceName);
		}
		if (FirstOccupyAllianceAbbr.Length != 0)
		{
			output.WriteRawTag(114);
			output.WriteString(FirstOccupyAllianceAbbr);
		}
		if (FirstOccupyServerId != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(FirstOccupyServerId);
		}
		if (LastRepairTime != 0L)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt64(LastRepairTime);
		}
		if (RepairScore != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(RepairScore);
		}
		if (ConnectSwitch != 0)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt32(ConnectSwitch);
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
		if (CityId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CityId);
		}
		if (PointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PointId);
		}
		if (WorldId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WorldId);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (OwnerAllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(OwnerAllianceId);
		}
		if (OwnerServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OwnerServerId);
		}
		if (TmpOwnerServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TmpOwnerServerId);
		}
		if (ProtectTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ProtectTime);
		}
		if (BattleStartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(BattleStartTime);
		}
		if (FirstOccupyTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FirstOccupyTime);
		}
		if (FirstOccupyAllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(FirstOccupyAllianceId);
		}
		if (FirstOccupyAllianceName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(FirstOccupyAllianceName);
		}
		if (FirstOccupyAllianceAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(FirstOccupyAllianceAbbr);
		}
		if (FirstOccupyServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FirstOccupyServerId);
		}
		if (LastRepairTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(LastRepairTime);
		}
		if (RepairScore != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(RepairScore);
		}
		if (ConnectSwitch != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ConnectSwitch);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OutpostInfo other)
	{
		if (other != null)
		{
			if (other.Uuid != 0L)
			{
				Uuid = other.Uuid;
			}
			if (other.CityId != 0)
			{
				CityId = other.CityId;
			}
			if (other.PointId != 0)
			{
				PointId = other.PointId;
			}
			if (other.WorldId != 0)
			{
				WorldId = other.WorldId;
			}
			if (other.State != 0)
			{
				State = other.State;
			}
			if (other.OwnerAllianceId.Length != 0)
			{
				OwnerAllianceId = other.OwnerAllianceId;
			}
			if (other.OwnerServerId != 0)
			{
				OwnerServerId = other.OwnerServerId;
			}
			if (other.TmpOwnerServerId != 0)
			{
				TmpOwnerServerId = other.TmpOwnerServerId;
			}
			if (other.ProtectTime != 0L)
			{
				ProtectTime = other.ProtectTime;
			}
			if (other.BattleStartTime != 0L)
			{
				BattleStartTime = other.BattleStartTime;
			}
			if (other.FirstOccupyTime != 0L)
			{
				FirstOccupyTime = other.FirstOccupyTime;
			}
			if (other.FirstOccupyAllianceId.Length != 0)
			{
				FirstOccupyAllianceId = other.FirstOccupyAllianceId;
			}
			if (other.FirstOccupyAllianceName.Length != 0)
			{
				FirstOccupyAllianceName = other.FirstOccupyAllianceName;
			}
			if (other.FirstOccupyAllianceAbbr.Length != 0)
			{
				FirstOccupyAllianceAbbr = other.FirstOccupyAllianceAbbr;
			}
			if (other.FirstOccupyServerId != 0)
			{
				FirstOccupyServerId = other.FirstOccupyServerId;
			}
			if (other.LastRepairTime != 0L)
			{
				LastRepairTime = other.LastRepairTime;
			}
			if (other.RepairScore != 0)
			{
				RepairScore = other.RepairScore;
			}
			if (other.ConnectSwitch != 0)
			{
				ConnectSwitch = other.ConnectSwitch;
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
				Uuid = input.ReadInt64();
				break;
			case 16u:
				CityId = input.ReadInt32();
				break;
			case 24u:
				PointId = input.ReadInt32();
				break;
			case 32u:
				WorldId = input.ReadInt32();
				break;
			case 40u:
				State = input.ReadInt32();
				break;
			case 50u:
				OwnerAllianceId = input.ReadString();
				break;
			case 56u:
				OwnerServerId = input.ReadInt32();
				break;
			case 64u:
				TmpOwnerServerId = input.ReadInt32();
				break;
			case 72u:
				ProtectTime = input.ReadInt64();
				break;
			case 80u:
				BattleStartTime = input.ReadInt64();
				break;
			case 88u:
				FirstOccupyTime = input.ReadInt64();
				break;
			case 98u:
				FirstOccupyAllianceId = input.ReadString();
				break;
			case 106u:
				FirstOccupyAllianceName = input.ReadString();
				break;
			case 114u:
				FirstOccupyAllianceAbbr = input.ReadString();
				break;
			case 120u:
				FirstOccupyServerId = input.ReadInt32();
				break;
			case 128u:
				LastRepairTime = input.ReadInt64();
				break;
			case 136u:
				RepairScore = input.ReadInt32();
				break;
			case 144u:
				ConnectSwitch = input.ReadInt32();
				break;
			}
		}
	}
}
