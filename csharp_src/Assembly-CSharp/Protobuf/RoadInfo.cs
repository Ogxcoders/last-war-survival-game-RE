using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class RoadInfo : IMessage<RoadInfo>, IMessage, IEquatable<RoadInfo>, IDeepCloneable<RoadInfo>
{
	private static readonly MessageParser<RoadInfo> _parser = new MessageParser<RoadInfo>(() => new RoadInfo());

	private UnknownFieldSet _unknownFields;

	public const int OwnerUidFieldNumber = 1;

	private string ownerUid_ = "";

	public const int UuidFieldNumber = 2;

	private long uuid_;

	public const int RoadStateFieldNumber = 3;

	private int roadState_;

	public const int InsideFieldNumber = 4;

	private int inside_;

	public const int CurrentHpFieldNumber = 5;

	private int currentHp_;

	public const int AllianceIdFieldNumber = 6;

	private string allianceId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<RoadInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[11];

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
	public int RoadState
	{
		get
		{
			return roadState_;
		}
		set
		{
			roadState_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Inside
	{
		get
		{
			return inside_;
		}
		set
		{
			inside_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurrentHp
	{
		get
		{
			return currentHp_;
		}
		set
		{
			currentHp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string AllianceId
	{
		get
		{
			return allianceId_;
		}
		set
		{
			allianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public RoadInfo()
	{
	}

	[DebuggerNonUserCode]
	public RoadInfo(RoadInfo other)
		: this()
	{
		ownerUid_ = other.ownerUid_;
		uuid_ = other.uuid_;
		roadState_ = other.roadState_;
		inside_ = other.inside_;
		currentHp_ = other.currentHp_;
		allianceId_ = other.allianceId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RoadInfo Clone()
	{
		return new RoadInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RoadInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(RoadInfo other)
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
		if (RoadState != other.RoadState)
		{
			return false;
		}
		if (Inside != other.Inside)
		{
			return false;
		}
		if (CurrentHp != other.CurrentHp)
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
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
		if (RoadState != 0)
		{
			num ^= RoadState.GetHashCode();
		}
		if (Inside != 0)
		{
			num ^= Inside.GetHashCode();
		}
		if (CurrentHp != 0)
		{
			num ^= CurrentHp.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
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
		if (RoadState != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(RoadState);
		}
		if (Inside != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Inside);
		}
		if (CurrentHp != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(CurrentHp);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(AllianceId);
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
		if (RoadState != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RoadState);
		}
		if (Inside != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Inside);
		}
		if (CurrentHp != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurrentHp);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RoadInfo other)
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
			if (other.RoadState != 0)
			{
				RoadState = other.RoadState;
			}
			if (other.Inside != 0)
			{
				Inside = other.Inside;
			}
			if (other.CurrentHp != 0)
			{
				CurrentHp = other.CurrentHp;
			}
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
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
			case 24u:
				RoadState = input.ReadInt32();
				break;
			case 32u:
				Inside = input.ReadInt32();
				break;
			case 40u:
				CurrentHp = input.ReadInt32();
				break;
			case 50u:
				AllianceId = input.ReadString();
				break;
			}
		}
	}
}
