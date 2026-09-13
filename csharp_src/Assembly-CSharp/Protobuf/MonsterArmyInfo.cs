using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MonsterArmyInfo : IMessage<MonsterArmyInfo>, IMessage, IEquatable<MonsterArmyInfo>, IDeepCloneable<MonsterArmyInfo>
{
	public enum ExtraOneofCase
	{
		None = 0,
		MummyPatrol = 101,
		MuseMummy = 102,
		DarknessMonster = 103
	}

	private static readonly MessageParser<MonsterArmyInfo> _parser = new MessageParser<MonsterArmyInfo>(() => new MonsterArmyInfo());

	private UnknownFieldSet _unknownFields;

	public const int MonsterIdFieldNumber = 1;

	private int monsterId_;

	public const int ArmyUnitsFieldNumber = 2;

	private BatchArmyUnit armyUnits_;

	public const int ExpireTimeFieldNumber = 3;

	private long expireTime_;

	public const int MummyPatrolFieldNumber = 101;

	public const int MuseMummyFieldNumber = 102;

	public const int DarknessMonsterFieldNumber = 103;

	private object extra_;

	private ExtraOneofCase extraCase_;

	[DebuggerNonUserCode]
	public static MessageParser<MonsterArmyInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[32];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int MonsterId
	{
		get
		{
			return monsterId_;
		}
		set
		{
			monsterId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BatchArmyUnit ArmyUnits
	{
		get
		{
			return armyUnits_;
		}
		set
		{
			armyUnits_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ExpireTime
	{
		get
		{
			return expireTime_;
		}
		set
		{
			expireTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MummyPatrol MummyPatrol
	{
		get
		{
			if (extraCase_ != ExtraOneofCase.MummyPatrol)
			{
				return null;
			}
			return (MummyPatrol)extra_;
		}
		set
		{
			extra_ = value;
			extraCase_ = ((value != null) ? ExtraOneofCase.MummyPatrol : ExtraOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public MuseMummy MuseMummy
	{
		get
		{
			if (extraCase_ != ExtraOneofCase.MuseMummy)
			{
				return null;
			}
			return (MuseMummy)extra_;
		}
		set
		{
			extra_ = value;
			extraCase_ = ((value != null) ? ExtraOneofCase.MuseMummy : ExtraOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public DarknessMonster DarknessMonster
	{
		get
		{
			if (extraCase_ != ExtraOneofCase.DarknessMonster)
			{
				return null;
			}
			return (DarknessMonster)extra_;
		}
		set
		{
			extra_ = value;
			extraCase_ = ((value != null) ? ExtraOneofCase.DarknessMonster : ExtraOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public ExtraOneofCase ExtraCase => extraCase_;

	[DebuggerNonUserCode]
	public MonsterArmyInfo()
	{
	}

	[DebuggerNonUserCode]
	public MonsterArmyInfo(MonsterArmyInfo other)
		: this()
	{
		monsterId_ = other.monsterId_;
		armyUnits_ = ((other.armyUnits_ != null) ? other.armyUnits_.Clone() : null);
		expireTime_ = other.expireTime_;
		switch (other.ExtraCase)
		{
		case ExtraOneofCase.MummyPatrol:
			MummyPatrol = other.MummyPatrol.Clone();
			break;
		case ExtraOneofCase.MuseMummy:
			MuseMummy = other.MuseMummy.Clone();
			break;
		case ExtraOneofCase.DarknessMonster:
			DarknessMonster = other.DarknessMonster.Clone();
			break;
		}
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MonsterArmyInfo Clone()
	{
		return new MonsterArmyInfo(this);
	}

	[DebuggerNonUserCode]
	public void ClearExtra()
	{
		extraCase_ = ExtraOneofCase.None;
		extra_ = null;
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MonsterArmyInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(MonsterArmyInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MonsterId != other.MonsterId)
		{
			return false;
		}
		if (!object.Equals(ArmyUnits, other.ArmyUnits))
		{
			return false;
		}
		if (ExpireTime != other.ExpireTime)
		{
			return false;
		}
		if (!object.Equals(MummyPatrol, other.MummyPatrol))
		{
			return false;
		}
		if (!object.Equals(MuseMummy, other.MuseMummy))
		{
			return false;
		}
		if (!object.Equals(DarknessMonster, other.DarknessMonster))
		{
			return false;
		}
		if (ExtraCase != other.ExtraCase)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MonsterId != 0)
		{
			num ^= MonsterId.GetHashCode();
		}
		if (armyUnits_ != null)
		{
			num ^= ArmyUnits.GetHashCode();
		}
		if (ExpireTime != 0L)
		{
			num ^= ExpireTime.GetHashCode();
		}
		if (extraCase_ == ExtraOneofCase.MummyPatrol)
		{
			num ^= MummyPatrol.GetHashCode();
		}
		if (extraCase_ == ExtraOneofCase.MuseMummy)
		{
			num ^= MuseMummy.GetHashCode();
		}
		if (extraCase_ == ExtraOneofCase.DarknessMonster)
		{
			num ^= DarknessMonster.GetHashCode();
		}
		num ^= (int)extraCase_;
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
		if (MonsterId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(MonsterId);
		}
		if (armyUnits_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(ArmyUnits);
		}
		if (ExpireTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(ExpireTime);
		}
		if (extraCase_ == ExtraOneofCase.MummyPatrol)
		{
			output.WriteRawTag(170, 6);
			output.WriteMessage(MummyPatrol);
		}
		if (extraCase_ == ExtraOneofCase.MuseMummy)
		{
			output.WriteRawTag(178, 6);
			output.WriteMessage(MuseMummy);
		}
		if (extraCase_ == ExtraOneofCase.DarknessMonster)
		{
			output.WriteRawTag(186, 6);
			output.WriteMessage(DarknessMonster);
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
		if (MonsterId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MonsterId);
		}
		if (armyUnits_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ArmyUnits);
		}
		if (ExpireTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ExpireTime);
		}
		if (extraCase_ == ExtraOneofCase.MummyPatrol)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MummyPatrol);
		}
		if (extraCase_ == ExtraOneofCase.MuseMummy)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MuseMummy);
		}
		if (extraCase_ == ExtraOneofCase.DarknessMonster)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(DarknessMonster);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MonsterArmyInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.MonsterId != 0)
		{
			MonsterId = other.MonsterId;
		}
		if (other.armyUnits_ != null)
		{
			if (armyUnits_ == null)
			{
				ArmyUnits = new BatchArmyUnit();
			}
			ArmyUnits.MergeFrom(other.ArmyUnits);
		}
		if (other.ExpireTime != 0L)
		{
			ExpireTime = other.ExpireTime;
		}
		switch (other.ExtraCase)
		{
		case ExtraOneofCase.MummyPatrol:
			if (MummyPatrol == null)
			{
				MummyPatrol = new MummyPatrol();
			}
			MummyPatrol.MergeFrom(other.MummyPatrol);
			break;
		case ExtraOneofCase.MuseMummy:
			if (MuseMummy == null)
			{
				MuseMummy = new MuseMummy();
			}
			MuseMummy.MergeFrom(other.MuseMummy);
			break;
		case ExtraOneofCase.DarknessMonster:
			if (DarknessMonster == null)
			{
				DarknessMonster = new DarknessMonster();
			}
			DarknessMonster.MergeFrom(other.DarknessMonster);
			break;
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
				MonsterId = input.ReadInt32();
				break;
			case 18u:
				if (armyUnits_ == null)
				{
					ArmyUnits = new BatchArmyUnit();
				}
				input.ReadMessage(ArmyUnits);
				break;
			case 24u:
				ExpireTime = input.ReadInt64();
				break;
			case 810u:
			{
				MummyPatrol mummyPatrol = new MummyPatrol();
				if (extraCase_ == ExtraOneofCase.MummyPatrol)
				{
					mummyPatrol.MergeFrom(MummyPatrol);
				}
				input.ReadMessage(mummyPatrol);
				MummyPatrol = mummyPatrol;
				break;
			}
			case 818u:
			{
				MuseMummy museMummy = new MuseMummy();
				if (extraCase_ == ExtraOneofCase.MuseMummy)
				{
					museMummy.MergeFrom(MuseMummy);
				}
				input.ReadMessage(museMummy);
				MuseMummy = museMummy;
				break;
			}
			case 826u:
			{
				DarknessMonster darknessMonster = new DarknessMonster();
				if (extraCase_ == ExtraOneofCase.DarknessMonster)
				{
					darknessMonster.MergeFrom(DarknessMonster);
				}
				input.ReadMessage(darknessMonster);
				DarknessMonster = darknessMonster;
				break;
			}
			}
		}
	}
}
