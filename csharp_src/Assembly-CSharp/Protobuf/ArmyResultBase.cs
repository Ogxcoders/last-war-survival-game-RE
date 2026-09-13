using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ArmyResultBase : IMessage<ArmyResultBase>, IMessage, IEquatable<ArmyResultBase>, IDeepCloneable<ArmyResultBase>
{
	private static readonly MessageParser<ArmyResultBase> _parser = new MessageParser<ArmyResultBase>(() => new ArmyResultBase());

	private UnknownFieldSet _unknownFields;

	public const int PointIdFieldNumber = 1;

	private int pointId_;

	public const int ArmyObjFieldNumber = 2;

	private CombatUnit armyObj_;

	public const int AfterArmyObjFieldNumber = 3;

	private CombatUnit afterArmyObj_;

	public const int DestroyValueFieldNumber = 4;

	private static readonly FieldCodec<int> _repeated_destroyValue_codec = FieldCodec.ForInt32(34u);

	private readonly RepeatedField<int> destroyValue_ = new RepeatedField<int>();

	public const int DamagePercentFieldNumber = 5;

	private static readonly FieldCodec<DamagePercentInfo> _repeated_damagePercent_codec = FieldCodec.ForMessage(42u, DamagePercentInfo.Parser);

	private readonly RepeatedField<DamagePercentInfo> damagePercent_ = new RepeatedField<DamagePercentInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<ArmyResultBase> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[15];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int PointId
	{
		get
		{
			return pointId_;
		}
		set
		{
			pointId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CombatUnit ArmyObj
	{
		get
		{
			return armyObj_;
		}
		set
		{
			armyObj_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CombatUnit AfterArmyObj
	{
		get
		{
			return afterArmyObj_;
		}
		set
		{
			afterArmyObj_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<int> DestroyValue => destroyValue_;

	[DebuggerNonUserCode]
	public RepeatedField<DamagePercentInfo> DamagePercent => damagePercent_;

	[DebuggerNonUserCode]
	public ArmyResultBase()
	{
	}

	[DebuggerNonUserCode]
	public ArmyResultBase(ArmyResultBase other)
		: this()
	{
		pointId_ = other.pointId_;
		armyObj_ = ((other.armyObj_ != null) ? other.armyObj_.Clone() : null);
		afterArmyObj_ = ((other.afterArmyObj_ != null) ? other.afterArmyObj_.Clone() : null);
		destroyValue_ = other.destroyValue_.Clone();
		damagePercent_ = other.damagePercent_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ArmyResultBase Clone()
	{
		return new ArmyResultBase(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ArmyResultBase);
	}

	[DebuggerNonUserCode]
	public bool Equals(ArmyResultBase other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (PointId != other.PointId)
		{
			return false;
		}
		if (!object.Equals(ArmyObj, other.ArmyObj))
		{
			return false;
		}
		if (!object.Equals(AfterArmyObj, other.AfterArmyObj))
		{
			return false;
		}
		if (!destroyValue_.Equals(other.destroyValue_))
		{
			return false;
		}
		if (!damagePercent_.Equals(other.damagePercent_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (PointId != 0)
		{
			num ^= PointId.GetHashCode();
		}
		if (armyObj_ != null)
		{
			num ^= ArmyObj.GetHashCode();
		}
		if (afterArmyObj_ != null)
		{
			num ^= AfterArmyObj.GetHashCode();
		}
		num ^= destroyValue_.GetHashCode();
		num ^= damagePercent_.GetHashCode();
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
		if (PointId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(PointId);
		}
		if (armyObj_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(ArmyObj);
		}
		if (afterArmyObj_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(AfterArmyObj);
		}
		destroyValue_.WriteTo(output, _repeated_destroyValue_codec);
		damagePercent_.WriteTo(output, _repeated_damagePercent_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (PointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PointId);
		}
		if (armyObj_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ArmyObj);
		}
		if (afterArmyObj_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(AfterArmyObj);
		}
		num += destroyValue_.CalculateSize(_repeated_destroyValue_codec);
		num += damagePercent_.CalculateSize(_repeated_damagePercent_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ArmyResultBase other)
	{
		if (other == null)
		{
			return;
		}
		if (other.PointId != 0)
		{
			PointId = other.PointId;
		}
		if (other.armyObj_ != null)
		{
			if (armyObj_ == null)
			{
				ArmyObj = new CombatUnit();
			}
			ArmyObj.MergeFrom(other.ArmyObj);
		}
		if (other.afterArmyObj_ != null)
		{
			if (afterArmyObj_ == null)
			{
				AfterArmyObj = new CombatUnit();
			}
			AfterArmyObj.MergeFrom(other.AfterArmyObj);
		}
		destroyValue_.Add(other.destroyValue_);
		damagePercent_.Add(other.damagePercent_);
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
				PointId = input.ReadInt32();
				break;
			case 18u:
				if (armyObj_ == null)
				{
					ArmyObj = new CombatUnit();
				}
				input.ReadMessage(ArmyObj);
				break;
			case 26u:
				if (afterArmyObj_ == null)
				{
					AfterArmyObj = new CombatUnit();
				}
				input.ReadMessage(AfterArmyObj);
				break;
			case 32u:
			case 34u:
				destroyValue_.AddEntriesFrom(input, _repeated_destroyValue_codec);
				break;
			case 42u:
				damagePercent_.AddEntriesFrom(input, _repeated_damagePercent_codec);
				break;
			}
		}
	}
}
