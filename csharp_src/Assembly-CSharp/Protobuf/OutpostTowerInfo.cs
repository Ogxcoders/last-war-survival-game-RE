using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class OutpostTowerInfo : IMessage<OutpostTowerInfo>, IMessage, IEquatable<OutpostTowerInfo>, IDeepCloneable<OutpostTowerInfo>
{
	private static readonly MessageParser<OutpostTowerInfo> _parser = new MessageParser<OutpostTowerInfo>(() => new OutpostTowerInfo());

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

	public const int TmpOwnerServerIdFieldNumber = 6;

	private int tmpOwnerServerId_;

	public const int ProtectTimeFieldNumber = 7;

	private long protectTime_;

	public const int BattleStartTimeFieldNumber = 8;

	private long battleStartTime_;

	public const int LastTowerAttackTimeFieldNumber = 9;

	private long lastTowerAttackTime_;

	[DebuggerNonUserCode]
	public static MessageParser<OutpostTowerInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[66];

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
	public long LastTowerAttackTime
	{
		get
		{
			return lastTowerAttackTime_;
		}
		set
		{
			lastTowerAttackTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public OutpostTowerInfo()
	{
	}

	[DebuggerNonUserCode]
	public OutpostTowerInfo(OutpostTowerInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		cityId_ = other.cityId_;
		pointId_ = other.pointId_;
		worldId_ = other.worldId_;
		state_ = other.state_;
		tmpOwnerServerId_ = other.tmpOwnerServerId_;
		protectTime_ = other.protectTime_;
		battleStartTime_ = other.battleStartTime_;
		lastTowerAttackTime_ = other.lastTowerAttackTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public OutpostTowerInfo Clone()
	{
		return new OutpostTowerInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as OutpostTowerInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(OutpostTowerInfo other)
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
		if (LastTowerAttackTime != other.LastTowerAttackTime)
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
		if (LastTowerAttackTime != 0L)
		{
			num ^= LastTowerAttackTime.GetHashCode();
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
		if (TmpOwnerServerId != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(TmpOwnerServerId);
		}
		if (ProtectTime != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(ProtectTime);
		}
		if (BattleStartTime != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(BattleStartTime);
		}
		if (LastTowerAttackTime != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(LastTowerAttackTime);
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
		if (LastTowerAttackTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(LastTowerAttackTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(OutpostTowerInfo other)
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
			if (other.LastTowerAttackTime != 0L)
			{
				LastTowerAttackTime = other.LastTowerAttackTime;
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
			case 48u:
				TmpOwnerServerId = input.ReadInt32();
				break;
			case 56u:
				ProtectTime = input.ReadInt64();
				break;
			case 64u:
				BattleStartTime = input.ReadInt64();
				break;
			case 72u:
				LastTowerAttackTime = input.ReadInt64();
				break;
			}
		}
	}
}
