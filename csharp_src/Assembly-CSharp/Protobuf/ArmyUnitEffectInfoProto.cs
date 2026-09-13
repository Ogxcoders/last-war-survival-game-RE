using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ArmyUnitEffectInfoProto : IMessage<ArmyUnitEffectInfoProto>, IMessage, IEquatable<ArmyUnitEffectInfoProto>, IDeepCloneable<ArmyUnitEffectInfoProto>
{
	private static readonly MessageParser<ArmyUnitEffectInfoProto> _parser = new MessageParser<ArmyUnitEffectInfoProto>(() => new ArmyUnitEffectInfoProto());

	private UnknownFieldSet _unknownFields;

	public const int PlayerEffectFieldNumber = 1;

	private static readonly FieldCodec<float> _repeated_playerEffect_codec = FieldCodec.ForFloat(10u);

	private readonly RepeatedField<float> playerEffect_ = new RepeatedField<float>();

	public const int FormationEffectFieldNumber = 2;

	private static readonly FieldCodec<float> _repeated_formationEffect_codec = FieldCodec.ForFloat(18u);

	private readonly RepeatedField<float> formationEffect_ = new RepeatedField<float>();

	public const int UnitEffectReasonsFieldNumber = 3;

	private static readonly FieldCodec<UnitEffectReason> _repeated_unitEffectReasons_codec = FieldCodec.ForMessage(26u, UnitEffectReason.Parser);

	private readonly RepeatedField<UnitEffectReason> unitEffectReasons_ = new RepeatedField<UnitEffectReason>();

	public const int UnitBuffsFieldNumber = 4;

	private static readonly FieldCodec<ArmyUnitBuff> _repeated_unitBuffs_codec = FieldCodec.ForMessage(34u, ArmyUnitBuff.Parser);

	private readonly RepeatedField<ArmyUnitBuff> unitBuffs_ = new RepeatedField<ArmyUnitBuff>();

	public const int UnitEffectFieldNumber = 5;

	private static readonly MapField<int, Effect>.Codec _map_unitEffect_codec = new MapField<int, Effect>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForMessage(18u, Effect.Parser), 42u);

	private readonly MapField<int, Effect> unitEffect_ = new MapField<int, Effect>();

	public const int FormationEffectMapFieldNumber = 6;

	private static readonly MapField<int, Effect>.Codec _map_formationEffectMap_codec = new MapField<int, Effect>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForMessage(18u, Effect.Parser), 50u);

	private readonly MapField<int, Effect> formationEffectMap_ = new MapField<int, Effect>();

	public const int VersionFieldNumber = 7;

	private int version_;

	public const int ExtraEffectMapFieldNumber = 8;

	private static readonly MapField<int, ExtraEffectSource>.Codec _map_extraEffectMap_codec = new MapField<int, ExtraEffectSource>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForMessage(18u, ExtraEffectSource.Parser), 66u);

	private readonly MapField<int, ExtraEffectSource> extraEffectMap_ = new MapField<int, ExtraEffectSource>();

	public const int TempUnitBuffsFieldNumber = 9;

	private static readonly FieldCodec<ArmyUnitBuff> _repeated_tempUnitBuffs_codec = FieldCodec.ForMessage(74u, ArmyUnitBuff.Parser);

	private readonly RepeatedField<ArmyUnitBuff> tempUnitBuffs_ = new RepeatedField<ArmyUnitBuff>();

	public const int TempEffectsFieldNumber = 10;

	private static readonly MapField<int, Effect>.Codec _map_tempEffects_codec = new MapField<int, Effect>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForMessage(18u, Effect.Parser), 82u);

	private readonly MapField<int, Effect> tempEffects_ = new MapField<int, Effect>();

	public const int SoldierSpecialEffectsFieldNumber = 11;

	private static readonly MapField<int, Effect>.Codec _map_soldierSpecialEffects_codec = new MapField<int, Effect>.Codec(FieldCodec.ForInt32(8u, 0), FieldCodec.ForMessage(18u, Effect.Parser), 90u);

	private readonly MapField<int, Effect> soldierSpecialEffects_ = new MapField<int, Effect>();

	[DebuggerNonUserCode]
	public static MessageParser<ArmyUnitEffectInfoProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[18];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<float> PlayerEffect => playerEffect_;

	[DebuggerNonUserCode]
	public RepeatedField<float> FormationEffect => formationEffect_;

	[DebuggerNonUserCode]
	public RepeatedField<UnitEffectReason> UnitEffectReasons => unitEffectReasons_;

	[DebuggerNonUserCode]
	public RepeatedField<ArmyUnitBuff> UnitBuffs => unitBuffs_;

	[DebuggerNonUserCode]
	public MapField<int, Effect> UnitEffect => unitEffect_;

	[DebuggerNonUserCode]
	public MapField<int, Effect> FormationEffectMap => formationEffectMap_;

	[DebuggerNonUserCode]
	public int Version
	{
		get
		{
			return version_;
		}
		set
		{
			version_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MapField<int, ExtraEffectSource> ExtraEffectMap => extraEffectMap_;

	[DebuggerNonUserCode]
	public RepeatedField<ArmyUnitBuff> TempUnitBuffs => tempUnitBuffs_;

	[DebuggerNonUserCode]
	public MapField<int, Effect> TempEffects => tempEffects_;

	[DebuggerNonUserCode]
	public MapField<int, Effect> SoldierSpecialEffects => soldierSpecialEffects_;

	[DebuggerNonUserCode]
	public ArmyUnitEffectInfoProto()
	{
	}

	[DebuggerNonUserCode]
	public ArmyUnitEffectInfoProto(ArmyUnitEffectInfoProto other)
		: this()
	{
		playerEffect_ = other.playerEffect_.Clone();
		formationEffect_ = other.formationEffect_.Clone();
		unitEffectReasons_ = other.unitEffectReasons_.Clone();
		unitBuffs_ = other.unitBuffs_.Clone();
		unitEffect_ = other.unitEffect_.Clone();
		formationEffectMap_ = other.formationEffectMap_.Clone();
		version_ = other.version_;
		extraEffectMap_ = other.extraEffectMap_.Clone();
		tempUnitBuffs_ = other.tempUnitBuffs_.Clone();
		tempEffects_ = other.tempEffects_.Clone();
		soldierSpecialEffects_ = other.soldierSpecialEffects_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ArmyUnitEffectInfoProto Clone()
	{
		return new ArmyUnitEffectInfoProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ArmyUnitEffectInfoProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(ArmyUnitEffectInfoProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!playerEffect_.Equals(other.playerEffect_))
		{
			return false;
		}
		if (!formationEffect_.Equals(other.formationEffect_))
		{
			return false;
		}
		if (!unitEffectReasons_.Equals(other.unitEffectReasons_))
		{
			return false;
		}
		if (!unitBuffs_.Equals(other.unitBuffs_))
		{
			return false;
		}
		if (!UnitEffect.Equals(other.UnitEffect))
		{
			return false;
		}
		if (!FormationEffectMap.Equals(other.FormationEffectMap))
		{
			return false;
		}
		if (Version != other.Version)
		{
			return false;
		}
		if (!ExtraEffectMap.Equals(other.ExtraEffectMap))
		{
			return false;
		}
		if (!tempUnitBuffs_.Equals(other.tempUnitBuffs_))
		{
			return false;
		}
		if (!TempEffects.Equals(other.TempEffects))
		{
			return false;
		}
		if (!SoldierSpecialEffects.Equals(other.SoldierSpecialEffects))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= playerEffect_.GetHashCode();
		num ^= formationEffect_.GetHashCode();
		num ^= unitEffectReasons_.GetHashCode();
		num ^= unitBuffs_.GetHashCode();
		num ^= UnitEffect.GetHashCode();
		num ^= FormationEffectMap.GetHashCode();
		if (Version != 0)
		{
			num ^= Version.GetHashCode();
		}
		num ^= ExtraEffectMap.GetHashCode();
		num ^= tempUnitBuffs_.GetHashCode();
		num ^= TempEffects.GetHashCode();
		num ^= SoldierSpecialEffects.GetHashCode();
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
		playerEffect_.WriteTo(output, _repeated_playerEffect_codec);
		formationEffect_.WriteTo(output, _repeated_formationEffect_codec);
		unitEffectReasons_.WriteTo(output, _repeated_unitEffectReasons_codec);
		unitBuffs_.WriteTo(output, _repeated_unitBuffs_codec);
		unitEffect_.WriteTo(output, _map_unitEffect_codec);
		formationEffectMap_.WriteTo(output, _map_formationEffectMap_codec);
		if (Version != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Version);
		}
		extraEffectMap_.WriteTo(output, _map_extraEffectMap_codec);
		tempUnitBuffs_.WriteTo(output, _repeated_tempUnitBuffs_codec);
		tempEffects_.WriteTo(output, _map_tempEffects_codec);
		soldierSpecialEffects_.WriteTo(output, _map_soldierSpecialEffects_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += playerEffect_.CalculateSize(_repeated_playerEffect_codec);
		num += formationEffect_.CalculateSize(_repeated_formationEffect_codec);
		num += unitEffectReasons_.CalculateSize(_repeated_unitEffectReasons_codec);
		num += unitBuffs_.CalculateSize(_repeated_unitBuffs_codec);
		num += unitEffect_.CalculateSize(_map_unitEffect_codec);
		num += formationEffectMap_.CalculateSize(_map_formationEffectMap_codec);
		if (Version != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Version);
		}
		num += extraEffectMap_.CalculateSize(_map_extraEffectMap_codec);
		num += tempUnitBuffs_.CalculateSize(_repeated_tempUnitBuffs_codec);
		num += tempEffects_.CalculateSize(_map_tempEffects_codec);
		num += soldierSpecialEffects_.CalculateSize(_map_soldierSpecialEffects_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ArmyUnitEffectInfoProto other)
	{
		if (other != null)
		{
			playerEffect_.Add(other.playerEffect_);
			formationEffect_.Add(other.formationEffect_);
			unitEffectReasons_.Add(other.unitEffectReasons_);
			unitBuffs_.Add(other.unitBuffs_);
			unitEffect_.Add(other.unitEffect_);
			formationEffectMap_.Add(other.formationEffectMap_);
			if (other.Version != 0)
			{
				Version = other.Version;
			}
			extraEffectMap_.Add(other.extraEffectMap_);
			tempUnitBuffs_.Add(other.tempUnitBuffs_);
			tempEffects_.Add(other.tempEffects_);
			soldierSpecialEffects_.Add(other.soldierSpecialEffects_);
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
			case 13u:
				playerEffect_.AddEntriesFrom(input, _repeated_playerEffect_codec);
				break;
			case 18u:
			case 21u:
				formationEffect_.AddEntriesFrom(input, _repeated_formationEffect_codec);
				break;
			case 26u:
				unitEffectReasons_.AddEntriesFrom(input, _repeated_unitEffectReasons_codec);
				break;
			case 34u:
				unitBuffs_.AddEntriesFrom(input, _repeated_unitBuffs_codec);
				break;
			case 42u:
				unitEffect_.AddEntriesFrom(input, _map_unitEffect_codec);
				break;
			case 50u:
				formationEffectMap_.AddEntriesFrom(input, _map_formationEffectMap_codec);
				break;
			case 56u:
				Version = input.ReadInt32();
				break;
			case 66u:
				extraEffectMap_.AddEntriesFrom(input, _map_extraEffectMap_codec);
				break;
			case 74u:
				tempUnitBuffs_.AddEntriesFrom(input, _repeated_tempUnitBuffs_codec);
				break;
			case 82u:
				tempEffects_.AddEntriesFrom(input, _map_tempEffects_codec);
				break;
			case 90u:
				soldierSpecialEffects_.AddEntriesFrom(input, _map_soldierSpecialEffects_codec);
				break;
			}
		}
	}
}
