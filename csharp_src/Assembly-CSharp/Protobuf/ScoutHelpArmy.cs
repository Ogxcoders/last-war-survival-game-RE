using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutHelpArmy : IMessage<ScoutHelpArmy>, IMessage, IEquatable<ScoutHelpArmy>, IDeepCloneable<ScoutHelpArmy>
{
	private static readonly MessageParser<ScoutHelpArmy> _parser = new MessageParser<ScoutHelpArmy>(() => new ScoutHelpArmy());

	private UnknownFieldSet _unknownFields;

	public const int FormationFieldNumber = 1;

	private static readonly FieldCodec<ScoutHelpArmyUnit> _repeated_formation_codec = FieldCodec.ForMessage(10u, ScoutHelpArmyUnit.Parser);

	private readonly RepeatedField<ScoutHelpArmyUnit> formation_ = new RepeatedField<ScoutHelpArmyUnit>();

	public const int VisibleFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_visible_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? visible_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutHelpArmy> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[14];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<ScoutHelpArmyUnit> Formation => formation_;

	[DebuggerNonUserCode]
	public int? Visible
	{
		get
		{
			return visible_;
		}
		set
		{
			visible_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutHelpArmy()
	{
	}

	[DebuggerNonUserCode]
	public ScoutHelpArmy(ScoutHelpArmy other)
		: this()
	{
		formation_ = other.formation_.Clone();
		Visible = other.Visible;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutHelpArmy Clone()
	{
		return new ScoutHelpArmy(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutHelpArmy);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutHelpArmy other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!formation_.Equals(other.formation_))
		{
			return false;
		}
		if (Visible != other.Visible)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= formation_.GetHashCode();
		if (visible_.HasValue)
		{
			num ^= Visible.GetHashCode();
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
		formation_.WriteTo(output, _repeated_formation_codec);
		if (visible_.HasValue)
		{
			_single_visible_codec.WriteTagAndValue(output, Visible);
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
		num += formation_.CalculateSize(_repeated_formation_codec);
		if (visible_.HasValue)
		{
			num += _single_visible_codec.CalculateSizeWithTag(Visible);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutHelpArmy other)
	{
		if (other != null)
		{
			formation_.Add(other.formation_);
			if (other.visible_.HasValue && (!visible_.HasValue || other.Visible != 0))
			{
				Visible = other.Visible;
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
			case 10u:
				formation_.AddEntriesFrom(input, _repeated_formation_codec);
				break;
			case 18u:
			{
				int? num2 = _single_visible_codec.Read(input);
				if (!visible_.HasValue || num2 != 0)
				{
					Visible = num2;
				}
				break;
			}
			}
		}
	}
}
