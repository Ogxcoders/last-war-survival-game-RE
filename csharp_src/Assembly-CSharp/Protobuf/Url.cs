using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Url : IMessage<Url>, IMessage, IEquatable<Url>, IDeepCloneable<Url>
{
	private static readonly MessageParser<Url> _parser = new MessageParser<Url>(() => new Url());

	private UnknownFieldSet _unknownFields;

	public const int HrefFieldNumber = 1;

	private string href_ = "";

	public const int TextFieldNumber = 2;

	private string text_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<Url> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[28];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Href
	{
		get
		{
			return href_;
		}
		set
		{
			href_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Text
	{
		get
		{
			return text_;
		}
		set
		{
			text_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public Url()
	{
	}

	[DebuggerNonUserCode]
	public Url(Url other)
		: this()
	{
		href_ = other.href_;
		text_ = other.text_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Url Clone()
	{
		return new Url(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Url);
	}

	[DebuggerNonUserCode]
	public bool Equals(Url other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Href != other.Href)
		{
			return false;
		}
		if (Text != other.Text)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Href.Length != 0)
		{
			num ^= Href.GetHashCode();
		}
		if (Text.Length != 0)
		{
			num ^= Text.GetHashCode();
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
		if (Href.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Href);
		}
		if (Text.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Text);
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
		if (Href.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Href);
		}
		if (Text.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Text);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Url other)
	{
		if (other != null)
		{
			if (other.Href.Length != 0)
			{
				Href = other.Href;
			}
			if (other.Text.Length != 0)
			{
				Text = other.Text;
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
				Href = input.ReadString();
				break;
			case 18u:
				Text = input.ReadString();
				break;
			}
		}
	}
}
