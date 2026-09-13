using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class QuarantineSkillInfo : IMessage<QuarantineSkillInfo>, IMessage, IEquatable<QuarantineSkillInfo>, IDeepCloneable<QuarantineSkillInfo>
{
	private static readonly MessageParser<QuarantineSkillInfo> _parser = new MessageParser<QuarantineSkillInfo>(() => new QuarantineSkillInfo());

	private UnknownFieldSet _unknownFields;

	public const int SkilIdFieldNumber = 1;

	private int skilId_;

	public const int ActiveStartTimeFieldNumber = 2;

	private long activeStartTime_;

	public const int ActiveEndTimeFieldNumber = 3;

	private long activeEndTime_;

	public const int TagetPointIdFieldNumber = 4;

	private int tagetPointId_;

	[DebuggerNonUserCode]
	public static MessageParser<QuarantineSkillInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[64];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int SkilId
	{
		get
		{
			return skilId_;
		}
		set
		{
			skilId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ActiveStartTime
	{
		get
		{
			return activeStartTime_;
		}
		set
		{
			activeStartTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ActiveEndTime
	{
		get
		{
			return activeEndTime_;
		}
		set
		{
			activeEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TagetPointId
	{
		get
		{
			return tagetPointId_;
		}
		set
		{
			tagetPointId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public QuarantineSkillInfo()
	{
	}

	[DebuggerNonUserCode]
	public QuarantineSkillInfo(QuarantineSkillInfo other)
		: this()
	{
		skilId_ = other.skilId_;
		activeStartTime_ = other.activeStartTime_;
		activeEndTime_ = other.activeEndTime_;
		tagetPointId_ = other.tagetPointId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public QuarantineSkillInfo Clone()
	{
		return new QuarantineSkillInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as QuarantineSkillInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(QuarantineSkillInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SkilId != other.SkilId)
		{
			return false;
		}
		if (ActiveStartTime != other.ActiveStartTime)
		{
			return false;
		}
		if (ActiveEndTime != other.ActiveEndTime)
		{
			return false;
		}
		if (TagetPointId != other.TagetPointId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SkilId != 0)
		{
			num ^= SkilId.GetHashCode();
		}
		if (ActiveStartTime != 0L)
		{
			num ^= ActiveStartTime.GetHashCode();
		}
		if (ActiveEndTime != 0L)
		{
			num ^= ActiveEndTime.GetHashCode();
		}
		if (TagetPointId != 0)
		{
			num ^= TagetPointId.GetHashCode();
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
		if (SkilId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(SkilId);
		}
		if (ActiveStartTime != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(ActiveStartTime);
		}
		if (ActiveEndTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ActiveEndTime);
		}
		if (TagetPointId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(TagetPointId);
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
		if (SkilId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkilId);
		}
		if (ActiveStartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ActiveStartTime);
		}
		if (ActiveEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ActiveEndTime);
		}
		if (TagetPointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TagetPointId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(QuarantineSkillInfo other)
	{
		if (other != null)
		{
			if (other.SkilId != 0)
			{
				SkilId = other.SkilId;
			}
			if (other.ActiveStartTime != 0L)
			{
				ActiveStartTime = other.ActiveStartTime;
			}
			if (other.ActiveEndTime != 0L)
			{
				ActiveEndTime = other.ActiveEndTime;
			}
			if (other.TagetPointId != 0)
			{
				TagetPointId = other.TagetPointId;
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
				SkilId = input.ReadInt32();
				break;
			case 16u:
				ActiveStartTime = input.ReadInt64();
				break;
			case 24u:
				ActiveEndTime = input.ReadInt64();
				break;
			case 32u:
				TagetPointId = input.ReadInt32();
				break;
			}
		}
	}
}
