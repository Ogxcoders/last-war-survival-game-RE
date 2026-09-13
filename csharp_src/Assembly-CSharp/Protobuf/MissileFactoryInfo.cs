using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class MissileFactoryInfo : IMessage<MissileFactoryInfo>, IMessage, IEquatable<MissileFactoryInfo>, IDeepCloneable<MissileFactoryInfo>
{
	private static readonly MessageParser<MissileFactoryInfo> _parser = new MessageParser<MissileFactoryInfo>(() => new MissileFactoryInfo());

	private UnknownFieldSet _unknownFields;

	public const int ProductInfoFieldNumber = 1;

	private productInfo productInfo_;

	[DebuggerNonUserCode]
	public static MessageParser<MissileFactoryInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[23];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public productInfo ProductInfo
	{
		get
		{
			return productInfo_;
		}
		set
		{
			productInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MissileFactoryInfo()
	{
	}

	[DebuggerNonUserCode]
	public MissileFactoryInfo(MissileFactoryInfo other)
		: this()
	{
		productInfo_ = ((other.productInfo_ != null) ? other.productInfo_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public MissileFactoryInfo Clone()
	{
		return new MissileFactoryInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as MissileFactoryInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(MissileFactoryInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(ProductInfo, other.ProductInfo))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (productInfo_ != null)
		{
			num ^= ProductInfo.GetHashCode();
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
		if (productInfo_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(ProductInfo);
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
		if (productInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ProductInfo);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(MissileFactoryInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.productInfo_ != null)
		{
			if (productInfo_ == null)
			{
				ProductInfo = new productInfo();
			}
			ProductInfo.MergeFrom(other.ProductInfo);
		}
		_unknownFields = UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
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
				continue;
			}
			if (productInfo_ == null)
			{
				ProductInfo = new productInfo();
			}
			input.ReadMessage(ProductInfo);
		}
	}
}
