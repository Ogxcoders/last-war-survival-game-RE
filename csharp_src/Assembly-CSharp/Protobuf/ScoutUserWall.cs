using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutUserWall : IMessage<ScoutUserWall>, IMessage, IEquatable<ScoutUserWall>, IDeepCloneable<ScoutUserWall>
{
	private static readonly MessageParser<ScoutUserWall> _parser = new MessageParser<ScoutUserWall>(() => new ScoutUserWall());

	private UnknownFieldSet _unknownFields;

	public const int HpFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_hp_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? hp_;

	public const int HpMaxFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_hpMax_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? hpMax_;

	public const int VisibleFieldNumber = 3;

	private static readonly FieldCodec<int?> _single_visible_codec = FieldCodec.ForStructWrapper<int>(26u);

	private int? visible_;

	public const int BuildingIdFieldNumber = 4;

	private string buildingId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ScoutUserWall> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? Hp
	{
		get
		{
			return hp_;
		}
		set
		{
			hp_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? HpMax
	{
		get
		{
			return hpMax_;
		}
		set
		{
			hpMax_ = value;
		}
	}

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
	public string BuildingId
	{
		get
		{
			return buildingId_;
		}
		set
		{
			buildingId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public ScoutUserWall()
	{
	}

	[DebuggerNonUserCode]
	public ScoutUserWall(ScoutUserWall other)
		: this()
	{
		Hp = other.Hp;
		HpMax = other.HpMax;
		Visible = other.Visible;
		buildingId_ = other.buildingId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutUserWall Clone()
	{
		return new ScoutUserWall(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutUserWall);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutUserWall other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Hp != other.Hp)
		{
			return false;
		}
		if (HpMax != other.HpMax)
		{
			return false;
		}
		if (Visible != other.Visible)
		{
			return false;
		}
		if (BuildingId != other.BuildingId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (hp_.HasValue)
		{
			num ^= Hp.GetHashCode();
		}
		if (hpMax_.HasValue)
		{
			num ^= HpMax.GetHashCode();
		}
		if (visible_.HasValue)
		{
			num ^= Visible.GetHashCode();
		}
		if (BuildingId.Length != 0)
		{
			num ^= BuildingId.GetHashCode();
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
		if (hp_.HasValue)
		{
			_single_hp_codec.WriteTagAndValue(output, Hp);
		}
		if (hpMax_.HasValue)
		{
			_single_hpMax_codec.WriteTagAndValue(output, HpMax);
		}
		if (visible_.HasValue)
		{
			_single_visible_codec.WriteTagAndValue(output, Visible);
		}
		if (BuildingId.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(BuildingId);
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
		if (hp_.HasValue)
		{
			num += _single_hp_codec.CalculateSizeWithTag(Hp);
		}
		if (hpMax_.HasValue)
		{
			num += _single_hpMax_codec.CalculateSizeWithTag(HpMax);
		}
		if (visible_.HasValue)
		{
			num += _single_visible_codec.CalculateSizeWithTag(Visible);
		}
		if (BuildingId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(BuildingId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutUserWall other)
	{
		if (other != null)
		{
			if (other.hp_.HasValue && (!hp_.HasValue || other.Hp != 0))
			{
				Hp = other.Hp;
			}
			if (other.hpMax_.HasValue && (!hpMax_.HasValue || other.HpMax != 0))
			{
				HpMax = other.HpMax;
			}
			if (other.visible_.HasValue && (!visible_.HasValue || other.Visible != 0))
			{
				Visible = other.Visible;
			}
			if (other.BuildingId.Length != 0)
			{
				BuildingId = other.BuildingId;
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
			{
				int? num3 = _single_hp_codec.Read(input);
				if (!hp_.HasValue || num3 != 0)
				{
					Hp = num3;
				}
				break;
			}
			case 18u:
			{
				int? num4 = _single_hpMax_codec.Read(input);
				if (!hpMax_.HasValue || num4 != 0)
				{
					HpMax = num4;
				}
				break;
			}
			case 26u:
			{
				int? num2 = _single_visible_codec.Read(input);
				if (!visible_.HasValue || num2 != 0)
				{
					Visible = num2;
				}
				break;
			}
			case 34u:
				BuildingId = input.ReadString();
				break;
			}
		}
	}
}
