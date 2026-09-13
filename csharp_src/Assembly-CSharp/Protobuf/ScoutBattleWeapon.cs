using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutBattleWeapon : IMessage<ScoutBattleWeapon>, IMessage, IEquatable<ScoutBattleWeapon>, IDeepCloneable<ScoutBattleWeapon>
{
	private static readonly MessageParser<ScoutBattleWeapon> _parser = new MessageParser<ScoutBattleWeapon>(() => new ScoutBattleWeapon());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private int id_;

	public const int LevelFieldNumber = 2;

	private int level_;

	public const int ChipGroupsFieldNumber = 3;

	private static readonly FieldCodec<SkillChipGroupProto> _repeated_chipGroups_codec = FieldCodec.ForMessage(26u, SkillChipGroupProto.Parser);

	private readonly RepeatedField<SkillChipGroupProto> chipGroups_ = new RepeatedField<SkillChipGroupProto>();

	public const int EffectsFieldNumber = 4;

	private static readonly FieldCodec<BattleEffectInfo> _repeated_effects_codec = FieldCodec.ForMessage(34u, BattleEffectInfo.Parser);

	private readonly RepeatedField<BattleEffectInfo> effects_ = new RepeatedField<BattleEffectInfo>();

	public const int SkinIdFieldNumber = 5;

	private int skinId_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutBattleWeapon> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[20];

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
	public RepeatedField<SkillChipGroupProto> ChipGroups => chipGroups_;

	[DebuggerNonUserCode]
	public RepeatedField<BattleEffectInfo> Effects => effects_;

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
	public ScoutBattleWeapon()
	{
	}

	[DebuggerNonUserCode]
	public ScoutBattleWeapon(ScoutBattleWeapon other)
		: this()
	{
		id_ = other.id_;
		level_ = other.level_;
		chipGroups_ = other.chipGroups_.Clone();
		effects_ = other.effects_.Clone();
		skinId_ = other.skinId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutBattleWeapon Clone()
	{
		return new ScoutBattleWeapon(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutBattleWeapon);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutBattleWeapon other)
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
		if (Level != other.Level)
		{
			return false;
		}
		if (!chipGroups_.Equals(other.chipGroups_))
		{
			return false;
		}
		if (!effects_.Equals(other.effects_))
		{
			return false;
		}
		if (SkinId != other.SkinId)
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
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		num ^= chipGroups_.GetHashCode();
		num ^= effects_.GetHashCode();
		if (SkinId != 0)
		{
			num ^= SkinId.GetHashCode();
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
		if (Level != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Level);
		}
		chipGroups_.WriteTo(output, _repeated_chipGroups_codec);
		effects_.WriteTo(output, _repeated_effects_codec);
		if (SkinId != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(SkinId);
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
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		num += chipGroups_.CalculateSize(_repeated_chipGroups_codec);
		num += effects_.CalculateSize(_repeated_effects_codec);
		if (SkinId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkinId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutBattleWeapon other)
	{
		if (other != null)
		{
			if (other.Id != 0)
			{
				Id = other.Id;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			chipGroups_.Add(other.chipGroups_);
			effects_.Add(other.effects_);
			if (other.SkinId != 0)
			{
				SkinId = other.SkinId;
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
				Level = input.ReadInt32();
				break;
			case 26u:
				chipGroups_.AddEntriesFrom(input, _repeated_chipGroups_codec);
				break;
			case 34u:
				effects_.AddEntriesFrom(input, _repeated_effects_codec);
				break;
			case 40u:
				SkinId = input.ReadInt32();
				break;
			}
		}
	}
}
