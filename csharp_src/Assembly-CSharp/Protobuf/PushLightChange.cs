using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class PushLightChange : IMessage<PushLightChange>, IMessage, IEquatable<PushLightChange>, IDeepCloneable<PushLightChange>
{
	private static readonly MessageParser<PushLightChange> _parser = new MessageParser<PushLightChange>(() => new PushLightChange());

	private UnknownFieldSet _unknownFields;

	public const int LightFieldNumber = 1;

	private LightData light_;

	public const int StateFieldNumber = 2;

	private bool state_;

	[DebuggerNonUserCode]
	public static MessageParser<PushLightChange> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldLightDataReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public LightData Light
	{
		get
		{
			return light_;
		}
		set
		{
			light_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool State
	{
		get
		{
			return state_;
		}
		set
		{
			state_ = value;
		}
	}

	[DebuggerNonUserCode]
	public PushLightChange()
	{
	}

	[DebuggerNonUserCode]
	public PushLightChange(PushLightChange other)
		: this()
	{
		light_ = ((other.light_ != null) ? other.light_.Clone() : null);
		state_ = other.state_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public PushLightChange Clone()
	{
		return new PushLightChange(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as PushLightChange);
	}

	[DebuggerNonUserCode]
	public bool Equals(PushLightChange other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (!object.Equals(Light, other.Light))
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (light_ != null)
		{
			num ^= Light.GetHashCode();
		}
		if (State)
		{
			num ^= State.GetHashCode();
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
		if (light_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(Light);
		}
		if (State)
		{
			output.WriteRawTag(16);
			output.WriteBool(State);
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
		if (light_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Light);
		}
		if (State)
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
	public void MergeFrom(PushLightChange other)
	{
		if (other == null)
		{
			return;
		}
		if (other.light_ != null)
		{
			if (light_ == null)
			{
				Light = new LightData();
			}
			Light.MergeFrom(other.Light);
		}
		if (other.State)
		{
			State = other.State;
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
				if (light_ == null)
				{
					Light = new LightData();
				}
				input.ReadMessage(Light);
				break;
			case 16u:
				State = input.ReadBool();
				break;
			}
		}
	}
}
