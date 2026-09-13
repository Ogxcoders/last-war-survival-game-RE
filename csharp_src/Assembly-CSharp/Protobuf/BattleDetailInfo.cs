using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BattleDetailInfo : IMessage<BattleDetailInfo>, IMessage, IEquatable<BattleDetailInfo>, IDeepCloneable<BattleDetailInfo>
{
	private static readonly MessageParser<BattleDetailInfo> _parser = new MessageParser<BattleDetailInfo>(() => new BattleDetailInfo());

	private UnknownFieldSet _unknownFields;

	public const int RoundReportsFieldNumber = 1;

	private static readonly FieldCodec<BaseRoundReport> _repeated_roundReports_codec = FieldCodec.ForMessage(10u, BaseRoundReport.Parser);

	private readonly RepeatedField<BaseRoundReport> roundReports_ = new RepeatedField<BaseRoundReport>();

	public const int EffectReportsFieldNumber = 2;

	private static readonly FieldCodec<EffectRoundReport> _repeated_effectReports_codec = FieldCodec.ForMessage(18u, EffectRoundReport.Parser);

	private readonly RepeatedField<EffectRoundReport> effectReports_ = new RepeatedField<EffectRoundReport>();

	public const int PlayerInfosFieldNumber = 3;

	private static readonly FieldCodec<DetailReportPlayerInfo> _repeated_playerInfos_codec = FieldCodec.ForMessage(26u, DetailReportPlayerInfo.Parser);

	private readonly RepeatedField<DetailReportPlayerInfo> playerInfos_ = new RepeatedField<DetailReportPlayerInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<BattleDetailInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[32];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<BaseRoundReport> RoundReports => roundReports_;

	[DebuggerNonUserCode]
	public RepeatedField<EffectRoundReport> EffectReports => effectReports_;

	[DebuggerNonUserCode]
	public RepeatedField<DetailReportPlayerInfo> PlayerInfos => playerInfos_;

	[DebuggerNonUserCode]
	public BattleDetailInfo()
	{
	}

	[DebuggerNonUserCode]
	public BattleDetailInfo(BattleDetailInfo other)
		: this()
	{
		roundReports_ = other.roundReports_.Clone();
		effectReports_ = other.effectReports_.Clone();
		playerInfos_ = other.playerInfos_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleDetailInfo Clone()
	{
		return new BattleDetailInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleDetailInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleDetailInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!roundReports_.Equals(other.roundReports_))
		{
			return false;
		}
		if (!effectReports_.Equals(other.effectReports_))
		{
			return false;
		}
		if (!playerInfos_.Equals(other.playerInfos_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= roundReports_.GetHashCode();
		num ^= effectReports_.GetHashCode();
		num ^= playerInfos_.GetHashCode();
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
		roundReports_.WriteTo(output, _repeated_roundReports_codec);
		effectReports_.WriteTo(output, _repeated_effectReports_codec);
		playerInfos_.WriteTo(output, _repeated_playerInfos_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += roundReports_.CalculateSize(_repeated_roundReports_codec);
		num += effectReports_.CalculateSize(_repeated_effectReports_codec);
		num += playerInfos_.CalculateSize(_repeated_playerInfos_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleDetailInfo other)
	{
		if (other != null)
		{
			roundReports_.Add(other.roundReports_);
			effectReports_.Add(other.effectReports_);
			playerInfos_.Add(other.playerInfos_);
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
				roundReports_.AddEntriesFrom(input, _repeated_roundReports_codec);
				break;
			case 18u:
				effectReports_.AddEntriesFrom(input, _repeated_effectReports_codec);
				break;
			case 26u:
				playerInfos_.AddEntriesFrom(input, _repeated_playerInfos_codec);
				break;
			}
		}
	}
}
