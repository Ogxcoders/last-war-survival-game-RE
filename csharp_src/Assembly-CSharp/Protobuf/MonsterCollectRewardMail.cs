using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MonsterCollectRewardMail : IMessage<MonsterCollectRewardMail>, IMessage, IEquatable<MonsterCollectRewardMail>, IDeepCloneable<MonsterCollectRewardMail>
{
	private static readonly MessageParser<MonsterCollectRewardMail> _parser = new MessageParser<MonsterCollectRewardMail>(() => new MonsterCollectRewardMail());

	private UnknownFieldSet _unknownFields;

	public const int StartTimeFieldNumber = 1;

	private long startTime_;

	public const int MonsterIdFieldNumber = 2;

	private int monsterId_;

	public const int NeedCollectRewardFieldNumber = 3;

	private ReportRewardNeedCollect needCollectReward_;

	[DebuggerNonUserCode]
	public static MessageParser<MonsterCollectRewardMail> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[38];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long StartTime
	{
		get
		{
			return startTime_;
		}
		set
		{
			startTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MonsterId
	{
		get
		{
			return monsterId_;
		}
		set
		{
			monsterId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ReportRewardNeedCollect NeedCollectReward
	{
		get
		{
			return needCollectReward_;
		}
		set
		{
			needCollectReward_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MonsterCollectRewardMail()
	{
	}

	[DebuggerNonUserCode]
	public MonsterCollectRewardMail(MonsterCollectRewardMail other)
		: this()
	{
		startTime_ = other.startTime_;
		monsterId_ = other.monsterId_;
		needCollectReward_ = ((other.needCollectReward_ != null) ? other.needCollectReward_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MonsterCollectRewardMail Clone()
	{
		return new MonsterCollectRewardMail(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MonsterCollectRewardMail);
	}

	[DebuggerNonUserCode]
	public bool Equals(MonsterCollectRewardMail other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (MonsterId != other.MonsterId)
		{
			return false;
		}
		if (!object.Equals(NeedCollectReward, other.NeedCollectReward))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (MonsterId != 0)
		{
			num ^= MonsterId.GetHashCode();
		}
		if (needCollectReward_ != null)
		{
			num ^= NeedCollectReward.GetHashCode();
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
		if (StartTime != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(StartTime);
		}
		if (MonsterId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(MonsterId);
		}
		if (needCollectReward_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(NeedCollectReward);
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
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (MonsterId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MonsterId);
		}
		if (needCollectReward_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(NeedCollectReward);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MonsterCollectRewardMail other)
	{
		if (other == null)
		{
			return;
		}
		if (other.StartTime != 0L)
		{
			StartTime = other.StartTime;
		}
		if (other.MonsterId != 0)
		{
			MonsterId = other.MonsterId;
		}
		if (other.needCollectReward_ != null)
		{
			if (needCollectReward_ == null)
			{
				NeedCollectReward = new ReportRewardNeedCollect();
			}
			NeedCollectReward.MergeFrom(other.NeedCollectReward);
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				StartTime = input.ReadInt64();
				break;
			case 16u:
				MonsterId = input.ReadInt32();
				break;
			case 26u:
				if (needCollectReward_ == null)
				{
					NeedCollectReward = new ReportRewardNeedCollect();
				}
				input.ReadMessage(NeedCollectReward);
				break;
			}
		}
	}
}
