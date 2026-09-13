using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class CommonMonsterSkillInfo : IMessage<CommonMonsterSkillInfo>, IMessage, IEquatable<CommonMonsterSkillInfo>, IDeepCloneable<CommonMonsterSkillInfo>
{
	private static readonly MessageParser<CommonMonsterSkillInfo> _parser = new MessageParser<CommonMonsterSkillInfo>(() => new CommonMonsterSkillInfo());

	private UnknownFieldSet _unknownFields;

	public const int DetailInfoFieldNumber = 1;

	private static readonly FieldCodec<CommonMonsterSkillDetailInfo> _repeated_detailInfo_codec = FieldCodec.ForMessage(10u, CommonMonsterSkillDetailInfo.Parser);

	private readonly RepeatedField<CommonMonsterSkillDetailInfo> detailInfo_ = new RepeatedField<CommonMonsterSkillDetailInfo>();

	[DebuggerNonUserCode]
	public static MessageParser<CommonMonsterSkillInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[2];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<CommonMonsterSkillDetailInfo> DetailInfo => detailInfo_;

	[DebuggerNonUserCode]
	public CommonMonsterSkillInfo()
	{
	}

	[DebuggerNonUserCode]
	public CommonMonsterSkillInfo(CommonMonsterSkillInfo other)
		: this()
	{
		detailInfo_ = other.detailInfo_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public CommonMonsterSkillInfo Clone()
	{
		return new CommonMonsterSkillInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as CommonMonsterSkillInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(CommonMonsterSkillInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!detailInfo_.Equals(other.detailInfo_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= detailInfo_.GetHashCode();
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
		detailInfo_.WriteTo(output, _repeated_detailInfo_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += detailInfo_.CalculateSize(_repeated_detailInfo_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CommonMonsterSkillInfo other)
	{
		if (other != null)
		{
			detailInfo_.Add(other.detailInfo_);
			_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
		}
	}

	[DebuggerNonUserCode]
	public void MergeFrom(CodedInputStream input)
	{
		uint num;
		while ((num = input.ReadTag()) != 0)
		{
			if (num != 10)
			{
				_unknownFields = UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
			}
			else
			{
				detailInfo_.AddEntriesFrom(input, _repeated_detailInfo_codec);
			}
		}
	}
}
