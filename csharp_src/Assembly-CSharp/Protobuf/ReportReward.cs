using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ReportReward : IMessage<ReportReward>, IMessage, IEquatable<ReportReward>, IDeepCloneable<ReportReward>
{
	private static readonly MessageParser<ReportReward> _parser = new MessageParser<ReportReward>(() => new ReportReward());

	private UnknownFieldSet _unknownFields;

	public const int RewardItemsFieldNumber = 1;

	private static readonly FieldCodec<ReportRewardItem> _repeated_rewardItems_codec = FieldCodec.ForMessage(10u, ReportRewardItem.Parser);

	private readonly RepeatedField<ReportRewardItem> rewardItems_ = new RepeatedField<ReportRewardItem>();

	public const int RewardResourceItemsFieldNumber = 2;

	private static readonly FieldCodec<ReportRewardItem> _repeated_rewardResourceItems_codec = FieldCodec.ForMessage(18u, ReportRewardItem.Parser);

	private readonly RepeatedField<ReportRewardItem> rewardResourceItems_ = new RepeatedField<ReportRewardItem>();

	public const int RewardHeroExpsFieldNumber = 3;

	private static readonly FieldCodec<ReportRewardHeroExp> _repeated_rewardHeroExps_codec = FieldCodec.ForMessage(26u, ReportRewardHeroExp.Parser);

	private readonly RepeatedField<ReportRewardHeroExp> rewardHeroExps_ = new RepeatedField<ReportRewardHeroExp>();

	public const int RewardResourcesFieldNumber = 4;

	private static readonly FieldCodec<ReportRewardResource> _repeated_rewardResources_codec = FieldCodec.ForMessage(34u, ReportRewardResource.Parser);

	private readonly RepeatedField<ReportRewardResource> rewardResources_ = new RepeatedField<ReportRewardResource>();

	public const int PlunderResRateFieldNumber = 5;

	private int plunderResRate_;

	[DebuggerNonUserCode]
	public static MessageParser<ReportReward> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<ReportRewardItem> RewardItems => rewardItems_;

	[DebuggerNonUserCode]
	public RepeatedField<ReportRewardItem> RewardResourceItems => rewardResourceItems_;

	[DebuggerNonUserCode]
	public RepeatedField<ReportRewardHeroExp> RewardHeroExps => rewardHeroExps_;

	[DebuggerNonUserCode]
	public RepeatedField<ReportRewardResource> RewardResources => rewardResources_;

	[DebuggerNonUserCode]
	public int PlunderResRate
	{
		get
		{
			return plunderResRate_;
		}
		set
		{
			plunderResRate_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ReportReward()
	{
	}

	[DebuggerNonUserCode]
	public ReportReward(ReportReward other)
		: this()
	{
		rewardItems_ = other.rewardItems_.Clone();
		rewardResourceItems_ = other.rewardResourceItems_.Clone();
		rewardHeroExps_ = other.rewardHeroExps_.Clone();
		rewardResources_ = other.rewardResources_.Clone();
		plunderResRate_ = other.plunderResRate_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ReportReward Clone()
	{
		return new ReportReward(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ReportReward);
	}

	[DebuggerNonUserCode]
	public bool Equals(ReportReward other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!rewardItems_.Equals(other.rewardItems_))
		{
			return false;
		}
		if (!rewardResourceItems_.Equals(other.rewardResourceItems_))
		{
			return false;
		}
		if (!rewardHeroExps_.Equals(other.rewardHeroExps_))
		{
			return false;
		}
		if (!rewardResources_.Equals(other.rewardResources_))
		{
			return false;
		}
		if (PlunderResRate != other.PlunderResRate)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= rewardItems_.GetHashCode();
		num ^= rewardResourceItems_.GetHashCode();
		num ^= rewardHeroExps_.GetHashCode();
		num ^= rewardResources_.GetHashCode();
		if (PlunderResRate != 0)
		{
			num ^= PlunderResRate.GetHashCode();
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
		rewardItems_.WriteTo(output, _repeated_rewardItems_codec);
		rewardResourceItems_.WriteTo(output, _repeated_rewardResourceItems_codec);
		rewardHeroExps_.WriteTo(output, _repeated_rewardHeroExps_codec);
		rewardResources_.WriteTo(output, _repeated_rewardResources_codec);
		if (PlunderResRate != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(PlunderResRate);
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
		num += rewardItems_.CalculateSize(_repeated_rewardItems_codec);
		num += rewardResourceItems_.CalculateSize(_repeated_rewardResourceItems_codec);
		num += rewardHeroExps_.CalculateSize(_repeated_rewardHeroExps_codec);
		num += rewardResources_.CalculateSize(_repeated_rewardResources_codec);
		if (PlunderResRate != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PlunderResRate);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ReportReward other)
	{
		if (other != null)
		{
			rewardItems_.Add(other.rewardItems_);
			rewardResourceItems_.Add(other.rewardResourceItems_);
			rewardHeroExps_.Add(other.rewardHeroExps_);
			rewardResources_.Add(other.rewardResources_);
			if (other.PlunderResRate != 0)
			{
				PlunderResRate = other.PlunderResRate;
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
				rewardItems_.AddEntriesFrom(input, _repeated_rewardItems_codec);
				break;
			case 18u:
				rewardResourceItems_.AddEntriesFrom(input, _repeated_rewardResourceItems_codec);
				break;
			case 26u:
				rewardHeroExps_.AddEntriesFrom(input, _repeated_rewardHeroExps_codec);
				break;
			case 34u:
				rewardResources_.AddEntriesFrom(input, _repeated_rewardResources_codec);
				break;
			case 40u:
				PlunderResRate = input.ReadInt32();
				break;
			}
		}
	}
}
