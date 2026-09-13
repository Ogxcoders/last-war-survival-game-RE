using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DetectRetryTaskInfo : IMessage<DetectRetryTaskInfo>, IMessage, IEquatable<DetectRetryTaskInfo>, IDeepCloneable<DetectRetryTaskInfo>
{
	private static readonly MessageParser<DetectRetryTaskInfo> _parser = new MessageParser<DetectRetryTaskInfo>(() => new DetectRetryTaskInfo());

	private UnknownFieldSet _unknownFields;

	public const int FeatureConfigIdFieldNumber = 1;

	private int featureConfigId_;

	public const int ExtendStringFieldNumber = 2;

	private string extendString_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<DetectRetryTaskInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[18];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int FeatureConfigId
	{
		get
		{
			return featureConfigId_;
		}
		set
		{
			featureConfigId_ = value;
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
	public DetectRetryTaskInfo()
	{
	}

	[DebuggerNonUserCode]
	public DetectRetryTaskInfo(DetectRetryTaskInfo other)
		: this()
	{
		featureConfigId_ = other.featureConfigId_;
		extendString_ = other.extendString_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DetectRetryTaskInfo Clone()
	{
		return new DetectRetryTaskInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DetectRetryTaskInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(DetectRetryTaskInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (FeatureConfigId != other.FeatureConfigId)
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
		if (FeatureConfigId != 0)
		{
			num ^= FeatureConfigId.GetHashCode();
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
		if (FeatureConfigId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(FeatureConfigId);
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
		if (FeatureConfigId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FeatureConfigId);
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
	public void MergeFrom(DetectRetryTaskInfo other)
	{
		if (other != null)
		{
			if (other.FeatureConfigId != 0)
			{
				FeatureConfigId = other.FeatureConfigId;
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
				FeatureConfigId = input.ReadInt32();
				break;
			case 18u:
				ExtendString = input.ReadString();
				break;
			}
		}
	}
}
