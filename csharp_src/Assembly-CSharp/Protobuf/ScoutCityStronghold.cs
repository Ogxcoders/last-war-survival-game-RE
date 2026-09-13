using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutCityStronghold : IMessage<ScoutCityStronghold>, IMessage, IEquatable<ScoutCityStronghold>, IDeepCloneable<ScoutCityStronghold>
{
	private static readonly MessageParser<ScoutCityStronghold> _parser = new MessageParser<ScoutCityStronghold>(() => new ScoutCityStronghold());

	private UnknownFieldSet _unknownFields;

	public const int StrongholdIdFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_strongholdId_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? strongholdId_;

	public const int PointFieldNumber = 2;

	private PointInfo point_;

	public const int AbbrFieldNumber = 3;

	private string abbr_ = "";

	public const int NameFieldNumber = 4;

	private string name_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ScoutCityStronghold> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[4];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? StrongholdId
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
	public ScoutCityStronghold()
	{
	}

	[DebuggerNonUserCode]
	public ScoutCityStronghold(ScoutCityStronghold other)
		: this()
	{
		StrongholdId = other.StrongholdId;
		point_ = ((other.point_ != null) ? other.point_.Clone() : null);
		abbr_ = other.abbr_;
		name_ = other.name_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutCityStronghold Clone()
	{
		return new ScoutCityStronghold(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutCityStronghold);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutCityStronghold other)
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
		if (strongholdId_.HasValue)
		{
			num ^= StrongholdId.GetHashCode();
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
		if (strongholdId_.HasValue)
		{
			_single_strongholdId_codec.WriteTagAndValue(output, StrongholdId);
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
		if (strongholdId_.HasValue)
		{
			num += _single_strongholdId_codec.CalculateSizeWithTag(StrongholdId);
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
	public void MergeFrom(ScoutCityStronghold other)
	{
		if (other == null)
		{
			return;
		}
		if (other.strongholdId_.HasValue && (!strongholdId_.HasValue || other.StrongholdId != 0))
		{
			StrongholdId = other.StrongholdId;
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
				int? num2 = _single_strongholdId_codec.Read(input);
				if (!strongholdId_.HasValue || num2 != 0)
				{
					StrongholdId = num2;
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
