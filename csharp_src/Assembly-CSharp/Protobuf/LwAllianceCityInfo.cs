using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwAllianceCityInfo : IMessage<LwAllianceCityInfo>, IMessage, IEquatable<LwAllianceCityInfo>, IDeepCloneable<LwAllianceCityInfo>
{
	private static readonly MessageParser<LwAllianceCityInfo> _parser = new MessageParser<LwAllianceCityInfo>(() => new LwAllianceCityInfo());

	private UnknownFieldSet _unknownFields;

	public const int CityIdFieldNumber = 1;

	private int cityId_;

	public const int MaxDurabilityFieldNumber = 2;

	private int maxDurability_;

	public const int DurabilityFieldNumber = 3;

	private int durability_;

	public const int DurabilityBeforeStartFieldNumber = 4;

	private int durabilityBeforeStart_;

	[DebuggerNonUserCode]
	public static MessageParser<LwAllianceCityInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[12];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int CityId
	{
		get
		{
			return cityId_;
		}
		set
		{
			cityId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MaxDurability
	{
		get
		{
			return maxDurability_;
		}
		set
		{
			maxDurability_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Durability
	{
		get
		{
			return durability_;
		}
		set
		{
			durability_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DurabilityBeforeStart
	{
		get
		{
			return durabilityBeforeStart_;
		}
		set
		{
			durabilityBeforeStart_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LwAllianceCityInfo()
	{
	}

	[DebuggerNonUserCode]
	public LwAllianceCityInfo(LwAllianceCityInfo other)
		: this()
	{
		cityId_ = other.cityId_;
		maxDurability_ = other.maxDurability_;
		durability_ = other.durability_;
		durabilityBeforeStart_ = other.durabilityBeforeStart_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwAllianceCityInfo Clone()
	{
		return new LwAllianceCityInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwAllianceCityInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwAllianceCityInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CityId != other.CityId)
		{
			return false;
		}
		if (MaxDurability != other.MaxDurability)
		{
			return false;
		}
		if (Durability != other.Durability)
		{
			return false;
		}
		if (DurabilityBeforeStart != other.DurabilityBeforeStart)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CityId != 0)
		{
			num ^= CityId.GetHashCode();
		}
		if (MaxDurability != 0)
		{
			num ^= MaxDurability.GetHashCode();
		}
		if (Durability != 0)
		{
			num ^= Durability.GetHashCode();
		}
		if (DurabilityBeforeStart != 0)
		{
			num ^= DurabilityBeforeStart.GetHashCode();
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
		if (CityId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(CityId);
		}
		if (MaxDurability != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(MaxDurability);
		}
		if (Durability != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Durability);
		}
		if (DurabilityBeforeStart != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(DurabilityBeforeStart);
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
		if (CityId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CityId);
		}
		if (MaxDurability != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MaxDurability);
		}
		if (Durability != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Durability);
		}
		if (DurabilityBeforeStart != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DurabilityBeforeStart);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwAllianceCityInfo other)
	{
		if (other != null)
		{
			if (other.CityId != 0)
			{
				CityId = other.CityId;
			}
			if (other.MaxDurability != 0)
			{
				MaxDurability = other.MaxDurability;
			}
			if (other.Durability != 0)
			{
				Durability = other.Durability;
			}
			if (other.DurabilityBeforeStart != 0)
			{
				DurabilityBeforeStart = other.DurabilityBeforeStart;
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
				CityId = input.ReadInt32();
				break;
			case 16u:
				MaxDurability = input.ReadInt32();
				break;
			case 24u:
				Durability = input.ReadInt32();
				break;
			case 32u:
				DurabilityBeforeStart = input.ReadInt32();
				break;
			}
		}
	}
}
