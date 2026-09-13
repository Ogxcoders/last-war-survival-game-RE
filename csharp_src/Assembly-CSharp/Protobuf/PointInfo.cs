using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class PointInfo : IMessage<PointInfo>, IMessage, IEquatable<PointInfo>, IDeepCloneable<PointInfo>
{
	private static readonly MessageParser<PointInfo> _parser = new MessageParser<PointInfo>(() => new PointInfo());

	private UnknownFieldSet _unknownFields;

	public const int ServerFieldNumber = 1;

	private static readonly FieldCodec<int?> _single_server_codec = FieldCodec.ForStructWrapper<int>(10u);

	private int? server_;

	public const int XFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_x_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? x_;

	public const int YFieldNumber = 3;

	private static readonly FieldCodec<int?> _single_y_codec = FieldCodec.ForStructWrapper<int>(26u);

	private int? y_;

	public const int WorldIdFieldNumber = 4;

	private static readonly FieldCodec<int?> _single_worldId_codec = FieldCodec.ForStructWrapper<int>(34u);

	private int? worldId_;

	[DebuggerNonUserCode]
	public static MessageParser<PointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[10];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int? Server
	{
		get
		{
			return server_;
		}
		set
		{
			server_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? X
	{
		get
		{
			return x_;
		}
		set
		{
			x_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? Y
	{
		get
		{
			return y_;
		}
		set
		{
			y_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? WorldId
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
	public PointInfo()
	{
	}

	[DebuggerNonUserCode]
	public PointInfo(PointInfo other)
		: this()
	{
		Server = other.Server;
		X = other.X;
		Y = other.Y;
		WorldId = other.WorldId;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PointInfo Clone()
	{
		return new PointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(PointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Server != other.Server)
		{
			return false;
		}
		if (X != other.X)
		{
			return false;
		}
		if (Y != other.Y)
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
		if (server_.HasValue)
		{
			num ^= Server.GetHashCode();
		}
		if (x_.HasValue)
		{
			num ^= X.GetHashCode();
		}
		if (y_.HasValue)
		{
			num ^= Y.GetHashCode();
		}
		if (worldId_.HasValue)
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
		if (server_.HasValue)
		{
			_single_server_codec.WriteTagAndValue(output, Server);
		}
		if (x_.HasValue)
		{
			_single_x_codec.WriteTagAndValue(output, X);
		}
		if (y_.HasValue)
		{
			_single_y_codec.WriteTagAndValue(output, Y);
		}
		if (worldId_.HasValue)
		{
			_single_worldId_codec.WriteTagAndValue(output, WorldId);
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
		if (server_.HasValue)
		{
			num += _single_server_codec.CalculateSizeWithTag(Server);
		}
		if (x_.HasValue)
		{
			num += _single_x_codec.CalculateSizeWithTag(X);
		}
		if (y_.HasValue)
		{
			num += _single_y_codec.CalculateSizeWithTag(Y);
		}
		if (worldId_.HasValue)
		{
			num += _single_worldId_codec.CalculateSizeWithTag(WorldId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(PointInfo other)
	{
		if (other != null)
		{
			if (other.server_.HasValue && (!server_.HasValue || other.Server != 0))
			{
				Server = other.Server;
			}
			if (other.x_.HasValue && (!x_.HasValue || other.X != 0))
			{
				X = other.X;
			}
			if (other.y_.HasValue && (!y_.HasValue || other.Y != 0))
			{
				Y = other.Y;
			}
			if (other.worldId_.HasValue && (!worldId_.HasValue || other.WorldId != 0))
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
			case 10u:
			{
				int? num5 = _single_server_codec.Read(input);
				if (!server_.HasValue || num5 != 0)
				{
					Server = num5;
				}
				break;
			}
			case 18u:
			{
				int? num3 = _single_x_codec.Read(input);
				if (!x_.HasValue || num3 != 0)
				{
					X = num3;
				}
				break;
			}
			case 26u:
			{
				int? num4 = _single_y_codec.Read(input);
				if (!y_.HasValue || num4 != 0)
				{
					Y = num4;
				}
				break;
			}
			case 34u:
			{
				int? num2 = _single_worldId_codec.Read(input);
				if (!worldId_.HasValue || num2 != 0)
				{
					WorldId = num2;
				}
				break;
			}
			}
		}
	}
}
