using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CombineSelfArmyInfo : IMessage<CombineSelfArmyInfo>, IMessage, IEquatable<CombineSelfArmyInfo>, IDeepCloneable<CombineSelfArmyInfo>
{
	private static readonly MessageParser<CombineSelfArmyInfo> _parser = new MessageParser<CombineSelfArmyInfo>(() => new CombineSelfArmyInfo());

	private UnknownFieldSet _unknownFields;

	public const int MembersFieldNumber = 1;

	private static readonly FieldCodec<SimpleSelfArmyInfo> _repeated_members_codec = FieldCodec.ForMessage(10u, SimpleSelfArmyInfo.Parser);

	private readonly RepeatedField<SimpleSelfArmyInfo> members_ = new RepeatedField<SimpleSelfArmyInfo>();

	public const int TargetInfoFieldNumber = 2;

	private SimpleCombatUnitPushObj targetInfo_;

	[DebuggerNonUserCode]
	public static MessageParser<CombineSelfArmyInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleRoundPushReflection.Descriptor.MessageTypes[3];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<SimpleSelfArmyInfo> Members => members_;

	[DebuggerNonUserCode]
	public SimpleCombatUnitPushObj TargetInfo
	{
		get
		{
			return targetInfo_;
		}
		set
		{
			targetInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CombineSelfArmyInfo()
	{
	}

	[DebuggerNonUserCode]
	public CombineSelfArmyInfo(CombineSelfArmyInfo other)
		: this()
	{
		members_ = other.members_.Clone();
		targetInfo_ = ((other.targetInfo_ != null) ? other.targetInfo_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CombineSelfArmyInfo Clone()
	{
		return new CombineSelfArmyInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CombineSelfArmyInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CombineSelfArmyInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!members_.Equals(other.members_))
		{
			return false;
		}
		if (!object.Equals(TargetInfo, other.TargetInfo))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= members_.GetHashCode();
		if (targetInfo_ != null)
		{
			num ^= TargetInfo.GetHashCode();
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
		members_.WriteTo(output, _repeated_members_codec);
		if (targetInfo_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(TargetInfo);
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
		num += members_.CalculateSize(_repeated_members_codec);
		if (targetInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(TargetInfo);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CombineSelfArmyInfo other)
	{
		if (other == null)
		{
			return;
		}
		members_.Add(other.members_);
		if (other.targetInfo_ != null)
		{
			if (targetInfo_ == null)
			{
				TargetInfo = new SimpleCombatUnitPushObj();
			}
			TargetInfo.MergeFrom(other.TargetInfo);
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
				members_.AddEntriesFrom(input, _repeated_members_codec);
				break;
			case 18u:
				if (targetInfo_ == null)
				{
					TargetInfo = new SimpleCombatUnitPushObj();
				}
				input.ReadMessage(TargetInfo);
				break;
			}
		}
	}
}
