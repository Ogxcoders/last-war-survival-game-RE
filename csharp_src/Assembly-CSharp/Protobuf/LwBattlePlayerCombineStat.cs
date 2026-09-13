using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwBattlePlayerCombineStat : IMessage<LwBattlePlayerCombineStat>, IMessage, IEquatable<LwBattlePlayerCombineStat>, IDeepCloneable<LwBattlePlayerCombineStat>
{
	private static readonly MessageParser<LwBattlePlayerCombineStat> _parser = new MessageParser<LwBattlePlayerCombineStat>(() => new LwBattlePlayerCombineStat());

	private UnknownFieldSet _unknownFields;

	public const int PlayerFieldNumber = 1;

	private LwBattlePlayerStat player_;

	public const int MvpFieldNumber = 2;

	private int mvp_;

	public const int TotalDamageSoldierPowerFieldNumber = 3;

	private int totalDamageSoldierPower_;

	public const int IndexFieldNumber = 6;

	private int index_;

	public const int SideFieldNumber = 7;

	private int side_;

	public const int TotalDamageFieldNumber = 8;

	private int totalDamage_;

	public const int TotalDamageDoubleFieldNumber = 9;

	private double totalDamageDouble_;

	[DebuggerNonUserCode]
	public static MessageParser<LwBattlePlayerCombineStat> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[11];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public LwBattlePlayerStat Player
	{
		get
		{
			return player_;
		}
		set
		{
			player_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Mvp
	{
		get
		{
			return mvp_;
		}
		set
		{
			mvp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TotalDamageSoldierPower
	{
		get
		{
			return totalDamageSoldierPower_;
		}
		set
		{
			totalDamageSoldierPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Index
	{
		get
		{
			return index_;
		}
		set
		{
			index_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Side
	{
		get
		{
			return side_;
		}
		set
		{
			side_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int TotalDamage
	{
		get
		{
			return totalDamage_;
		}
		set
		{
			totalDamage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public double TotalDamageDouble
	{
		get
		{
			return totalDamageDouble_;
		}
		set
		{
			totalDamageDouble_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LwBattlePlayerCombineStat()
	{
	}

	[DebuggerNonUserCode]
	public LwBattlePlayerCombineStat(LwBattlePlayerCombineStat other)
		: this()
	{
		player_ = ((other.player_ != null) ? other.player_.Clone() : null);
		mvp_ = other.mvp_;
		totalDamageSoldierPower_ = other.totalDamageSoldierPower_;
		index_ = other.index_;
		side_ = other.side_;
		totalDamage_ = other.totalDamage_;
		totalDamageDouble_ = other.totalDamageDouble_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwBattlePlayerCombineStat Clone()
	{
		return new LwBattlePlayerCombineStat(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwBattlePlayerCombineStat);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwBattlePlayerCombineStat other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(Player, other.Player))
		{
			return false;
		}
		if (Mvp != other.Mvp)
		{
			return false;
		}
		if (TotalDamageSoldierPower != other.TotalDamageSoldierPower)
		{
			return false;
		}
		if (Index != other.Index)
		{
			return false;
		}
		if (Side != other.Side)
		{
			return false;
		}
		if (TotalDamage != other.TotalDamage)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(TotalDamageDouble, other.TotalDamageDouble))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (player_ != null)
		{
			num ^= Player.GetHashCode();
		}
		if (Mvp != 0)
		{
			num ^= Mvp.GetHashCode();
		}
		if (TotalDamageSoldierPower != 0)
		{
			num ^= TotalDamageSoldierPower.GetHashCode();
		}
		if (Index != 0)
		{
			num ^= Index.GetHashCode();
		}
		if (Side != 0)
		{
			num ^= Side.GetHashCode();
		}
		if (TotalDamage != 0)
		{
			num ^= TotalDamage.GetHashCode();
		}
		if (TotalDamageDouble != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(TotalDamageDouble);
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
		if (player_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(Player);
		}
		if (Mvp != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Mvp);
		}
		if (TotalDamageSoldierPower != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(TotalDamageSoldierPower);
		}
		if (Index != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(Index);
		}
		if (Side != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(Side);
		}
		if (TotalDamage != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(TotalDamage);
		}
		if (TotalDamageDouble != 0.0)
		{
			output.WriteRawTag(73);
			output.WriteDouble(TotalDamageDouble);
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
		if (player_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Player);
		}
		if (Mvp != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Mvp);
		}
		if (TotalDamageSoldierPower != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TotalDamageSoldierPower);
		}
		if (Index != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Index);
		}
		if (Side != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Side);
		}
		if (TotalDamage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(TotalDamage);
		}
		if (TotalDamageDouble != 0.0)
		{
			num += 9;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwBattlePlayerCombineStat other)
	{
		if (other == null)
		{
			return;
		}
		if (other.player_ != null)
		{
			if (player_ == null)
			{
				Player = new LwBattlePlayerStat();
			}
			Player.MergeFrom(other.Player);
		}
		if (other.Mvp != 0)
		{
			Mvp = other.Mvp;
		}
		if (other.TotalDamageSoldierPower != 0)
		{
			TotalDamageSoldierPower = other.TotalDamageSoldierPower;
		}
		if (other.Index != 0)
		{
			Index = other.Index;
		}
		if (other.Side != 0)
		{
			Side = other.Side;
		}
		if (other.TotalDamage != 0)
		{
			TotalDamage = other.TotalDamage;
		}
		if (other.TotalDamageDouble != 0.0)
		{
			TotalDamageDouble = other.TotalDamageDouble;
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
			case 10u:
				if (player_ == null)
				{
					Player = new LwBattlePlayerStat();
				}
				input.ReadMessage(Player);
				break;
			case 16u:
				Mvp = input.ReadInt32();
				break;
			case 24u:
				TotalDamageSoldierPower = input.ReadInt32();
				break;
			case 48u:
				Index = input.ReadInt32();
				break;
			case 56u:
				Side = input.ReadInt32();
				break;
			case 64u:
				TotalDamage = input.ReadInt32();
				break;
			case 73u:
				TotalDamageDouble = input.ReadDouble();
				break;
			}
		}
	}
}
