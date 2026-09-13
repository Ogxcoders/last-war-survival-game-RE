using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class FloatInfo : IMessage<FloatInfo>, IMessage, IEquatable<FloatInfo>, IDeepCloneable<FloatInfo>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public enum State
		{
			[OriginalName("NORMAL")]
			Normal,
			[OriginalName("STUNNED")]
			Stunned
		}
	}

	private static readonly MessageParser<FloatInfo> _parser = new MessageParser<FloatInfo>(() => new FloatInfo());

	private UnknownFieldSet _unknownFields;

	public const int StateFieldNumber = 1;

	private int state_;

	public const int StateChangeTimeFieldNumber = 2;

	private long stateChangeTime_;

	public const int CurrentArmorFieldNumber = 3;

	private long currentArmor_;

	public const int MaxArmorFieldNumber = 4;

	private long maxArmor_;

	public const int ArmorChestDroppedFieldNumber = 5;

	private int armorChestDropped_;

	public const int BloodChestDroppedFieldNumber = 6;

	private int bloodChestDropped_;

	[DebuggerNonUserCode]
	public static MessageParser<FloatInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[38];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int State
	{
		get
		{
			return state_;
		}
		set
		{
			state_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StateChangeTime
	{
		get
		{
			return stateChangeTime_;
		}
		set
		{
			stateChangeTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurrentArmor
	{
		get
		{
			return currentArmor_;
		}
		set
		{
			currentArmor_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long MaxArmor
	{
		get
		{
			return maxArmor_;
		}
		set
		{
			maxArmor_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ArmorChestDropped
	{
		get
		{
			return armorChestDropped_;
		}
		set
		{
			armorChestDropped_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BloodChestDropped
	{
		get
		{
			return bloodChestDropped_;
		}
		set
		{
			bloodChestDropped_ = value;
		}
	}

	[DebuggerNonUserCode]
	public FloatInfo()
	{
	}

	[DebuggerNonUserCode]
	public FloatInfo(FloatInfo other)
		: this()
	{
		state_ = other.state_;
		stateChangeTime_ = other.stateChangeTime_;
		currentArmor_ = other.currentArmor_;
		maxArmor_ = other.maxArmor_;
		armorChestDropped_ = other.armorChestDropped_;
		bloodChestDropped_ = other.bloodChestDropped_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public FloatInfo Clone()
	{
		return new FloatInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as FloatInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(FloatInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (State != other.State)
		{
			return false;
		}
		if (StateChangeTime != other.StateChangeTime)
		{
			return false;
		}
		if (CurrentArmor != other.CurrentArmor)
		{
			return false;
		}
		if (MaxArmor != other.MaxArmor)
		{
			return false;
		}
		if (ArmorChestDropped != other.ArmorChestDropped)
		{
			return false;
		}
		if (BloodChestDropped != other.BloodChestDropped)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (StateChangeTime != 0L)
		{
			num ^= StateChangeTime.GetHashCode();
		}
		if (CurrentArmor != 0L)
		{
			num ^= CurrentArmor.GetHashCode();
		}
		if (MaxArmor != 0L)
		{
			num ^= MaxArmor.GetHashCode();
		}
		if (ArmorChestDropped != 0)
		{
			num ^= ArmorChestDropped.GetHashCode();
		}
		if (BloodChestDropped != 0)
		{
			num ^= BloodChestDropped.GetHashCode();
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
		if (State != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(State);
		}
		if (StateChangeTime != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(StateChangeTime);
		}
		if (CurrentArmor != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(CurrentArmor);
		}
		if (MaxArmor != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(MaxArmor);
		}
		if (ArmorChestDropped != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(ArmorChestDropped);
		}
		if (BloodChestDropped != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(BloodChestDropped);
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
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (StateChangeTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StateChangeTime);
		}
		if (CurrentArmor != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrentArmor);
		}
		if (MaxArmor != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxArmor);
		}
		if (ArmorChestDropped != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ArmorChestDropped);
		}
		if (BloodChestDropped != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BloodChestDropped);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(FloatInfo other)
	{
		if (other != null)
		{
			if (other.State != 0)
			{
				State = other.State;
			}
			if (other.StateChangeTime != 0L)
			{
				StateChangeTime = other.StateChangeTime;
			}
			if (other.CurrentArmor != 0L)
			{
				CurrentArmor = other.CurrentArmor;
			}
			if (other.MaxArmor != 0L)
			{
				MaxArmor = other.MaxArmor;
			}
			if (other.ArmorChestDropped != 0)
			{
				ArmorChestDropped = other.ArmorChestDropped;
			}
			if (other.BloodChestDropped != 0)
			{
				BloodChestDropped = other.BloodChestDropped;
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
				State = input.ReadInt32();
				break;
			case 16u:
				StateChangeTime = input.ReadInt64();
				break;
			case 24u:
				CurrentArmor = input.ReadInt64();
				break;
			case 32u:
				MaxArmor = input.ReadInt64();
				break;
			case 40u:
				ArmorChestDropped = input.ReadInt32();
				break;
			case 48u:
				BloodChestDropped = input.ReadInt32();
				break;
			}
		}
	}
}
