using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DateTime : IMessage<DateTime>, IMessage, IEquatable<DateTime>, IDeepCloneable<DateTime>
{
	private static readonly MessageParser<DateTime> _parser = new MessageParser<DateTime>(() => new DateTime());

	private UnknownFieldSet _unknownFields;

	public const int MillisecondsFieldNumber = 1;

	private static readonly FieldCodec<long?> _single_milliseconds_codec = FieldCodec.ForStructWrapper<long>(10u);

	private long? milliseconds_;

	public const int SecondsFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_seconds_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? seconds_;

	public const int UtcMillisecondsFieldNumber = 3;

	private static readonly FieldCodec<long?> _single_utcMilliseconds_codec = FieldCodec.ForStructWrapper<long>(26u);

	private long? utcMilliseconds_;

	public const int LocalMillisecondsFieldNumber = 4;

	private static readonly FieldCodec<long?> _single_localMilliseconds_codec = FieldCodec.ForStructWrapper<long>(34u);

	private long? localMilliseconds_;

	[DebuggerNonUserCode]
	public static MessageParser<DateTime> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[17];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long? Milliseconds
	{
		get
		{
			return milliseconds_;
		}
		set
		{
			milliseconds_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? Seconds
	{
		get
		{
			return seconds_;
		}
		set
		{
			seconds_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long? UtcMilliseconds
	{
		get
		{
			return utcMilliseconds_;
		}
		set
		{
			utcMilliseconds_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long? LocalMilliseconds
	{
		get
		{
			return localMilliseconds_;
		}
		set
		{
			localMilliseconds_ = value;
		}
	}

	[DebuggerNonUserCode]
	public DateTime()
	{
	}

	[DebuggerNonUserCode]
	public DateTime(DateTime other)
		: this()
	{
		Milliseconds = other.Milliseconds;
		Seconds = other.Seconds;
		UtcMilliseconds = other.UtcMilliseconds;
		LocalMilliseconds = other.LocalMilliseconds;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DateTime Clone()
	{
		return new DateTime(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DateTime);
	}

	[DebuggerNonUserCode]
	public bool Equals(DateTime other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Milliseconds != other.Milliseconds)
		{
			return false;
		}
		if (Seconds != other.Seconds)
		{
			return false;
		}
		if (UtcMilliseconds != other.UtcMilliseconds)
		{
			return false;
		}
		if (LocalMilliseconds != other.LocalMilliseconds)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (milliseconds_.HasValue)
		{
			num ^= Milliseconds.GetHashCode();
		}
		if (seconds_.HasValue)
		{
			num ^= Seconds.GetHashCode();
		}
		if (utcMilliseconds_.HasValue)
		{
			num ^= UtcMilliseconds.GetHashCode();
		}
		if (localMilliseconds_.HasValue)
		{
			num ^= LocalMilliseconds.GetHashCode();
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
		if (milliseconds_.HasValue)
		{
			_single_milliseconds_codec.WriteTagAndValue(output, Milliseconds);
		}
		if (seconds_.HasValue)
		{
			_single_seconds_codec.WriteTagAndValue(output, Seconds);
		}
		if (utcMilliseconds_.HasValue)
		{
			_single_utcMilliseconds_codec.WriteTagAndValue(output, UtcMilliseconds);
		}
		if (localMilliseconds_.HasValue)
		{
			_single_localMilliseconds_codec.WriteTagAndValue(output, LocalMilliseconds);
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
		if (milliseconds_.HasValue)
		{
			num += _single_milliseconds_codec.CalculateSizeWithTag(Milliseconds);
		}
		if (seconds_.HasValue)
		{
			num += _single_seconds_codec.CalculateSizeWithTag(Seconds);
		}
		if (utcMilliseconds_.HasValue)
		{
			num += _single_utcMilliseconds_codec.CalculateSizeWithTag(UtcMilliseconds);
		}
		if (localMilliseconds_.HasValue)
		{
			num += _single_localMilliseconds_codec.CalculateSizeWithTag(LocalMilliseconds);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DateTime other)
	{
		if (other != null)
		{
			if (other.milliseconds_.HasValue && (!milliseconds_.HasValue || other.Milliseconds != 0))
			{
				Milliseconds = other.Milliseconds;
			}
			if (other.seconds_.HasValue && (!seconds_.HasValue || other.Seconds != 0))
			{
				Seconds = other.Seconds;
			}
			if (other.utcMilliseconds_.HasValue && (!utcMilliseconds_.HasValue || other.UtcMilliseconds != 0))
			{
				UtcMilliseconds = other.UtcMilliseconds;
			}
			if (other.localMilliseconds_.HasValue && (!localMilliseconds_.HasValue || other.LocalMilliseconds != 0))
			{
				LocalMilliseconds = other.LocalMilliseconds;
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
				long? num5 = _single_milliseconds_codec.Read(input);
				if (!milliseconds_.HasValue || num5 != 0)
				{
					Milliseconds = num5;
				}
				break;
			}
			case 18u:
			{
				int? num3 = _single_seconds_codec.Read(input);
				if (!seconds_.HasValue || num3 != 0)
				{
					Seconds = num3;
				}
				break;
			}
			case 26u:
			{
				long? num4 = _single_utcMilliseconds_codec.Read(input);
				if (!utcMilliseconds_.HasValue || num4 != 0)
				{
					UtcMilliseconds = num4;
				}
				break;
			}
			case 34u:
			{
				long? num2 = _single_localMilliseconds_codec.Read(input);
				if (!localMilliseconds_.HasValue || num2 != 0)
				{
					LocalMilliseconds = num2;
				}
				break;
			}
			}
		}
	}
}
