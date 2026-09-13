using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class UnitBuffChange : IMessage<UnitBuffChange>, IMessage, IEquatable<UnitBuffChange>, IDeepCloneable<UnitBuffChange>
{
	private static readonly MessageParser<UnitBuffChange> _parser = new MessageParser<UnitBuffChange>(() => new UnitBuffChange());

	private UnknownFieldSet _unknownFields;

	public const int BuffIdFieldNumber = 1;

	private int buffId_;

	public const int EffectChangesFieldNumber = 2;

	private static readonly FieldCodec<BattleEffectInfo> _repeated_effectChanges_codec = FieldCodec.ForMessage(18u, BattleEffectInfo.Parser);

	private readonly RepeatedField<BattleEffectInfo> effectChanges_ = new RepeatedField<BattleEffectInfo>();

	public const int SkillLevelFieldNumber = 3;

	private int skillLevel_;

	public const int RemoveFieldNumber = 4;

	private bool remove_;

	public const int BuffDurationFieldNumber = 5;

	private long buffDuration_;

	[DebuggerNonUserCode]
	public static MessageParser<UnitBuffChange> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int BuffId
	{
		get
		{
			return buffId_;
		}
		set
		{
			buffId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<BattleEffectInfo> EffectChanges => effectChanges_;

	[DebuggerNonUserCode]
	public int SkillLevel
	{
		get
		{
			return skillLevel_;
		}
		set
		{
			skillLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool Remove
	{
		get
		{
			return remove_;
		}
		set
		{
			remove_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long BuffDuration
	{
		get
		{
			return buffDuration_;
		}
		set
		{
			buffDuration_ = value;
		}
	}

	[DebuggerNonUserCode]
	public UnitBuffChange()
	{
	}

	[DebuggerNonUserCode]
	public UnitBuffChange(UnitBuffChange other)
		: this()
	{
		buffId_ = other.buffId_;
		effectChanges_ = other.effectChanges_.Clone();
		skillLevel_ = other.skillLevel_;
		remove_ = other.remove_;
		buffDuration_ = other.buffDuration_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UnitBuffChange Clone()
	{
		return new UnitBuffChange(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UnitBuffChange);
	}

	[DebuggerNonUserCode]
	public bool Equals(UnitBuffChange other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BuffId != other.BuffId)
		{
			return false;
		}
		if (!effectChanges_.Equals(other.effectChanges_))
		{
			return false;
		}
		if (SkillLevel != other.SkillLevel)
		{
			return false;
		}
		if (Remove != other.Remove)
		{
			return false;
		}
		if (BuffDuration != other.BuffDuration)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (BuffId != 0)
		{
			num ^= BuffId.GetHashCode();
		}
		num ^= effectChanges_.GetHashCode();
		if (SkillLevel != 0)
		{
			num ^= SkillLevel.GetHashCode();
		}
		if (Remove)
		{
			num ^= Remove.GetHashCode();
		}
		if (BuffDuration != 0L)
		{
			num ^= BuffDuration.GetHashCode();
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
		if (BuffId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(BuffId);
		}
		effectChanges_.WriteTo(output, _repeated_effectChanges_codec);
		if (SkillLevel != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(SkillLevel);
		}
		if (Remove)
		{
			output.WriteRawTag(32);
			output.WriteBool(Remove);
		}
		if (BuffDuration != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(BuffDuration);
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
		if (BuffId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuffId);
		}
		num += effectChanges_.CalculateSize(_repeated_effectChanges_codec);
		if (SkillLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillLevel);
		}
		if (Remove)
		{
			num += 2;
		}
		if (BuffDuration != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(BuffDuration);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UnitBuffChange other)
	{
		if (other != null)
		{
			if (other.BuffId != 0)
			{
				BuffId = other.BuffId;
			}
			effectChanges_.Add(other.effectChanges_);
			if (other.SkillLevel != 0)
			{
				SkillLevel = other.SkillLevel;
			}
			if (other.Remove)
			{
				Remove = other.Remove;
			}
			if (other.BuffDuration != 0L)
			{
				BuffDuration = other.BuffDuration;
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
				BuffId = input.ReadInt32();
				break;
			case 18u:
				effectChanges_.AddEntriesFrom(input, _repeated_effectChanges_codec);
				break;
			case 24u:
				SkillLevel = input.ReadInt32();
				break;
			case 32u:
				Remove = input.ReadBool();
				break;
			case 40u:
				BuffDuration = input.ReadInt64();
				break;
			}
		}
	}
}
