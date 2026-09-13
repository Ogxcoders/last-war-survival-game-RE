using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class SkillChipProto : IMessage<SkillChipProto>, IMessage, IEquatable<SkillChipProto>, IDeepCloneable<SkillChipProto>
{
	private static readonly MessageParser<SkillChipProto> _parser = new MessageParser<SkillChipProto>(() => new SkillChipProto());

	private UnknownFieldSet _unknownFields;

	public const int CfgIdFieldNumber = 1;

	private int cfgId_;

	public const int LvFieldNumber = 2;

	private int lv_;

	public const int StarFieldNumber = 3;

	private int star_;

	[DebuggerNonUserCode]
	public static MessageParser<SkillChipProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[34];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int CfgId
	{
		get
		{
			return cfgId_;
		}
		set
		{
			cfgId_ = value;
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
	public int Star
	{
		get
		{
			return star_;
		}
		set
		{
			star_ = value;
		}
	}

	[DebuggerNonUserCode]
	public SkillChipProto()
	{
	}

	[DebuggerNonUserCode]
	public SkillChipProto(SkillChipProto other)
		: this()
	{
		cfgId_ = other.cfgId_;
		lv_ = other.lv_;
		star_ = other.star_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SkillChipProto Clone()
	{
		return new SkillChipProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SkillChipProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(SkillChipProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CfgId != other.CfgId)
		{
			return false;
		}
		if (Lv != other.Lv)
		{
			return false;
		}
		if (Star != other.Star)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CfgId != 0)
		{
			num ^= CfgId.GetHashCode();
		}
		if (Lv != 0)
		{
			num ^= Lv.GetHashCode();
		}
		if (Star != 0)
		{
			num ^= Star.GetHashCode();
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
		if (CfgId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(CfgId);
		}
		if (Lv != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Lv);
		}
		if (Star != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Star);
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
		if (CfgId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CfgId);
		}
		if (Lv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Lv);
		}
		if (Star != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Star);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SkillChipProto other)
	{
		if (other != null)
		{
			if (other.CfgId != 0)
			{
				CfgId = other.CfgId;
			}
			if (other.Lv != 0)
			{
				Lv = other.Lv;
			}
			if (other.Star != 0)
			{
				Star = other.Star;
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
				CfgId = input.ReadInt32();
				break;
			case 16u:
				Lv = input.ReadInt32();
				break;
			case 24u:
				Star = input.ReadInt32();
				break;
			}
		}
	}
}
