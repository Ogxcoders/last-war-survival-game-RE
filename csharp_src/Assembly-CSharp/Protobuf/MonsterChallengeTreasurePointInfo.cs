using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MonsterChallengeTreasurePointInfo : IMessage<MonsterChallengeTreasurePointInfo>, IMessage, IEquatable<MonsterChallengeTreasurePointInfo>, IDeepCloneable<MonsterChallengeTreasurePointInfo>
{
	private static readonly MessageParser<MonsterChallengeTreasurePointInfo> _parser = new MessageParser<MonsterChallengeTreasurePointInfo>(() => new MonsterChallengeTreasurePointInfo());

	private UnknownFieldSet _unknownFields;

	public const int AllianceIdFieldNumber = 1;

	private string allianceId_ = "";

	public const int StartTimeFieldNumber = 2;

	private long startTime_;

	public const int AllianceNameFieldNumber = 3;

	private string allianceName_ = "";

	public const int AllianceAbbrFieldNumber = 4;

	private string allianceAbbr_ = "";

	public const int AllianceDamageFieldNumber = 5;

	private long allianceDamage_;

	public const int ConfigIdFieldNumber = 6;

	private int configId_;

	public const int EndTimeFieldNumber = 7;

	private long endTime_;

	[DebuggerNonUserCode]
	public static MessageParser<MonsterChallengeTreasurePointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[47];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public long StartTime
	{
		get
		{
			return startTime_;
		}
		set
		{
			startTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string AllianceName
	{
		get
		{
			return allianceName_;
		}
		set
		{
			allianceName_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public long AllianceDamage
	{
		get
		{
			return allianceDamage_;
		}
		set
		{
			allianceDamage_ = value;
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
	public long EndTime
	{
		get
		{
			return endTime_;
		}
		set
		{
			endTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MonsterChallengeTreasurePointInfo()
	{
	}

	[DebuggerNonUserCode]
	public MonsterChallengeTreasurePointInfo(MonsterChallengeTreasurePointInfo other)
		: this()
	{
		allianceId_ = other.allianceId_;
		startTime_ = other.startTime_;
		allianceName_ = other.allianceName_;
		allianceAbbr_ = other.allianceAbbr_;
		allianceDamage_ = other.allianceDamage_;
		configId_ = other.configId_;
		endTime_ = other.endTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MonsterChallengeTreasurePointInfo Clone()
	{
		return new MonsterChallengeTreasurePointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MonsterChallengeTreasurePointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(MonsterChallengeTreasurePointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (AllianceName != other.AllianceName)
		{
			return false;
		}
		if (AllianceAbbr != other.AllianceAbbr)
		{
			return false;
		}
		if (AllianceDamage != other.AllianceDamage)
		{
			return false;
		}
		if (ConfigId != other.ConfigId)
		{
			return false;
		}
		if (EndTime != other.EndTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (AllianceName.Length != 0)
		{
			num ^= AllianceName.GetHashCode();
		}
		if (AllianceAbbr.Length != 0)
		{
			num ^= AllianceAbbr.GetHashCode();
		}
		if (AllianceDamage != 0L)
		{
			num ^= AllianceDamage.GetHashCode();
		}
		if (ConfigId != 0)
		{
			num ^= ConfigId.GetHashCode();
		}
		if (EndTime != 0L)
		{
			num ^= EndTime.GetHashCode();
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
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(AllianceId);
		}
		if (StartTime != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(StartTime);
		}
		if (AllianceName.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(AllianceName);
		}
		if (AllianceAbbr.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(AllianceAbbr);
		}
		if (AllianceDamage != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(AllianceDamage);
		}
		if (ConfigId != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(ConfigId);
		}
		if (EndTime != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(EndTime);
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
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (AllianceName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceName);
		}
		if (AllianceAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceAbbr);
		}
		if (AllianceDamage != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(AllianceDamage);
		}
		if (ConfigId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ConfigId);
		}
		if (EndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(EndTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MonsterChallengeTreasurePointInfo other)
	{
		if (other != null)
		{
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.StartTime != 0L)
			{
				StartTime = other.StartTime;
			}
			if (other.AllianceName.Length != 0)
			{
				AllianceName = other.AllianceName;
			}
			if (other.AllianceAbbr.Length != 0)
			{
				AllianceAbbr = other.AllianceAbbr;
			}
			if (other.AllianceDamage != 0L)
			{
				AllianceDamage = other.AllianceDamage;
			}
			if (other.ConfigId != 0)
			{
				ConfigId = other.ConfigId;
			}
			if (other.EndTime != 0L)
			{
				EndTime = other.EndTime;
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
			case 10u:
				AllianceId = input.ReadString();
				break;
			case 16u:
				StartTime = input.ReadInt64();
				break;
			case 26u:
				AllianceName = input.ReadString();
				break;
			case 34u:
				AllianceAbbr = input.ReadString();
				break;
			case 40u:
				AllianceDamage = input.ReadInt64();
				break;
			case 48u:
				ConfigId = input.ReadInt32();
				break;
			case 56u:
				EndTime = input.ReadInt64();
				break;
			}
		}
	}
}
