using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class RangeLightData : IMessage<RangeLightData>, IMessage, IEquatable<RangeLightData>, IDeepCloneable<RangeLightData>
{
	private static readonly MessageParser<RangeLightData> _parser = new MessageParser<RangeLightData>(() => new RangeLightData());

	private UnknownFieldSet _unknownFields;

	public const int LightListFieldNumber = 1;

	private static readonly FieldCodec<LightData> _repeated_lightList_codec = FieldCodec.ForMessage(10u, LightData.Parser);

	private readonly RepeatedField<LightData> lightList_ = new RepeatedField<LightData>();

	[DebuggerNonUserCode]
	public static MessageParser<RangeLightData> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldLightDataReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<LightData> LightList => lightList_;

	[DebuggerNonUserCode]
	public RangeLightData()
	{
	}

	[DebuggerNonUserCode]
	public RangeLightData(RangeLightData other)
		: this()
	{
		lightList_ = other.lightList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public RangeLightData Clone()
	{
		return new RangeLightData(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as RangeLightData);
	}

	[DebuggerNonUserCode]
	public bool Equals(RangeLightData other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!lightList_.Equals(other.lightList_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= lightList_.GetHashCode();
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
		lightList_.WriteTo(output, _repeated_lightList_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += lightList_.CalculateSize(_repeated_lightList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(RangeLightData other)
	{
		if (other != null)
		{
			lightList_.Add(other.lightList_);
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				lightList_.AddEntriesFrom(input, _repeated_lightList_codec);
			}
		}
	}
}
