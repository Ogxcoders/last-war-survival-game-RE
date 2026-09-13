using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WorldAllCityGreenInfo : IMessage<WorldAllCityGreenInfo>, IMessage, IEquatable<WorldAllCityGreenInfo>, IDeepCloneable<WorldAllCityGreenInfo>
{
	private static readonly MessageParser<WorldAllCityGreenInfo> _parser = new MessageParser<WorldAllCityGreenInfo>(() => new WorldAllCityGreenInfo());

	private UnknownFieldSet _unknownFields;

	public const int InfoListFieldNumber = 1;

	private static readonly FieldCodec<CityGreenInfo> _repeated_infoList_codec = FieldCodec.ForMessage(10u, CityGreenInfo.Parser);

	private readonly RepeatedField<CityGreenInfo> infoList_ = new RepeatedField<CityGreenInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<WorldAllCityGreenInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => CityAreaGreenInfoReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<CityGreenInfo> InfoList => infoList_;

	[DebuggerNonUserCode]
	public WorldAllCityGreenInfo()
	{
	}

	[DebuggerNonUserCode]
	public WorldAllCityGreenInfo(WorldAllCityGreenInfo other)
		: this()
	{
		infoList_ = other.infoList_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WorldAllCityGreenInfo Clone()
	{
		return new WorldAllCityGreenInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WorldAllCityGreenInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(WorldAllCityGreenInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!infoList_.Equals(other.infoList_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= infoList_.GetHashCode();
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
		infoList_.WriteTo(output, _repeated_infoList_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += infoList_.CalculateSize(_repeated_infoList_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WorldAllCityGreenInfo other)
	{
		if (other != null)
		{
			infoList_.Add(other.infoList_);
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
				infoList_.AddEntriesFrom(input, _repeated_infoList_codec);
			}
		}
	}
}
