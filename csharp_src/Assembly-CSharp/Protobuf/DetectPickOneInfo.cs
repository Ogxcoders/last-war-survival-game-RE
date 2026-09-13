using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DetectPickOneInfo : IMessage<DetectPickOneInfo>, IMessage, IEquatable<DetectPickOneInfo>, IDeepCloneable<DetectPickOneInfo>
{
	private static readonly MessageParser<DetectPickOneInfo> _parser = new MessageParser<DetectPickOneInfo>(() => new DetectPickOneInfo());

	private UnknownFieldSet _unknownFields;

	public const int ConfigIdFieldNumber = 1;

	private int configId_;

	public const int ExtendStringFieldNumber = 2;

	private string extendString_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<DetectPickOneInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[19];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int ConfigId
	{
		get
		{
			return configId_;
		}
		set
		{
			configId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string ExtendString
	{
		get
		{
			return extendString_;
		}
		set
		{
			extendString_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public DetectPickOneInfo()
	{
	}

	[DebuggerNonUserCode]
	public DetectPickOneInfo(DetectPickOneInfo other)
		: this()
	{
		configId_ = other.configId_;
		extendString_ = other.extendString_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DetectPickOneInfo Clone()
	{
		return new DetectPickOneInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DetectPickOneInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(DetectPickOneInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ConfigId != other.ConfigId)
		{
			return false;
		}
		if (ExtendString != other.ExtendString)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ConfigId != 0)
		{
			num ^= ConfigId.GetHashCode();
		}
		if (ExtendString.Length != 0)
		{
			num ^= ExtendString.GetHashCode();
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
		if (ConfigId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ConfigId);
		}
		if (ExtendString.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(ExtendString);
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
		if (ConfigId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ConfigId);
		}
		if (ExtendString.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ExtendString);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DetectPickOneInfo other)
	{
		if (other != null)
		{
			if (other.ConfigId != 0)
			{
				ConfigId = other.ConfigId;
			}
			if (other.ExtendString.Length != 0)
			{
				ExtendString = other.ExtendString;
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
			case 8u:
				ConfigId = input.ReadInt32();
				break;
			case 18u:
				ExtendString = input.ReadString();
				break;
			}
		}
	}
}
