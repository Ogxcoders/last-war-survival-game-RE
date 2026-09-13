using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class QuarantinePointInfo : IMessage<QuarantinePointInfo>, IMessage, IEquatable<QuarantinePointInfo>, IDeepCloneable<QuarantinePointInfo>
{
	private static readonly MessageParser<QuarantinePointInfo> _parser = new MessageParser<QuarantinePointInfo>(() => new QuarantinePointInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int BuildIdFieldNumber = 2;

	private int buildId_;

	public const int OpenTimeFieldNumber = 3;

	private int openTime_;

	public const int RoleFieldNumber = 4;

	private int role_;

	public const int FindAimTimeFieldNumber = 5;

	private int findAimTime_;

	public const int HitAimTimeFieldNumber = 6;

	private int hitAimTime_;

	public const int StateFieldNumber = 7;

	private int state_;

	public const int PointIdFieldNumber = 8;

	private int pointId_;

	public const int ScoreFieldNumber = 9;

	private int score_;

	public const int OverflowScoreFieldNumber = 10;

	private int overflowScore_;

	public const int OccupyTimeFieldNumber = 11;

	private int occupyTime_;

	public const int BuffIdFieldNumber = 12;

	private int buffId_;

	public const int BuffEndTimeFieldNumber = 13;

	private int buffEndTime_;

	public const int MarchUUIDFieldNumber = 14;

	private long marchUUID_;

	public const int MarchUidFieldNumber = 15;

	private string marchUid_ = "";

	public const int TargetUUIDFieldNumber = 16;

	private static readonly FieldCodec<long> _repeated_targetUUID_codec = FieldCodec.ForInt64(130u);

	private readonly RepeatedField<long> targetUUID_ = new RepeatedField<long>();

	[DebuggerNonUserCode]
	public static MessageParser<QuarantinePointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[63];

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
	public int Role
	{
		get
		{
			return role_;
		}
		set
		{
			role_ = value;
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
	public int HitAimTime
	{
		get
		{
			return hitAimTime_;
		}
		set
		{
			hitAimTime_ = value;
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
	public int OverflowScore
	{
		get
		{
			return overflowScore_;
		}
		set
		{
			overflowScore_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OccupyTime
	{
		get
		{
			return occupyTime_;
		}
		set
		{
			occupyTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BuffId
	{
		get
		{
			return buffId_;
		}
		set
		{
			buffId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BuffEndTime
	{
		get
		{
			return buffEndTime_;
		}
		set
		{
			buffEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long MarchUUID
	{
		get
		{
			return marchUUID_;
		}
		set
		{
			marchUUID_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string MarchUid
	{
		get
		{
			return marchUid_;
		}
		set
		{
			marchUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<long> TargetUUID => targetUUID_;

	[DebuggerNonUserCode]
	public QuarantinePointInfo()
	{
	}

	[DebuggerNonUserCode]
	public QuarantinePointInfo(QuarantinePointInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		buildId_ = other.buildId_;
		openTime_ = other.openTime_;
		role_ = other.role_;
		findAimTime_ = other.findAimTime_;
		hitAimTime_ = other.hitAimTime_;
		state_ = other.state_;
		pointId_ = other.pointId_;
		score_ = other.score_;
		overflowScore_ = other.overflowScore_;
		occupyTime_ = other.occupyTime_;
		buffId_ = other.buffId_;
		buffEndTime_ = other.buffEndTime_;
		marchUUID_ = other.marchUUID_;
		marchUid_ = other.marchUid_;
		targetUUID_ = other.targetUUID_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public QuarantinePointInfo Clone()
	{
		return new QuarantinePointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as QuarantinePointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(QuarantinePointInfo other)
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
		if (OpenTime != other.OpenTime)
		{
			return false;
		}
		if (Role != other.Role)
		{
			return false;
		}
		if (FindAimTime != other.FindAimTime)
		{
			return false;
		}
		if (HitAimTime != other.HitAimTime)
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
		if (Score != other.Score)
		{
			return false;
		}
		if (OverflowScore != other.OverflowScore)
		{
			return false;
		}
		if (OccupyTime != other.OccupyTime)
		{
			return false;
		}
		if (BuffId != other.BuffId)
		{
			return false;
		}
		if (BuffEndTime != other.BuffEndTime)
		{
			return false;
		}
		if (MarchUUID != other.MarchUUID)
		{
			return false;
		}
		if (MarchUid != other.MarchUid)
		{
			return false;
		}
		if (!targetUUID_.Equals(other.targetUUID_))
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
		if (OpenTime != 0)
		{
			num ^= OpenTime.GetHashCode();
		}
		if (Role != 0)
		{
			num ^= Role.GetHashCode();
		}
		if (FindAimTime != 0)
		{
			num ^= FindAimTime.GetHashCode();
		}
		if (HitAimTime != 0)
		{
			num ^= HitAimTime.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (PointId != 0)
		{
			num ^= PointId.GetHashCode();
		}
		if (Score != 0)
		{
			num ^= Score.GetHashCode();
		}
		if (OverflowScore != 0)
		{
			num ^= OverflowScore.GetHashCode();
		}
		if (OccupyTime != 0)
		{
			num ^= OccupyTime.GetHashCode();
		}
		if (BuffId != 0)
		{
			num ^= BuffId.GetHashCode();
		}
		if (BuffEndTime != 0)
		{
			num ^= BuffEndTime.GetHashCode();
		}
		if (MarchUUID != 0L)
		{
			num ^= MarchUUID.GetHashCode();
		}
		if (MarchUid.Length != 0)
		{
			num ^= MarchUid.GetHashCode();
		}
		num ^= targetUUID_.GetHashCode();
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
		if (OpenTime != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(OpenTime);
		}
		if (Role != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Role);
		}
		if (FindAimTime != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(FindAimTime);
		}
		if (HitAimTime != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(HitAimTime);
		}
		if (State != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(State);
		}
		if (PointId != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(PointId);
		}
		if (Score != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Score);
		}
		if (OverflowScore != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(OverflowScore);
		}
		if (OccupyTime != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(OccupyTime);
		}
		if (BuffId != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(BuffId);
		}
		if (BuffEndTime != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(BuffEndTime);
		}
		if (MarchUUID != 0L)
		{
			output.WriteRawTag(112);
			output.WriteInt64(MarchUUID);
		}
		if (MarchUid.Length != 0)
		{
			output.WriteRawTag(122);
			output.WriteString(MarchUid);
		}
		targetUUID_.WriteTo(output, _repeated_targetUUID_codec);
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
		if (OpenTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OpenTime);
		}
		if (Role != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Role);
		}
		if (FindAimTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FindAimTime);
		}
		if (HitAimTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HitAimTime);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (PointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PointId);
		}
		if (Score != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Score);
		}
		if (OverflowScore != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OverflowScore);
		}
		if (OccupyTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OccupyTime);
		}
		if (BuffId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuffId);
		}
		if (BuffEndTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuffEndTime);
		}
		if (MarchUUID != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MarchUUID);
		}
		if (MarchUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(MarchUid);
		}
		num += targetUUID_.CalculateSize(_repeated_targetUUID_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(QuarantinePointInfo other)
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
			if (other.OpenTime != 0)
			{
				OpenTime = other.OpenTime;
			}
			if (other.Role != 0)
			{
				Role = other.Role;
			}
			if (other.FindAimTime != 0)
			{
				FindAimTime = other.FindAimTime;
			}
			if (other.HitAimTime != 0)
			{
				HitAimTime = other.HitAimTime;
			}
			if (other.State != 0)
			{
				State = other.State;
			}
			if (other.PointId != 0)
			{
				PointId = other.PointId;
			}
			if (other.Score != 0)
			{
				Score = other.Score;
			}
			if (other.OverflowScore != 0)
			{
				OverflowScore = other.OverflowScore;
			}
			if (other.OccupyTime != 0)
			{
				OccupyTime = other.OccupyTime;
			}
			if (other.BuffId != 0)
			{
				BuffId = other.BuffId;
			}
			if (other.BuffEndTime != 0)
			{
				BuffEndTime = other.BuffEndTime;
			}
			if (other.MarchUUID != 0L)
			{
				MarchUUID = other.MarchUUID;
			}
			if (other.MarchUid.Length != 0)
			{
				MarchUid = other.MarchUid;
			}
			targetUUID_.Add(other.targetUUID_);
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
				OpenTime = input.ReadInt32();
				break;
			case 32u:
				Role = input.ReadInt32();
				break;
			case 40u:
				FindAimTime = input.ReadInt32();
				break;
			case 48u:
				HitAimTime = input.ReadInt32();
				break;
			case 56u:
				State = input.ReadInt32();
				break;
			case 64u:
				PointId = input.ReadInt32();
				break;
			case 72u:
				Score = input.ReadInt32();
				break;
			case 80u:
				OverflowScore = input.ReadInt32();
				break;
			case 88u:
				OccupyTime = input.ReadInt32();
				break;
			case 96u:
				BuffId = input.ReadInt32();
				break;
			case 104u:
				BuffEndTime = input.ReadInt32();
				break;
			case 112u:
				MarchUUID = input.ReadInt64();
				break;
			case 122u:
				MarchUid = input.ReadString();
				break;
			case 128u:
			case 130u:
				targetUUID_.AddEntriesFrom(input, _repeated_targetUUID_codec);
				break;
			}
		}
	}
}
