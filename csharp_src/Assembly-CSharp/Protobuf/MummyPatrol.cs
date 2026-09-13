using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MummyPatrol : IMessage<MummyPatrol>, IMessage, IEquatable<MummyPatrol>, IDeepCloneable<MummyPatrol>
{
	private static readonly MessageParser<MummyPatrol> _parser = new MessageParser<MummyPatrol>(() => new MummyPatrol());

	private UnknownFieldSet _unknownFields;

	public const int FirstTargetPointFieldNumber = 1;

	private int firstTargetPoint_;

	public const int ChangeTimesFieldNumber = 2;

	private int changeTimes_;

	public const int AttackTimesFieldNumber = 3;

	private static readonly MapField<string, int>.Codec _map_attackTimes_codec = new MapField<string, int>.Codec(FieldCodec.ForString(10u, ""), FieldCodec.ForInt32(16u, 0), 26u);

	private readonly MapField<string, int> attackTimes_ = new MapField<string, int>();

	public const int SkillIdFieldNumber = 4;

	private string skillId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<MummyPatrol> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[36];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int FirstTargetPoint
	{
		get
		{
			return firstTargetPoint_;
		}
		set
		{
			firstTargetPoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ChangeTimes
	{
		get
		{
			return changeTimes_;
		}
		set
		{
			changeTimes_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MapField<string, int> AttackTimes => attackTimes_;

	[DebuggerNonUserCode]
	public string SkillId
	{
		get
		{
			return skillId_;
		}
		set
		{
			skillId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public MummyPatrol()
	{
	}

	[DebuggerNonUserCode]
	public MummyPatrol(MummyPatrol other)
		: this()
	{
		firstTargetPoint_ = other.firstTargetPoint_;
		changeTimes_ = other.changeTimes_;
		attackTimes_ = other.attackTimes_.Clone();
		skillId_ = other.skillId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MummyPatrol Clone()
	{
		return new MummyPatrol(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MummyPatrol);
	}

	[DebuggerNonUserCode]
	public bool Equals(MummyPatrol other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (FirstTargetPoint != other.FirstTargetPoint)
		{
			return false;
		}
		if (ChangeTimes != other.ChangeTimes)
		{
			return false;
		}
		if (!AttackTimes.Equals(other.AttackTimes))
		{
			return false;
		}
		if (SkillId != other.SkillId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (FirstTargetPoint != 0)
		{
			num ^= FirstTargetPoint.GetHashCode();
		}
		if (ChangeTimes != 0)
		{
			num ^= ChangeTimes.GetHashCode();
		}
		num ^= AttackTimes.GetHashCode();
		if (SkillId.Length != 0)
		{
			num ^= SkillId.GetHashCode();
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
		if (FirstTargetPoint != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(FirstTargetPoint);
		}
		if (ChangeTimes != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(ChangeTimes);
		}
		attackTimes_.WriteTo(output, _map_attackTimes_codec);
		if (SkillId.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(SkillId);
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
		if (FirstTargetPoint != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(FirstTargetPoint);
		}
		if (ChangeTimes != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ChangeTimes);
		}
		num += attackTimes_.CalculateSize(_map_attackTimes_codec);
		if (SkillId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(SkillId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MummyPatrol other)
	{
		if (other != null)
		{
			if (other.FirstTargetPoint != 0)
			{
				FirstTargetPoint = other.FirstTargetPoint;
			}
			if (other.ChangeTimes != 0)
			{
				ChangeTimes = other.ChangeTimes;
			}
			attackTimes_.Add(other.attackTimes_);
			if (other.SkillId.Length != 0)
			{
				SkillId = other.SkillId;
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
				FirstTargetPoint = input.ReadInt32();
				break;
			case 16u:
				ChangeTimes = input.ReadInt32();
				break;
			case 26u:
				attackTimes_.AddEntriesFrom(input, _map_attackTimes_codec);
				break;
			case 34u:
				SkillId = input.ReadString();
				break;
			}
		}
	}
}
