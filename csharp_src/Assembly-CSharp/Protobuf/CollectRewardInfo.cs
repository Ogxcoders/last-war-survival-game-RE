using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CollectRewardInfo : IMessage<CollectRewardInfo>, IMessage, IEquatable<CollectRewardInfo>, IDeepCloneable<CollectRewardInfo>
{
	private static readonly MessageParser<CollectRewardInfo> _parser = new MessageParser<CollectRewardInfo>(() => new CollectRewardInfo());

	private UnknownFieldSet _unknownFields;

	public const int OwnerUidFieldNumber = 1;

	private string ownerUid_ = "";

	public const int UuidFieldNumber = 2;

	private long uuid_;

	public const int ExpireTimeFieldNumber = 3;

	private long expireTime_;

	public const int ContentIdFieldNumber = 4;

	private int contentId_;

	[DebuggerNonUserCode]
	public static MessageParser<CollectRewardInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[33];

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
	public long ExpireTime
	{
		get
		{
			return expireTime_;
		}
		set
		{
			expireTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ContentId
	{
		get
		{
			return contentId_;
		}
		set
		{
			contentId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CollectRewardInfo()
	{
	}

	[DebuggerNonUserCode]
	public CollectRewardInfo(CollectRewardInfo other)
		: this()
	{
		ownerUid_ = other.ownerUid_;
		uuid_ = other.uuid_;
		expireTime_ = other.expireTime_;
		contentId_ = other.contentId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CollectRewardInfo Clone()
	{
		return new CollectRewardInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CollectRewardInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CollectRewardInfo other)
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
		if (ExpireTime != other.ExpireTime)
		{
			return false;
		}
		if (ContentId != other.ContentId)
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
		if (ExpireTime != 0L)
		{
			num ^= ExpireTime.GetHashCode();
		}
		if (ContentId != 0)
		{
			num ^= ContentId.GetHashCode();
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
		if (ExpireTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ExpireTime);
		}
		if (ContentId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(ContentId);
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
		if (ExpireTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ExpireTime);
		}
		if (ContentId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ContentId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CollectRewardInfo other)
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
			if (other.ExpireTime != 0L)
			{
				ExpireTime = other.ExpireTime;
			}
			if (other.ContentId != 0)
			{
				ContentId = other.ContentId;
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
				ExpireTime = input.ReadInt64();
				break;
			case 32u:
				ContentId = input.ReadInt32();
				break;
			}
		}
	}
}
