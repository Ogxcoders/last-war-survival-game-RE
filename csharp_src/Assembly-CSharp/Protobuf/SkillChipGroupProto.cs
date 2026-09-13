using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class SkillChipGroupProto : IMessage<SkillChipGroupProto>, IMessage, IEquatable<SkillChipGroupProto>, IDeepCloneable<SkillChipGroupProto>
{
	private static readonly MessageParser<SkillChipGroupProto> _parser = new MessageParser<SkillChipGroupProto>(() => new SkillChipGroupProto());

	private UnknownFieldSet _unknownFields;

	public const int GroupFieldNumber = 1;

	private int group_;

	public const int SkillChipInfosFieldNumber = 2;

	private static readonly FieldCodec<SkillChipProto> _repeated_skillChipInfos_codec = FieldCodec.ForMessage(18u, SkillChipProto.Parser);

	private readonly RepeatedField<SkillChipProto> skillChipInfos_ = new RepeatedField<SkillChipProto>();

	[DebuggerNonUserCode]
	public static MessageParser<SkillChipGroupProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[35];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Group
	{
		get
		{
			return group_;
		}
		set
		{
			group_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<SkillChipProto> SkillChipInfos => skillChipInfos_;

	[DebuggerNonUserCode]
	public SkillChipGroupProto()
	{
	}

	[DebuggerNonUserCode]
	public SkillChipGroupProto(SkillChipGroupProto other)
		: this()
	{
		group_ = other.group_;
		skillChipInfos_ = other.skillChipInfos_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SkillChipGroupProto Clone()
	{
		return new SkillChipGroupProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SkillChipGroupProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(SkillChipGroupProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Group != other.Group)
		{
			return false;
		}
		if (!skillChipInfos_.Equals(other.skillChipInfos_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Group != 0)
		{
			num ^= Group.GetHashCode();
		}
		num ^= skillChipInfos_.GetHashCode();
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
		if (Group != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Group);
		}
		skillChipInfos_.WriteTo(output, _repeated_skillChipInfos_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (Group != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Group);
		}
		num += skillChipInfos_.CalculateSize(_repeated_skillChipInfos_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SkillChipGroupProto other)
	{
		if (other != null)
		{
			if (other.Group != 0)
			{
				Group = other.Group;
			}
			skillChipInfos_.Add(other.skillChipInfos_);
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
				Group = input.ReadInt32();
				break;
			case 18u:
				skillChipInfos_.AddEntriesFrom(input, _repeated_skillChipInfos_codec);
				break;
			}
		}
	}
}
