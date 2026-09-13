using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BattlePointInfo : IMessage<BattlePointInfo>, IMessage, IEquatable<BattlePointInfo>, IDeepCloneable<BattlePointInfo>
{
	private static readonly MessageParser<BattlePointInfo> _parser = new MessageParser<BattlePointInfo>(() => new BattlePointInfo());

	private UnknownFieldSet _unknownFields;

	public const int BattleServerIdFieldNumber = 1;

	private int battleServerId_;

	public const int ServerTypeFieldNumber = 2;

	private int serverType_;

	public const int PointIdFieldNumber = 3;

	private int pointId_;

	public const int WorldIdFieldNumber = 4;

	private int worldId_;

	[DebuggerNonUserCode]
	public static MessageParser<BattlePointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[25];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int BattleServerId
	{
		get
		{
			return battleServerId_;
		}
		set
		{
			battleServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ServerType
	{
		get
		{
			return serverType_;
		}
		set
		{
			serverType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PointId
	{
		get
		{
			return pointId_;
		}
		set
		{
			pointId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WorldId
	{
		get
		{
			return worldId_;
		}
		set
		{
			worldId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BattlePointInfo()
	{
	}

	[DebuggerNonUserCode]
	public BattlePointInfo(BattlePointInfo other)
		: this()
	{
		battleServerId_ = other.battleServerId_;
		serverType_ = other.serverType_;
		pointId_ = other.pointId_;
		worldId_ = other.worldId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattlePointInfo Clone()
	{
		return new BattlePointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattlePointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattlePointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BattleServerId != other.BattleServerId)
		{
			return false;
		}
		if (ServerType != other.ServerType)
		{
			return false;
		}
		if (PointId != other.PointId)
		{
			return false;
		}
		if (WorldId != other.WorldId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (BattleServerId != 0)
		{
			num ^= BattleServerId.GetHashCode();
		}
		if (ServerType != 0)
		{
			num ^= ServerType.GetHashCode();
		}
		if (PointId != 0)
		{
			num ^= PointId.GetHashCode();
		}
		if (WorldId != 0)
		{
			num ^= WorldId.GetHashCode();
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
		if (BattleServerId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(BattleServerId);
		}
		if (ServerType != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(ServerType);
		}
		if (PointId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(PointId);
		}
		if (WorldId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(WorldId);
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
		if (BattleServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BattleServerId);
		}
		if (ServerType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ServerType);
		}
		if (PointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PointId);
		}
		if (WorldId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WorldId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattlePointInfo other)
	{
		if (other != null)
		{
			if (other.BattleServerId != 0)
			{
				BattleServerId = other.BattleServerId;
			}
			if (other.ServerType != 0)
			{
				ServerType = other.ServerType;
			}
			if (other.PointId != 0)
			{
				PointId = other.PointId;
			}
			if (other.WorldId != 0)
			{
				WorldId = other.WorldId;
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
				BattleServerId = input.ReadInt32();
				break;
			case 16u:
				ServerType = input.ReadInt32();
				break;
			case 24u:
				PointId = input.ReadInt32();
				break;
			case 32u:
				WorldId = input.ReadInt32();
				break;
			}
		}
	}
}
