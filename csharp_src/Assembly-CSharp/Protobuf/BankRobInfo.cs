using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BankRobInfo : IMessage<BankRobInfo>, IMessage, IEquatable<BankRobInfo>, IDeepCloneable<BankRobInfo>
{
	private static readonly MessageParser<BankRobInfo> _parser = new MessageParser<BankRobInfo>(() => new BankRobInfo());

	private UnknownFieldSet _unknownFields;

	public const int TotalAmountFieldNumber = 1;

	private long totalAmount_;

	public const int RobAmountFieldNumber = 2;

	private long robAmount_;

	public const int RobEndTimeFieldNumber = 3;

	private long robEndTime_;

	public const int RobStageFieldNumber = 4;

	private int robStage_;

	[DebuggerNonUserCode]
	public static MessageParser<BankRobInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[29];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long TotalAmount
	{
		get
		{
			return totalAmount_;
		}
		set
		{
			totalAmount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long RobAmount
	{
		get
		{
			return robAmount_;
		}
		set
		{
			robAmount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long RobEndTime
	{
		get
		{
			return robEndTime_;
		}
		set
		{
			robEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RobStage
	{
		get
		{
			return robStage_;
		}
		set
		{
			robStage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BankRobInfo()
	{
	}

	[DebuggerNonUserCode]
	public BankRobInfo(BankRobInfo other)
		: this()
	{
		totalAmount_ = other.totalAmount_;
		robAmount_ = other.robAmount_;
		robEndTime_ = other.robEndTime_;
		robStage_ = other.robStage_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BankRobInfo Clone()
	{
		return new BankRobInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BankRobInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(BankRobInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TotalAmount != other.TotalAmount)
		{
			return false;
		}
		if (RobAmount != other.RobAmount)
		{
			return false;
		}
		if (RobEndTime != other.RobEndTime)
		{
			return false;
		}
		if (RobStage != other.RobStage)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TotalAmount != 0L)
		{
			num ^= TotalAmount.GetHashCode();
		}
		if (RobAmount != 0L)
		{
			num ^= RobAmount.GetHashCode();
		}
		if (RobEndTime != 0L)
		{
			num ^= RobEndTime.GetHashCode();
		}
		if (RobStage != 0)
		{
			num ^= RobStage.GetHashCode();
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
		if (TotalAmount != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(TotalAmount);
		}
		if (RobAmount != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(RobAmount);
		}
		if (RobEndTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(RobEndTime);
		}
		if (RobStage != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(RobStage);
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
		if (TotalAmount != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TotalAmount);
		}
		if (RobAmount != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(RobAmount);
		}
		if (RobEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(RobEndTime);
		}
		if (RobStage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RobStage);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BankRobInfo other)
	{
		if (other != null)
		{
			if (other.TotalAmount != 0L)
			{
				TotalAmount = other.TotalAmount;
			}
			if (other.RobAmount != 0L)
			{
				RobAmount = other.RobAmount;
			}
			if (other.RobEndTime != 0L)
			{
				RobEndTime = other.RobEndTime;
			}
			if (other.RobStage != 0)
			{
				RobStage = other.RobStage;
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
				TotalAmount = input.ReadInt64();
				break;
			case 16u:
				RobAmount = input.ReadInt64();
				break;
			case 24u:
				RobEndTime = input.ReadInt64();
				break;
			case 32u:
				RobStage = input.ReadInt32();
				break;
			}
		}
	}
}
