using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DotDamageDetail : IMessage<DotDamageDetail>, IMessage, IEquatable<DotDamageDetail>, IDeepCloneable<DotDamageDetail>
{
	private static readonly MessageParser<DotDamageDetail> _parser = new MessageParser<DotDamageDetail>(() => new DotDamageDetail());

	private UnknownFieldSet _unknownFields;

	public const int BuffIdFieldNumber = 1;

	private int buffId_;

	public const int DamageFieldNumber = 2;

	private int damage_;

	public const int StartTimeFieldNumber = 3;

	private int startTime_;

	public const int DotTimesFieldNumber = 4;

	private int dotTimes_;

	[DebuggerNonUserCode]
	public static MessageParser<DotDamageDetail> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int BuffId
	{
		get
		{
			return buffId_;
		}
		set
		{
			buffId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Damage
	{
		get
		{
			return damage_;
		}
		set
		{
			damage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int StartTime
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
	public int DotTimes
	{
		get
		{
			return dotTimes_;
		}
		set
		{
			dotTimes_ = value;
		}
	}

	[DebuggerNonUserCode]
	public DotDamageDetail()
	{
	}

	[DebuggerNonUserCode]
	public DotDamageDetail(DotDamageDetail other)
		: this()
	{
		buffId_ = other.buffId_;
		damage_ = other.damage_;
		startTime_ = other.startTime_;
		dotTimes_ = other.dotTimes_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DotDamageDetail Clone()
	{
		return new DotDamageDetail(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DotDamageDetail);
	}

	[DebuggerNonUserCode]
	public bool Equals(DotDamageDetail other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BuffId != other.BuffId)
		{
			return false;
		}
		if (Damage != other.Damage)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (DotTimes != other.DotTimes)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (BuffId != 0)
		{
			num ^= BuffId.GetHashCode();
		}
		if (Damage != 0)
		{
			num ^= Damage.GetHashCode();
		}
		if (StartTime != 0)
		{
			num ^= StartTime.GetHashCode();
		}
		if (DotTimes != 0)
		{
			num ^= DotTimes.GetHashCode();
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
		if (BuffId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(BuffId);
		}
		if (Damage != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Damage);
		}
		if (StartTime != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(StartTime);
		}
		if (DotTimes != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(DotTimes);
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
		if (BuffId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuffId);
		}
		if (Damage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Damage);
		}
		if (StartTime != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(StartTime);
		}
		if (DotTimes != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DotTimes);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DotDamageDetail other)
	{
		if (other != null)
		{
			if (other.BuffId != 0)
			{
				BuffId = other.BuffId;
			}
			if (other.Damage != 0)
			{
				Damage = other.Damage;
			}
			if (other.StartTime != 0)
			{
				StartTime = other.StartTime;
			}
			if (other.DotTimes != 0)
			{
				DotTimes = other.DotTimes;
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
				BuffId = input.ReadInt32();
				break;
			case 16u:
				Damage = input.ReadInt32();
				break;
			case 24u:
				StartTime = input.ReadInt32();
				break;
			case 32u:
				DotTimes = input.ReadInt32();
				break;
			}
		}
	}
}
