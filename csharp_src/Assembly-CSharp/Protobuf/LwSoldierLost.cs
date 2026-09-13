using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwSoldierLost : IMessage<LwSoldierLost>, IMessage, IEquatable<LwSoldierLost>, IDeepCloneable<LwSoldierLost>
{
	private static readonly MessageParser<LwSoldierLost> _parser = new MessageParser<LwSoldierLost>(() => new LwSoldierLost());

	private UnknownFieldSet _unknownFields;

	public const int SoldierIdFieldNumber = 1;

	private int soldierId_;

	public const int LostFieldNumber = 2;

	private int lost_;

	public const int InjuredFieldNumber = 3;

	private int injured_;

	public const int WoundedFieldNumber = 4;

	private int wounded_;

	public const int DeadFieldNumber = 5;

	private int dead_;

	public const int TotalFieldNumber = 6;

	private int total_;

	public const int DegradeFieldNumber = 7;

	private int degrade_;

	[DebuggerNonUserCode]
	public static MessageParser<LwSoldierLost> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[8];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int SoldierId
	{
		get
		{
			return soldierId_;
		}
		set
		{
			soldierId_ = value;
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
	public LwSoldierLost()
	{
	}

	[DebuggerNonUserCode]
	public LwSoldierLost(LwSoldierLost other)
		: this()
	{
		soldierId_ = other.soldierId_;
		lost_ = other.lost_;
		injured_ = other.injured_;
		wounded_ = other.wounded_;
		dead_ = other.dead_;
		total_ = other.total_;
		degrade_ = other.degrade_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwSoldierLost Clone()
	{
		return new LwSoldierLost(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwSoldierLost);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwSoldierLost other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SoldierId != other.SoldierId)
		{
			return false;
		}
		if (Lost != other.Lost)
		{
			return false;
		}
		if (Injured != other.Injured)
		{
			return false;
		}
		if (Wounded != other.Wounded)
		{
			return false;
		}
		if (Dead != other.Dead)
		{
			return false;
		}
		if (Total != other.Total)
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
		if (SoldierId != 0)
		{
			num ^= SoldierId.GetHashCode();
		}
		if (Lost != 0)
		{
			num ^= Lost.GetHashCode();
		}
		if (Injured != 0)
		{
			num ^= Injured.GetHashCode();
		}
		if (Wounded != 0)
		{
			num ^= Wounded.GetHashCode();
		}
		if (Dead != 0)
		{
			num ^= Dead.GetHashCode();
		}
		if (Total != 0)
		{
			num ^= Total.GetHashCode();
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
		if (SoldierId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(SoldierId);
		}
		if (Lost != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Lost);
		}
		if (Injured != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Injured);
		}
		if (Wounded != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(Wounded);
		}
		if (Dead != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Dead);
		}
		if (Total != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(Total);
		}
		if (Degrade != 0)
		{
			output.WriteRawTag(56);
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
		if (SoldierId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SoldierId);
		}
		if (Lost != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Lost);
		}
		if (Injured != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Injured);
		}
		if (Wounded != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Wounded);
		}
		if (Dead != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Dead);
		}
		if (Total != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Total);
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
	public void MergeFrom(LwSoldierLost other)
	{
		if (other != null)
		{
			if (other.SoldierId != 0)
			{
				SoldierId = other.SoldierId;
			}
			if (other.Lost != 0)
			{
				Lost = other.Lost;
			}
			if (other.Injured != 0)
			{
				Injured = other.Injured;
			}
			if (other.Wounded != 0)
			{
				Wounded = other.Wounded;
			}
			if (other.Dead != 0)
			{
				Dead = other.Dead;
			}
			if (other.Total != 0)
			{
				Total = other.Total;
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
			case 8u:
				SoldierId = input.ReadInt32();
				break;
			case 16u:
				Lost = input.ReadInt32();
				break;
			case 24u:
				Injured = input.ReadInt32();
				break;
			case 32u:
				Wounded = input.ReadInt32();
				break;
			case 40u:
				Dead = input.ReadInt32();
				break;
			case 48u:
				Total = input.ReadInt32();
				break;
			case 56u:
				Degrade = input.ReadInt32();
				break;
			}
		}
	}
}
