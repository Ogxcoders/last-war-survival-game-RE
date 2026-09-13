using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DragonScoreInfo : IMessage<DragonScoreInfo>, IMessage, IEquatable<DragonScoreInfo>, IDeepCloneable<DragonScoreInfo>
{
	private static readonly MessageParser<DragonScoreInfo> _parser = new MessageParser<DragonScoreInfo>(() => new DragonScoreInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int ScoreFieldNumber = 2;

	private int score_;

	public const int ItemIdFieldNumber = 3;

	private int itemId_;

	[DebuggerNonUserCode]
	public static MessageParser<DragonScoreInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[50];

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
	public int Score
	{
		get
		{
			return score_;
		}
		set
		{
			score_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ItemId
	{
		get
		{
			return itemId_;
		}
		set
		{
			itemId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public DragonScoreInfo()
	{
	}

	[DebuggerNonUserCode]
	public DragonScoreInfo(DragonScoreInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		score_ = other.score_;
		itemId_ = other.itemId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DragonScoreInfo Clone()
	{
		return new DragonScoreInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DragonScoreInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(DragonScoreInfo other)
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
		if (Score != other.Score)
		{
			return false;
		}
		if (ItemId != other.ItemId)
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
		if (Score != 0)
		{
			num ^= Score.GetHashCode();
		}
		if (ItemId != 0)
		{
			num ^= ItemId.GetHashCode();
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
		if (Score != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Score);
		}
		if (ItemId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(ItemId);
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
		if (Score != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Score);
		}
		if (ItemId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ItemId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DragonScoreInfo other)
	{
		if (other != null)
		{
			if (other.Uuid != 0L)
			{
				Uuid = other.Uuid;
			}
			if (other.Score != 0)
			{
				Score = other.Score;
			}
			if (other.ItemId != 0)
			{
				ItemId = other.ItemId;
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
				Score = input.ReadInt32();
				break;
			case 24u:
				ItemId = input.ReadInt32();
				break;
			}
		}
	}
}
