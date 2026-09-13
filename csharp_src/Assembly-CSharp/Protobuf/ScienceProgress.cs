using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScienceProgress : IMessage<ScienceProgress>, IMessage, IEquatable<ScienceProgress>, IDeepCloneable<ScienceProgress>
{
	private static readonly MessageParser<ScienceProgress> _parser = new MessageParser<ScienceProgress>(() => new ScienceProgress());

	private UnknownFieldSet _unknownFields;

	public const int ScienceTabIdFieldNumber = 1;

	private int scienceTabId_;

	public const int ProgressFieldNumber = 2;

	private int progress_;

	[DebuggerNonUserCode]
	public static MessageParser<ScienceProgress> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[23];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int ScienceTabId
	{
		get
		{
			return scienceTabId_;
		}
		set
		{
			scienceTabId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Progress
	{
		get
		{
			return progress_;
		}
		set
		{
			progress_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScienceProgress()
	{
	}

	[DebuggerNonUserCode]
	public ScienceProgress(ScienceProgress other)
		: this()
	{
		scienceTabId_ = other.scienceTabId_;
		progress_ = other.progress_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScienceProgress Clone()
	{
		return new ScienceProgress(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScienceProgress);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScienceProgress other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ScienceTabId != other.ScienceTabId)
		{
			return false;
		}
		if (Progress != other.Progress)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ScienceTabId != 0)
		{
			num ^= ScienceTabId.GetHashCode();
		}
		if (Progress != 0)
		{
			num ^= Progress.GetHashCode();
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
		if (ScienceTabId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ScienceTabId);
		}
		if (Progress != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Progress);
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
		if (ScienceTabId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ScienceTabId);
		}
		if (Progress != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Progress);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScienceProgress other)
	{
		if (other != null)
		{
			if (other.ScienceTabId != 0)
			{
				ScienceTabId = other.ScienceTabId;
			}
			if (other.Progress != 0)
			{
				Progress = other.Progress;
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
				ScienceTabId = input.ReadInt32();
				break;
			case 16u:
				Progress = input.ReadInt32();
				break;
			}
		}
	}
}
