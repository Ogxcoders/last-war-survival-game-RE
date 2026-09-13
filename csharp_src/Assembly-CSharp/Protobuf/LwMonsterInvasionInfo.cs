using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwMonsterInvasionInfo : IMessage<LwMonsterInvasionInfo>, IMessage, IEquatable<LwMonsterInvasionInfo>, IDeepCloneable<LwMonsterInvasionInfo>
{
	private static readonly MessageParser<LwMonsterInvasionInfo> _parser = new MessageParser<LwMonsterInvasionInfo>(() => new LwMonsterInvasionInfo());

	private UnknownFieldSet _unknownFields;

	public const int DamageFieldNumber = 1;

	private int damage_;

	public const int CurHpFieldNumber = 2;

	private int curHp_;

	public const int IsCritFieldNumber = 3;

	private bool isCrit_;

	[DebuggerNonUserCode]
	public static MessageParser<LwMonsterInvasionInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[13];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Damage
	{
		get
		{
			return damage_;
		}
		set
		{
			damage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CurHp
	{
		get
		{
			return curHp_;
		}
		set
		{
			curHp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsCrit
	{
		get
		{
			return isCrit_;
		}
		set
		{
			isCrit_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LwMonsterInvasionInfo()
	{
	}

	[DebuggerNonUserCode]
	public LwMonsterInvasionInfo(LwMonsterInvasionInfo other)
		: this()
	{
		damage_ = other.damage_;
		curHp_ = other.curHp_;
		isCrit_ = other.isCrit_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwMonsterInvasionInfo Clone()
	{
		return new LwMonsterInvasionInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwMonsterInvasionInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwMonsterInvasionInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Damage != other.Damage)
		{
			return false;
		}
		if (CurHp != other.CurHp)
		{
			return false;
		}
		if (IsCrit != other.IsCrit)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Damage != 0)
		{
			num ^= Damage.GetHashCode();
		}
		if (CurHp != 0)
		{
			num ^= CurHp.GetHashCode();
		}
		if (IsCrit)
		{
			num ^= IsCrit.GetHashCode();
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
		if (Damage != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Damage);
		}
		if (CurHp != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(CurHp);
		}
		if (IsCrit)
		{
			output.WriteRawTag(24);
			output.WriteBool(IsCrit);
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
		if (Damage != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Damage);
		}
		if (CurHp != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CurHp);
		}
		if (IsCrit)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LwMonsterInvasionInfo other)
	{
		if (other != null)
		{
			if (other.Damage != 0)
			{
				Damage = other.Damage;
			}
			if (other.CurHp != 0)
			{
				CurHp = other.CurHp;
			}
			if (other.IsCrit)
			{
				IsCrit = other.IsCrit;
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
				Damage = input.ReadInt32();
				break;
			case 16u:
				CurHp = input.ReadInt32();
				break;
			case 24u:
				IsCrit = input.ReadBool();
				break;
			}
		}
	}
}
