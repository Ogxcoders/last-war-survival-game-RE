using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class TextWithParams : IMessage<TextWithParams>, IMessage, IEquatable<TextWithParams>, IDeepCloneable<TextWithParams>
{
	private static readonly MessageParser<TextWithParams> _parser = new MessageParser<TextWithParams>(() => new TextWithParams());

	private UnknownFieldSet _unknownFields;

	public const int TextFieldNumber = 1;

	private string text_ = "";

	public const int ParamsFieldNumber = 2;

	private static readonly FieldCodec<Message> _repeated_params_codec = FieldCodec.ForMessage(18u, Message.Parser);

	private readonly RepeatedField<Message> params_ = new RepeatedField<Message>();

	[DebuggerNonUserCode]
	public static MessageParser<TextWithParams> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[29];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public RepeatedField<Message> Params => params_;

	[DebuggerNonUserCode]
	public TextWithParams()
	{
	}

	[DebuggerNonUserCode]
	public TextWithParams(TextWithParams other)
		: this()
	{
		text_ = other.text_;
		params_ = other.params_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public TextWithParams Clone()
	{
		return new TextWithParams(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as TextWithParams);
	}

	[DebuggerNonUserCode]
	public bool Equals(TextWithParams other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Text != other.Text)
		{
			return false;
		}
		if (!params_.Equals(other.params_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Text.Length != 0)
		{
			num ^= Text.GetHashCode();
		}
		num ^= params_.GetHashCode();
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
		if (Text.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Text);
		}
		params_.WriteTo(output, _repeated_params_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (Text.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Text);
		}
		num += params_.CalculateSize(_repeated_params_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(TextWithParams other)
	{
		if (other != null)
		{
			if (other.Text.Length != 0)
			{
				Text = other.Text;
			}
			params_.Add(other.params_);
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
				Text = input.ReadString();
				break;
			case 18u:
				params_.AddEntriesFrom(input, _repeated_params_codec);
				break;
			}
		}
	}
}
