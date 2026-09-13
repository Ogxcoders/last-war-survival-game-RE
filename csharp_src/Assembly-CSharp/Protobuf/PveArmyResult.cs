using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class PveArmyResult : IMessage<PveArmyResult>, IMessage, IEquatable<PveArmyResult>, IDeepCloneable<PveArmyResult>
{
	private static readonly MessageParser<PveArmyResult> _parser = new MessageParser<PveArmyResult>(() => new PveArmyResult());

	private UnknownFieldSet _unknownFields;

	public const int BaseFieldNumber = 1;

	private ArmyResultBase base_;

	public const int MonsterIdFieldNumber = 2;

	private int monsterId_;

	[DebuggerNonUserCode]
	public static MessageParser<PveArmyResult> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[20];

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
	public PveArmyResult()
	{
	}

	[DebuggerNonUserCode]
	public PveArmyResult(PveArmyResult other)
		: this()
	{
		base_ = ((other.base_ != null) ? other.base_.Clone() : null);
		monsterId_ = other.monsterId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PveArmyResult Clone()
	{
		return new PveArmyResult(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PveArmyResult);
	}

	[DebuggerNonUserCode]
	public bool Equals(PveArmyResult other)
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
	public void MergeFrom(PveArmyResult other)
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
