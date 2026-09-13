using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class HonorWallProgress : IMessage<HonorWallProgress>, IMessage, IEquatable<HonorWallProgress>, IDeepCloneable<HonorWallProgress>
{
	private static readonly MessageParser<HonorWallProgress> _parser = new MessageParser<HonorWallProgress>(() => new HonorWallProgress());

	private UnknownFieldSet _unknownFields;

	public const int CountFieldNumber = 1;

	private int count_;

	public const int TotalLevelFieldNumber = 2;

	private int totalLevel_;

	[DebuggerNonUserCode]
	public static MessageParser<HonorWallProgress> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[25];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Count
	{
		get
		{
			return count_;
		}
		set
		{
			count_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TotalLevel
	{
		get
		{
			return totalLevel_;
		}
		set
		{
			totalLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public HonorWallProgress()
	{
	}

	[DebuggerNonUserCode]
	public HonorWallProgress(HonorWallProgress other)
		: this()
	{
		count_ = other.count_;
		totalLevel_ = other.totalLevel_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public HonorWallProgress Clone()
	{
		return new HonorWallProgress(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as HonorWallProgress);
	}

	[DebuggerNonUserCode]
	public bool Equals(HonorWallProgress other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Count != other.Count)
		{
			return false;
		}
		if (TotalLevel != other.TotalLevel)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Count != 0)
		{
			num ^= Count.GetHashCode();
		}
		if (TotalLevel != 0)
		{
			num ^= TotalLevel.GetHashCode();
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
		if (Count != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Count);
		}
		if (TotalLevel != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(TotalLevel);
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
		if (Count != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Count);
		}
		if (TotalLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TotalLevel);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(HonorWallProgress other)
	{
		if (other != null)
		{
			if (other.Count != 0)
			{
				Count = other.Count;
			}
			if (other.TotalLevel != 0)
			{
				TotalLevel = other.TotalLevel;
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
				Count = input.ReadInt32();
				break;
			case 16u:
				TotalLevel = input.ReadInt32();
				break;
			}
		}
	}
}
