using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DragonBuildingPointInfo : IMessage<DragonBuildingPointInfo>, IMessage, IEquatable<DragonBuildingPointInfo>, IDeepCloneable<DragonBuildingPointInfo>
{
	private static readonly MessageParser<DragonBuildingPointInfo> _parser = new MessageParser<DragonBuildingPointInfo>(() => new DragonBuildingPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int BuildIdFieldNumber = 2;

	private int buildId_;

	public const int StateFieldNumber = 3;

	private int state_;

	public const int AlAbbrFieldNumber = 4;

	private string alAbbr_ = "";

	public const int AllianceIdFieldNumber = 5;

	private string allianceId_ = "";

	public const int OccupyTimeFieldNumber = 6;

	private long occupyTime_;

	public const int OpenTimeFieldNumber = 7;

	private long openTime_;

	public const int StartTimeFieldNumber = 8;

	private long startTime_;

	public const int ProtectTimeFieldNumber = 9;

	private long protectTime_;

	public const int RewardCountFieldNumber = 10;

	private int rewardCount_;

	public const int OverflowScoreFieldNumber = 11;

	private int overflowScore_;

	public const int ScoreFieldNumber = 12;

	private int score_;

	[DebuggerNonUserCode]
	public static MessageParser<DragonBuildingPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[42];

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
	public long OccupyTime
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
	public long OpenTime
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
	public int RewardCount
	{
		get
		{
			return rewardCount_;
		}
		set
		{
			rewardCount_ = value;
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
	public DragonBuildingPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public DragonBuildingPointInfo(DragonBuildingPointInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		buildId_ = other.buildId_;
		state_ = other.state_;
		alAbbr_ = other.alAbbr_;
		allianceId_ = other.allianceId_;
		occupyTime_ = other.occupyTime_;
		openTime_ = other.openTime_;
		startTime_ = other.startTime_;
		protectTime_ = other.protectTime_;
		rewardCount_ = other.rewardCount_;
		overflowScore_ = other.overflowScore_;
		score_ = other.score_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DragonBuildingPointInfo Clone()
	{
		return new DragonBuildingPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DragonBuildingPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(DragonBuildingPointInfo other)
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
		if (OccupyTime != other.OccupyTime)
		{
			return false;
		}
		if (OpenTime != other.OpenTime)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (ProtectTime != other.ProtectTime)
		{
			return false;
		}
		if (RewardCount != other.RewardCount)
		{
			return false;
		}
		if (OverflowScore != other.OverflowScore)
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
		if (OccupyTime != 0L)
		{
			num ^= OccupyTime.GetHashCode();
		}
		if (OpenTime != 0L)
		{
			num ^= OpenTime.GetHashCode();
		}
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (ProtectTime != 0L)
		{
			num ^= ProtectTime.GetHashCode();
		}
		if (RewardCount != 0)
		{
			num ^= RewardCount.GetHashCode();
		}
		if (OverflowScore != 0)
		{
			num ^= OverflowScore.GetHashCode();
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
		if (State != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(State);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(AllianceId);
		}
		if (OccupyTime != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(OccupyTime);
		}
		if (OpenTime != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(OpenTime);
		}
		if (StartTime != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(StartTime);
		}
		if (ProtectTime != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(ProtectTime);
		}
		if (RewardCount != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(RewardCount);
		}
		if (OverflowScore != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(OverflowScore);
		}
		if (Score != 0)
		{
			output.WriteRawTag(96);
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
		if (OccupyTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(OccupyTime);
		}
		if (OpenTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(OpenTime);
		}
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (ProtectTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ProtectTime);
		}
		if (RewardCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RewardCount);
		}
		if (OverflowScore != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OverflowScore);
		}
		if (Score != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Score);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DragonBuildingPointInfo other)
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
			if (other.OccupyTime != 0L)
			{
				OccupyTime = other.OccupyTime;
			}
			if (other.OpenTime != 0L)
			{
				OpenTime = other.OpenTime;
			}
			if (other.StartTime != 0L)
			{
				StartTime = other.StartTime;
			}
			if (other.ProtectTime != 0L)
			{
				ProtectTime = other.ProtectTime;
			}
			if (other.RewardCount != 0)
			{
				RewardCount = other.RewardCount;
			}
			if (other.OverflowScore != 0)
			{
				OverflowScore = other.OverflowScore;
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
				State = input.ReadInt32();
				break;
			case 34u:
				AlAbbr = input.ReadString();
				break;
			case 42u:
				AllianceId = input.ReadString();
				break;
			case 48u:
				OccupyTime = input.ReadInt64();
				break;
			case 56u:
				OpenTime = input.ReadInt64();
				break;
			case 64u:
				StartTime = input.ReadInt64();
				break;
			case 72u:
				ProtectTime = input.ReadInt64();
				break;
			case 80u:
				RewardCount = input.ReadInt32();
				break;
			case 88u:
				OverflowScore = input.ReadInt32();
				break;
			case 96u:
				Score = input.ReadInt32();
				break;
			}
		}
	}
}
