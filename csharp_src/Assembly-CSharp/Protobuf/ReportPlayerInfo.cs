using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ReportPlayerInfo : IMessage<ReportPlayerInfo>, IMessage, IEquatable<ReportPlayerInfo>, IDeepCloneable<ReportPlayerInfo>
{
	private static readonly MessageParser<ReportPlayerInfo> _parser = new MessageParser<ReportPlayerInfo>(() => new ReportPlayerInfo());

	private UnknownFieldSet _unknownFields;

	public const int UidFieldNumber = 1;

	private string uid_ = "";

	public const int NameFieldNumber = 2;

	private string name_ = "";

	public const int PicFieldNumber = 3;

	private string pic_ = "";

	public const int PicVerFieldNumber = 4;

	private int picVer_;

	public const int SrcServerIdFieldNumber = 5;

	private int srcServerId_;

	public const int PointIdFieldNumber = 6;

	private int pointId_;

	public const int AllianceInfoFieldNumber = 7;

	private ReportAllianceInfo allianceInfo_;

	public const int HeadFrameFieldNumber = 8;

	private int headFrame_;

	public const int CareerTypeFieldNumber = 9;

	private int careerType_;

	public const int CareerLvFieldNumber = 10;

	private int careerLv_;

	public const int LevelFieldNumber = 11;

	private int level_;

	public const int HeadSkinIdFieldNumber = 12;

	private int headSkinId_;

	public const int CurServerIdFieldNumber = 13;

	private int curServerId_;

	public const int WorldIdFieldNumber = 14;

	private int worldId_;

	public const int IsActiveAnonymityFieldNumber = 15;

	private bool isActiveAnonymity_;

	public const int WolfEndTimeFieldNumber = 16;

	private long wolfEndTime_;

	[DebuggerNonUserCode]
	public static MessageParser<ReportPlayerInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public int SrcServerId
	{
		get
		{
			return srcServerId_;
		}
		set
		{
			srcServerId_ = value;
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
	public ReportAllianceInfo AllianceInfo
	{
		get
		{
			return allianceInfo_;
		}
		set
		{
			allianceInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HeadFrame
	{
		get
		{
			return headFrame_;
		}
		set
		{
			headFrame_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CareerType
	{
		get
		{
			return careerType_;
		}
		set
		{
			careerType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CareerLv
	{
		get
		{
			return careerLv_;
		}
		set
		{
			careerLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
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
	public int CurServerId
	{
		get
		{
			return curServerId_;
		}
		set
		{
			curServerId_ = value;
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
	public bool IsActiveAnonymity
	{
		get
		{
			return isActiveAnonymity_;
		}
		set
		{
			isActiveAnonymity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long WolfEndTime
	{
		get
		{
			return wolfEndTime_;
		}
		set
		{
			wolfEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ReportPlayerInfo()
	{
	}

	[DebuggerNonUserCode]
	public ReportPlayerInfo(ReportPlayerInfo other)
		: this()
	{
		uid_ = other.uid_;
		name_ = other.name_;
		pic_ = other.pic_;
		picVer_ = other.picVer_;
		srcServerId_ = other.srcServerId_;
		pointId_ = other.pointId_;
		allianceInfo_ = ((other.allianceInfo_ != null) ? other.allianceInfo_.Clone() : null);
		headFrame_ = other.headFrame_;
		careerType_ = other.careerType_;
		careerLv_ = other.careerLv_;
		level_ = other.level_;
		headSkinId_ = other.headSkinId_;
		curServerId_ = other.curServerId_;
		worldId_ = other.worldId_;
		isActiveAnonymity_ = other.isActiveAnonymity_;
		wolfEndTime_ = other.wolfEndTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ReportPlayerInfo Clone()
	{
		return new ReportPlayerInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ReportPlayerInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ReportPlayerInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (Name != other.Name)
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
		if (SrcServerId != other.SrcServerId)
		{
			return false;
		}
		if (PointId != other.PointId)
		{
			return false;
		}
		if (!object.Equals(AllianceInfo, other.AllianceInfo))
		{
			return false;
		}
		if (HeadFrame != other.HeadFrame)
		{
			return false;
		}
		if (CareerType != other.CareerType)
		{
			return false;
		}
		if (CareerLv != other.CareerLv)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (HeadSkinId != other.HeadSkinId)
		{
			return false;
		}
		if (CurServerId != other.CurServerId)
		{
			return false;
		}
		if (WorldId != other.WorldId)
		{
			return false;
		}
		if (IsActiveAnonymity != other.IsActiveAnonymity)
		{
			return false;
		}
		if (WolfEndTime != other.WolfEndTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (Pic.Length != 0)
		{
			num ^= Pic.GetHashCode();
		}
		if (PicVer != 0)
		{
			num ^= PicVer.GetHashCode();
		}
		if (SrcServerId != 0)
		{
			num ^= SrcServerId.GetHashCode();
		}
		if (PointId != 0)
		{
			num ^= PointId.GetHashCode();
		}
		if (allianceInfo_ != null)
		{
			num ^= AllianceInfo.GetHashCode();
		}
		if (HeadFrame != 0)
		{
			num ^= HeadFrame.GetHashCode();
		}
		if (CareerType != 0)
		{
			num ^= CareerType.GetHashCode();
		}
		if (CareerLv != 0)
		{
			num ^= CareerLv.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (HeadSkinId != 0)
		{
			num ^= HeadSkinId.GetHashCode();
		}
		if (CurServerId != 0)
		{
			num ^= CurServerId.GetHashCode();
		}
		if (WorldId != 0)
		{
			num ^= WorldId.GetHashCode();
		}
		if (IsActiveAnonymity)
		{
			num ^= IsActiveAnonymity.GetHashCode();
		}
		if (WolfEndTime != 0L)
		{
			num ^= WolfEndTime.GetHashCode();
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
		if (Uid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Uid);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Name);
		}
		if (Pic.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Pic);
		}
		if (PicVer != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(PicVer);
		}
		if (SrcServerId != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(SrcServerId);
		}
		if (PointId != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(PointId);
		}
		if (allianceInfo_ != null)
		{
			output.WriteRawTag(58);
			output.WriteMessage(AllianceInfo);
		}
		if (HeadFrame != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(HeadFrame);
		}
		if (CareerType != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(CareerType);
		}
		if (CareerLv != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(CareerLv);
		}
		if (Level != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(Level);
		}
		if (HeadSkinId != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(HeadSkinId);
		}
		if (CurServerId != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(CurServerId);
		}
		if (WorldId != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(WorldId);
		}
		if (IsActiveAnonymity)
		{
			output.WriteRawTag(120);
			output.WriteBool(IsActiveAnonymity);
		}
		if (WolfEndTime != 0L)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt64(WolfEndTime);
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
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (Pic.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Pic);
		}
		if (PicVer != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PicVer);
		}
		if (SrcServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SrcServerId);
		}
		if (PointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PointId);
		}
		if (allianceInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(AllianceInfo);
		}
		if (HeadFrame != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadFrame);
		}
		if (CareerType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CareerType);
		}
		if (CareerLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CareerLv);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (HeadSkinId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadSkinId);
		}
		if (CurServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurServerId);
		}
		if (WorldId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WorldId);
		}
		if (IsActiveAnonymity)
		{
			num += 2;
		}
		if (WolfEndTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(WolfEndTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ReportPlayerInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Uid.Length != 0)
		{
			Uid = other.Uid;
		}
		if (other.Name.Length != 0)
		{
			Name = other.Name;
		}
		if (other.Pic.Length != 0)
		{
			Pic = other.Pic;
		}
		if (other.PicVer != 0)
		{
			PicVer = other.PicVer;
		}
		if (other.SrcServerId != 0)
		{
			SrcServerId = other.SrcServerId;
		}
		if (other.PointId != 0)
		{
			PointId = other.PointId;
		}
		if (other.allianceInfo_ != null)
		{
			if (allianceInfo_ == null)
			{
				AllianceInfo = new ReportAllianceInfo();
			}
			AllianceInfo.MergeFrom(other.AllianceInfo);
		}
		if (other.HeadFrame != 0)
		{
			HeadFrame = other.HeadFrame;
		}
		if (other.CareerType != 0)
		{
			CareerType = other.CareerType;
		}
		if (other.CareerLv != 0)
		{
			CareerLv = other.CareerLv;
		}
		if (other.Level != 0)
		{
			Level = other.Level;
		}
		if (other.HeadSkinId != 0)
		{
			HeadSkinId = other.HeadSkinId;
		}
		if (other.CurServerId != 0)
		{
			CurServerId = other.CurServerId;
		}
		if (other.WorldId != 0)
		{
			WorldId = other.WorldId;
		}
		if (other.IsActiveAnonymity)
		{
			IsActiveAnonymity = other.IsActiveAnonymity;
		}
		if (other.WolfEndTime != 0L)
		{
			WolfEndTime = other.WolfEndTime;
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
			case 10u:
				Uid = input.ReadString();
				break;
			case 18u:
				Name = input.ReadString();
				break;
			case 26u:
				Pic = input.ReadString();
				break;
			case 32u:
				PicVer = input.ReadInt32();
				break;
			case 40u:
				SrcServerId = input.ReadInt32();
				break;
			case 48u:
				PointId = input.ReadInt32();
				break;
			case 58u:
				if (allianceInfo_ == null)
				{
					AllianceInfo = new ReportAllianceInfo();
				}
				input.ReadMessage(AllianceInfo);
				break;
			case 64u:
				HeadFrame = input.ReadInt32();
				break;
			case 72u:
				CareerType = input.ReadInt32();
				break;
			case 80u:
				CareerLv = input.ReadInt32();
				break;
			case 88u:
				Level = input.ReadInt32();
				break;
			case 96u:
				HeadSkinId = input.ReadInt32();
				break;
			case 104u:
				CurServerId = input.ReadInt32();
				break;
			case 112u:
				WorldId = input.ReadInt32();
				break;
			case 120u:
				IsActiveAnonymity = input.ReadBool();
				break;
			case 128u:
				WolfEndTime = input.ReadInt64();
				break;
			}
		}
	}
}
