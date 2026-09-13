using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class SimpleCombatUnitPushObj : IMessage<SimpleCombatUnitPushObj>, IMessage, IEquatable<SimpleCombatUnitPushObj>, IDeepCloneable<SimpleCombatUnitPushObj>
{
	private static readonly MessageParser<SimpleCombatUnitPushObj> _parser = new MessageParser<SimpleCombatUnitPushObj>(() => new SimpleCombatUnitPushObj());

	private UnknownFieldSet _unknownFields;

	public const int ArmyInfoFieldNumber = 1;

	private SimpleCombatUnit armyInfo_;

	public const int TypeFieldNumber = 2;

	private int type_;

	public const int TopUuidFieldNumber = 3;

	private long topUuid_;

	[DebuggerNonUserCode]
	public static MessageParser<SimpleCombatUnitPushObj> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleRoundPushReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public SimpleCombatUnit ArmyInfo
	{
		get
		{
			return armyInfo_;
		}
		set
		{
			armyInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long TopUuid
	{
		get
		{
			return topUuid_;
		}
		set
		{
			topUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public SimpleCombatUnitPushObj()
	{
	}

	[DebuggerNonUserCode]
	public SimpleCombatUnitPushObj(SimpleCombatUnitPushObj other)
		: this()
	{
		armyInfo_ = ((other.armyInfo_ != null) ? other.armyInfo_.Clone() : null);
		type_ = other.type_;
		topUuid_ = other.topUuid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SimpleCombatUnitPushObj Clone()
	{
		return new SimpleCombatUnitPushObj(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SimpleCombatUnitPushObj);
	}

	[DebuggerNonUserCode]
	public bool Equals(SimpleCombatUnitPushObj other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(ArmyInfo, other.ArmyInfo))
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (TopUuid != other.TopUuid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (armyInfo_ != null)
		{
			num ^= ArmyInfo.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (TopUuid != 0L)
		{
			num ^= TopUuid.GetHashCode();
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
		if (armyInfo_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(ArmyInfo);
		}
		if (Type != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Type);
		}
		if (TopUuid != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(TopUuid);
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
		if (armyInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ArmyInfo);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (TopUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TopUuid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SimpleCombatUnitPushObj other)
	{
		if (other == null)
		{
			return;
		}
		if (other.armyInfo_ != null)
		{
			if (armyInfo_ == null)
			{
				ArmyInfo = new SimpleCombatUnit();
			}
			ArmyInfo.MergeFrom(other.ArmyInfo);
		}
		if (other.Type != 0)
		{
			Type = other.Type;
		}
		if (other.TopUuid != 0L)
		{
			TopUuid = other.TopUuid;
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
				if (armyInfo_ == null)
				{
					ArmyInfo = new SimpleCombatUnit();
				}
				input.ReadMessage(ArmyInfo);
				break;
			case 16u:
				Type = input.ReadInt32();
				break;
			case 24u:
				TopUuid = input.ReadInt64();
				break;
			}
		}
	}
}
