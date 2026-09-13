using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class IceSuppliesPointInfo : IMessage<IceSuppliesPointInfo>, IMessage, IEquatable<IceSuppliesPointInfo>, IDeepCloneable<IceSuppliesPointInfo>
{
	private static readonly MessageParser<IceSuppliesPointInfo> _parser = new MessageParser<IceSuppliesPointInfo>(() => new IceSuppliesPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int ConfigIdFieldNumber = 2;

	private int configId_;

	public const int StateFieldNumber = 3;

	private int state_;

	public const int RewardUserListFieldNumber = 4;

	private static readonly FieldCodec<string> _repeated_rewardUserList_codec = FieldCodec.ForString(34u);

	private readonly RepeatedField<string> rewardUserList_ = new RepeatedField<string>();

	public const int ThermalConductorFieldNumber = 5;

	private ThermalConductor thermalConductor_;

	public const int CreateTimeFieldNumber = 6;

	private long createTime_;

	public const int DiscovererAllianceIdFieldNumber = 7;

	private string discovererAllianceId_ = "";

	public const int DiscovererUidFieldNumber = 8;

	private string discovererUid_ = "";

	public const int ChargeEndTimeFieldNumber = 9;

	private long chargeEndTime_;

	public const int ChargeStateFieldNumber = 10;

	private int chargeState_;

	[DebuggerNonUserCode]
	public static MessageParser<IceSuppliesPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[53];

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
	public int ConfigId
	{
		get
		{
			return configId_;
		}
		set
		{
			configId_ = value;
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
	public RepeatedField<string> RewardUserList => rewardUserList_;

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
	public string DiscovererAllianceId
	{
		get
		{
			return discovererAllianceId_;
		}
		set
		{
			discovererAllianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string DiscovererUid
	{
		get
		{
			return discovererUid_;
		}
		set
		{
			discovererUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public long ChargeEndTime
	{
		get
		{
			return chargeEndTime_;
		}
		set
		{
			chargeEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ChargeState
	{
		get
		{
			return chargeState_;
		}
		set
		{
			chargeState_ = value;
		}
	}

	[DebuggerNonUserCode]
	public IceSuppliesPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public IceSuppliesPointInfo(IceSuppliesPointInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		configId_ = other.configId_;
		state_ = other.state_;
		rewardUserList_ = other.rewardUserList_.Clone();
		thermalConductor_ = ((other.thermalConductor_ != null) ? other.thermalConductor_.Clone() : null);
		createTime_ = other.createTime_;
		discovererAllianceId_ = other.discovererAllianceId_;
		discovererUid_ = other.discovererUid_;
		chargeEndTime_ = other.chargeEndTime_;
		chargeState_ = other.chargeState_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public IceSuppliesPointInfo Clone()
	{
		return new IceSuppliesPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as IceSuppliesPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(IceSuppliesPointInfo other)
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
		if (ConfigId != other.ConfigId)
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		if (!rewardUserList_.Equals(other.rewardUserList_))
		{
			return false;
		}
		if (!object.Equals(ThermalConductor, other.ThermalConductor))
		{
			return false;
		}
		if (CreateTime != other.CreateTime)
		{
			return false;
		}
		if (DiscovererAllianceId != other.DiscovererAllianceId)
		{
			return false;
		}
		if (DiscovererUid != other.DiscovererUid)
		{
			return false;
		}
		if (ChargeEndTime != other.ChargeEndTime)
		{
			return false;
		}
		if (ChargeState != other.ChargeState)
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
		if (ConfigId != 0)
		{
			num ^= ConfigId.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		num ^= rewardUserList_.GetHashCode();
		if (thermalConductor_ != null)
		{
			num ^= ThermalConductor.GetHashCode();
		}
		if (CreateTime != 0L)
		{
			num ^= CreateTime.GetHashCode();
		}
		if (DiscovererAllianceId.Length != 0)
		{
			num ^= DiscovererAllianceId.GetHashCode();
		}
		if (DiscovererUid.Length != 0)
		{
			num ^= DiscovererUid.GetHashCode();
		}
		if (ChargeEndTime != 0L)
		{
			num ^= ChargeEndTime.GetHashCode();
		}
		if (ChargeState != 0)
		{
			num ^= ChargeState.GetHashCode();
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
		if (ConfigId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(ConfigId);
		}
		if (State != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(State);
		}
		rewardUserList_.WriteTo(output, _repeated_rewardUserList_codec);
		if (thermalConductor_ != null)
		{
			output.WriteRawTag(42);
			output.WriteMessage(ThermalConductor);
		}
		if (CreateTime != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(CreateTime);
		}
		if (DiscovererAllianceId.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(DiscovererAllianceId);
		}
		if (DiscovererUid.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(DiscovererUid);
		}
		if (ChargeEndTime != 0L)
		{
			output.WriteRawTag(72);
			output.WriteInt64(ChargeEndTime);
		}
		if (ChargeState != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(ChargeState);
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
		if (ConfigId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ConfigId);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		num += rewardUserList_.CalculateSize(_repeated_rewardUserList_codec);
		if (thermalConductor_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ThermalConductor);
		}
		if (CreateTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CreateTime);
		}
		if (DiscovererAllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(DiscovererAllianceId);
		}
		if (DiscovererUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(DiscovererUid);
		}
		if (ChargeEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ChargeEndTime);
		}
		if (ChargeState != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ChargeState);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(IceSuppliesPointInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Uuid != 0L)
		{
			Uuid = other.Uuid;
		}
		if (other.ConfigId != 0)
		{
			ConfigId = other.ConfigId;
		}
		if (other.State != 0)
		{
			State = other.State;
		}
		rewardUserList_.Add(other.rewardUserList_);
		if (other.thermalConductor_ != null)
		{
			if (thermalConductor_ == null)
			{
				ThermalConductor = new ThermalConductor();
			}
			ThermalConductor.MergeFrom(other.ThermalConductor);
		}
		if (other.CreateTime != 0L)
		{
			CreateTime = other.CreateTime;
		}
		if (other.DiscovererAllianceId.Length != 0)
		{
			DiscovererAllianceId = other.DiscovererAllianceId;
		}
		if (other.DiscovererUid.Length != 0)
		{
			DiscovererUid = other.DiscovererUid;
		}
		if (other.ChargeEndTime != 0L)
		{
			ChargeEndTime = other.ChargeEndTime;
		}
		if (other.ChargeState != 0)
		{
			ChargeState = other.ChargeState;
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
			case 16u:
				ConfigId = input.ReadInt32();
				break;
			case 24u:
				State = input.ReadInt32();
				break;
			case 34u:
				rewardUserList_.AddEntriesFrom(input, _repeated_rewardUserList_codec);
				break;
			case 42u:
				if (thermalConductor_ == null)
				{
					ThermalConductor = new ThermalConductor();
				}
				input.ReadMessage(ThermalConductor);
				break;
			case 48u:
				CreateTime = input.ReadInt64();
				break;
			case 58u:
				DiscovererAllianceId = input.ReadString();
				break;
			case 66u:
				DiscovererUid = input.ReadString();
				break;
			case 72u:
				ChargeEndTime = input.ReadInt64();
				break;
			case 80u:
				ChargeState = input.ReadInt32();
				break;
			}
		}
	}
}
