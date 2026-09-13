using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BattleEffectGroup : IMessage<BattleEffectGroup>, IMessage, IEquatable<BattleEffectGroup>, IDeepCloneable<BattleEffectGroup>
{
	private static readonly MessageParser<BattleEffectGroup> _parser = new MessageParser<BattleEffectGroup>(() => new BattleEffectGroup());

	private UnknownFieldSet _unknownFields;

	public const int MemberUuidFieldNumber = 1;

	private long memberUuid_;

	public const int BattleEffectInfosFieldNumber = 2;

	private static readonly FieldCodec<BattleEffectInfo> _repeated_battleEffectInfos_codec = FieldCodec.ForMessage(18u, BattleEffectInfo.Parser);

	private readonly RepeatedField<BattleEffectInfo> battleEffectInfos_ = new RepeatedField<BattleEffectInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<BattleEffectGroup> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[33];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public long MemberUuid
	{
		get
		{
			return memberUuid_;
		}
		set
		{
			memberUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<BattleEffectInfo> BattleEffectInfos => battleEffectInfos_;

	[DebuggerNonUserCode]
	public BattleEffectGroup()
	{
	}

	[DebuggerNonUserCode]
	public BattleEffectGroup(BattleEffectGroup other)
		: this()
	{
		memberUuid_ = other.memberUuid_;
		battleEffectInfos_ = other.battleEffectInfos_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BattleEffectGroup Clone()
	{
		return new BattleEffectGroup(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BattleEffectGroup);
	}

	[DebuggerNonUserCode]
	public bool Equals(BattleEffectGroup other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (MemberUuid != other.MemberUuid)
		{
			return false;
		}
		if (!battleEffectInfos_.Equals(other.battleEffectInfos_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (MemberUuid != 0L)
		{
			num ^= MemberUuid.GetHashCode();
		}
		num ^= battleEffectInfos_.GetHashCode();
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
		if (MemberUuid != 0L)
		{
			output.WriteRawTag(8);
			output.WriteInt64(MemberUuid);
		}
		battleEffectInfos_.WriteTo(output, _repeated_battleEffectInfos_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		if (MemberUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MemberUuid);
		}
		num += battleEffectInfos_.CalculateSize(_repeated_battleEffectInfos_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BattleEffectGroup other)
	{
		if (other != null)
		{
			if (other.MemberUuid != 0L)
			{
				MemberUuid = other.MemberUuid;
			}
			battleEffectInfos_.Add(other.battleEffectInfos_);
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
				MemberUuid = input.ReadInt64();
				break;
			case 18u:
				battleEffectInfos_.AddEntriesFrom(input, _repeated_battleEffectInfos_codec);
				break;
			}
		}
	}
}
