using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class StrongholdBuildPointInfo : IMessage<StrongholdBuildPointInfo>, IMessage, IEquatable<StrongholdBuildPointInfo>, IDeepCloneable<StrongholdBuildPointInfo>
{
	private static readonly MessageParser<StrongholdBuildPointInfo> _parser = new MessageParser<StrongholdBuildPointInfo>(() => new StrongholdBuildPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int BuildPointFieldNumber = 1;

	private int buildPoint_;

	public const int BuildSpeedFieldNumber = 3;

	private int buildSpeed_;

	public const int ServerIdFieldNumber = 2;

	private int serverId_;

	public const int AlAbbrFieldNumber = 4;

	private string alAbbr_ = "";

	public const int AllianceIdFieldNumber = 5;

	private string allianceId_ = "";

	public const int AlNameFieldNumber = 6;

	private string alName_ = "";

	public const int AlIconFieldNumber = 7;

	private string alIcon_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<StrongholdBuildPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[27];

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
	public StrongholdBuildPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public StrongholdBuildPointInfo(StrongholdBuildPointInfo other)
		: this()
	{
		buildPoint_ = other.buildPoint_;
		buildSpeed_ = other.buildSpeed_;
		serverId_ = other.serverId_;
		alAbbr_ = other.alAbbr_;
		allianceId_ = other.allianceId_;
		alName_ = other.alName_;
		alIcon_ = other.alIcon_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public StrongholdBuildPointInfo Clone()
	{
		return new StrongholdBuildPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as StrongholdBuildPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(StrongholdBuildPointInfo other)
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
		if (ServerId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(ServerId);
		}
		if (BuildSpeed != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(BuildSpeed);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(AllianceId);
		}
		if (AlName.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(AlName);
		}
		if (AlIcon.Length != 0)
		{
			output.WriteRawTag(58);
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
	public void MergeFrom(StrongholdBuildPointInfo other)
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
				ServerId = input.ReadInt32();
				break;
			case 24u:
				BuildSpeed = input.ReadInt32();
				break;
			case 34u:
				AlAbbr = input.ReadString();
				break;
			case 42u:
				AllianceId = input.ReadString();
				break;
			case 50u:
				AlName = input.ReadString();
				break;
			case 58u:
				AlIcon = input.ReadString();
				break;
			}
		}
	}
}
