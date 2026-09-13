using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MuseMummy : IMessage<MuseMummy>, IMessage, IEquatable<MuseMummy>, IDeepCloneable<MuseMummy>
{
	private static readonly MessageParser<MuseMummy> _parser = new MessageParser<MuseMummy>(() => new MuseMummy());

	private UnknownFieldSet _unknownFields;

	public const int SearchTimesFieldNumber = 1;

	private int searchTimes_;

	public const int SkillTargetPointIdFieldNumber = 2;

	private int skillTargetPointId_;

	[DebuggerNonUserCode]
	public static MessageParser<MuseMummy> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[37];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int SearchTimes
	{
		get
		{
			return searchTimes_;
		}
		set
		{
			searchTimes_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SkillTargetPointId
	{
		get
		{
			return skillTargetPointId_;
		}
		set
		{
			skillTargetPointId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MuseMummy()
	{
	}

	[DebuggerNonUserCode]
	public MuseMummy(MuseMummy other)
		: this()
	{
		searchTimes_ = other.searchTimes_;
		skillTargetPointId_ = other.skillTargetPointId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MuseMummy Clone()
	{
		return new MuseMummy(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MuseMummy);
	}

	[DebuggerNonUserCode]
	public bool Equals(MuseMummy other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SearchTimes != other.SearchTimes)
		{
			return false;
		}
		if (SkillTargetPointId != other.SkillTargetPointId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SearchTimes != 0)
		{
			num ^= SearchTimes.GetHashCode();
		}
		if (SkillTargetPointId != 0)
		{
			num ^= SkillTargetPointId.GetHashCode();
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
		if (SearchTimes != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(SearchTimes);
		}
		if (SkillTargetPointId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(SkillTargetPointId);
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
		if (SearchTimes != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SearchTimes);
		}
		if (SkillTargetPointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillTargetPointId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MuseMummy other)
	{
		if (other != null)
		{
			if (other.SearchTimes != 0)
			{
				SearchTimes = other.SearchTimes;
			}
			if (other.SkillTargetPointId != 0)
			{
				SkillTargetPointId = other.SkillTargetPointId;
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
				SearchTimes = input.ReadInt32();
				break;
			case 16u:
				SkillTargetPointId = input.ReadInt32();
				break;
			}
		}
	}
}
