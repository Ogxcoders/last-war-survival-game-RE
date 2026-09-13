using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection;

public sealed class EnumOptions : IExtendableMessage<EnumOptions>, IMessage<EnumOptions>, IMessage, IEquatable<EnumOptions>, IDeepCloneable<EnumOptions>
{
	private static readonly MessageParser<EnumOptions> _parser = new MessageParser<EnumOptions>(() => new EnumOptions());

	private UnknownFieldSet _unknownFields;

	internal ExtensionSet<EnumOptions> _extensions;

	private int _hasBits0;

	public const int AllowAliasFieldNumber = 2;

	private static readonly bool AllowAliasDefaultValue = false;

	private bool allowAlias_;

	public const int DeprecatedFieldNumber = 3;

	private static readonly bool DeprecatedDefaultValue = false;

	private bool deprecated_;

	public const int UninterpretedOptionFieldNumber = 999;

	private static readonly FieldCodec<UninterpretedOption> _repeated_uninterpretedOption_codec = FieldCodec.ForMessage(7994u, Google.Protobuf.Reflection.UninterpretedOption.Parser);

	private readonly RepeatedField<UninterpretedOption> uninterpretedOption_ = new RepeatedField<UninterpretedOption>();

	private ExtensionSet<EnumOptions> _Extensions => _extensions;

	[DebuggerNonUserCode]
	public static MessageParser<EnumOptions> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => DescriptorReflection.Descriptor.MessageTypes[14];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public bool AllowAlias
	{
		get
		{
			if ((_hasBits0 & 1) != 0)
			{
				return allowAlias_;
			}
			return AllowAliasDefaultValue;
		}
		set
		{
			_hasBits0 |= 1;
			allowAlias_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasAllowAlias => (_hasBits0 & 1) != 0;

	[DebuggerNonUserCode]
	public bool Deprecated
	{
		get
		{
			if ((_hasBits0 & 2) != 0)
			{
				return deprecated_;
			}
			return DeprecatedDefaultValue;
		}
		set
		{
			_hasBits0 |= 2;
			deprecated_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool HasDeprecated => (_hasBits0 & 2) != 0;

	[DebuggerNonUserCode]
	public RepeatedField<UninterpretedOption> UninterpretedOption => uninterpretedOption_;

	[DebuggerNonUserCode]
	public EnumOptions()
	{
	}

	[DebuggerNonUserCode]
	public EnumOptions(EnumOptions other)
		: this()
	{
		_hasBits0 = other._hasBits0;
		allowAlias_ = other.allowAlias_;
		deprecated_ = other.deprecated_;
		uninterpretedOption_ = other.uninterpretedOption_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		_extensions = ExtensionSet.Clone(other._extensions);
	}

	[DebuggerNonUserCode]
	public EnumOptions Clone()
	{
		return new EnumOptions(this);
	}

	[DebuggerNonUserCode]
	public void ClearAllowAlias()
	{
		_hasBits0 &= -2;
	}

	[DebuggerNonUserCode]
	public void ClearDeprecated()
	{
		_hasBits0 &= -3;
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EnumOptions);
	}

	[DebuggerNonUserCode]
	public bool Equals(EnumOptions other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (AllowAlias != other.AllowAlias)
		{
			return false;
		}
		if (Deprecated != other.Deprecated)
		{
			return false;
		}
		if (!uninterpretedOption_.Equals(other.uninterpretedOption_))
		{
			return false;
		}
		if (!object.Equals(_extensions, other._extensions))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (HasAllowAlias)
		{
			num ^= AllowAlias.GetHashCode();
		}
		if (HasDeprecated)
		{
			num ^= Deprecated.GetHashCode();
		}
		num ^= uninterpretedOption_.GetHashCode();
		if (_extensions != null)
		{
			num ^= _extensions.GetHashCode();
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
		if (HasAllowAlias)
		{
			output.WriteRawTag(16);
			output.WriteBool(AllowAlias);
		}
		if (HasDeprecated)
		{
			output.WriteRawTag(24);
			output.WriteBool(Deprecated);
		}
		uninterpretedOption_.WriteTo(output, _repeated_uninterpretedOption_codec);
		if (_extensions != null)
		{
			_extensions.WriteTo(output);
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
		if (HasAllowAlias)
		{
			num += 2;
		}
		if (HasDeprecated)
		{
			num += 2;
		}
		num += uninterpretedOption_.CalculateSize(_repeated_uninterpretedOption_codec);
		if (_extensions != null)
		{
			num += _extensions.CalculateSize();
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EnumOptions other)
	{
		if (other != null)
		{
			if (other.HasAllowAlias)
			{
				AllowAlias = other.AllowAlias;
			}
			if (other.HasDeprecated)
			{
				Deprecated = other.Deprecated;
			}
			uninterpretedOption_.Add(other.uninterpretedOption_);
			ExtensionSet.MergeFrom(ref _extensions, other._extensions);
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
			case 16u:
				AllowAlias = input.ReadBool();
				continue;
			case 24u:
				Deprecated = input.ReadBool();
				continue;
			case 7994u:
				uninterpretedOption_.AddEntriesFrom(input, _repeated_uninterpretedOption_codec);
				continue;
			}
			if (!ExtensionSet.TryMergeFieldFrom(ref _extensions, input))
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
		}
	}

	public TValue GetExtension<TValue>(Extension<EnumOptions, TValue> extension)
	{
		return ExtensionSet.Get(ref _extensions, extension);
	}

	public RepeatedField<TValue> GetExtension<TValue>(RepeatedExtension<EnumOptions, TValue> extension)
	{
		return ExtensionSet.Get(ref _extensions, extension);
	}

	public RepeatedField<TValue> GetOrInitializeExtension<TValue>(RepeatedExtension<EnumOptions, TValue> extension)
	{
		return ExtensionSet.GetOrInitialize(ref _extensions, extension);
	}

	public void SetExtension<TValue>(Extension<EnumOptions, TValue> extension, TValue value)
	{
		ExtensionSet.Set(ref _extensions, extension, value);
	}

	public bool HasExtension<TValue>(Extension<EnumOptions, TValue> extension)
	{
		return ExtensionSet.Has(ref _extensions, extension);
	}

	public void ClearExtension<TValue>(Extension<EnumOptions, TValue> extension)
	{
		ExtensionSet.Clear(ref _extensions, extension);
	}

	public void ClearExtension<TValue>(RepeatedExtension<EnumOptions, TValue> extension)
	{
		ExtensionSet.Clear(ref _extensions, extension);
	}
}
