using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Activation : IMessage<Activation>, IMessage, IEquatable<Activation>, IDeepCloneable<Activation>
{
	private static readonly MessageParser<Activation> _parser = new MessageParser<Activation>(() => new Activation());

	private UnknownFieldSet _unknownFields;

	public const int CodeFieldNumber = 1;

	private string code_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<Activation> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[35];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Code
	{
		get
		{
			return code_;
		}
		set
		{
			code_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public Activation()
	{
	}

	[DebuggerNonUserCode]
	public Activation(Activation other)
		: this()
	{
		code_ = other.code_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Activation Clone()
	{
		return new Activation(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Activation);
	}

	[DebuggerNonUserCode]
	public bool Equals(Activation other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Code != other.Code)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Code.Length != 0)
		{
			num ^= Code.GetHashCode();
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
		if (Code.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Code);
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
		if (Code.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Code);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Activation other)
	{
		if (other != null)
		{
			if (other.Code.Length != 0)
			{
				Code = other.Code;
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
			}
			else
			{
				Code = input.ReadString();
			}
		}
	}
}
