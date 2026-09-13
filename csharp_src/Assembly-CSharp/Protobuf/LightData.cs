using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LightData : IMessage<LightData>, IMessage, IEquatable<LightData>, IDeepCloneable<LightData>
{
	private static readonly MessageParser<LightData> _parser = new MessageParser<LightData>(() => new LightData());

	private UnknownFieldSet _unknownFields;

	public const int PointIdFieldNumber = 1;

	private int pointId_;

	public const int RadiusFieldNumber = 2;

	private int radius_;

	public const int LevelFieldNumber = 3;

	private int level_;

	[DebuggerNonUserCode]
	public static MessageParser<LightData> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldLightDataReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

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
	public int Radius
	{
		get
		{
			return radius_;
		}
		set
		{
			radius_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LightData()
	{
	}

	[DebuggerNonUserCode]
	public LightData(LightData other)
		: this()
	{
		pointId_ = other.pointId_;
		radius_ = other.radius_;
		level_ = other.level_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LightData Clone()
	{
		return new LightData(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LightData);
	}

	[DebuggerNonUserCode]
	public bool Equals(LightData other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (PointId != other.PointId)
		{
			return false;
		}
		if (Radius != other.Radius)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (PointId != 0)
		{
			num ^= PointId.GetHashCode();
		}
		if (Radius != 0)
		{
			num ^= Radius.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
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
		if (PointId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(PointId);
		}
		if (Radius != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Radius);
		}
		if (Level != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Level);
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
		if (PointId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PointId);
		}
		if (Radius != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Radius);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LightData other)
	{
		if (other != null)
		{
			if (other.PointId != 0)
			{
				PointId = other.PointId;
			}
			if (other.Radius != 0)
			{
				Radius = other.Radius;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
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
				PointId = input.ReadInt32();
				break;
			case 16u:
				Radius = input.ReadInt32();
				break;
			case 24u:
				Level = input.ReadInt32();
				break;
			}
		}
	}
}
