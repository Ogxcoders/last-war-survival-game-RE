using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutDominator : IMessage<ScoutDominator>, IMessage, IEquatable<ScoutDominator>, IDeepCloneable<ScoutDominator>
{
	private static readonly MessageParser<ScoutDominator> _parser = new MessageParser<ScoutDominator>(() => new ScoutDominator());

	private UnknownFieldSet _unknownFields;

	public const int DominatorIdFieldNumber = 1;

	private int dominatorId_;

	public const int RankLvFieldNumber = 2;

	private int rankLv_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutDominator> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[18];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int DominatorId
	{
		get
		{
			return dominatorId_;
		}
		set
		{
			dominatorId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RankLv
	{
		get
		{
			return rankLv_;
		}
		set
		{
			rankLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutDominator()
	{
	}

	[DebuggerNonUserCode]
	public ScoutDominator(ScoutDominator other)
		: this()
	{
		dominatorId_ = other.dominatorId_;
		rankLv_ = other.rankLv_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutDominator Clone()
	{
		return new ScoutDominator(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutDominator);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutDominator other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (DominatorId != other.DominatorId)
		{
			return false;
		}
		if (RankLv != other.RankLv)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (DominatorId != 0)
		{
			num ^= DominatorId.GetHashCode();
		}
		if (RankLv != 0)
		{
			num ^= RankLv.GetHashCode();
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
		if (DominatorId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(DominatorId);
		}
		if (RankLv != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(RankLv);
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
		if (DominatorId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DominatorId);
		}
		if (RankLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RankLv);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutDominator other)
	{
		if (other != null)
		{
			if (other.DominatorId != 0)
			{
				DominatorId = other.DominatorId;
			}
			if (other.RankLv != 0)
			{
				RankLv = other.RankLv;
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
				DominatorId = input.ReadInt32();
				break;
			case 16u:
				RankLv = input.ReadInt32();
				break;
			}
		}
	}
}
