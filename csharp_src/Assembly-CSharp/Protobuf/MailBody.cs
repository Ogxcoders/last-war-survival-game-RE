using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MailBody : IMessage<MailBody>, IMessage, IEquatable<MailBody>, IDeepCloneable<MailBody>
{
	private static readonly MessageParser<MailBody> _parser = new MessageParser<MailBody>(() => new MailBody());

	private UnknownFieldSet _unknownFields;

	public const int ContentFieldNumber = 1;

	private Message content_;

	public const int PayFieldNumber = 2;

	private Pay pay_;

	public const int RewardFieldNumber = 3;

	private Reward reward_;

	public const int UserInfoFieldNumber = 4;

	private UserInfo userInfo_;

	public const int AllianceInfoFieldNumber = 5;

	private AllianceInfo allianceInfo_;

	public const int AllianceInviteMoveFieldNumber = 6;

	private AllianceInviteMove allianceInviteMove_;

	public const int ActivationFieldNumber = 7;

	private Activation activation_;

	public const int AllianceInviteFieldNumber = 8;

	private AllianceInvite allianceInvite_;

	public const int MailIdFieldNumber = 9;

	private int mailId_;

	public const int ExtraFieldNumber = 10;

	private string extra_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<MailBody> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public Message Content
	{
		get
		{
			return content_;
		}
		set
		{
			content_ = value;
		}
	}

	[DebuggerNonUserCode]
	public Pay Pay
	{
		get
		{
			return pay_;
		}
		set
		{
			pay_ = value;
		}
	}

	[DebuggerNonUserCode]
	public Reward Reward
	{
		get
		{
			return reward_;
		}
		set
		{
			reward_ = value;
		}
	}

	[DebuggerNonUserCode]
	public UserInfo UserInfo
	{
		get
		{
			return userInfo_;
		}
		set
		{
			userInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceInfo AllianceInfo
	{
		get
		{
			return allianceInfo_;
		}
		set
		{
			allianceInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceInviteMove AllianceInviteMove
	{
		get
		{
			return allianceInviteMove_;
		}
		set
		{
			allianceInviteMove_ = value;
		}
	}

	[DebuggerNonUserCode]
	public Activation Activation
	{
		get
		{
			return activation_;
		}
		set
		{
			activation_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceInvite AllianceInvite
	{
		get
		{
			return allianceInvite_;
		}
		set
		{
			allianceInvite_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int MailId
	{
		get
		{
			return mailId_;
		}
		set
		{
			mailId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Extra
	{
		get
		{
			return extra_;
		}
		set
		{
			extra_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public MailBody()
	{
	}

	[DebuggerNonUserCode]
	public MailBody(MailBody other)
		: this()
	{
		content_ = ((other.content_ != null) ? other.content_.Clone() : null);
		pay_ = ((other.pay_ != null) ? other.pay_.Clone() : null);
		reward_ = ((other.reward_ != null) ? other.reward_.Clone() : null);
		userInfo_ = ((other.userInfo_ != null) ? other.userInfo_.Clone() : null);
		allianceInfo_ = ((other.allianceInfo_ != null) ? other.allianceInfo_.Clone() : null);
		allianceInviteMove_ = ((other.allianceInviteMove_ != null) ? other.allianceInviteMove_.Clone() : null);
		activation_ = ((other.activation_ != null) ? other.activation_.Clone() : null);
		allianceInvite_ = ((other.allianceInvite_ != null) ? other.allianceInvite_.Clone() : null);
		mailId_ = other.mailId_;
		extra_ = other.extra_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MailBody Clone()
	{
		return new MailBody(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MailBody);
	}

	[DebuggerNonUserCode]
	public bool Equals(MailBody other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(Content, other.Content))
		{
			return false;
		}
		if (!object.Equals(Pay, other.Pay))
		{
			return false;
		}
		if (!object.Equals(Reward, other.Reward))
		{
			return false;
		}
		if (!object.Equals(UserInfo, other.UserInfo))
		{
			return false;
		}
		if (!object.Equals(AllianceInfo, other.AllianceInfo))
		{
			return false;
		}
		if (!object.Equals(AllianceInviteMove, other.AllianceInviteMove))
		{
			return false;
		}
		if (!object.Equals(Activation, other.Activation))
		{
			return false;
		}
		if (!object.Equals(AllianceInvite, other.AllianceInvite))
		{
			return false;
		}
		if (MailId != other.MailId)
		{
			return false;
		}
		if (Extra != other.Extra)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (content_ != null)
		{
			num ^= Content.GetHashCode();
		}
		if (pay_ != null)
		{
			num ^= Pay.GetHashCode();
		}
		if (reward_ != null)
		{
			num ^= Reward.GetHashCode();
		}
		if (userInfo_ != null)
		{
			num ^= UserInfo.GetHashCode();
		}
		if (allianceInfo_ != null)
		{
			num ^= AllianceInfo.GetHashCode();
		}
		if (allianceInviteMove_ != null)
		{
			num ^= AllianceInviteMove.GetHashCode();
		}
		if (activation_ != null)
		{
			num ^= Activation.GetHashCode();
		}
		if (allianceInvite_ != null)
		{
			num ^= AllianceInvite.GetHashCode();
		}
		if (MailId != 0)
		{
			num ^= MailId.GetHashCode();
		}
		if (Extra.Length != 0)
		{
			num ^= Extra.GetHashCode();
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
		if (content_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(Content);
		}
		if (pay_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(Pay);
		}
		if (reward_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(Reward);
		}
		if (userInfo_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(UserInfo);
		}
		if (allianceInfo_ != null)
		{
			output.WriteRawTag(42);
			output.WriteMessage(AllianceInfo);
		}
		if (allianceInviteMove_ != null)
		{
			output.WriteRawTag(50);
			output.WriteMessage(AllianceInviteMove);
		}
		if (activation_ != null)
		{
			output.WriteRawTag(58);
			output.WriteMessage(Activation);
		}
		if (allianceInvite_ != null)
		{
			output.WriteRawTag(66);
			output.WriteMessage(AllianceInvite);
		}
		if (MailId != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(MailId);
		}
		if (Extra.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(Extra);
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
		if (content_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Content);
		}
		if (pay_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Pay);
		}
		if (reward_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Reward);
		}
		if (userInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(UserInfo);
		}
		if (allianceInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(AllianceInfo);
		}
		if (allianceInviteMove_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(AllianceInviteMove);
		}
		if (activation_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Activation);
		}
		if (allianceInvite_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(AllianceInvite);
		}
		if (MailId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(MailId);
		}
		if (Extra.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Extra);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MailBody other)
	{
		if (other == null)
		{
			return;
		}
		if (other.content_ != null)
		{
			if (content_ == null)
			{
				Content = new Message();
			}
			Content.MergeFrom(other.Content);
		}
		if (other.pay_ != null)
		{
			if (pay_ == null)
			{
				Pay = new Pay();
			}
			Pay.MergeFrom(other.Pay);
		}
		if (other.reward_ != null)
		{
			if (reward_ == null)
			{
				Reward = new Reward();
			}
			Reward.MergeFrom(other.Reward);
		}
		if (other.userInfo_ != null)
		{
			if (userInfo_ == null)
			{
				UserInfo = new UserInfo();
			}
			UserInfo.MergeFrom(other.UserInfo);
		}
		if (other.allianceInfo_ != null)
		{
			if (allianceInfo_ == null)
			{
				AllianceInfo = new AllianceInfo();
			}
			AllianceInfo.MergeFrom(other.AllianceInfo);
		}
		if (other.allianceInviteMove_ != null)
		{
			if (allianceInviteMove_ == null)
			{
				AllianceInviteMove = new AllianceInviteMove();
			}
			AllianceInviteMove.MergeFrom(other.AllianceInviteMove);
		}
		if (other.activation_ != null)
		{
			if (activation_ == null)
			{
				Activation = new Activation();
			}
			Activation.MergeFrom(other.Activation);
		}
		if (other.allianceInvite_ != null)
		{
			if (allianceInvite_ == null)
			{
				AllianceInvite = new AllianceInvite();
			}
			AllianceInvite.MergeFrom(other.AllianceInvite);
		}
		if (other.MailId != 0)
		{
			MailId = other.MailId;
		}
		if (other.Extra.Length != 0)
		{
			Extra = other.Extra;
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
				if (content_ == null)
				{
					Content = new Message();
				}
				input.ReadMessage(Content);
				break;
			case 18u:
				if (pay_ == null)
				{
					Pay = new Pay();
				}
				input.ReadMessage(Pay);
				break;
			case 26u:
				if (reward_ == null)
				{
					Reward = new Reward();
				}
				input.ReadMessage(Reward);
				break;
			case 34u:
				if (userInfo_ == null)
				{
					UserInfo = new UserInfo();
				}
				input.ReadMessage(UserInfo);
				break;
			case 42u:
				if (allianceInfo_ == null)
				{
					AllianceInfo = new AllianceInfo();
				}
				input.ReadMessage(AllianceInfo);
				break;
			case 50u:
				if (allianceInviteMove_ == null)
				{
					AllianceInviteMove = new AllianceInviteMove();
				}
				input.ReadMessage(AllianceInviteMove);
				break;
			case 58u:
				if (activation_ == null)
				{
					Activation = new Activation();
				}
				input.ReadMessage(Activation);
				break;
			case 66u:
				if (allianceInvite_ == null)
				{
					AllianceInvite = new AllianceInvite();
				}
				input.ReadMessage(AllianceInvite);
				break;
			case 72u:
				MailId = input.ReadInt32();
				break;
			case 82u:
				Extra = input.ReadString();
				break;
			}
		}
	}
}
