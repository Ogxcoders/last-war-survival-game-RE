using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CityAttachment : IMessage<CityAttachment>, IMessage, IEquatable<CityAttachment>, IDeepCloneable<CityAttachment>
{
	private static readonly MessageParser<CityAttachment> _parser = new MessageParser<CityAttachment>(() => new CityAttachment());

	private UnknownFieldSet _unknownFields;

	public const int BuildIdFieldNumber = 1;

	private int buildId_;

	public const int StateFieldNumber = 2;

	private int state_;

	public const int CurExpFieldNumber = 3;

	private long curExp_;

	public const int AlAbbrFieldNumber = 4;

	private string alAbbr_ = "";

	public const int AllianceIdFieldNumber = 5;

	private string allianceId_ = "";

	public const int RewardNumFieldNumber = 6;

	private int rewardNum_;

	public const int AlNameFieldNumber = 7;

	private string alName_ = "";

	public const int ServerIdFieldNumber = 8;

	private int serverId_;

	public const int IsFirstFieldNumber = 9;

	private bool isFirst_;

	[DebuggerNonUserCode]
	public static MessageParser<CityAttachment> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[39];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int BuildId
	{
		get
		{
			return buildId_;
		}
		set
		{
			buildId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int State
	{
		get
		{
			return state_;
		}
		set
		{
			state_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurExp
	{
		get
		{
			return curExp_;
		}
		set
		{
			curExp_ = value;
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
	public int RewardNum
	{
		get
		{
			return rewardNum_;
		}
		set
		{
			rewardNum_ = value;
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
	public int ServerId
	{
		get
		{
			return serverId_;
		}
		set
		{
			serverId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsFirst
	{
		get
		{
			return isFirst_;
		}
		set
		{
			isFirst_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityAttachment()
	{
	}

	[DebuggerNonUserCode]
	public CityAttachment(CityAttachment other)
		: this()
	{
		buildId_ = other.buildId_;
		state_ = other.state_;
		curExp_ = other.curExp_;
		alAbbr_ = other.alAbbr_;
		allianceId_ = other.allianceId_;
		rewardNum_ = other.rewardNum_;
		alName_ = other.alName_;
		serverId_ = other.serverId_;
		isFirst_ = other.isFirst_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CityAttachment Clone()
	{
		return new CityAttachment(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CityAttachment);
	}

	[DebuggerNonUserCode]
	public bool Equals(CityAttachment other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (BuildId != other.BuildId)
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		if (CurExp != other.CurExp)
		{
			return false;
		}
		if (AlAbbr != other.AlAbbr)
		{
			return false;
		}
		if (AllianceId != other.AllianceId)
		{
			return false;
		}
		if (RewardNum != other.RewardNum)
		{
			return false;
		}
		if (AlName != other.AlName)
		{
			return false;
		}
		if (ServerId != other.ServerId)
		{
			return false;
		}
		if (IsFirst != other.IsFirst)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (BuildId != 0)
		{
			num ^= BuildId.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (CurExp != 0L)
		{
			num ^= CurExp.GetHashCode();
		}
		if (AlAbbr.Length != 0)
		{
			num ^= AlAbbr.GetHashCode();
		}
		if (AllianceId.Length != 0)
		{
			num ^= AllianceId.GetHashCode();
		}
		if (RewardNum != 0)
		{
			num ^= RewardNum.GetHashCode();
		}
		if (AlName.Length != 0)
		{
			num ^= AlName.GetHashCode();
		}
		if (ServerId != 0)
		{
			num ^= ServerId.GetHashCode();
		}
		if (IsFirst)
		{
			num ^= IsFirst.GetHashCode();
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
		if (BuildId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(BuildId);
		}
		if (State != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(State);
		}
		if (CurExp != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(CurExp);
		}
		if (AlAbbr.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			output.WriteRawTag(42);
			output.WriteString(AllianceId);
		}
		if (RewardNum != 0)
		{
			output.WriteRawTag(48);
			output.WriteInt32(RewardNum);
		}
		if (AlName.Length != 0)
		{
			output.WriteRawTag(58);
			output.WriteString(AlName);
		}
		if (ServerId != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(ServerId);
		}
		if (IsFirst)
		{
			output.WriteRawTag(72);
			output.WriteBool(IsFirst);
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
		if (BuildId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildId);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (CurExp != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurExp);
		}
		if (AlAbbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlAbbr);
		}
		if (AllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AllianceId);
		}
		if (RewardNum != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(RewardNum);
		}
		if (AlName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(AlName);
		}
		if (ServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ServerId);
		}
		if (IsFirst)
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
	public void MergeFrom(CityAttachment other)
	{
		if (other != null)
		{
			if (other.BuildId != 0)
			{
				BuildId = other.BuildId;
			}
			if (other.State != 0)
			{
				State = other.State;
			}
			if (other.CurExp != 0L)
			{
				CurExp = other.CurExp;
			}
			if (other.AlAbbr.Length != 0)
			{
				AlAbbr = other.AlAbbr;
			}
			if (other.AllianceId.Length != 0)
			{
				AllianceId = other.AllianceId;
			}
			if (other.RewardNum != 0)
			{
				RewardNum = other.RewardNum;
			}
			if (other.AlName.Length != 0)
			{
				AlName = other.AlName;
			}
			if (other.ServerId != 0)
			{
				ServerId = other.ServerId;
			}
			if (other.IsFirst)
			{
				IsFirst = other.IsFirst;
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
				BuildId = input.ReadInt32();
				break;
			case 16u:
				State = input.ReadInt32();
				break;
			case 24u:
				CurExp = input.ReadInt64();
				break;
			case 34u:
				AlAbbr = input.ReadString();
				break;
			case 42u:
				AllianceId = input.ReadString();
				break;
			case 48u:
				RewardNum = input.ReadInt32();
				break;
			case 58u:
				AlName = input.ReadString();
				break;
			case 64u:
				ServerId = input.ReadInt32();
				break;
			case 72u:
				IsFirst = input.ReadBool();
				break;
			}
		}
	}
}
