using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WeaponProto : IMessage<WeaponProto>, IMessage, IEquatable<WeaponProto>, IDeepCloneable<WeaponProto>
{
	private static readonly MessageParser<WeaponProto> _parser = new MessageParser<WeaponProto>(() => new WeaponProto());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private int id_;

	public const int LvFieldNumber = 2;

	private int lv_;

	public const int SkillInfosFieldNumber = 3;

	private static readonly FieldCodec<HeroSkillInfoProto> _repeated_skillInfos_codec = FieldCodec.ForMessage(26u, HeroSkillInfoProto.Parser);

	private readonly RepeatedField<HeroSkillInfoProto> skillInfos_ = new RepeatedField<HeroSkillInfoProto>();

	public const int EffectInfosFieldNumber = 4;

	private static readonly FieldCodec<HeroEffectProto> _repeated_effectInfos_codec = FieldCodec.ForMessage(34u, HeroEffectProto.Parser);

	private readonly RepeatedField<HeroEffectProto> effectInfos_ = new RepeatedField<HeroEffectProto>();

	public const int SkinIdFieldNumber = 5;

	private int skinId_;

	public const int ChipsFieldNumber = 6;

	private static readonly FieldCodec<SkillChipProto> _repeated_chips_codec = FieldCodec.ForMessage(50u, SkillChipProto.Parser);

	private readonly RepeatedField<SkillChipProto> chips_ = new RepeatedField<SkillChipProto>();

	public const int TotalChipLvFieldNumber = 7;

	private int totalChipLv_;

	public const int UavExpFieldNumber = 8;

	private int uavExp_;

	[DebuggerNonUserCode]
	public static MessageParser<WeaponProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[13];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = value;
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
	public RepeatedField<HeroSkillInfoProto> SkillInfos => skillInfos_;

	[DebuggerNonUserCode]
	public RepeatedField<HeroEffectProto> EffectInfos => effectInfos_;

	[DebuggerNonUserCode]
	public int SkinId
	{
		get
		{
			return skinId_;
		}
		set
		{
			skinId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<SkillChipProto> Chips => chips_;

	[DebuggerNonUserCode]
	public int TotalChipLv
	{
		get
		{
			return totalChipLv_;
		}
		set
		{
			totalChipLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int UavExp
	{
		get
		{
			return uavExp_;
		}
		set
		{
			uavExp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WeaponProto()
	{
	}

	[DebuggerNonUserCode]
	public WeaponProto(WeaponProto other)
		: this()
	{
		id_ = other.id_;
		lv_ = other.lv_;
		skillInfos_ = other.skillInfos_.Clone();
		effectInfos_ = other.effectInfos_.Clone();
		skinId_ = other.skinId_;
		chips_ = other.chips_.Clone();
		totalChipLv_ = other.totalChipLv_;
		uavExp_ = other.uavExp_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WeaponProto Clone()
	{
		return new WeaponProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WeaponProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(WeaponProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (Lv != other.Lv)
		{
			return false;
		}
		if (!skillInfos_.Equals(other.skillInfos_))
		{
			return false;
		}
		if (!effectInfos_.Equals(other.effectInfos_))
		{
			return false;
		}
		if (SkinId != other.SkinId)
		{
			return false;
		}
		if (!chips_.Equals(other.chips_))
		{
			return false;
		}
		if (TotalChipLv != other.TotalChipLv)
		{
			return false;
		}
		if (UavExp != other.UavExp)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Id != 0)
		{
			num ^= Id.GetHashCode();
		}
		if (Lv != 0)
		{
			num ^= Lv.GetHashCode();
		}
		num ^= skillInfos_.GetHashCode();
		num ^= effectInfos_.GetHashCode();
		if (SkinId != 0)
		{
			num ^= SkinId.GetHashCode();
		}
		num ^= chips_.GetHashCode();
		if (TotalChipLv != 0)
		{
			num ^= TotalChipLv.GetHashCode();
		}
		if (UavExp != 0)
		{
			num ^= UavExp.GetHashCode();
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
		if (Id != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Id);
		}
		if (Lv != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Lv);
		}
		skillInfos_.WriteTo(output, _repeated_skillInfos_codec);
		effectInfos_.WriteTo(output, _repeated_effectInfos_codec);
		if (SkinId != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(SkinId);
		}
		chips_.WriteTo(output, _repeated_chips_codec);
		if (TotalChipLv != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(TotalChipLv);
		}
		if (UavExp != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(UavExp);
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
		if (Id != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Id);
		}
		if (Lv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Lv);
		}
		num += skillInfos_.CalculateSize(_repeated_skillInfos_codec);
		num += effectInfos_.CalculateSize(_repeated_effectInfos_codec);
		if (SkinId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkinId);
		}
		num += chips_.CalculateSize(_repeated_chips_codec);
		if (TotalChipLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TotalChipLv);
		}
		if (UavExp != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(UavExp);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WeaponProto other)
	{
		if (other != null)
		{
			if (other.Id != 0)
			{
				Id = other.Id;
			}
			if (other.Lv != 0)
			{
				Lv = other.Lv;
			}
			skillInfos_.Add(other.skillInfos_);
			effectInfos_.Add(other.effectInfos_);
			if (other.SkinId != 0)
			{
				SkinId = other.SkinId;
			}
			chips_.Add(other.chips_);
			if (other.TotalChipLv != 0)
			{
				TotalChipLv = other.TotalChipLv;
			}
			if (other.UavExp != 0)
			{
				UavExp = other.UavExp;
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
				Id = input.ReadInt32();
				break;
			case 16u:
				Lv = input.ReadInt32();
				break;
			case 26u:
				skillInfos_.AddEntriesFrom(input, _repeated_skillInfos_codec);
				break;
			case 34u:
				effectInfos_.AddEntriesFrom(input, _repeated_effectInfos_codec);
				break;
			case 40u:
				SkinId = input.ReadInt32();
				break;
			case 50u:
				chips_.AddEntriesFrom(input, _repeated_chips_codec);
				break;
			case 56u:
				TotalChipLv = input.ReadInt32();
				break;
			case 64u:
				UavExp = input.ReadInt32();
				break;
			}
		}
	}
}
