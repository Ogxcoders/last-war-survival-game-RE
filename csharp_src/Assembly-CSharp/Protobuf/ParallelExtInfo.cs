using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ParallelExtInfo : IMessage<ParallelExtInfo>, IMessage, IEquatable<ParallelExtInfo>, IDeepCloneable<ParallelExtInfo>
{
	private static readonly MessageParser<ParallelExtInfo> _parser = new MessageParser<ParallelExtInfo>(() => new ParallelExtInfo());

	private UnknownFieldSet _unknownFields;

	public const int TotalPayFieldNumber = 1;

	private double totalPay_;

	public const int VipLevelFieldNumber = 2;

	private int vipLevel_;

	[DebuggerNonUserCode]
	public static MessageParser<ParallelExtInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[30];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public double TotalPay
	{
		get
		{
			return totalPay_;
		}
		set
		{
			totalPay_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int VipLevel
	{
		get
		{
			return vipLevel_;
		}
		set
		{
			vipLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ParallelExtInfo()
	{
	}

	[DebuggerNonUserCode]
	public ParallelExtInfo(ParallelExtInfo other)
		: this()
	{
		totalPay_ = other.totalPay_;
		vipLevel_ = other.vipLevel_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ParallelExtInfo Clone()
	{
		return new ParallelExtInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ParallelExtInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ParallelExtInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(TotalPay, other.TotalPay))
		{
			return false;
		}
		if (VipLevel != other.VipLevel)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TotalPay != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(TotalPay);
		}
		if (VipLevel != 0)
		{
			num ^= VipLevel.GetHashCode();
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
		if (TotalPay != 0.0)
		{
			output.WriteRawTag(9);
			output.WriteDouble(TotalPay);
		}
		if (VipLevel != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(VipLevel);
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
		if (TotalPay != 0.0)
		{
			num += 9;
		}
		if (VipLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(VipLevel);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ParallelExtInfo other)
	{
		if (other != null)
		{
			if (other.TotalPay != 0.0)
			{
				TotalPay = other.TotalPay;
			}
			if (other.VipLevel != 0)
			{
				VipLevel = other.VipLevel;
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
			case 9u:
				TotalPay = input.ReadDouble();
				break;
			case 16u:
				VipLevel = input.ReadInt32();
				break;
			}
		}
	}
}
