using System;
using System.Diagnostics;

namespace Google.Protobuf.Reflection;

public sealed class EnumValueDescriptorProto : IMessage<EnumValueDescriptorProto>, IMessage, IEquatable<EnumValueDescriptorProto>, IDeepCloneable<EnumValueDescriptorProto>
{
	private static readonly MessageParser<EnumValueDescriptorProto> _parser = new MessageParser<EnumValueDescriptorProto>(() => new EnumValueDescriptorProto());

	private UnknownFieldSet _unknownFields;

	private int _hasBits0;

	public const int NameFieldNumber = 1;

	private static readonly string NameDefaultValue = "";

	private string name_;

	public const int NumberFieldNumber = 2;

	private static readonly int NumberDefaultValue = 0;

	private int number_;

	public const int OptionsFieldNumber = 3;

	private EnumValueOptions options_;

	[DebuggerNonUserCode]
	public static MessageParser<EnumValueDescriptorProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => DescriptorReflection.Descriptor.MessageTypes[7];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_ ?? NameDefaultValue;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public bool HasName => name_ != null;

	[DebuggerNonUserCode]
	public int Number
	{
		get
		{
			if ((_hasBits0 & 1) != 0)
			{
				return number_;
			}
			return NumberDefaultValue;
		}
		set
		{
			_hasBits0 |= 1;
			number_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasNumber => (_hasBits0 & 1) != 0;

	[DebuggerNonUserCode]
	public EnumValueOptions Options
	{
		get
		{
			return options_;
		}
		set
		{
			options_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasOptions => options_ != null;

	[DebuggerNonUserCode]
	public EnumValueDescriptorProto()
	{
	}

	[DebuggerNonUserCode]
	public EnumValueDescriptorProto(EnumValueDescriptorProto other)
		: this()
	{
		_hasBits0 = other._hasBits0;
		name_ = other.name_;
		number_ = other.number_;
		options_ = (other.HasOptions ? other.options_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EnumValueDescriptorProto Clone()
	{
		return new EnumValueDescriptorProto(this);
	}

	[DebuggerNonUserCode]
	public void ClearName()
	{
		name_ = null;
	}

	[DebuggerNonUserCode]
	public void ClearNumber()
	{
		_hasBits0 &= -2;
	}

	[DebuggerNonUserCode]
	public void ClearOptions()
	{
		options_ = null;
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnumValueDescriptorProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnumValueDescriptorProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (Number != other.Number)
		{
			return false;
		}
		if (!object.Equals(Options, other.Options))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (HasName)
		{
			num ^= Name.GetHashCode();
		}
		if (HasNumber)
		{
			num ^= Number.GetHashCode();
		}
		if (HasOptions)
		{
			num ^= Options.GetHashCode();
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
		if (HasName)
		{
			output.WriteRawTag(10);
			output.WriteString(Name);
		}
		if (HasNumber)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Number);
		}
		if (HasOptions)
		{
			output.WriteRawTag(26);
			output.WriteMessage(Options);
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
		if (HasName)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (HasNumber)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Number);
		}
		if (HasOptions)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Options);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnumValueDescriptorProto other)
	{
		if (other == null)
		{
			return;
		}
		if (other.HasName)
		{
			Name = other.Name;
		}
		if (other.HasNumber)
		{
			Number = other.Number;
		}
		if (other.HasOptions)
		{
			if (!HasOptions)
			{
				Options = new EnumValueOptions();
			}
			Options.MergeFrom(other.Options);
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
				Name = input.ReadString();
				break;
			case 16u:
				Number = input.ReadInt32();
				break;
			case 26u:
				if (!HasOptions)
				{
					Options = new EnumValueOptions();
				}
				input.ReadMessage(Options);
				break;
			}
		}
	}
}
