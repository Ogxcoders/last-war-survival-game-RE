using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class HeroEquipInfoProto : IMessage<HeroEquipInfoProto>, IMessage, IEquatable<HeroEquipInfoProto>, IDeepCloneable<HeroEquipInfoProto>
{
	private static readonly MessageParser<HeroEquipInfoProto> _parser = new MessageParser<HeroEquipInfoProto>(() => new HeroEquipInfoProto());

	private UnknownFieldSet _unknownFields;

	public const int EquipIdFieldNumber = 1;

	private int equipId_;

	public const int EquipLvFieldNumber = 2;

	private int equipLv_;

	public const int PromoteFieldNumber = 3;

	private int promote_;

	[DebuggerNonUserCode]
	public static MessageParser<HeroEquipInfoProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int EquipId
	{
		get
		{
			return equipId_;
		}
		set
		{
			equipId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int EquipLv
	{
		get
		{
			return equipLv_;
		}
		set
		{
			equipLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Promote
	{
		get
		{
			return promote_;
		}
		set
		{
			promote_ = value;
		}
	}

	[DebuggerNonUserCode]
	public HeroEquipInfoProto()
	{
	}

	[DebuggerNonUserCode]
	public HeroEquipInfoProto(HeroEquipInfoProto other)
		: this()
	{
		equipId_ = other.equipId_;
		equipLv_ = other.equipLv_;
		promote_ = other.promote_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public HeroEquipInfoProto Clone()
	{
		return new HeroEquipInfoProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as HeroEquipInfoProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(HeroEquipInfoProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (EquipId != other.EquipId)
		{
			return false;
		}
		if (EquipLv != other.EquipLv)
		{
			return false;
		}
		if (Promote != other.Promote)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (EquipId != 0)
		{
			num ^= EquipId.GetHashCode();
		}
		if (EquipLv != 0)
		{
			num ^= EquipLv.GetHashCode();
		}
		if (Promote != 0)
		{
			num ^= Promote.GetHashCode();
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
		if (EquipId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(EquipId);
		}
		if (EquipLv != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(EquipLv);
		}
		if (Promote != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Promote);
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
		if (EquipId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EquipId);
		}
		if (EquipLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(EquipLv);
		}
		if (Promote != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Promote);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(HeroEquipInfoProto other)
	{
		if (other != null)
		{
			if (other.EquipId != 0)
			{
				EquipId = other.EquipId;
			}
			if (other.EquipLv != 0)
			{
				EquipLv = other.EquipLv;
			}
			if (other.Promote != 0)
			{
				Promote = other.Promote;
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
				EquipId = input.ReadInt32();
				break;
			case 16u:
				EquipLv = input.ReadInt32();
				break;
			case 24u:
				Promote = input.ReadInt32();
				break;
			}
		}
	}
}
