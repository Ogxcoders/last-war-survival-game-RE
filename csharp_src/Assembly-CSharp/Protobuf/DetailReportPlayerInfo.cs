using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class DetailReportPlayerInfo : IMessage<DetailReportPlayerInfo>, IMessage, IEquatable<DetailReportPlayerInfo>, IDeepCloneable<DetailReportPlayerInfo>
{
	private static readonly MessageParser<DetailReportPlayerInfo> _parser = new MessageParser<DetailReportPlayerInfo>(() => new DetailReportPlayerInfo());

	private UnknownFieldSet _unknownFields;

	public const int IndexFieldNumber = 1;

	private int index_;

	public const int NameFieldNumber = 2;

	private string name_ = "";

	public const int AlAbbrFieldNumber = 3;

	private string alAbbr_ = "";

	public const int IsSelfFieldNumber = 4;

	private bool isSelf_;

	public const int InfoTypeFieldNumber = 5;

	private int infoType_;

	public const int UuidFieldNumber = 6;

	private long uuid_;

	[DebuggerNonUserCode]
	public static MessageParser<DetailReportPlayerInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[31];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Index
	{
		get
		{
			return index_;
		}
		set
		{
			index_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public bool IsSelf
	{
		get
		{
			return isSelf_;
		}
		set
		{
			isSelf_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int InfoType
	{
		get
		{
			return infoType_;
		}
		set
		{
			infoType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Uuid
	{
		get
		{
			return uuid_;
		}
		set
		{
			uuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public DetailReportPlayerInfo()
	{
	}

	[DebuggerNonUserCode]
	public DetailReportPlayerInfo(DetailReportPlayerInfo other)
		: this()
	{
		index_ = other.index_;
		name_ = other.name_;
		alAbbr_ = other.alAbbr_;
		isSelf_ = other.isSelf_;
		infoType_ = other.infoType_;
		uuid_ = other.uuid_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public DetailReportPlayerInfo Clone()
	{
		return new DetailReportPlayerInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as DetailReportPlayerInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(DetailReportPlayerInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Index != other.Index)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (AlAbbr != other.AlAbbr)
		{
			return false;
		}
		if (IsSelf != other.IsSelf)
		{
			return false;
		}
		if (InfoType != other.InfoType)
		{
			return false;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Index != 0)
		{
			num ^= Index.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (AlAbbr.Length != 0)
		{
			num ^= AlAbbr.GetHashCode();
		}
		if (IsSelf)
		{
			num ^= IsSelf.GetHashCode();
		}
		if (InfoType != 0)
		{
			num ^= InfoType.GetHashCode();
		}
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
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
		if (Index != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Index);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Name);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(AlAbbr);
		}
		if (IsSelf)
		{
			output.WriteRawTag(32);
			output.WriteBool(IsSelf);
		}
		if (InfoType != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(InfoType);
		}
		if (Uuid != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(Uuid);
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
		if (Index != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Index);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (AlAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlAbbr);
		}
		if (IsSelf)
		{
			num += 2;
		}
		if (InfoType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(InfoType);
		}
		if (Uuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(DetailReportPlayerInfo other)
	{
		if (other != null)
		{
			if (other.Index != 0)
			{
				Index = other.Index;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.AlAbbr.Length != 0)
			{
				AlAbbr = other.AlAbbr;
			}
			if (other.IsSelf)
			{
				IsSelf = other.IsSelf;
			}
			if (other.InfoType != 0)
			{
				InfoType = other.InfoType;
			}
			if (other.Uuid != 0L)
			{
				Uuid = other.Uuid;
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
				Index = input.ReadInt32();
				break;
			case 18u:
				Name = input.ReadString();
				break;
			case 26u:
				AlAbbr = input.ReadString();
				break;
			case 32u:
				IsSelf = input.ReadBool();
				break;
			case 40u:
				InfoType = input.ReadInt32();
				break;
			case 48u:
				Uuid = input.ReadInt64();
				break;
			}
		}
	}
}
