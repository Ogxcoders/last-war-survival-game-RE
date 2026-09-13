using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BattleReport : IMessage<BattleReport>, IMessage, IEquatable<BattleReport>, IDeepCloneable<BattleReport>
{
	private static readonly MessageParser<BattleReport> _parser = new MessageParser<BattleReport>(() => new BattleReport());

	private UnknownFieldSet _unknownFields;

	public const int BattleResultFieldNumber = 1;

	private int battleResult_;

	public const int StartTimeFieldNumber = 2;

	private long startTime_;

	public const int StartRoundFieldNumber = 3;

	private int startRound_;

	public const int BattlePointInfoFieldNumber = 4;

	private BattlePointInfo battlePointInfo_;

	public const int FightReportsFieldNumber = 5;

	private static readonly FieldCodec<FightReport> _repeated_fightReports_codec = FieldCodec.ForMessage(42u, FightReport.Parser);

	private readonly RepeatedField<FightReport> fightReports_ = new RepeatedField<FightReport>();

	public const int FightLostFieldNumber = 6;

	private FightLost fightLost_;

	public const int SelfBattleEffectGroupsFieldNumber = 7;

	private static readonly FieldCodec<BattleEffectGroup> _repeated_selfBattleEffectGroups_codec = FieldCodec.ForMessage(58u, BattleEffectGroup.Parser);

	private readonly RepeatedField<BattleEffectGroup> selfBattleEffectGroups_ = new RepeatedField<BattleEffectGroup>();

	public const int VersionFieldNumber = 8;

	private int version_;

	public const int AllFightLostFieldNumber = 9;

	private static readonly FieldCodec<AllFightLost> _repeated_allFightLost_codec = FieldCodec.ForMessage(74u, Protobuf.AllFightLost.Parser);

	private readonly RepeatedField<AllFightLost> allFightLost_ = new RepeatedField<AllFightLost>();

	public const int MarchTypeFieldNumber = 10;

	private int marchType_;

	[DebuggerNonUserCode]
	public static MessageParser<BattleReport> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[30];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int BattleResult
	{
		get
		{
			return battleResult_;
		}
		set
		{
			battleResult_ = value;
		}
	}

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
	public int StartRound
	{
		get
		{
			return startRound_;
		}
		set
		{
			startRound_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BattlePointInfo BattlePointInfo
	{
		get
		{
			return battlePointInfo_;
		}
		set
		{
			battlePointInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<FightReport> FightReports => fightReports_;

	[DebuggerNonUserCode]
	public FightLost FightLost
	{
		get
		{
			return fightLost_;
		}
		set
		{
			fightLost_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<BattleEffectGroup> SelfBattleEffectGroups => selfBattleEffectGroups_;

	[DebuggerNonUserCode]
	public int Version
	{
		get
		{
			return version_;
		}
		set
		{
			version_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<AllFightLost> AllFightLost => allFightLost_;

	[DebuggerNonUserCode]
	public int MarchType
	{
		get
		{
			return marchType_;
		}
		set
		{
			marchType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BattleReport()
	{
	}

	[DebuggerNonUserCode]
	public BattleReport(BattleReport other)
		: this()
	{
		battleResult_ = other.battleResult_;
		startTime_ = other.startTime_;
		startRound_ = other.startRound_;
		battlePointInfo_ = ((other.battlePointInfo_ != null) ? other.battlePointInfo_.Clone() : null);
		fightReports_ = other.fightReports_.Clone();
		fightLost_ = ((other.fightLost_ != null) ? other.fightLost_.Clone() : null);
		selfBattleEffectGroups_ = other.selfBattleEffectGroups_.Clone();
		version_ = other.version_;
		allFightLost_ = other.allFightLost_.Clone();
		marchType_ = other.marchType_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleReport Clone()
	{
		return new BattleReport(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleReport);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleReport other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BattleResult != other.BattleResult)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (StartRound != other.StartRound)
		{
			return false;
		}
		if (!object.Equals(BattlePointInfo, other.BattlePointInfo))
		{
			return false;
		}
		if (!fightReports_.Equals(other.fightReports_))
		{
			return false;
		}
		if (!object.Equals(FightLost, other.FightLost))
		{
			return false;
		}
		if (!selfBattleEffectGroups_.Equals(other.selfBattleEffectGroups_))
		{
			return false;
		}
		if (Version != other.Version)
		{
			return false;
		}
		if (!allFightLost_.Equals(other.allFightLost_))
		{
			return false;
		}
		if (MarchType != other.MarchType)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (BattleResult != 0)
		{
			num ^= BattleResult.GetHashCode();
		}
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (StartRound != 0)
		{
			num ^= StartRound.GetHashCode();
		}
		if (battlePointInfo_ != null)
		{
			num ^= BattlePointInfo.GetHashCode();
		}
		num ^= fightReports_.GetHashCode();
		if (fightLost_ != null)
		{
			num ^= FightLost.GetHashCode();
		}
		num ^= selfBattleEffectGroups_.GetHashCode();
		if (Version != 0)
		{
			num ^= Version.GetHashCode();
		}
		num ^= allFightLost_.GetHashCode();
		if (MarchType != 0)
		{
			num ^= MarchType.GetHashCode();
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
		if (BattleResult != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(BattleResult);
		}
		if (StartTime != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(StartTime);
		}
		if (StartRound != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(StartRound);
		}
		if (battlePointInfo_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(BattlePointInfo);
		}
		fightReports_.WriteTo(output, _repeated_fightReports_codec);
		if (fightLost_ != null)
		{
			output.WriteRawTag(50);
			output.WriteMessage(FightLost);
		}
		selfBattleEffectGroups_.WriteTo(output, _repeated_selfBattleEffectGroups_codec);
		if (Version != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(Version);
		}
		allFightLost_.WriteTo(output, _repeated_allFightLost_codec);
		if (MarchType != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(MarchType);
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
		if (BattleResult != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BattleResult);
		}
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (StartRound != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(StartRound);
		}
		if (battlePointInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(BattlePointInfo);
		}
		num += fightReports_.CalculateSize(_repeated_fightReports_codec);
		if (fightLost_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(FightLost);
		}
		num += selfBattleEffectGroups_.CalculateSize(_repeated_selfBattleEffectGroups_codec);
		if (Version != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Version);
		}
		num += allFightLost_.CalculateSize(_repeated_allFightLost_codec);
		if (MarchType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MarchType);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleReport other)
	{
		if (other == null)
		{
			return;
		}
		if (other.BattleResult != 0)
		{
			BattleResult = other.BattleResult;
		}
		if (other.StartTime != 0L)
		{
			StartTime = other.StartTime;
		}
		if (other.StartRound != 0)
		{
			StartRound = other.StartRound;
		}
		if (other.battlePointInfo_ != null)
		{
			if (battlePointInfo_ == null)
			{
				BattlePointInfo = new BattlePointInfo();
			}
			BattlePointInfo.MergeFrom(other.BattlePointInfo);
		}
		fightReports_.Add(other.fightReports_);
		if (other.fightLost_ != null)
		{
			if (fightLost_ == null)
			{
				FightLost = new FightLost();
			}
			FightLost.MergeFrom(other.FightLost);
		}
		selfBattleEffectGroups_.Add(other.selfBattleEffectGroups_);
		if (other.Version != 0)
		{
			Version = other.Version;
		}
		allFightLost_.Add(other.allFightLost_);
		if (other.MarchType != 0)
		{
			MarchType = other.MarchType;
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
				BattleResult = input.ReadInt32();
				break;
			case 16u:
				StartTime = input.ReadInt64();
				break;
			case 24u:
				StartRound = input.ReadInt32();
				break;
			case 34u:
				if (battlePointInfo_ == null)
				{
					BattlePointInfo = new BattlePointInfo();
				}
				input.ReadMessage(BattlePointInfo);
				break;
			case 42u:
				fightReports_.AddEntriesFrom(input, _repeated_fightReports_codec);
				break;
			case 50u:
				if (fightLost_ == null)
				{
					FightLost = new FightLost();
				}
				input.ReadMessage(FightLost);
				break;
			case 58u:
				selfBattleEffectGroups_.AddEntriesFrom(input, _repeated_selfBattleEffectGroups_codec);
				break;
			case 64u:
				Version = input.ReadInt32();
				break;
			case 74u:
				allFightLost_.AddEntriesFrom(input, _repeated_allFightLost_codec);
				break;
			case 80u:
				MarchType = input.ReadInt32();
				break;
			}
		}
	}
}
