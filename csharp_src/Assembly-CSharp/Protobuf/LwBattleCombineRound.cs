using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwBattleCombineRound : IMessage<LwBattleCombineRound>, IMessage, IEquatable<LwBattleCombineRound>, IDeepCloneable<LwBattleCombineRound>
{
	private static readonly MessageParser<LwBattleCombineRound> _parser = new MessageParser<LwBattleCombineRound>(() => new LwBattleCombineRound());

	private UnknownFieldSet _unknownFields;

	public const int BattleFieldNumber = 1;

	private static readonly FieldCodec<LwBattleReport> _repeated_battle_codec = FieldCodec.ForMessage(10u, LwBattleReport.Parser);

	private readonly RepeatedField<LwBattleReport> battle_ = new RepeatedField<LwBattleReport>();

	public const int RoundIndexFieldNumber = 2;

	private int roundIndex_;

	[DebuggerNonUserCode]
	public static MessageParser<LwBattleCombineRound> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[17];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<LwBattleReport> Battle => battle_;

	[DebuggerNonUserCode]
	public int RoundIndex
	{
		get
		{
			return roundIndex_;
		}
		set
		{
			roundIndex_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LwBattleCombineRound()
	{
	}

	[DebuggerNonUserCode]
	public LwBattleCombineRound(LwBattleCombineRound other)
		: this()
	{
		battle_ = other.battle_.Clone();
		roundIndex_ = other.roundIndex_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwBattleCombineRound Clone()
	{
		return new LwBattleCombineRound(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwBattleCombineRound);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwBattleCombineRound other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!battle_.Equals(other.battle_))
		{
			return false;
		}
		if (RoundIndex != other.RoundIndex)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= battle_.GetHashCode();
		if (RoundIndex != 0)
		{
			num ^= RoundIndex.GetHashCode();
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
		battle_.WriteTo(output, _repeated_battle_codec);
		if (RoundIndex != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(RoundIndex);
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
		num += battle_.CalculateSize(_repeated_battle_codec);
		if (RoundIndex != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RoundIndex);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwBattleCombineRound other)
	{
		if (other != null)
		{
			battle_.Add(other.battle_);
			if (other.RoundIndex != 0)
			{
				RoundIndex = other.RoundIndex;
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
				battle_.AddEntriesFrom(input, _repeated_battle_codec);
				break;
			case 16u:
				RoundIndex = input.ReadInt32();
				break;
			}
		}
	}
}
