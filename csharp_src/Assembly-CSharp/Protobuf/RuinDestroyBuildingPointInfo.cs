using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class RuinDestroyBuildingPointInfo : IMessage<RuinDestroyBuildingPointInfo>, IMessage, IEquatable<RuinDestroyBuildingPointInfo>, IDeepCloneable<RuinDestroyBuildingPointInfo>
{
	private static readonly MessageParser<RuinDestroyBuildingPointInfo> _parser = new MessageParser<RuinDestroyBuildingPointInfo>(() => new RuinDestroyBuildingPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int ExpireTimeFieldNumber = 1;

	private long expireTime_;

	public const int UidFieldNumber = 2;

	private string uid_ = "";

	public const int UserNameFieldNumber = 3;

	private string userName_ = "";

	public const int PlayerServerIdFieldNumber = 4;

	private int playerServerId_;

	public const int PicFieldNumber = 5;

	private string pic_ = "";

	public const int PicVerFieldNumber = 6;

	private int picVer_;

	public const int HeadSkinIdFieldNumber = 7;

	private int headSkinId_;

	public const int HeadSkinETFieldNumber = 8;

	private long headSkinET_;

	public const int AlAbbrFieldNumber = 9;

	private string alAbbr_ = "";

	public const int AllianceIdFieldNumber = 10;

	private string allianceId_ = "";

	public const int AlNameFieldNumber = 11;

	private string alName_ = "";

	public const int AlIconFieldNumber = 12;

	private string alIcon_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RuinDestroyBuildingPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[26];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public string UserName
	{
		get
		{
			return userName_;
		}
		set
		{
			userName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int PlayerServerId
	{
		get
		{
			return playerServerId_;
		}
		set
		{
			playerServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Pic
	{
		get
		{
			return pic_;
		}
		set
		{
			pic_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int PicVer
	{
		get
		{
			return picVer_;
		}
		set
		{
			picVer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeadSkinId
	{
		get
		{
			return headSkinId_;
		}
		set
		{
			headSkinId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long HeadSkinET
	{
		get
		{
			return headSkinET_;
		}
		set
		{
			headSkinET_ = value;
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
	public string AlName
	{
		get
		{
			return alName_;
		}
		set
		{
			alName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AlIcon
	{
		get
		{
			return alIcon_;
		}
		set
		{
			alIcon_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public RuinDestroyBuildingPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public RuinDestroyBuildingPointInfo(RuinDestroyBuildingPointInfo other)
		: this()
	{
		expireTime_ = other.expireTime_;
		uid_ = other.uid_;
		userName_ = other.userName_;
		playerServerId_ = other.playerServerId_;
		pic_ = other.pic_;
		picVer_ = other.picVer_;
		headSkinId_ = other.headSkinId_;
		headSkinET_ = other.headSkinET_;
		alAbbr_ = other.alAbbr_;
		allianceId_ = other.allianceId_;
		alName_ = other.alName_;
		alIcon_ = other.alIcon_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RuinDestroyBuildingPointInfo Clone()
	{
		return new RuinDestroyBuildingPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RuinDestroyBuildingPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(RuinDestroyBuildingPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ExpireTime != other.ExpireTime)
		{
			return false;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (UserName != other.UserName)
		{
			return false;
		}
		if (PlayerServerId != other.PlayerServerId)
		{
			return false;
		}
		if (Pic != other.Pic)
		{
			return false;
		}
		if (PicVer != other.PicVer)
		{
			return false;
		}
		if (HeadSkinId != other.HeadSkinId)
		{
			return false;
		}
		if (HeadSkinET != other.HeadSkinET)
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
		if (AlName != other.AlName)
		{
			return false;
		}
		if (AlIcon != other.AlIcon)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ExpireTime != 0L)
		{
			num ^= ExpireTime.GetHashCode();
		}
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (UserName.Length != 0)
		{
			num ^= UserName.GetHashCode();
		}
		if (PlayerServerId != 0)
		{
			num ^= PlayerServerId.GetHashCode();
		}
		if (Pic.Length != 0)
		{
			num ^= Pic.GetHashCode();
		}
		if (PicVer != 0)
		{
			num ^= PicVer.GetHashCode();
		}
		if (HeadSkinId != 0)
		{
			num ^= HeadSkinId.GetHashCode();
		}
		if (HeadSkinET != 0L)
		{
			num ^= HeadSkinET.GetHashCode();
		}
		if (AlAbbr.Length != 0)
		{
			num ^= AlAbbr.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (AlName.Length != 0)
		{
			num ^= AlName.GetHashCode();
		}
		if (AlIcon.Length != 0)
		{
			num ^= AlIcon.GetHashCode();
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
		if (ExpireTime != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(ExpireTime);
		}
		if (Uid.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Uid);
		}
		if (UserName.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(UserName);
		}
		if (PlayerServerId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(PlayerServerId);
		}
		if (Pic.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(Pic);
		}
		if (PicVer != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(PicVer);
		}
		if (HeadSkinId != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(HeadSkinId);
		}
		if (HeadSkinET != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(HeadSkinET);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(74);
			output.WriteString(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(AllianceId);
		}
		if (AlName.Length != 0)
		{
			output.WriteRawTag(90);
			output.WriteString(AlName);
		}
		if (AlIcon.Length != 0)
		{
			output.WriteRawTag(98);
			output.WriteString(AlIcon);
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
		if (ExpireTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ExpireTime);
		}
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (UserName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(UserName);
		}
		if (PlayerServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PlayerServerId);
		}
		if (Pic.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Pic);
		}
		if (PicVer != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PicVer);
		}
		if (HeadSkinId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadSkinId);
		}
		if (HeadSkinET != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(HeadSkinET);
		}
		if (AlAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (AlName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlName);
		}
		if (AlIcon.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlIcon);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RuinDestroyBuildingPointInfo other)
	{
		if (other != null)
		{
			if (other.ExpireTime != 0L)
			{
				ExpireTime = other.ExpireTime;
			}
			if (other.Uid.Length != 0)
			{
				Uid = other.Uid;
			}
			if (other.UserName.Length != 0)
			{
				UserName = other.UserName;
			}
			if (other.PlayerServerId != 0)
			{
				PlayerServerId = other.PlayerServerId;
			}
			if (other.Pic.Length != 0)
			{
				Pic = other.Pic;
			}
			if (other.PicVer != 0)
			{
				PicVer = other.PicVer;
			}
			if (other.HeadSkinId != 0)
			{
				HeadSkinId = other.HeadSkinId;
			}
			if (other.HeadSkinET != 0L)
			{
				HeadSkinET = other.HeadSkinET;
			}
			if (other.AlAbbr.Length != 0)
			{
				AlAbbr = other.AlAbbr;
			}
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.AlName.Length != 0)
			{
				AlName = other.AlName;
			}
			if (other.AlIcon.Length != 0)
			{
				AlIcon = other.AlIcon;
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
				ExpireTime = input.ReadInt64();
				break;
			case 18u:
				Uid = input.ReadString();
				break;
			case 26u:
				UserName = input.ReadString();
				break;
			case 32u:
				PlayerServerId = input.ReadInt32();
				break;
			case 42u:
				Pic = input.ReadString();
				break;
			case 48u:
				PicVer = input.ReadInt32();
				break;
			case 56u:
				HeadSkinId = input.ReadInt32();
				break;
			case 64u:
				HeadSkinET = input.ReadInt64();
				break;
			case 74u:
				AlAbbr = input.ReadString();
				break;
			case 82u:
				AllianceId = input.ReadString();
				break;
			case 90u:
				AlName = input.ReadString();
				break;
			case 98u:
				AlIcon = input.ReadString();
				break;
			}
		}
	}
}
