using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutResource : IMessage<ScoutResource>, IMessage, IEquatable<ScoutResource>, IDeepCloneable<ScoutResource>
{
	private static readonly MessageParser<ScoutResource> _parser = new MessageParser<ScoutResource>(() => new ScoutResource());

	private UnknownFieldSet _unknownFields;

	public const int DataFieldNumber = 1;

	private static readonly FieldCodec<Resource> _repeated_data_codec = FieldCodec.ForMessage(10u, Resource.Parser);

	private readonly RepeatedField<Resource> data_ = new RepeatedField<Resource>();

	public const int VisibleFieldNumber = 2;

	private static readonly FieldCodec<int?> _single_visible_codec = FieldCodec.ForStructWrapper<int>(18u);

	private int? visible_;

	public const int PlunderResRateFieldNumber = 3;

	private static readonly FieldCodec<int?> _single_plunderResRate_codec = FieldCodec.ForStructWrapper<int>(26u);

	private int? plunderResRate_;

	public const int ResourceItemCountFieldNumber = 4;

	private static readonly FieldCodec<int?> _single_resourceItemCount_codec = FieldCodec.ForStructWrapper<int>(34u);

	private int? resourceItemCount_;

	public const int ExtraGoodsFieldNumber = 5;

	private static readonly FieldCodec<Reward> _repeated_extraGoods_codec = FieldCodec.ForMessage(42u, Reward.Parser);

	private readonly RepeatedField<Reward> extraGoods_ = new RepeatedField<Reward>();

	[DebuggerNonUserCode]
	public static MessageParser<ScoutResource> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[8];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public RepeatedField<Resource> Data => data_;

	[DebuggerNonUserCode]
	public int? Visible
	{
		get
		{
			return visible_;
		}
		set
		{
			visible_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? PlunderResRate
	{
		get
		{
			return plunderResRate_;
		}
		set
		{
			plunderResRate_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? ResourceItemCount
	{
		get
		{
			return resourceItemCount_;
		}
		set
		{
			resourceItemCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Reward> ExtraGoods => extraGoods_;

	[DebuggerNonUserCode]
	public ScoutResource()
	{
	}

	[DebuggerNonUserCode]
	public ScoutResource(ScoutResource other)
		: this()
	{
		data_ = other.data_.Clone();
		Visible = other.Visible;
		PlunderResRate = other.PlunderResRate;
		ResourceItemCount = other.ResourceItemCount;
		extraGoods_ = other.extraGoods_.Clone();
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutResource Clone()
	{
		return new ScoutResource(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutResource);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutResource other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!data_.Equals(other.data_))
		{
			return false;
		}
		if (Visible != other.Visible)
		{
			return false;
		}
		if (PlunderResRate != other.PlunderResRate)
		{
			return false;
		}
		if (ResourceItemCount != other.ResourceItemCount)
		{
			return false;
		}
		if (!extraGoods_.Equals(other.extraGoods_))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		num ^= data_.GetHashCode();
		if (visible_.HasValue)
		{
			num ^= Visible.GetHashCode();
		}
		if (plunderResRate_.HasValue)
		{
			num ^= PlunderResRate.GetHashCode();
		}
		if (resourceItemCount_.HasValue)
		{
			num ^= ResourceItemCount.GetHashCode();
		}
		num ^= extraGoods_.GetHashCode();
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
		data_.WriteTo(output, _repeated_data_codec);
		if (visible_.HasValue)
		{
			_single_visible_codec.WriteTagAndValue(output, Visible);
		}
		if (plunderResRate_.HasValue)
		{
			_single_plunderResRate_codec.WriteTagAndValue(output, PlunderResRate);
		}
		if (resourceItemCount_.HasValue)
		{
			_single_resourceItemCount_codec.WriteTagAndValue(output, ResourceItemCount);
		}
		extraGoods_.WriteTo(output, _repeated_extraGoods_codec);
		if (_unknownFields != null)
		{
			_unknownFields.WriteTo(output);
		}
	}

	[DebuggerNonUserCode]
	public int CalculateSize()
	{
		int num = 0;
		num += data_.CalculateSize(_repeated_data_codec);
		if (visible_.HasValue)
		{
			num += _single_visible_codec.CalculateSizeWithTag(Visible);
		}
		if (plunderResRate_.HasValue)
		{
			num += _single_plunderResRate_codec.CalculateSizeWithTag(PlunderResRate);
		}
		if (resourceItemCount_.HasValue)
		{
			num += _single_resourceItemCount_codec.CalculateSizeWithTag(ResourceItemCount);
		}
		num += extraGoods_.CalculateSize(_repeated_extraGoods_codec);
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutResource other)
	{
		if (other != null)
		{
			data_.Add(other.data_);
			if (other.visible_.HasValue && (!visible_.HasValue || other.Visible != 0))
			{
				Visible = other.Visible;
			}
			if (other.plunderResRate_.HasValue && (!plunderResRate_.HasValue || other.PlunderResRate != 0))
			{
				PlunderResRate = other.PlunderResRate;
			}
			if (other.resourceItemCount_.HasValue && (!resourceItemCount_.HasValue || other.ResourceItemCount != 0))
			{
				ResourceItemCount = other.ResourceItemCount;
			}
			extraGoods_.Add(other.extraGoods_);
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
				data_.AddEntriesFrom(input, _repeated_data_codec);
				break;
			case 18u:
			{
				int? num3 = _single_visible_codec.Read(input);
				if (!visible_.HasValue || num3 != 0)
				{
					Visible = num3;
				}
				break;
			}
			case 26u:
			{
				int? num4 = _single_plunderResRate_codec.Read(input);
				if (!plunderResRate_.HasValue || num4 != 0)
				{
					PlunderResRate = num4;
				}
				break;
			}
			case 34u:
			{
				int? num2 = _single_resourceItemCount_codec.Read(input);
				if (!resourceItemCount_.HasValue || num2 != 0)
				{
					ResourceItemCount = num2;
				}
				break;
			}
			case 42u:
				extraGoods_.AddEntriesFrom(input, _repeated_extraGoods_codec);
				break;
			}
		}
	}
}
