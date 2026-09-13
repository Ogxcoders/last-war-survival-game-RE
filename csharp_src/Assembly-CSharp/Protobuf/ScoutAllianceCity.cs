using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutAllianceCity : IMessage<ScoutAllianceCity>, IMessage, IEquatable<ScoutAllianceCity>, IDeepCloneable<ScoutAllianceCity>
{
	private static readonly MessageParser<ScoutAllianceCity> _parser = new MessageParser<ScoutAllianceCity>(() => new ScoutAllianceCity());

	private UnknownFieldSet _unknownFields;

	public const int CityIdFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_cityId_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? cityId_;

	public const int PointFieldNumber = 2;

	private PointInfo point_;

	public const int AbbrFieldNumber = 3;

	private string abbr_ = "";

	public const int NameFieldNumber = 4;

	private string name_ = "";

	public const int CityNameFieldNumber = 5;

	private string cityName_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ScoutAllianceCity> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? CityId
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
	public PointInfo Point
	{
		get
		{
			return point_;
		}
		set
		{
			point_ = value;
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
	public ScoutAllianceCity()
	{
	}

	[DebuggerNonUserCode]
	public ScoutAllianceCity(ScoutAllianceCity other)
		: this()
	{
		CityId = other.CityId;
		point_ = ((other.point_ != null) ? other.point_.Clone() : null);
		abbr_ = other.abbr_;
		name_ = other.name_;
		cityName_ = other.cityName_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutAllianceCity Clone()
	{
		return new ScoutAllianceCity(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutAllianceCity);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutAllianceCity other)
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
		if (!object.Equals(Point, other.Point))
		{
			return false;
		}
		if (Abbr != other.Abbr)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (CityName != other.CityName)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (cityId_.HasValue)
		{
			num ^= CityId.GetHashCode();
		}
		if (point_ != null)
		{
			num ^= Point.GetHashCode();
		}
		if (Abbr.Length != 0)
		{
			num ^= Abbr.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (CityName.Length != 0)
		{
			num ^= CityName.GetHashCode();
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
		if (cityId_.HasValue)
		{
			_single_cityId_codec.WriteTagAndValue(output, CityId);
		}
		if (point_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(Point);
		}
		if (Abbr.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Abbr);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(Name);
		}
		if (CityName.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(CityName);
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
		if (cityId_.HasValue)
		{
			num += _single_cityId_codec.CalculateSizeWithTag(CityId);
		}
		if (point_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Point);
		}
		if (Abbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Abbr);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (CityName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CityName);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutAllianceCity other)
	{
		if (other == null)
		{
			return;
		}
		if (other.cityId_.HasValue && (!cityId_.HasValue || other.CityId != 0))
		{
			CityId = other.CityId;
		}
		if (other.point_ != null)
		{
			if (point_ == null)
			{
				Point = new PointInfo();
			}
			Point.MergeFrom(other.Point);
		}
		if (other.Abbr.Length != 0)
		{
			Abbr = other.Abbr;
		}
		if (other.Name.Length != 0)
		{
			Name = other.Name;
		}
		if (other.CityName.Length != 0)
		{
			CityName = other.CityName;
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
			{
				int? num2 = _single_cityId_codec.Read(input);
				if (!cityId_.HasValue || num2 != 0)
				{
					CityId = num2;
				}
				break;
			}
			case 18u:
				if (point_ == null)
				{
					Point = new PointInfo();
				}
				input.ReadMessage(Point);
				break;
			case 26u:
				Abbr = input.ReadString();
				break;
			case 34u:
				Name = input.ReadString();
				break;
			case 42u:
				CityName = input.ReadString();
				break;
			}
		}
	}
}
