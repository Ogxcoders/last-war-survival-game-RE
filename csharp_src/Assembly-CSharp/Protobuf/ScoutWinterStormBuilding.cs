using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutWinterStormBuilding : IMessage<ScoutWinterStormBuilding>, IMessage, IEquatable<ScoutWinterStormBuilding>, IDeepCloneable<ScoutWinterStormBuilding>
{
	private static readonly MessageParser<ScoutWinterStormBuilding> _parser = new MessageParser<ScoutWinterStormBuilding>(() => new ScoutWinterStormBuilding());

	private UnknownFieldSet _unknownFields;

	public const int BuildIdFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_buildId_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? buildId_;

	public const int PointFieldNumber = 2;

	private PointInfo point_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutWinterStormBuilding> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[27];

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
	public ScoutWinterStormBuilding()
	{
	}

	[DebuggerNonUserCode]
	public ScoutWinterStormBuilding(ScoutWinterStormBuilding other)
		: this()
	{
		BuildId = other.BuildId;
		point_ = ((other.point_ != null) ? other.point_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutWinterStormBuilding Clone()
	{
		return new ScoutWinterStormBuilding(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutWinterStormBuilding);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutWinterStormBuilding other)
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
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutWinterStormBuilding other)
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
				int? num2 = _single_buildId_codec.Read(input);
				if (!buildId_.HasValue || num2 != 0)
				{
					BuildId = num2;
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
			}
		}
	}
}
