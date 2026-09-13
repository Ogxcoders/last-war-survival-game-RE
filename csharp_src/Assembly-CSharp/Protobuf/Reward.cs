using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Reward : IMessage<Reward>, IMessage, IEquatable<Reward>, IDeepCloneable<Reward>
{
	private static readonly MessageParser<Reward> _parser = new MessageParser<Reward>(() => new Reward());

	private UnknownFieldSet _unknownFields;

	public const int Reward_FieldNumber = 1;

	private string reward_ = "";

	public const int RewardLevelFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_rewardLevel_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? rewardLevel_;

	public const int RewardVersionFieldNumber = 3;

	private string rewardVersion_ = "";

	public const int RewardModuleFieldNumber = 4;

	private static readonly FieldCodec<int?> _single_rewardModule_codec = FieldCodec.ForStructWrapper<int>(34u);

	private int? rewardModule_;

	public const int RewardInfoFieldNumber = 5;

	private static readonly FieldCodec<RewardInfo> _repeated_rewardInfo_codec = FieldCodec.ForMessage(42u, Protobuf.RewardInfo.Parser);

	private readonly RepeatedField<RewardInfo> rewardInfo_ = new RepeatedField<RewardInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<Reward> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[8];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Reward_
	{
		get
		{
			return reward_;
		}
		set
		{
			reward_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int? RewardLevel
	{
		get
		{
			return rewardLevel_;
		}
		set
		{
			rewardLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string RewardVersion
	{
		get
		{
			return rewardVersion_;
		}
		set
		{
			rewardVersion_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int? RewardModule
	{
		get
		{
			return rewardModule_;
		}
		set
		{
			rewardModule_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<RewardInfo> RewardInfo => rewardInfo_;

	[DebuggerNonUserCode]
	public Reward()
	{
	}

	[DebuggerNonUserCode]
	public Reward(Reward other)
		: this()
	{
		reward_ = other.reward_;
		RewardLevel = other.RewardLevel;
		rewardVersion_ = other.rewardVersion_;
		RewardModule = other.RewardModule;
		rewardInfo_ = other.rewardInfo_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Reward Clone()
	{
		return new Reward(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Reward);
	}

	[DebuggerNonUserCode]
	public bool Equals(Reward other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Reward_ != other.Reward_)
		{
			return false;
		}
		if (RewardLevel != other.RewardLevel)
		{
			return false;
		}
		if (RewardVersion != other.RewardVersion)
		{
			return false;
		}
		if (RewardModule != other.RewardModule)
		{
			return false;
		}
		if (!rewardInfo_.Equals(other.rewardInfo_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Reward_.Length != 0)
		{
			num ^= Reward_.GetHashCode();
		}
		if (rewardLevel_.HasValue)
		{
			num ^= RewardLevel.GetHashCode();
		}
		if (RewardVersion.Length != 0)
		{
			num ^= RewardVersion.GetHashCode();
		}
		if (rewardModule_.HasValue)
		{
			num ^= RewardModule.GetHashCode();
		}
		num ^= rewardInfo_.GetHashCode();
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
		if (Reward_.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Reward_);
		}
		if (rewardLevel_.HasValue)
		{
			_single_rewardLevel_codec.WriteTagAndValue(output, RewardLevel);
		}
		if (RewardVersion.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(RewardVersion);
		}
		if (rewardModule_.HasValue)
		{
			_single_rewardModule_codec.WriteTagAndValue(output, RewardModule);
		}
		rewardInfo_.WriteTo(output, _repeated_rewardInfo_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (Reward_.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Reward_);
		}
		if (rewardLevel_.HasValue)
		{
			num += _single_rewardLevel_codec.CalculateSizeWithTag(RewardLevel);
		}
		if (RewardVersion.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(RewardVersion);
		}
		if (rewardModule_.HasValue)
		{
			num += _single_rewardModule_codec.CalculateSizeWithTag(RewardModule);
		}
		num += rewardInfo_.CalculateSize(_repeated_rewardInfo_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Reward other)
	{
		if (other != null)
		{
			if (other.Reward_.Length != 0)
			{
				Reward_ = other.Reward_;
			}
			if (other.rewardLevel_.HasValue && (!rewardLevel_.HasValue || other.RewardLevel != 0))
			{
				RewardLevel = other.RewardLevel;
			}
			if (other.RewardVersion.Length != 0)
			{
				RewardVersion = other.RewardVersion;
			}
			if (other.rewardModule_.HasValue && (!rewardModule_.HasValue || other.RewardModule != 0))
			{
				RewardModule = other.RewardModule;
			}
			rewardInfo_.Add(other.rewardInfo_);
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
				Reward_ = input.ReadString();
				break;
			case 18u:
			{
				int? num3 = _single_rewardLevel_codec.Read(input);
				if (!rewardLevel_.HasValue || num3 != 0)
				{
					RewardLevel = num3;
				}
				break;
			}
			case 26u:
				RewardVersion = input.ReadString();
				break;
			case 34u:
			{
				int? num2 = _single_rewardModule_codec.Read(input);
				if (!rewardModule_.HasValue || num2 != 0)
				{
					RewardModule = num2;
				}
				break;
			}
			case 42u:
				rewardInfo_.AddEntriesFrom(input, _repeated_rewardInfo_codec);
				break;
			}
		}
	}
}
