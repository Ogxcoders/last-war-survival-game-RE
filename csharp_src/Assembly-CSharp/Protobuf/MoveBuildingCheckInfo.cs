using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MoveBuildingCheckInfo : IMessage<MoveBuildingCheckInfo>, IMessage, IEquatable<MoveBuildingCheckInfo>, IDeepCloneable<MoveBuildingCheckInfo>
{
	private static readonly MessageParser<MoveBuildingCheckInfo> _parser = new MessageParser<MoveBuildingCheckInfo>(() => new MoveBuildingCheckInfo());

	private UnknownFieldSet _unknownFields;

	public const int IsBlackPointFieldNumber = 1;

	private bool isBlackPoint_;

	public const int IsMeteoriteFreeMoveFieldNumber = 2;

	private bool isMeteoriteFreeMove_;

	[DebuggerNonUserCode]
	public static MessageParser<MoveBuildingCheckInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[67];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public bool IsBlackPoint
	{
		get
		{
			return isBlackPoint_;
		}
		set
		{
			isBlackPoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsMeteoriteFreeMove
	{
		get
		{
			return isMeteoriteFreeMove_;
		}
		set
		{
			isMeteoriteFreeMove_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MoveBuildingCheckInfo()
	{
	}

	[DebuggerNonUserCode]
	public MoveBuildingCheckInfo(MoveBuildingCheckInfo other)
		: this()
	{
		isBlackPoint_ = other.isBlackPoint_;
		isMeteoriteFreeMove_ = other.isMeteoriteFreeMove_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MoveBuildingCheckInfo Clone()
	{
		return new MoveBuildingCheckInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MoveBuildingCheckInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(MoveBuildingCheckInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (IsBlackPoint != other.IsBlackPoint)
		{
			return false;
		}
		if (IsMeteoriteFreeMove != other.IsMeteoriteFreeMove)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (IsBlackPoint)
		{
			num ^= IsBlackPoint.GetHashCode();
		}
		if (IsMeteoriteFreeMove)
		{
			num ^= IsMeteoriteFreeMove.GetHashCode();
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
		if (IsBlackPoint)
		{
			output.WriteRawTag(8);
			output.WriteBool(IsBlackPoint);
		}
		if (IsMeteoriteFreeMove)
		{
			output.WriteRawTag(16);
			output.WriteBool(IsMeteoriteFreeMove);
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
		if (IsBlackPoint)
		{
			num += 2;
		}
		if (IsMeteoriteFreeMove)
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
	public void MergeFrom(MoveBuildingCheckInfo other)
	{
		if (other != null)
		{
			if (other.IsBlackPoint)
			{
				IsBlackPoint = other.IsBlackPoint;
			}
			if (other.IsMeteoriteFreeMove)
			{
				IsMeteoriteFreeMove = other.IsMeteoriteFreeMove;
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
				IsBlackPoint = input.ReadBool();
				break;
			case 16u:
				IsMeteoriteFreeMove = input.ReadBool();
				break;
			}
		}
	}
}
