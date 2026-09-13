using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BankDeposit : IMessage<BankDeposit>, IMessage, IEquatable<BankDeposit>, IDeepCloneable<BankDeposit>
{
	private static readonly MessageParser<BankDeposit> _parser = new MessageParser<BankDeposit>(() => new BankDeposit());

	private UnknownFieldSet _unknownFields;

	public const int DepositAmountFieldNumber = 1;

	private long depositAmount_;

	public const int DepositDaysFieldNumber = 2;

	private int depositDays_;

	public const int ItemIdFieldNumber = 3;

	private string itemId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<BankDeposit> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => StrongholdBankMessageReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long DepositAmount
	{
		get
		{
			return depositAmount_;
		}
		set
		{
			depositAmount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int DepositDays
	{
		get
		{
			return depositDays_;
		}
		set
		{
			depositDays_ = value;
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
	public BankDeposit()
	{
	}

	[DebuggerNonUserCode]
	public BankDeposit(BankDeposit other)
		: this()
	{
		depositAmount_ = other.depositAmount_;
		depositDays_ = other.depositDays_;
		itemId_ = other.itemId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BankDeposit Clone()
	{
		return new BankDeposit(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BankDeposit);
	}

	[DebuggerNonUserCode]
	public bool Equals(BankDeposit other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (DepositAmount != other.DepositAmount)
		{
			return false;
		}
		if (DepositDays != other.DepositDays)
		{
			return false;
		}
		if (ItemId != other.ItemId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (DepositAmount != 0L)
		{
			num ^= DepositAmount.GetHashCode();
		}
		if (DepositDays != 0)
		{
			num ^= DepositDays.GetHashCode();
		}
		if (ItemId.Length != 0)
		{
			num ^= ItemId.GetHashCode();
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
		if (DepositAmount != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(DepositAmount);
		}
		if (DepositDays != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(DepositDays);
		}
		if (ItemId.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(ItemId);
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
		if (DepositAmount != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(DepositAmount);
		}
		if (DepositDays != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(DepositDays);
		}
		if (ItemId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(ItemId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BankDeposit other)
	{
		if (other != null)
		{
			if (other.DepositAmount != 0L)
			{
				DepositAmount = other.DepositAmount;
			}
			if (other.DepositDays != 0)
			{
				DepositDays = other.DepositDays;
			}
			if (other.ItemId.Length != 0)
			{
				ItemId = other.ItemId;
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
				DepositAmount = input.ReadInt64();
				break;
			case 16u:
				DepositDays = input.ReadInt32();
				break;
			case 26u:
				ItemId = input.ReadString();
				break;
			}
		}
	}
}
