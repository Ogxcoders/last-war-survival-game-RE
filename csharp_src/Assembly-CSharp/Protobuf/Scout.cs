using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Scout : IMessage<Scout>, IMessage, IEquatable<Scout>, IDeepCloneable<Scout>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public enum Target
		{
			[OriginalName("DEFAULT")]
			Default,
			[OriginalName("BASE")]
			Base,
			[OriginalName("BUILDING")]
			Building,
			[OriginalName("ARMY")]
			Army,
			[OriginalName("ALLIANCE_CITY")]
			AllianceCity,
			[OriginalName("DESERT")]
			Desert,
			[OriginalName("ALLIANCE_BUILD")]
			AllianceBuild,
			[OriginalName("WINTER_STORM_BUILDING")]
			WinterStormBuilding,
			[OriginalName("CITY_STRONGHOLD")]
			CityStronghold,
			[OriginalName("CITY_TRADE")]
			CityTrade,
			[OriginalName("QUARANTINE_BUILDING")]
			QuarantineBuilding
		}
	}

	private static readonly MessageParser<Scout> _parser = new MessageParser<Scout>(() => new Scout());

	private UnknownFieldSet _unknownFields;

	public const int TargetFieldNumber = 1;

	private Types.Target target_;

	public const int TargetIdFieldNumber = 2;

	private string targetId_ = "";

	public const int WorldTypeFieldNumber = 3;

	private int worldType_;

	public const int BattleConfigIdFieldNumber = 4;

	private int battleConfigId_;

	[DebuggerNonUserCode]
	public static MessageParser<Scout> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[16];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public Types.Target Target
	{
		get
		{
			return target_;
		}
		set
		{
			target_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string TargetId
	{
		get
		{
			return targetId_;
		}
		set
		{
			targetId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int WorldType
	{
		get
		{
			return worldType_;
		}
		set
		{
			worldType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int BattleConfigId
	{
		get
		{
			return battleConfigId_;
		}
		set
		{
			battleConfigId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public Scout()
	{
	}

	[DebuggerNonUserCode]
	public Scout(Scout other)
		: this()
	{
		target_ = other.target_;
		targetId_ = other.targetId_;
		worldType_ = other.worldType_;
		battleConfigId_ = other.battleConfigId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Scout Clone()
	{
		return new Scout(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Scout);
	}

	[DebuggerNonUserCode]
	public bool Equals(Scout other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Target != other.Target)
		{
			return false;
		}
		if (TargetId != other.TargetId)
		{
			return false;
		}
		if (WorldType != other.WorldType)
		{
			return false;
		}
		if (BattleConfigId != other.BattleConfigId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Target != Types.Target.Default)
		{
			num ^= Target.GetHashCode();
		}
		if (TargetId.Length != 0)
		{
			num ^= TargetId.GetHashCode();
		}
		if (WorldType != 0)
		{
			num ^= WorldType.GetHashCode();
		}
		if (BattleConfigId != 0)
		{
			num ^= BattleConfigId.GetHashCode();
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
		if (Target != Types.Target.Default)
		{
			output.WriteRawTag(8);
			output.WriteEnum((int)Target);
		}
		if (TargetId.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(TargetId);
		}
		if (WorldType != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(WorldType);
		}
		if (BattleConfigId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(BattleConfigId);
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
		if (Target != Types.Target.Default)
		{
			num += 1 + CodedOutputStream.ComputeEnumSize((int)Target);
		}
		if (TargetId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(TargetId);
		}
		if (WorldType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WorldType);
		}
		if (BattleConfigId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BattleConfigId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Scout other)
	{
		if (other != null)
		{
			if (other.Target != Types.Target.Default)
			{
				Target = other.Target;
			}
			if (other.TargetId.Length != 0)
			{
				TargetId = other.TargetId;
			}
			if (other.WorldType != 0)
			{
				WorldType = other.WorldType;
			}
			if (other.BattleConfigId != 0)
			{
				BattleConfigId = other.BattleConfigId;
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
				Target = (Types.Target)input.ReadEnum();
				break;
			case 18u:
				TargetId = input.ReadString();
				break;
			case 24u:
				WorldType = input.ReadInt32();
				break;
			case 32u:
				BattleConfigId = input.ReadInt32();
				break;
			}
		}
	}
}
