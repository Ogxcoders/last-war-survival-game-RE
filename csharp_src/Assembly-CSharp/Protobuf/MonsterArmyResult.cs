using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MonsterArmyResult : IMessage<MonsterArmyResult>, IMessage, IEquatable<MonsterArmyResult>, IDeepCloneable<MonsterArmyResult>
{
	private static readonly MessageParser<MonsterArmyResult> _parser = new MessageParser<MonsterArmyResult>(() => new MonsterArmyResult());

	private UnknownFieldSet _unknownFields;

	public const int BaseFieldNumber = 1;

	private ArmyResultBase base_;

	public const int MonsterIdFieldNumber = 2;

	private int monsterId_;

	[DebuggerNonUserCode]
	public static MessageParser<MonsterArmyResult> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[18];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public ArmyResultBase Base
	{
		get
		{
			return base_;
		}
		set
		{
			base_ = value;
		}
	}

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
	public MonsterArmyResult()
	{
	}

	[DebuggerNonUserCode]
	public MonsterArmyResult(MonsterArmyResult other)
		: this()
	{
		base_ = ((other.base_ != null) ? other.base_.Clone() : null);
		monsterId_ = other.monsterId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MonsterArmyResult Clone()
	{
		return new MonsterArmyResult(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MonsterArmyResult);
	}

	[DebuggerNonUserCode]
	public bool Equals(MonsterArmyResult other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(Base, other.Base))
		{
			return false;
		}
		if (MonsterId != other.MonsterId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (base_ != null)
		{
			num ^= Base.GetHashCode();
		}
		if (MonsterId != 0)
		{
			num ^= MonsterId.GetHashCode();
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
		if (base_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(Base);
		}
		if (MonsterId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(MonsterId);
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
		if (base_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Base);
		}
		if (MonsterId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MonsterId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MonsterArmyResult other)
	{
		if (other == null)
		{
			return;
		}
		if (other.base_ != null)
		{
			if (base_ == null)
			{
				Base = new ArmyResultBase();
			}
			Base.MergeFrom(other.Base);
		}
		if (other.MonsterId != 0)
		{
			MonsterId = other.MonsterId;
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
				if (base_ == null)
				{
					Base = new ArmyResultBase();
				}
				input.ReadMessage(Base);
				break;
			case 16u:
				MonsterId = input.ReadInt32();
				break;
			}
		}
	}
}
