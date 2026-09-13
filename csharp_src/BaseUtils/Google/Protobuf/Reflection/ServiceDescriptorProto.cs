using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection;

public sealed class ServiceDescriptorProto : IMessage<ServiceDescriptorProto>, IMessage, IEquatable<ServiceDescriptorProto>, IDeepCloneable<ServiceDescriptorProto>
{
	private static readonly MessageParser<ServiceDescriptorProto> _parser = new MessageParser<ServiceDescriptorProto>(() => new ServiceDescriptorProto());

	private UnknownFieldSet _unknownFields;

	public const int NameFieldNumber = 1;

	private static readonly string NameDefaultValue = "";

	private string name_;

	public const int MethodFieldNumber = 2;

	private static readonly FieldCodec<MethodDescriptorProto> _repeated_method_codec = FieldCodec.ForMessage(18u, MethodDescriptorProto.Parser);

	private readonly RepeatedField<MethodDescriptorProto> method_ = new RepeatedField<MethodDescriptorProto>();

	public const int OptionsFieldNumber = 3;

	private ServiceOptions options_;

	[DebuggerNonUserCode]
	public static MessageParser<ServiceDescriptorProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => DescriptorReflection.Descriptor.MessageTypes[8];

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
	public RepeatedField<MethodDescriptorProto> Method => method_;

	[DebuggerNonUserCode]
	public ServiceOptions Options
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
	public ServiceDescriptorProto()
	{
	}

	[DebuggerNonUserCode]
	public ServiceDescriptorProto(ServiceDescriptorProto other)
		: this()
	{
		name_ = other.name_;
		method_ = other.method_.Clone();
		options_ = (other.HasOptions ? other.options_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ServiceDescriptorProto Clone()
	{
		return new ServiceDescriptorProto(this);
	}

	[DebuggerNonUserCode]
	public void ClearName()
	{
		name_ = null;
	}

	[DebuggerNonUserCode]
	public void ClearOptions()
	{
		options_ = null;
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ServiceDescriptorProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(ServiceDescriptorProto other)
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
		if (!method_.Equals(other.method_))
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
		num ^= method_.GetHashCode();
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
		method_.WriteTo(output, _repeated_method_codec);
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
		num += method_.CalculateSize(_repeated_method_codec);
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
	public void MergeFrom(ServiceDescriptorProto other)
	{
		if (other == null)
		{
			return;
		}
		if (other.HasName)
		{
			Name = other.Name;
		}
		method_.Add(other.method_);
		if (other.HasOptions)
		{
			if (!HasOptions)
			{
				Options = new ServiceOptions();
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
			case 18u:
				method_.AddEntriesFrom(input, _repeated_method_codec);
				break;
			case 26u:
				if (!HasOptions)
				{
					Options = new ServiceOptions();
				}
				input.ReadMessage(Options);
				break;
			}
		}
	}
}
