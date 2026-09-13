using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DarknessMonster : IMessage<DarknessMonster>, IMessage, IEquatable<DarknessMonster>, IDeepCloneable<DarknessMonster>
{
	private static readonly MessageParser<DarknessMonster> _parser = new MessageParser<DarknessMonster>(() => new DarknessMonster());

	private UnknownFieldSet _unknownFields;

	public const int BornPointIdFieldNumber = 1;

	private int bornPointId_;

	public const int IsBloodNightFieldNumber = 2;

	private bool isBloodNight_;

	public const int LastAttackTimeFieldNumber = 3;

	private long lastAttackTime_;

	public const int WhistleInfoFieldNumber = 4;

	private WhistleInfo whistleInfo_;

	public const int CurHpFieldNumber = 5;

	private long curHp_;

	public const int MaxHpFieldNumber = 6;

	private long maxHp_;

	public const int WanderBossFieldNumber = 7;

	private WanderBoss wanderBoss_;

	public const int FloatInfoFieldNumber = 8;

	private FloatInfo floatInfo_;

	[DebuggerNonUserCode]
	public static MessageParser<DarknessMonster> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[39];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int BornPointId
	{
		get
		{
			return bornPointId_;
		}
		set
		{
			bornPointId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsBloodNight
	{
		get
		{
			return isBloodNight_;
		}
		set
		{
			isBloodNight_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long LastAttackTime
	{
		get
		{
			return lastAttackTime_;
		}
		set
		{
			lastAttackTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WhistleInfo WhistleInfo
	{
		get
		{
			return whistleInfo_;
		}
		set
		{
			whistleInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurHp
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
	public long MaxHp
	{
		get
		{
			return maxHp_;
		}
		set
		{
			maxHp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WanderBoss WanderBoss
	{
		get
		{
			return wanderBoss_;
		}
		set
		{
			wanderBoss_ = value;
		}
	}

	[DebuggerNonUserCode]
	public FloatInfo FloatInfo
	{
		get
		{
			return floatInfo_;
		}
		set
		{
			floatInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public DarknessMonster()
	{
	}

	[DebuggerNonUserCode]
	public DarknessMonster(DarknessMonster other)
		: this()
	{
		bornPointId_ = other.bornPointId_;
		isBloodNight_ = other.isBloodNight_;
		lastAttackTime_ = other.lastAttackTime_;
		whistleInfo_ = ((other.whistleInfo_ != null) ? other.whistleInfo_.Clone() : null);
		curHp_ = other.curHp_;
		maxHp_ = other.maxHp_;
		wanderBoss_ = ((other.wanderBoss_ != null) ? other.wanderBoss_.Clone() : null);
		floatInfo_ = ((other.floatInfo_ != null) ? other.floatInfo_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DarknessMonster Clone()
	{
		return new DarknessMonster(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DarknessMonster);
	}

	[DebuggerNonUserCode]
	public bool Equals(DarknessMonster other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BornPointId != other.BornPointId)
		{
			return false;
		}
		if (IsBloodNight != other.IsBloodNight)
		{
			return false;
		}
		if (LastAttackTime != other.LastAttackTime)
		{
			return false;
		}
		if (!object.Equals(WhistleInfo, other.WhistleInfo))
		{
			return false;
		}
		if (CurHp != other.CurHp)
		{
			return false;
		}
		if (MaxHp != other.MaxHp)
		{
			return false;
		}
		if (!object.Equals(WanderBoss, other.WanderBoss))
		{
			return false;
		}
		if (!object.Equals(FloatInfo, other.FloatInfo))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (BornPointId != 0)
		{
			num ^= BornPointId.GetHashCode();
		}
		if (IsBloodNight)
		{
			num ^= IsBloodNight.GetHashCode();
		}
		if (LastAttackTime != 0L)
		{
			num ^= LastAttackTime.GetHashCode();
		}
		if (whistleInfo_ != null)
		{
			num ^= WhistleInfo.GetHashCode();
		}
		if (CurHp != 0L)
		{
			num ^= CurHp.GetHashCode();
		}
		if (MaxHp != 0L)
		{
			num ^= MaxHp.GetHashCode();
		}
		if (wanderBoss_ != null)
		{
			num ^= WanderBoss.GetHashCode();
		}
		if (floatInfo_ != null)
		{
			num ^= FloatInfo.GetHashCode();
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
		if (BornPointId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(BornPointId);
		}
		if (IsBloodNight)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsBloodNight);
		}
		if (LastAttackTime != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(LastAttackTime);
		}
		if (whistleInfo_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(WhistleInfo);
		}
		if (CurHp != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(CurHp);
		}
		if (MaxHp != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(MaxHp);
		}
		if (wanderBoss_ != null)
		{
			output.WriteRawTag(58);
			output.WriteMessage(WanderBoss);
		}
		if (floatInfo_ != null)
		{
			output.WriteRawTag(66);
			output.WriteMessage(FloatInfo);
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
		if (BornPointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BornPointId);
		}
		if (IsBloodNight)
		{
			num += 2;
		}
		if (LastAttackTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(LastAttackTime);
		}
		if (whistleInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(WhistleInfo);
		}
		if (CurHp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurHp);
		}
		if (MaxHp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxHp);
		}
		if (wanderBoss_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(WanderBoss);
		}
		if (floatInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(FloatInfo);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DarknessMonster other)
	{
		if (other == null)
		{
			return;
		}
		if (other.BornPointId != 0)
		{
			BornPointId = other.BornPointId;
		}
		if (other.IsBloodNight)
		{
			IsBloodNight = other.IsBloodNight;
		}
		if (other.LastAttackTime != 0L)
		{
			LastAttackTime = other.LastAttackTime;
		}
		if (other.whistleInfo_ != null)
		{
			if (whistleInfo_ == null)
			{
				WhistleInfo = new WhistleInfo();
			}
			WhistleInfo.MergeFrom(other.WhistleInfo);
		}
		if (other.CurHp != 0L)
		{
			CurHp = other.CurHp;
		}
		if (other.MaxHp != 0L)
		{
			MaxHp = other.MaxHp;
		}
		if (other.wanderBoss_ != null)
		{
			if (wanderBoss_ == null)
			{
				WanderBoss = new WanderBoss();
			}
			WanderBoss.MergeFrom(other.WanderBoss);
		}
		if (other.floatInfo_ != null)
		{
			if (floatInfo_ == null)
			{
				FloatInfo = new FloatInfo();
			}
			FloatInfo.MergeFrom(other.FloatInfo);
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
				BornPointId = input.ReadInt32();
				break;
			case 16u:
				IsBloodNight = input.ReadBool();
				break;
			case 24u:
				LastAttackTime = input.ReadInt64();
				break;
			case 34u:
				if (whistleInfo_ == null)
				{
					WhistleInfo = new WhistleInfo();
				}
				input.ReadMessage(WhistleInfo);
				break;
			case 40u:
				CurHp = input.ReadInt64();
				break;
			case 48u:
				MaxHp = input.ReadInt64();
				break;
			case 58u:
				if (wanderBoss_ == null)
				{
					WanderBoss = new WanderBoss();
				}
				input.ReadMessage(WanderBoss);
				break;
			case 66u:
				if (floatInfo_ == null)
				{
					FloatInfo = new FloatInfo();
				}
				input.ReadMessage(FloatInfo);
				break;
			}
		}
	}
}
