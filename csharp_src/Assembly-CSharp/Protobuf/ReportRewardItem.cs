using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ReportRewardItem : IMessage<ReportRewardItem>, IMessage, IEquatable<ReportRewardItem>, IDeepCloneable<ReportRewardItem>
{
	private static readonly MessageParser<ReportRewardItem> _parser = new MessageParser<ReportRewardItem>(() => new ReportRewardItem());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private string type_ = "";

	public const int ValueFieldNumber = 2;

	private int value_;

	[DebuggerNonUserCode]
	public static MessageParser<ReportRewardItem> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[4];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Value
	{
		get
		{
			return value_;
		}
		set
		{
			value_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ReportRewardItem()
	{
	}

	[DebuggerNonUserCode]
	public ReportRewardItem(ReportRewardItem other)
		: this()
	{
		type_ = other.type_;
		value_ = other.value_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ReportRewardItem Clone()
	{
		return new ReportRewardItem(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ReportRewardItem);
	}

	[DebuggerNonUserCode]
	public bool Equals(ReportRewardItem other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (Value != other.Value)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Type.Length != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (Value != 0)
		{
			num ^= Value.GetHashCode();
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
		if (Type.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Type);
		}
		if (Value != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Value);
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
		if (Type.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Type);
		}
		if (Value != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Value);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ReportRewardItem other)
	{
		if (other != null)
		{
			if (other.Type.Length != 0)
			{
				Type = other.Type;
			}
			if (other.Value != 0)
			{
				Value = other.Value;
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
				Type = input.ReadString();
				break;
			case 16u:
				Value = input.ReadInt32();
				break;
			}
		}
	}
}
