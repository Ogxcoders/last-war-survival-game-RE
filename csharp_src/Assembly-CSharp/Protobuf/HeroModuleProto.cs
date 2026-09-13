using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class HeroModuleProto : IMessage<HeroModuleProto>, IMessage, IEquatable<HeroModuleProto>, IDeepCloneable<HeroModuleProto>
{
	private static readonly MessageParser<HeroModuleProto> _parser = new MessageParser<HeroModuleProto>(() => new HeroModuleProto());

	private UnknownFieldSet _unknownFields;

	public const int WeaponStrengTotalLvFieldNumber = 1;

	private int weaponStrengTotalLv_;

	[DebuggerNonUserCode]
	public static MessageParser<HeroModuleProto> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => ArmyUnitInfoReflection.Descriptor.MessageTypes[14];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int WeaponStrengTotalLv
	{
		get
		{
			return weaponStrengTotalLv_;
		}
		set
		{
			weaponStrengTotalLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public HeroModuleProto()
	{
	}

	[DebuggerNonUserCode]
	public HeroModuleProto(HeroModuleProto other)
		: this()
	{
		weaponStrengTotalLv_ = other.weaponStrengTotalLv_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public HeroModuleProto Clone()
	{
		return new HeroModuleProto(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as HeroModuleProto);
	}

	[DebuggerNonUserCode]
	public bool Equals(HeroModuleProto other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (WeaponStrengTotalLv != other.WeaponStrengTotalLv)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (WeaponStrengTotalLv != 0)
		{
			num ^= WeaponStrengTotalLv.GetHashCode();
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
		if (WeaponStrengTotalLv != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(WeaponStrengTotalLv);
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
		if (WeaponStrengTotalLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(WeaponStrengTotalLv);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(HeroModuleProto other)
	{
		if (other != null)
		{
			if (other.WeaponStrengTotalLv != 0)
			{
				WeaponStrengTotalLv = other.WeaponStrengTotalLv;
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
			if (num != 8)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				WeaponStrengTotalLv = input.ReadInt32();
			}
		}
	}
}
