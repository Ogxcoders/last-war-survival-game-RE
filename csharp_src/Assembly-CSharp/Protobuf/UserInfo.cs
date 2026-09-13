using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class UserInfo : IMessage<UserInfo>, IMessage, IEquatable<UserInfo>, IDeepCloneable<UserInfo>
{
	private static readonly MessageParser<UserInfo> _parser = new MessageParser<UserInfo>(() => new UserInfo());

	private UnknownFieldSet _unknownFields;

	public const int UidFieldNumber = 1;

	private string uid_ = "";

	public const int NameFieldNumber = 2;

	private string name_ = "";

	public const int PicFieldNumber = 3;

	private string pic_ = "";

	public const int PicVerFieldNumber = 4;

	private static readonly FieldCodec<int?> _single_picVer_codec = FieldCodec.ForStructWrapper<int>(34u);

	private int? picVer_;

	public const int LevelFieldNumber = 5;

	private int level_;

	public const int WolfEndTimeFieldNumber = 6;

	private long wolfEndTime_;

	[DebuggerNonUserCode]
	public static MessageParser<UserInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[31];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Uid
	{
		get
		{
			return uid_;
		}
		set
		{
			uid_ = ProtoPreconditions.CheckNotNull(value, "value");
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
	public int? PicVer
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
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long WolfEndTime
	{
		get
		{
			return wolfEndTime_;
		}
		set
		{
			wolfEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public UserInfo()
	{
	}

	[DebuggerNonUserCode]
	public UserInfo(UserInfo other)
		: this()
	{
		uid_ = other.uid_;
		name_ = other.name_;
		pic_ = other.pic_;
		PicVer = other.PicVer;
		level_ = other.level_;
		wolfEndTime_ = other.wolfEndTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public UserInfo Clone()
	{
		return new UserInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as UserInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(UserInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (Name != other.Name)
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
		if (Level != other.Level)
		{
			return false;
		}
		if (WolfEndTime != other.WolfEndTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (Pic.Length != 0)
		{
			num ^= Pic.GetHashCode();
		}
		if (picVer_.HasValue)
		{
			num ^= PicVer.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (WolfEndTime != 0L)
		{
			num ^= WolfEndTime.GetHashCode();
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
		if (Uid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Uid);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Name);
		}
		if (Pic.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Pic);
		}
		if (picVer_.HasValue)
		{
			_single_picVer_codec.WriteTagAndValue(output, PicVer);
		}
		if (Level != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Level);
		}
		if (WolfEndTime != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(WolfEndTime);
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
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (Pic.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Pic);
		}
		if (picVer_.HasValue)
		{
			num += _single_picVer_codec.CalculateSizeWithTag(PicVer);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (WolfEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(WolfEndTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(UserInfo other)
	{
		if (other != null)
		{
			if (other.Uid.Length != 0)
			{
				Uid = other.Uid;
			}
			if (other.Name.Length != 0)
			{
				Name = other.Name;
			}
			if (other.Pic.Length != 0)
			{
				Pic = other.Pic;
			}
			if (other.picVer_.HasValue && (!picVer_.HasValue || other.PicVer != 0))
			{
				PicVer = other.PicVer;
			}
			if (other.Level != 0)
			{
				Level = other.Level;
			}
			if (other.WolfEndTime != 0L)
			{
				WolfEndTime = other.WolfEndTime;
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
				Uid = input.ReadString();
				break;
			case 18u:
				Name = input.ReadString();
				break;
			case 26u:
				Pic = input.ReadString();
				break;
			case 34u:
			{
				int? num2 = _single_picVer_codec.Read(input);
				if (!picVer_.HasValue || num2 != 0)
				{
					PicVer = num2;
				}
				break;
			}
			case 40u:
				Level = input.ReadInt32();
				break;
			case 48u:
				WolfEndTime = input.ReadInt64();
				break;
			}
		}
	}
}
