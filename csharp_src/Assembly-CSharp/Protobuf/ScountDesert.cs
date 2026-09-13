using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScountDesert : IMessage<ScountDesert>, IMessage, IEquatable<ScountDesert>, IDeepCloneable<ScountDesert>
{
	private static readonly MessageParser<ScountDesert> _parser = new MessageParser<ScountDesert>(() => new ScountDesert());

	private UnknownFieldSet _unknownFields;

	public const int DesIdFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_desId_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? desId_;

	public const int PointFieldNumber = 2;

	private PointInfo point_;

	[DebuggerNonUserCode]
	public static MessageParser<ScountDesert> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? DesId
	{
		get
		{
			return desId_;
		}
		set
		{
			desId_ = value;
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
	public ScountDesert()
	{
	}

	[DebuggerNonUserCode]
	public ScountDesert(ScountDesert other)
		: this()
	{
		DesId = other.DesId;
		point_ = ((other.point_ != null) ? other.point_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScountDesert Clone()
	{
		return new ScountDesert(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScountDesert);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScountDesert other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (DesId != other.DesId)
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
		if (desId_.HasValue)
		{
			num ^= DesId.GetHashCode();
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
		if (desId_.HasValue)
		{
			_single_desId_codec.WriteTagAndValue(output, DesId);
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
		if (desId_.HasValue)
		{
			num += _single_desId_codec.CalculateSizeWithTag(DesId);
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
	public void MergeFrom(ScountDesert other)
	{
		if (other == null)
		{
			return;
		}
		if (other.desId_.HasValue && (!desId_.HasValue || other.DesId != 0))
		{
			DesId = other.DesId;
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
				int? num2 = _single_desId_codec.Read(input);
				if (!desId_.HasValue || num2 != 0)
				{
					DesId = num2;
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
