using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class EffectInfo : IMessage<EffectInfo>, IMessage, IEquatable<EffectInfo>, IDeepCloneable<EffectInfo>
{
	private static readonly MessageParser<EffectInfo> _parser = new MessageParser<EffectInfo>(() => new EffectInfo());

	private UnknownFieldSet _unknownFields;

	public const int EffectStrFieldNumber = 1;

	private string effectStr_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<EffectInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[14];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string EffectStr
	{
		get
		{
			return effectStr_;
		}
		set
		{
			effectStr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public EffectInfo()
	{
	}

	[DebuggerNonUserCode]
	public EffectInfo(EffectInfo other)
		: this()
	{
		effectStr_ = other.effectStr_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EffectInfo Clone()
	{
		return new EffectInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EffectInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(EffectInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (EffectStr != other.EffectStr)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (EffectStr.Length != 0)
		{
			num ^= EffectStr.GetHashCode();
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
		if (EffectStr.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(EffectStr);
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
		if (EffectStr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(EffectStr);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EffectInfo other)
	{
		if (other != null)
		{
			if (other.EffectStr.Length != 0)
			{
				EffectStr = other.EffectStr;
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
				EffectStr = input.ReadString();
			}
		}
	}
}
