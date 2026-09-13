using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MeteoritePoint : IMessage<MeteoritePoint>, IMessage, IEquatable<MeteoritePoint>, IDeepCloneable<MeteoritePoint>
{
	private static readonly MessageParser<MeteoritePoint> _parser = new MessageParser<MeteoritePoint>(() => new MeteoritePoint());

	private UnknownFieldSet _unknownFields;

	public const int BuildIdFieldNumber = 1;

	private int buildId_;

	public const int OpenTimeFieldNumber = 2;

	private int openTime_;

	public const int CollectEndTimeFieldNumber = 3;

	private int collectEndTime_;

	public const int ExpireTimeFieldNumber = 4;

	private int expireTime_;

	public const int RemainTimeFieldNumber = 5;

	private int remainTime_;

	public const int GatherUUIDFieldNumber = 6;

	private long gatherUUID_;

	public const int GatherUidFieldNumber = 7;

	private string gatherUid_ = "";

	public const int GatherAllianceIdFieldNumber = 8;

	private string gatherAllianceId_ = "";

	public const int FromPointFieldNumber = 9;

	private int fromPoint_;

	public const int TargetPicFieldNumber = 10;

	private string targetPic_ = "";

	public const int TargetPicVerFieldNumber = 11;

	private int targetPicVer_;

	public const int TargetHeadIdFieldNumber = 12;

	private int targetHeadId_;

	public const int TargetHeadETFieldNumber = 13;

	private long targetHeadET_;

	[DebuggerNonUserCode]
	public static MessageParser<MeteoritePoint> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[61];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public int CollectEndTime
	{
		get
		{
			return collectEndTime_;
		}
		set
		{
			collectEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ExpireTime
	{
		get
		{
			return expireTime_;
		}
		set
		{
			expireTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RemainTime
	{
		get
		{
			return remainTime_;
		}
		set
		{
			remainTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long GatherUUID
	{
		get
		{
			return gatherUUID_;
		}
		set
		{
			gatherUUID_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string GatherUid
	{
		get
		{
			return gatherUid_;
		}
		set
		{
			gatherUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string GatherAllianceId
	{
		get
		{
			return gatherAllianceId_;
		}
		set
		{
			gatherAllianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int FromPoint
	{
		get
		{
			return fromPoint_;
		}
		set
		{
			fromPoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string TargetPic
	{
		get
		{
			return targetPic_;
		}
		set
		{
			targetPic_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int TargetPicVer
	{
		get
		{
			return targetPicVer_;
		}
		set
		{
			targetPicVer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TargetHeadId
	{
		get
		{
			return targetHeadId_;
		}
		set
		{
			targetHeadId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long TargetHeadET
	{
		get
		{
			return targetHeadET_;
		}
		set
		{
			targetHeadET_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MeteoritePoint()
	{
	}

	[DebuggerNonUserCode]
	public MeteoritePoint(MeteoritePoint other)
		: this()
	{
		buildId_ = other.buildId_;
		openTime_ = other.openTime_;
		collectEndTime_ = other.collectEndTime_;
		expireTime_ = other.expireTime_;
		remainTime_ = other.remainTime_;
		gatherUUID_ = other.gatherUUID_;
		gatherUid_ = other.gatherUid_;
		gatherAllianceId_ = other.gatherAllianceId_;
		fromPoint_ = other.fromPoint_;
		targetPic_ = other.targetPic_;
		targetPicVer_ = other.targetPicVer_;
		targetHeadId_ = other.targetHeadId_;
		targetHeadET_ = other.targetHeadET_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MeteoritePoint Clone()
	{
		return new MeteoritePoint(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MeteoritePoint);
	}

	[DebuggerNonUserCode]
	public bool Equals(MeteoritePoint other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BuildId != other.BuildId)
		{
			return false;
		}
		if (OpenTime != other.OpenTime)
		{
			return false;
		}
		if (CollectEndTime != other.CollectEndTime)
		{
			return false;
		}
		if (ExpireTime != other.ExpireTime)
		{
			return false;
		}
		if (RemainTime != other.RemainTime)
		{
			return false;
		}
		if (GatherUUID != other.GatherUUID)
		{
			return false;
		}
		if (GatherUid != other.GatherUid)
		{
			return false;
		}
		if (GatherAllianceId != other.GatherAllianceId)
		{
			return false;
		}
		if (FromPoint != other.FromPoint)
		{
			return false;
		}
		if (TargetPic != other.TargetPic)
		{
			return false;
		}
		if (TargetPicVer != other.TargetPicVer)
		{
			return false;
		}
		if (TargetHeadId != other.TargetHeadId)
		{
			return false;
		}
		if (TargetHeadET != other.TargetHeadET)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (BuildId != 0)
		{
			num ^= BuildId.GetHashCode();
		}
		if (OpenTime != 0)
		{
			num ^= OpenTime.GetHashCode();
		}
		if (CollectEndTime != 0)
		{
			num ^= CollectEndTime.GetHashCode();
		}
		if (ExpireTime != 0)
		{
			num ^= ExpireTime.GetHashCode();
		}
		if (RemainTime != 0)
		{
			num ^= RemainTime.GetHashCode();
		}
		if (GatherUUID != 0L)
		{
			num ^= GatherUUID.GetHashCode();
		}
		if (GatherUid.Length != 0)
		{
			num ^= GatherUid.GetHashCode();
		}
		if (GatherAllianceId.Length != 0)
		{
			num ^= GatherAllianceId.GetHashCode();
		}
		if (FromPoint != 0)
		{
			num ^= FromPoint.GetHashCode();
		}
		if (TargetPic.Length != 0)
		{
			num ^= TargetPic.GetHashCode();
		}
		if (TargetPicVer != 0)
		{
			num ^= TargetPicVer.GetHashCode();
		}
		if (TargetHeadId != 0)
		{
			num ^= TargetHeadId.GetHashCode();
		}
		if (TargetHeadET != 0L)
		{
			num ^= TargetHeadET.GetHashCode();
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
		if (BuildId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(BuildId);
		}
		if (OpenTime != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(OpenTime);
		}
		if (CollectEndTime != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(CollectEndTime);
		}
		if (ExpireTime != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(ExpireTime);
		}
		if (RemainTime != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(RemainTime);
		}
		if (GatherUUID != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(GatherUUID);
		}
		if (GatherUid.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(GatherUid);
		}
		if (GatherAllianceId.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(GatherAllianceId);
		}
		if (FromPoint != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(FromPoint);
		}
		if (TargetPic.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(TargetPic);
		}
		if (TargetPicVer != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(TargetPicVer);
		}
		if (TargetHeadId != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(TargetHeadId);
		}
		if (TargetHeadET != 0L)
		{
			output.WriteRawTag(104);
			output.WriteInt64(TargetHeadET);
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
		if (BuildId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildId);
		}
		if (OpenTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OpenTime);
		}
		if (CollectEndTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CollectEndTime);
		}
		if (ExpireTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ExpireTime);
		}
		if (RemainTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RemainTime);
		}
		if (GatherUUID != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(GatherUUID);
		}
		if (GatherUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GatherUid);
		}
		if (GatherAllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GatherAllianceId);
		}
		if (FromPoint != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FromPoint);
		}
		if (TargetPic.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(TargetPic);
		}
		if (TargetPicVer != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TargetPicVer);
		}
		if (TargetHeadId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TargetHeadId);
		}
		if (TargetHeadET != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TargetHeadET);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MeteoritePoint other)
	{
		if (other != null)
		{
			if (other.BuildId != 0)
			{
				BuildId = other.BuildId;
			}
			if (other.OpenTime != 0)
			{
				OpenTime = other.OpenTime;
			}
			if (other.CollectEndTime != 0)
			{
				CollectEndTime = other.CollectEndTime;
			}
			if (other.ExpireTime != 0)
			{
				ExpireTime = other.ExpireTime;
			}
			if (other.RemainTime != 0)
			{
				RemainTime = other.RemainTime;
			}
			if (other.GatherUUID != 0L)
			{
				GatherUUID = other.GatherUUID;
			}
			if (other.GatherUid.Length != 0)
			{
				GatherUid = other.GatherUid;
			}
			if (other.GatherAllianceId.Length != 0)
			{
				GatherAllianceId = other.GatherAllianceId;
			}
			if (other.FromPoint != 0)
			{
				FromPoint = other.FromPoint;
			}
			if (other.TargetPic.Length != 0)
			{
				TargetPic = other.TargetPic;
			}
			if (other.TargetPicVer != 0)
			{
				TargetPicVer = other.TargetPicVer;
			}
			if (other.TargetHeadId != 0)
			{
				TargetHeadId = other.TargetHeadId;
			}
			if (other.TargetHeadET != 0L)
			{
				TargetHeadET = other.TargetHeadET;
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
				BuildId = input.ReadInt32();
				break;
			case 16u:
				OpenTime = input.ReadInt32();
				break;
			case 24u:
				CollectEndTime = input.ReadInt32();
				break;
			case 32u:
				ExpireTime = input.ReadInt32();
				break;
			case 40u:
				RemainTime = input.ReadInt32();
				break;
			case 48u:
				GatherUUID = input.ReadInt64();
				break;
			case 58u:
				GatherUid = input.ReadString();
				break;
			case 66u:
				GatherAllianceId = input.ReadString();
				break;
			case 72u:
				FromPoint = input.ReadInt32();
				break;
			case 82u:
				TargetPic = input.ReadString();
				break;
			case 88u:
				TargetPicVer = input.ReadInt32();
				break;
			case 96u:
				TargetHeadId = input.ReadInt32();
				break;
			case 104u:
				TargetHeadET = input.ReadInt64();
				break;
			}
		}
	}
}
