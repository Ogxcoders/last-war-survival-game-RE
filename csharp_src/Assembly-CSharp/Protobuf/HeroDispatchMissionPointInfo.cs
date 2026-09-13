using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class HeroDispatchMissionPointInfo : IMessage<HeroDispatchMissionPointInfo>, IMessage, IEquatable<HeroDispatchMissionPointInfo>, IDeepCloneable<HeroDispatchMissionPointInfo>
{
	private static readonly MessageParser<HeroDispatchMissionPointInfo> _parser = new MessageParser<HeroDispatchMissionPointInfo>(() => new HeroDispatchMissionPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int OwnerUidFieldNumber = 1;

	private string ownerUid_ = "";

	public const int CfgIdFieldNumber = 2;

	private int cfgId_;

	public const int CompletionTimeFieldNumber = 3;

	private long completionTime_;

	public const int StealListFieldNumber = 4;

	private static readonly FieldCodec<string> _repeated_stealList_codec = FieldCodec.ForString(34u);

	private readonly RepeatedField<string> stealList_ = new RepeatedField<string>();

	public const int AccListFieldNumber = 5;

	private static readonly FieldCodec<string> _repeated_accList_codec = FieldCodec.ForString(42u);

	private readonly RepeatedField<string> accList_ = new RepeatedField<string>();

	public const int HeroListFieldNumber = 6;

	private static readonly FieldCodec<long> _repeated_heroList_codec = FieldCodec.ForInt64(50u);

	private readonly RepeatedField<long> heroList_ = new RepeatedField<long>();

	public const int RewardedFieldNumber = 7;

	private int rewarded_;

	public const int ActEndTimeFieldNumber = 8;

	private long actEndTime_;

	public const int AllianceIdFieldNumber = 9;

	private string allianceId_ = "";

	public const int SizeFieldNumber = 10;

	private int size_;

	public const int ConditionListFieldNumber = 11;

	private string conditionList_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<HeroDispatchMissionPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[44];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string OwnerUid
	{
		get
		{
			return ownerUid_;
		}
		set
		{
			ownerUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int CfgId
	{
		get
		{
			return cfgId_;
		}
		set
		{
			cfgId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CompletionTime
	{
		get
		{
			return completionTime_;
		}
		set
		{
			completionTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<string> StealList => stealList_;

	[DebuggerNonUserCode]
	public RepeatedField<string> AccList => accList_;

	[DebuggerNonUserCode]
	public RepeatedField<long> HeroList => heroList_;

	[DebuggerNonUserCode]
	public int Rewarded
	{
		get
		{
			return rewarded_;
		}
		set
		{
			rewarded_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ActEndTime
	{
		get
		{
			return actEndTime_;
		}
		set
		{
			actEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string AllianceId
	{
		get
		{
			return allianceId_;
		}
		set
		{
			allianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Size
	{
		get
		{
			return size_;
		}
		set
		{
			size_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string ConditionList
	{
		get
		{
			return conditionList_;
		}
		set
		{
			conditionList_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public HeroDispatchMissionPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public HeroDispatchMissionPointInfo(HeroDispatchMissionPointInfo other)
		: this()
	{
		ownerUid_ = other.ownerUid_;
		cfgId_ = other.cfgId_;
		completionTime_ = other.completionTime_;
		stealList_ = other.stealList_.Clone();
		accList_ = other.accList_.Clone();
		heroList_ = other.heroList_.Clone();
		rewarded_ = other.rewarded_;
		actEndTime_ = other.actEndTime_;
		allianceId_ = other.allianceId_;
		size_ = other.size_;
		conditionList_ = other.conditionList_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public HeroDispatchMissionPointInfo Clone()
	{
		return new HeroDispatchMissionPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as HeroDispatchMissionPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(HeroDispatchMissionPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (OwnerUid != other.OwnerUid)
		{
			return false;
		}
		if (CfgId != other.CfgId)
		{
			return false;
		}
		if (CompletionTime != other.CompletionTime)
		{
			return false;
		}
		if (!stealList_.Equals(other.stealList_))
		{
			return false;
		}
		if (!accList_.Equals(other.accList_))
		{
			return false;
		}
		if (!heroList_.Equals(other.heroList_))
		{
			return false;
		}
		if (Rewarded != other.Rewarded)
		{
			return false;
		}
		if (ActEndTime != other.ActEndTime)
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (Size != other.Size)
		{
			return false;
		}
		if (ConditionList != other.ConditionList)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (OwnerUid.Length != 0)
		{
			num ^= OwnerUid.GetHashCode();
		}
		if (CfgId != 0)
		{
			num ^= CfgId.GetHashCode();
		}
		if (CompletionTime != 0L)
		{
			num ^= CompletionTime.GetHashCode();
		}
		num ^= stealList_.GetHashCode();
		num ^= accList_.GetHashCode();
		num ^= heroList_.GetHashCode();
		if (Rewarded != 0)
		{
			num ^= Rewarded.GetHashCode();
		}
		if (ActEndTime != 0L)
		{
			num ^= ActEndTime.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (Size != 0)
		{
			num ^= Size.GetHashCode();
		}
		if (ConditionList.Length != 0)
		{
			num ^= ConditionList.GetHashCode();
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
		if (OwnerUid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(OwnerUid);
		}
		if (CfgId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(CfgId);
		}
		if (CompletionTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(CompletionTime);
		}
		stealList_.WriteTo(output, _repeated_stealList_codec);
		accList_.WriteTo(output, _repeated_accList_codec);
		heroList_.WriteTo(output, _repeated_heroList_codec);
		if (Rewarded != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Rewarded);
		}
		if (ActEndTime != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(ActEndTime);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(74);
			output.WriteString(AllianceId);
		}
		if (Size != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(Size);
		}
		if (ConditionList.Length != 0)
		{
			output.WriteRawTag(90);
			output.WriteString(ConditionList);
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
		if (OwnerUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(OwnerUid);
		}
		if (CfgId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CfgId);
		}
		if (CompletionTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CompletionTime);
		}
		num += stealList_.CalculateSize(_repeated_stealList_codec);
		num += accList_.CalculateSize(_repeated_accList_codec);
		num += heroList_.CalculateSize(_repeated_heroList_codec);
		if (Rewarded != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Rewarded);
		}
		if (ActEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ActEndTime);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (Size != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Size);
		}
		if (ConditionList.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ConditionList);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(HeroDispatchMissionPointInfo other)
	{
		if (other != null)
		{
			if (other.OwnerUid.Length != 0)
			{
				OwnerUid = other.OwnerUid;
			}
			if (other.CfgId != 0)
			{
				CfgId = other.CfgId;
			}
			if (other.CompletionTime != 0L)
			{
				CompletionTime = other.CompletionTime;
			}
			stealList_.Add(other.stealList_);
			accList_.Add(other.accList_);
			heroList_.Add(other.heroList_);
			if (other.Rewarded != 0)
			{
				Rewarded = other.Rewarded;
			}
			if (other.ActEndTime != 0L)
			{
				ActEndTime = other.ActEndTime;
			}
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.Size != 0)
			{
				Size = other.Size;
			}
			if (other.ConditionList.Length != 0)
			{
				ConditionList = other.ConditionList;
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
				OwnerUid = input.ReadString();
				break;
			case 16u:
				CfgId = input.ReadInt32();
				break;
			case 24u:
				CompletionTime = input.ReadInt64();
				break;
			case 34u:
				stealList_.AddEntriesFrom(input, _repeated_stealList_codec);
				break;
			case 42u:
				accList_.AddEntriesFrom(input, _repeated_accList_codec);
				break;
			case 48u:
			case 50u:
				heroList_.AddEntriesFrom(input, _repeated_heroList_codec);
				break;
			case 56u:
				Rewarded = input.ReadInt32();
				break;
			case 64u:
				ActEndTime = input.ReadInt64();
				break;
			case 74u:
				AllianceId = input.ReadString();
				break;
			case 80u:
				Size = input.ReadInt32();
				break;
			case 90u:
				ConditionList = input.ReadString();
				break;
			}
		}
	}
}
