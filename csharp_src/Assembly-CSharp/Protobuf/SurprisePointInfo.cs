using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class SurprisePointInfo : IMessage<SurprisePointInfo>, IMessage, IEquatable<SurprisePointInfo>, IDeepCloneable<SurprisePointInfo>
{
	private static readonly MessageParser<SurprisePointInfo> _parser = new MessageParser<SurprisePointInfo>(() => new SurprisePointInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int ConfigIdFieldNumber = 2;

	private int configId_;

	public const int ShowTimeFieldNumber = 3;

	private long showTime_;

	public const int OpenTimeFieldNumber = 4;

	private long openTime_;

	[DebuggerNonUserCode]
	public static MessageParser<SurprisePointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[54];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long Uuid
	{
		get
		{
			return uuid_;
		}
		set
		{
			uuid_ = value;
		}
	}

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
	public long ShowTime
	{
		get
		{
			return showTime_;
		}
		set
		{
			showTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long OpenTime
	{
		get
		{
			return openTime_;
		}
		set
		{
			openTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public SurprisePointInfo()
	{
	}

	[DebuggerNonUserCode]
	public SurprisePointInfo(SurprisePointInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		configId_ = other.configId_;
		showTime_ = other.showTime_;
		openTime_ = other.openTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SurprisePointInfo Clone()
	{
		return new SurprisePointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SurprisePointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(SurprisePointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (ConfigId != other.ConfigId)
		{
			return false;
		}
		if (ShowTime != other.ShowTime)
		{
			return false;
		}
		if (OpenTime != other.OpenTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (ConfigId != 0)
		{
			num ^= ConfigId.GetHashCode();
		}
		if (ShowTime != 0L)
		{
			num ^= ShowTime.GetHashCode();
		}
		if (OpenTime != 0L)
		{
			num ^= OpenTime.GetHashCode();
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
		if (Uuid != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(Uuid);
		}
		if (ConfigId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(ConfigId);
		}
		if (ShowTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ShowTime);
		}
		if (OpenTime != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(OpenTime);
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
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (ConfigId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ConfigId);
		}
		if (ShowTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ShowTime);
		}
		if (OpenTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(OpenTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SurprisePointInfo other)
	{
		if (other != null)
		{
			if (other.Uuid != 0L)
			{
				Uuid = other.Uuid;
			}
			if (other.ConfigId != 0)
			{
				ConfigId = other.ConfigId;
			}
			if (other.ShowTime != 0L)
			{
				ShowTime = other.ShowTime;
			}
			if (other.OpenTime != 0L)
			{
				OpenTime = other.OpenTime;
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
				Uuid = input.ReadInt64();
				break;
			case 16u:
				ConfigId = input.ReadInt32();
				break;
			case 24u:
				ShowTime = input.ReadInt64();
				break;
			case 32u:
				OpenTime = input.ReadInt64();
				break;
			}
		}
	}
}
