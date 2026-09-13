using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class SimpleCombatUnit : IMessage<SimpleCombatUnit>, IMessage, IEquatable<SimpleCombatUnit>, IDeepCloneable<SimpleCombatUnit>
{
	private static readonly MessageParser<SimpleCombatUnit> _parser = new MessageParser<SimpleCombatUnit>(() => new SimpleCombatUnit());

	private UnknownFieldSet _unknownFields;

	public const int InitHealthFieldNumber = 1;

	private int initHealth_;

	public const int HealthFieldNumber = 2;

	private int health_;

	public const int UuidFieldNumber = 3;

	private long uuid_;

	public const int UidFieldNumber = 4;

	private string uid_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<SimpleCombatUnit> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[12];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int InitHealth
	{
		get
		{
			return initHealth_;
		}
		set
		{
			initHealth_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Health
	{
		get
		{
			return health_;
		}
		set
		{
			health_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Uuid
	{
		get
		{
			return uuid_;
		}
		set
		{
			uuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Uid
	{
		get
		{
			return uid_;
		}
		set
		{
			uid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public SimpleCombatUnit()
	{
	}

	[DebuggerNonUserCode]
	public SimpleCombatUnit(SimpleCombatUnit other)
		: this()
	{
		initHealth_ = other.initHealth_;
		health_ = other.health_;
		uuid_ = other.uuid_;
		uid_ = other.uid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public SimpleCombatUnit Clone()
	{
		return new SimpleCombatUnit(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as SimpleCombatUnit);
	}

	[DebuggerNonUserCode]
	public bool Equals(SimpleCombatUnit other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (InitHealth != other.InitHealth)
		{
			return false;
		}
		if (Health != other.Health)
		{
			return false;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (InitHealth != 0)
		{
			num ^= InitHealth.GetHashCode();
		}
		if (Health != 0)
		{
			num ^= Health.GetHashCode();
		}
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
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
		if (InitHealth != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(InitHealth);
		}
		if (Health != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Health);
		}
		if (Uuid != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(Uuid);
		}
		if (Uid.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(Uid);
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
		if (InitHealth != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(InitHealth);
		}
		if (Health != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Health);
		}
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(SimpleCombatUnit other)
	{
		if (other != null)
		{
			if (other.InitHealth != 0)
			{
				InitHealth = other.InitHealth;
			}
			if (other.Health != 0)
			{
				Health = other.Health;
			}
			if (other.Uuid != 0L)
			{
				Uuid = other.Uuid;
			}
			if (other.Uid.Length != 0)
			{
				Uid = other.Uid;
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
				InitHealth = input.ReadInt32();
				break;
			case 16u:
				Health = input.ReadInt32();
				break;
			case 24u:
				Uuid = input.ReadInt64();
				break;
			case 34u:
				Uid = input.ReadString();
				break;
			}
		}
	}
}
