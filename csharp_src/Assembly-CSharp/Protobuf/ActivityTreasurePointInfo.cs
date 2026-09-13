using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ActivityTreasurePointInfo : IMessage<ActivityTreasurePointInfo>, IMessage, IEquatable<ActivityTreasurePointInfo>, IDeepCloneable<ActivityTreasurePointInfo>
{
	private static readonly MessageParser<ActivityTreasurePointInfo> _parser = new MessageParser<ActivityTreasurePointInfo>(() => new ActivityTreasurePointInfo());

	private UnknownFieldSet _unknownFields;

	public const int CfgIdFieldNumber = 1;

	private int cfgId_;

	public const int UuidFieldNumber = 2;

	private long uuid_;

	public const int OwnerIdFieldNumber = 3;

	private string ownerId_ = "";

	public const int AidFieldNumber = 4;

	private int aid_;

	[DebuggerNonUserCode]
	public static MessageParser<ActivityTreasurePointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[62];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int CfgId
	{
		get
		{
			return cfgId_;
		}
		set
		{
			cfgId_ = value;
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
	public string OwnerId
	{
		get
		{
			return ownerId_;
		}
		set
		{
			ownerId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Aid
	{
		get
		{
			return aid_;
		}
		set
		{
			aid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ActivityTreasurePointInfo()
	{
	}

	[DebuggerNonUserCode]
	public ActivityTreasurePointInfo(ActivityTreasurePointInfo other)
		: this()
	{
		cfgId_ = other.cfgId_;
		uuid_ = other.uuid_;
		ownerId_ = other.ownerId_;
		aid_ = other.aid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ActivityTreasurePointInfo Clone()
	{
		return new ActivityTreasurePointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ActivityTreasurePointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ActivityTreasurePointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CfgId != other.CfgId)
		{
			return false;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (OwnerId != other.OwnerId)
		{
			return false;
		}
		if (Aid != other.Aid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CfgId != 0)
		{
			num ^= CfgId.GetHashCode();
		}
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (OwnerId.Length != 0)
		{
			num ^= OwnerId.GetHashCode();
		}
		if (Aid != 0)
		{
			num ^= Aid.GetHashCode();
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
		if (CfgId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(CfgId);
		}
		if (Uuid != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Uuid);
		}
		if (OwnerId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(OwnerId);
		}
		if (Aid != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Aid);
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
		if (CfgId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CfgId);
		}
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (OwnerId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(OwnerId);
		}
		if (Aid != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Aid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ActivityTreasurePointInfo other)
	{
		if (other != null)
		{
			if (other.CfgId != 0)
			{
				CfgId = other.CfgId;
			}
			if (other.Uuid != 0L)
			{
				Uuid = other.Uuid;
			}
			if (other.OwnerId.Length != 0)
			{
				OwnerId = other.OwnerId;
			}
			if (other.Aid != 0)
			{
				Aid = other.Aid;
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
				CfgId = input.ReadInt32();
				break;
			case 16u:
				Uuid = input.ReadInt64();
				break;
			case 26u:
				OwnerId = input.ReadString();
				break;
			case 32u:
				Aid = input.ReadInt32();
				break;
			}
		}
	}
}
