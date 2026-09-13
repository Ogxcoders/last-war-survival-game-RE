using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class HeroSkillInfoProto : IMessage<HeroSkillInfoProto>, IMessage, IEquatable<HeroSkillInfoProto>, IDeepCloneable<HeroSkillInfoProto>
{
	private static readonly MessageParser<HeroSkillInfoProto> _parser = new MessageParser<HeroSkillInfoProto>(() => new HeroSkillInfoProto());

	private UnknownFieldSet _unknownFields;

	public const int SkillIdFieldNumber = 1;

	private int skillId_;

	public const int SkillLvFieldNumber = 2;

	private int skillLv_;

	public const int SlotFieldNumber = 3;

	private int slot_;

	[DebuggerNonUserCode]
	public static MessageParser<HeroSkillInfoProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public int SkillLv
	{
		get
		{
			return skillLv_;
		}
		set
		{
			skillLv_ = value;
		}
	}

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
	public HeroSkillInfoProto()
	{
	}

	[DebuggerNonUserCode]
	public HeroSkillInfoProto(HeroSkillInfoProto other)
		: this()
	{
		skillId_ = other.skillId_;
		skillLv_ = other.skillLv_;
		slot_ = other.slot_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public HeroSkillInfoProto Clone()
	{
		return new HeroSkillInfoProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as HeroSkillInfoProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(HeroSkillInfoProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SkillId != other.SkillId)
		{
			return false;
		}
		if (SkillLv != other.SkillLv)
		{
			return false;
		}
		if (Slot != other.Slot)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SkillId != 0)
		{
			num ^= SkillId.GetHashCode();
		}
		if (SkillLv != 0)
		{
			num ^= SkillLv.GetHashCode();
		}
		if (Slot != 0)
		{
			num ^= Slot.GetHashCode();
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
		if (SkillId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(SkillId);
		}
		if (SkillLv != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(SkillLv);
		}
		if (Slot != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Slot);
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
		if (SkillId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillId);
		}
		if (SkillLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkillLv);
		}
		if (Slot != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Slot);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(HeroSkillInfoProto other)
	{
		if (other != null)
		{
			if (other.SkillId != 0)
			{
				SkillId = other.SkillId;
			}
			if (other.SkillLv != 0)
			{
				SkillLv = other.SkillLv;
			}
			if (other.Slot != 0)
			{
				Slot = other.Slot;
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
				SkillId = input.ReadInt32();
				break;
			case 16u:
				SkillLv = input.ReadInt32();
				break;
			case 24u:
				Slot = input.ReadInt32();
				break;
			}
		}
	}
}
