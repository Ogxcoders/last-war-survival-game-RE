using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DefendArmyInfo : IMessage<DefendArmyInfo>, IMessage, IEquatable<DefendArmyInfo>, IDeepCloneable<DefendArmyInfo>
{
	private static readonly MessageParser<DefendArmyInfo> _parser = new MessageParser<DefendArmyInfo>(() => new DefendArmyInfo());

	private UnknownFieldSet _unknownFields;

	public const int DefendsFieldNumber = 1;

	private static readonly FieldCodec<ArmyDBUnitInfo> _repeated_defends_codec = FieldCodec.ForMessage(10u, ArmyDBUnitInfo.Parser);

	private readonly RepeatedField<ArmyDBUnitInfo> defends_ = new RepeatedField<ArmyDBUnitInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<DefendArmyInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[33];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<ArmyDBUnitInfo> Defends => defends_;

	[DebuggerNonUserCode]
	public DefendArmyInfo()
	{
	}

	[DebuggerNonUserCode]
	public DefendArmyInfo(DefendArmyInfo other)
		: this()
	{
		defends_ = other.defends_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DefendArmyInfo Clone()
	{
		return new DefendArmyInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DefendArmyInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(DefendArmyInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!defends_.Equals(other.defends_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= defends_.GetHashCode();
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
		defends_.WriteTo(output, _repeated_defends_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += defends_.CalculateSize(_repeated_defends_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DefendArmyInfo other)
	{
		if (other != null)
		{
			defends_.Add(other.defends_);
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
				defends_.AddEntriesFrom(input, _repeated_defends_codec);
			}
		}
	}
}
