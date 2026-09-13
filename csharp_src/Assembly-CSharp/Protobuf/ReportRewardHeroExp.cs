using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ReportRewardHeroExp : IMessage<ReportRewardHeroExp>, IMessage, IEquatable<ReportRewardHeroExp>, IDeepCloneable<ReportRewardHeroExp>
{
	private static readonly MessageParser<ReportRewardHeroExp> _parser = new MessageParser<ReportRewardHeroExp>(() => new ReportRewardHeroExp());

	private UnknownFieldSet _unknownFields;

	public const int HeroIdFieldNumber = 1;

	private int heroId_;

	public const int LevelFieldNumber = 2;

	private int level_;

	public const int ExpAddFieldNumber = 3;

	private int expAdd_;

	public const int NowExpFieldNumber = 4;

	private int nowExp_;

	public const int OldExpFieldNumber = 5;

	private int oldExp_;

	public const int HeroUuidFieldNumber = 6;

	private long heroUuid_;

	[DebuggerNonUserCode]
	public static MessageParser<ReportRewardHeroExp> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int HeroId
	{
		get
		{
			return heroId_;
		}
		set
		{
			heroId_ = value;
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
	public int ExpAdd
	{
		get
		{
			return expAdd_;
		}
		set
		{
			expAdd_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int NowExp
	{
		get
		{
			return nowExp_;
		}
		set
		{
			nowExp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int OldExp
	{
		get
		{
			return oldExp_;
		}
		set
		{
			oldExp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long HeroUuid
	{
		get
		{
			return heroUuid_;
		}
		set
		{
			heroUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ReportRewardHeroExp()
	{
	}

	[DebuggerNonUserCode]
	public ReportRewardHeroExp(ReportRewardHeroExp other)
		: this()
	{
		heroId_ = other.heroId_;
		level_ = other.level_;
		expAdd_ = other.expAdd_;
		nowExp_ = other.nowExp_;
		oldExp_ = other.oldExp_;
		heroUuid_ = other.heroUuid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ReportRewardHeroExp Clone()
	{
		return new ReportRewardHeroExp(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ReportRewardHeroExp);
	}

	[DebuggerNonUserCode]
	public bool Equals(ReportRewardHeroExp other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (HeroId != other.HeroId)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (ExpAdd != other.ExpAdd)
		{
			return false;
		}
		if (NowExp != other.NowExp)
		{
			return false;
		}
		if (OldExp != other.OldExp)
		{
			return false;
		}
		if (HeroUuid != other.HeroUuid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (HeroId != 0)
		{
			num ^= HeroId.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (ExpAdd != 0)
		{
			num ^= ExpAdd.GetHashCode();
		}
		if (NowExp != 0)
		{
			num ^= NowExp.GetHashCode();
		}
		if (OldExp != 0)
		{
			num ^= OldExp.GetHashCode();
		}
		if (HeroUuid != 0L)
		{
			num ^= HeroUuid.GetHashCode();
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
		if (HeroId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(HeroId);
		}
		if (Level != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Level);
		}
		if (ExpAdd != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(ExpAdd);
		}
		if (NowExp != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(NowExp);
		}
		if (OldExp != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(OldExp);
		}
		if (HeroUuid != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(HeroUuid);
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
		if (HeroId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeroId);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (ExpAdd != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ExpAdd);
		}
		if (NowExp != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(NowExp);
		}
		if (OldExp != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(OldExp);
		}
		if (HeroUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(HeroUuid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ReportRewardHeroExp other)
	{
		if (other != null)
		{
			if (other.HeroId != 0)
			{
				HeroId = other.HeroId;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.ExpAdd != 0)
			{
				ExpAdd = other.ExpAdd;
			}
			if (other.NowExp != 0)
			{
				NowExp = other.NowExp;
			}
			if (other.OldExp != 0)
			{
				OldExp = other.OldExp;
			}
			if (other.HeroUuid != 0L)
			{
				HeroUuid = other.HeroUuid;
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
				HeroId = input.ReadInt32();
				break;
			case 16u:
				Level = input.ReadInt32();
				break;
			case 24u:
				ExpAdd = input.ReadInt32();
				break;
			case 32u:
				NowExp = input.ReadInt32();
				break;
			case 40u:
				OldExp = input.ReadInt32();
				break;
			case 48u:
				HeroUuid = input.ReadInt64();
				break;
			}
		}
	}
}
