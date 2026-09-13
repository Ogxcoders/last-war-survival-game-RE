using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DominatorProto : IMessage<DominatorProto>, IMessage, IEquatable<DominatorProto>, IDeepCloneable<DominatorProto>
{
	private static readonly MessageParser<DominatorProto> _parser = new MessageParser<DominatorProto>(() => new DominatorProto());

	private UnknownFieldSet _unknownFields;

	public const int DominatorIdFieldNumber = 1;

	private int dominatorId_;

	public const int RankLvFieldNumber = 2;

	private int rankLv_;

	public const int SkillInfosFieldNumber = 3;

	private static readonly FieldCodec<HeroSkillInfoProto> _repeated_skillInfos_codec = FieldCodec.ForMessage(26u, HeroSkillInfoProto.Parser);

	private readonly RepeatedField<HeroSkillInfoProto> skillInfos_ = new RepeatedField<HeroSkillInfoProto>();

	public const int DominatorUuidFieldNumber = 4;

	private long dominatorUuid_;

	public const int EffectInfosFieldNumber = 5;

	private static readonly FieldCodec<HeroEffectProto> _repeated_effectInfos_codec = FieldCodec.ForMessage(42u, HeroEffectProto.Parser);

	private readonly RepeatedField<HeroEffectProto> effectInfos_ = new RepeatedField<HeroEffectProto>();

	public const int NameFieldNumber = 6;

	private string name_ = "";

	public const int LevelFieldNumber = 7;

	private int level_;

	[DebuggerNonUserCode]
	public static MessageParser<DominatorProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int DominatorId
	{
		get
		{
			return dominatorId_;
		}
		set
		{
			dominatorId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int RankLv
	{
		get
		{
			return rankLv_;
		}
		set
		{
			rankLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<HeroSkillInfoProto> SkillInfos => skillInfos_;

	[DebuggerNonUserCode]
	public long DominatorUuid
	{
		get
		{
			return dominatorUuid_;
		}
		set
		{
			dominatorUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<HeroEffectProto> EffectInfos => effectInfos_;

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public DominatorProto()
	{
	}

	[DebuggerNonUserCode]
	public DominatorProto(DominatorProto other)
		: this()
	{
		dominatorId_ = other.dominatorId_;
		rankLv_ = other.rankLv_;
		skillInfos_ = other.skillInfos_.Clone();
		dominatorUuid_ = other.dominatorUuid_;
		effectInfos_ = other.effectInfos_.Clone();
		name_ = other.name_;
		level_ = other.level_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DominatorProto Clone()
	{
		return new DominatorProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DominatorProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(DominatorProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (DominatorId != other.DominatorId)
		{
			return false;
		}
		if (RankLv != other.RankLv)
		{
			return false;
		}
		if (!skillInfos_.Equals(other.skillInfos_))
		{
			return false;
		}
		if (DominatorUuid != other.DominatorUuid)
		{
			return false;
		}
		if (!effectInfos_.Equals(other.effectInfos_))
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (DominatorId != 0)
		{
			num ^= DominatorId.GetHashCode();
		}
		if (RankLv != 0)
		{
			num ^= RankLv.GetHashCode();
		}
		num ^= skillInfos_.GetHashCode();
		if (DominatorUuid != 0L)
		{
			num ^= DominatorUuid.GetHashCode();
		}
		num ^= effectInfos_.GetHashCode();
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
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
		if (DominatorId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(DominatorId);
		}
		if (RankLv != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(RankLv);
		}
		skillInfos_.WriteTo(output, _repeated_skillInfos_codec);
		if (DominatorUuid != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(DominatorUuid);
		}
		effectInfos_.WriteTo(output, _repeated_effectInfos_codec);
		if (Name.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(Name);
		}
		if (Level != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Level);
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
		if (DominatorId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DominatorId);
		}
		if (RankLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RankLv);
		}
		num += skillInfos_.CalculateSize(_repeated_skillInfos_codec);
		if (DominatorUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(DominatorUuid);
		}
		num += effectInfos_.CalculateSize(_repeated_effectInfos_codec);
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DominatorProto other)
	{
		if (other != null)
		{
			if (other.DominatorId != 0)
			{
				DominatorId = other.DominatorId;
			}
			if (other.RankLv != 0)
			{
				RankLv = other.RankLv;
			}
			skillInfos_.Add(other.skillInfos_);
			if (other.DominatorUuid != 0L)
			{
				DominatorUuid = other.DominatorUuid;
			}
			effectInfos_.Add(other.effectInfos_);
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
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
				DominatorId = input.ReadInt32();
				break;
			case 16u:
				RankLv = input.ReadInt32();
				break;
			case 26u:
				skillInfos_.AddEntriesFrom(input, _repeated_skillInfos_codec);
				break;
			case 32u:
				DominatorUuid = input.ReadInt64();
				break;
			case 42u:
				effectInfos_.AddEntriesFrom(input, _repeated_effectInfos_codec);
				break;
			case 50u:
				Name = input.ReadString();
				break;
			case 56u:
				Level = input.ReadInt32();
				break;
			}
		}
	}
}
