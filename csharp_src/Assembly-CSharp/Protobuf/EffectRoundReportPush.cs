using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class EffectRoundReportPush : IMessage<EffectRoundReportPush>, IMessage, IEquatable<EffectRoundReportPush>, IDeepCloneable<EffectRoundReportPush>
{
	private static readonly MessageParser<EffectRoundReportPush> _parser = new MessageParser<EffectRoundReportPush>(() => new EffectRoundReportPush());

	private UnknownFieldSet _unknownFields;

	public const int RoundReportFieldNumber = 1;

	private EffectRoundReport roundReport_;

	public const int TriggerUuidFieldNumber = 2;

	private long triggerUuid_;

	public const int TargetUuidFieldNumber = 3;

	private long targetUuid_;

	[DebuggerNonUserCode]
	public static MessageParser<EffectRoundReportPush> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleRoundPushReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public EffectRoundReport RoundReport
	{
		get
		{
			return roundReport_;
		}
		set
		{
			roundReport_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long TriggerUuid
	{
		get
		{
			return triggerUuid_;
		}
		set
		{
			triggerUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long TargetUuid
	{
		get
		{
			return targetUuid_;
		}
		set
		{
			targetUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public EffectRoundReportPush()
	{
	}

	[DebuggerNonUserCode]
	public EffectRoundReportPush(EffectRoundReportPush other)
		: this()
	{
		roundReport_ = ((other.roundReport_ != null) ? other.roundReport_.Clone() : null);
		triggerUuid_ = other.triggerUuid_;
		targetUuid_ = other.targetUuid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public EffectRoundReportPush Clone()
	{
		return new EffectRoundReportPush(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as EffectRoundReportPush);
	}

	[DebuggerNonUserCode]
	public bool Equals(EffectRoundReportPush other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(RoundReport, other.RoundReport))
		{
			return false;
		}
		if (TriggerUuid != other.TriggerUuid)
		{
			return false;
		}
		if (TargetUuid != other.TargetUuid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (roundReport_ != null)
		{
			num ^= RoundReport.GetHashCode();
		}
		if (TriggerUuid != 0L)
		{
			num ^= TriggerUuid.GetHashCode();
		}
		if (TargetUuid != 0L)
		{
			num ^= TargetUuid.GetHashCode();
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
		if (roundReport_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(RoundReport);
		}
		if (TriggerUuid != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(TriggerUuid);
		}
		if (TargetUuid != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(TargetUuid);
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
		if (roundReport_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(RoundReport);
		}
		if (TriggerUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TriggerUuid);
		}
		if (TargetUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(TargetUuid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(EffectRoundReportPush other)
	{
		if (other == null)
		{
			return;
		}
		if (other.roundReport_ != null)
		{
			if (roundReport_ == null)
			{
				RoundReport = new EffectRoundReport();
			}
			RoundReport.MergeFrom(other.RoundReport);
		}
		if (other.TriggerUuid != 0L)
		{
			TriggerUuid = other.TriggerUuid;
		}
		if (other.TargetUuid != 0L)
		{
			TargetUuid = other.TargetUuid;
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
				if (roundReport_ == null)
				{
					RoundReport = new EffectRoundReport();
				}
				input.ReadMessage(RoundReport);
				break;
			case 16u:
				TriggerUuid = input.ReadInt64();
				break;
			case 24u:
				TargetUuid = input.ReadInt64();
				break;
			}
		}
	}
}
