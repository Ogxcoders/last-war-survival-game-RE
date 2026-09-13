using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CityStrongholdPointInfo : IMessage<CityStrongholdPointInfo>, IMessage, IEquatable<CityStrongholdPointInfo>, IDeepCloneable<CityStrongholdPointInfo>
{
	private static readonly MessageParser<CityStrongholdPointInfo> _parser = new MessageParser<CityStrongholdPointInfo>(() => new CityStrongholdPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int StrongholdIdFieldNumber = 1;

	private int strongholdId_;

	public const int StateFieldNumber = 3;

	private int state_;

	public const int ProtectTimeFieldNumber = 4;

	private int protectTime_;

	public const int AlAbbrFieldNumber = 5;

	private string alAbbr_ = "";

	public const int AllianceIdFieldNumber = 7;

	private string allianceId_ = "";

	public const int AlNameFieldNumber = 10;

	private string alName_ = "";

	public const int GiveUpTimeFieldNumber = 11;

	private int giveUpTime_;

	public const int CityNameFieldNumber = 12;

	private string cityName_ = "";

	public const int BuildPointFieldNumber = 13;

	private int buildPoint_;

	public const int BuildStartTimeFieldNumber = 14;

	private int buildStartTime_;

	public const int ServerIdFieldNumber = 15;

	private int serverId_;

	public const int BattleStartTimeFieldNumber = 16;

	private int battleStartTime_;

	public const int BattleEndTimeFieldNumber = 17;

	private int battleEndTime_;

	public const int BuildPointInfoFieldNumber = 18;

	private StrongholdBuildPointInfo buildPointInfo_;

	public const int ThermalConductorFieldNumber = 19;

	private ThermalConductor thermalConductor_;

	public const int IconFieldNumber = 20;

	private string icon_ = "";

	public const int BankRobInfoFieldNumber = 21;

	private BankRobInfo bankRobInfo_;

	public const int WarTimeIndexFieldNumber = 22;

	private int warTimeIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<CityStrongholdPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[28];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int StrongholdId
	{
		get
		{
			return strongholdId_;
		}
		set
		{
			strongholdId_ = value;
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
	public int ProtectTime
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
	public int GiveUpTime
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
	public string CityName
	{
		get
		{
			return cityName_;
		}
		set
		{
			cityName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

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
	public int BuildStartTime
	{
		get
		{
			return buildStartTime_;
		}
		set
		{
			buildStartTime_ = value;
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
	public int BattleStartTime
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
	public int BattleEndTime
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
	public StrongholdBuildPointInfo BuildPointInfo
	{
		get
		{
			return buildPointInfo_;
		}
		set
		{
			buildPointInfo_ = value;
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
	public BankRobInfo BankRobInfo
	{
		get
		{
			return bankRobInfo_;
		}
		set
		{
			bankRobInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WarTimeIndex
	{
		get
		{
			return warTimeIndex_;
		}
		set
		{
			warTimeIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityStrongholdPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public CityStrongholdPointInfo(CityStrongholdPointInfo other)
		: this()
	{
		strongholdId_ = other.strongholdId_;
		state_ = other.state_;
		protectTime_ = other.protectTime_;
		alAbbr_ = other.alAbbr_;
		allianceId_ = other.allianceId_;
		alName_ = other.alName_;
		giveUpTime_ = other.giveUpTime_;
		cityName_ = other.cityName_;
		buildPoint_ = other.buildPoint_;
		buildStartTime_ = other.buildStartTime_;
		serverId_ = other.serverId_;
		battleStartTime_ = other.battleStartTime_;
		battleEndTime_ = other.battleEndTime_;
		buildPointInfo_ = ((other.buildPointInfo_ != null) ? other.buildPointInfo_.Clone() : null);
		thermalConductor_ = ((other.thermalConductor_ != null) ? other.thermalConductor_.Clone() : null);
		icon_ = other.icon_;
		bankRobInfo_ = ((other.bankRobInfo_ != null) ? other.bankRobInfo_.Clone() : null);
		warTimeIndex_ = other.warTimeIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CityStrongholdPointInfo Clone()
	{
		return new CityStrongholdPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CityStrongholdPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CityStrongholdPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (StrongholdId != other.StrongholdId)
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		if (ProtectTime != other.ProtectTime)
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
		if (GiveUpTime != other.GiveUpTime)
		{
			return false;
		}
		if (CityName != other.CityName)
		{
			return false;
		}
		if (BuildPoint != other.BuildPoint)
		{
			return false;
		}
		if (BuildStartTime != other.BuildStartTime)
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
		if (!object.Equals(BuildPointInfo, other.BuildPointInfo))
		{
			return false;
		}
		if (!object.Equals(ThermalConductor, other.ThermalConductor))
		{
			return false;
		}
		if (Icon != other.Icon)
		{
			return false;
		}
		if (!object.Equals(BankRobInfo, other.BankRobInfo))
		{
			return false;
		}
		if (WarTimeIndex != other.WarTimeIndex)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (StrongholdId != 0)
		{
			num ^= StrongholdId.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (ProtectTime != 0)
		{
			num ^= ProtectTime.GetHashCode();
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
		if (GiveUpTime != 0)
		{
			num ^= GiveUpTime.GetHashCode();
		}
		if (CityName.Length != 0)
		{
			num ^= CityName.GetHashCode();
		}
		if (BuildPoint != 0)
		{
			num ^= BuildPoint.GetHashCode();
		}
		if (BuildStartTime != 0)
		{
			num ^= BuildStartTime.GetHashCode();
		}
		if (ServerId != 0)
		{
			num ^= ServerId.GetHashCode();
		}
		if (BattleStartTime != 0)
		{
			num ^= BattleStartTime.GetHashCode();
		}
		if (BattleEndTime != 0)
		{
			num ^= BattleEndTime.GetHashCode();
		}
		if (buildPointInfo_ != null)
		{
			num ^= BuildPointInfo.GetHashCode();
		}
		if (thermalConductor_ != null)
		{
			num ^= ThermalConductor.GetHashCode();
		}
		if (Icon.Length != 0)
		{
			num ^= Icon.GetHashCode();
		}
		if (bankRobInfo_ != null)
		{
			num ^= BankRobInfo.GetHashCode();
		}
		if (WarTimeIndex != 0)
		{
			num ^= WarTimeIndex.GetHashCode();
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
		if (StrongholdId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(StrongholdId);
		}
		if (State != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(State);
		}
		if (ProtectTime != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(ProtectTime);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(AllianceId);
		}
		if (AlName.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(AlName);
		}
		if (GiveUpTime != 0)
		{
			output.WriteRawTag(88);
			output.WriteInt32(GiveUpTime);
		}
		if (CityName.Length != 0)
		{
			output.WriteRawTag(98);
			output.WriteString(CityName);
		}
		if (BuildPoint != 0)
		{
			output.WriteRawTag(104);
			output.WriteInt32(BuildPoint);
		}
		if (BuildStartTime != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(BuildStartTime);
		}
		if (ServerId != 0)
		{
			output.WriteRawTag(120);
			output.WriteInt32(ServerId);
		}
		if (BattleStartTime != 0)
		{
			output.WriteRawTag(128, 1);
			output.WriteInt32(BattleStartTime);
		}
		if (BattleEndTime != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(BattleEndTime);
		}
		if (buildPointInfo_ != null)
		{
			output.WriteRawTag(146, 1);
			output.WriteMessage(BuildPointInfo);
		}
		if (thermalConductor_ != null)
		{
			output.WriteRawTag(154, 1);
			output.WriteMessage(ThermalConductor);
		}
		if (Icon.Length != 0)
		{
			output.WriteRawTag(162, 1);
			output.WriteString(Icon);
		}
		if (bankRobInfo_ != null)
		{
			output.WriteRawTag(170, 1);
			output.WriteMessage(BankRobInfo);
		}
		if (WarTimeIndex != 0)
		{
			output.WriteRawTag(176, 1);
			output.WriteInt32(WarTimeIndex);
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
		if (StrongholdId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(StrongholdId);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (ProtectTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ProtectTime);
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
		if (GiveUpTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GiveUpTime);
		}
		if (CityName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CityName);
		}
		if (BuildPoint != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildPoint);
		}
		if (BuildStartTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildStartTime);
		}
		if (ServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ServerId);
		}
		if (BattleStartTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(BattleStartTime);
		}
		if (BattleEndTime != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(BattleEndTime);
		}
		if (buildPointInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(BuildPointInfo);
		}
		if (thermalConductor_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ThermalConductor);
		}
		if (Icon.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(Icon);
		}
		if (bankRobInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(BankRobInfo);
		}
		if (WarTimeIndex != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(WarTimeIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CityStrongholdPointInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.StrongholdId != 0)
		{
			StrongholdId = other.StrongholdId;
		}
		if (other.State != 0)
		{
			State = other.State;
		}
		if (other.ProtectTime != 0)
		{
			ProtectTime = other.ProtectTime;
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
		if (other.GiveUpTime != 0)
		{
			GiveUpTime = other.GiveUpTime;
		}
		if (other.CityName.Length != 0)
		{
			CityName = other.CityName;
		}
		if (other.BuildPoint != 0)
		{
			BuildPoint = other.BuildPoint;
		}
		if (other.BuildStartTime != 0)
		{
			BuildStartTime = other.BuildStartTime;
		}
		if (other.ServerId != 0)
		{
			ServerId = other.ServerId;
		}
		if (other.BattleStartTime != 0)
		{
			BattleStartTime = other.BattleStartTime;
		}
		if (other.BattleEndTime != 0)
		{
			BattleEndTime = other.BattleEndTime;
		}
		if (other.buildPointInfo_ != null)
		{
			if (buildPointInfo_ == null)
			{
				BuildPointInfo = new StrongholdBuildPointInfo();
			}
			BuildPointInfo.MergeFrom(other.BuildPointInfo);
		}
		if (other.thermalConductor_ != null)
		{
			if (thermalConductor_ == null)
			{
				ThermalConductor = new ThermalConductor();
			}
			ThermalConductor.MergeFrom(other.ThermalConductor);
		}
		if (other.Icon.Length != 0)
		{
			Icon = other.Icon;
		}
		if (other.bankRobInfo_ != null)
		{
			if (bankRobInfo_ == null)
			{
				BankRobInfo = new BankRobInfo();
			}
			BankRobInfo.MergeFrom(other.BankRobInfo);
		}
		if (other.WarTimeIndex != 0)
		{
			WarTimeIndex = other.WarTimeIndex;
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
				StrongholdId = input.ReadInt32();
				break;
			case 24u:
				State = input.ReadInt32();
				break;
			case 32u:
				ProtectTime = input.ReadInt32();
				break;
			case 42u:
				AlAbbr = input.ReadString();
				break;
			case 58u:
				AllianceId = input.ReadString();
				break;
			case 82u:
				AlName = input.ReadString();
				break;
			case 88u:
				GiveUpTime = input.ReadInt32();
				break;
			case 98u:
				CityName = input.ReadString();
				break;
			case 104u:
				BuildPoint = input.ReadInt32();
				break;
			case 112u:
				BuildStartTime = input.ReadInt32();
				break;
			case 120u:
				ServerId = input.ReadInt32();
				break;
			case 128u:
				BattleStartTime = input.ReadInt32();
				break;
			case 136u:
				BattleEndTime = input.ReadInt32();
				break;
			case 146u:
				if (buildPointInfo_ == null)
				{
					BuildPointInfo = new StrongholdBuildPointInfo();
				}
				input.ReadMessage(BuildPointInfo);
				break;
			case 154u:
				if (thermalConductor_ == null)
				{
					ThermalConductor = new ThermalConductor();
				}
				input.ReadMessage(ThermalConductor);
				break;
			case 162u:
				Icon = input.ReadString();
				break;
			case 170u:
				if (bankRobInfo_ == null)
				{
					BankRobInfo = new BankRobInfo();
				}
				input.ReadMessage(BankRobInfo);
				break;
			case 176u:
				WarTimeIndex = input.ReadInt32();
				break;
			}
		}
	}
}
