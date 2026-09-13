using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ZoneMobilizationPointInfo : IMessage<ZoneMobilizationPointInfo>, IMessage, IEquatable<ZoneMobilizationPointInfo>, IDeepCloneable<ZoneMobilizationPointInfo>
{
	private static readonly MessageParser<ZoneMobilizationPointInfo> _parser = new MessageParser<ZoneMobilizationPointInfo>(() => new ZoneMobilizationPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int BossIdFieldNumber = 1;

	private int bossId_;

	public const int StageFieldNumber = 2;

	private int stage_;

	public const int DonateMaxFieldNumber = 3;

	private bool donateMax_;

	[DebuggerNonUserCode]
	public static MessageParser<ZoneMobilizationPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[46];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int BossId
	{
		get
		{
			return bossId_;
		}
		set
		{
			bossId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Stage
	{
		get
		{
			return stage_;
		}
		set
		{
			stage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool DonateMax
	{
		get
		{
			return donateMax_;
		}
		set
		{
			donateMax_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ZoneMobilizationPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public ZoneMobilizationPointInfo(ZoneMobilizationPointInfo other)
		: this()
	{
		bossId_ = other.bossId_;
		stage_ = other.stage_;
		donateMax_ = other.donateMax_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ZoneMobilizationPointInfo Clone()
	{
		return new ZoneMobilizationPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ZoneMobilizationPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ZoneMobilizationPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BossId != other.BossId)
		{
			return false;
		}
		if (Stage != other.Stage)
		{
			return false;
		}
		if (DonateMax != other.DonateMax)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (BossId != 0)
		{
			num ^= BossId.GetHashCode();
		}
		if (Stage != 0)
		{
			num ^= Stage.GetHashCode();
		}
		if (DonateMax)
		{
			num ^= DonateMax.GetHashCode();
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
		if (BossId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(BossId);
		}
		if (Stage != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Stage);
		}
		if (DonateMax)
		{
			output.WriteRawTag(24);
			output.WriteBool(DonateMax);
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
		if (BossId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BossId);
		}
		if (Stage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Stage);
		}
		if (DonateMax)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ZoneMobilizationPointInfo other)
	{
		if (other != null)
		{
			if (other.BossId != 0)
			{
				BossId = other.BossId;
			}
			if (other.Stage != 0)
			{
				Stage = other.Stage;
			}
			if (other.DonateMax)
			{
				DonateMax = other.DonateMax;
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
				BossId = input.ReadInt32();
				break;
			case 16u:
				Stage = input.ReadInt32();
				break;
			case 24u:
				DonateMax = input.ReadBool();
				break;
			}
		}
	}
}
