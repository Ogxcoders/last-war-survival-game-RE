using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class LightHouseInfo : IMessage<LightHouseInfo>, IMessage, IEquatable<LightHouseInfo>, IDeepCloneable<LightHouseInfo>
{
	private static readonly MessageParser<LightHouseInfo> _parser = new MessageParser<LightHouseInfo>(() => new LightHouseInfo());

	private UnknownFieldSet _unknownFields;

	public const int LightHouseActiveFieldNumber = 1;

	private bool lightHouseActive_;

	public const int WorkerActiveFieldNumber = 2;

	private bool workerActive_;

	public const int BrightnessFieldNumber = 3;

	private int brightness_;

	public const int CurrPowerFieldNumber = 4;

	private long currPower_;

	public const int PowerSpeedFieldNumber = 5;

	private long powerSpeed_;

	public const int PowerUpdateTimeFieldNumber = 6;

	private long powerUpdateTime_;

	public const int MaxPowerFieldNumber = 7;

	private long maxPower_;

	public const int LightHouseLevelFieldNumber = 8;

	private int lightHouseLevel_;

	[DebuggerNonUserCode]
	public static MessageParser<LightHouseInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[6];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public bool LightHouseActive
	{
		get
		{
			return lightHouseActive_;
		}
		set
		{
			lightHouseActive_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool WorkerActive
	{
		get
		{
			return workerActive_;
		}
		set
		{
			workerActive_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Brightness
	{
		get
		{
			return brightness_;
		}
		set
		{
			brightness_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long CurrPower
	{
		get
		{
			return currPower_;
		}
		set
		{
			currPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long PowerSpeed
	{
		get
		{
			return powerSpeed_;
		}
		set
		{
			powerSpeed_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long PowerUpdateTime
	{
		get
		{
			return powerUpdateTime_;
		}
		set
		{
			powerUpdateTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long MaxPower
	{
		get
		{
			return maxPower_;
		}
		set
		{
			maxPower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int LightHouseLevel
	{
		get
		{
			return lightHouseLevel_;
		}
		set
		{
			lightHouseLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public LightHouseInfo()
	{
	}

	[DebuggerNonUserCode]
	public LightHouseInfo(LightHouseInfo other)
		: this()
	{
		lightHouseActive_ = other.lightHouseActive_;
		workerActive_ = other.workerActive_;
		brightness_ = other.brightness_;
		currPower_ = other.currPower_;
		powerSpeed_ = other.powerSpeed_;
		powerUpdateTime_ = other.powerUpdateTime_;
		maxPower_ = other.maxPower_;
		lightHouseLevel_ = other.lightHouseLevel_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public LightHouseInfo Clone()
	{
		return new LightHouseInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as LightHouseInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(LightHouseInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (LightHouseActive != other.LightHouseActive)
		{
			return false;
		}
		if (WorkerActive != other.WorkerActive)
		{
			return false;
		}
		if (Brightness != other.Brightness)
		{
			return false;
		}
		if (CurrPower != other.CurrPower)
		{
			return false;
		}
		if (PowerSpeed != other.PowerSpeed)
		{
			return false;
		}
		if (PowerUpdateTime != other.PowerUpdateTime)
		{
			return false;
		}
		if (MaxPower != other.MaxPower)
		{
			return false;
		}
		if (LightHouseLevel != other.LightHouseLevel)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (LightHouseActive)
		{
			num ^= LightHouseActive.GetHashCode();
		}
		if (WorkerActive)
		{
			num ^= WorkerActive.GetHashCode();
		}
		if (Brightness != 0)
		{
			num ^= Brightness.GetHashCode();
		}
		if (CurrPower != 0L)
		{
			num ^= CurrPower.GetHashCode();
		}
		if (PowerSpeed != 0L)
		{
			num ^= PowerSpeed.GetHashCode();
		}
		if (PowerUpdateTime != 0L)
		{
			num ^= PowerUpdateTime.GetHashCode();
		}
		if (MaxPower != 0L)
		{
			num ^= MaxPower.GetHashCode();
		}
		if (LightHouseLevel != 0)
		{
			num ^= LightHouseLevel.GetHashCode();
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
		if (LightHouseActive)
		{
			output.WriteRawTag(8);
			output.WriteBool(LightHouseActive);
		}
		if (WorkerActive)
		{
			output.WriteRawTag(16);
			output.WriteBool(WorkerActive);
		}
		if (Brightness != 0)
		{
			output.WriteRawTag(24);
			output.WriteInt32(Brightness);
		}
		if (CurrPower != 0L)
		{
			output.WriteRawTag(32);
			output.WriteInt64(CurrPower);
		}
		if (PowerSpeed != 0L)
		{
			output.WriteRawTag(40);
			output.WriteInt64(PowerSpeed);
		}
		if (PowerUpdateTime != 0L)
		{
			output.WriteRawTag(48);
			output.WriteInt64(PowerUpdateTime);
		}
		if (MaxPower != 0L)
		{
			output.WriteRawTag(56);
			output.WriteInt64(MaxPower);
		}
		if (LightHouseLevel != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(LightHouseLevel);
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
		if (LightHouseActive)
		{
			num += 2;
		}
		if (WorkerActive)
		{
			num += 2;
		}
		if (Brightness != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Brightness);
		}
		if (CurrPower != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(CurrPower);
		}
		if (PowerSpeed != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(PowerSpeed);
		}
		if (PowerUpdateTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(PowerUpdateTime);
		}
		if (MaxPower != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(MaxPower);
		}
		if (LightHouseLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(LightHouseLevel);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(LightHouseInfo other)
	{
		if (other != null)
		{
			if (other.LightHouseActive)
			{
				LightHouseActive = other.LightHouseActive;
			}
			if (other.WorkerActive)
			{
				WorkerActive = other.WorkerActive;
			}
			if (other.Brightness != 0)
			{
				Brightness = other.Brightness;
			}
			if (other.CurrPower != 0L)
			{
				CurrPower = other.CurrPower;
			}
			if (other.PowerSpeed != 0L)
			{
				PowerSpeed = other.PowerSpeed;
			}
			if (other.PowerUpdateTime != 0L)
			{
				PowerUpdateTime = other.PowerUpdateTime;
			}
			if (other.MaxPower != 0L)
			{
				MaxPower = other.MaxPower;
			}
			if (other.LightHouseLevel != 0)
			{
				LightHouseLevel = other.LightHouseLevel;
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
				LightHouseActive = input.ReadBool();
				break;
			case 16u:
				WorkerActive = input.ReadBool();
				break;
			case 24u:
				Brightness = input.ReadInt32();
				break;
			case 32u:
				CurrPower = input.ReadInt64();
				break;
			case 40u:
				PowerSpeed = input.ReadInt64();
				break;
			case 48u:
				PowerUpdateTime = input.ReadInt64();
				break;
			case 56u:
				MaxPower = input.ReadInt64();
				break;
			case 64u:
				LightHouseLevel = input.ReadInt32();
				break;
			}
		}
	}
}
