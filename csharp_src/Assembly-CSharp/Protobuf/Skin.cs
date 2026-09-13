using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Skin : IMessage<Skin>, IMessage, IEquatable<Skin>, IDeepCloneable<Skin>
{
	private static readonly MessageParser<Skin> _parser = new MessageParser<Skin>(() => new Skin());

	private UnknownFieldSet _unknownFields;

	public const int SkinIdFieldNumber = 1;

	private int skinId_;

	public const int TypeFieldNumber = 2;

	private int type_;

	public const int SkinETFieldNumber = 3;

	private long skinET_;

	public const int ColourIdFieldNumber = 4;

	private int colourId_;

	public const int ColourTimeFieldNumber = 5;

	private long colourTime_;

	[DebuggerNonUserCode]
	public static MessageParser<Skin> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[9];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int SkinId
	{
		get
		{
			return skinId_;
		}
		set
		{
			skinId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Type
	{
		get
		{
			return type_;
		}
		set
		{
			type_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long SkinET
	{
		get
		{
			return skinET_;
		}
		set
		{
			skinET_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ColourId
	{
		get
		{
			return colourId_;
		}
		set
		{
			colourId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long ColourTime
	{
		get
		{
			return colourTime_;
		}
		set
		{
			colourTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public Skin()
	{
	}

	[DebuggerNonUserCode]
	public Skin(Skin other)
		: this()
	{
		skinId_ = other.skinId_;
		type_ = other.type_;
		skinET_ = other.skinET_;
		colourId_ = other.colourId_;
		colourTime_ = other.colourTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Skin Clone()
	{
		return new Skin(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Skin);
	}

	[DebuggerNonUserCode]
	public bool Equals(Skin other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (SkinId != other.SkinId)
		{
			return false;
		}
		if (Type != other.Type)
		{
			return false;
		}
		if (SkinET != other.SkinET)
		{
			return false;
		}
		if (ColourId != other.ColourId)
		{
			return false;
		}
		if (ColourTime != other.ColourTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (SkinId != 0)
		{
			num ^= SkinId.GetHashCode();
		}
		if (Type != 0)
		{
			num ^= Type.GetHashCode();
		}
		if (SkinET != 0L)
		{
			num ^= SkinET.GetHashCode();
		}
		if (ColourId != 0)
		{
			num ^= ColourId.GetHashCode();
		}
		if (ColourTime != 0L)
		{
			num ^= ColourTime.GetHashCode();
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
		if (SkinId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(SkinId);
		}
		if (Type != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Type);
		}
		if (SkinET != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(SkinET);
		}
		if (ColourId != 0)
		{
			output.WriteRawTag(32);
			output.WriteInt32(ColourId);
		}
		if (ColourTime != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(ColourTime);
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
		if (SkinId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SkinId);
		}
		if (Type != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Type);
		}
		if (SkinET != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(SkinET);
		}
		if (ColourId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ColourId);
		}
		if (ColourTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(ColourTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Skin other)
	{
		if (other != null)
		{
			if (other.SkinId != 0)
			{
				SkinId = other.SkinId;
			}
			if (other.Type != 0)
			{
				Type = other.Type;
			}
			if (other.SkinET != 0L)
			{
				SkinET = other.SkinET;
			}
			if (other.ColourId != 0)
			{
				ColourId = other.ColourId;
			}
			if (other.ColourTime != 0L)
			{
				ColourTime = other.ColourTime;
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
				SkinId = input.ReadInt32();
				break;
			case 16u:
				Type = input.ReadInt32();
				break;
			case 24u:
				SkinET = input.ReadInt64();
				break;
			case 32u:
				ColourId = input.ReadInt32();
				break;
			case 40u:
				ColourTime = input.ReadInt64();
				break;
			}
		}
	}
}
