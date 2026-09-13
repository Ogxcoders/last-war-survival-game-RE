using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class FightLost : IMessage<FightLost>, IMessage, IEquatable<FightLost>, IDeepCloneable<FightLost>
{
	private static readonly MessageParser<FightLost> _parser = new MessageParser<FightLost>(() => new FightLost());

	private UnknownFieldSet _unknownFields;

	public const int ResLostArrFieldNumber = 1;

	private static readonly FieldCodec<FightResLost> _repeated_resLostArr_codec = FieldCodec.ForMessage(10u, FightResLost.Parser);

	private readonly RepeatedField<FightResLost> resLostArr_ = new RepeatedField<FightResLost>();

	public const int ResItemLostArrFieldNumber = 2;

	private static readonly FieldCodec<FightResItemLost> _repeated_resItemLostArr_codec = FieldCodec.ForMessage(18u, FightResItemLost.Parser);

	private readonly RepeatedField<FightResItemLost> resItemLostArr_ = new RepeatedField<FightResItemLost>();

	[DebuggerNonUserCode]
	public static MessageParser<FightLost> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[28];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<FightResLost> ResLostArr => resLostArr_;

	[DebuggerNonUserCode]
	public RepeatedField<FightResItemLost> ResItemLostArr => resItemLostArr_;

	[DebuggerNonUserCode]
	public FightLost()
	{
	}

	[DebuggerNonUserCode]
	public FightLost(FightLost other)
		: this()
	{
		resLostArr_ = other.resLostArr_.Clone();
		resItemLostArr_ = other.resItemLostArr_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public FightLost Clone()
	{
		return new FightLost(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as FightLost);
	}

	[DebuggerNonUserCode]
	public bool Equals(FightLost other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!resLostArr_.Equals(other.resLostArr_))
		{
			return false;
		}
		if (!resItemLostArr_.Equals(other.resItemLostArr_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= resLostArr_.GetHashCode();
		num ^= resItemLostArr_.GetHashCode();
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
		resLostArr_.WriteTo(output, _repeated_resLostArr_codec);
		resItemLostArr_.WriteTo(output, _repeated_resItemLostArr_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += resLostArr_.CalculateSize(_repeated_resLostArr_codec);
		num += resItemLostArr_.CalculateSize(_repeated_resItemLostArr_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(FightLost other)
	{
		if (other != null)
		{
			resLostArr_.Add(other.resLostArr_);
			resItemLostArr_.Add(other.resItemLostArr_);
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
				resLostArr_.AddEntriesFrom(input, _repeated_resLostArr_codec);
				break;
			case 18u:
				resItemLostArr_.AddEntriesFrom(input, _repeated_resItemLostArr_codec);
				break;
			}
		}
	}
}
