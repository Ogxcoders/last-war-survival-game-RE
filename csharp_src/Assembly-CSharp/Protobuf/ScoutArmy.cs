using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutArmy : IMessage<ScoutArmy>, IMessage, IEquatable<ScoutArmy>, IDeepCloneable<ScoutArmy>
{
	private static readonly MessageParser<ScoutArmy> _parser = new MessageParser<ScoutArmy>(() => new ScoutArmy());

	private UnknownFieldSet _unknownFields;

	public const int TargetFieldNumber = 1;

	private ScoutTargetArmyUnit target_;

	public const int HelpFieldNumber = 2;

	private ScoutHelpArmy help_;

	public const int TargetTotalFieldNumber = 3;

	private ScoutTargetArmyTotal targetTotal_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutArmy> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[11];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public ScoutTargetArmyUnit Target
	{
		get
		{
			return target_;
		}
		set
		{
			target_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutHelpArmy Help
	{
		get
		{
			return help_;
		}
		set
		{
			help_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutTargetArmyTotal TargetTotal
	{
		get
		{
			return targetTotal_;
		}
		set
		{
			targetTotal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutArmy()
	{
	}

	[DebuggerNonUserCode]
	public ScoutArmy(ScoutArmy other)
		: this()
	{
		target_ = ((other.target_ != null) ? other.target_.Clone() : null);
		help_ = ((other.help_ != null) ? other.help_.Clone() : null);
		targetTotal_ = ((other.targetTotal_ != null) ? other.targetTotal_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutArmy Clone()
	{
		return new ScoutArmy(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutArmy);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutArmy other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(Target, other.Target))
		{
			return false;
		}
		if (!object.Equals(Help, other.Help))
		{
			return false;
		}
		if (!object.Equals(TargetTotal, other.TargetTotal))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (target_ != null)
		{
			num ^= Target.GetHashCode();
		}
		if (help_ != null)
		{
			num ^= Help.GetHashCode();
		}
		if (targetTotal_ != null)
		{
			num ^= TargetTotal.GetHashCode();
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
		if (target_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(Target);
		}
		if (help_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(Help);
		}
		if (targetTotal_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(TargetTotal);
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
		if (target_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Target);
		}
		if (help_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Help);
		}
		if (targetTotal_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(TargetTotal);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutArmy other)
	{
		if (other == null)
		{
			return;
		}
		if (other.target_ != null)
		{
			if (target_ == null)
			{
				Target = new ScoutTargetArmyUnit();
			}
			Target.MergeFrom(other.Target);
		}
		if (other.help_ != null)
		{
			if (help_ == null)
			{
				Help = new ScoutHelpArmy();
			}
			Help.MergeFrom(other.Help);
		}
		if (other.targetTotal_ != null)
		{
			if (targetTotal_ == null)
			{
				TargetTotal = new ScoutTargetArmyTotal();
			}
			TargetTotal.MergeFrom(other.TargetTotal);
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
				if (target_ == null)
				{
					Target = new ScoutTargetArmyUnit();
				}
				input.ReadMessage(Target);
				break;
			case 18u:
				if (help_ == null)
				{
					Help = new ScoutHelpArmy();
				}
				input.ReadMessage(Help);
				break;
			case 26u:
				if (targetTotal_ == null)
				{
					TargetTotal = new ScoutTargetArmyTotal();
				}
				input.ReadMessage(TargetTotal);
				break;
			}
		}
	}
}
