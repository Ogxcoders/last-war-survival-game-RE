using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MonsterChallengeNewDonate : IMessage<MonsterChallengeNewDonate>, IMessage, IEquatable<MonsterChallengeNewDonate>, IDeepCloneable<MonsterChallengeNewDonate>
{
	private static readonly MessageParser<MonsterChallengeNewDonate> _parser = new MessageParser<MonsterChallengeNewDonate>(() => new MonsterChallengeNewDonate());

	private UnknownFieldSet _unknownFields;

	public const int ConfigIdFieldNumber = 1;

	private int configId_;

	public const int CountFieldNumber = 2;

	private int count_;

	public const int EndTimeStampFieldNumber = 3;

	private long endTimeStamp_;

	[DebuggerNonUserCode]
	public static MessageParser<MonsterChallengeNewDonate> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int ConfigId
	{
		get
		{
			return configId_;
		}
		set
		{
			configId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Count
	{
		get
		{
			return count_;
		}
		set
		{
			count_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long EndTimeStamp
	{
		get
		{
			return endTimeStamp_;
		}
		set
		{
			endTimeStamp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MonsterChallengeNewDonate()
	{
	}

	[DebuggerNonUserCode]
	public MonsterChallengeNewDonate(MonsterChallengeNewDonate other)
		: this()
	{
		configId_ = other.configId_;
		count_ = other.count_;
		endTimeStamp_ = other.endTimeStamp_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MonsterChallengeNewDonate Clone()
	{
		return new MonsterChallengeNewDonate(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MonsterChallengeNewDonate);
	}

	[DebuggerNonUserCode]
	public bool Equals(MonsterChallengeNewDonate other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ConfigId != other.ConfigId)
		{
			return false;
		}
		if (Count != other.Count)
		{
			return false;
		}
		if (EndTimeStamp != other.EndTimeStamp)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ConfigId != 0)
		{
			num ^= ConfigId.GetHashCode();
		}
		if (Count != 0)
		{
			num ^= Count.GetHashCode();
		}
		if (EndTimeStamp != 0L)
		{
			num ^= EndTimeStamp.GetHashCode();
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
		if (ConfigId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ConfigId);
		}
		if (Count != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Count);
		}
		if (EndTimeStamp != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(EndTimeStamp);
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
		if (ConfigId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ConfigId);
		}
		if (Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Count);
		}
		if (EndTimeStamp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(EndTimeStamp);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MonsterChallengeNewDonate other)
	{
		if (other != null)
		{
			if (other.ConfigId != 0)
			{
				ConfigId = other.ConfigId;
			}
			if (other.Count != 0)
			{
				Count = other.Count;
			}
			if (other.EndTimeStamp != 0L)
			{
				EndTimeStamp = other.EndTimeStamp;
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
				ConfigId = input.ReadInt32();
				break;
			case 16u:
				Count = input.ReadInt32();
				break;
			case 24u:
				EndTimeStamp = input.ReadInt64();
				break;
			}
		}
	}
}
