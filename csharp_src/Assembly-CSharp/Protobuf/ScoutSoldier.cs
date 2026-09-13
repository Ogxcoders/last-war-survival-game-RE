using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutSoldier : IMessage<ScoutSoldier>, IMessage, IEquatable<ScoutSoldier>, IDeepCloneable<ScoutSoldier>
{
	private static readonly MessageParser<ScoutSoldier> _parser = new MessageParser<ScoutSoldier>(() => new ScoutSoldier());

	private UnknownFieldSet _unknownFields;

	public const int ArmsIdFieldNumber = 1;

	private string armsId_ = "";

	public const int TypeFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_type_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? type_;

	public const int TotalFieldNumber = 3;

	private static readonly FieldCodec<int?> _single_total_codec = FieldCodec.ForStructWrapper<int>(26u);

	private int? total_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutSoldier> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[21];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string ArmsId
	{
		get
		{
			return armsId_;
		}
		set
		{
			armsId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int? Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? Total
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
	public ScoutSoldier()
	{
	}

	[DebuggerNonUserCode]
	public ScoutSoldier(ScoutSoldier other)
		: this()
	{
		armsId_ = other.armsId_;
		Type = other.Type;
		Total = other.Total;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutSoldier Clone()
	{
		return new ScoutSoldier(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutSoldier);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutSoldier other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ArmsId != other.ArmsId)
		{
			return false;
		}
		if (Type != other.Type)
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
		if (ArmsId.Length != 0)
		{
			num ^= ArmsId.GetHashCode();
		}
		if (type_.HasValue)
		{
			num ^= Type.GetHashCode();
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
		if (ArmsId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(ArmsId);
		}
		if (type_.HasValue)
		{
			_single_type_codec.WriteTagAndValue(output, Type);
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
		if (ArmsId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ArmsId);
		}
		if (type_.HasValue)
		{
			num += _single_type_codec.CalculateSizeWithTag(Type);
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
	public void MergeFrom(ScoutSoldier other)
	{
		if (other != null)
		{
			if (other.ArmsId.Length != 0)
			{
				ArmsId = other.ArmsId;
			}
			if (other.type_.HasValue && (!type_.HasValue || other.Type != 0))
			{
				Type = other.Type;
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
				ArmsId = input.ReadString();
				break;
			case 18u:
			{
				int? num3 = _single_type_codec.Read(input);
				if (!type_.HasValue || num3 != 0)
				{
					Type = num3;
				}
				break;
			}
			case 26u:
			{
				int? num2 = _single_total_codec.Read(input);
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
