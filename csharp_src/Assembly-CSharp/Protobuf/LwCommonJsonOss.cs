using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwCommonJsonOss : IMessage<LwCommonJsonOss>, IMessage, IEquatable<LwCommonJsonOss>, IDeepCloneable<LwCommonJsonOss>
{
	private static readonly MessageParser<LwCommonJsonOss> _parser = new MessageParser<LwCommonJsonOss>(() => new LwCommonJsonOss());

	private UnknownFieldSet _unknownFields;

	public const int TypeFieldNumber = 1;

	private int type_;

	public const int JsonDataFieldNumber = 2;

	private string jsonData_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<LwCommonJsonOss> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[18];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string JsonData
	{
		get
		{
			return jsonData_;
		}
		set
		{
			jsonData_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public LwCommonJsonOss()
	{
	}

	[DebuggerNonUserCode]
	public LwCommonJsonOss(LwCommonJsonOss other)
		: this()
	{
		type_ = other.type_;
		jsonData_ = other.jsonData_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwCommonJsonOss Clone()
	{
		return new LwCommonJsonOss(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwCommonJsonOss);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwCommonJsonOss other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (JsonData != other.JsonData)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (JsonData.Length != 0)
		{
			num ^= JsonData.GetHashCode();
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
		if (Type != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Type);
		}
		if (JsonData.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(JsonData);
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
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (JsonData.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(JsonData);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwCommonJsonOss other)
	{
		if (other != null)
		{
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.JsonData.Length != 0)
			{
				JsonData = other.JsonData;
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
				Type = input.ReadInt32();
				break;
			case 18u:
				JsonData = input.ReadString();
				break;
			}
		}
	}
}
