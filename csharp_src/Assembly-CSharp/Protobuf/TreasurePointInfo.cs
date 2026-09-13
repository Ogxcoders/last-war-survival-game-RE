using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class TreasurePointInfo : IMessage<TreasurePointInfo>, IMessage, IEquatable<TreasurePointInfo>, IDeepCloneable<TreasurePointInfo>
{
	private static readonly MessageParser<TreasurePointInfo> _parser = new MessageParser<TreasurePointInfo>(() => new TreasurePointInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int OwnerUidFieldNumber = 2;

	private string ownerUid_ = "";

	public const int EventIdFieldNumber = 3;

	private string eventId_ = "";

	public const int CompletionTimeFieldNumber = 4;

	private long completionTime_;

	public const int AllianceIdFieldNumber = 5;

	private string allianceId_ = "";

	public const int AllianceAbbrFieldNumber = 6;

	private string allianceAbbr_ = "";

	public const int RewardUserListFieldNumber = 7;

	private static readonly FieldCodec<string> _repeated_rewardUserList_codec = FieldCodec.ForString(58u);

	private readonly RepeatedField<string> rewardUserList_ = new RepeatedField<string>();

	public const int DiggingUserListFieldNumber = 8;

	private static readonly FieldCodec<UserInfo> _repeated_diggingUserList_codec = FieldCodec.ForMessage(66u, UserInfo.Parser);

	private readonly RepeatedField<UserInfo> diggingUserList_ = new RepeatedField<UserInfo>();

	public const int StartTimeFieldNumber = 9;

	private long startTime_;

	public const int CompleteFieldNumber = 10;

	private bool complete_;

	public const int SpeedFieldNumber = 11;

	private float speed_;

	public const int OwnerNameFieldNumber = 12;

	private string ownerName_ = "";

	public const int ExpireTimeFieldNumber = 13;

	private long expireTime_;

	public const int TypeFieldNumber = 14;

	private int type_;

	public const int CreateTimeFieldNumber = 15;

	private long createTime_;

	public const int ThermalConductorFieldNumber = 16;

	private ThermalConductor thermalConductor_;

	public const int FromPointFieldNumber = 17;

	private int fromPoint_;

	public const int MultipleFieldNumber = 18;

	private int multiple_;

	public const int KillerIdFieldNumber = 19;

	private string killerId_ = "";

	public const int CustomInfoFieldNumber = 20;

	private string customInfo_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<TreasurePointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[51];

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
	public string EventId
	{
		get
		{
			return eventId_;
		}
		set
		{
			eventId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public string AllianceAbbr
	{
		get
		{
			return allianceAbbr_;
		}
		set
		{
			allianceAbbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<string> RewardUserList => rewardUserList_;

	[DebuggerNonUserCode]
	public RepeatedField<UserInfo> DiggingUserList => diggingUserList_;

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
	public bool Complete
	{
		get
		{
			return complete_;
		}
		set
		{
			complete_ = value;
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
	public string OwnerName
	{
		get
		{
			return ownerName_;
		}
		set
		{
			ownerName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public long ExpireTime
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
	public long CreateTime
	{
		get
		{
			return createTime_;
		}
		set
		{
			createTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ThermalConductor ThermalConductor
	{
		get
		{
			return thermalConductor_;
		}
		set
		{
			thermalConductor_ = value;
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
	public int Multiple
	{
		get
		{
			return multiple_;
		}
		set
		{
			multiple_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string KillerId
	{
		get
		{
			return killerId_;
		}
		set
		{
			killerId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string CustomInfo
	{
		get
		{
			return customInfo_;
		}
		set
		{
			customInfo_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public TreasurePointInfo()
	{
	}

	[DebuggerNonUserCode]
	public TreasurePointInfo(TreasurePointInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		ownerUid_ = other.ownerUid_;
		eventId_ = other.eventId_;
		completionTime_ = other.completionTime_;
		allianceId_ = other.allianceId_;
		allianceAbbr_ = other.allianceAbbr_;
		rewardUserList_ = other.rewardUserList_.Clone();
		diggingUserList_ = other.diggingUserList_.Clone();
		startTime_ = other.startTime_;
		complete_ = other.complete_;
		speed_ = other.speed_;
		ownerName_ = other.ownerName_;
		expireTime_ = other.expireTime_;
		type_ = other.type_;
		createTime_ = other.createTime_;
		thermalConductor_ = ((other.thermalConductor_ != null) ? other.thermalConductor_.Clone() : null);
		fromPoint_ = other.fromPoint_;
		multiple_ = other.multiple_;
		killerId_ = other.killerId_;
		customInfo_ = other.customInfo_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TreasurePointInfo Clone()
	{
		return new TreasurePointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TreasurePointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(TreasurePointInfo other)
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
		if (OwnerUid != other.OwnerUid)
		{
			return false;
		}
		if (EventId != other.EventId)
		{
			return false;
		}
		if (CompletionTime != other.CompletionTime)
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (AllianceAbbr != other.AllianceAbbr)
		{
			return false;
		}
		if (!rewardUserList_.Equals(other.rewardUserList_))
		{
			return false;
		}
		if (!diggingUserList_.Equals(other.diggingUserList_))
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (Complete != other.Complete)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Speed, other.Speed))
		{
			return false;
		}
		if (OwnerName != other.OwnerName)
		{
			return false;
		}
		if (ExpireTime != other.ExpireTime)
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (CreateTime != other.CreateTime)
		{
			return false;
		}
		if (!object.Equals(ThermalConductor, other.ThermalConductor))
		{
			return false;
		}
		if (FromPoint != other.FromPoint)
		{
			return false;
		}
		if (Multiple != other.Multiple)
		{
			return false;
		}
		if (KillerId != other.KillerId)
		{
			return false;
		}
		if (CustomInfo != other.CustomInfo)
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
		if (OwnerUid.Length != 0)
		{
			num ^= OwnerUid.GetHashCode();
		}
		if (EventId.Length != 0)
		{
			num ^= EventId.GetHashCode();
		}
		if (CompletionTime != 0L)
		{
			num ^= CompletionTime.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (AllianceAbbr.Length != 0)
		{
			num ^= AllianceAbbr.GetHashCode();
		}
		num ^= rewardUserList_.GetHashCode();
		num ^= diggingUserList_.GetHashCode();
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (Complete)
		{
			num ^= Complete.GetHashCode();
		}
		if (Speed != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Speed);
		}
		if (OwnerName.Length != 0)
		{
			num ^= OwnerName.GetHashCode();
		}
		if (ExpireTime != 0L)
		{
			num ^= ExpireTime.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (CreateTime != 0L)
		{
			num ^= CreateTime.GetHashCode();
		}
		if (thermalConductor_ != null)
		{
			num ^= ThermalConductor.GetHashCode();
		}
		if (FromPoint != 0)
		{
			num ^= FromPoint.GetHashCode();
		}
		if (Multiple != 0)
		{
			num ^= Multiple.GetHashCode();
		}
		if (KillerId.Length != 0)
		{
			num ^= KillerId.GetHashCode();
		}
		if (CustomInfo.Length != 0)
		{
			num ^= CustomInfo.GetHashCode();
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
		if (OwnerUid.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(OwnerUid);
		}
		if (EventId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(EventId);
		}
		if (CompletionTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(CompletionTime);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(AllianceId);
		}
		if (AllianceAbbr.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(AllianceAbbr);
		}
		rewardUserList_.WriteTo(output, _repeated_rewardUserList_codec);
		diggingUserList_.WriteTo(output, _repeated_diggingUserList_codec);
		if (StartTime != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(StartTime);
		}
		if (Complete)
		{
			output.WriteRawTag(80);
			output.WriteBool(Complete);
		}
		if (Speed != 0f)
		{
			output.WriteRawTag(93);
			output.WriteFloat(Speed);
		}
		if (OwnerName.Length != 0)
		{
			output.WriteRawTag(98);
			output.WriteString(OwnerName);
		}
		if (ExpireTime != 0L)
		{
			output.WriteRawTag(104);
			output.WriteInt64(ExpireTime);
		}
		if (Type != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(Type);
		}
		if (CreateTime != 0L)
		{
			output.WriteRawTag(120);
			output.WriteInt64(CreateTime);
		}
		if (thermalConductor_ != null)
		{
			output.WriteRawTag(130, 1);
			output.WriteMessage(ThermalConductor);
		}
		if (FromPoint != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(FromPoint);
		}
		if (Multiple != 0)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt32(Multiple);
		}
		if (KillerId.Length != 0)
		{
			output.WriteRawTag(154, 1);
			output.WriteString(KillerId);
		}
		if (CustomInfo.Length != 0)
		{
			output.WriteRawTag(162, 1);
			output.WriteString(CustomInfo);
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
		if (OwnerUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(OwnerUid);
		}
		if (EventId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(EventId);
		}
		if (CompletionTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CompletionTime);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (AllianceAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceAbbr);
		}
		num += rewardUserList_.CalculateSize(_repeated_rewardUserList_codec);
		num += diggingUserList_.CalculateSize(_repeated_diggingUserList_codec);
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (Complete)
		{
			num += 2;
		}
		if (Speed != 0f)
		{
			num += 5;
		}
		if (OwnerName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(OwnerName);
		}
		if (ExpireTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ExpireTime);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (CreateTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CreateTime);
		}
		if (thermalConductor_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ThermalConductor);
		}
		if (FromPoint != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(FromPoint);
		}
		if (Multiple != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Multiple);
		}
		if (KillerId.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(KillerId);
		}
		if (CustomInfo.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(CustomInfo);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TreasurePointInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Uuid != 0L)
		{
			Uuid = other.Uuid;
		}
		if (other.OwnerUid.Length != 0)
		{
			OwnerUid = other.OwnerUid;
		}
		if (other.EventId.Length != 0)
		{
			EventId = other.EventId;
		}
		if (other.CompletionTime != 0L)
		{
			CompletionTime = other.CompletionTime;
		}
		if (other.AllianceId.Length != 0)
		{
			AllianceId = other.AllianceId;
		}
		if (other.AllianceAbbr.Length != 0)
		{
			AllianceAbbr = other.AllianceAbbr;
		}
		rewardUserList_.Add(other.rewardUserList_);
		diggingUserList_.Add(other.diggingUserList_);
		if (other.StartTime != 0L)
		{
			StartTime = other.StartTime;
		}
		if (other.Complete)
		{
			Complete = other.Complete;
		}
		if (other.Speed != 0f)
		{
			Speed = other.Speed;
		}
		if (other.OwnerName.Length != 0)
		{
			OwnerName = other.OwnerName;
		}
		if (other.ExpireTime != 0L)
		{
			ExpireTime = other.ExpireTime;
		}
		if (other.Type != 0)
		{
			Type = other.Type;
		}
		if (other.CreateTime != 0L)
		{
			CreateTime = other.CreateTime;
		}
		if (other.thermalConductor_ != null)
		{
			if (thermalConductor_ == null)
			{
				ThermalConductor = new ThermalConductor();
			}
			ThermalConductor.MergeFrom(other.ThermalConductor);
		}
		if (other.FromPoint != 0)
		{
			FromPoint = other.FromPoint;
		}
		if (other.Multiple != 0)
		{
			Multiple = other.Multiple;
		}
		if (other.KillerId.Length != 0)
		{
			KillerId = other.KillerId;
		}
		if (other.CustomInfo.Length != 0)
		{
			CustomInfo = other.CustomInfo;
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
			case 18u:
				OwnerUid = input.ReadString();
				break;
			case 26u:
				EventId = input.ReadString();
				break;
			case 32u:
				CompletionTime = input.ReadInt64();
				break;
			case 42u:
				AllianceId = input.ReadString();
				break;
			case 50u:
				AllianceAbbr = input.ReadString();
				break;
			case 58u:
				rewardUserList_.AddEntriesFrom(input, _repeated_rewardUserList_codec);
				break;
			case 66u:
				diggingUserList_.AddEntriesFrom(input, _repeated_diggingUserList_codec);
				break;
			case 72u:
				StartTime = input.ReadInt64();
				break;
			case 80u:
				Complete = input.ReadBool();
				break;
			case 93u:
				Speed = input.ReadFloat();
				break;
			case 98u:
				OwnerName = input.ReadString();
				break;
			case 104u:
				ExpireTime = input.ReadInt64();
				break;
			case 112u:
				Type = input.ReadInt32();
				break;
			case 120u:
				CreateTime = input.ReadInt64();
				break;
			case 130u:
				if (thermalConductor_ == null)
				{
					ThermalConductor = new ThermalConductor();
				}
				input.ReadMessage(ThermalConductor);
				break;
			case 136u:
				FromPoint = input.ReadInt32();
				break;
			case 144u:
				Multiple = input.ReadInt32();
				break;
			case 154u:
				KillerId = input.ReadString();
				break;
			case 162u:
				CustomInfo = input.ReadString();
				break;
			}
		}
	}
}
