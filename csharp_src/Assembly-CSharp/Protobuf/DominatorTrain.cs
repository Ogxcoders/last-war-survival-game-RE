using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DominatorTrain : IMessage<DominatorTrain>, IMessage, IEquatable<DominatorTrain>, IDeepCloneable<DominatorTrain>
{
	private static readonly MessageParser<DominatorTrain> _parser = new MessageParser<DominatorTrain>(() => new DominatorTrain());

	private UnknownFieldSet _unknownFields;

	public const int TrainIdFieldNumber = 1;

	private int trainId_;

	public const int LevelFieldNumber = 2;

	private int level_;

	[DebuggerNonUserCode]
	public static MessageParser<DominatorTrain> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[8];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int TrainId
	{
		get
		{
			return trainId_;
		}
		set
		{
			trainId_ = value;
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
	public DominatorTrain()
	{
	}

	[DebuggerNonUserCode]
	public DominatorTrain(DominatorTrain other)
		: this()
	{
		trainId_ = other.trainId_;
		level_ = other.level_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DominatorTrain Clone()
	{
		return new DominatorTrain(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DominatorTrain);
	}

	[DebuggerNonUserCode]
	public bool Equals(DominatorTrain other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TrainId != other.TrainId)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TrainId != 0)
		{
			num ^= TrainId.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
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
		if (TrainId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(TrainId);
		}
		if (Level != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Level);
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
		if (TrainId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TrainId);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DominatorTrain other)
	{
		if (other != null)
		{
			if (other.TrainId != 0)
			{
				TrainId = other.TrainId;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
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
				TrainId = input.ReadInt32();
				break;
			case 16u:
				Level = input.ReadInt32();
				break;
			}
		}
	}
}
