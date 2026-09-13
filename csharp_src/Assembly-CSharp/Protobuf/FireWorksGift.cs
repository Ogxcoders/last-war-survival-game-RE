using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class FireWorksGift : IMessage<FireWorksGift>, IMessage, IEquatable<FireWorksGift>, IDeepCloneable<FireWorksGift>
{
	private static readonly MessageParser<FireWorksGift> _parser = new MessageParser<FireWorksGift>(() => new FireWorksGift());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int ConfigIdFieldNumber = 2;

	private int configId_;

	public const int IndexFieldNumber = 10;

	private int index_;

	public const int NumFieldNumber = 3;

	private int num_;

	public const int MaxFieldNumber = 4;

	private int max_;

	public const int SendTimeFieldNumber = 5;

	private long sendTime_;

	public const int SendUidFieldNumber = 6;

	private string sendUid_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<FireWorksGift> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[59];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public int Index
	{
		get
		{
			return index_;
		}
		set
		{
			index_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Num
	{
		get
		{
			return num_;
		}
		set
		{
			num_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Max
	{
		get
		{
			return max_;
		}
		set
		{
			max_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long SendTime
	{
		get
		{
			return sendTime_;
		}
		set
		{
			sendTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string SendUid
	{
		get
		{
			return sendUid_;
		}
		set
		{
			sendUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public FireWorksGift()
	{
	}

	[DebuggerNonUserCode]
	public FireWorksGift(FireWorksGift other)
		: this()
	{
		uuid_ = other.uuid_;
		configId_ = other.configId_;
		index_ = other.index_;
		num_ = other.num_;
		max_ = other.max_;
		sendTime_ = other.sendTime_;
		sendUid_ = other.sendUid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public FireWorksGift Clone()
	{
		return new FireWorksGift(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as FireWorksGift);
	}

	[DebuggerNonUserCode]
	public bool Equals(FireWorksGift other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (ConfigId != other.ConfigId)
		{
			return false;
		}
		if (Index != other.Index)
		{
			return false;
		}
		if (Num != other.Num)
		{
			return false;
		}
		if (Max != other.Max)
		{
			return false;
		}
		if (SendTime != other.SendTime)
		{
			return false;
		}
		if (SendUid != other.SendUid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (ConfigId != 0)
		{
			num ^= ConfigId.GetHashCode();
		}
		if (Index != 0)
		{
			num ^= Index.GetHashCode();
		}
		if (Num != 0)
		{
			num ^= Num.GetHashCode();
		}
		if (Max != 0)
		{
			num ^= Max.GetHashCode();
		}
		if (SendTime != 0L)
		{
			num ^= SendTime.GetHashCode();
		}
		if (SendUid.Length != 0)
		{
			num ^= SendUid.GetHashCode();
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
		if (Uuid != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(Uuid);
		}
		if (ConfigId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(ConfigId);
		}
		if (Num != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Num);
		}
		if (Max != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Max);
		}
		if (SendTime != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(SendTime);
		}
		if (SendUid.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(SendUid);
		}
		if (Index != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(Index);
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
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (ConfigId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ConfigId);
		}
		if (Index != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Index);
		}
		if (Num != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Num);
		}
		if (Max != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Max);
		}
		if (SendTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(SendTime);
		}
		if (SendUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(SendUid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(FireWorksGift other)
	{
		if (other != null)
		{
			if (other.Uuid != 0L)
			{
				Uuid = other.Uuid;
			}
			if (other.ConfigId != 0)
			{
				ConfigId = other.ConfigId;
			}
			if (other.Index != 0)
			{
				Index = other.Index;
			}
			if (other.Num != 0)
			{
				Num = other.Num;
			}
			if (other.Max != 0)
			{
				Max = other.Max;
			}
			if (other.SendTime != 0L)
			{
				SendTime = other.SendTime;
			}
			if (other.SendUid.Length != 0)
			{
				SendUid = other.SendUid;
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
				Uuid = input.ReadInt64();
				break;
			case 16u:
				ConfigId = input.ReadInt32();
				break;
			case 24u:
				Num = input.ReadInt32();
				break;
			case 32u:
				Max = input.ReadInt32();
				break;
			case 40u:
				SendTime = input.ReadInt64();
				break;
			case 50u:
				SendUid = input.ReadString();
				break;
			case 80u:
				Index = input.ReadInt32();
				break;
			}
		}
	}
}
