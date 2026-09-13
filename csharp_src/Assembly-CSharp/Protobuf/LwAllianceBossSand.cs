using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LwAllianceBossSand : IMessage<LwAllianceBossSand>, IMessage, IEquatable<LwAllianceBossSand>, IDeepCloneable<LwAllianceBossSand>
{
	private static readonly MessageParser<LwAllianceBossSand> _parser = new MessageParser<LwAllianceBossSand>(() => new LwAllianceBossSand());

	private UnknownFieldSet _unknownFields;

	public const int UidFieldNumber = 1;

	private string uid_ = "";

	public const int DamageFieldNumber = 2;

	private long damage_;

	public const int IsCritFieldNumber = 3;

	private bool isCrit_;

	[DebuggerNonUserCode]
	public static MessageParser<LwAllianceBossSand> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwBattleReportReflection.Descriptor.MessageTypes[14];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Uid
	{
		get
		{
			return uid_;
		}
		set
		{
			uid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public long Damage
	{
		get
		{
			return damage_;
		}
		set
		{
			damage_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool IsCrit
	{
		get
		{
			return isCrit_;
		}
		set
		{
			isCrit_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LwAllianceBossSand()
	{
	}

	[DebuggerNonUserCode]
	public LwAllianceBossSand(LwAllianceBossSand other)
		: this()
	{
		uid_ = other.uid_;
		damage_ = other.damage_;
		isCrit_ = other.isCrit_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LwAllianceBossSand Clone()
	{
		return new LwAllianceBossSand(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LwAllianceBossSand);
	}

	[DebuggerNonUserCode]
	public bool Equals(LwAllianceBossSand other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (Damage != other.Damage)
		{
			return false;
		}
		if (IsCrit != other.IsCrit)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (Damage != 0L)
		{
			num ^= Damage.GetHashCode();
		}
		if (IsCrit)
		{
			num ^= IsCrit.GetHashCode();
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
		if (Uid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Uid);
		}
		if (Damage != 0L)
		{
			output.WriteRawTag(16);
			output.WriteInt64(Damage);
		}
		if (IsCrit)
		{
			output.WriteRawTag(24);
			output.WriteBool(IsCrit);
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
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (Damage != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(Damage);
		}
		if (IsCrit)
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
	public void MergeFrom(LwAllianceBossSand other)
	{
		if (other != null)
		{
			if (other.Uid.Length != 0)
			{
				Uid = other.Uid;
			}
			if (other.Damage != 0L)
			{
				Damage = other.Damage;
			}
			if (other.IsCrit)
			{
				IsCrit = other.IsCrit;
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
			case 10u:
				Uid = input.ReadString();
				break;
			case 16u:
				Damage = input.ReadInt64();
				break;
			case 24u:
				IsCrit = input.ReadBool();
				break;
			}
		}
	}
}
