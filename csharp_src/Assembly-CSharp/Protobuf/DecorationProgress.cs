using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DecorationProgress : IMessage<DecorationProgress>, IMessage, IEquatable<DecorationProgress>, IDeepCloneable<DecorationProgress>
{
	private static readonly MessageParser<DecorationProgress> _parser = new MessageParser<DecorationProgress>(() => new DecorationProgress());

	private UnknownFieldSet _unknownFields;

	public const int DecoQualityIdFieldNumber = 1;

	private int decoQualityId_;

	public const int TotalCountFieldNumber = 2;

	private int totalCount_;

	public const int TotalLvFieldNumber = 3;

	private int totalLv_;

	[DebuggerNonUserCode]
	public static MessageParser<DecorationProgress> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[24];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int DecoQualityId
	{
		get
		{
			return decoQualityId_;
		}
		set
		{
			decoQualityId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TotalCount
	{
		get
		{
			return totalCount_;
		}
		set
		{
			totalCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TotalLv
	{
		get
		{
			return totalLv_;
		}
		set
		{
			totalLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public DecorationProgress()
	{
	}

	[DebuggerNonUserCode]
	public DecorationProgress(DecorationProgress other)
		: this()
	{
		decoQualityId_ = other.decoQualityId_;
		totalCount_ = other.totalCount_;
		totalLv_ = other.totalLv_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DecorationProgress Clone()
	{
		return new DecorationProgress(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DecorationProgress);
	}

	[DebuggerNonUserCode]
	public bool Equals(DecorationProgress other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (DecoQualityId != other.DecoQualityId)
		{
			return false;
		}
		if (TotalCount != other.TotalCount)
		{
			return false;
		}
		if (TotalLv != other.TotalLv)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (DecoQualityId != 0)
		{
			num ^= DecoQualityId.GetHashCode();
		}
		if (TotalCount != 0)
		{
			num ^= TotalCount.GetHashCode();
		}
		if (TotalLv != 0)
		{
			num ^= TotalLv.GetHashCode();
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
		if (DecoQualityId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(DecoQualityId);
		}
		if (TotalCount != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(TotalCount);
		}
		if (TotalLv != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(TotalLv);
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
		if (DecoQualityId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DecoQualityId);
		}
		if (TotalCount != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TotalCount);
		}
		if (TotalLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TotalLv);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DecorationProgress other)
	{
		if (other != null)
		{
			if (other.DecoQualityId != 0)
			{
				DecoQualityId = other.DecoQualityId;
			}
			if (other.TotalCount != 0)
			{
				TotalCount = other.TotalCount;
			}
			if (other.TotalLv != 0)
			{
				TotalLv = other.TotalLv;
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
				DecoQualityId = input.ReadInt32();
				break;
			case 16u:
				TotalCount = input.ReadInt32();
				break;
			case 24u:
				TotalLv = input.ReadInt32();
				break;
			}
		}
	}
}
