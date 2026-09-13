using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceCityExtraInfo : IMessage<AllianceCityExtraInfo>, IMessage, IEquatable<AllianceCityExtraInfo>, IDeepCloneable<AllianceCityExtraInfo>
{
	public enum ExtraOneofCase
	{
		None = 0,
		Default = 100,
		MissileFactory = 101
	}

	private static readonly MessageParser<AllianceCityExtraInfo> _parser = new MessageParser<AllianceCityExtraInfo>(() => new AllianceCityExtraInfo());

	private UnknownFieldSet _unknownFields;

	public const int DefaultFieldNumber = 100;

	public const int MissileFactoryFieldNumber = 101;

	private object extra_;

	private ExtraOneofCase extraCase_;

	[DebuggerNonUserCode]
	public static MessageParser<AllianceCityExtraInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[22];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Default
	{
		get
		{
			if (extraCase_ != ExtraOneofCase.Default)
			{
				return 0;
			}
			return (int)extra_;
		}
		set
		{
			extra_ = value;
			extraCase_ = ExtraOneofCase.Default;
		}
	}

	[DebuggerNonUserCode]
	public MissileFactoryInfo MissileFactory
	{
		get
		{
			if (extraCase_ != ExtraOneofCase.MissileFactory)
			{
				return null;
			}
			return (MissileFactoryInfo)extra_;
		}
		set
		{
			extra_ = value;
			extraCase_ = ((value != null) ? ExtraOneofCase.MissileFactory : ExtraOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public ExtraOneofCase ExtraCase => extraCase_;

	[DebuggerNonUserCode]
	public AllianceCityExtraInfo()
	{
	}

	[DebuggerNonUserCode]
	public AllianceCityExtraInfo(AllianceCityExtraInfo other)
		: this()
	{
		switch (other.ExtraCase)
		{
		case ExtraOneofCase.Default:
			Default = other.Default;
			break;
		case ExtraOneofCase.MissileFactory:
			MissileFactory = other.MissileFactory.Clone();
			break;
		}
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceCityExtraInfo Clone()
	{
		return new AllianceCityExtraInfo(this);
	}

	[DebuggerNonUserCode]
	public void ClearExtra()
	{
		extraCase_ = ExtraOneofCase.None;
		extra_ = null;
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceCityExtraInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceCityExtraInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Default != other.Default)
		{
			return false;
		}
		if (!object.Equals(MissileFactory, other.MissileFactory))
		{
			return false;
		}
		if (ExtraCase != other.ExtraCase)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (extraCase_ == ExtraOneofCase.Default)
		{
			num ^= Default.GetHashCode();
		}
		if (extraCase_ == ExtraOneofCase.MissileFactory)
		{
			num ^= MissileFactory.GetHashCode();
		}
		num ^= (int)extraCase_;
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
		if (extraCase_ == ExtraOneofCase.Default)
		{
			output.WriteRawTag(160, 6);
			output.WriteInt32(Default);
		}
		if (extraCase_ == ExtraOneofCase.MissileFactory)
		{
			output.WriteRawTag(170, 6);
			output.WriteMessage(MissileFactory);
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
		if (extraCase_ == ExtraOneofCase.Default)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Default);
		}
		if (extraCase_ == ExtraOneofCase.MissileFactory)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MissileFactory);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceCityExtraInfo other)
	{
		if (other == null)
		{
			return;
		}
		switch (other.ExtraCase)
		{
		case ExtraOneofCase.Default:
			Default = other.Default;
			break;
		case ExtraOneofCase.MissileFactory:
			if (MissileFactory == null)
			{
				MissileFactory = new MissileFactoryInfo();
			}
			MissileFactory.MergeFrom(other.MissileFactory);
			break;
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
			case 800u:
				Default = input.ReadInt32();
				break;
			case 810u:
			{
				MissileFactoryInfo missileFactoryInfo = new MissileFactoryInfo();
				if (extraCase_ == ExtraOneofCase.MissileFactory)
				{
					missileFactoryInfo.MergeFrom(MissileFactory);
				}
				input.ReadMessage(missileFactoryInfo);
				MissileFactory = missileFactoryInfo;
				break;
			}
			}
		}
	}
}
