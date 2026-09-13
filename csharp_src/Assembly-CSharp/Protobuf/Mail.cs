using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Mail : IMessage<Mail>, IMessage, IEquatable<Mail>, IDeepCloneable<Mail>
{
	private static readonly MessageParser<Mail> _parser = new MessageParser<Mail>(() => new Mail());

	private UnknownFieldSet _unknownFields;

	public const int HeaderFieldNumber = 1;

	private MailHeader header_;

	public const int BodyFieldNumber = 2;

	private MailBody body_;

	public const int CustomFieldNumber = 3;

	private MailCustom custom_;

	[DebuggerNonUserCode]
	public static MessageParser<Mail> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public MailHeader Header
	{
		get
		{
			return header_;
		}
		set
		{
			header_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MailBody Body
	{
		get
		{
			return body_;
		}
		set
		{
			body_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MailCustom Custom
	{
		get
		{
			return custom_;
		}
		set
		{
			custom_ = value;
		}
	}

	[DebuggerNonUserCode]
	public Mail()
	{
	}

	[DebuggerNonUserCode]
	public Mail(Mail other)
		: this()
	{
		header_ = ((other.header_ != null) ? other.header_.Clone() : null);
		body_ = ((other.body_ != null) ? other.body_.Clone() : null);
		custom_ = ((other.custom_ != null) ? other.custom_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Mail Clone()
	{
		return new Mail(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Mail);
	}

	[DebuggerNonUserCode]
	public bool Equals(Mail other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(Header, other.Header))
		{
			return false;
		}
		if (!object.Equals(Body, other.Body))
		{
			return false;
		}
		if (!object.Equals(Custom, other.Custom))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (header_ != null)
		{
			num ^= Header.GetHashCode();
		}
		if (body_ != null)
		{
			num ^= Body.GetHashCode();
		}
		if (custom_ != null)
		{
			num ^= Custom.GetHashCode();
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
		if (header_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(Header);
		}
		if (body_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(Body);
		}
		if (custom_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(Custom);
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
		if (header_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Header);
		}
		if (body_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Body);
		}
		if (custom_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Custom);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Mail other)
	{
		if (other == null)
		{
			return;
		}
		if (other.header_ != null)
		{
			if (header_ == null)
			{
				Header = new MailHeader();
			}
			Header.MergeFrom(other.Header);
		}
		if (other.body_ != null)
		{
			if (body_ == null)
			{
				Body = new MailBody();
			}
			Body.MergeFrom(other.Body);
		}
		if (other.custom_ != null)
		{
			if (custom_ == null)
			{
				Custom = new MailCustom();
			}
			Custom.MergeFrom(other.Custom);
		}
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
			case 10u:
				if (header_ == null)
				{
					Header = new MailHeader();
				}
				input.ReadMessage(Header);
				break;
			case 18u:
				if (body_ == null)
				{
					Body = new MailBody();
				}
				input.ReadMessage(Body);
				break;
			case 26u:
				if (custom_ == null)
				{
					Custom = new MailCustom();
				}
				input.ReadMessage(Custom);
				break;
			}
		}
	}
}
