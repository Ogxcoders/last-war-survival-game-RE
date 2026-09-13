using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CityGreenInfo : IMessage<CityGreenInfo>, IMessage, IEquatable<CityGreenInfo>, IDeepCloneable<CityGreenInfo>
{
	private static readonly MessageParser<CityGreenInfo> _parser = new MessageParser<CityGreenInfo>(() => new CityGreenInfo());

	private UnknownFieldSet _unknownFields;

	public const int CityIdFieldNumber = 1;

	private int cityId_;

	public const int GreenRateFieldNumber = 2;

	private double greenRate_;

	[DebuggerNonUserCode]
	public static MessageParser<CityGreenInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => CityAreaGreenInfoReflection.Descriptor.MessageTypes[0];

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
	public double GreenRate
	{
		get
		{
			return greenRate_;
		}
		set
		{
			greenRate_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityGreenInfo()
	{
	}

	[DebuggerNonUserCode]
	public CityGreenInfo(CityGreenInfo other)
		: this()
	{
		cityId_ = other.cityId_;
		greenRate_ = other.greenRate_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CityGreenInfo Clone()
	{
		return new CityGreenInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CityGreenInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CityGreenInfo other)
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
		if (!ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(GreenRate, other.GreenRate))
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
		if (GreenRate != 0.0)
		{
			num ^= ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(GreenRate);
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
		if (GreenRate != 0.0)
		{
			output.WriteRawTag(17);
			output.WriteDouble(GreenRate);
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
		if (GreenRate != 0.0)
		{
			num += 9;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CityGreenInfo other)
	{
		if (other != null)
		{
			if (other.CityId != 0)
			{
				CityId = other.CityId;
			}
			if (other.GreenRate != 0.0)
			{
				GreenRate = other.GreenRate;
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
				CityId = input.ReadInt32();
				break;
			case 17u:
				GreenRate = input.ReadDouble();
				break;
			}
		}
	}
}
