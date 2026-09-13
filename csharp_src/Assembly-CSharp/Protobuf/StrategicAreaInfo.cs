using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class StrategicAreaInfo : IMessage<StrategicAreaInfo>, IMessage, IEquatable<StrategicAreaInfo>, IDeepCloneable<StrategicAreaInfo>
{
	private static readonly MessageParser<StrategicAreaInfo> _parser = new MessageParser<StrategicAreaInfo>(() => new StrategicAreaInfo());

	private UnknownFieldSet _unknownFields;

	public const int ServersFieldNumber = 1;

	private static readonly FieldCodec<StrategicAreaServer> _repeated_servers_codec = FieldCodec.ForMessage(10u, StrategicAreaServer.Parser);

	private readonly RepeatedField<StrategicAreaServer> servers_ = new RepeatedField<StrategicAreaServer>();

	[DebuggerNonUserCode]
	public static MessageParser<StrategicAreaInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[25];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<StrategicAreaServer> Servers => servers_;

	[DebuggerNonUserCode]
	public StrategicAreaInfo()
	{
	}

	[DebuggerNonUserCode]
	public StrategicAreaInfo(StrategicAreaInfo other)
		: this()
	{
		servers_ = other.servers_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public StrategicAreaInfo Clone()
	{
		return new StrategicAreaInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as StrategicAreaInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(StrategicAreaInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!servers_.Equals(other.servers_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= servers_.GetHashCode();
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
		servers_.WriteTo(output, _repeated_servers_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += servers_.CalculateSize(_repeated_servers_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(StrategicAreaInfo other)
	{
		if (other != null)
		{
			servers_.Add(other.servers_);
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
				servers_.AddEntriesFrom(input, _repeated_servers_codec);
			}
		}
	}
}
