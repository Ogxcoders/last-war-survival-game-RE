using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ReportRewardNeedCollect : IMessage<ReportRewardNeedCollect>, IMessage, IEquatable<ReportRewardNeedCollect>, IDeepCloneable<ReportRewardNeedCollect>
{
	private static readonly MessageParser<ReportRewardNeedCollect> _parser = new MessageParser<ReportRewardNeedCollect>(() => new ReportRewardNeedCollect());

	private UnknownFieldSet _unknownFields;

	public const int PointIdFieldNumber = 1;

	private int pointId_;

	public const int InUserWorldFieldNumber = 2;

	private bool inUserWorld_;

	public const int RewardUuidFieldNumber = 3;

	private long rewardUuid_;

	[DebuggerNonUserCode]
	public static MessageParser<ReportRewardNeedCollect> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[37];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int PointId
	{
		get
		{
			return pointId_;
		}
		set
		{
			pointId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool InUserWorld
	{
		get
		{
			return inUserWorld_;
		}
		set
		{
			inUserWorld_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long RewardUuid
	{
		get
		{
			return rewardUuid_;
		}
		set
		{
			rewardUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ReportRewardNeedCollect()
	{
	}

	[DebuggerNonUserCode]
	public ReportRewardNeedCollect(ReportRewardNeedCollect other)
		: this()
	{
		pointId_ = other.pointId_;
		inUserWorld_ = other.inUserWorld_;
		rewardUuid_ = other.rewardUuid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ReportRewardNeedCollect Clone()
	{
		return new ReportRewardNeedCollect(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ReportRewardNeedCollect);
	}

	[DebuggerNonUserCode]
	public bool Equals(ReportRewardNeedCollect other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (PointId != other.PointId)
		{
			return false;
		}
		if (InUserWorld != other.InUserWorld)
		{
			return false;
		}
		if (RewardUuid != other.RewardUuid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (PointId != 0)
		{
			num ^= PointId.GetHashCode();
		}
		if (InUserWorld)
		{
			num ^= InUserWorld.GetHashCode();
		}
		if (RewardUuid != 0L)
		{
			num ^= RewardUuid.GetHashCode();
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
		if (PointId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(PointId);
		}
		if (InUserWorld)
		{
			output.WriteRawTag(16);
			output.WriteBool(InUserWorld);
		}
		if (RewardUuid != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(RewardUuid);
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
		if (PointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PointId);
		}
		if (InUserWorld)
		{
			num += 2;
		}
		if (RewardUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(RewardUuid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ReportRewardNeedCollect other)
	{
		if (other != null)
		{
			if (other.PointId != 0)
			{
				PointId = other.PointId;
			}
			if (other.InUserWorld)
			{
				InUserWorld = other.InUserWorld;
			}
			if (other.RewardUuid != 0L)
			{
				RewardUuid = other.RewardUuid;
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
				PointId = input.ReadInt32();
				break;
			case 16u:
				InUserWorld = input.ReadBool();
				break;
			case 24u:
				RewardUuid = input.ReadInt64();
				break;
			}
		}
	}
}
