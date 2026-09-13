using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MeteoriteInfo : IMessage<MeteoriteInfo>, IMessage, IEquatable<MeteoriteInfo>, IDeepCloneable<MeteoriteInfo>
{
	private static readonly MessageParser<MeteoriteInfo> _parser = new MessageParser<MeteoriteInfo>(() => new MeteoriteInfo());

	private UnknownFieldSet _unknownFields;

	public const int CrystalFieldNumber = 1;

	private int crystal_;

	public const int NucleusFieldNumber = 2;

	private int nucleus_;

	[DebuggerNonUserCode]
	public static MessageParser<MeteoriteInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[42];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Crystal
	{
		get
		{
			return crystal_;
		}
		set
		{
			crystal_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Nucleus
	{
		get
		{
			return nucleus_;
		}
		set
		{
			nucleus_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MeteoriteInfo()
	{
	}

	[DebuggerNonUserCode]
	public MeteoriteInfo(MeteoriteInfo other)
		: this()
	{
		crystal_ = other.crystal_;
		nucleus_ = other.nucleus_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MeteoriteInfo Clone()
	{
		return new MeteoriteInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MeteoriteInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(MeteoriteInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Crystal != other.Crystal)
		{
			return false;
		}
		if (Nucleus != other.Nucleus)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Crystal != 0)
		{
			num ^= Crystal.GetHashCode();
		}
		if (Nucleus != 0)
		{
			num ^= Nucleus.GetHashCode();
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
		if (Crystal != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Crystal);
		}
		if (Nucleus != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(Nucleus);
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
		if (Crystal != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Crystal);
		}
		if (Nucleus != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Nucleus);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MeteoriteInfo other)
	{
		if (other != null)
		{
			if (other.Crystal != 0)
			{
				Crystal = other.Crystal;
			}
			if (other.Nucleus != 0)
			{
				Nucleus = other.Nucleus;
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
				Crystal = input.ReadInt32();
				break;
			case 16u:
				Nucleus = input.ReadInt32();
				break;
			}
		}
	}
}
