using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class GoldTreePointInfo : IMessage<GoldTreePointInfo>, IMessage, IEquatable<GoldTreePointInfo>, IDeepCloneable<GoldTreePointInfo>
{
	private static readonly MessageParser<GoldTreePointInfo> _parser = new MessageParser<GoldTreePointInfo>(() => new GoldTreePointInfo());

	private UnknownFieldSet _unknownFields;

	public const int TreeIdFieldNumber = 1;

	private int treeId_;

	public const int PowerFieldNumber = 2;

	private long power_;

	public const int StartTimeFieldNumber = 3;

	private long startTime_;

	public const int EndTimeFieldNumber = 4;

	private long endTime_;

	public const int FinishTimeFieldNumber = 5;

	private long finishTime_;

	public const int ChargePlayerNumFieldNumber = 6;

	private int chargePlayerNum_;

	[DebuggerNonUserCode]
	public static MessageParser<GoldTreePointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[32];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int TreeId
	{
		get
		{
			return treeId_;
		}
		set
		{
			treeId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Power
	{
		get
		{
			return power_;
		}
		set
		{
			power_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StartTime
	{
		get
		{
			return startTime_;
		}
		set
		{
			startTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long EndTime
	{
		get
		{
			return endTime_;
		}
		set
		{
			endTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long FinishTime
	{
		get
		{
			return finishTime_;
		}
		set
		{
			finishTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ChargePlayerNum
	{
		get
		{
			return chargePlayerNum_;
		}
		set
		{
			chargePlayerNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public GoldTreePointInfo()
	{
	}

	[DebuggerNonUserCode]
	public GoldTreePointInfo(GoldTreePointInfo other)
		: this()
	{
		treeId_ = other.treeId_;
		power_ = other.power_;
		startTime_ = other.startTime_;
		endTime_ = other.endTime_;
		finishTime_ = other.finishTime_;
		chargePlayerNum_ = other.chargePlayerNum_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GoldTreePointInfo Clone()
	{
		return new GoldTreePointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GoldTreePointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(GoldTreePointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TreeId != other.TreeId)
		{
			return false;
		}
		if (Power != other.Power)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (EndTime != other.EndTime)
		{
			return false;
		}
		if (FinishTime != other.FinishTime)
		{
			return false;
		}
		if (ChargePlayerNum != other.ChargePlayerNum)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TreeId != 0)
		{
			num ^= TreeId.GetHashCode();
		}
		if (Power != 0L)
		{
			num ^= Power.GetHashCode();
		}
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (EndTime != 0L)
		{
			num ^= EndTime.GetHashCode();
		}
		if (FinishTime != 0L)
		{
			num ^= FinishTime.GetHashCode();
		}
		if (ChargePlayerNum != 0)
		{
			num ^= ChargePlayerNum.GetHashCode();
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
		if (TreeId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(TreeId);
		}
		if (Power != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Power);
		}
		if (StartTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(StartTime);
		}
		if (EndTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(EndTime);
		}
		if (FinishTime != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(FinishTime);
		}
		if (ChargePlayerNum != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(ChargePlayerNum);
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
		if (TreeId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TreeId);
		}
		if (Power != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Power);
		}
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (EndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(EndTime);
		}
		if (FinishTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(FinishTime);
		}
		if (ChargePlayerNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ChargePlayerNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GoldTreePointInfo other)
	{
		if (other != null)
		{
			if (other.TreeId != 0)
			{
				TreeId = other.TreeId;
			}
			if (other.Power != 0L)
			{
				Power = other.Power;
			}
			if (other.StartTime != 0L)
			{
				StartTime = other.StartTime;
			}
			if (other.EndTime != 0L)
			{
				EndTime = other.EndTime;
			}
			if (other.FinishTime != 0L)
			{
				FinishTime = other.FinishTime;
			}
			if (other.ChargePlayerNum != 0)
			{
				ChargePlayerNum = other.ChargePlayerNum;
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
				TreeId = input.ReadInt32();
				break;
			case 16u:
				Power = input.ReadInt64();
				break;
			case 24u:
				StartTime = input.ReadInt64();
				break;
			case 32u:
				EndTime = input.ReadInt64();
				break;
			case 40u:
				FinishTime = input.ReadInt64();
				break;
			case 48u:
				ChargePlayerNum = input.ReadInt32();
				break;
			}
		}
	}
}
