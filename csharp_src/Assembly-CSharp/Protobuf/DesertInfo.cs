using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DesertInfo : IMessage<DesertInfo>, IMessage, IEquatable<DesertInfo>, IDeepCloneable<DesertInfo>
{
	private static readonly MessageParser<DesertInfo> _parser = new MessageParser<DesertInfo>(() => new DesertInfo());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private int id_;

	public const int UuidFieldNumber = 2;

	private long uuid_;

	public const int DesertIdFieldNumber = 3;

	private int desertId_;

	public const int ProtectEndTimeFieldNumber = 4;

	private long protectEndTime_;

	public const int UidFieldNumber = 5;

	private string uid_ = "";

	public const int AllianceIdFieldNumber = 6;

	private string allianceId_ = "";

	public const int ServerIdFieldNumber = 7;

	private int serverId_;

	public const int HasAssistanceFieldNumber = 8;

	private bool hasAssistance_;

	public const int MineIdFieldNumber = 9;

	private int mineId_;

	public const int OwnerServerFieldNumber = 10;

	private int ownerServer_;

	public const int OriDesertIdFieldNumber = 11;

	private int oriDesertId_;

	public const int UserInfoFieldNumber = 12;

	private UserInfo userInfo_;

	public const int FireEndTimeFieldNumber = 13;

	private long fireEndTime_;

	public const int GiveUpTimeFieldNumber = 14;

	private long giveUpTime_;

	[DebuggerNonUserCode]
	public static MessageParser<DesertInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[41];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = value;
		}
	}

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
	public int DesertId
	{
		get
		{
			return desertId_;
		}
		set
		{
			desertId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ProtectEndTime
	{
		get
		{
			return protectEndTime_;
		}
		set
		{
			protectEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Uid
	{
		get
		{
			return uid_;
		}
		set
		{
			uid_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public int ServerId
	{
		get
		{
			return serverId_;
		}
		set
		{
			serverId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasAssistance
	{
		get
		{
			return hasAssistance_;
		}
		set
		{
			hasAssistance_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MineId
	{
		get
		{
			return mineId_;
		}
		set
		{
			mineId_ = value;
		}
	}

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
	public int OriDesertId
	{
		get
		{
			return oriDesertId_;
		}
		set
		{
			oriDesertId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public UserInfo UserInfo
	{
		get
		{
			return userInfo_;
		}
		set
		{
			userInfo_ = value;
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
	public long GiveUpTime
	{
		get
		{
			return giveUpTime_;
		}
		set
		{
			giveUpTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public DesertInfo()
	{
	}

	[DebuggerNonUserCode]
	public DesertInfo(DesertInfo other)
		: this()
	{
		id_ = other.id_;
		uuid_ = other.uuid_;
		desertId_ = other.desertId_;
		protectEndTime_ = other.protectEndTime_;
		uid_ = other.uid_;
		allianceId_ = other.allianceId_;
		serverId_ = other.serverId_;
		hasAssistance_ = other.hasAssistance_;
		mineId_ = other.mineId_;
		ownerServer_ = other.ownerServer_;
		oriDesertId_ = other.oriDesertId_;
		userInfo_ = ((other.userInfo_ != null) ? other.userInfo_.Clone() : null);
		fireEndTime_ = other.fireEndTime_;
		giveUpTime_ = other.giveUpTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DesertInfo Clone()
	{
		return new DesertInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DesertInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(DesertInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (DesertId != other.DesertId)
		{
			return false;
		}
		if (ProtectEndTime != other.ProtectEndTime)
		{
			return false;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (ServerId != other.ServerId)
		{
			return false;
		}
		if (HasAssistance != other.HasAssistance)
		{
			return false;
		}
		if (MineId != other.MineId)
		{
			return false;
		}
		if (OwnerServer != other.OwnerServer)
		{
			return false;
		}
		if (OriDesertId != other.OriDesertId)
		{
			return false;
		}
		if (!object.Equals(UserInfo, other.UserInfo))
		{
			return false;
		}
		if (FireEndTime != other.FireEndTime)
		{
			return false;
		}
		if (GiveUpTime != other.GiveUpTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Id != 0)
		{
			num ^= Id.GetHashCode();
		}
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (DesertId != 0)
		{
			num ^= DesertId.GetHashCode();
		}
		if (ProtectEndTime != 0L)
		{
			num ^= ProtectEndTime.GetHashCode();
		}
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (ServerId != 0)
		{
			num ^= ServerId.GetHashCode();
		}
		if (HasAssistance)
		{
			num ^= HasAssistance.GetHashCode();
		}
		if (MineId != 0)
		{
			num ^= MineId.GetHashCode();
		}
		if (OwnerServer != 0)
		{
			num ^= OwnerServer.GetHashCode();
		}
		if (OriDesertId != 0)
		{
			num ^= OriDesertId.GetHashCode();
		}
		if (userInfo_ != null)
		{
			num ^= UserInfo.GetHashCode();
		}
		if (FireEndTime != 0L)
		{
			num ^= FireEndTime.GetHashCode();
		}
		if (GiveUpTime != 0L)
		{
			num ^= GiveUpTime.GetHashCode();
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
		if (Id != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Id);
		}
		if (Uuid != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Uuid);
		}
		if (DesertId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(DesertId);
		}
		if (ProtectEndTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(ProtectEndTime);
		}
		if (Uid.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(Uid);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(AllianceId);
		}
		if (ServerId != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(ServerId);
		}
		if (HasAssistance)
		{
			output.WriteRawTag(64);
			output.WriteBool(HasAssistance);
		}
		if (MineId != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(MineId);
		}
		if (OwnerServer != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(OwnerServer);
		}
		if (OriDesertId != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(OriDesertId);
		}
		if (userInfo_ != null)
		{
			output.WriteRawTag(98);
			output.WriteMessage(UserInfo);
		}
		if (FireEndTime != 0L)
		{
			output.WriteRawTag(104);
			output.WriteInt64(FireEndTime);
		}
		if (GiveUpTime != 0L)
		{
			output.WriteRawTag(112);
			output.WriteInt64(GiveUpTime);
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
		if (Id != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Id);
		}
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (DesertId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DesertId);
		}
		if (ProtectEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ProtectEndTime);
		}
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (ServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ServerId);
		}
		if (HasAssistance)
		{
			num += 2;
		}
		if (MineId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MineId);
		}
		if (OwnerServer != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OwnerServer);
		}
		if (OriDesertId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OriDesertId);
		}
		if (userInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(UserInfo);
		}
		if (FireEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FireEndTime);
		}
		if (GiveUpTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(GiveUpTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DesertInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Id != 0)
		{
			Id = other.Id;
		}
		if (other.Uuid != 0L)
		{
			Uuid = other.Uuid;
		}
		if (other.DesertId != 0)
		{
			DesertId = other.DesertId;
		}
		if (other.ProtectEndTime != 0L)
		{
			ProtectEndTime = other.ProtectEndTime;
		}
		if (other.Uid.Length != 0)
		{
			Uid = other.Uid;
		}
		if (other.AllianceId.Length != 0)
		{
			AllianceId = other.AllianceId;
		}
		if (other.ServerId != 0)
		{
			ServerId = other.ServerId;
		}
		if (other.HasAssistance)
		{
			HasAssistance = other.HasAssistance;
		}
		if (other.MineId != 0)
		{
			MineId = other.MineId;
		}
		if (other.OwnerServer != 0)
		{
			OwnerServer = other.OwnerServer;
		}
		if (other.OriDesertId != 0)
		{
			OriDesertId = other.OriDesertId;
		}
		if (other.userInfo_ != null)
		{
			if (userInfo_ == null)
			{
				UserInfo = new UserInfo();
			}
			UserInfo.MergeFrom(other.UserInfo);
		}
		if (other.FireEndTime != 0L)
		{
			FireEndTime = other.FireEndTime;
		}
		if (other.GiveUpTime != 0L)
		{
			GiveUpTime = other.GiveUpTime;
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
				Id = input.ReadInt32();
				break;
			case 16u:
				Uuid = input.ReadInt64();
				break;
			case 24u:
				DesertId = input.ReadInt32();
				break;
			case 32u:
				ProtectEndTime = input.ReadInt64();
				break;
			case 42u:
				Uid = input.ReadString();
				break;
			case 50u:
				AllianceId = input.ReadString();
				break;
			case 56u:
				ServerId = input.ReadInt32();
				break;
			case 64u:
				HasAssistance = input.ReadBool();
				break;
			case 72u:
				MineId = input.ReadInt32();
				break;
			case 80u:
				OwnerServer = input.ReadInt32();
				break;
			case 88u:
				OriDesertId = input.ReadInt32();
				break;
			case 98u:
				if (userInfo_ == null)
				{
					UserInfo = new UserInfo();
				}
				input.ReadMessage(UserInfo);
				break;
			case 104u:
				FireEndTime = input.ReadInt64();
				break;
			case 112u:
				GiveUpTime = input.ReadInt64();
				break;
			}
		}
	}
}
