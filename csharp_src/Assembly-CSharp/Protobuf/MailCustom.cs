using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MailCustom : IMessage<MailCustom>, IMessage, IEquatable<MailCustom>, IDeepCloneable<MailCustom>
{
	private static readonly MessageParser<MailCustom> _parser = new MessageParser<MailCustom>(() => new MailCustom());

	private UnknownFieldSet _unknownFields;

	public const int ReplyFieldNumber = 1;

	private int reply_;

	public const int ShareFieldNumber = 2;

	private int share_;

	public const int LikeFieldNumber = 3;

	private int like_;

	public const int ActivityFieldNumber = 4;

	private string activity_ = "";

	public const int DealFieldNumber = 5;

	private int deal_;

	public const int GroupIdFieldNumber = 6;

	private string groupId_ = "";

	public const int SeasonIdFieldNumber = 7;

	private int seasonId_;

	public const int CheckSeasonFieldNumber = 8;

	private bool checkSeason_;

	[DebuggerNonUserCode]
	public static MessageParser<MailCustom> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Reply
	{
		get
		{
			return reply_;
		}
		set
		{
			reply_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Share
	{
		get
		{
			return share_;
		}
		set
		{
			share_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Like
	{
		get
		{
			return like_;
		}
		set
		{
			like_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Activity
	{
		get
		{
			return activity_;
		}
		set
		{
			activity_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int Deal
	{
		get
		{
			return deal_;
		}
		set
		{
			deal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string GroupId
	{
		get
		{
			return groupId_;
		}
		set
		{
			groupId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int SeasonId
	{
		get
		{
			return seasonId_;
		}
		set
		{
			seasonId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool CheckSeason
	{
		get
		{
			return checkSeason_;
		}
		set
		{
			checkSeason_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MailCustom()
	{
	}

	[DebuggerNonUserCode]
	public MailCustom(MailCustom other)
		: this()
	{
		reply_ = other.reply_;
		share_ = other.share_;
		like_ = other.like_;
		activity_ = other.activity_;
		deal_ = other.deal_;
		groupId_ = other.groupId_;
		seasonId_ = other.seasonId_;
		checkSeason_ = other.checkSeason_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MailCustom Clone()
	{
		return new MailCustom(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MailCustom);
	}

	[DebuggerNonUserCode]
	public bool Equals(MailCustom other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Reply != other.Reply)
		{
			return false;
		}
		if (Share != other.Share)
		{
			return false;
		}
		if (Like != other.Like)
		{
			return false;
		}
		if (Activity != other.Activity)
		{
			return false;
		}
		if (Deal != other.Deal)
		{
			return false;
		}
		if (GroupId != other.GroupId)
		{
			return false;
		}
		if (SeasonId != other.SeasonId)
		{
			return false;
		}
		if (CheckSeason != other.CheckSeason)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Reply != 0)
		{
			num ^= Reply.GetHashCode();
		}
		if (Share != 0)
		{
			num ^= Share.GetHashCode();
		}
		if (Like != 0)
		{
			num ^= Like.GetHashCode();
		}
		if (Activity.Length != 0)
		{
			num ^= Activity.GetHashCode();
		}
		if (Deal != 0)
		{
			num ^= Deal.GetHashCode();
		}
		if (GroupId.Length != 0)
		{
			num ^= GroupId.GetHashCode();
		}
		if (SeasonId != 0)
		{
			num ^= SeasonId.GetHashCode();
		}
		if (CheckSeason)
		{
			num ^= CheckSeason.GetHashCode();
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
		if (Reply != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Reply);
		}
		if (Share != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Share);
		}
		if (Like != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Like);
		}
		if (Activity.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(Activity);
		}
		if (Deal != 0)
		{
			output.WriteRawTag(40);
			output.WriteInt32(Deal);
		}
		if (GroupId.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(GroupId);
		}
		if (SeasonId != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(SeasonId);
		}
		if (CheckSeason)
		{
			output.WriteRawTag(64);
			output.WriteBool(CheckSeason);
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
		if (Reply != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Reply);
		}
		if (Share != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Share);
		}
		if (Like != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Like);
		}
		if (Activity.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Activity);
		}
		if (Deal != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Deal);
		}
		if (GroupId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GroupId);
		}
		if (SeasonId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SeasonId);
		}
		if (CheckSeason)
		{
			num += 2;
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MailCustom other)
	{
		if (other != null)
		{
			if (other.Reply != 0)
			{
				Reply = other.Reply;
			}
			if (other.Share != 0)
			{
				Share = other.Share;
			}
			if (other.Like != 0)
			{
				Like = other.Like;
			}
			if (other.Activity.Length != 0)
			{
				Activity = other.Activity;
			}
			if (other.Deal != 0)
			{
				Deal = other.Deal;
			}
			if (other.GroupId.Length != 0)
			{
				GroupId = other.GroupId;
			}
			if (other.SeasonId != 0)
			{
				SeasonId = other.SeasonId;
			}
			if (other.CheckSeason)
			{
				CheckSeason = other.CheckSeason;
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
				Reply = input.ReadInt32();
				break;
			case 16u:
				Share = input.ReadInt32();
				break;
			case 24u:
				Like = input.ReadInt32();
				break;
			case 34u:
				Activity = input.ReadString();
				break;
			case 40u:
				Deal = input.ReadInt32();
				break;
			case 50u:
				GroupId = input.ReadString();
				break;
			case 56u:
				SeasonId = input.ReadInt32();
				break;
			case 64u:
				CheckSeason = input.ReadBool();
				break;
			}
		}
	}
}
