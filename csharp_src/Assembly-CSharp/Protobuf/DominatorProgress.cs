using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DominatorProgress : IMessage<DominatorProgress>, IMessage, IEquatable<DominatorProgress>, IDeepCloneable<DominatorProgress>
{
	private static readonly MessageParser<DominatorProgress> _parser = new MessageParser<DominatorProgress>(() => new DominatorProgress());

	private UnknownFieldSet _unknownFields;

	public const int DominatorTrainInfosFieldNumber = 1;

	private static readonly FieldCodec<DominatorTrain> _repeated_dominatorTrainInfos_codec = FieldCodec.ForMessage(10u, DominatorTrain.Parser);

	private readonly RepeatedField<DominatorTrain> dominatorTrainInfos_ = new RepeatedField<DominatorTrain>();

	public const int PowerFieldNumber = 2;

	private int power_;

	[DebuggerNonUserCode]
	public static MessageParser<DominatorProgress> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<DominatorTrain> DominatorTrainInfos => dominatorTrainInfos_;

	[DebuggerNonUserCode]
	public int Power
	{
		get
		{
			return power_;
		}
		set
		{
			power_ = value;
		}
	}

	[DebuggerNonUserCode]
	public DominatorProgress()
	{
	}

	[DebuggerNonUserCode]
	public DominatorProgress(DominatorProgress other)
		: this()
	{
		dominatorTrainInfos_ = other.dominatorTrainInfos_.Clone();
		power_ = other.power_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DominatorProgress Clone()
	{
		return new DominatorProgress(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DominatorProgress);
	}

	[DebuggerNonUserCode]
	public bool Equals(DominatorProgress other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!dominatorTrainInfos_.Equals(other.dominatorTrainInfos_))
		{
			return false;
		}
		if (Power != other.Power)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= dominatorTrainInfos_.GetHashCode();
		if (Power != 0)
		{
			num ^= Power.GetHashCode();
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
		dominatorTrainInfos_.WriteTo(output, _repeated_dominatorTrainInfos_codec);
		if (Power != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Power);
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
		num += dominatorTrainInfos_.CalculateSize(_repeated_dominatorTrainInfos_codec);
		if (Power != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Power);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DominatorProgress other)
	{
		if (other != null)
		{
			dominatorTrainInfos_.Add(other.dominatorTrainInfos_);
			if (other.Power != 0)
			{
				Power = other.Power;
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
				dominatorTrainInfos_.AddEntriesFrom(input, _repeated_dominatorTrainInfos_codec);
				break;
			case 16u:
				Power = input.ReadInt32();
				break;
			}
		}
	}
}
