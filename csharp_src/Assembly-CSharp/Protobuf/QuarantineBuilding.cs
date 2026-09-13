using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class QuarantineBuilding : IMessage<QuarantineBuilding>, IMessage, IEquatable<QuarantineBuilding>, IDeepCloneable<QuarantineBuilding>
{
	private static readonly MessageParser<QuarantineBuilding> _parser = new MessageParser<QuarantineBuilding>(() => new QuarantineBuilding());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private int id_;

	public const int BattleConfigIdFieldNumber = 2;

	private int battleConfigId_;

	[DebuggerNonUserCode]
	public static MessageParser<QuarantineBuilding> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[26];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BattleConfigId
	{
		get
		{
			return battleConfigId_;
		}
		set
		{
			battleConfigId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public QuarantineBuilding()
	{
	}

	[DebuggerNonUserCode]
	public QuarantineBuilding(QuarantineBuilding other)
		: this()
	{
		id_ = other.id_;
		battleConfigId_ = other.battleConfigId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public QuarantineBuilding Clone()
	{
		return new QuarantineBuilding(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as QuarantineBuilding);
	}

	[DebuggerNonUserCode]
	public bool Equals(QuarantineBuilding other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (BattleConfigId != other.BattleConfigId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Id != 0)
		{
			num ^= Id.GetHashCode();
		}
		if (BattleConfigId != 0)
		{
			num ^= BattleConfigId.GetHashCode();
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
		if (Id != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Id);
		}
		if (BattleConfigId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(BattleConfigId);
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
		if (Id != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Id);
		}
		if (BattleConfigId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BattleConfigId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(QuarantineBuilding other)
	{
		if (other != null)
		{
			if (other.Id != 0)
			{
				Id = other.Id;
			}
			if (other.BattleConfigId != 0)
			{
				BattleConfigId = other.BattleConfigId;
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
				Id = input.ReadInt32();
				break;
			case 16u:
				BattleConfigId = input.ReadInt32();
				break;
			}
		}
	}
}
