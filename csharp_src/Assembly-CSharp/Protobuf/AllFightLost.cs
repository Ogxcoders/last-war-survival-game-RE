using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllFightLost : IMessage<AllFightLost>, IMessage, IEquatable<AllFightLost>, IDeepCloneable<AllFightLost>
{
	private static readonly MessageParser<AllFightLost> _parser = new MessageParser<AllFightLost>(() => new AllFightLost());

	private UnknownFieldSet _unknownFields;

	public const int LostFieldNumber = 1;

	private FightLost lost_;

	public const int UuidFieldNumber = 2;

	private long uuid_;

	[DebuggerNonUserCode]
	public static MessageParser<AllFightLost> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[29];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public FightLost Lost
	{
		get
		{
			return lost_;
		}
		set
		{
			lost_ = value;
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
	public AllFightLost()
	{
	}

	[DebuggerNonUserCode]
	public AllFightLost(AllFightLost other)
		: this()
	{
		lost_ = ((other.lost_ != null) ? other.lost_.Clone() : null);
		uuid_ = other.uuid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllFightLost Clone()
	{
		return new AllFightLost(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllFightLost);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllFightLost other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(Lost, other.Lost))
		{
			return false;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (lost_ != null)
		{
			num ^= Lost.GetHashCode();
		}
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
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
		if (lost_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(Lost);
		}
		if (Uuid != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Uuid);
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
		if (lost_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Lost);
		}
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllFightLost other)
	{
		if (other == null)
		{
			return;
		}
		if (other.lost_ != null)
		{
			if (lost_ == null)
			{
				Lost = new FightLost();
			}
			Lost.MergeFrom(other.Lost);
		}
		if (other.Uuid != 0L)
		{
			Uuid = other.Uuid;
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
				if (lost_ == null)
				{
					Lost = new FightLost();
				}
				input.ReadMessage(Lost);
				break;
			case 16u:
				Uuid = input.ReadInt64();
				break;
			}
		}
	}
}
