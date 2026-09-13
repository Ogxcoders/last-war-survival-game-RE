using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CollectResourceInfo : IMessage<CollectResourceInfo>, IMessage, IEquatable<CollectResourceInfo>, IDeepCloneable<CollectResourceInfo>
{
	private static readonly MessageParser<CollectResourceInfo> _parser = new MessageParser<CollectResourceInfo>(() => new CollectResourceInfo());

	private UnknownFieldSet _unknownFields;

	public const int ResourceTypeFieldNumber = 1;

	private int resourceType_;

	public const int LevelFieldNumber = 2;

	private int level_;

	public const int TypeFieldNumber = 3;

	private int type_;

	public const int AttachIdFieldNumber = 4;

	private int attachId_;

	[DebuggerNonUserCode]
	public static MessageParser<CollectResourceInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[12];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int ResourceType
	{
		get
		{
			return resourceType_;
		}
		set
		{
			resourceType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AttachId
	{
		get
		{
			return attachId_;
		}
		set
		{
			attachId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CollectResourceInfo()
	{
	}

	[DebuggerNonUserCode]
	public CollectResourceInfo(CollectResourceInfo other)
		: this()
	{
		resourceType_ = other.resourceType_;
		level_ = other.level_;
		type_ = other.type_;
		attachId_ = other.attachId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CollectResourceInfo Clone()
	{
		return new CollectResourceInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CollectResourceInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CollectResourceInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ResourceType != other.ResourceType)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (AttachId != other.AttachId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ResourceType != 0)
		{
			num ^= ResourceType.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (AttachId != 0)
		{
			num ^= AttachId.GetHashCode();
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
		if (ResourceType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ResourceType);
		}
		if (Level != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Level);
		}
		if (Type != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Type);
		}
		if (AttachId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(AttachId);
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
		if (ResourceType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ResourceType);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (AttachId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AttachId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CollectResourceInfo other)
	{
		if (other != null)
		{
			if (other.ResourceType != 0)
			{
				ResourceType = other.ResourceType;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.AttachId != 0)
			{
				AttachId = other.AttachId;
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
				ResourceType = input.ReadInt32();
				break;
			case 16u:
				Level = input.ReadInt32();
				break;
			case 24u:
				Type = input.ReadInt32();
				break;
			case 32u:
				AttachId = input.ReadInt32();
				break;
			}
		}
	}
}
