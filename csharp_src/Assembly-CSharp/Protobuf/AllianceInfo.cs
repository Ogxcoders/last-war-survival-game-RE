using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceInfo : IMessage<AllianceInfo>, IMessage, IEquatable<AllianceInfo>, IDeepCloneable<AllianceInfo>
{
	private static readonly MessageParser<AllianceInfo> _parser = new MessageParser<AllianceInfo>(() => new AllianceInfo());

	private UnknownFieldSet _unknownFields;

	public const int UidFieldNumber = 1;

	private string uid_ = "";

	public const int NameFieldNumber = 2;

	private string name_ = "";

	public const int AbbrFieldNumber = 3;

	private string abbr_ = "";

	public const int LangFieldNumber = 4;

	private string lang_ = "";

	public const int IconFieldNumber = 5;

	private string icon_ = "";

	public const int CurMemberFieldNumber = 6;

	private static readonly FieldCodec<int?> _single_curMember_codec = FieldCodec.ForStructWrapper<int>(50u);

	private int? curMember_;

	public const int MaxMemberFieldNumber = 7;

	private static readonly FieldCodec<int?> _single_maxMember_codec = FieldCodec.ForStructWrapper<int>(58u);

	private int? maxMember_;

	public const int FightPowerFieldNumber = 8;

	private static readonly FieldCodec<long?> _single_fightPower_codec = FieldCodec.ForStructWrapper<long>(66u);

	private long? fightPower_;

	public const int LeaderNameFieldNumber = 9;

	private string leaderName_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<AllianceInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[32];

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
	public string Abbr
	{
		get
		{
			return abbr_;
		}
		set
		{
			abbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Lang
	{
		get
		{
			return lang_;
		}
		set
		{
			lang_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Icon
	{
		get
		{
			return icon_;
		}
		set
		{
			icon_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int? CurMember
	{
		get
		{
			return curMember_;
		}
		set
		{
			curMember_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? MaxMember
	{
		get
		{
			return maxMember_;
		}
		set
		{
			maxMember_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long? FightPower
	{
		get
		{
			return fightPower_;
		}
		set
		{
			fightPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string LeaderName
	{
		get
		{
			return leaderName_;
		}
		set
		{
			leaderName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public AllianceInfo()
	{
	}

	[DebuggerNonUserCode]
	public AllianceInfo(AllianceInfo other)
		: this()
	{
		uid_ = other.uid_;
		name_ = other.name_;
		abbr_ = other.abbr_;
		lang_ = other.lang_;
		icon_ = other.icon_;
		CurMember = other.CurMember;
		MaxMember = other.MaxMember;
		FightPower = other.FightPower;
		leaderName_ = other.leaderName_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceInfo Clone()
	{
		return new AllianceInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceInfo other)
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
		if (Abbr != other.Abbr)
		{
			return false;
		}
		if (Lang != other.Lang)
		{
			return false;
		}
		if (Icon != other.Icon)
		{
			return false;
		}
		if (CurMember != other.CurMember)
		{
			return false;
		}
		if (MaxMember != other.MaxMember)
		{
			return false;
		}
		if (FightPower != other.FightPower)
		{
			return false;
		}
		if (LeaderName != other.LeaderName)
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
		if (Abbr.Length != 0)
		{
			num ^= Abbr.GetHashCode();
		}
		if (Lang.Length != 0)
		{
			num ^= Lang.GetHashCode();
		}
		if (Icon.Length != 0)
		{
			num ^= Icon.GetHashCode();
		}
		if (curMember_.HasValue)
		{
			num ^= CurMember.GetHashCode();
		}
		if (maxMember_.HasValue)
		{
			num ^= MaxMember.GetHashCode();
		}
		if (fightPower_.HasValue)
		{
			num ^= FightPower.GetHashCode();
		}
		if (LeaderName.Length != 0)
		{
			num ^= LeaderName.GetHashCode();
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
		if (Abbr.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Abbr);
		}
		if (Lang.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(Lang);
		}
		if (Icon.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(Icon);
		}
		if (curMember_.HasValue)
		{
			_single_curMember_codec.WriteTagAndValue(output, CurMember);
		}
		if (maxMember_.HasValue)
		{
			_single_maxMember_codec.WriteTagAndValue(output, MaxMember);
		}
		if (fightPower_.HasValue)
		{
			_single_fightPower_codec.WriteTagAndValue(output, FightPower);
		}
		if (LeaderName.Length != 0)
		{
			output.WriteRawTag(74);
			output.WriteString(LeaderName);
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
		if (Abbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Abbr);
		}
		if (Lang.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Lang);
		}
		if (Icon.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Icon);
		}
		if (curMember_.HasValue)
		{
			num += _single_curMember_codec.CalculateSizeWithTag(CurMember);
		}
		if (maxMember_.HasValue)
		{
			num += _single_maxMember_codec.CalculateSizeWithTag(MaxMember);
		}
		if (fightPower_.HasValue)
		{
			num += _single_fightPower_codec.CalculateSizeWithTag(FightPower);
		}
		if (LeaderName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(LeaderName);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceInfo other)
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
			if (other.Abbr.Length != 0)
			{
				Abbr = other.Abbr;
			}
			if (other.Lang.Length != 0)
			{
				Lang = other.Lang;
			}
			if (other.Icon.Length != 0)
			{
				Icon = other.Icon;
			}
			if (other.curMember_.HasValue && (!curMember_.HasValue || other.CurMember != 0))
			{
				CurMember = other.CurMember;
			}
			if (other.maxMember_.HasValue && (!maxMember_.HasValue || other.MaxMember != 0))
			{
				MaxMember = other.MaxMember;
			}
			if (other.fightPower_.HasValue && (!fightPower_.HasValue || other.FightPower != 0))
			{
				FightPower = other.FightPower;
			}
			if (other.LeaderName.Length != 0)
			{
				LeaderName = other.LeaderName;
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
				Abbr = input.ReadString();
				break;
			case 34u:
				Lang = input.ReadString();
				break;
			case 42u:
				Icon = input.ReadString();
				break;
			case 50u:
			{
				int? num4 = _single_curMember_codec.Read(input);
				if (!curMember_.HasValue || num4 != 0)
				{
					CurMember = num4;
				}
				break;
			}
			case 58u:
			{
				int? num3 = _single_maxMember_codec.Read(input);
				if (!maxMember_.HasValue || num3 != 0)
				{
					MaxMember = num3;
				}
				break;
			}
			case 66u:
			{
				long? num2 = _single_fightPower_codec.Read(input);
				if (!fightPower_.HasValue || num2 != 0)
				{
					FightPower = num2;
				}
				break;
			}
			case 74u:
				LeaderName = input.ReadString();
				break;
			}
		}
	}
}
