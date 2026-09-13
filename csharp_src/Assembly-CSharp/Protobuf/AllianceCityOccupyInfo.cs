using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceCityOccupyInfo : IMessage<AllianceCityOccupyInfo>, IMessage, IEquatable<AllianceCityOccupyInfo>, IDeepCloneable<AllianceCityOccupyInfo>
{
	private static readonly MessageParser<AllianceCityOccupyInfo> _parser = new MessageParser<AllianceCityOccupyInfo>(() => new AllianceCityOccupyInfo());

	private UnknownFieldSet _unknownFields;

	public const int CityIdFieldNumber = 1;

	private int cityId_;

	public const int AbbrFieldNumber = 2;

	private string abbr_ = "";

	public const int AllainceIdFieldNumber = 3;

	private string allainceId_ = "";

	public const int ColorFieldNumber = 4;

	private int color_;

	public const int CityNameFieldNumber = 5;

	private string cityName_ = "";

	public const int AllianceNameFieldNumber = 6;

	private string allianceName_ = "";

	public const int OccupyServerIdFieldNumber = 7;

	private int occupyServerId_;

	[DebuggerNonUserCode]
	public static MessageParser<AllianceCityOccupyInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => AllianceCityRecordProtoReflection.Descriptor.MessageTypes[2];

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
	public string Abbr
	{
		get
		{
			return abbr_;
		}
		set
		{
			abbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AllainceId
	{
		get
		{
			return allainceId_;
		}
		set
		{
			allainceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Color
	{
		get
		{
			return color_;
		}
		set
		{
			color_ = value;
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
	public int OccupyServerId
	{
		get
		{
			return occupyServerId_;
		}
		set
		{
			occupyServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceCityOccupyInfo()
	{
	}

	[DebuggerNonUserCode]
	public AllianceCityOccupyInfo(AllianceCityOccupyInfo other)
		: this()
	{
		cityId_ = other.cityId_;
		abbr_ = other.abbr_;
		allainceId_ = other.allainceId_;
		color_ = other.color_;
		cityName_ = other.cityName_;
		allianceName_ = other.allianceName_;
		occupyServerId_ = other.occupyServerId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceCityOccupyInfo Clone()
	{
		return new AllianceCityOccupyInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceCityOccupyInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceCityOccupyInfo other)
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
		if (Abbr != other.Abbr)
		{
			return false;
		}
		if (AllainceId != other.AllainceId)
		{
			return false;
		}
		if (Color != other.Color)
		{
			return false;
		}
		if (CityName != other.CityName)
		{
			return false;
		}
		if (AllianceName != other.AllianceName)
		{
			return false;
		}
		if (OccupyServerId != other.OccupyServerId)
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
		if (Abbr.Length != 0)
		{
			num ^= Abbr.GetHashCode();
		}
		if (AllainceId.Length != 0)
		{
			num ^= AllainceId.GetHashCode();
		}
		if (Color != 0)
		{
			num ^= Color.GetHashCode();
		}
		if (CityName.Length != 0)
		{
			num ^= CityName.GetHashCode();
		}
		if (AllianceName.Length != 0)
		{
			num ^= AllianceName.GetHashCode();
		}
		if (OccupyServerId != 0)
		{
			num ^= OccupyServerId.GetHashCode();
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
		if (Abbr.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Abbr);
		}
		if (AllainceId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(AllainceId);
		}
		if (Color != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Color);
		}
		if (CityName.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(CityName);
		}
		if (AllianceName.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(AllianceName);
		}
		if (OccupyServerId != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(OccupyServerId);
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
		if (Abbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Abbr);
		}
		if (AllainceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllainceId);
		}
		if (Color != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Color);
		}
		if (CityName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CityName);
		}
		if (AllianceName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceName);
		}
		if (OccupyServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OccupyServerId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceCityOccupyInfo other)
	{
		if (other != null)
		{
			if (other.CityId != 0)
			{
				CityId = other.CityId;
			}
			if (other.Abbr.Length != 0)
			{
				Abbr = other.Abbr;
			}
			if (other.AllainceId.Length != 0)
			{
				AllainceId = other.AllainceId;
			}
			if (other.Color != 0)
			{
				Color = other.Color;
			}
			if (other.CityName.Length != 0)
			{
				CityName = other.CityName;
			}
			if (other.AllianceName.Length != 0)
			{
				AllianceName = other.AllianceName;
			}
			if (other.OccupyServerId != 0)
			{
				OccupyServerId = other.OccupyServerId;
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
				CityId = input.ReadInt32();
				break;
			case 18u:
				Abbr = input.ReadString();
				break;
			case 26u:
				AllainceId = input.ReadString();
				break;
			case 32u:
				Color = input.ReadInt32();
				break;
			case 42u:
				CityName = input.ReadString();
				break;
			case 50u:
				AllianceName = input.ReadString();
				break;
			case 56u:
				OccupyServerId = input.ReadInt32();
				break;
			}
		}
	}
}
