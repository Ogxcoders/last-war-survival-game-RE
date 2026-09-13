using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class GhostReconPointInfo : IMessage<GhostReconPointInfo>, IMessage, IEquatable<GhostReconPointInfo>, IDeepCloneable<GhostReconPointInfo>
{
	private static readonly MessageParser<GhostReconPointInfo> _parser = new MessageParser<GhostReconPointInfo>(() => new GhostReconPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int OwnerUidFieldNumber = 1;

	private string ownerUid_ = "";

	public const int CfgIdFieldNumber = 2;

	private int cfgId_;

	public const int CompletionTimeFieldNumber = 3;

	private long completionTime_;

	public const int StealListFieldNumber = 4;

	private static readonly FieldCodec<string> _repeated_stealList_codec = FieldCodec.ForString(34u);

	private readonly RepeatedField<string> stealList_ = new RepeatedField<string>();

	public const int MemberListFieldNumber = 5;

	private static readonly FieldCodec<GhostReconMemberData> _repeated_memberList_codec = FieldCodec.ForMessage(42u, GhostReconMemberData.Parser);

	private readonly RepeatedField<GhostReconMemberData> memberList_ = new RepeatedField<GhostReconMemberData>();

	public const int OwnerServerFieldNumber = 6;

	private int ownerServer_;

	public const int TaskExpireTimeFieldNumber = 7;

	private long taskExpireTime_;

	public const int AllianceIdFieldNumber = 8;

	private string allianceId_ = "";

	public const int SizeFieldNumber = 9;

	private int size_;

	public const int ActEndTimeFieldNumber = 10;

	private long actEndTime_;

	public const int TeamStartTimeFieldNumber = 11;

	private long teamStartTime_;

	[DebuggerNonUserCode]
	public static MessageParser<GhostReconPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[45];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public int CfgId
	{
		get
		{
			return cfgId_;
		}
		set
		{
			cfgId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CompletionTime
	{
		get
		{
			return completionTime_;
		}
		set
		{
			completionTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<string> StealList => stealList_;

	[DebuggerNonUserCode]
	public RepeatedField<GhostReconMemberData> MemberList => memberList_;

	[DebuggerNonUserCode]
	public int OwnerServer
	{
		get
		{
			return ownerServer_;
		}
		set
		{
			ownerServer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long TaskExpireTime
	{
		get
		{
			return taskExpireTime_;
		}
		set
		{
			taskExpireTime_ = value;
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
	public int Size
	{
		get
		{
			return size_;
		}
		set
		{
			size_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ActEndTime
	{
		get
		{
			return actEndTime_;
		}
		set
		{
			actEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long TeamStartTime
	{
		get
		{
			return teamStartTime_;
		}
		set
		{
			teamStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public GhostReconPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public GhostReconPointInfo(GhostReconPointInfo other)
		: this()
	{
		ownerUid_ = other.ownerUid_;
		cfgId_ = other.cfgId_;
		completionTime_ = other.completionTime_;
		stealList_ = other.stealList_.Clone();
		memberList_ = other.memberList_.Clone();
		ownerServer_ = other.ownerServer_;
		taskExpireTime_ = other.taskExpireTime_;
		allianceId_ = other.allianceId_;
		size_ = other.size_;
		actEndTime_ = other.actEndTime_;
		teamStartTime_ = other.teamStartTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GhostReconPointInfo Clone()
	{
		return new GhostReconPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GhostReconPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(GhostReconPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (OwnerUid != other.OwnerUid)
		{
			return false;
		}
		if (CfgId != other.CfgId)
		{
			return false;
		}
		if (CompletionTime != other.CompletionTime)
		{
			return false;
		}
		if (!stealList_.Equals(other.stealList_))
		{
			return false;
		}
		if (!memberList_.Equals(other.memberList_))
		{
			return false;
		}
		if (OwnerServer != other.OwnerServer)
		{
			return false;
		}
		if (TaskExpireTime != other.TaskExpireTime)
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (Size != other.Size)
		{
			return false;
		}
		if (ActEndTime != other.ActEndTime)
		{
			return false;
		}
		if (TeamStartTime != other.TeamStartTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (OwnerUid.Length != 0)
		{
			num ^= OwnerUid.GetHashCode();
		}
		if (CfgId != 0)
		{
			num ^= CfgId.GetHashCode();
		}
		if (CompletionTime != 0L)
		{
			num ^= CompletionTime.GetHashCode();
		}
		num ^= stealList_.GetHashCode();
		num ^= memberList_.GetHashCode();
		if (OwnerServer != 0)
		{
			num ^= OwnerServer.GetHashCode();
		}
		if (TaskExpireTime != 0L)
		{
			num ^= TaskExpireTime.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (Size != 0)
		{
			num ^= Size.GetHashCode();
		}
		if (ActEndTime != 0L)
		{
			num ^= ActEndTime.GetHashCode();
		}
		if (TeamStartTime != 0L)
		{
			num ^= TeamStartTime.GetHashCode();
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
		if (OwnerUid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(OwnerUid);
		}
		if (CfgId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(CfgId);
		}
		if (CompletionTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(CompletionTime);
		}
		stealList_.WriteTo(output, _repeated_stealList_codec);
		memberList_.WriteTo(output, _repeated_memberList_codec);
		if (OwnerServer != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(OwnerServer);
		}
		if (TaskExpireTime != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(TaskExpireTime);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(AllianceId);
		}
		if (Size != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Size);
		}
		if (ActEndTime != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(ActEndTime);
		}
		if (TeamStartTime != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(TeamStartTime);
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
		if (OwnerUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(OwnerUid);
		}
		if (CfgId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CfgId);
		}
		if (CompletionTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CompletionTime);
		}
		num += stealList_.CalculateSize(_repeated_stealList_codec);
		num += memberList_.CalculateSize(_repeated_memberList_codec);
		if (OwnerServer != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OwnerServer);
		}
		if (TaskExpireTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TaskExpireTime);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (Size != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Size);
		}
		if (ActEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ActEndTime);
		}
		if (TeamStartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TeamStartTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GhostReconPointInfo other)
	{
		if (other != null)
		{
			if (other.OwnerUid.Length != 0)
			{
				OwnerUid = other.OwnerUid;
			}
			if (other.CfgId != 0)
			{
				CfgId = other.CfgId;
			}
			if (other.CompletionTime != 0L)
			{
				CompletionTime = other.CompletionTime;
			}
			stealList_.Add(other.stealList_);
			memberList_.Add(other.memberList_);
			if (other.OwnerServer != 0)
			{
				OwnerServer = other.OwnerServer;
			}
			if (other.TaskExpireTime != 0L)
			{
				TaskExpireTime = other.TaskExpireTime;
			}
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.Size != 0)
			{
				Size = other.Size;
			}
			if (other.ActEndTime != 0L)
			{
				ActEndTime = other.ActEndTime;
			}
			if (other.TeamStartTime != 0L)
			{
				TeamStartTime = other.TeamStartTime;
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
				OwnerUid = input.ReadString();
				break;
			case 16u:
				CfgId = input.ReadInt32();
				break;
			case 24u:
				CompletionTime = input.ReadInt64();
				break;
			case 34u:
				stealList_.AddEntriesFrom(input, _repeated_stealList_codec);
				break;
			case 42u:
				memberList_.AddEntriesFrom(input, _repeated_memberList_codec);
				break;
			case 48u:
				OwnerServer = input.ReadInt32();
				break;
			case 56u:
				TaskExpireTime = input.ReadInt64();
				break;
			case 66u:
				AllianceId = input.ReadString();
				break;
			case 72u:
				Size = input.ReadInt32();
				break;
			case 80u:
				ActEndTime = input.ReadInt64();
				break;
			case 88u:
				TeamStartTime = input.ReadInt64();
				break;
			}
		}
	}
}
