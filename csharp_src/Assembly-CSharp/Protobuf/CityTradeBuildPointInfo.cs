using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CityTradeBuildPointInfo : IMessage<CityTradeBuildPointInfo>, IMessage, IEquatable<CityTradeBuildPointInfo>, IDeepCloneable<CityTradeBuildPointInfo>
{
	private static readonly MessageParser<CityTradeBuildPointInfo> _parser = new MessageParser<CityTradeBuildPointInfo>(() => new CityTradeBuildPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int BuildPointFieldNumber = 1;

	private int buildPoint_;

	public const int BuildSpeedFieldNumber = 2;

	private int buildSpeed_;

	public const int ServerIdFieldNumber = 3;

	private int serverId_;

	public const int RefreshTimeFieldNumber = 4;

	private long refreshTime_;

	public const int UidFieldNumber = 5;

	private string uid_ = "";

	public const int UidNameFieldNumber = 6;

	private string uidName_ = "";

	public const int PicFieldNumber = 7;

	private string pic_ = "";

	public const int PicVerFieldNumber = 8;

	private int picVer_;

	public const int HeadSkinIdFieldNumber = 9;

	private int headSkinId_;

	public const int HeadSkinETFieldNumber = 10;

	private long headSkinET_;

	public const int AlAbbrFieldNumber = 11;

	private string alAbbr_ = "";

	public const int AllianceIdFieldNumber = 12;

	private string allianceId_ = "";

	public const int AlNameFieldNumber = 13;

	private string alName_ = "";

	public const int AlIconFieldNumber = 14;

	private string alIcon_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<CityTradeBuildPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[30];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int BuildPoint
	{
		get
		{
			return buildPoint_;
		}
		set
		{
			buildPoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BuildSpeed
	{
		get
		{
			return buildSpeed_;
		}
		set
		{
			buildSpeed_ = value;
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
	public long RefreshTime
	{
		get
		{
			return refreshTime_;
		}
		set
		{
			refreshTime_ = value;
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
	public string UidName
	{
		get
		{
			return uidName_;
		}
		set
		{
			uidName_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public CityTradeBuildPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public CityTradeBuildPointInfo(CityTradeBuildPointInfo other)
		: this()
	{
		buildPoint_ = other.buildPoint_;
		buildSpeed_ = other.buildSpeed_;
		serverId_ = other.serverId_;
		refreshTime_ = other.refreshTime_;
		uid_ = other.uid_;
		uidName_ = other.uidName_;
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
	public CityTradeBuildPointInfo Clone()
	{
		return new CityTradeBuildPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CityTradeBuildPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CityTradeBuildPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BuildPoint != other.BuildPoint)
		{
			return false;
		}
		if (BuildSpeed != other.BuildSpeed)
		{
			return false;
		}
		if (ServerId != other.ServerId)
		{
			return false;
		}
		if (RefreshTime != other.RefreshTime)
		{
			return false;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (UidName != other.UidName)
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
		if (BuildPoint != 0)
		{
			num ^= BuildPoint.GetHashCode();
		}
		if (BuildSpeed != 0)
		{
			num ^= BuildSpeed.GetHashCode();
		}
		if (ServerId != 0)
		{
			num ^= ServerId.GetHashCode();
		}
		if (RefreshTime != 0L)
		{
			num ^= RefreshTime.GetHashCode();
		}
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (UidName.Length != 0)
		{
			num ^= UidName.GetHashCode();
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
		if (BuildPoint != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(BuildPoint);
		}
		if (BuildSpeed != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(BuildSpeed);
		}
		if (ServerId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(ServerId);
		}
		if (RefreshTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(RefreshTime);
		}
		if (Uid.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(Uid);
		}
		if (UidName.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(UidName);
		}
		if (Pic.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(Pic);
		}
		if (PicVer != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(PicVer);
		}
		if (HeadSkinId != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(HeadSkinId);
		}
		if (HeadSkinET != 0L)
		{
			output.WriteRawTag(80);
			output.WriteInt64(HeadSkinET);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(90);
			output.WriteString(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(98);
			output.WriteString(AllianceId);
		}
		if (AlName.Length != 0)
		{
			output.WriteRawTag(106);
			output.WriteString(AlName);
		}
		if (AlIcon.Length != 0)
		{
			output.WriteRawTag(114);
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
		if (BuildPoint != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildPoint);
		}
		if (BuildSpeed != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildSpeed);
		}
		if (ServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ServerId);
		}
		if (RefreshTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(RefreshTime);
		}
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (UidName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(UidName);
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
	public void MergeFrom(CityTradeBuildPointInfo other)
	{
		if (other != null)
		{
			if (other.BuildPoint != 0)
			{
				BuildPoint = other.BuildPoint;
			}
			if (other.BuildSpeed != 0)
			{
				BuildSpeed = other.BuildSpeed;
			}
			if (other.ServerId != 0)
			{
				ServerId = other.ServerId;
			}
			if (other.RefreshTime != 0L)
			{
				RefreshTime = other.RefreshTime;
			}
			if (other.Uid.Length != 0)
			{
				Uid = other.Uid;
			}
			if (other.UidName.Length != 0)
			{
				UidName = other.UidName;
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
				BuildPoint = input.ReadInt32();
				break;
			case 16u:
				BuildSpeed = input.ReadInt32();
				break;
			case 24u:
				ServerId = input.ReadInt32();
				break;
			case 32u:
				RefreshTime = input.ReadInt64();
				break;
			case 42u:
				Uid = input.ReadString();
				break;
			case 50u:
				UidName = input.ReadString();
				break;
			case 58u:
				Pic = input.ReadString();
				break;
			case 64u:
				PicVer = input.ReadInt32();
				break;
			case 72u:
				HeadSkinId = input.ReadInt32();
				break;
			case 80u:
				HeadSkinET = input.ReadInt64();
				break;
			case 90u:
				AlAbbr = input.ReadString();
				break;
			case 98u:
				AllianceId = input.ReadString();
				break;
			case 106u:
				AlName = input.ReadString();
				break;
			case 114u:
				AlIcon = input.ReadString();
				break;
			}
		}
	}
}
