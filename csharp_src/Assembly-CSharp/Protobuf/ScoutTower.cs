using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutTower : IMessage<ScoutTower>, IMessage, IEquatable<ScoutTower>, IDeepCloneable<ScoutTower>
{
	private static readonly MessageParser<ScoutTower> _parser = new MessageParser<ScoutTower>(() => new ScoutTower());

	private UnknownFieldSet _unknownFields;

	public const int TowerFieldNumber = 1;

	private static readonly FieldCodec<ScoutTowerUnit> _repeated_tower_codec = FieldCodec.ForMessage(10u, ScoutTowerUnit.Parser);

	private readonly RepeatedField<ScoutTowerUnit> tower_ = new RepeatedField<ScoutTowerUnit>();

	public const int VisibleFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_visible_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? visible_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutTower> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[22];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<ScoutTowerUnit> Tower => tower_;

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
	public ScoutTower()
	{
	}

	[DebuggerNonUserCode]
	public ScoutTower(ScoutTower other)
		: this()
	{
		tower_ = other.tower_.Clone();
		Visible = other.Visible;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutTower Clone()
	{
		return new ScoutTower(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutTower);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutTower other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!tower_.Equals(other.tower_))
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
		num ^= tower_.GetHashCode();
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
		tower_.WriteTo(output, _repeated_tower_codec);
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
		num += tower_.CalculateSize(_repeated_tower_codec);
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
	public void MergeFrom(ScoutTower other)
	{
		if (other != null)
		{
			tower_.Add(other.tower_);
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
				tower_.AddEntriesFrom(input, _repeated_tower_codec);
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
