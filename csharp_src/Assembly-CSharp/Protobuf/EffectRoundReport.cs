using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class EffectRoundReport : IMessage<EffectRoundReport>, IMessage, IEquatable<EffectRoundReport>, IDeepCloneable<EffectRoundReport>
{
	private static readonly MessageParser<EffectRoundReport> _parser = new MessageParser<EffectRoundReport>(() => new EffectRoundReport());

	private UnknownFieldSet _unknownFields;

	public const int BaseReportFieldNumber = 1;

	private BaseRoundReport baseReport_;

	public const int TimeFieldNumber = 2;

	private int time_;

	[DebuggerNonUserCode]
	public static MessageParser<EffectRoundReport> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public BaseRoundReport BaseReport
	{
		get
		{
			return baseReport_;
		}
		set
		{
			baseReport_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Time
	{
		get
		{
			return time_;
		}
		set
		{
			time_ = value;
		}
	}

	[DebuggerNonUserCode]
	public EffectRoundReport()
	{
	}

	[DebuggerNonUserCode]
	public EffectRoundReport(EffectRoundReport other)
		: this()
	{
		baseReport_ = ((other.baseReport_ != null) ? other.baseReport_.Clone() : null);
		time_ = other.time_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EffectRoundReport Clone()
	{
		return new EffectRoundReport(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EffectRoundReport);
	}

	[DebuggerNonUserCode]
	public bool Equals(EffectRoundReport other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(BaseReport, other.BaseReport))
		{
			return false;
		}
		if (Time != other.Time)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (baseReport_ != null)
		{
			num ^= BaseReport.GetHashCode();
		}
		if (Time != 0)
		{
			num ^= Time.GetHashCode();
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
		if (baseReport_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(BaseReport);
		}
		if (Time != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Time);
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
		if (baseReport_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(BaseReport);
		}
		if (Time != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Time);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EffectRoundReport other)
	{
		if (other == null)
		{
			return;
		}
		if (other.baseReport_ != null)
		{
			if (baseReport_ == null)
			{
				BaseReport = new BaseRoundReport();
			}
			BaseReport.MergeFrom(other.BaseReport);
		}
		if (other.Time != 0)
		{
			Time = other.Time;
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				if (baseReport_ == null)
				{
					BaseReport = new BaseRoundReport();
				}
				input.ReadMessage(BaseReport);
				break;
			case 16u:
				Time = input.ReadInt32();
				break;
			}
		}
	}
}
