using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CityCompetitionPoint : IMessage<CityCompetitionPoint>, IMessage, IEquatable<CityCompetitionPoint>, IDeepCloneable<CityCompetitionPoint>
{
	private static readonly MessageParser<CityCompetitionPoint> _parser = new MessageParser<CityCompetitionPoint>(() => new CityCompetitionPoint());

	private UnknownFieldSet _unknownFields;

	public const int CityIdFieldNumber = 1;

	private int cityId_;

	[DebuggerNonUserCode]
	public static MessageParser<CityCompetitionPoint> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[48];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int CityId
	{
		get
		{
			return cityId_;
		}
		set
		{
			cityId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityCompetitionPoint()
	{
	}

	[DebuggerNonUserCode]
	public CityCompetitionPoint(CityCompetitionPoint other)
		: this()
	{
		cityId_ = other.cityId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CityCompetitionPoint Clone()
	{
		return new CityCompetitionPoint(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CityCompetitionPoint);
	}

	[DebuggerNonUserCode]
	public bool Equals(CityCompetitionPoint other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (CityId != other.CityId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (CityId != 0)
		{
			num ^= CityId.GetHashCode();
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
		if (CityId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(CityId);
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
		if (CityId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CityId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CityCompetitionPoint other)
	{
		if (other != null)
		{
			if (other.CityId != 0)
			{
				CityId = other.CityId;
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
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				CityId = input.ReadInt32();
			}
		}
	}
}
