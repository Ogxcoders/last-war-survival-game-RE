using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ZoneMobilization : IMessage<ZoneMobilization>, IMessage, IEquatable<ZoneMobilization>, IDeepCloneable<ZoneMobilization>
{
	private static readonly MessageParser<ZoneMobilization> _parser = new MessageParser<ZoneMobilization>(() => new ZoneMobilization());

	private UnknownFieldSet _unknownFields;

	public const int AimByPointFieldNumber = 1;

	private int aimByPoint_;

	public const int AimEndTimeFieldNumber = 2;

	private long aimEndTime_;

	public const int AimBossUuidFieldNumber = 3;

	private long aimBossUuid_;

	[DebuggerNonUserCode]
	public static MessageParser<ZoneMobilization> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[8];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int AimByPoint
	{
		get
		{
			return aimByPoint_;
		}
		set
		{
			aimByPoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long AimEndTime
	{
		get
		{
			return aimEndTime_;
		}
		set
		{
			aimEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long AimBossUuid
	{
		get
		{
			return aimBossUuid_;
		}
		set
		{
			aimBossUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ZoneMobilization()
	{
	}

	[DebuggerNonUserCode]
	public ZoneMobilization(ZoneMobilization other)
		: this()
	{
		aimByPoint_ = other.aimByPoint_;
		aimEndTime_ = other.aimEndTime_;
		aimBossUuid_ = other.aimBossUuid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ZoneMobilization Clone()
	{
		return new ZoneMobilization(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ZoneMobilization);
	}

	[DebuggerNonUserCode]
	public bool Equals(ZoneMobilization other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (AimByPoint != other.AimByPoint)
		{
			return false;
		}
		if (AimEndTime != other.AimEndTime)
		{
			return false;
		}
		if (AimBossUuid != other.AimBossUuid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (AimByPoint != 0)
		{
			num ^= AimByPoint.GetHashCode();
		}
		if (AimEndTime != 0L)
		{
			num ^= AimEndTime.GetHashCode();
		}
		if (AimBossUuid != 0L)
		{
			num ^= AimBossUuid.GetHashCode();
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
		if (AimByPoint != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(AimByPoint);
		}
		if (AimEndTime != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(AimEndTime);
		}
		if (AimBossUuid != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(AimBossUuid);
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
		if (AimByPoint != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(AimByPoint);
		}
		if (AimEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(AimEndTime);
		}
		if (AimBossUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(AimBossUuid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ZoneMobilization other)
	{
		if (other != null)
		{
			if (other.AimByPoint != 0)
			{
				AimByPoint = other.AimByPoint;
			}
			if (other.AimEndTime != 0L)
			{
				AimEndTime = other.AimEndTime;
			}
			if (other.AimBossUuid != 0L)
			{
				AimBossUuid = other.AimBossUuid;
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
				AimByPoint = input.ReadInt32();
				break;
			case 16u:
				AimEndTime = input.ReadInt64();
				break;
			case 24u:
				AimBossUuid = input.ReadInt64();
				break;
			}
		}
	}
}
