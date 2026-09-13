using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwBattleChampionDuelScore : IMessage<LwBattleChampionDuelScore>, IMessage, IEquatable<LwBattleChampionDuelScore>, IDeepCloneable<LwBattleChampionDuelScore>
{
	private static readonly MessageParser<LwBattleChampionDuelScore> _parser = new MessageParser<LwBattleChampionDuelScore>(() => new LwBattleChampionDuelScore());

	private UnknownFieldSet _unknownFields;

	public const int KillScoreFieldNumber = 1;

	private int killScore_;

	public const int AliveScoreFieldNumber = 2;

	private int aliveScore_;

	public const int WinScoreFieldNumber = 3;

	private int winScore_;

	[DebuggerNonUserCode]
	public static MessageParser<LwBattleChampionDuelScore> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[16];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int KillScore
	{
		get
		{
			return killScore_;
		}
		set
		{
			killScore_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int AliveScore
	{
		get
		{
			return aliveScore_;
		}
		set
		{
			aliveScore_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WinScore
	{
		get
		{
			return winScore_;
		}
		set
		{
			winScore_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LwBattleChampionDuelScore()
	{
	}

	[DebuggerNonUserCode]
	public LwBattleChampionDuelScore(LwBattleChampionDuelScore other)
		: this()
	{
		killScore_ = other.killScore_;
		aliveScore_ = other.aliveScore_;
		winScore_ = other.winScore_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwBattleChampionDuelScore Clone()
	{
		return new LwBattleChampionDuelScore(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwBattleChampionDuelScore);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwBattleChampionDuelScore other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (KillScore != other.KillScore)
		{
			return false;
		}
		if (AliveScore != other.AliveScore)
		{
			return false;
		}
		if (WinScore != other.WinScore)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (KillScore != 0)
		{
			num ^= KillScore.GetHashCode();
		}
		if (AliveScore != 0)
		{
			num ^= AliveScore.GetHashCode();
		}
		if (WinScore != 0)
		{
			num ^= WinScore.GetHashCode();
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
		if (KillScore != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(KillScore);
		}
		if (AliveScore != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(AliveScore);
		}
		if (WinScore != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(WinScore);
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
		if (KillScore != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(KillScore);
		}
		if (AliveScore != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AliveScore);
		}
		if (WinScore != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WinScore);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwBattleChampionDuelScore other)
	{
		if (other != null)
		{
			if (other.KillScore != 0)
			{
				KillScore = other.KillScore;
			}
			if (other.AliveScore != 0)
			{
				AliveScore = other.AliveScore;
			}
			if (other.WinScore != 0)
			{
				WinScore = other.WinScore;
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
				KillScore = input.ReadInt32();
				break;
			case 16u:
				AliveScore = input.ReadInt32();
				break;
			case 24u:
				WinScore = input.ReadInt32();
				break;
			}
		}
	}
}
