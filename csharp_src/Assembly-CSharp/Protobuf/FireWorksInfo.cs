using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class FireWorksInfo : IMessage<FireWorksInfo>, IMessage, IEquatable<FireWorksInfo>, IDeepCloneable<FireWorksInfo>
{
	private static readonly MessageParser<FireWorksInfo> _parser = new MessageParser<FireWorksInfo>(() => new FireWorksInfo());

	private UnknownFieldSet _unknownFields;

	public const int SendUidFieldNumber = 1;

	private string sendUid_ = "";

	public const int PicFieldNumber = 2;

	private string pic_ = "";

	public const int PicVerFieldNumber = 3;

	private int picVer_;

	public const int CountryFlagFieldNumber = 4;

	private string countryFlag_ = "";

	public const int HeadSkinIdFieldNumber = 5;

	private int headSkinId_;

	public const int HeadSkinETFieldNumber = 6;

	private long headSkinET_;

	public const int StartTimeFieldNumber = 7;

	private long startTime_;

	public const int EndTimeFieldNumber = 8;

	private long endTime_;

	public const int ConfigIdFieldNumber = 9;

	private int configId_;

	[DebuggerNonUserCode]
	public static MessageParser<FireWorksInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[58];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string SendUid
	{
		get
		{
			return sendUid_;
		}
		set
		{
			sendUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Pic
	{
		get
		{
			return pic_;
		}
		set
		{
			pic_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int PicVer
	{
		get
		{
			return picVer_;
		}
		set
		{
			picVer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string CountryFlag
	{
		get
		{
			return countryFlag_;
		}
		set
		{
			countryFlag_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int HeadSkinId
	{
		get
		{
			return headSkinId_;
		}
		set
		{
			headSkinId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long HeadSkinET
	{
		get
		{
			return headSkinET_;
		}
		set
		{
			headSkinET_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long StartTime
	{
		get
		{
			return startTime_;
		}
		set
		{
			startTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long EndTime
	{
		get
		{
			return endTime_;
		}
		set
		{
			endTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ConfigId
	{
		get
		{
			return configId_;
		}
		set
		{
			configId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public FireWorksInfo()
	{
	}

	[DebuggerNonUserCode]
	public FireWorksInfo(FireWorksInfo other)
		: this()
	{
		sendUid_ = other.sendUid_;
		pic_ = other.pic_;
		picVer_ = other.picVer_;
		countryFlag_ = other.countryFlag_;
		headSkinId_ = other.headSkinId_;
		headSkinET_ = other.headSkinET_;
		startTime_ = other.startTime_;
		endTime_ = other.endTime_;
		configId_ = other.configId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public FireWorksInfo Clone()
	{
		return new FireWorksInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as FireWorksInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(FireWorksInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SendUid != other.SendUid)
		{
			return false;
		}
		if (Pic != other.Pic)
		{
			return false;
		}
		if (PicVer != other.PicVer)
		{
			return false;
		}
		if (CountryFlag != other.CountryFlag)
		{
			return false;
		}
		if (HeadSkinId != other.HeadSkinId)
		{
			return false;
		}
		if (HeadSkinET != other.HeadSkinET)
		{
			return false;
		}
		if (StartTime != other.StartTime)
		{
			return false;
		}
		if (EndTime != other.EndTime)
		{
			return false;
		}
		if (ConfigId != other.ConfigId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SendUid.Length != 0)
		{
			num ^= SendUid.GetHashCode();
		}
		if (Pic.Length != 0)
		{
			num ^= Pic.GetHashCode();
		}
		if (PicVer != 0)
		{
			num ^= PicVer.GetHashCode();
		}
		if (CountryFlag.Length != 0)
		{
			num ^= CountryFlag.GetHashCode();
		}
		if (HeadSkinId != 0)
		{
			num ^= HeadSkinId.GetHashCode();
		}
		if (HeadSkinET != 0L)
		{
			num ^= HeadSkinET.GetHashCode();
		}
		if (StartTime != 0L)
		{
			num ^= StartTime.GetHashCode();
		}
		if (EndTime != 0L)
		{
			num ^= EndTime.GetHashCode();
		}
		if (ConfigId != 0)
		{
			num ^= ConfigId.GetHashCode();
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
		if (SendUid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(SendUid);
		}
		if (Pic.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Pic);
		}
		if (PicVer != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(PicVer);
		}
		if (CountryFlag.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(CountryFlag);
		}
		if (HeadSkinId != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(HeadSkinId);
		}
		if (HeadSkinET != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(HeadSkinET);
		}
		if (StartTime != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(StartTime);
		}
		if (EndTime != 0L)
		{
			output.WriteRawTag(64);
			output.WriteInt64(EndTime);
		}
		if (ConfigId != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(ConfigId);
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
		if (SendUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(SendUid);
		}
		if (Pic.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Pic);
		}
		if (PicVer != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PicVer);
		}
		if (CountryFlag.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(CountryFlag);
		}
		if (HeadSkinId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(HeadSkinId);
		}
		if (HeadSkinET != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(HeadSkinET);
		}
		if (StartTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(StartTime);
		}
		if (EndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(EndTime);
		}
		if (ConfigId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ConfigId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(FireWorksInfo other)
	{
		if (other != null)
		{
			if (other.SendUid.Length != 0)
			{
				SendUid = other.SendUid;
			}
			if (other.Pic.Length != 0)
			{
				Pic = other.Pic;
			}
			if (other.PicVer != 0)
			{
				PicVer = other.PicVer;
			}
			if (other.CountryFlag.Length != 0)
			{
				CountryFlag = other.CountryFlag;
			}
			if (other.HeadSkinId != 0)
			{
				HeadSkinId = other.HeadSkinId;
			}
			if (other.HeadSkinET != 0L)
			{
				HeadSkinET = other.HeadSkinET;
			}
			if (other.StartTime != 0L)
			{
				StartTime = other.StartTime;
			}
			if (other.EndTime != 0L)
			{
				EndTime = other.EndTime;
			}
			if (other.ConfigId != 0)
			{
				ConfigId = other.ConfigId;
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
				SendUid = input.ReadString();
				break;
			case 18u:
				Pic = input.ReadString();
				break;
			case 24u:
				PicVer = input.ReadInt32();
				break;
			case 34u:
				CountryFlag = input.ReadString();
				break;
			case 40u:
				HeadSkinId = input.ReadInt32();
				break;
			case 48u:
				HeadSkinET = input.ReadInt64();
				break;
			case 56u:
				StartTime = input.ReadInt64();
				break;
			case 64u:
				EndTime = input.ReadInt64();
				break;
			case 72u:
				ConfigId = input.ReadInt32();
				break;
			}
		}
	}
}
