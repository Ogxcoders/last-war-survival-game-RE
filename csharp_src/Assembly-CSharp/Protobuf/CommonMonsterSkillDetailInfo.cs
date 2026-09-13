using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CommonMonsterSkillDetailInfo : IMessage<CommonMonsterSkillDetailInfo>, IMessage, IEquatable<CommonMonsterSkillDetailInfo>, IDeepCloneable<CommonMonsterSkillDetailInfo>
{
	private static readonly MessageParser<CommonMonsterSkillDetailInfo> _parser = new MessageParser<CommonMonsterSkillDetailInfo>(() => new CommonMonsterSkillDetailInfo());

	private UnknownFieldSet _unknownFields;

	public const int AimByPointFieldNumber = 1;

	private int aimByPoint_;

	public const int AimEndTimeFieldNumber = 2;

	private long aimEndTime_;

	public const int AimBossUuidFieldNumber = 3;

	private long aimBossUuid_;

	public const int SkillIdFieldNumber = 4;

	private int skillId_;

	[DebuggerNonUserCode]
	public static MessageParser<CommonMonsterSkillDetailInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[3];

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
	public int SkillId
	{
		get
		{
			return skillId_;
		}
		set
		{
			skillId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CommonMonsterSkillDetailInfo()
	{
	}

	[DebuggerNonUserCode]
	public CommonMonsterSkillDetailInfo(CommonMonsterSkillDetailInfo other)
		: this()
	{
		aimByPoint_ = other.aimByPoint_;
		aimEndTime_ = other.aimEndTime_;
		aimBossUuid_ = other.aimBossUuid_;
		skillId_ = other.skillId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CommonMonsterSkillDetailInfo Clone()
	{
		return new CommonMonsterSkillDetailInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CommonMonsterSkillDetailInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CommonMonsterSkillDetailInfo other)
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
		if (SkillId != other.SkillId)
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
		if (SkillId != 0)
		{
			num ^= SkillId.GetHashCode();
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
		if (SkillId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(SkillId);
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
		if (SkillId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CommonMonsterSkillDetailInfo other)
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
			if (other.SkillId != 0)
			{
				SkillId = other.SkillId;
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
			case 32u:
				SkillId = input.ReadInt32();
				break;
			}
		}
	}
}
