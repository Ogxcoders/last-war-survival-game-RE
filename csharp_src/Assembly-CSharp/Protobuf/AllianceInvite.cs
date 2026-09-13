using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceInvite : IMessage<AllianceInvite>, IMessage, IEquatable<AllianceInvite>, IDeepCloneable<AllianceInvite>
{
	private static readonly MessageParser<AllianceInvite> _parser = new MessageParser<AllianceInvite>(() => new AllianceInvite());

	private UnknownFieldSet _unknownFields;

	public const int ExpireTimeFieldNumber = 1;

	private static readonly FieldCodec<long?> _single_expireTime_codec = FieldCodec.ForStructWrapper<long>(10u);

	private long? expireTime_;

	[DebuggerNonUserCode]
	public static MessageParser<AllianceInvite> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[34];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long? ExpireTime
	{
		get
		{
			return expireTime_;
		}
		set
		{
			expireTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceInvite()
	{
	}

	[DebuggerNonUserCode]
	public AllianceInvite(AllianceInvite other)
		: this()
	{
		ExpireTime = other.ExpireTime;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceInvite Clone()
	{
		return new AllianceInvite(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceInvite);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceInvite other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ExpireTime != other.ExpireTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (expireTime_.HasValue)
		{
			num ^= ExpireTime.GetHashCode();
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
		if (expireTime_.HasValue)
		{
			_single_expireTime_codec.WriteTagAndValue(output, ExpireTime);
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
		if (expireTime_.HasValue)
		{
			num += _single_expireTime_codec.CalculateSizeWithTag(ExpireTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceInvite other)
	{
		if (other != null)
		{
			if (other.expireTime_.HasValue && (!expireTime_.HasValue || other.ExpireTime != 0))
			{
				ExpireTime = other.ExpireTime;
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
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
				continue;
			}
			long? num2 = _single_expireTime_codec.Read(input);
			if (!expireTime_.HasValue || num2 != 0)
			{
				ExpireTime = num2;
			}
		}
	}
}
