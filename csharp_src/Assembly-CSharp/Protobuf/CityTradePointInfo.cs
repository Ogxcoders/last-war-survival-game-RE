using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CityTradePointInfo : IMessage<CityTradePointInfo>, IMessage, IEquatable<CityTradePointInfo>, IDeepCloneable<CityTradePointInfo>
{
	private static readonly MessageParser<CityTradePointInfo> _parser = new MessageParser<CityTradePointInfo>(() => new CityTradePointInfo());

	private UnknownFieldSet _unknownFields;

	public const int TradeIdFieldNumber = 1;

	private int tradeId_;

	public const int StateFieldNumber = 2;

	private int state_;

	public const int UidFieldNumber = 3;

	private string uid_ = "";

	public const int UserNameFieldNumber = 4;

	private string userName_ = "";

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

	public const int ServerIdFieldNumber = 12;

	private int serverId_;

	public const int BattleStartTimeFieldNumber = 13;

	private long battleStartTime_;

	public const int BattleEndTimeFieldNumber = 14;

	private long battleEndTime_;

	public const int BuildPointInfoFieldNumber = 15;

	private static readonly FieldCodec<CityTradeBuildPointInfo> _repeated_buildPointInfo_codec = FieldCodec.ForMessage(122u, CityTradeBuildPointInfo.Parser);

	private readonly RepeatedField<CityTradeBuildPointInfo> buildPointInfo_ = new RepeatedField<CityTradeBuildPointInfo>();

	public const int TempAlAbbrFieldNumber = 16;

	private string tempAlAbbr_ = "";

	public const int TempAllianceIdFieldNumber = 17;

	private string tempAllianceId_ = "";

	public const int TempAlNameFieldNumber = 18;

	private string tempAlName_ = "";

	public const int OpenTimeFieldNumber = 19;

	private long openTime_;

	public const int ShopRefreshNumFieldNumber = 20;

	private int shopRefreshNum_;

	public const int ShopStateFieldNumber = 21;

	private int shopState_;

	public const int IconFieldNumber = 22;

	private string icon_ = "";

	public const int NightShopRefreshNumFieldNumber = 23;

	private int nightShopRefreshNum_;

	public const int NightShopStateFieldNumber = 24;

	private int nightShopState_;

	public const int GiveUpTimeFieldNumber = 25;

	private long giveUpTime_;

	[DebuggerNonUserCode]
	public static MessageParser<CityTradePointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[31];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int TradeId
	{
		get
		{
			return tradeId_;
		}
		set
		{
			tradeId_ = value;
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
	public long BattleStartTime
	{
		get
		{
			return battleStartTime_;
		}
		set
		{
			battleStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long BattleEndTime
	{
		get
		{
			return battleEndTime_;
		}
		set
		{
			battleEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<CityTradeBuildPointInfo> BuildPointInfo => buildPointInfo_;

	[DebuggerNonUserCode]
	public string TempAlAbbr
	{
		get
		{
			return tempAlAbbr_;
		}
		set
		{
			tempAlAbbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string TempAllianceId
	{
		get
		{
			return tempAllianceId_;
		}
		set
		{
			tempAllianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string TempAlName
	{
		get
		{
			return tempAlName_;
		}
		set
		{
			tempAlName_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public int ShopRefreshNum
	{
		get
		{
			return shopRefreshNum_;
		}
		set
		{
			shopRefreshNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ShopState
	{
		get
		{
			return shopState_;
		}
		set
		{
			shopState_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Icon
	{
		get
		{
			return icon_;
		}
		set
		{
			icon_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int NightShopRefreshNum
	{
		get
		{
			return nightShopRefreshNum_;
		}
		set
		{
			nightShopRefreshNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int NightShopState
	{
		get
		{
			return nightShopState_;
		}
		set
		{
			nightShopState_ = value;
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
	public CityTradePointInfo()
	{
	}

	[DebuggerNonUserCode]
	public CityTradePointInfo(CityTradePointInfo other)
		: this()
	{
		tradeId_ = other.tradeId_;
		state_ = other.state_;
		uid_ = other.uid_;
		userName_ = other.userName_;
		pic_ = other.pic_;
		picVer_ = other.picVer_;
		headSkinId_ = other.headSkinId_;
		headSkinET_ = other.headSkinET_;
		alAbbr_ = other.alAbbr_;
		allianceId_ = other.allianceId_;
		alName_ = other.alName_;
		serverId_ = other.serverId_;
		battleStartTime_ = other.battleStartTime_;
		battleEndTime_ = other.battleEndTime_;
		buildPointInfo_ = other.buildPointInfo_.Clone();
		tempAlAbbr_ = other.tempAlAbbr_;
		tempAllianceId_ = other.tempAllianceId_;
		tempAlName_ = other.tempAlName_;
		openTime_ = other.openTime_;
		shopRefreshNum_ = other.shopRefreshNum_;
		shopState_ = other.shopState_;
		icon_ = other.icon_;
		nightShopRefreshNum_ = other.nightShopRefreshNum_;
		nightShopState_ = other.nightShopState_;
		giveUpTime_ = other.giveUpTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CityTradePointInfo Clone()
	{
		return new CityTradePointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CityTradePointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CityTradePointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TradeId != other.TradeId)
		{
			return false;
		}
		if (State != other.State)
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
		if (ServerId != other.ServerId)
		{
			return false;
		}
		if (BattleStartTime != other.BattleStartTime)
		{
			return false;
		}
		if (BattleEndTime != other.BattleEndTime)
		{
			return false;
		}
		if (!buildPointInfo_.Equals(other.buildPointInfo_))
		{
			return false;
		}
		if (TempAlAbbr != other.TempAlAbbr)
		{
			return false;
		}
		if (TempAllianceId != other.TempAllianceId)
		{
			return false;
		}
		if (TempAlName != other.TempAlName)
		{
			return false;
		}
		if (OpenTime != other.OpenTime)
		{
			return false;
		}
		if (ShopRefreshNum != other.ShopRefreshNum)
		{
			return false;
		}
		if (ShopState != other.ShopState)
		{
			return false;
		}
		if (Icon != other.Icon)
		{
			return false;
		}
		if (NightShopRefreshNum != other.NightShopRefreshNum)
		{
			return false;
		}
		if (NightShopState != other.NightShopState)
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
		if (TradeId != 0)
		{
			num ^= TradeId.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (UserName.Length != 0)
		{
			num ^= UserName.GetHashCode();
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
		if (ServerId != 0)
		{
			num ^= ServerId.GetHashCode();
		}
		if (BattleStartTime != 0L)
		{
			num ^= BattleStartTime.GetHashCode();
		}
		if (BattleEndTime != 0L)
		{
			num ^= BattleEndTime.GetHashCode();
		}
		num ^= buildPointInfo_.GetHashCode();
		if (TempAlAbbr.Length != 0)
		{
			num ^= TempAlAbbr.GetHashCode();
		}
		if (TempAllianceId.Length != 0)
		{
			num ^= TempAllianceId.GetHashCode();
		}
		if (TempAlName.Length != 0)
		{
			num ^= TempAlName.GetHashCode();
		}
		if (OpenTime != 0L)
		{
			num ^= OpenTime.GetHashCode();
		}
		if (ShopRefreshNum != 0)
		{
			num ^= ShopRefreshNum.GetHashCode();
		}
		if (ShopState != 0)
		{
			num ^= ShopState.GetHashCode();
		}
		if (Icon.Length != 0)
		{
			num ^= Icon.GetHashCode();
		}
		if (NightShopRefreshNum != 0)
		{
			num ^= NightShopRefreshNum.GetHashCode();
		}
		if (NightShopState != 0)
		{
			num ^= NightShopState.GetHashCode();
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
		if (TradeId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(TradeId);
		}
		if (State != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(State);
		}
		if (Uid.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Uid);
		}
		if (UserName.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(UserName);
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
		if (ServerId != 0)
		{
			output.WriteRawTag(96);
			output.WriteInt32(ServerId);
		}
		if (BattleStartTime != 0L)
		{
			output.WriteRawTag(104);
			output.WriteInt64(BattleStartTime);
		}
		if (BattleEndTime != 0L)
		{
			output.WriteRawTag(112);
			output.WriteInt64(BattleEndTime);
		}
		buildPointInfo_.WriteTo(output, _repeated_buildPointInfo_codec);
		if (TempAlAbbr.Length != 0)
		{
			output.WriteRawTag(130, 1);
			output.WriteString(TempAlAbbr);
		}
		if (TempAllianceId.Length != 0)
		{
			output.WriteRawTag(138, 1);
			output.WriteString(TempAllianceId);
		}
		if (TempAlName.Length != 0)
		{
			output.WriteRawTag(146, 1);
			output.WriteString(TempAlName);
		}
		if (OpenTime != 0L)
		{
			output.WriteRawTag(152, 1);
			output.WriteInt64(OpenTime);
		}
		if (ShopRefreshNum != 0)
		{
			output.WriteRawTag(160, 1);
			output.WriteInt32(ShopRefreshNum);
		}
		if (ShopState != 0)
		{
			output.WriteRawTag(168, 1);
			output.WriteInt32(ShopState);
		}
		if (Icon.Length != 0)
		{
			output.WriteRawTag(178, 1);
			output.WriteString(Icon);
		}
		if (NightShopRefreshNum != 0)
		{
			output.WriteRawTag(184, 1);
			output.WriteInt32(NightShopRefreshNum);
		}
		if (NightShopState != 0)
		{
			output.WriteRawTag(192, 1);
			output.WriteInt32(NightShopState);
		}
		if (GiveUpTime != 0L)
		{
			output.WriteRawTag(200, 1);
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
		if (TradeId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TradeId);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (UserName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(UserName);
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
		if (ServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ServerId);
		}
		if (BattleStartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(BattleStartTime);
		}
		if (BattleEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(BattleEndTime);
		}
		num += buildPointInfo_.CalculateSize(_repeated_buildPointInfo_codec);
		if (TempAlAbbr.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(TempAlAbbr);
		}
		if (TempAllianceId.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(TempAllianceId);
		}
		if (TempAlName.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(TempAlName);
		}
		if (OpenTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(OpenTime);
		}
		if (ShopRefreshNum != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ShopRefreshNum);
		}
		if (ShopState != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ShopState);
		}
		if (Icon.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(Icon);
		}
		if (NightShopRefreshNum != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(NightShopRefreshNum);
		}
		if (NightShopState != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(NightShopState);
		}
		if (GiveUpTime != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(GiveUpTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CityTradePointInfo other)
	{
		if (other != null)
		{
			if (other.TradeId != 0)
			{
				TradeId = other.TradeId;
			}
			if (other.State != 0)
			{
				State = other.State;
			}
			if (other.Uid.Length != 0)
			{
				Uid = other.Uid;
			}
			if (other.UserName.Length != 0)
			{
				UserName = other.UserName;
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
			if (other.ServerId != 0)
			{
				ServerId = other.ServerId;
			}
			if (other.BattleStartTime != 0L)
			{
				BattleStartTime = other.BattleStartTime;
			}
			if (other.BattleEndTime != 0L)
			{
				BattleEndTime = other.BattleEndTime;
			}
			buildPointInfo_.Add(other.buildPointInfo_);
			if (other.TempAlAbbr.Length != 0)
			{
				TempAlAbbr = other.TempAlAbbr;
			}
			if (other.TempAllianceId.Length != 0)
			{
				TempAllianceId = other.TempAllianceId;
			}
			if (other.TempAlName.Length != 0)
			{
				TempAlName = other.TempAlName;
			}
			if (other.OpenTime != 0L)
			{
				OpenTime = other.OpenTime;
			}
			if (other.ShopRefreshNum != 0)
			{
				ShopRefreshNum = other.ShopRefreshNum;
			}
			if (other.ShopState != 0)
			{
				ShopState = other.ShopState;
			}
			if (other.Icon.Length != 0)
			{
				Icon = other.Icon;
			}
			if (other.NightShopRefreshNum != 0)
			{
				NightShopRefreshNum = other.NightShopRefreshNum;
			}
			if (other.NightShopState != 0)
			{
				NightShopState = other.NightShopState;
			}
			if (other.GiveUpTime != 0L)
			{
				GiveUpTime = other.GiveUpTime;
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
				TradeId = input.ReadInt32();
				break;
			case 16u:
				State = input.ReadInt32();
				break;
			case 26u:
				Uid = input.ReadString();
				break;
			case 34u:
				UserName = input.ReadString();
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
			case 96u:
				ServerId = input.ReadInt32();
				break;
			case 104u:
				BattleStartTime = input.ReadInt64();
				break;
			case 112u:
				BattleEndTime = input.ReadInt64();
				break;
			case 122u:
				buildPointInfo_.AddEntriesFrom(input, _repeated_buildPointInfo_codec);
				break;
			case 130u:
				TempAlAbbr = input.ReadString();
				break;
			case 138u:
				TempAllianceId = input.ReadString();
				break;
			case 146u:
				TempAlName = input.ReadString();
				break;
			case 152u:
				OpenTime = input.ReadInt64();
				break;
			case 160u:
				ShopRefreshNum = input.ReadInt32();
				break;
			case 168u:
				ShopState = input.ReadInt32();
				break;
			case 178u:
				Icon = input.ReadString();
				break;
			case 184u:
				NightShopRefreshNum = input.ReadInt32();
				break;
			case 192u:
				NightShopState = input.ReadInt32();
				break;
			case 200u:
				GiveUpTime = input.ReadInt64();
				break;
			}
		}
	}
}
