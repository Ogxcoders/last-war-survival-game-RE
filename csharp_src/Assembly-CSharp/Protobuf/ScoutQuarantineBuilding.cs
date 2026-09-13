using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutQuarantineBuilding : IMessage<ScoutQuarantineBuilding>, IMessage, IEquatable<ScoutQuarantineBuilding>, IDeepCloneable<ScoutQuarantineBuilding>
{
	private static readonly MessageParser<ScoutQuarantineBuilding> _parser = new MessageParser<ScoutQuarantineBuilding>(() => new ScoutQuarantineBuilding());

	private UnknownFieldSet _unknownFields;

	public const int BuildIdFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_buildId_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? buildId_;

	public const int PointFieldNumber = 2;

	private PointInfo point_;

	public const int WorldTypeFieldNumber = 3;

	private int worldType_;

	public const int BattleConfigIdFieldNumber = 4;

	private int battleConfigId_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutQuarantineBuilding> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[28];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? BuildId
	{
		get
		{
			return buildId_;
		}
		set
		{
			buildId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public PointInfo Point
	{
		get
		{
			return point_;
		}
		set
		{
			point_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WorldType
	{
		get
		{
			return worldType_;
		}
		set
		{
			worldType_ = value;
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
	public ScoutQuarantineBuilding()
	{
	}

	[DebuggerNonUserCode]
	public ScoutQuarantineBuilding(ScoutQuarantineBuilding other)
		: this()
	{
		BuildId = other.BuildId;
		point_ = ((other.point_ != null) ? other.point_.Clone() : null);
		worldType_ = other.worldType_;
		battleConfigId_ = other.battleConfigId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutQuarantineBuilding Clone()
	{
		return new ScoutQuarantineBuilding(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutQuarantineBuilding);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutQuarantineBuilding other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BuildId != other.BuildId)
		{
			return false;
		}
		if (!object.Equals(Point, other.Point))
		{
			return false;
		}
		if (WorldType != other.WorldType)
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
		if (buildId_.HasValue)
		{
			num ^= BuildId.GetHashCode();
		}
		if (point_ != null)
		{
			num ^= Point.GetHashCode();
		}
		if (WorldType != 0)
		{
			num ^= WorldType.GetHashCode();
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
		if (buildId_.HasValue)
		{
			_single_buildId_codec.WriteTagAndValue(output, BuildId);
		}
		if (point_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(Point);
		}
		if (WorldType != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(WorldType);
		}
		if (BattleConfigId != 0)
		{
			output.WriteRawTag(32);
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
		if (buildId_.HasValue)
		{
			num += _single_buildId_codec.CalculateSizeWithTag(BuildId);
		}
		if (point_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Point);
		}
		if (WorldType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WorldType);
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
	public void MergeFrom(ScoutQuarantineBuilding other)
	{
		if (other == null)
		{
			return;
		}
		if (other.buildId_.HasValue && (!buildId_.HasValue || other.BuildId != 0))
		{
			BuildId = other.BuildId;
		}
		if (other.point_ != null)
		{
			if (point_ == null)
			{
				Point = new PointInfo();
			}
			Point.MergeFrom(other.Point);
		}
		if (other.WorldType != 0)
		{
			WorldType = other.WorldType;
		}
		if (other.BattleConfigId != 0)
		{
			BattleConfigId = other.BattleConfigId;
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
			{
				int? num2 = _single_buildId_codec.Read(input);
				if (!buildId_.HasValue || num2 != 0)
				{
					BuildId = num2;
				}
				break;
			}
			case 18u:
				if (point_ == null)
				{
					Point = new PointInfo();
				}
				input.ReadMessage(Point);
				break;
			case 24u:
				WorldType = input.ReadInt32();
				break;
			case 32u:
				BattleConfigId = input.ReadInt32();
				break;
			}
		}
	}
}
