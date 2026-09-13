using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BattleRoundPushInfo : IMessage<BattleRoundPushInfo>, IMessage, IEquatable<BattleRoundPushInfo>, IDeepCloneable<BattleRoundPushInfo>
{
	private static readonly MessageParser<BattleRoundPushInfo> _parser = new MessageParser<BattleRoundPushInfo>(() => new BattleRoundPushInfo());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private int type_;

	public const int SimpleArmyInfoFieldNumber = 2;

	private SimpleSelfArmyInfo simpleArmyInfo_;

	public const int CombineArmyInfoFieldNumber = 3;

	private CombineSelfArmyInfo combineArmyInfo_;

	public const int OutRangeFieldNumber = 4;

	private bool outRange_;

	public const int RoundReportsFieldNumber = 5;

	private static readonly FieldCodec<BaseRoundReportPush> _repeated_roundReports_codec = FieldCodec.ForMessage(42u, BaseRoundReportPush.Parser);

	private readonly RepeatedField<BaseRoundReportPush> roundReports_ = new RepeatedField<BaseRoundReportPush>();

	[DebuggerNonUserCode]
	public static MessageParser<BattleRoundPushInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleRoundPushReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public SimpleSelfArmyInfo SimpleArmyInfo
	{
		get
		{
			return simpleArmyInfo_;
		}
		set
		{
			simpleArmyInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CombineSelfArmyInfo CombineArmyInfo
	{
		get
		{
			return combineArmyInfo_;
		}
		set
		{
			combineArmyInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool OutRange
	{
		get
		{
			return outRange_;
		}
		set
		{
			outRange_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<BaseRoundReportPush> RoundReports => roundReports_;

	[DebuggerNonUserCode]
	public BattleRoundPushInfo()
	{
	}

	[DebuggerNonUserCode]
	public BattleRoundPushInfo(BattleRoundPushInfo other)
		: this()
	{
		type_ = other.type_;
		simpleArmyInfo_ = ((other.simpleArmyInfo_ != null) ? other.simpleArmyInfo_.Clone() : null);
		combineArmyInfo_ = ((other.combineArmyInfo_ != null) ? other.combineArmyInfo_.Clone() : null);
		outRange_ = other.outRange_;
		roundReports_ = other.roundReports_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleRoundPushInfo Clone()
	{
		return new BattleRoundPushInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleRoundPushInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleRoundPushInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (!object.Equals(SimpleArmyInfo, other.SimpleArmyInfo))
		{
			return false;
		}
		if (!object.Equals(CombineArmyInfo, other.CombineArmyInfo))
		{
			return false;
		}
		if (OutRange != other.OutRange)
		{
			return false;
		}
		if (!roundReports_.Equals(other.roundReports_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (simpleArmyInfo_ != null)
		{
			num ^= SimpleArmyInfo.GetHashCode();
		}
		if (combineArmyInfo_ != null)
		{
			num ^= CombineArmyInfo.GetHashCode();
		}
		if (OutRange)
		{
			num ^= OutRange.GetHashCode();
		}
		num ^= roundReports_.GetHashCode();
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
		if (Type != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Type);
		}
		if (simpleArmyInfo_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(SimpleArmyInfo);
		}
		if (combineArmyInfo_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(CombineArmyInfo);
		}
		if (OutRange)
		{
			output.WriteRawTag(32);
			output.WriteBool(OutRange);
		}
		roundReports_.WriteTo(output, _repeated_roundReports_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (simpleArmyInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SimpleArmyInfo);
		}
		if (combineArmyInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(CombineArmyInfo);
		}
		if (OutRange)
		{
			num += 2;
		}
		num += roundReports_.CalculateSize(_repeated_roundReports_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleRoundPushInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Type != 0)
		{
			Type = other.Type;
		}
		if (other.simpleArmyInfo_ != null)
		{
			if (simpleArmyInfo_ == null)
			{
				SimpleArmyInfo = new SimpleSelfArmyInfo();
			}
			SimpleArmyInfo.MergeFrom(other.SimpleArmyInfo);
		}
		if (other.combineArmyInfo_ != null)
		{
			if (combineArmyInfo_ == null)
			{
				CombineArmyInfo = new CombineSelfArmyInfo();
			}
			CombineArmyInfo.MergeFrom(other.CombineArmyInfo);
		}
		if (other.OutRange)
		{
			OutRange = other.OutRange;
		}
		roundReports_.Add(other.roundReports_);
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
				Type = input.ReadInt32();
				break;
			case 18u:
				if (simpleArmyInfo_ == null)
				{
					SimpleArmyInfo = new SimpleSelfArmyInfo();
				}
				input.ReadMessage(SimpleArmyInfo);
				break;
			case 26u:
				if (combineArmyInfo_ == null)
				{
					CombineArmyInfo = new CombineSelfArmyInfo();
				}
				input.ReadMessage(CombineArmyInfo);
				break;
			case 32u:
				OutRange = input.ReadBool();
				break;
			case 42u:
				roundReports_.AddEntriesFrom(input, _repeated_roundReports_codec);
				break;
			}
		}
	}
}
