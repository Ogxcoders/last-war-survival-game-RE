using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WeaponStrengthen : IMessage<WeaponStrengthen>, IMessage, IEquatable<WeaponStrengthen>, IDeepCloneable<WeaponStrengthen>
{
	private static readonly MessageParser<WeaponStrengthen> _parser = new MessageParser<WeaponStrengthen>(() => new WeaponStrengthen());

	private UnknownFieldSet _unknownFields;

	public const int SlotFieldNumber = 1;

	private int slot_;

	public const int LvFieldNumber = 2;

	private int lv_;

	[DebuggerNonUserCode]
	public static MessageParser<WeaponStrengthen> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Slot
	{
		get
		{
			return slot_;
		}
		set
		{
			slot_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Lv
	{
		get
		{
			return lv_;
		}
		set
		{
			lv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WeaponStrengthen()
	{
	}

	[DebuggerNonUserCode]
	public WeaponStrengthen(WeaponStrengthen other)
		: this()
	{
		slot_ = other.slot_;
		lv_ = other.lv_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WeaponStrengthen Clone()
	{
		return new WeaponStrengthen(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WeaponStrengthen);
	}

	[DebuggerNonUserCode]
	public bool Equals(WeaponStrengthen other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Slot != other.Slot)
		{
			return false;
		}
		if (Lv != other.Lv)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Slot != 0)
		{
			num ^= Slot.GetHashCode();
		}
		if (Lv != 0)
		{
			num ^= Lv.GetHashCode();
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
		if (Slot != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Slot);
		}
		if (Lv != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Lv);
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
		if (Slot != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Slot);
		}
		if (Lv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Lv);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WeaponStrengthen other)
	{
		if (other != null)
		{
			if (other.Slot != 0)
			{
				Slot = other.Slot;
			}
			if (other.Lv != 0)
			{
				Lv = other.Lv;
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
				Slot = input.ReadInt32();
				break;
			case 16u:
				Lv = input.ReadInt32();
				break;
			}
		}
	}
}
