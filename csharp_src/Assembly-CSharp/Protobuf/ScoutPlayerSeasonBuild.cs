using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutPlayerSeasonBuild : IMessage<ScoutPlayerSeasonBuild>, IMessage, IEquatable<ScoutPlayerSeasonBuild>, IDeepCloneable<ScoutPlayerSeasonBuild>
{
	private static readonly MessageParser<ScoutPlayerSeasonBuild> _parser = new MessageParser<ScoutPlayerSeasonBuild>(() => new ScoutPlayerSeasonBuild());

	private UnknownFieldSet _unknownFields;

	public const int BuildIdFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_buildId_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? buildId_;

	public const int PointFieldNumber = 2;

	private PointInfo point_;

	public const int AbbrFieldNumber = 3;

	private string abbr_ = "";

	public const int NameFieldNumber = 4;

	private string name_ = "";

	public const int LevelFieldNumber = 5;

	private static readonly FieldCodec<int?> _single_level_codec = FieldCodec.ForStructWrapper<int>(42u);

	private int? level_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutPlayerSeasonBuild> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[25];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? BuildId
	{
		get
		{
			return buildId_;
		}
		set
		{
			buildId_ = value;
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
	public int? Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutPlayerSeasonBuild()
	{
	}

	[DebuggerNonUserCode]
	public ScoutPlayerSeasonBuild(ScoutPlayerSeasonBuild other)
		: this()
	{
		BuildId = other.BuildId;
		point_ = ((other.point_ != null) ? other.point_.Clone() : null);
		abbr_ = other.abbr_;
		name_ = other.name_;
		Level = other.Level;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutPlayerSeasonBuild Clone()
	{
		return new ScoutPlayerSeasonBuild(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutPlayerSeasonBuild);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutPlayerSeasonBuild other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BuildId != other.BuildId)
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
		if (Level != other.Level)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (buildId_.HasValue)
		{
			num ^= BuildId.GetHashCode();
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
		if (level_.HasValue)
		{
			num ^= Level.GetHashCode();
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
		if (buildId_.HasValue)
		{
			_single_buildId_codec.WriteTagAndValue(output, BuildId);
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
		if (level_.HasValue)
		{
			_single_level_codec.WriteTagAndValue(output, Level);
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
		if (buildId_.HasValue)
		{
			num += _single_buildId_codec.CalculateSizeWithTag(BuildId);
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
		if (level_.HasValue)
		{
			num += _single_level_codec.CalculateSizeWithTag(Level);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutPlayerSeasonBuild other)
	{
		if (other == null)
		{
			return;
		}
		if (other.buildId_.HasValue && (!buildId_.HasValue || other.BuildId != 0))
		{
			BuildId = other.BuildId;
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
		if (other.level_.HasValue && (!level_.HasValue || other.Level != 0))
		{
			Level = other.Level;
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
				int? num3 = _single_buildId_codec.Read(input);
				if (!buildId_.HasValue || num3 != 0)
				{
					BuildId = num3;
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
			{
				int? num2 = _single_level_codec.Read(input);
				if (!level_.HasValue || num2 != 0)
				{
					Level = num2;
				}
				break;
			}
			}
		}
	}
}
