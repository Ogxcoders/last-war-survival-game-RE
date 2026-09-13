using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutTowerUnit : IMessage<ScoutTowerUnit>, IMessage, IEquatable<ScoutTowerUnit>, IDeepCloneable<ScoutTowerUnit>
{
	private static readonly MessageParser<ScoutTowerUnit> _parser = new MessageParser<ScoutTowerUnit>(() => new ScoutTowerUnit());

	private UnknownFieldSet _unknownFields;

	public const int LevelFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_level_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? level_;

	public const int AttackFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_attack_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? attack_;

	public const int HpFieldNumber = 3;

	private static readonly FieldCodec<int?> _single_hp_codec = FieldCodec.ForStructWrapper<int>(26u);

	private int? hp_;

	public const int HpMaxFieldNumber = 4;

	private static readonly FieldCodec<int?> _single_hpMax_codec = FieldCodec.ForStructWrapper<int>(34u);

	private int? hpMax_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutTowerUnit> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[23];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? Level
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
	public int? Attack
	{
		get
		{
			return attack_;
		}
		set
		{
			attack_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? Hp
	{
		get
		{
			return hp_;
		}
		set
		{
			hp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? HpMax
	{
		get
		{
			return hpMax_;
		}
		set
		{
			hpMax_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutTowerUnit()
	{
	}

	[DebuggerNonUserCode]
	public ScoutTowerUnit(ScoutTowerUnit other)
		: this()
	{
		Level = other.Level;
		Attack = other.Attack;
		Hp = other.Hp;
		HpMax = other.HpMax;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutTowerUnit Clone()
	{
		return new ScoutTowerUnit(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutTowerUnit);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutTowerUnit other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (Attack != other.Attack)
		{
			return false;
		}
		if (Hp != other.Hp)
		{
			return false;
		}
		if (HpMax != other.HpMax)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (level_.HasValue)
		{
			num ^= Level.GetHashCode();
		}
		if (attack_.HasValue)
		{
			num ^= Attack.GetHashCode();
		}
		if (hp_.HasValue)
		{
			num ^= Hp.GetHashCode();
		}
		if (hpMax_.HasValue)
		{
			num ^= HpMax.GetHashCode();
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
		if (level_.HasValue)
		{
			_single_level_codec.WriteTagAndValue(output, Level);
		}
		if (attack_.HasValue)
		{
			_single_attack_codec.WriteTagAndValue(output, Attack);
		}
		if (hp_.HasValue)
		{
			_single_hp_codec.WriteTagAndValue(output, Hp);
		}
		if (hpMax_.HasValue)
		{
			_single_hpMax_codec.WriteTagAndValue(output, HpMax);
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
		if (level_.HasValue)
		{
			num += _single_level_codec.CalculateSizeWithTag(Level);
		}
		if (attack_.HasValue)
		{
			num += _single_attack_codec.CalculateSizeWithTag(Attack);
		}
		if (hp_.HasValue)
		{
			num += _single_hp_codec.CalculateSizeWithTag(Hp);
		}
		if (hpMax_.HasValue)
		{
			num += _single_hpMax_codec.CalculateSizeWithTag(HpMax);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutTowerUnit other)
	{
		if (other != null)
		{
			if (other.level_.HasValue && (!level_.HasValue || other.Level != 0))
			{
				Level = other.Level;
			}
			if (other.attack_.HasValue && (!attack_.HasValue || other.Attack != 0))
			{
				Attack = other.Attack;
			}
			if (other.hp_.HasValue && (!hp_.HasValue || other.Hp != 0))
			{
				Hp = other.Hp;
			}
			if (other.hpMax_.HasValue && (!hpMax_.HasValue || other.HpMax != 0))
			{
				HpMax = other.HpMax;
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
			case 10u:
			{
				int? num5 = _single_level_codec.Read(input);
				if (!level_.HasValue || num5 != 0)
				{
					Level = num5;
				}
				break;
			}
			case 18u:
			{
				int? num3 = _single_attack_codec.Read(input);
				if (!attack_.HasValue || num3 != 0)
				{
					Attack = num3;
				}
				break;
			}
			case 26u:
			{
				int? num4 = _single_hp_codec.Read(input);
				if (!hp_.HasValue || num4 != 0)
				{
					Hp = num4;
				}
				break;
			}
			case 34u:
			{
				int? num2 = _single_hpMax_codec.Read(input);
				if (!hpMax_.HasValue || num2 != 0)
				{
					HpMax = num2;
				}
				break;
			}
			}
		}
	}
}
