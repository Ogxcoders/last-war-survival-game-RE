using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceAssistanceInfo : IMessage<AllianceAssistanceInfo>, IMessage, IEquatable<AllianceAssistanceInfo>, IDeepCloneable<AllianceAssistanceInfo>
{
	private static readonly MessageParser<AllianceAssistanceInfo> _parser = new MessageParser<AllianceAssistanceInfo>(() => new AllianceAssistanceInfo());

	private UnknownFieldSet _unknownFields;

	public const int PointIdFieldNumber = 1;

	private int pointId_;

	public const int NumberFieldNumber = 2;

	private int number_;

	public const int MaxFieldNumber = 3;

	private int max_;

	[DebuggerNonUserCode]
	public static MessageParser<AllianceAssistanceInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldAllianceAssistanceInfoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int PointId
	{
		get
		{
			return pointId_;
		}
		set
		{
			pointId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Number
	{
		get
		{
			return number_;
		}
		set
		{
			number_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Max
	{
		get
		{
			return max_;
		}
		set
		{
			max_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceAssistanceInfo()
	{
	}

	[DebuggerNonUserCode]
	public AllianceAssistanceInfo(AllianceAssistanceInfo other)
		: this()
	{
		pointId_ = other.pointId_;
		number_ = other.number_;
		max_ = other.max_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceAssistanceInfo Clone()
	{
		return new AllianceAssistanceInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceAssistanceInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceAssistanceInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (PointId != other.PointId)
		{
			return false;
		}
		if (Number != other.Number)
		{
			return false;
		}
		if (Max != other.Max)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (PointId != 0)
		{
			num ^= PointId.GetHashCode();
		}
		if (Number != 0)
		{
			num ^= Number.GetHashCode();
		}
		if (Max != 0)
		{
			num ^= Max.GetHashCode();
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
		if (PointId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(PointId);
		}
		if (Number != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Number);
		}
		if (Max != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Max);
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
		if (PointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PointId);
		}
		if (Number != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Number);
		}
		if (Max != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Max);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceAssistanceInfo other)
	{
		if (other != null)
		{
			if (other.PointId != 0)
			{
				PointId = other.PointId;
			}
			if (other.Number != 0)
			{
				Number = other.Number;
			}
			if (other.Max != 0)
			{
				Max = other.Max;
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
				PointId = input.ReadInt32();
				break;
			case 16u:
				Number = input.ReadInt32();
				break;
			case 24u:
				Max = input.ReadInt32();
				break;
			}
		}
	}
}
