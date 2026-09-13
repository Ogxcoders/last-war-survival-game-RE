using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ResourceInfo : IMessage<ResourceInfo>, IMessage, IEquatable<ResourceInfo>, IDeepCloneable<ResourceInfo>
{
	private static readonly MessageParser<ResourceInfo> _parser = new MessageParser<ResourceInfo>(() => new ResourceInfo());

	private UnknownFieldSet _unknownFields;

	public const int ResourceIdFieldNumber = 1;

	private int resourceId_;

	public const int StateFieldNumber = 2;

	private int state_;

	public const int GatherUuidFieldNumber = 3;

	private long gatherUuid_;

	public const int SpecialTypeFieldNumber = 6;

	private SpecialType specialType_;

	public const int SamplePointInfoFieldNumber = 7;

	private SamplePointInfo samplePointInfo_;

	public const int GatherUidFieldNumber = 8;

	private string gatherUid_ = "";

	public const int GatherServerIdFieldNumber = 9;

	private int gatherServerId_;

	public const int GatherAllianceIdFieldNumber = 10;

	private string gatherAllianceId_ = "";

	[DebuggerNonUserCode]
	public static MessageParser<ResourceInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[13];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int ResourceId
	{
		get
		{
			return resourceId_;
		}
		set
		{
			resourceId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int State
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
	public long GatherUuid
	{
		get
		{
			return gatherUuid_;
		}
		set
		{
			gatherUuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public SpecialType SpecialType
	{
		get
		{
			return specialType_;
		}
		set
		{
			specialType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public SamplePointInfo SamplePointInfo
	{
		get
		{
			return samplePointInfo_;
		}
		set
		{
			samplePointInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string GatherUid
	{
		get
		{
			return gatherUid_;
		}
		set
		{
			gatherUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int GatherServerId
	{
		get
		{
			return gatherServerId_;
		}
		set
		{
			gatherServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string GatherAllianceId
	{
		get
		{
			return gatherAllianceId_;
		}
		set
		{
			gatherAllianceId_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public ResourceInfo()
	{
	}

	[DebuggerNonUserCode]
	public ResourceInfo(ResourceInfo other)
		: this()
	{
		resourceId_ = other.resourceId_;
		state_ = other.state_;
		gatherUuid_ = other.gatherUuid_;
		specialType_ = other.specialType_;
		samplePointInfo_ = ((other.samplePointInfo_ != null) ? other.samplePointInfo_.Clone() : null);
		gatherUid_ = other.gatherUid_;
		gatherServerId_ = other.gatherServerId_;
		gatherAllianceId_ = other.gatherAllianceId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ResourceInfo Clone()
	{
		return new ResourceInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ResourceInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(ResourceInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (ResourceId != other.ResourceId)
		{
			return false;
		}
		if (State != other.State)
		{
			return false;
		}
		if (GatherUuid != other.GatherUuid)
		{
			return false;
		}
		if (SpecialType != other.SpecialType)
		{
			return false;
		}
		if (!object.Equals(SamplePointInfo, other.SamplePointInfo))
		{
			return false;
		}
		if (GatherUid != other.GatherUid)
		{
			return false;
		}
		if (GatherServerId != other.GatherServerId)
		{
			return false;
		}
		if (GatherAllianceId != other.GatherAllianceId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (ResourceId != 0)
		{
			num ^= ResourceId.GetHashCode();
		}
		if (State != 0)
		{
			num ^= State.GetHashCode();
		}
		if (GatherUuid != 0L)
		{
			num ^= GatherUuid.GetHashCode();
		}
		if (SpecialType != SpecialType.None)
		{
			num ^= SpecialType.GetHashCode();
		}
		if (samplePointInfo_ != null)
		{
			num ^= SamplePointInfo.GetHashCode();
		}
		if (GatherUid.Length != 0)
		{
			num ^= GatherUid.GetHashCode();
		}
		if (GatherServerId != 0)
		{
			num ^= GatherServerId.GetHashCode();
		}
		if (GatherAllianceId.Length != 0)
		{
			num ^= GatherAllianceId.GetHashCode();
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
		if (ResourceId != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(ResourceId);
		}
		if (State != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(State);
		}
		if (GatherUuid != 0L)
		{
			output.WriteRawTag(24);
			output.WriteInt64(GatherUuid);
		}
		if (SpecialType != SpecialType.None)
		{
			output.WriteRawTag(48);
			output.WriteEnum((int)SpecialType);
		}
		if (samplePointInfo_ != null)
		{
			output.WriteRawTag(58);
			output.WriteMessage(SamplePointInfo);
		}
		if (GatherUid.Length != 0)
		{
			output.WriteRawTag(66);
			output.WriteString(GatherUid);
		}
		if (GatherServerId != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(GatherServerId);
		}
		if (GatherAllianceId.Length != 0)
		{
			output.WriteRawTag(82);
			output.WriteString(GatherAllianceId);
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
		if (ResourceId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ResourceId);
		}
		if (State != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(State);
		}
		if (GatherUuid != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(GatherUuid);
		}
		if (SpecialType != SpecialType.None)
		{
			num += 1 + CodedOutputStream.ComputeEnumSize((int)SpecialType);
		}
		if (samplePointInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SamplePointInfo);
		}
		if (GatherUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GatherUid);
		}
		if (GatherServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(GatherServerId);
		}
		if (GatherAllianceId.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(GatherAllianceId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ResourceInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.ResourceId != 0)
		{
			ResourceId = other.ResourceId;
		}
		if (other.State != 0)
		{
			State = other.State;
		}
		if (other.GatherUuid != 0L)
		{
			GatherUuid = other.GatherUuid;
		}
		if (other.SpecialType != SpecialType.None)
		{
			SpecialType = other.SpecialType;
		}
		if (other.samplePointInfo_ != null)
		{
			if (samplePointInfo_ == null)
			{
				SamplePointInfo = new SamplePointInfo();
			}
			SamplePointInfo.MergeFrom(other.SamplePointInfo);
		}
		if (other.GatherUid.Length != 0)
		{
			GatherUid = other.GatherUid;
		}
		if (other.GatherServerId != 0)
		{
			GatherServerId = other.GatherServerId;
		}
		if (other.GatherAllianceId.Length != 0)
		{
			GatherAllianceId = other.GatherAllianceId;
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
			case 8u:
				ResourceId = input.ReadInt32();
				break;
			case 16u:
				State = input.ReadInt32();
				break;
			case 24u:
				GatherUuid = input.ReadInt64();
				break;
			case 48u:
				SpecialType = (SpecialType)input.ReadEnum();
				break;
			case 58u:
				if (samplePointInfo_ == null)
				{
					SamplePointInfo = new SamplePointInfo();
				}
				input.ReadMessage(SamplePointInfo);
				break;
			case 66u:
				GatherUid = input.ReadString();
				break;
			case 72u:
				GatherServerId = input.ReadInt32();
				break;
			case 82u:
				GatherAllianceId = input.ReadString();
				break;
			}
		}
	}
}
