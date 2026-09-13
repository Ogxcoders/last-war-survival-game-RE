using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WinterEntityPointInfo : IMessage<WinterEntityPointInfo>, IMessage, IEquatable<WinterEntityPointInfo>, IDeepCloneable<WinterEntityPointInfo>
{
	private static readonly MessageParser<WinterEntityPointInfo> _parser = new MessageParser<WinterEntityPointInfo>(() => new WinterEntityPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int BuildIdFieldNumber = 2;

	private int buildId_;

	public const int CloseTimeFieldNumber = 3;

	private int closeTime_;

	public const int OpenTimeFieldNumber = 4;

	private int openTime_;

	public const int OccupyingStartTimeFieldNumber = 5;

	private int occupyingStartTime_;

	public const int SideFieldNumber = 6;

	private int side_;

	public const int EventStartTimeFieldNumber = 7;

	private int eventStartTime_;

	public const int EventFinishTimeFieldNumber = 8;

	private int eventFinishTime_;

	public const int FindAimTimeFieldNumber = 9;

	private int findAimTime_;

	public const int AimDeadTimeFieldNumber = 10;

	private int aimDeadTime_;

	public const int TargetEnemyUUIDFieldNumber = 11;

	private static readonly FieldCodec<long> _repeated_targetEnemyUUID_codec = FieldCodec.ForInt64(90u);

	private readonly RepeatedField<long> targetEnemyUUID_ = new RepeatedField<long>();

	public const int StateFieldNumber = 12;

	private int state_;

	public const int PointIdFieldNumber = 13;

	private int pointId_;

	public const int OwnerUidFieldNumber = 14;

	private string ownerUid_ = "";

	public const int LastOccupyTimeFieldNumber = 15;

	private int lastOccupyTime_;

	public const int HpFieldNumber = 16;

	private int hp_;

	public const int ScoreFieldNumber = 17;

	private int score_;

	[DebuggerNonUserCode]
	public static MessageParser<WinterEntityPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[55];

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
	public int CloseTime
	{
		get
		{
			return closeTime_;
		}
		set
		{
			closeTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OpenTime
	{
		get
		{
			return openTime_;
		}
		set
		{
			openTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OccupyingStartTime
	{
		get
		{
			return occupyingStartTime_;
		}
		set
		{
			occupyingStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Side
	{
		get
		{
			return side_;
		}
		set
		{
			side_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EventStartTime
	{
		get
		{
			return eventStartTime_;
		}
		set
		{
			eventStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EventFinishTime
	{
		get
		{
			return eventFinishTime_;
		}
		set
		{
			eventFinishTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int FindAimTime
	{
		get
		{
			return findAimTime_;
		}
		set
		{
			findAimTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AimDeadTime
	{
		get
		{
			return aimDeadTime_;
		}
		set
		{
			aimDeadTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<long> TargetEnemyUUID => targetEnemyUUID_;

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
	public string OwnerUid
	{
		get
		{
			return ownerUid_;
		}
		set
		{
			ownerUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int LastOccupyTime
	{
		get
		{
			return lastOccupyTime_;
		}
		set
		{
			lastOccupyTime_ = value;
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
	public int Score
	{
		get
		{
			return score_;
		}
		set
		{
			score_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WinterEntityPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public WinterEntityPointInfo(WinterEntityPointInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		buildId_ = other.buildId_;
		closeTime_ = other.closeTime_;
		openTime_ = other.openTime_;
		occupyingStartTime_ = other.occupyingStartTime_;
		side_ = other.side_;
		eventStartTime_ = other.eventStartTime_;
		eventFinishTime_ = other.eventFinishTime_;
		findAimTime_ = other.findAimTime_;
		aimDeadTime_ = other.aimDeadTime_;
		targetEnemyUUID_ = other.targetEnemyUUID_.Clone();
		state_ = other.state_;
		pointId_ = other.pointId_;
		ownerUid_ = other.ownerUid_;
		lastOccupyTime_ = other.lastOccupyTime_;
		hp_ = other.hp_;
		score_ = other.score_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WinterEntityPointInfo Clone()
	{
		return new WinterEntityPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WinterEntityPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(WinterEntityPointInfo other)
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
		if (CloseTime != other.CloseTime)
		{
			return false;
		}
		if (OpenTime != other.OpenTime)
		{
			return false;
		}
		if (OccupyingStartTime != other.OccupyingStartTime)
		{
			return false;
		}
		if (Side != other.Side)
		{
			return false;
		}
		if (EventStartTime != other.EventStartTime)
		{
			return false;
		}
		if (EventFinishTime != other.EventFinishTime)
		{
			return false;
		}
		if (FindAimTime != other.FindAimTime)
		{
			return false;
		}
		if (AimDeadTime != other.AimDeadTime)
		{
			return false;
		}
		if (!targetEnemyUUID_.Equals(other.targetEnemyUUID_))
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		if (PointId != other.PointId)
		{
			return false;
		}
		if (OwnerUid != other.OwnerUid)
		{
			return false;
		}
		if (LastOccupyTime != other.LastOccupyTime)
		{
			return false;
		}
		if (Hp != other.Hp)
		{
			return false;
		}
		if (Score != other.Score)
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
		if (CloseTime != 0)
		{
			num ^= CloseTime.GetHashCode();
		}
		if (OpenTime != 0)
		{
			num ^= OpenTime.GetHashCode();
		}
		if (OccupyingStartTime != 0)
		{
			num ^= OccupyingStartTime.GetHashCode();
		}
		if (Side != 0)
		{
			num ^= Side.GetHashCode();
		}
		if (EventStartTime != 0)
		{
			num ^= EventStartTime.GetHashCode();
		}
		if (EventFinishTime != 0)
		{
			num ^= EventFinishTime.GetHashCode();
		}
		if (FindAimTime != 0)
		{
			num ^= FindAimTime.GetHashCode();
		}
		if (AimDeadTime != 0)
		{
			num ^= AimDeadTime.GetHashCode();
		}
		num ^= targetEnemyUUID_.GetHashCode();
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (PointId != 0)
		{
			num ^= PointId.GetHashCode();
		}
		if (OwnerUid.Length != 0)
		{
			num ^= OwnerUid.GetHashCode();
		}
		if (LastOccupyTime != 0)
		{
			num ^= LastOccupyTime.GetHashCode();
		}
		if (Hp != 0)
		{
			num ^= Hp.GetHashCode();
		}
		if (Score != 0)
		{
			num ^= Score.GetHashCode();
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
		if (CloseTime != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(CloseTime);
		}
		if (OpenTime != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(OpenTime);
		}
		if (OccupyingStartTime != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(OccupyingStartTime);
		}
		if (Side != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(Side);
		}
		if (EventStartTime != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(EventStartTime);
		}
		if (EventFinishTime != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(EventFinishTime);
		}
		if (FindAimTime != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(FindAimTime);
		}
		if (AimDeadTime != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(AimDeadTime);
		}
		targetEnemyUUID_.WriteTo(output, _repeated_targetEnemyUUID_codec);
		if (State != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(State);
		}
		if (PointId != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(PointId);
		}
		if (OwnerUid.Length != 0)
		{
			output.WriteRawTag(114);
			output.WriteString(OwnerUid);
		}
		if (LastOccupyTime != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(LastOccupyTime);
		}
		if (Hp != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(Hp);
		}
		if (Score != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(Score);
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
		if (CloseTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CloseTime);
		}
		if (OpenTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OpenTime);
		}
		if (OccupyingStartTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OccupyingStartTime);
		}
		if (Side != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Side);
		}
		if (EventStartTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EventStartTime);
		}
		if (EventFinishTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EventFinishTime);
		}
		if (FindAimTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FindAimTime);
		}
		if (AimDeadTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AimDeadTime);
		}
		num += targetEnemyUUID_.CalculateSize(_repeated_targetEnemyUUID_codec);
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (PointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PointId);
		}
		if (OwnerUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(OwnerUid);
		}
		if (LastOccupyTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LastOccupyTime);
		}
		if (Hp != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Hp);
		}
		if (Score != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Score);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WinterEntityPointInfo other)
	{
		if (other != null)
		{
			if (other.Uuid != 0L)
			{
				Uuid = other.Uuid;
			}
			if (other.BuildId != 0)
			{
				BuildId = other.BuildId;
			}
			if (other.CloseTime != 0)
			{
				CloseTime = other.CloseTime;
			}
			if (other.OpenTime != 0)
			{
				OpenTime = other.OpenTime;
			}
			if (other.OccupyingStartTime != 0)
			{
				OccupyingStartTime = other.OccupyingStartTime;
			}
			if (other.Side != 0)
			{
				Side = other.Side;
			}
			if (other.EventStartTime != 0)
			{
				EventStartTime = other.EventStartTime;
			}
			if (other.EventFinishTime != 0)
			{
				EventFinishTime = other.EventFinishTime;
			}
			if (other.FindAimTime != 0)
			{
				FindAimTime = other.FindAimTime;
			}
			if (other.AimDeadTime != 0)
			{
				AimDeadTime = other.AimDeadTime;
			}
			targetEnemyUUID_.Add(other.targetEnemyUUID_);
			if (other.State != 0)
			{
				State = other.State;
			}
			if (other.PointId != 0)
			{
				PointId = other.PointId;
			}
			if (other.OwnerUid.Length != 0)
			{
				OwnerUid = other.OwnerUid;
			}
			if (other.LastOccupyTime != 0)
			{
				LastOccupyTime = other.LastOccupyTime;
			}
			if (other.Hp != 0)
			{
				Hp = other.Hp;
			}
			if (other.Score != 0)
			{
				Score = other.Score;
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
				BuildId = input.ReadInt32();
				break;
			case 24u:
				CloseTime = input.ReadInt32();
				break;
			case 32u:
				OpenTime = input.ReadInt32();
				break;
			case 40u:
				OccupyingStartTime = input.ReadInt32();
				break;
			case 48u:
				Side = input.ReadInt32();
				break;
			case 56u:
				EventStartTime = input.ReadInt32();
				break;
			case 64u:
				EventFinishTime = input.ReadInt32();
				break;
			case 72u:
				FindAimTime = input.ReadInt32();
				break;
			case 80u:
				AimDeadTime = input.ReadInt32();
				break;
			case 88u:
			case 90u:
				targetEnemyUUID_.AddEntriesFrom(input, _repeated_targetEnemyUUID_codec);
				break;
			case 96u:
				State = input.ReadInt32();
				break;
			case 104u:
				PointId = input.ReadInt32();
				break;
			case 114u:
				OwnerUid = input.ReadString();
				break;
			case 120u:
				LastOccupyTime = input.ReadInt32();
				break;
			case 128u:
				Hp = input.ReadInt32();
				break;
			case 136u:
				Score = input.ReadInt32();
				break;
			}
		}
	}
}
