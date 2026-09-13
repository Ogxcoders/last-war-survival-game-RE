using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BuildPointInfo : IMessage<BuildPointInfo>, IMessage, IEquatable<BuildPointInfo>, IDeepCloneable<BuildPointInfo>
{
	private static readonly MessageParser<BuildPointInfo> _parser = new MessageParser<BuildPointInfo>(() => new BuildPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int BuildPointFieldNumber = 1;

	private int buildPoint_;

	public const int ServerIdFieldNumber = 2;

	private int serverId_;

	public const int BuildSpeedFieldNumber = 3;

	private int buildSpeed_;

	public const int CampIdFieldNumber = 4;

	private int campId_;

	public const int AllianceIdFieldNumber = 5;

	private string allianceId_ = "";

	public const int AllianceAbbrFieldNumber = 6;

	private string allianceAbbr_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<BuildPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[17];

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
	public int CampId
	{
		get
		{
			return campId_;
		}
		set
		{
			campId_ = value;
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
	public BuildPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public BuildPointInfo(BuildPointInfo other)
		: this()
	{
		buildPoint_ = other.buildPoint_;
		serverId_ = other.serverId_;
		buildSpeed_ = other.buildSpeed_;
		campId_ = other.campId_;
		allianceId_ = other.allianceId_;
		allianceAbbr_ = other.allianceAbbr_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuildPointInfo Clone()
	{
		return new BuildPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuildPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuildPointInfo other)
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
		if (ServerId != other.ServerId)
		{
			return false;
		}
		if (BuildSpeed != other.BuildSpeed)
		{
			return false;
		}
		if (CampId != other.CampId)
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
		if (ServerId != 0)
		{
			num ^= ServerId.GetHashCode();
		}
		if (BuildSpeed != 0)
		{
			num ^= BuildSpeed.GetHashCode();
		}
		if (CampId != 0)
		{
			num ^= CampId.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (AllianceAbbr.Length != 0)
		{
			num ^= AllianceAbbr.GetHashCode();
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
		if (CampId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(CampId);
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
		if (ServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ServerId);
		}
		if (BuildSpeed != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildSpeed);
		}
		if (CampId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CampId);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (AllianceAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceAbbr);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BuildPointInfo other)
	{
		if (other != null)
		{
			if (other.BuildPoint != 0)
			{
				BuildPoint = other.BuildPoint;
			}
			if (other.ServerId != 0)
			{
				ServerId = other.ServerId;
			}
			if (other.BuildSpeed != 0)
			{
				BuildSpeed = other.BuildSpeed;
			}
			if (other.CampId != 0)
			{
				CampId = other.CampId;
			}
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.AllianceAbbr.Length != 0)
			{
				AllianceAbbr = other.AllianceAbbr;
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
			case 32u:
				CampId = input.ReadInt32();
				break;
			case 42u:
				AllianceId = input.ReadString();
				break;
			case 50u:
				AllianceAbbr = input.ReadString();
				break;
			}
		}
	}
}
