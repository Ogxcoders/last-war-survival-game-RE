using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ExplorePointInfo : IMessage<ExplorePointInfo>, IMessage, IEquatable<ExplorePointInfo>, IDeepCloneable<ExplorePointInfo>
{
	private static readonly MessageParser<ExplorePointInfo> _parser = new MessageParser<ExplorePointInfo>(() => new ExplorePointInfo());

	private UnknownFieldSet _unknownFields;

	public const int OwnerUidFieldNumber = 1;

	private string ownerUid_ = "";

	public const int UuidFieldNumber = 2;

	private long uuid_;

	public const int EventIdFieldNumber = 3;

	private string eventId_ = "";

	public const int ThermalConductorFieldNumber = 4;

	private ThermalConductor thermalConductor_;

	[DebuggerNonUserCode]
	public static MessageParser<ExplorePointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[14];

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
	public ThermalConductor ThermalConductor
	{
		get
		{
			return thermalConductor_;
		}
		set
		{
			thermalConductor_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ExplorePointInfo()
	{
	}

	[DebuggerNonUserCode]
	public ExplorePointInfo(ExplorePointInfo other)
		: this()
	{
		ownerUid_ = other.ownerUid_;
		uuid_ = other.uuid_;
		eventId_ = other.eventId_;
		thermalConductor_ = ((other.thermalConductor_ != null) ? other.thermalConductor_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ExplorePointInfo Clone()
	{
		return new ExplorePointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ExplorePointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ExplorePointInfo other)
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
		if (!object.Equals(ThermalConductor, other.ThermalConductor))
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
		if (thermalConductor_ != null)
		{
			num ^= ThermalConductor.GetHashCode();
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
		if (thermalConductor_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(ThermalConductor);
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
		if (thermalConductor_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ThermalConductor);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ExplorePointInfo other)
	{
		if (other == null)
		{
			return;
		}
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
		if (other.thermalConductor_ != null)
		{
			if (thermalConductor_ == null)
			{
				ThermalConductor = new ThermalConductor();
			}
			ThermalConductor.MergeFrom(other.ThermalConductor);
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
			case 34u:
				if (thermalConductor_ == null)
				{
					ThermalConductor = new ThermalConductor();
				}
				input.ReadMessage(ThermalConductor);
				break;
			}
		}
	}
}
