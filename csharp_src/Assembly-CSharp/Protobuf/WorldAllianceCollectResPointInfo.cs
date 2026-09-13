using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WorldAllianceCollectResPointInfo : IMessage<WorldAllianceCollectResPointInfo>, IMessage, IEquatable<WorldAllianceCollectResPointInfo>, IDeepCloneable<WorldAllianceCollectResPointInfo>
{
	private static readonly MessageParser<WorldAllianceCollectResPointInfo> _parser = new MessageParser<WorldAllianceCollectResPointInfo>(() => new WorldAllianceCollectResPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int UuidFieldNumber = 1;

	private long uuid_;

	public const int BuildIdFieldNumber = 2;

	private int buildId_;

	public const int AllianceIdFieldNumber = 3;

	private string allianceId_ = "";

	public const int AllianceAbbrFieldNumber = 4;

	private string allianceAbbr_ = "";

	public const int StateFieldNumber = 5;

	private int state_;

	[DebuggerNonUserCode]
	public static MessageParser<WorldAllianceCollectResPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[52];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public int BuildId
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
	public string AllianceId
	{
		get
		{
			return allianceId_;
		}
		set
		{
			allianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AllianceAbbr
	{
		get
		{
			return allianceAbbr_;
		}
		set
		{
			allianceAbbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int State
	{
		get
		{
			return state_;
		}
		set
		{
			state_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WorldAllianceCollectResPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public WorldAllianceCollectResPointInfo(WorldAllianceCollectResPointInfo other)
		: this()
	{
		uuid_ = other.uuid_;
		buildId_ = other.buildId_;
		allianceId_ = other.allianceId_;
		allianceAbbr_ = other.allianceAbbr_;
		state_ = other.state_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WorldAllianceCollectResPointInfo Clone()
	{
		return new WorldAllianceCollectResPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WorldAllianceCollectResPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(WorldAllianceCollectResPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (BuildId != other.BuildId)
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (AllianceAbbr != other.AllianceAbbr)
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (BuildId != 0)
		{
			num ^= BuildId.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (AllianceAbbr.Length != 0)
		{
			num ^= AllianceAbbr.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
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
		if (Uuid != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(Uuid);
		}
		if (BuildId != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(BuildId);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(AllianceId);
		}
		if (AllianceAbbr.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(AllianceAbbr);
		}
		if (State != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(State);
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
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (BuildId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildId);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (AllianceAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceAbbr);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WorldAllianceCollectResPointInfo other)
	{
		if (other != null)
		{
			if (other.Uuid != 0L)
			{
				Uuid = other.Uuid;
			}
			if (other.BuildId != 0)
			{
				BuildId = other.BuildId;
			}
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.AllianceAbbr.Length != 0)
			{
				AllianceAbbr = other.AllianceAbbr;
			}
			if (other.State != 0)
			{
				State = other.State;
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
				Uuid = input.ReadInt64();
				break;
			case 16u:
				BuildId = input.ReadInt32();
				break;
			case 26u:
				AllianceId = input.ReadString();
				break;
			case 34u:
				AllianceAbbr = input.ReadString();
				break;
			case 40u:
				State = input.ReadInt32();
				break;
			}
		}
	}
}
