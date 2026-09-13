using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class AllianceInviteMove : IMessage<AllianceInviteMove>, IMessage, IEquatable<AllianceInviteMove>, IDeepCloneable<AllianceInviteMove>
{
	private static readonly MessageParser<AllianceInviteMove> _parser = new MessageParser<AllianceInviteMove>(() => new AllianceInviteMove());

	private UnknownFieldSet _unknownFields;

	public const int InviteeUidFieldNumber = 1;

	private string inviteeUid_ = "";

	public const int InviteeNameFieldNumber = 2;

	private string inviteeName_ = "";

	public const int InviterUidFieldNumber = 3;

	private string inviterUid_ = "";

	public const int InviterNameFieldNumber = 4;

	private string inviterName_ = "";

	public const int TargetPointFieldNumber = 5;

	private static readonly FieldCodec<int?> _single_targetPoint_codec = FieldCodec.ForStructWrapper<int>(42u);

	private int? targetPoint_;

	public const int ServerIdFieldNumber = 6;

	private static readonly FieldCodec<int?> _single_serverId_codec = FieldCodec.ForStructWrapper<int>(50u);

	private int? serverId_;

	[DebuggerNonUserCode]
	public static MessageParser<AllianceInviteMove> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[33];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string InviteeUid
	{
		get
		{
			return inviteeUid_;
		}
		set
		{
			inviteeUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string InviteeName
	{
		get
		{
			return inviteeName_;
		}
		set
		{
			inviteeName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string InviterUid
	{
		get
		{
			return inviterUid_;
		}
		set
		{
			inviterUid_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string InviterName
	{
		get
		{
			return inviterName_;
		}
		set
		{
			inviterName_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int? TargetPoint
	{
		get
		{
			return targetPoint_;
		}
		set
		{
			targetPoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int? ServerId
	{
		get
		{
			return serverId_;
		}
		set
		{
			serverId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public AllianceInviteMove()
	{
	}

	[DebuggerNonUserCode]
	public AllianceInviteMove(AllianceInviteMove other)
		: this()
	{
		inviteeUid_ = other.inviteeUid_;
		inviteeName_ = other.inviteeName_;
		inviterUid_ = other.inviterUid_;
		inviterName_ = other.inviterName_;
		TargetPoint = other.TargetPoint;
		ServerId = other.ServerId;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public AllianceInviteMove Clone()
	{
		return new AllianceInviteMove(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as AllianceInviteMove);
	}

	[DebuggerNonUserCode]
	public bool Equals(AllianceInviteMove other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (InviteeUid != other.InviteeUid)
		{
			return false;
		}
		if (InviteeName != other.InviteeName)
		{
			return false;
		}
		if (InviterUid != other.InviterUid)
		{
			return false;
		}
		if (InviterName != other.InviterName)
		{
			return false;
		}
		if (TargetPoint != other.TargetPoint)
		{
			return false;
		}
		if (ServerId != other.ServerId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (InviteeUid.Length != 0)
		{
			num ^= InviteeUid.GetHashCode();
		}
		if (InviteeName.Length != 0)
		{
			num ^= InviteeName.GetHashCode();
		}
		if (InviterUid.Length != 0)
		{
			num ^= InviterUid.GetHashCode();
		}
		if (InviterName.Length != 0)
		{
			num ^= InviterName.GetHashCode();
		}
		if (targetPoint_.HasValue)
		{
			num ^= TargetPoint.GetHashCode();
		}
		if (serverId_.HasValue)
		{
			num ^= ServerId.GetHashCode();
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
		if (InviteeUid.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(InviteeUid);
		}
		if (InviteeName.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(InviteeName);
		}
		if (InviterUid.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(InviterUid);
		}
		if (InviterName.Length != 0)
		{
			output.WriteRawTag(34);
			output.WriteString(InviterName);
		}
		if (targetPoint_.HasValue)
		{
			_single_targetPoint_codec.WriteTagAndValue(output, TargetPoint);
		}
		if (serverId_.HasValue)
		{
			_single_serverId_codec.WriteTagAndValue(output, ServerId);
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
		if (InviteeUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(InviteeUid);
		}
		if (InviteeName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(InviteeName);
		}
		if (InviterUid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(InviterUid);
		}
		if (InviterName.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(InviterName);
		}
		if (targetPoint_.HasValue)
		{
			num += _single_targetPoint_codec.CalculateSizeWithTag(TargetPoint);
		}
		if (serverId_.HasValue)
		{
			num += _single_serverId_codec.CalculateSizeWithTag(ServerId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(AllianceInviteMove other)
	{
		if (other != null)
		{
			if (other.InviteeUid.Length != 0)
			{
				InviteeUid = other.InviteeUid;
			}
			if (other.InviteeName.Length != 0)
			{
				InviteeName = other.InviteeName;
			}
			if (other.InviterUid.Length != 0)
			{
				InviterUid = other.InviterUid;
			}
			if (other.InviterName.Length != 0)
			{
				InviterName = other.InviterName;
			}
			if (other.targetPoint_.HasValue && (!targetPoint_.HasValue || other.TargetPoint != 0))
			{
				TargetPoint = other.TargetPoint;
			}
			if (other.serverId_.HasValue && (!serverId_.HasValue || other.ServerId != 0))
			{
				ServerId = other.ServerId;
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
				InviteeUid = input.ReadString();
				break;
			case 18u:
				InviteeName = input.ReadString();
				break;
			case 26u:
				InviterUid = input.ReadString();
				break;
			case 34u:
				InviterName = input.ReadString();
				break;
			case 42u:
			{
				int? num3 = _single_targetPoint_codec.Read(input);
				if (!targetPoint_.HasValue || num3 != 0)
				{
					TargetPoint = num3;
				}
				break;
			}
			case 50u:
			{
				int? num2 = _single_serverId_codec.Read(input);
				if (!serverId_.HasValue || num2 != 0)
				{
					ServerId = num2;
				}
				break;
			}
			}
		}
	}
}
