using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class RoundStatusInfo : IMessage<RoundStatusInfo>, IMessage, IEquatable<RoundStatusInfo>, IDeepCloneable<RoundStatusInfo>
{
	private static readonly MessageParser<RoundStatusInfo> _parser = new MessageParser<RoundStatusInfo>(() => new RoundStatusInfo());

	private UnknownFieldSet _unknownFields;

	public const int StatusIdFieldNumber = 1;

	private int statusId_;

	public const int TimeFieldNumber = 2;

	private int time_;

	[DebuggerNonUserCode]
	public static MessageParser<RoundStatusInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleRoundPushReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int StatusId
	{
		get
		{
			return statusId_;
		}
		set
		{
			statusId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Time
	{
		get
		{
			return time_;
		}
		set
		{
			time_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RoundStatusInfo()
	{
	}

	[DebuggerNonUserCode]
	public RoundStatusInfo(RoundStatusInfo other)
		: this()
	{
		statusId_ = other.statusId_;
		time_ = other.time_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoundStatusInfo Clone()
	{
		return new RoundStatusInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoundStatusInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoundStatusInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (StatusId != other.StatusId)
		{
			return false;
		}
		if (Time != other.Time)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (StatusId != 0)
		{
			num ^= StatusId.GetHashCode();
		}
		if (Time != 0)
		{
			num ^= Time.GetHashCode();
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
		if (StatusId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(StatusId);
		}
		if (Time != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Time);
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
		if (StatusId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(StatusId);
		}
		if (Time != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Time);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoundStatusInfo other)
	{
		if (other != null)
		{
			if (other.StatusId != 0)
			{
				StatusId = other.StatusId;
			}
			if (other.Time != 0)
			{
				Time = other.Time;
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
				StatusId = input.ReadInt32();
				break;
			case 16u:
				Time = input.ReadInt32();
				break;
			}
		}
	}
}
