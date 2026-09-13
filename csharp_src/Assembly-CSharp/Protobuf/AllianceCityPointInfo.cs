using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceCityPointInfo : IMessage<AllianceCityPointInfo>, IMessage, IEquatable<AllianceCityPointInfo>, IDeepCloneable<AllianceCityPointInfo>
{
	private static readonly MessageParser<AllianceCityPointInfo> _parser = new MessageParser<AllianceCityPointInfo>(() => new AllianceCityPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int CityIdFieldNumber = 1;

	private int cityId_;

	public const int OpenTimeFieldNumber = 2;

	private int openTime_;

	public const int StateFieldNumber = 3;

	private int state_;

	public const int ProtectTimeFieldNumber = 4;

	private int protectTime_;

	public const int AlAbbrFieldNumber = 5;

	private string alAbbr_ = "";

	public const int AllianceIdFieldNumber = 7;

	private string allianceId_ = "";

	public const int DurabilityFieldNumber = 8;

	private int durability_;

	public const int LastDurabilityTimeFieldNumber = 9;

	private int lastDurabilityTime_;

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

	public const int BuildPointInfoFieldNumber = 16;

	private static readonly FieldCodec<BuildPointInfo> _repeated_buildPointInfo_codec = FieldCodec.ForMessage(130u, Protobuf.BuildPointInfo.Parser);

	private readonly RepeatedField<BuildPointInfo> buildPointInfo_ = new RepeatedField<BuildPointInfo>();

	public const int SoldierRemainFieldNumber = 17;

	private int soldierRemain_;

	public const int ThermalConductorFieldNumber = 18;

	private ThermalConductor thermalConductor_;

	public const int ProductInfoFieldNumber = 19;

	private productInfo productInfo_;

	public const int IconFieldNumber = 20;

	private string icon_ = "";

	public const int StrategicAreaInfoFieldNumber = 21;

	private StrategicAreaInfo strategicAreaInfo_;

	public const int WarTimeIndexFieldNumber = 22;

	private int warTimeIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<AllianceCityPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[20];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int CityId
	{
		get
		{
			return cityId_;
		}
		set
		{
			cityId_ = value;
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
	public int Durability
	{
		get
		{
			return durability_;
		}
		set
		{
			durability_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LastDurabilityTime
	{
		get
		{
			return lastDurabilityTime_;
		}
		set
		{
			lastDurabilityTime_ = value;
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
	public RepeatedField<BuildPointInfo> BuildPointInfo => buildPointInfo_;

	[DebuggerNonUserCode]
	public int SoldierRemain
	{
		get
		{
			return soldierRemain_;
		}
		set
		{
			soldierRemain_ = value;
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
	public productInfo ProductInfo
	{
		get
		{
			return productInfo_;
		}
		set
		{
			productInfo_ = value;
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
	public StrategicAreaInfo StrategicAreaInfo
	{
		get
		{
			return strategicAreaInfo_;
		}
		set
		{
			strategicAreaInfo_ = value;
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
	public AllianceCityPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public AllianceCityPointInfo(AllianceCityPointInfo other)
		: this()
	{
		cityId_ = other.cityId_;
		openTime_ = other.openTime_;
		state_ = other.state_;
		protectTime_ = other.protectTime_;
		alAbbr_ = other.alAbbr_;
		allianceId_ = other.allianceId_;
		durability_ = other.durability_;
		lastDurabilityTime_ = other.lastDurabilityTime_;
		alName_ = other.alName_;
		giveUpTime_ = other.giveUpTime_;
		cityName_ = other.cityName_;
		buildPoint_ = other.buildPoint_;
		buildStartTime_ = other.buildStartTime_;
		serverId_ = other.serverId_;
		buildPointInfo_ = other.buildPointInfo_.Clone();
		soldierRemain_ = other.soldierRemain_;
		thermalConductor_ = ((other.thermalConductor_ != null) ? other.thermalConductor_.Clone() : null);
		productInfo_ = ((other.productInfo_ != null) ? other.productInfo_.Clone() : null);
		icon_ = other.icon_;
		strategicAreaInfo_ = ((other.strategicAreaInfo_ != null) ? other.strategicAreaInfo_.Clone() : null);
		warTimeIndex_ = other.warTimeIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceCityPointInfo Clone()
	{
		return new AllianceCityPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceCityPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceCityPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CityId != other.CityId)
		{
			return false;
		}
		if (OpenTime != other.OpenTime)
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
		if (Durability != other.Durability)
		{
			return false;
		}
		if (LastDurabilityTime != other.LastDurabilityTime)
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
		if (!buildPointInfo_.Equals(other.buildPointInfo_))
		{
			return false;
		}
		if (SoldierRemain != other.SoldierRemain)
		{
			return false;
		}
		if (!object.Equals(ThermalConductor, other.ThermalConductor))
		{
			return false;
		}
		if (!object.Equals(ProductInfo, other.ProductInfo))
		{
			return false;
		}
		if (Icon != other.Icon)
		{
			return false;
		}
		if (!object.Equals(StrategicAreaInfo, other.StrategicAreaInfo))
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
		if (CityId != 0)
		{
			num ^= CityId.GetHashCode();
		}
		if (OpenTime != 0)
		{
			num ^= OpenTime.GetHashCode();
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
		if (Durability != 0)
		{
			num ^= Durability.GetHashCode();
		}
		if (LastDurabilityTime != 0)
		{
			num ^= LastDurabilityTime.GetHashCode();
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
		num ^= buildPointInfo_.GetHashCode();
		if (SoldierRemain != 0)
		{
			num ^= SoldierRemain.GetHashCode();
		}
		if (thermalConductor_ != null)
		{
			num ^= ThermalConductor.GetHashCode();
		}
		if (productInfo_ != null)
		{
			num ^= ProductInfo.GetHashCode();
		}
		if (Icon.Length != 0)
		{
			num ^= Icon.GetHashCode();
		}
		if (strategicAreaInfo_ != null)
		{
			num ^= StrategicAreaInfo.GetHashCode();
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
		if (CityId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(CityId);
		}
		if (OpenTime != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(OpenTime);
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
		if (Durability != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(Durability);
		}
		if (LastDurabilityTime != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(LastDurabilityTime);
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
		buildPointInfo_.WriteTo(output, _repeated_buildPointInfo_codec);
		if (SoldierRemain != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(SoldierRemain);
		}
		if (thermalConductor_ != null)
		{
			output.WriteRawTag(146, 1);
			output.WriteMessage(ThermalConductor);
		}
		if (productInfo_ != null)
		{
			output.WriteRawTag(154, 1);
			output.WriteMessage(ProductInfo);
		}
		if (Icon.Length != 0)
		{
			output.WriteRawTag(162, 1);
			output.WriteString(Icon);
		}
		if (strategicAreaInfo_ != null)
		{
			output.WriteRawTag(170, 1);
			output.WriteMessage(StrategicAreaInfo);
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
		if (CityId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CityId);
		}
		if (OpenTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OpenTime);
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
		if (Durability != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Durability);
		}
		if (LastDurabilityTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LastDurabilityTime);
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
		num += buildPointInfo_.CalculateSize(_repeated_buildPointInfo_codec);
		if (SoldierRemain != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SoldierRemain);
		}
		if (thermalConductor_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ThermalConductor);
		}
		if (productInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ProductInfo);
		}
		if (Icon.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(Icon);
		}
		if (strategicAreaInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(StrategicAreaInfo);
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
	public void MergeFrom(AllianceCityPointInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.CityId != 0)
		{
			CityId = other.CityId;
		}
		if (other.OpenTime != 0)
		{
			OpenTime = other.OpenTime;
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
		if (other.Durability != 0)
		{
			Durability = other.Durability;
		}
		if (other.LastDurabilityTime != 0)
		{
			LastDurabilityTime = other.LastDurabilityTime;
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
		buildPointInfo_.Add(other.buildPointInfo_);
		if (other.SoldierRemain != 0)
		{
			SoldierRemain = other.SoldierRemain;
		}
		if (other.thermalConductor_ != null)
		{
			if (thermalConductor_ == null)
			{
				ThermalConductor = new ThermalConductor();
			}
			ThermalConductor.MergeFrom(other.ThermalConductor);
		}
		if (other.productInfo_ != null)
		{
			if (productInfo_ == null)
			{
				ProductInfo = new productInfo();
			}
			ProductInfo.MergeFrom(other.ProductInfo);
		}
		if (other.Icon.Length != 0)
		{
			Icon = other.Icon;
		}
		if (other.strategicAreaInfo_ != null)
		{
			if (strategicAreaInfo_ == null)
			{
				StrategicAreaInfo = new StrategicAreaInfo();
			}
			StrategicAreaInfo.MergeFrom(other.StrategicAreaInfo);
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
				CityId = input.ReadInt32();
				break;
			case 16u:
				OpenTime = input.ReadInt32();
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
			case 64u:
				Durability = input.ReadInt32();
				break;
			case 72u:
				LastDurabilityTime = input.ReadInt32();
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
			case 130u:
				buildPointInfo_.AddEntriesFrom(input, _repeated_buildPointInfo_codec);
				break;
			case 136u:
				SoldierRemain = input.ReadInt32();
				break;
			case 146u:
				if (thermalConductor_ == null)
				{
					ThermalConductor = new ThermalConductor();
				}
				input.ReadMessage(ThermalConductor);
				break;
			case 154u:
				if (productInfo_ == null)
				{
					ProductInfo = new productInfo();
				}
				input.ReadMessage(ProductInfo);
				break;
			case 162u:
				Icon = input.ReadString();
				break;
			case 170u:
				if (strategicAreaInfo_ == null)
				{
					StrategicAreaInfo = new StrategicAreaInfo();
				}
				input.ReadMessage(StrategicAreaInfo);
				break;
			case 176u:
				WarTimeIndex = input.ReadInt32();
				break;
			}
		}
	}
}
