using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MailHeader : IMessage<MailHeader>, IMessage, IEquatable<MailHeader>, IDeepCloneable<MailHeader>
{
	private static readonly MessageParser<MailHeader> _parser = new MessageParser<MailHeader>(() => new MailHeader());

	private UnknownFieldSet _unknownFields;

	public const int TitleFieldNumber = 1;

	private Message title_;

	public const int SubTitleFieldNumber = 2;

	private Message subTitle_;

	[DebuggerNonUserCode]
	public static MessageParser<MailHeader> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public Message Title
	{
		get
		{
			return title_;
		}
		set
		{
			title_ = value;
		}
	}

	[DebuggerNonUserCode]
	public Message SubTitle
	{
		get
		{
			return subTitle_;
		}
		set
		{
			subTitle_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MailHeader()
	{
	}

	[DebuggerNonUserCode]
	public MailHeader(MailHeader other)
		: this()
	{
		title_ = ((other.title_ != null) ? other.title_.Clone() : null);
		subTitle_ = ((other.subTitle_ != null) ? other.subTitle_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MailHeader Clone()
	{
		return new MailHeader(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MailHeader);
	}

	[DebuggerNonUserCode]
	public bool Equals(MailHeader other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(Title, other.Title))
		{
			return false;
		}
		if (!object.Equals(SubTitle, other.SubTitle))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (title_ != null)
		{
			num ^= Title.GetHashCode();
		}
		if (subTitle_ != null)
		{
			num ^= SubTitle.GetHashCode();
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
		if (title_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(Title);
		}
		if (subTitle_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(SubTitle);
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
		if (title_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Title);
		}
		if (subTitle_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SubTitle);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MailHeader other)
	{
		if (other == null)
		{
			return;
		}
		if (other.title_ != null)
		{
			if (title_ == null)
			{
				Title = new Message();
			}
			Title.MergeFrom(other.Title);
		}
		if (other.subTitle_ != null)
		{
			if (subTitle_ == null)
			{
				SubTitle = new Message();
			}
			SubTitle.MergeFrom(other.SubTitle);
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
				if (title_ == null)
				{
					Title = new Message();
				}
				input.ReadMessage(Title);
				break;
			case 18u:
				if (subTitle_ == null)
				{
					SubTitle = new Message();
				}
				input.ReadMessage(SubTitle);
				break;
			}
		}
	}
}
