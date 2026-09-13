using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class SkillChantInfo : IMessage<SkillChantInfo>, IMessage, IEquatable<SkillChantInfo>, IDeepCloneable<SkillChantInfo>
{
	private static readonly MessageParser<SkillChantInfo> _parser = new MessageParser<SkillChantInfo>(() => new SkillChantInfo());

	private UnknownFieldSet _unknownFields;

	public const int SkillIdFieldNumber = 1;

	private int skillId_;

	public const int OverTimeFieldNumber = 2;

	private long overTime_;

	[DebuggerNonUserCode]
	public static MessageParser<SkillChantInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[5];

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
	public SkillChantInfo()
	{
	}

	[DebuggerNonUserCode]
	public SkillChantInfo(SkillChantInfo other)
		: this()
	{
		skillId_ = other.skillId_;
		overTime_ = other.overTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SkillChantInfo Clone()
	{
		return new SkillChantInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SkillChantInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(SkillChantInfo other)
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
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SkillChantInfo other)
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
			}
		}
	}
}
