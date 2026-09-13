using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class UnitAttrInfo : IMessage<UnitAttrInfo>, IMessage, IEquatable<UnitAttrInfo>, IDeepCloneable<UnitAttrInfo>
{
	private static readonly MessageParser<UnitAttrInfo> _parser = new MessageParser<UnitAttrInfo>(() => new UnitAttrInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int AtkInfoFieldNumber = 4;

	private static readonly FieldCodec<ArmyAttrInfo> _repeated_atkInfo_codec = FieldCodec.ForMessage(34u, ArmyAttrInfo.Parser);

	private readonly RepeatedField<ArmyAttrInfo> atkInfo_ = new RepeatedField<ArmyAttrInfo>();

	public const int DefInfoFieldNumber = 5;

	private static readonly FieldCodec<ArmyAttrInfo> _repeated_defInfo_codec = FieldCodec.ForMessage(42u, ArmyAttrInfo.Parser);

	private readonly RepeatedField<ArmyAttrInfo> defInfo_ = new RepeatedField<ArmyAttrInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<UnitAttrInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[23];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long Uuid
	{
		get
		{
			return uuid_;
		}
		set
		{
			uuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<ArmyAttrInfo> AtkInfo => atkInfo_;

	[DebuggerNonUserCode]
	public RepeatedField<ArmyAttrInfo> DefInfo => defInfo_;

	[DebuggerNonUserCode]
	public UnitAttrInfo()
	{
	}

	[DebuggerNonUserCode]
	public UnitAttrInfo(UnitAttrInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		atkInfo_ = other.atkInfo_.Clone();
		defInfo_ = other.defInfo_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UnitAttrInfo Clone()
	{
		return new UnitAttrInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UnitAttrInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(UnitAttrInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (!atkInfo_.Equals(other.atkInfo_))
		{
			return false;
		}
		if (!defInfo_.Equals(other.defInfo_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		num ^= atkInfo_.GetHashCode();
		num ^= defInfo_.GetHashCode();
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
		if (Uuid != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(Uuid);
		}
		atkInfo_.WriteTo(output, _repeated_atkInfo_codec);
		defInfo_.WriteTo(output, _repeated_defInfo_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		num += atkInfo_.CalculateSize(_repeated_atkInfo_codec);
		num += defInfo_.CalculateSize(_repeated_defInfo_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UnitAttrInfo other)
	{
		if (other != null)
		{
			if (other.Uuid != 0L)
			{
				Uuid = other.Uuid;
			}
			atkInfo_.Add(other.atkInfo_);
			defInfo_.Add(other.defInfo_);
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
				Uuid = input.ReadInt64();
				break;
			case 34u:
				atkInfo_.AddEntriesFrom(input, _repeated_atkInfo_codec);
				break;
			case 42u:
				defInfo_.AddEntriesFrom(input, _repeated_defInfo_codec);
				break;
			}
		}
	}
}
