using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutCityTrade : IMessage<ScoutCityTrade>, IMessage, IEquatable<ScoutCityTrade>, IDeepCloneable<ScoutCityTrade>
{
	private static readonly MessageParser<ScoutCityTrade> _parser = new MessageParser<ScoutCityTrade>(() => new ScoutCityTrade());

	private UnknownFieldSet _unknownFields;

	public const int TradeIdFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_tradeId_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? tradeId_;

	public const int PointFieldNumber = 2;

	private PointInfo point_;

	public const int AbbrFieldNumber = 3;

	private string abbr_ = "";

	public const int NameFieldNumber = 4;

	private string name_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ScoutCityTrade> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? TradeId
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
	public ScoutCityTrade()
	{
	}

	[DebuggerNonUserCode]
	public ScoutCityTrade(ScoutCityTrade other)
		: this()
	{
		TradeId = other.TradeId;
		point_ = ((other.point_ != null) ? other.point_.Clone() : null);
		abbr_ = other.abbr_;
		name_ = other.name_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutCityTrade Clone()
	{
		return new ScoutCityTrade(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutCityTrade);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutCityTrade other)
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
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (tradeId_.HasValue)
		{
			num ^= TradeId.GetHashCode();
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
		if (tradeId_.HasValue)
		{
			_single_tradeId_codec.WriteTagAndValue(output, TradeId);
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
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (tradeId_.HasValue)
		{
			num += _single_tradeId_codec.CalculateSizeWithTag(TradeId);
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
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutCityTrade other)
	{
		if (other == null)
		{
			return;
		}
		if (other.tradeId_.HasValue && (!tradeId_.HasValue || other.TradeId != 0))
		{
			TradeId = other.TradeId;
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
				int? num2 = _single_tradeId_codec.Read(input);
				if (!tradeId_.HasValue || num2 != 0)
				{
					TradeId = num2;
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
			}
		}
	}
}
