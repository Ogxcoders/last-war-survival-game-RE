using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class SoldierProto : IMessage<SoldierProto>, IMessage, IEquatable<SoldierProto>, IDeepCloneable<SoldierProto>
{
	private static readonly MessageParser<SoldierProto> _parser = new MessageParser<SoldierProto>(() => new SoldierProto());

	private UnknownFieldSet _unknownFields;

	public const int ArmsIdFieldNumber = 1;

	private string armsId_ = "";

	public const int TypeFieldNumber = 2;

	private int type_;

	public const int TotalFieldNumber = 3;

	private int total_;

	public const int LostFieldNumber = 4;

	private int lost_;

	public const int WoundedFieldNumber = 5;

	private int wounded_;

	public const int InjuredFieldNumber = 6;

	private int injured_;

	public const int DeadFieldNumber = 7;

	private int dead_;

	public const int CureFieldNumber = 8;

	private int cure_;

	public const int DegradeFieldNumber = 9;

	private int degrade_;

	[DebuggerNonUserCode]
	public static MessageParser<SoldierProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string ArmsId
	{
		get
		{
			return armsId_;
		}
		set
		{
			armsId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Total
	{
		get
		{
			return total_;
		}
		set
		{
			total_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Lost
	{
		get
		{
			return lost_;
		}
		set
		{
			lost_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Wounded
	{
		get
		{
			return wounded_;
		}
		set
		{
			wounded_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Injured
	{
		get
		{
			return injured_;
		}
		set
		{
			injured_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Dead
	{
		get
		{
			return dead_;
		}
		set
		{
			dead_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Cure
	{
		get
		{
			return cure_;
		}
		set
		{
			cure_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Degrade
	{
		get
		{
			return degrade_;
		}
		set
		{
			degrade_ = value;
		}
	}

	[DebuggerNonUserCode]
	public SoldierProto()
	{
	}

	[DebuggerNonUserCode]
	public SoldierProto(SoldierProto other)
		: this()
	{
		armsId_ = other.armsId_;
		type_ = other.type_;
		total_ = other.total_;
		lost_ = other.lost_;
		wounded_ = other.wounded_;
		injured_ = other.injured_;
		dead_ = other.dead_;
		cure_ = other.cure_;
		degrade_ = other.degrade_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SoldierProto Clone()
	{
		return new SoldierProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SoldierProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(SoldierProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ArmsId != other.ArmsId)
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (Total != other.Total)
		{
			return false;
		}
		if (Lost != other.Lost)
		{
			return false;
		}
		if (Wounded != other.Wounded)
		{
			return false;
		}
		if (Injured != other.Injured)
		{
			return false;
		}
		if (Dead != other.Dead)
		{
			return false;
		}
		if (Cure != other.Cure)
		{
			return false;
		}
		if (Degrade != other.Degrade)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ArmsId.Length != 0)
		{
			num ^= ArmsId.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (Total != 0)
		{
			num ^= Total.GetHashCode();
		}
		if (Lost != 0)
		{
			num ^= Lost.GetHashCode();
		}
		if (Wounded != 0)
		{
			num ^= Wounded.GetHashCode();
		}
		if (Injured != 0)
		{
			num ^= Injured.GetHashCode();
		}
		if (Dead != 0)
		{
			num ^= Dead.GetHashCode();
		}
		if (Cure != 0)
		{
			num ^= Cure.GetHashCode();
		}
		if (Degrade != 0)
		{
			num ^= Degrade.GetHashCode();
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
		if (ArmsId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(ArmsId);
		}
		if (Type != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Type);
		}
		if (Total != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Total);
		}
		if (Lost != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Lost);
		}
		if (Wounded != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Wounded);
		}
		if (Injured != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(Injured);
		}
		if (Dead != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Dead);
		}
		if (Cure != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(Cure);
		}
		if (Degrade != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Degrade);
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
		if (ArmsId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ArmsId);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (Total != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Total);
		}
		if (Lost != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Lost);
		}
		if (Wounded != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Wounded);
		}
		if (Injured != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Injured);
		}
		if (Dead != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Dead);
		}
		if (Cure != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Cure);
		}
		if (Degrade != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Degrade);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SoldierProto other)
	{
		if (other != null)
		{
			if (other.ArmsId.Length != 0)
			{
				ArmsId = other.ArmsId;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.Total != 0)
			{
				Total = other.Total;
			}
			if (other.Lost != 0)
			{
				Lost = other.Lost;
			}
			if (other.Wounded != 0)
			{
				Wounded = other.Wounded;
			}
			if (other.Injured != 0)
			{
				Injured = other.Injured;
			}
			if (other.Dead != 0)
			{
				Dead = other.Dead;
			}
			if (other.Cure != 0)
			{
				Cure = other.Cure;
			}
			if (other.Degrade != 0)
			{
				Degrade = other.Degrade;
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
			case 10u:
				ArmsId = input.ReadString();
				break;
			case 16u:
				Type = input.ReadInt32();
				break;
			case 24u:
				Total = input.ReadInt32();
				break;
			case 32u:
				Lost = input.ReadInt32();
				break;
			case 40u:
				Wounded = input.ReadInt32();
				break;
			case 48u:
				Injured = input.ReadInt32();
				break;
			case 56u:
				Dead = input.ReadInt32();
				break;
			case 64u:
				Cure = input.ReadInt32();
				break;
			case 72u:
				Degrade = input.ReadInt32();
				break;
			}
		}
	}
}
