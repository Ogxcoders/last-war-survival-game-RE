using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DamagePercentInfo : IMessage<DamagePercentInfo>, IMessage, IEquatable<DamagePercentInfo>, IDeepCloneable<DamagePercentInfo>
{
	private static readonly MessageParser<DamagePercentInfo> _parser = new MessageParser<DamagePercentInfo>(() => new DamagePercentInfo());

	private UnknownFieldSet _unknownFields;

	public const int TargetUuidFieldNumber = 1;

	private long targetUuid_;

	public const int DamagePercentFieldNumber = 2;

	private float damagePercent_;

	public const int WoundedPercentFieldNumber = 3;

	private float woundedPercent_;

	public const int InjuredPercentFieldNumber = 4;

	private float injuredPercent_;

	public const int DeadPercentFieldNumber = 5;

	private float deadPercent_;

	public const int SelfUuidFieldNumber = 6;

	private long selfUuid_;

	[DebuggerNonUserCode]
	public static MessageParser<DamagePercentInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[14];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long TargetUuid
	{
		get
		{
			return targetUuid_;
		}
		set
		{
			targetUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float DamagePercent
	{
		get
		{
			return damagePercent_;
		}
		set
		{
			damagePercent_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float WoundedPercent
	{
		get
		{
			return woundedPercent_;
		}
		set
		{
			woundedPercent_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float InjuredPercent
	{
		get
		{
			return injuredPercent_;
		}
		set
		{
			injuredPercent_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float DeadPercent
	{
		get
		{
			return deadPercent_;
		}
		set
		{
			deadPercent_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long SelfUuid
	{
		get
		{
			return selfUuid_;
		}
		set
		{
			selfUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public DamagePercentInfo()
	{
	}

	[DebuggerNonUserCode]
	public DamagePercentInfo(DamagePercentInfo other)
		: this()
	{
		targetUuid_ = other.targetUuid_;
		damagePercent_ = other.damagePercent_;
		woundedPercent_ = other.woundedPercent_;
		injuredPercent_ = other.injuredPercent_;
		deadPercent_ = other.deadPercent_;
		selfUuid_ = other.selfUuid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DamagePercentInfo Clone()
	{
		return new DamagePercentInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DamagePercentInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(DamagePercentInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TargetUuid != other.TargetUuid)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(DamagePercent, other.DamagePercent))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(WoundedPercent, other.WoundedPercent))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(InjuredPercent, other.InjuredPercent))
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(DeadPercent, other.DeadPercent))
		{
			return false;
		}
		if (SelfUuid != other.SelfUuid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TargetUuid != 0L)
		{
			num ^= TargetUuid.GetHashCode();
		}
		if (DamagePercent != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(DamagePercent);
		}
		if (WoundedPercent != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(WoundedPercent);
		}
		if (InjuredPercent != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(InjuredPercent);
		}
		if (DeadPercent != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(DeadPercent);
		}
		if (SelfUuid != 0L)
		{
			num ^= SelfUuid.GetHashCode();
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
		if (TargetUuid != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(TargetUuid);
		}
		if (DamagePercent != 0f)
		{
			output.WriteRawTag(21);
			output.WriteFloat(DamagePercent);
		}
		if (WoundedPercent != 0f)
		{
			output.WriteRawTag(29);
			output.WriteFloat(WoundedPercent);
		}
		if (InjuredPercent != 0f)
		{
			output.WriteRawTag(37);
			output.WriteFloat(InjuredPercent);
		}
		if (DeadPercent != 0f)
		{
			output.WriteRawTag(45);
			output.WriteFloat(DeadPercent);
		}
		if (SelfUuid != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(SelfUuid);
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
		if (TargetUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TargetUuid);
		}
		if (DamagePercent != 0f)
		{
			num += 5;
		}
		if (WoundedPercent != 0f)
		{
			num += 5;
		}
		if (InjuredPercent != 0f)
		{
			num += 5;
		}
		if (DeadPercent != 0f)
		{
			num += 5;
		}
		if (SelfUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(SelfUuid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DamagePercentInfo other)
	{
		if (other != null)
		{
			if (other.TargetUuid != 0L)
			{
				TargetUuid = other.TargetUuid;
			}
			if (other.DamagePercent != 0f)
			{
				DamagePercent = other.DamagePercent;
			}
			if (other.WoundedPercent != 0f)
			{
				WoundedPercent = other.WoundedPercent;
			}
			if (other.InjuredPercent != 0f)
			{
				InjuredPercent = other.InjuredPercent;
			}
			if (other.DeadPercent != 0f)
			{
				DeadPercent = other.DeadPercent;
			}
			if (other.SelfUuid != 0L)
			{
				SelfUuid = other.SelfUuid;
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
				TargetUuid = input.ReadInt64();
				break;
			case 21u:
				DamagePercent = input.ReadFloat();
				break;
			case 29u:
				WoundedPercent = input.ReadFloat();
				break;
			case 37u:
				InjuredPercent = input.ReadFloat();
				break;
			case 45u:
				DeadPercent = input.ReadFloat();
				break;
			case 48u:
				SelfUuid = input.ReadInt64();
				break;
			}
		}
	}
}
