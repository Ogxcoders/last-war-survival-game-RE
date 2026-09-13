using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class RewardInfo : IMessage<RewardInfo>, IMessage, IEquatable<RewardInfo>, IDeepCloneable<RewardInfo>
{
	private static readonly MessageParser<RewardInfo> _parser = new MessageParser<RewardInfo>(() => new RewardInfo());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_type_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? type_;

	public const int IdFieldNumber = 2;

	private static readonly FieldCodec<string> _single_id_codec = FieldCodec.ForClassWrapper<string>(18u);

	private string id_;

	public const int NumFieldNumber = 3;

	private static readonly FieldCodec<long?> _single_num_codec = FieldCodec.ForStructWrapper<long>(26u);

	private long? num_;

	[DebuggerNonUserCode]
	public static MessageParser<RewardInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? Type
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
	public string Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long? Num
	{
		get
		{
			return num_;
		}
		set
		{
			num_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RewardInfo()
	{
	}

	[DebuggerNonUserCode]
	public RewardInfo(RewardInfo other)
		: this()
	{
		Type = other.Type;
		Id = other.Id;
		Num = other.Num;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RewardInfo Clone()
	{
		return new RewardInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RewardInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(RewardInfo other)
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
		if (Id != other.Id)
		{
			return false;
		}
		if (Num != other.Num)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (type_.HasValue)
		{
			num ^= Type.GetHashCode();
		}
		if (id_ != null)
		{
			num ^= Id.GetHashCode();
		}
		if (num_.HasValue)
		{
			num ^= Num.GetHashCode();
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
		if (type_.HasValue)
		{
			_single_type_codec.WriteTagAndValue(output, Type);
		}
		if (id_ != null)
		{
			_single_id_codec.WriteTagAndValue(output, Id);
		}
		if (num_.HasValue)
		{
			_single_num_codec.WriteTagAndValue(output, Num);
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
		if (type_.HasValue)
		{
			num += _single_type_codec.CalculateSizeWithTag(Type);
		}
		if (id_ != null)
		{
			num += _single_id_codec.CalculateSizeWithTag(Id);
		}
		if (num_.HasValue)
		{
			num += _single_num_codec.CalculateSizeWithTag(Num);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RewardInfo other)
	{
		if (other != null)
		{
			if (other.type_.HasValue && (!type_.HasValue || other.Type != 0))
			{
				Type = other.Type;
			}
			if (other.id_ != null && (id_ == null || other.Id != ""))
			{
				Id = other.Id;
			}
			if (other.num_.HasValue && (!num_.HasValue || other.Num != 0))
			{
				Num = other.Num;
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
				int? num3 = _single_type_codec.Read(input);
				if (!type_.HasValue || num3 != 0)
				{
					Type = num3;
				}
				break;
			}
			case 18u:
			{
				string text = _single_id_codec.Read(input);
				if (id_ == null || text != "")
				{
					Id = text;
				}
				break;
			}
			case 26u:
			{
				long? num2 = _single_num_codec.Read(input);
				if (!num_.HasValue || num2 != 0)
				{
					Num = num2;
				}
				break;
			}
			}
		}
	}
}
