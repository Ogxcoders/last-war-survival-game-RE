using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutUser : IMessage<ScoutUser>, IMessage, IEquatable<ScoutUser>, IDeepCloneable<ScoutUser>
{
	private static readonly MessageParser<ScoutUser> _parser = new MessageParser<ScoutUser>(() => new ScoutUser());

	private UnknownFieldSet _unknownFields;

	public const int AbbrFieldNumber = 1;

	private string abbr_ = "";

	public const int NameFieldNumber = 2;

	private string name_ = "";

	public const int PicFieldNumber = 3;

	private string pic_ = "";

	public const int PicVerFieldNumber = 4;

	private static readonly FieldCodec<int?> _single_picVer_codec = FieldCodec.ForStructWrapper<int>(34u);

	private int? picVer_;

	public const int PointFieldNumber = 5;

	private PointInfo point_;

	public const int UidFieldNumber = 6;

	private string uid_ = "";

	public const int CareerTypeFieldNumber = 7;

	private int careerType_;

	public const int CareerLvFieldNumber = 8;

	private int careerLv_;

	public const int LevelFieldNumber = 9;

	private int level_;

	public const int SourceServerIdFieldNumber = 10;

	private int sourceServerId_;

	public const int WolfEndTimeFieldNumber = 11;

	private long wolfEndTime_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutUser> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[1];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public string Abbr
	{
		get
		{
			return abbr_;
		}
		set
		{
			abbr_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Name
	{
		get
		{
			return name_;
		}
		set
		{
			name_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public string Pic
	{
		get
		{
			return pic_;
		}
		set
		{
			pic_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int? PicVer
	{
		get
		{
			return picVer_;
		}
		set
		{
			picVer_ = value;
		}
	}

	[DebuggerNonUserCode]
	public PointInfo Point
	{
		get
		{
			return point_;
		}
		set
		{
			point_ = value;
		}
	}

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
	public int CareerType
	{
		get
		{
			return careerType_;
		}
		set
		{
			careerType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int CareerLv
	{
		get
		{
			return careerLv_;
		}
		set
		{
			careerLv_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Level
	{
		get
		{
			return level_;
		}
		set
		{
			level_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int SourceServerId
	{
		get
		{
			return sourceServerId_;
		}
		set
		{
			sourceServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long WolfEndTime
	{
		get
		{
			return wolfEndTime_;
		}
		set
		{
			wolfEndTime_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutUser()
	{
	}

	[DebuggerNonUserCode]
	public ScoutUser(ScoutUser other)
		: this()
	{
		abbr_ = other.abbr_;
		name_ = other.name_;
		pic_ = other.pic_;
		PicVer = other.PicVer;
		point_ = ((other.point_ != null) ? other.point_.Clone() : null);
		uid_ = other.uid_;
		careerType_ = other.careerType_;
		careerLv_ = other.careerLv_;
		level_ = other.level_;
		sourceServerId_ = other.sourceServerId_;
		wolfEndTime_ = other.wolfEndTime_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutUser Clone()
	{
		return new ScoutUser(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutUser);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutUser other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Abbr != other.Abbr)
		{
			return false;
		}
		if (Name != other.Name)
		{
			return false;
		}
		if (Pic != other.Pic)
		{
			return false;
		}
		if (PicVer != other.PicVer)
		{
			return false;
		}
		if (!object.Equals(Point, other.Point))
		{
			return false;
		}
		if (Uid != other.Uid)
		{
			return false;
		}
		if (CareerType != other.CareerType)
		{
			return false;
		}
		if (CareerLv != other.CareerLv)
		{
			return false;
		}
		if (Level != other.Level)
		{
			return false;
		}
		if (SourceServerId != other.SourceServerId)
		{
			return false;
		}
		if (WolfEndTime != other.WolfEndTime)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Abbr.Length != 0)
		{
			num ^= Abbr.GetHashCode();
		}
		if (Name.Length != 0)
		{
			num ^= Name.GetHashCode();
		}
		if (Pic.Length != 0)
		{
			num ^= Pic.GetHashCode();
		}
		if (picVer_.HasValue)
		{
			num ^= PicVer.GetHashCode();
		}
		if (point_ != null)
		{
			num ^= Point.GetHashCode();
		}
		if (Uid.Length != 0)
		{
			num ^= Uid.GetHashCode();
		}
		if (CareerType != 0)
		{
			num ^= CareerType.GetHashCode();
		}
		if (CareerLv != 0)
		{
			num ^= CareerLv.GetHashCode();
		}
		if (Level != 0)
		{
			num ^= Level.GetHashCode();
		}
		if (SourceServerId != 0)
		{
			num ^= SourceServerId.GetHashCode();
		}
		if (WolfEndTime != 0L)
		{
			num ^= WolfEndTime.GetHashCode();
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
		if (Abbr.Length != 0)
		{
			output.WriteRawTag(10);
			output.WriteString(Abbr);
		}
		if (Name.Length != 0)
		{
			output.WriteRawTag(18);
			output.WriteString(Name);
		}
		if (Pic.Length != 0)
		{
			output.WriteRawTag(26);
			output.WriteString(Pic);
		}
		if (picVer_.HasValue)
		{
			_single_picVer_codec.WriteTagAndValue(output, PicVer);
		}
		if (point_ != null)
		{
			output.WriteRawTag(42);
			output.WriteMessage(Point);
		}
		if (Uid.Length != 0)
		{
			output.WriteRawTag(50);
			output.WriteString(Uid);
		}
		if (CareerType != 0)
		{
			output.WriteRawTag(56);
			output.WriteInt32(CareerType);
		}
		if (CareerLv != 0)
		{
			output.WriteRawTag(64);
			output.WriteInt32(CareerLv);
		}
		if (Level != 0)
		{
			output.WriteRawTag(72);
			output.WriteInt32(Level);
		}
		if (SourceServerId != 0)
		{
			output.WriteRawTag(80);
			output.WriteInt32(SourceServerId);
		}
		if (WolfEndTime != 0L)
		{
			output.WriteRawTag(88);
			output.WriteInt64(WolfEndTime);
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
		if (Abbr.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Abbr);
		}
		if (Name.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Name);
		}
		if (Pic.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Pic);
		}
		if (picVer_.HasValue)
		{
			num += _single_picVer_codec.CalculateSizeWithTag(PicVer);
		}
		if (point_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Point);
		}
		if (Uid.Length != 0)
		{
			num += 1 + CodedOutputStream.ComputeStringSize(Uid);
		}
		if (CareerType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CareerType);
		}
		if (CareerLv != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(CareerLv);
		}
		if (Level != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Level);
		}
		if (SourceServerId != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(SourceServerId);
		}
		if (WolfEndTime != 0L)
		{
			num += 1 + CodedOutputStream.ComputeInt64Size(WolfEndTime);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(ScoutUser other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Abbr.Length != 0)
		{
			Abbr = other.Abbr;
		}
		if (other.Name.Length != 0)
		{
			Name = other.Name;
		}
		if (other.Pic.Length != 0)
		{
			Pic = other.Pic;
		}
		if (other.picVer_.HasValue && (!picVer_.HasValue || other.PicVer != 0))
		{
			PicVer = other.PicVer;
		}
		if (other.point_ != null)
		{
			if (point_ == null)
			{
				Point = new PointInfo();
			}
			Point.MergeFrom(other.Point);
		}
		if (other.Uid.Length != 0)
		{
			Uid = other.Uid;
		}
		if (other.CareerType != 0)
		{
			CareerType = other.CareerType;
		}
		if (other.CareerLv != 0)
		{
			CareerLv = other.CareerLv;
		}
		if (other.Level != 0)
		{
			Level = other.Level;
		}
		if (other.SourceServerId != 0)
		{
			SourceServerId = other.SourceServerId;
		}
		if (other.WolfEndTime != 0L)
		{
			WolfEndTime = other.WolfEndTime;
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
				Abbr = input.ReadString();
				break;
			case 18u:
				Name = input.ReadString();
				break;
			case 26u:
				Pic = input.ReadString();
				break;
			case 34u:
			{
				int? num2 = _single_picVer_codec.Read(input);
				if (!picVer_.HasValue || num2 != 0)
				{
					PicVer = num2;
				}
				break;
			}
			case 42u:
				if (point_ == null)
				{
					Point = new PointInfo();
				}
				input.ReadMessage(Point);
				break;
			case 50u:
				Uid = input.ReadString();
				break;
			case 56u:
				CareerType = input.ReadInt32();
				break;
			case 64u:
				CareerLv = input.ReadInt32();
				break;
			case 72u:
				Level = input.ReadInt32();
				break;
			case 80u:
				SourceServerId = input.ReadInt32();
				break;
			case 88u:
				WolfEndTime = input.ReadInt64();
				break;
			}
		}
	}
}
