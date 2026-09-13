using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MinePlunderInfo : IMessage<MinePlunderInfo>, IMessage, IEquatable<MinePlunderInfo>, IDeepCloneable<MinePlunderInfo>
{
	private static readonly MessageParser<MinePlunderInfo> _parser = new MessageParser<MinePlunderInfo>(() => new MinePlunderInfo());

	private UnknownFieldSet _unknownFields;

	public const int UserNameFieldNumber = 1;

	private string userName_ = "";

	public const int RewardTypeFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_rewardType_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? rewardType_;

	public const int ItemIdFieldNumber = 3;

	private string itemId_ = "";

	public const int PlunderRewardNumFieldNumber = 4;

	private static readonly FieldCodec<int?> _single_plunderRewardNum_codec = FieldCodec.ForStructWrapper<int>(34u);

	private int? plunderRewardNum_;

	[DebuggerNonUserCode]
	public static MessageParser<MinePlunderInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[5];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string UserName
	{
		get
		{
			return userName_;
		}
		set
		{
			userName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int? RewardType
	{
		get
		{
			return rewardType_;
		}
		set
		{
			rewardType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string ItemId
	{
		get
		{
			return itemId_;
		}
		set
		{
			itemId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int? PlunderRewardNum
	{
		get
		{
			return plunderRewardNum_;
		}
		set
		{
			plunderRewardNum_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MinePlunderInfo()
	{
	}

	[DebuggerNonUserCode]
	public MinePlunderInfo(MinePlunderInfo other)
		: this()
	{
		userName_ = other.userName_;
		RewardType = other.RewardType;
		itemId_ = other.itemId_;
		PlunderRewardNum = other.PlunderRewardNum;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MinePlunderInfo Clone()
	{
		return new MinePlunderInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MinePlunderInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(MinePlunderInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (UserName != other.UserName)
		{
			return false;
		}
		if (RewardType != other.RewardType)
		{
			return false;
		}
		if (ItemId != other.ItemId)
		{
			return false;
		}
		if (PlunderRewardNum != other.PlunderRewardNum)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (UserName.Length != 0)
		{
			num ^= UserName.GetHashCode();
		}
		if (rewardType_.HasValue)
		{
			num ^= RewardType.GetHashCode();
		}
		if (ItemId.Length != 0)
		{
			num ^= ItemId.GetHashCode();
		}
		if (plunderRewardNum_.HasValue)
		{
			num ^= PlunderRewardNum.GetHashCode();
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
		if (UserName.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(UserName);
		}
		if (rewardType_.HasValue)
		{
			_single_rewardType_codec.WriteTagAndValue(output, RewardType);
		}
		if (ItemId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(ItemId);
		}
		if (plunderRewardNum_.HasValue)
		{
			_single_plunderRewardNum_codec.WriteTagAndValue(output, PlunderRewardNum);
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
		if (UserName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(UserName);
		}
		if (rewardType_.HasValue)
		{
			num += _single_rewardType_codec.CalculateSizeWithTag(RewardType);
		}
		if (ItemId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ItemId);
		}
		if (plunderRewardNum_.HasValue)
		{
			num += _single_plunderRewardNum_codec.CalculateSizeWithTag(PlunderRewardNum);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MinePlunderInfo other)
	{
		if (other != null)
		{
			if (other.UserName.Length != 0)
			{
				UserName = other.UserName;
			}
			if (other.rewardType_.HasValue && (!rewardType_.HasValue || other.RewardType != 0))
			{
				RewardType = other.RewardType;
			}
			if (other.ItemId.Length != 0)
			{
				ItemId = other.ItemId;
			}
			if (other.plunderRewardNum_.HasValue && (!plunderRewardNum_.HasValue || other.PlunderRewardNum != 0))
			{
				PlunderRewardNum = other.PlunderRewardNum;
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
				UserName = input.ReadString();
				break;
			case 18u:
			{
				int? num3 = _single_rewardType_codec.Read(input);
				if (!rewardType_.HasValue || num3 != 0)
				{
					RewardType = num3;
				}
				break;
			}
			case 26u:
				ItemId = input.ReadString();
				break;
			case 34u:
			{
				int? num2 = _single_plunderRewardNum_codec.Read(input);
				if (!plunderRewardNum_.HasValue || num2 != 0)
				{
					PlunderRewardNum = num2;
				}
				break;
			}
			}
		}
	}
}
