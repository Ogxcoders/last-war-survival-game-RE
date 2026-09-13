using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutTargetArmyTotal : IMessage<ScoutTargetArmyTotal>, IMessage, IEquatable<ScoutTargetArmyTotal>, IDeepCloneable<ScoutTargetArmyTotal>
{
	private static readonly MessageParser<ScoutTargetArmyTotal> _parser = new MessageParser<ScoutTargetArmyTotal>(() => new ScoutTargetArmyTotal());

	private UnknownFieldSet _unknownFields;

	public const int VisibleFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_visible_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? visible_;

	public const int TotalFieldNumber = 2;

	private static readonly FieldCodec<long?> _single_total_codec = FieldCodec.ForStructWrapper<long>(18u);

	private long? total_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutTargetArmyTotal> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[13];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? Visible
	{
		get
		{
			return visible_;
		}
		set
		{
			visible_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long? Total
	{
		get
		{
			return total_;
		}
		set
		{
			total_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutTargetArmyTotal()
	{
	}

	[DebuggerNonUserCode]
	public ScoutTargetArmyTotal(ScoutTargetArmyTotal other)
		: this()
	{
		Visible = other.Visible;
		Total = other.Total;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutTargetArmyTotal Clone()
	{
		return new ScoutTargetArmyTotal(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutTargetArmyTotal);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutTargetArmyTotal other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Visible != other.Visible)
		{
			return false;
		}
		if (Total != other.Total)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (visible_.HasValue)
		{
			num ^= Visible.GetHashCode();
		}
		if (total_.HasValue)
		{
			num ^= Total.GetHashCode();
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
		if (visible_.HasValue)
		{
			_single_visible_codec.WriteTagAndValue(output, Visible);
		}
		if (total_.HasValue)
		{
			_single_total_codec.WriteTagAndValue(output, Total);
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
		if (visible_.HasValue)
		{
			num += _single_visible_codec.CalculateSizeWithTag(Visible);
		}
		if (total_.HasValue)
		{
			num += _single_total_codec.CalculateSizeWithTag(Total);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutTargetArmyTotal other)
	{
		if (other != null)
		{
			if (other.visible_.HasValue && (!visible_.HasValue || other.Visible != 0))
			{
				Visible = other.Visible;
			}
			if (other.total_.HasValue && (!total_.HasValue || other.Total != 0))
			{
				Total = other.Total;
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
			{
				int? num3 = _single_visible_codec.Read(input);
				if (!visible_.HasValue || num3 != 0)
				{
					Visible = num3;
				}
				break;
			}
			case 18u:
			{
				long? num2 = _single_total_codec.Read(input);
				if (!total_.HasValue || num2 != 0)
				{
					Total = num2;
				}
				break;
			}
			}
		}
	}
}
