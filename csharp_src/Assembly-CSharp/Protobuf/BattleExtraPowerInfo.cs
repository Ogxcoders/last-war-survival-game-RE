using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BattleExtraPowerInfo : IMessage<BattleExtraPowerInfo>, IMessage, IEquatable<BattleExtraPowerInfo>, IDeepCloneable<BattleExtraPowerInfo>
{
	private static readonly MessageParser<BattleExtraPowerInfo> _parser = new MessageParser<BattleExtraPowerInfo>(() => new BattleExtraPowerInfo());

	private UnknownFieldSet _unknownFields;

	public const int ViewTypeFieldNumber = 1;

	private int viewType_;

	public const int ValueFieldNumber = 2;

	private float value_;

	[DebuggerNonUserCode]
	public static MessageParser<BattleExtraPowerInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[36];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int ViewType
	{
		get
		{
			return viewType_;
		}
		set
		{
			viewType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public float Value
	{
		get
		{
			return value_;
		}
		set
		{
			value_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BattleExtraPowerInfo()
	{
	}

	[DebuggerNonUserCode]
	public BattleExtraPowerInfo(BattleExtraPowerInfo other)
		: this()
	{
		viewType_ = other.viewType_;
		value_ = other.value_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleExtraPowerInfo Clone()
	{
		return new BattleExtraPowerInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleExtraPowerInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleExtraPowerInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ViewType != other.ViewType)
		{
			return false;
		}
		if (!ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Value, other.Value))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ViewType != 0)
		{
			num ^= ViewType.GetHashCode();
		}
		if (Value != 0f)
		{
			num ^= ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Value);
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
		if (ViewType != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ViewType);
		}
		if (Value != 0f)
		{
			output.WriteRawTag(21);
			output.WriteFloat(Value);
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
		if (ViewType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ViewType);
		}
		if (Value != 0f)
		{
			num += 5;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleExtraPowerInfo other)
	{
		if (other != null)
		{
			if (other.ViewType != 0)
			{
				ViewType = other.ViewType;
			}
			if (other.Value != 0f)
			{
				Value = other.Value;
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
				ViewType = input.ReadInt32();
				break;
			case 21u:
				Value = input.ReadFloat();
				break;
			}
		}
	}
}
