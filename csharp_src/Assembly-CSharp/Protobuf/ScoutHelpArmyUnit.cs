using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutHelpArmyUnit : IMessage<ScoutHelpArmyUnit>, IMessage, IEquatable<ScoutHelpArmyUnit>, IDeepCloneable<ScoutHelpArmyUnit>
{
	private static readonly MessageParser<ScoutHelpArmyUnit> _parser = new MessageParser<ScoutHelpArmyUnit>(() => new ScoutHelpArmyUnit());

	private UnknownFieldSet _unknownFields;

	public const int FormationFieldNumber = 1;

	private ScoutFormation formation_;

	public const int UserFieldNumber = 2;

	private User user_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutHelpArmyUnit> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[15];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public ScoutFormation Formation
	{
		get
		{
			return formation_;
		}
		set
		{
			formation_ = value;
		}
	}

	[DebuggerNonUserCode]
	public User User
	{
		get
		{
			return user_;
		}
		set
		{
			user_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutHelpArmyUnit()
	{
	}

	[DebuggerNonUserCode]
	public ScoutHelpArmyUnit(ScoutHelpArmyUnit other)
		: this()
	{
		formation_ = ((other.formation_ != null) ? other.formation_.Clone() : null);
		user_ = ((other.user_ != null) ? other.user_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutHelpArmyUnit Clone()
	{
		return new ScoutHelpArmyUnit(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutHelpArmyUnit);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutHelpArmyUnit other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(Formation, other.Formation))
		{
			return false;
		}
		if (!object.Equals(User, other.User))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (formation_ != null)
		{
			num ^= Formation.GetHashCode();
		}
		if (user_ != null)
		{
			num ^= User.GetHashCode();
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
		if (formation_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(Formation);
		}
		if (user_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(User);
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
		if (formation_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Formation);
		}
		if (user_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(User);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutHelpArmyUnit other)
	{
		if (other == null)
		{
			return;
		}
		if (other.formation_ != null)
		{
			if (formation_ == null)
			{
				Formation = new ScoutFormation();
			}
			Formation.MergeFrom(other.Formation);
		}
		if (other.user_ != null)
		{
			if (user_ == null)
			{
				User = new User();
			}
			User.MergeFrom(other.User);
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
				if (formation_ == null)
				{
					Formation = new ScoutFormation();
				}
				input.ReadMessage(Formation);
				break;
			case 18u:
				if (user_ == null)
				{
					User = new User();
				}
				input.ReadMessage(User);
				break;
			}
		}
	}
}
