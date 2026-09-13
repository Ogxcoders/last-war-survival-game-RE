using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class BuildArmyResult : IMessage<BuildArmyResult>, IMessage, IEquatable<BuildArmyResult>, IDeepCloneable<BuildArmyResult>
{
	private static readonly MessageParser<BuildArmyResult> _parser = new MessageParser<BuildArmyResult>(() => new BuildArmyResult());

	private UnknownFieldSet _unknownFields;

	public const int BaseFieldNumber = 1;

	private ArmyResultBase base_;

	public const int LevelFieldNumber = 2;

	private int level_;

	public const int BuildIdFieldNumber = 3;

	private int buildId_;

	[DebuggerNonUserCode]
	public static MessageParser<BuildArmyResult> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => BattleReportReflection.Descriptor.MessageTypes[16];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public ArmyResultBase Base
	{
		get
		{
			return base_;
		}
		set
		{
			base_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

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
	public BuildArmyResult()
	{
	}

	[DebuggerNonUserCode]
	public BuildArmyResult(BuildArmyResult other)
		: this()
	{
		base_ = ((other.base_ != null) ? other.base_.Clone() : null);
		level_ = other.level_;
		buildId_ = other.buildId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public BuildArmyResult Clone()
	{
		return new BuildArmyResult(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as BuildArmyResult);
	}

	[DebuggerNonUserCode]
	public bool Equals(BuildArmyResult other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(Base, other.Base))
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (BuildId != other.BuildId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (base_ != null)
		{
			num ^= Base.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (BuildId != 0)
		{
			num ^= BuildId.GetHashCode();
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
		if (base_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(Base);
		}
		if (Level != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Level);
		}
		if (BuildId != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(BuildId);
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
		if (base_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Base);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (BuildId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(BuildId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(BuildArmyResult other)
	{
		if (other == null)
		{
			return;
		}
		if (other.base_ != null)
		{
			if (base_ == null)
			{
				Base = new ArmyResultBase();
			}
			Base.MergeFrom(other.Base);
		}
		if (other.Level != 0)
		{
			Level = other.Level;
		}
		if (other.BuildId != 0)
		{
			BuildId = other.BuildId;
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
				if (base_ == null)
				{
					Base = new ArmyResultBase();
				}
				input.ReadMessage(Base);
				break;
			case 16u:
				Level = input.ReadInt32();
				break;
			case 24u:
				BuildId = input.ReadInt32();
				break;
			}
		}
	}
}
