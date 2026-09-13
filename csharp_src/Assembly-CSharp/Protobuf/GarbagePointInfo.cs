using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class GarbagePointInfo : IMessage<GarbagePointInfo>, IMessage, IEquatable<GarbagePointInfo>, IDeepCloneable<GarbagePointInfo>
{
	private static readonly MessageParser<GarbagePointInfo> _parser = new MessageParser<GarbagePointInfo>(() => new GarbagePointInfo());

	private UnknownFieldSet _unknownFields;

	public const int OwnerUidFieldNumber = 1;

	private string ownerUid_ = "";

	public const int UuidFieldNumber = 2;

	private long uuid_;

	public const int EventIdFieldNumber = 3;

	private string eventId_ = "";

	public const int EndTimeFieldNumber = 4;

	private long endTime_;

	[DebuggerNonUserCode]
	public static MessageParser<GarbagePointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[16];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string OwnerUid
	{
		get
		{
			return ownerUid_;
		}
		set
		{
			ownerUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public long Uuid
	{
		get
		{
			return uuid_;
		}
		set
		{
			uuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string EventId
	{
		get
		{
			return eventId_;
		}
		set
		{
			eventId_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public GarbagePointInfo()
	{
	}

	[DebuggerNonUserCode]
	public GarbagePointInfo(GarbagePointInfo other)
		: this()
	{
		ownerUid_ = other.ownerUid_;
		uuid_ = other.uuid_;
		eventId_ = other.eventId_;
		endTime_ = other.endTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public GarbagePointInfo Clone()
	{
		return new GarbagePointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as GarbagePointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(GarbagePointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (OwnerUid != other.OwnerUid)
		{
			return false;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (EventId != other.EventId)
		{
			return false;
		}
		if (EndTime != other.EndTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (OwnerUid.Length != 0)
		{
			num ^= OwnerUid.GetHashCode();
		}
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (EventId.Length != 0)
		{
			num ^= EventId.GetHashCode();
		}
		if (EndTime != 0L)
		{
			num ^= EndTime.GetHashCode();
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
		if (OwnerUid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(OwnerUid);
		}
		if (Uuid != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Uuid);
		}
		if (EventId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(EventId);
		}
		if (EndTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(EndTime);
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
		if (OwnerUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(OwnerUid);
		}
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (EventId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(EventId);
		}
		if (EndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(EndTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(GarbagePointInfo other)
	{
		if (other != null)
		{
			if (other.OwnerUid.Length != 0)
			{
				OwnerUid = other.OwnerUid;
			}
			if (other.Uuid != 0L)
			{
				Uuid = other.Uuid;
			}
			if (other.EventId.Length != 0)
			{
				EventId = other.EventId;
			}
			if (other.EndTime != 0L)
			{
				EndTime = other.EndTime;
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
			case 10u:
				OwnerUid = input.ReadString();
				break;
			case 16u:
				Uuid = input.ReadInt64();
				break;
			case 26u:
				EventId = input.ReadString();
				break;
			case 32u:
				EndTime = input.ReadInt64();
				break;
			}
		}
	}
}
