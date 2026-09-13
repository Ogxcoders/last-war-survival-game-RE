using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ArmyProgress : IMessage<ArmyProgress>, IMessage, IEquatable<ArmyProgress>, IDeepCloneable<ArmyProgress>
{
	private static readonly MessageParser<ArmyProgress> _parser = new MessageParser<ArmyProgress>(() => new ArmyProgress());

	private UnknownFieldSet _unknownFields;

	public const int FormationEquipPowerFieldNumber = 1;

	private int formationEquipPower_;

	public const int SciencePowerFieldNumber = 2;

	private int sciencePower_;

	public const int DecoPowerFieldNumber = 3;

	private int decoPower_;

	public const int EquipIdFieldNumber = 4;

	private static readonly FieldCodec<int> _repeated_equipId_codec = FieldCodec.ForInt32(34u);

	private readonly RepeatedField<int> equipId_ = new RepeatedField<int>();

	public const int ScienceFieldNumber = 5;

	private static readonly FieldCodec<ScienceProgress> _repeated_science_codec = FieldCodec.ForMessage(42u, ScienceProgress.Parser);

	private readonly RepeatedField<ScienceProgress> science_ = new RepeatedField<ScienceProgress>();

	public const int DecoFieldNumber = 6;

	private static readonly FieldCodec<DecorationProgress> _repeated_deco_codec = FieldCodec.ForMessage(50u, DecorationProgress.Parser);

	private readonly RepeatedField<DecorationProgress> deco_ = new RepeatedField<DecorationProgress>();

	public const int TotalHonorLevelFieldNumber = 7;

	private static readonly FieldCodec<int> _repeated_totalHonorLevel_codec = FieldCodec.ForInt32(58u);

	private readonly RepeatedField<int> totalHonorLevel_ = new RepeatedField<int>();

	public const int HonorPowerFieldNumber = 8;

	private int honorPower_;

	public const int HonorWallFieldNumber = 9;

	private static readonly FieldCodec<HonorWallProgress> _repeated_honorWall_codec = FieldCodec.ForMessage(74u, HonorWallProgress.Parser);

	private readonly RepeatedField<HonorWallProgress> honorWall_ = new RepeatedField<HonorWallProgress>();

	public const int ScienceEffectsFieldNumber = 10;

	private static readonly FieldCodec<Effect> _repeated_scienceEffects_codec = FieldCodec.ForMessage(82u, Effect.Parser);

	private readonly RepeatedField<Effect> scienceEffects_ = new RepeatedField<Effect>();

	public const int DecoEffectsFieldNumber = 11;

	private static readonly FieldCodec<Effect> _repeated_decoEffects_codec = FieldCodec.ForMessage(90u, Effect.Parser);

	private readonly RepeatedField<Effect> decoEffects_ = new RepeatedField<Effect>();

	public const int DominatorFieldNumber = 12;

	private DominatorProgress dominator_;

	public const int ExtraEffectsFieldNumber = 13;

	private static readonly FieldCodec<Effect> _repeated_extraEffects_codec = FieldCodec.ForMessage(106u, Effect.Parser);

	private readonly RepeatedField<Effect> extraEffects_ = new RepeatedField<Effect>();

	public const int ExpFieldNumber = 14;

	private static readonly FieldCodec<int> _repeated_exp_codec = FieldCodec.ForInt32(114u);

	private readonly RepeatedField<int> exp_ = new RepeatedField<int>();

	public const int SkinProgressFieldNumber = 15;

	private SkinProgress skinProgress_;

	[DebuggerNonUserCode]
	public static MessageParser<ArmyProgress> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[26];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int FormationEquipPower
	{
		get
		{
			return formationEquipPower_;
		}
		set
		{
			formationEquipPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SciencePower
	{
		get
		{
			return sciencePower_;
		}
		set
		{
			sciencePower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DecoPower
	{
		get
		{
			return decoPower_;
		}
		set
		{
			decoPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<int> EquipId => equipId_;

	[DebuggerNonUserCode]
	public RepeatedField<ScienceProgress> Science => science_;

	[DebuggerNonUserCode]
	public RepeatedField<DecorationProgress> Deco => deco_;

	[DebuggerNonUserCode]
	public RepeatedField<int> TotalHonorLevel => totalHonorLevel_;

	[DebuggerNonUserCode]
	public int HonorPower
	{
		get
		{
			return honorPower_;
		}
		set
		{
			honorPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<HonorWallProgress> HonorWall => honorWall_;

	[DebuggerNonUserCode]
	public RepeatedField<Effect> ScienceEffects => scienceEffects_;

	[DebuggerNonUserCode]
	public RepeatedField<Effect> DecoEffects => decoEffects_;

	[DebuggerNonUserCode]
	public DominatorProgress Dominator
	{
		get
		{
			return dominator_;
		}
		set
		{
			dominator_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Effect> ExtraEffects => extraEffects_;

	[DebuggerNonUserCode]
	public RepeatedField<int> Exp => exp_;

	[DebuggerNonUserCode]
	public SkinProgress SkinProgress
	{
		get
		{
			return skinProgress_;
		}
		set
		{
			skinProgress_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyProgress()
	{
	}

	[DebuggerNonUserCode]
	public ArmyProgress(ArmyProgress other)
		: this()
	{
		formationEquipPower_ = other.formationEquipPower_;
		sciencePower_ = other.sciencePower_;
		decoPower_ = other.decoPower_;
		equipId_ = other.equipId_.Clone();
		science_ = other.science_.Clone();
		deco_ = other.deco_.Clone();
		totalHonorLevel_ = other.totalHonorLevel_.Clone();
		honorPower_ = other.honorPower_;
		honorWall_ = other.honorWall_.Clone();
		scienceEffects_ = other.scienceEffects_.Clone();
		decoEffects_ = other.decoEffects_.Clone();
		dominator_ = ((other.dominator_ != null) ? other.dominator_.Clone() : null);
		extraEffects_ = other.extraEffects_.Clone();
		exp_ = other.exp_.Clone();
		skinProgress_ = ((other.skinProgress_ != null) ? other.skinProgress_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ArmyProgress Clone()
	{
		return new ArmyProgress(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ArmyProgress);
	}

	[DebuggerNonUserCode]
	public bool Equals(ArmyProgress other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (FormationEquipPower != other.FormationEquipPower)
		{
			return false;
		}
		if (SciencePower != other.SciencePower)
		{
			return false;
		}
		if (DecoPower != other.DecoPower)
		{
			return false;
		}
		if (!equipId_.Equals(other.equipId_))
		{
			return false;
		}
		if (!science_.Equals(other.science_))
		{
			return false;
		}
		if (!deco_.Equals(other.deco_))
		{
			return false;
		}
		if (!totalHonorLevel_.Equals(other.totalHonorLevel_))
		{
			return false;
		}
		if (HonorPower != other.HonorPower)
		{
			return false;
		}
		if (!honorWall_.Equals(other.honorWall_))
		{
			return false;
		}
		if (!scienceEffects_.Equals(other.scienceEffects_))
		{
			return false;
		}
		if (!decoEffects_.Equals(other.decoEffects_))
		{
			return false;
		}
		if (!object.Equals(Dominator, other.Dominator))
		{
			return false;
		}
		if (!extraEffects_.Equals(other.extraEffects_))
		{
			return false;
		}
		if (!exp_.Equals(other.exp_))
		{
			return false;
		}
		if (!object.Equals(SkinProgress, other.SkinProgress))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (FormationEquipPower != 0)
		{
			num ^= FormationEquipPower.GetHashCode();
		}
		if (SciencePower != 0)
		{
			num ^= SciencePower.GetHashCode();
		}
		if (DecoPower != 0)
		{
			num ^= DecoPower.GetHashCode();
		}
		num ^= equipId_.GetHashCode();
		num ^= science_.GetHashCode();
		num ^= deco_.GetHashCode();
		num ^= totalHonorLevel_.GetHashCode();
		if (HonorPower != 0)
		{
			num ^= HonorPower.GetHashCode();
		}
		num ^= honorWall_.GetHashCode();
		num ^= scienceEffects_.GetHashCode();
		num ^= decoEffects_.GetHashCode();
		if (dominator_ != null)
		{
			num ^= Dominator.GetHashCode();
		}
		num ^= extraEffects_.GetHashCode();
		num ^= exp_.GetHashCode();
		if (skinProgress_ != null)
		{
			num ^= SkinProgress.GetHashCode();
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
		if (FormationEquipPower != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(FormationEquipPower);
		}
		if (SciencePower != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(SciencePower);
		}
		if (DecoPower != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(DecoPower);
		}
		equipId_.WriteTo(output, _repeated_equipId_codec);
		science_.WriteTo(output, _repeated_science_codec);
		deco_.WriteTo(output, _repeated_deco_codec);
		totalHonorLevel_.WriteTo(output, _repeated_totalHonorLevel_codec);
		if (HonorPower != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(HonorPower);
		}
		honorWall_.WriteTo(output, _repeated_honorWall_codec);
		scienceEffects_.WriteTo(output, _repeated_scienceEffects_codec);
		decoEffects_.WriteTo(output, _repeated_decoEffects_codec);
		if (dominator_ != null)
		{
			output.WriteRawTag(98);
			output.WriteMessage(Dominator);
		}
		extraEffects_.WriteTo(output, _repeated_extraEffects_codec);
		exp_.WriteTo(output, _repeated_exp_codec);
		if (skinProgress_ != null)
		{
			output.WriteRawTag(122);
			output.WriteMessage(SkinProgress);
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
		if (FormationEquipPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FormationEquipPower);
		}
		if (SciencePower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SciencePower);
		}
		if (DecoPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DecoPower);
		}
		num += equipId_.CalculateSize(_repeated_equipId_codec);
		num += science_.CalculateSize(_repeated_science_codec);
		num += deco_.CalculateSize(_repeated_deco_codec);
		num += totalHonorLevel_.CalculateSize(_repeated_totalHonorLevel_codec);
		if (HonorPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HonorPower);
		}
		num += honorWall_.CalculateSize(_repeated_honorWall_codec);
		num += scienceEffects_.CalculateSize(_repeated_scienceEffects_codec);
		num += decoEffects_.CalculateSize(_repeated_decoEffects_codec);
		if (dominator_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Dominator);
		}
		num += extraEffects_.CalculateSize(_repeated_extraEffects_codec);
		num += exp_.CalculateSize(_repeated_exp_codec);
		if (skinProgress_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SkinProgress);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ArmyProgress other)
	{
		if (other == null)
		{
			return;
		}
		if (other.FormationEquipPower != 0)
		{
			FormationEquipPower = other.FormationEquipPower;
		}
		if (other.SciencePower != 0)
		{
			SciencePower = other.SciencePower;
		}
		if (other.DecoPower != 0)
		{
			DecoPower = other.DecoPower;
		}
		equipId_.Add(other.equipId_);
		science_.Add(other.science_);
		deco_.Add(other.deco_);
		totalHonorLevel_.Add(other.totalHonorLevel_);
		if (other.HonorPower != 0)
		{
			HonorPower = other.HonorPower;
		}
		honorWall_.Add(other.honorWall_);
		scienceEffects_.Add(other.scienceEffects_);
		decoEffects_.Add(other.decoEffects_);
		if (other.dominator_ != null)
		{
			if (dominator_ == null)
			{
				Dominator = new DominatorProgress();
			}
			Dominator.MergeFrom(other.Dominator);
		}
		extraEffects_.Add(other.extraEffects_);
		exp_.Add(other.exp_);
		if (other.skinProgress_ != null)
		{
			if (skinProgress_ == null)
			{
				SkinProgress = new SkinProgress();
			}
			SkinProgress.MergeFrom(other.SkinProgress);
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				FormationEquipPower = input.ReadInt32();
				break;
			case 16u:
				SciencePower = input.ReadInt32();
				break;
			case 24u:
				DecoPower = input.ReadInt32();
				break;
			case 32u:
			case 34u:
				equipId_.AddEntriesFrom(input, _repeated_equipId_codec);
				break;
			case 42u:
				science_.AddEntriesFrom(input, _repeated_science_codec);
				break;
			case 50u:
				deco_.AddEntriesFrom(input, _repeated_deco_codec);
				break;
			case 56u:
			case 58u:
				totalHonorLevel_.AddEntriesFrom(input, _repeated_totalHonorLevel_codec);
				break;
			case 64u:
				HonorPower = input.ReadInt32();
				break;
			case 74u:
				honorWall_.AddEntriesFrom(input, _repeated_honorWall_codec);
				break;
			case 82u:
				scienceEffects_.AddEntriesFrom(input, _repeated_scienceEffects_codec);
				break;
			case 90u:
				decoEffects_.AddEntriesFrom(input, _repeated_decoEffects_codec);
				break;
			case 98u:
				if (dominator_ == null)
				{
					Dominator = new DominatorProgress();
				}
				input.ReadMessage(Dominator);
				break;
			case 106u:
				extraEffects_.AddEntriesFrom(input, _repeated_extraEffects_codec);
				break;
			case 112u:
			case 114u:
				exp_.AddEntriesFrom(input, _repeated_exp_codec);
				break;
			case 122u:
				if (skinProgress_ == null)
				{
					SkinProgress = new SkinProgress();
				}
				input.ReadMessage(SkinProgress);
				break;
			}
		}
	}
}
