using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Pay : IMessage<Pay>, IMessage, IEquatable<Pay>, IDeepCloneable<Pay>
{
	private static readonly MessageParser<Pay> _parser = new MessageParser<Pay>(() => new Pay());

	private UnknownFieldSet _unknownFields;

	public const int GoldFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_gold_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? gold_;

	public const int ChooseFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_choose_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? choose_;

	[DebuggerNonUserCode]
	public static MessageParser<Pay> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? Gold
	{
		get
		{
			return gold_;
		}
		set
		{
			gold_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? Choose
	{
		get
		{
			return choose_;
		}
		set
		{
			choose_ = value;
		}
	}

	[DebuggerNonUserCode]
	public Pay()
	{
	}

	[DebuggerNonUserCode]
	public Pay(Pay other)
		: this()
	{
		Gold = other.Gold;
		Choose = other.Choose;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Pay Clone()
	{
		return new Pay(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Pay);
	}

	[DebuggerNonUserCode]
	public bool Equals(Pay other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Gold != other.Gold)
		{
			return false;
		}
		if (Choose != other.Choose)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (gold_.HasValue)
		{
			num ^= Gold.GetHashCode();
		}
		if (choose_.HasValue)
		{
			num ^= Choose.GetHashCode();
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
		if (gold_.HasValue)
		{
			_single_gold_codec.WriteTagAndValue(output, Gold);
		}
		if (choose_.HasValue)
		{
			_single_choose_codec.WriteTagAndValue(output, Choose);
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
		if (gold_.HasValue)
		{
			num += _single_gold_codec.CalculateSizeWithTag(Gold);
		}
		if (choose_.HasValue)
		{
			num += _single_choose_codec.CalculateSizeWithTag(Choose);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Pay other)
	{
		if (other != null)
		{
			if (other.gold_.HasValue && (!gold_.HasValue || other.Gold != 0))
			{
				Gold = other.Gold;
			}
			if (other.choose_.HasValue && (!choose_.HasValue || other.Choose != 0))
			{
				Choose = other.Choose;
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
			{
				int? num3 = _single_gold_codec.Read(input);
				if (!gold_.HasValue || num3 != 0)
				{
					Gold = num3;
				}
				break;
			}
			case 18u:
			{
				int? num2 = _single_choose_codec.Read(input);
				if (!choose_.HasValue || num2 != 0)
				{
					Choose = num2;
				}
				break;
			}
			}
		}
	}
}
