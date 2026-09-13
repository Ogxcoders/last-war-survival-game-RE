using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ReportAllianceInfo : IMessage<ReportAllianceInfo>, IMessage, IEquatable<ReportAllianceInfo>, IDeepCloneable<ReportAllianceInfo>
{
	private static readonly MessageParser<ReportAllianceInfo> _parser = new MessageParser<ReportAllianceInfo>(() => new ReportAllianceInfo());

	private UnknownFieldSet _unknownFields;

	public const int AllianceIdFieldNumber = 1;

	private string allianceId_ = "";

	public const int AlAbbrFieldNumber = 2;

	private string alAbbr_ = "";

	public const int AlNameFieldNumber = 3;

	private string alName_ = "";

	public const int AlIconFieldNumber = 4;

	private string alIcon_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ReportAllianceInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string AllianceId
	{
		get
		{
			return allianceId_;
		}
		set
		{
			allianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AlAbbr
	{
		get
		{
			return alAbbr_;
		}
		set
		{
			alAbbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AlName
	{
		get
		{
			return alName_;
		}
		set
		{
			alName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string AlIcon
	{
		get
		{
			return alIcon_;
		}
		set
		{
			alIcon_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public ReportAllianceInfo()
	{
	}

	[DebuggerNonUserCode]
	public ReportAllianceInfo(ReportAllianceInfo other)
		: this()
	{
		allianceId_ = other.allianceId_;
		alAbbr_ = other.alAbbr_;
		alName_ = other.alName_;
		alIcon_ = other.alIcon_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ReportAllianceInfo Clone()
	{
		return new ReportAllianceInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ReportAllianceInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ReportAllianceInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (AlAbbr != other.AlAbbr)
		{
			return false;
		}
		if (AlName != other.AlName)
		{
			return false;
		}
		if (AlIcon != other.AlIcon)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (AlAbbr.Length != 0)
		{
			num ^= AlAbbr.GetHashCode();
		}
		if (AlName.Length != 0)
		{
			num ^= AlName.GetHashCode();
		}
		if (AlIcon.Length != 0)
		{
			num ^= AlIcon.GetHashCode();
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
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(AllianceId);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(AlAbbr);
		}
		if (AlName.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(AlName);
		}
		if (AlIcon.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(AlIcon);
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
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (AlAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlAbbr);
		}
		if (AlName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlName);
		}
		if (AlIcon.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlIcon);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ReportAllianceInfo other)
	{
		if (other != null)
		{
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.AlAbbr.Length != 0)
			{
				AlAbbr = other.AlAbbr;
			}
			if (other.AlName.Length != 0)
			{
				AlName = other.AlName;
			}
			if (other.AlIcon.Length != 0)
			{
				AlIcon = other.AlIcon;
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
			case 10u:
				AllianceId = input.ReadString();
				break;
			case 18u:
				AlAbbr = input.ReadString();
				break;
			case 26u:
				AlName = input.ReadString();
				break;
			case 34u:
				AlIcon = input.ReadString();
				break;
			}
		}
	}
}
