using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LandPointInfo : IMessage<LandPointInfo>, IMessage, IEquatable<LandPointInfo>, IDeepCloneable<LandPointInfo>
{
	private static readonly MessageParser<LandPointInfo> _parser = new MessageParser<LandPointInfo>(() => new LandPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private int id_;

	public const int LandIdFieldNumber = 2;

	private int landId_;

	public const int ServerIdFieldNumber = 3;

	private int serverId_;

	[DebuggerNonUserCode]
	public static MessageParser<LandPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[40];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LandId
	{
		get
		{
			return landId_;
		}
		set
		{
			landId_ = value;
		}
	}

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
	public LandPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public LandPointInfo(LandPointInfo other)
		: this()
	{
		id_ = other.id_;
		landId_ = other.landId_;
		serverId_ = other.serverId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LandPointInfo Clone()
	{
		return new LandPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LandPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(LandPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (LandId != other.LandId)
		{
			return false;
		}
		if (ServerId != other.ServerId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Id != 0)
		{
			num ^= Id.GetHashCode();
		}
		if (LandId != 0)
		{
			num ^= LandId.GetHashCode();
		}
		if (ServerId != 0)
		{
			num ^= ServerId.GetHashCode();
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
		if (Id != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Id);
		}
		if (LandId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(LandId);
		}
		if (ServerId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(ServerId);
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
		if (Id != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Id);
		}
		if (LandId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LandId);
		}
		if (ServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ServerId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LandPointInfo other)
	{
		if (other != null)
		{
			if (other.Id != 0)
			{
				Id = other.Id;
			}
			if (other.LandId != 0)
			{
				LandId = other.LandId;
			}
			if (other.ServerId != 0)
			{
				ServerId = other.ServerId;
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
				Id = input.ReadInt32();
				break;
			case 16u:
				LandId = input.ReadInt32();
				break;
			case 24u:
				ServerId = input.ReadInt32();
				break;
			}
		}
	}
}
