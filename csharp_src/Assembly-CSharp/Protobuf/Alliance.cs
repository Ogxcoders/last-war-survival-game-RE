using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Alliance : IMessage<Alliance>, IMessage, IEquatable<Alliance>, IDeepCloneable<Alliance>
{
	private static readonly MessageParser<Alliance> _parser = new MessageParser<Alliance>(() => new Alliance());

	private UnknownFieldSet _unknownFields;

	public const int AbbrFieldNumber = 1;

	private string abbr_ = "";

	public const int NameFieldNumber = 2;

	private string name_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<Alliance> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[12];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Abbr
	{
		get
		{
			return abbr_;
		}
		set
		{
			abbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public Alliance()
	{
	}

	[DebuggerNonUserCode]
	public Alliance(Alliance other)
		: this()
	{
		abbr_ = other.abbr_;
		name_ = other.name_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Alliance Clone()
	{
		return new Alliance(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Alliance);
	}

	[DebuggerNonUserCode]
	public bool Equals(Alliance other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Abbr != other.Abbr)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Abbr.Length != 0)
		{
			num ^= Abbr.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
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
		if (Abbr.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Abbr);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Name);
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
		if (Abbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Abbr);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Alliance other)
	{
		if (other != null)
		{
			if (other.Abbr.Length != 0)
			{
				Abbr = other.Abbr;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
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
				Abbr = input.ReadString();
				break;
			case 18u:
				Name = input.ReadString();
				break;
			}
		}
	}
}
