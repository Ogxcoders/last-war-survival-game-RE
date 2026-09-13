using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class StrategicAreaServer : IMessage<StrategicAreaServer>, IMessage, IEquatable<StrategicAreaServer>, IDeepCloneable<StrategicAreaServer>
{
	private static readonly MessageParser<StrategicAreaServer> _parser = new MessageParser<StrategicAreaServer>(() => new StrategicAreaServer());

	private UnknownFieldSet _unknownFields;

	public const int ServerIdFieldNumber = 1;

	private int serverId_;

	public const int CampIdFieldNumber = 2;

	private int campId_;

	[DebuggerNonUserCode]
	public static MessageParser<StrategicAreaServer> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[24];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int ServerId
	{
		get
		{
			return serverId_;
		}
		set
		{
			serverId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CampId
	{
		get
		{
			return campId_;
		}
		set
		{
			campId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public StrategicAreaServer()
	{
	}

	[DebuggerNonUserCode]
	public StrategicAreaServer(StrategicAreaServer other)
		: this()
	{
		serverId_ = other.serverId_;
		campId_ = other.campId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public StrategicAreaServer Clone()
	{
		return new StrategicAreaServer(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as StrategicAreaServer);
	}

	[DebuggerNonUserCode]
	public bool Equals(StrategicAreaServer other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ServerId != other.ServerId)
		{
			return false;
		}
		if (CampId != other.CampId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ServerId != 0)
		{
			num ^= ServerId.GetHashCode();
		}
		if (CampId != 0)
		{
			num ^= CampId.GetHashCode();
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
		if (ServerId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ServerId);
		}
		if (CampId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(CampId);
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
		if (ServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ServerId);
		}
		if (CampId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CampId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(StrategicAreaServer other)
	{
		if (other != null)
		{
			if (other.ServerId != 0)
			{
				ServerId = other.ServerId;
			}
			if (other.CampId != 0)
			{
				CampId = other.CampId;
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
				ServerId = input.ReadInt32();
				break;
			case 16u:
				CampId = input.ReadInt32();
				break;
			}
		}
	}
}
