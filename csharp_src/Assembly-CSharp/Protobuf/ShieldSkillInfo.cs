using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ShieldSkillInfo : IMessage<ShieldSkillInfo>, IMessage, IEquatable<ShieldSkillInfo>, IDeepCloneable<ShieldSkillInfo>
{
	private static readonly MessageParser<ShieldSkillInfo> _parser = new MessageParser<ShieldSkillInfo>(() => new ShieldSkillInfo());

	private UnknownFieldSet _unknownFields;

	public const int SkillIdFieldNumber = 1;

	private int skillId_;

	public const int OverTimeFieldNumber = 2;

	private long overTime_;

	public const int StartTimeFieldNumber = 3;

	private long startTime_;

	public const int MaxShieldFieldNumber = 4;

	private long maxShield_;

	public const int CurShieldFieldNumber = 5;

	private long curShield_;

	[DebuggerNonUserCode]
	public static MessageParser<ShieldSkillInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[35];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int SkillId
	{
		get
		{
			return skillId_;
		}
		set
		{
			skillId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long OverTime
	{
		get
		{
			return overTime_;
		}
		set
		{
			overTime_ = value;
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
	public long MaxShield
	{
		get
		{
			return maxShield_;
		}
		set
		{
			maxShield_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurShield
	{
		get
		{
			return curShield_;
		}
		set
		{
			curShield_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ShieldSkillInfo()
	{
	}

	[DebuggerNonUserCode]
	public ShieldSkillInfo(ShieldSkillInfo other)
		: this()
	{
		skillId_ = other.skillId_;
		overTime_ = other.overTime_;
		startTime_ = other.startTime_;
		maxShield_ = other.maxShield_;
		curShield_ = other.curShield_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ShieldSkillInfo Clone()
	{
		return new ShieldSkillInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ShieldSkillInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ShieldSkillInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SkillId != other.SkillId)
		{
			return false;
		}
		if (OverTime != other.OverTime)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (MaxShield != other.MaxShield)
		{
			return false;
		}
		if (CurShield != other.CurShield)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SkillId != 0)
		{
			num ^= SkillId.GetHashCode();
		}
		if (OverTime != 0L)
		{
			num ^= OverTime.GetHashCode();
		}
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (MaxShield != 0L)
		{
			num ^= MaxShield.GetHashCode();
		}
		if (CurShield != 0L)
		{
			num ^= CurShield.GetHashCode();
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
		if (SkillId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(SkillId);
		}
		if (OverTime != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(OverTime);
		}
		if (StartTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(StartTime);
		}
		if (MaxShield != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(MaxShield);
		}
		if (CurShield != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(CurShield);
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
		if (SkillId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillId);
		}
		if (OverTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(OverTime);
		}
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (MaxShield != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxShield);
		}
		if (CurShield != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurShield);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ShieldSkillInfo other)
	{
		if (other != null)
		{
			if (other.SkillId != 0)
			{
				SkillId = other.SkillId;
			}
			if (other.OverTime != 0L)
			{
				OverTime = other.OverTime;
			}
			if (other.StartTime != 0L)
			{
				StartTime = other.StartTime;
			}
			if (other.MaxShield != 0L)
			{
				MaxShield = other.MaxShield;
			}
			if (other.CurShield != 0L)
			{
				CurShield = other.CurShield;
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
				SkillId = input.ReadInt32();
				break;
			case 16u:
				OverTime = input.ReadInt64();
				break;
			case 24u:
				StartTime = input.ReadInt64();
				break;
			case 32u:
				MaxShield = input.ReadInt64();
				break;
			case 40u:
				CurShield = input.ReadInt64();
				break;
			}
		}
	}
}
