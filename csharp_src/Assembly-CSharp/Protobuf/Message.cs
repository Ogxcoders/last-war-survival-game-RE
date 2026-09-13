using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class Message : IMessage<Message>, IMessage, IEquatable<Message>, IDeepCloneable<Message>
{
	public enum DataOneofCase
	{
		None = 0,
		Text = 101,
		Dialog = 102,
		Point = 103,
		User = 104,
		Alliance = 105,
		Monster = 106,
		Percent = 107,
		Scout = 108,
		DateTime = 109,
		WorldAllianceCity = 110,
		Url = 111,
		TextWithParams = 112,
		IntegerWithSpearated = 113,
		MinePlunderInfo = 114,
		Effect = 115,
		WorldResource = 116,
		WorldAllianceBuild = 117,
		WorldUserBuild = 118,
		DragonBuilding = 119,
		WorldDesert = 120,
		WinterStormBuilding = 121,
		WorldCityStronghold = 122,
		WorldCityAttachment = 123,
		QuarantineBuilding = 124
	}

	private static readonly MessageParser<Message> _parser = new MessageParser<Message>(() => new Message());

	private UnknownFieldSet _unknownFields;

	public const int ColorFieldNumber = 1;

	private DialogColor color_;

	public const int BoldFieldNumber = 2;

	private bool bold_;

	public const int TextFieldNumber = 101;

	public const int DialogFieldNumber = 102;

	public const int PointFieldNumber = 103;

	public const int UserFieldNumber = 104;

	public const int AllianceFieldNumber = 105;

	public const int MonsterFieldNumber = 106;

	public const int PercentFieldNumber = 107;

	public const int ScoutFieldNumber = 108;

	public const int DateTimeFieldNumber = 109;

	public const int WorldAllianceCityFieldNumber = 110;

	public const int UrlFieldNumber = 111;

	public const int TextWithParamsFieldNumber = 112;

	public const int IntegerWithSpearatedFieldNumber = 113;

	public const int MinePlunderInfoFieldNumber = 114;

	public const int EffectFieldNumber = 115;

	public const int WorldResourceFieldNumber = 116;

	public const int WorldAllianceBuildFieldNumber = 117;

	public const int WorldUserBuildFieldNumber = 118;

	public const int DragonBuildingFieldNumber = 119;

	public const int WorldDesertFieldNumber = 120;

	public const int WinterStormBuildingFieldNumber = 121;

	public const int WorldCityStrongholdFieldNumber = 122;

	public const int WorldCityAttachmentFieldNumber = 123;

	public const int QuarantineBuildingFieldNumber = 124;

	private object data_;

	private DataOneofCase dataCase_;

	[DebuggerNonUserCode]
	public static MessageParser<Message> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => MailReflection.Descriptor.MessageTypes[4];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public DialogColor Color
	{
		get
		{
			return color_;
		}
		set
		{
			color_ = value;
		}
	}

	[DebuggerNonUserCode]
	public bool Bold
	{
		get
		{
			return bold_;
		}
		set
		{
			bold_ = value;
		}
	}

	[DebuggerNonUserCode]
	public string Text
	{
		get
		{
			if (dataCase_ != DataOneofCase.Text)
			{
				return "";
			}
			return (string)data_;
		}
		set
		{
			data_ = ProtoPreconditions.CheckNotNull(value, "value");
			dataCase_ = DataOneofCase.Text;
		}
	}

	[DebuggerNonUserCode]
	public Dialog Dialog
	{
		get
		{
			if (dataCase_ != DataOneofCase.Dialog)
			{
				return null;
			}
			return (Dialog)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.Dialog : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public PointInfo Point
	{
		get
		{
			if (dataCase_ != DataOneofCase.Point)
			{
				return null;
			}
			return (PointInfo)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.Point : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public User User
	{
		get
		{
			if (dataCase_ != DataOneofCase.User)
			{
				return null;
			}
			return (User)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.User : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public Alliance Alliance
	{
		get
		{
			if (dataCase_ != DataOneofCase.Alliance)
			{
				return null;
			}
			return (Alliance)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.Alliance : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public Monster Monster
	{
		get
		{
			if (dataCase_ != DataOneofCase.Monster)
			{
				return null;
			}
			return (Monster)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.Monster : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public Percent Percent
	{
		get
		{
			if (dataCase_ != DataOneofCase.Percent)
			{
				return null;
			}
			return (Percent)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.Percent : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public Scout Scout
	{
		get
		{
			if (dataCase_ != DataOneofCase.Scout)
			{
				return null;
			}
			return (Scout)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.Scout : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public DateTime DateTime
	{
		get
		{
			if (dataCase_ != DataOneofCase.DateTime)
			{
				return null;
			}
			return (DateTime)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.DateTime : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public WorldAllianceCity WorldAllianceCity
	{
		get
		{
			if (dataCase_ != DataOneofCase.WorldAllianceCity)
			{
				return null;
			}
			return (WorldAllianceCity)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.WorldAllianceCity : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public Url Url
	{
		get
		{
			if (dataCase_ != DataOneofCase.Url)
			{
				return null;
			}
			return (Url)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.Url : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public TextWithParams TextWithParams
	{
		get
		{
			if (dataCase_ != DataOneofCase.TextWithParams)
			{
				return null;
			}
			return (TextWithParams)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.TextWithParams : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public IntegerWithSpearated IntegerWithSpearated
	{
		get
		{
			if (dataCase_ != DataOneofCase.IntegerWithSpearated)
			{
				return null;
			}
			return (IntegerWithSpearated)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.IntegerWithSpearated : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public MinePlunderInfo MinePlunderInfo
	{
		get
		{
			if (dataCase_ != DataOneofCase.MinePlunderInfo)
			{
				return null;
			}
			return (MinePlunderInfo)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.MinePlunderInfo : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public EffectInfo Effect
	{
		get
		{
			if (dataCase_ != DataOneofCase.Effect)
			{
				return null;
			}
			return (EffectInfo)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.Effect : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public WorldResource WorldResource
	{
		get
		{
			if (dataCase_ != DataOneofCase.WorldResource)
			{
				return null;
			}
			return (WorldResource)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.WorldResource : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public WorldAllianceBuild WorldAllianceBuild
	{
		get
		{
			if (dataCase_ != DataOneofCase.WorldAllianceBuild)
			{
				return null;
			}
			return (WorldAllianceBuild)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.WorldAllianceBuild : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public WorldUserBuild WorldUserBuild
	{
		get
		{
			if (dataCase_ != DataOneofCase.WorldUserBuild)
			{
				return null;
			}
			return (WorldUserBuild)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.WorldUserBuild : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public DragonBuilding DragonBuilding
	{
		get
		{
			if (dataCase_ != DataOneofCase.DragonBuilding)
			{
				return null;
			}
			return (DragonBuilding)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.DragonBuilding : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public WorldDesert WorldDesert
	{
		get
		{
			if (dataCase_ != DataOneofCase.WorldDesert)
			{
				return null;
			}
			return (WorldDesert)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.WorldDesert : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public WinterStormBuilding WinterStormBuilding
	{
		get
		{
			if (dataCase_ != DataOneofCase.WinterStormBuilding)
			{
				return null;
			}
			return (WinterStormBuilding)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.WinterStormBuilding : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public WorldCityStronghold WorldCityStronghold
	{
		get
		{
			if (dataCase_ != DataOneofCase.WorldCityStronghold)
			{
				return null;
			}
			return (WorldCityStronghold)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.WorldCityStronghold : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public WorldCityAttachment WorldCityAttachment
	{
		get
		{
			if (dataCase_ != DataOneofCase.WorldCityAttachment)
			{
				return null;
			}
			return (WorldCityAttachment)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.WorldCityAttachment : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public QuarantineBuilding QuarantineBuilding
	{
		get
		{
			if (dataCase_ != DataOneofCase.QuarantineBuilding)
			{
				return null;
			}
			return (QuarantineBuilding)data_;
		}
		set
		{
			data_ = value;
			dataCase_ = ((value != null) ? DataOneofCase.QuarantineBuilding : DataOneofCase.None);
		}
	}

	[DebuggerNonUserCode]
	public DataOneofCase DataCase => dataCase_;

	[DebuggerNonUserCode]
	public Message()
	{
	}

	[DebuggerNonUserCode]
	public Message(Message other)
		: this()
	{
		color_ = other.color_;
		bold_ = other.bold_;
		switch (other.DataCase)
		{
		case DataOneofCase.Text:
			Text = other.Text;
			break;
		case DataOneofCase.Dialog:
			Dialog = other.Dialog.Clone();
			break;
		case DataOneofCase.Point:
			Point = other.Point.Clone();
			break;
		case DataOneofCase.User:
			User = other.User.Clone();
			break;
		case DataOneofCase.Alliance:
			Alliance = other.Alliance.Clone();
			break;
		case DataOneofCase.Monster:
			Monster = other.Monster.Clone();
			break;
		case DataOneofCase.Percent:
			Percent = other.Percent.Clone();
			break;
		case DataOneofCase.Scout:
			Scout = other.Scout.Clone();
			break;
		case DataOneofCase.DateTime:
			DateTime = other.DateTime.Clone();
			break;
		case DataOneofCase.WorldAllianceCity:
			WorldAllianceCity = other.WorldAllianceCity.Clone();
			break;
		case DataOneofCase.Url:
			Url = other.Url.Clone();
			break;
		case DataOneofCase.TextWithParams:
			TextWithParams = other.TextWithParams.Clone();
			break;
		case DataOneofCase.IntegerWithSpearated:
			IntegerWithSpearated = other.IntegerWithSpearated.Clone();
			break;
		case DataOneofCase.MinePlunderInfo:
			MinePlunderInfo = other.MinePlunderInfo.Clone();
			break;
		case DataOneofCase.Effect:
			Effect = other.Effect.Clone();
			break;
		case DataOneofCase.WorldResource:
			WorldResource = other.WorldResource.Clone();
			break;
		case DataOneofCase.WorldAllianceBuild:
			WorldAllianceBuild = other.WorldAllianceBuild.Clone();
			break;
		case DataOneofCase.WorldUserBuild:
			WorldUserBuild = other.WorldUserBuild.Clone();
			break;
		case DataOneofCase.DragonBuilding:
			DragonBuilding = other.DragonBuilding.Clone();
			break;
		case DataOneofCase.WorldDesert:
			WorldDesert = other.WorldDesert.Clone();
			break;
		case DataOneofCase.WinterStormBuilding:
			WinterStormBuilding = other.WinterStormBuilding.Clone();
			break;
		case DataOneofCase.WorldCityStronghold:
			WorldCityStronghold = other.WorldCityStronghold.Clone();
			break;
		case DataOneofCase.WorldCityAttachment:
			WorldCityAttachment = other.WorldCityAttachment.Clone();
			break;
		case DataOneofCase.QuarantineBuilding:
			QuarantineBuilding = other.QuarantineBuilding.Clone();
			break;
		}
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public Message Clone()
	{
		return new Message(this);
	}

	[DebuggerNonUserCode]
	public void ClearData()
	{
		dataCase_ = DataOneofCase.None;
		data_ = null;
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as Message);
	}

	[DebuggerNonUserCode]
	public bool Equals(Message other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Color != other.Color)
		{
			return false;
		}
		if (Bold != other.Bold)
		{
			return false;
		}
		if (Text != other.Text)
		{
			return false;
		}
		if (!object.Equals(Dialog, other.Dialog))
		{
			return false;
		}
		if (!object.Equals(Point, other.Point))
		{
			return false;
		}
		if (!object.Equals(User, other.User))
		{
			return false;
		}
		if (!object.Equals(Alliance, other.Alliance))
		{
			return false;
		}
		if (!object.Equals(Monster, other.Monster))
		{
			return false;
		}
		if (!object.Equals(Percent, other.Percent))
		{
			return false;
		}
		if (!object.Equals(Scout, other.Scout))
		{
			return false;
		}
		if (!object.Equals(DateTime, other.DateTime))
		{
			return false;
		}
		if (!object.Equals(WorldAllianceCity, other.WorldAllianceCity))
		{
			return false;
		}
		if (!object.Equals(Url, other.Url))
		{
			return false;
		}
		if (!object.Equals(TextWithParams, other.TextWithParams))
		{
			return false;
		}
		if (!object.Equals(IntegerWithSpearated, other.IntegerWithSpearated))
		{
			return false;
		}
		if (!object.Equals(MinePlunderInfo, other.MinePlunderInfo))
		{
			return false;
		}
		if (!object.Equals(Effect, other.Effect))
		{
			return false;
		}
		if (!object.Equals(WorldResource, other.WorldResource))
		{
			return false;
		}
		if (!object.Equals(WorldAllianceBuild, other.WorldAllianceBuild))
		{
			return false;
		}
		if (!object.Equals(WorldUserBuild, other.WorldUserBuild))
		{
			return false;
		}
		if (!object.Equals(DragonBuilding, other.DragonBuilding))
		{
			return false;
		}
		if (!object.Equals(WorldDesert, other.WorldDesert))
		{
			return false;
		}
		if (!object.Equals(WinterStormBuilding, other.WinterStormBuilding))
		{
			return false;
		}
		if (!object.Equals(WorldCityStronghold, other.WorldCityStronghold))
		{
			return false;
		}
		if (!object.Equals(WorldCityAttachment, other.WorldCityAttachment))
		{
			return false;
		}
		if (!object.Equals(QuarantineBuilding, other.QuarantineBuilding))
		{
			return false;
		}
		if (DataCase != other.DataCase)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Color != DialogColor.DefaultColor)
		{
			num ^= Color.GetHashCode();
		}
		if (Bold)
		{
			num ^= Bold.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.Text)
		{
			num ^= Text.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.Dialog)
		{
			num ^= Dialog.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.Point)
		{
			num ^= Point.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.User)
		{
			num ^= User.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.Alliance)
		{
			num ^= Alliance.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.Monster)
		{
			num ^= Monster.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.Percent)
		{
			num ^= Percent.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.Scout)
		{
			num ^= Scout.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.DateTime)
		{
			num ^= DateTime.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.WorldAllianceCity)
		{
			num ^= WorldAllianceCity.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.Url)
		{
			num ^= Url.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.TextWithParams)
		{
			num ^= TextWithParams.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.IntegerWithSpearated)
		{
			num ^= IntegerWithSpearated.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.MinePlunderInfo)
		{
			num ^= MinePlunderInfo.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.Effect)
		{
			num ^= Effect.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.WorldResource)
		{
			num ^= WorldResource.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.WorldAllianceBuild)
		{
			num ^= WorldAllianceBuild.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.WorldUserBuild)
		{
			num ^= WorldUserBuild.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.DragonBuilding)
		{
			num ^= DragonBuilding.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.WorldDesert)
		{
			num ^= WorldDesert.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.WinterStormBuilding)
		{
			num ^= WinterStormBuilding.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.WorldCityStronghold)
		{
			num ^= WorldCityStronghold.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.WorldCityAttachment)
		{
			num ^= WorldCityAttachment.GetHashCode();
		}
		if (dataCase_ == DataOneofCase.QuarantineBuilding)
		{
			num ^= QuarantineBuilding.GetHashCode();
		}
		num ^= (int)dataCase_;
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
		if (Color != DialogColor.DefaultColor)
		{
			output.WriteRawTag(8);
			output.WriteEnum((int)Color);
		}
		if (Bold)
		{
			output.WriteRawTag(16);
			output.WriteBool(Bold);
		}
		if (dataCase_ == DataOneofCase.Text)
		{
			output.WriteRawTag(170, 6);
			output.WriteString(Text);
		}
		if (dataCase_ == DataOneofCase.Dialog)
		{
			output.WriteRawTag(178, 6);
			output.WriteMessage(Dialog);
		}
		if (dataCase_ == DataOneofCase.Point)
		{
			output.WriteRawTag(186, 6);
			output.WriteMessage(Point);
		}
		if (dataCase_ == DataOneofCase.User)
		{
			output.WriteRawTag(194, 6);
			output.WriteMessage(User);
		}
		if (dataCase_ == DataOneofCase.Alliance)
		{
			output.WriteRawTag(202, 6);
			output.WriteMessage(Alliance);
		}
		if (dataCase_ == DataOneofCase.Monster)
		{
			output.WriteRawTag(210, 6);
			output.WriteMessage(Monster);
		}
		if (dataCase_ == DataOneofCase.Percent)
		{
			output.WriteRawTag(218, 6);
			output.WriteMessage(Percent);
		}
		if (dataCase_ == DataOneofCase.Scout)
		{
			output.WriteRawTag(226, 6);
			output.WriteMessage(Scout);
		}
		if (dataCase_ == DataOneofCase.DateTime)
		{
			output.WriteRawTag(234, 6);
			output.WriteMessage(DateTime);
		}
		if (dataCase_ == DataOneofCase.WorldAllianceCity)
		{
			output.WriteRawTag(242, 6);
			output.WriteMessage(WorldAllianceCity);
		}
		if (dataCase_ == DataOneofCase.Url)
		{
			output.WriteRawTag(250, 6);
			output.WriteMessage(Url);
		}
		if (dataCase_ == DataOneofCase.TextWithParams)
		{
			output.WriteRawTag(130, 7);
			output.WriteMessage(TextWithParams);
		}
		if (dataCase_ == DataOneofCase.IntegerWithSpearated)
		{
			output.WriteRawTag(138, 7);
			output.WriteMessage(IntegerWithSpearated);
		}
		if (dataCase_ == DataOneofCase.MinePlunderInfo)
		{
			output.WriteRawTag(146, 7);
			output.WriteMessage(MinePlunderInfo);
		}
		if (dataCase_ == DataOneofCase.Effect)
		{
			output.WriteRawTag(154, 7);
			output.WriteMessage(Effect);
		}
		if (dataCase_ == DataOneofCase.WorldResource)
		{
			output.WriteRawTag(162, 7);
			output.WriteMessage(WorldResource);
		}
		if (dataCase_ == DataOneofCase.WorldAllianceBuild)
		{
			output.WriteRawTag(170, 7);
			output.WriteMessage(WorldAllianceBuild);
		}
		if (dataCase_ == DataOneofCase.WorldUserBuild)
		{
			output.WriteRawTag(178, 7);
			output.WriteMessage(WorldUserBuild);
		}
		if (dataCase_ == DataOneofCase.DragonBuilding)
		{
			output.WriteRawTag(186, 7);
			output.WriteMessage(DragonBuilding);
		}
		if (dataCase_ == DataOneofCase.WorldDesert)
		{
			output.WriteRawTag(194, 7);
			output.WriteMessage(WorldDesert);
		}
		if (dataCase_ == DataOneofCase.WinterStormBuilding)
		{
			output.WriteRawTag(202, 7);
			output.WriteMessage(WinterStormBuilding);
		}
		if (dataCase_ == DataOneofCase.WorldCityStronghold)
		{
			output.WriteRawTag(210, 7);
			output.WriteMessage(WorldCityStronghold);
		}
		if (dataCase_ == DataOneofCase.WorldCityAttachment)
		{
			output.WriteRawTag(218, 7);
			output.WriteMessage(WorldCityAttachment);
		}
		if (dataCase_ == DataOneofCase.QuarantineBuilding)
		{
			output.WriteRawTag(226, 7);
			output.WriteMessage(QuarantineBuilding);
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
		if (Color != DialogColor.DefaultColor)
		{
			num += 1 + CodedOutputStream.ComputeEnumSize((int)Color);
		}
		if (Bold)
		{
			num += 2;
		}
		if (dataCase_ == DataOneofCase.Text)
		{
			num += 2 + CodedOutputStream.ComputeStringSize(Text);
		}
		if (dataCase_ == DataOneofCase.Dialog)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Dialog);
		}
		if (dataCase_ == DataOneofCase.Point)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Point);
		}
		if (dataCase_ == DataOneofCase.User)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(User);
		}
		if (dataCase_ == DataOneofCase.Alliance)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Alliance);
		}
		if (dataCase_ == DataOneofCase.Monster)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Monster);
		}
		if (dataCase_ == DataOneofCase.Percent)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Percent);
		}
		if (dataCase_ == DataOneofCase.Scout)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Scout);
		}
		if (dataCase_ == DataOneofCase.DateTime)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(DateTime);
		}
		if (dataCase_ == DataOneofCase.WorldAllianceCity)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(WorldAllianceCity);
		}
		if (dataCase_ == DataOneofCase.Url)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Url);
		}
		if (dataCase_ == DataOneofCase.TextWithParams)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(TextWithParams);
		}
		if (dataCase_ == DataOneofCase.IntegerWithSpearated)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(IntegerWithSpearated);
		}
		if (dataCase_ == DataOneofCase.MinePlunderInfo)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MinePlunderInfo);
		}
		if (dataCase_ == DataOneofCase.Effect)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(Effect);
		}
		if (dataCase_ == DataOneofCase.WorldResource)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(WorldResource);
		}
		if (dataCase_ == DataOneofCase.WorldAllianceBuild)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(WorldAllianceBuild);
		}
		if (dataCase_ == DataOneofCase.WorldUserBuild)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(WorldUserBuild);
		}
		if (dataCase_ == DataOneofCase.DragonBuilding)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(DragonBuilding);
		}
		if (dataCase_ == DataOneofCase.WorldDesert)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(WorldDesert);
		}
		if (dataCase_ == DataOneofCase.WinterStormBuilding)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(WinterStormBuilding);
		}
		if (dataCase_ == DataOneofCase.WorldCityStronghold)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(WorldCityStronghold);
		}
		if (dataCase_ == DataOneofCase.WorldCityAttachment)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(WorldCityAttachment);
		}
		if (dataCase_ == DataOneofCase.QuarantineBuilding)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(QuarantineBuilding);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(Message other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Color != DialogColor.DefaultColor)
		{
			Color = other.Color;
		}
		if (other.Bold)
		{
			Bold = other.Bold;
		}
		switch (other.DataCase)
		{
		case DataOneofCase.Text:
			Text = other.Text;
			break;
		case DataOneofCase.Dialog:
			if (Dialog == null)
			{
				Dialog = new Dialog();
			}
			Dialog.MergeFrom(other.Dialog);
			break;
		case DataOneofCase.Point:
			if (Point == null)
			{
				Point = new PointInfo();
			}
			Point.MergeFrom(other.Point);
			break;
		case DataOneofCase.User:
			if (User == null)
			{
				User = new User();
			}
			User.MergeFrom(other.User);
			break;
		case DataOneofCase.Alliance:
			if (Alliance == null)
			{
				Alliance = new Alliance();
			}
			Alliance.MergeFrom(other.Alliance);
			break;
		case DataOneofCase.Monster:
			if (Monster == null)
			{
				Monster = new Monster();
			}
			Monster.MergeFrom(other.Monster);
			break;
		case DataOneofCase.Percent:
			if (Percent == null)
			{
				Percent = new Percent();
			}
			Percent.MergeFrom(other.Percent);
			break;
		case DataOneofCase.Scout:
			if (Scout == null)
			{
				Scout = new Scout();
			}
			Scout.MergeFrom(other.Scout);
			break;
		case DataOneofCase.DateTime:
			if (DateTime == null)
			{
				DateTime = new DateTime();
			}
			DateTime.MergeFrom(other.DateTime);
			break;
		case DataOneofCase.WorldAllianceCity:
			if (WorldAllianceCity == null)
			{
				WorldAllianceCity = new WorldAllianceCity();
			}
			WorldAllianceCity.MergeFrom(other.WorldAllianceCity);
			break;
		case DataOneofCase.Url:
			if (Url == null)
			{
				Url = new Url();
			}
			Url.MergeFrom(other.Url);
			break;
		case DataOneofCase.TextWithParams:
			if (TextWithParams == null)
			{
				TextWithParams = new TextWithParams();
			}
			TextWithParams.MergeFrom(other.TextWithParams);
			break;
		case DataOneofCase.IntegerWithSpearated:
			if (IntegerWithSpearated == null)
			{
				IntegerWithSpearated = new IntegerWithSpearated();
			}
			IntegerWithSpearated.MergeFrom(other.IntegerWithSpearated);
			break;
		case DataOneofCase.MinePlunderInfo:
			if (MinePlunderInfo == null)
			{
				MinePlunderInfo = new MinePlunderInfo();
			}
			MinePlunderInfo.MergeFrom(other.MinePlunderInfo);
			break;
		case DataOneofCase.Effect:
			if (Effect == null)
			{
				Effect = new EffectInfo();
			}
			Effect.MergeFrom(other.Effect);
			break;
		case DataOneofCase.WorldResource:
			if (WorldResource == null)
			{
				WorldResource = new WorldResource();
			}
			WorldResource.MergeFrom(other.WorldResource);
			break;
		case DataOneofCase.WorldAllianceBuild:
			if (WorldAllianceBuild == null)
			{
				WorldAllianceBuild = new WorldAllianceBuild();
			}
			WorldAllianceBuild.MergeFrom(other.WorldAllianceBuild);
			break;
		case DataOneofCase.WorldUserBuild:
			if (WorldUserBuild == null)
			{
				WorldUserBuild = new WorldUserBuild();
			}
			WorldUserBuild.MergeFrom(other.WorldUserBuild);
			break;
		case DataOneofCase.DragonBuilding:
			if (DragonBuilding == null)
			{
				DragonBuilding = new DragonBuilding();
			}
			DragonBuilding.MergeFrom(other.DragonBuilding);
			break;
		case DataOneofCase.WorldDesert:
			if (WorldDesert == null)
			{
				WorldDesert = new WorldDesert();
			}
			WorldDesert.MergeFrom(other.WorldDesert);
			break;
		case DataOneofCase.WinterStormBuilding:
			if (WinterStormBuilding == null)
			{
				WinterStormBuilding = new WinterStormBuilding();
			}
			WinterStormBuilding.MergeFrom(other.WinterStormBuilding);
			break;
		case DataOneofCase.WorldCityStronghold:
			if (WorldCityStronghold == null)
			{
				WorldCityStronghold = new WorldCityStronghold();
			}
			WorldCityStronghold.MergeFrom(other.WorldCityStronghold);
			break;
		case DataOneofCase.WorldCityAttachment:
			if (WorldCityAttachment == null)
			{
				WorldCityAttachment = new WorldCityAttachment();
			}
			WorldCityAttachment.MergeFrom(other.WorldCityAttachment);
			break;
		case DataOneofCase.QuarantineBuilding:
			if (QuarantineBuilding == null)
			{
				QuarantineBuilding = new QuarantineBuilding();
			}
			QuarantineBuilding.MergeFrom(other.QuarantineBuilding);
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
			case 8u:
				Color = (DialogColor)input.ReadEnum();
				break;
			case 16u:
				Bold = input.ReadBool();
				break;
			case 810u:
				Text = input.ReadString();
				break;
			case 818u:
			{
				Dialog dialog = new Dialog();
				if (dataCase_ == DataOneofCase.Dialog)
				{
					dialog.MergeFrom(Dialog);
				}
				input.ReadMessage(dialog);
				Dialog = dialog;
				break;
			}
			case 826u:
			{
				PointInfo pointInfo = new PointInfo();
				if (dataCase_ == DataOneofCase.Point)
				{
					pointInfo.MergeFrom(Point);
				}
				input.ReadMessage(pointInfo);
				Point = pointInfo;
				break;
			}
			case 834u:
			{
				User user = new User();
				if (dataCase_ == DataOneofCase.User)
				{
					user.MergeFrom(User);
				}
				input.ReadMessage(user);
				User = user;
				break;
			}
			case 842u:
			{
				Alliance alliance = new Alliance();
				if (dataCase_ == DataOneofCase.Alliance)
				{
					alliance.MergeFrom(Alliance);
				}
				input.ReadMessage(alliance);
				Alliance = alliance;
				break;
			}
			case 850u:
			{
				Monster monster = new Monster();
				if (dataCase_ == DataOneofCase.Monster)
				{
					monster.MergeFrom(Monster);
				}
				input.ReadMessage(monster);
				Monster = monster;
				break;
			}
			case 858u:
			{
				Percent percent = new Percent();
				if (dataCase_ == DataOneofCase.Percent)
				{
					percent.MergeFrom(Percent);
				}
				input.ReadMessage(percent);
				Percent = percent;
				break;
			}
			case 866u:
			{
				Scout scout = new Scout();
				if (dataCase_ == DataOneofCase.Scout)
				{
					scout.MergeFrom(Scout);
				}
				input.ReadMessage(scout);
				Scout = scout;
				break;
			}
			case 874u:
			{
				DateTime dateTime = new DateTime();
				if (dataCase_ == DataOneofCase.DateTime)
				{
					dateTime.MergeFrom(DateTime);
				}
				input.ReadMessage(dateTime);
				DateTime = dateTime;
				break;
			}
			case 882u:
			{
				WorldAllianceCity worldAllianceCity = new WorldAllianceCity();
				if (dataCase_ == DataOneofCase.WorldAllianceCity)
				{
					worldAllianceCity.MergeFrom(WorldAllianceCity);
				}
				input.ReadMessage(worldAllianceCity);
				WorldAllianceCity = worldAllianceCity;
				break;
			}
			case 890u:
			{
				Url url = new Url();
				if (dataCase_ == DataOneofCase.Url)
				{
					url.MergeFrom(Url);
				}
				input.ReadMessage(url);
				Url = url;
				break;
			}
			case 898u:
			{
				TextWithParams textWithParams = new TextWithParams();
				if (dataCase_ == DataOneofCase.TextWithParams)
				{
					textWithParams.MergeFrom(TextWithParams);
				}
				input.ReadMessage(textWithParams);
				TextWithParams = textWithParams;
				break;
			}
			case 906u:
			{
				IntegerWithSpearated integerWithSpearated = new IntegerWithSpearated();
				if (dataCase_ == DataOneofCase.IntegerWithSpearated)
				{
					integerWithSpearated.MergeFrom(IntegerWithSpearated);
				}
				input.ReadMessage(integerWithSpearated);
				IntegerWithSpearated = integerWithSpearated;
				break;
			}
			case 914u:
			{
				MinePlunderInfo minePlunderInfo = new MinePlunderInfo();
				if (dataCase_ == DataOneofCase.MinePlunderInfo)
				{
					minePlunderInfo.MergeFrom(MinePlunderInfo);
				}
				input.ReadMessage(minePlunderInfo);
				MinePlunderInfo = minePlunderInfo;
				break;
			}
			case 922u:
			{
				EffectInfo effectInfo = new EffectInfo();
				if (dataCase_ == DataOneofCase.Effect)
				{
					effectInfo.MergeFrom(Effect);
				}
				input.ReadMessage(effectInfo);
				Effect = effectInfo;
				break;
			}
			case 930u:
			{
				WorldResource worldResource = new WorldResource();
				if (dataCase_ == DataOneofCase.WorldResource)
				{
					worldResource.MergeFrom(WorldResource);
				}
				input.ReadMessage(worldResource);
				WorldResource = worldResource;
				break;
			}
			case 938u:
			{
				WorldAllianceBuild worldAllianceBuild = new WorldAllianceBuild();
				if (dataCase_ == DataOneofCase.WorldAllianceBuild)
				{
					worldAllianceBuild.MergeFrom(WorldAllianceBuild);
				}
				input.ReadMessage(worldAllianceBuild);
				WorldAllianceBuild = worldAllianceBuild;
				break;
			}
			case 946u:
			{
				WorldUserBuild worldUserBuild = new WorldUserBuild();
				if (dataCase_ == DataOneofCase.WorldUserBuild)
				{
					worldUserBuild.MergeFrom(WorldUserBuild);
				}
				input.ReadMessage(worldUserBuild);
				WorldUserBuild = worldUserBuild;
				break;
			}
			case 954u:
			{
				DragonBuilding dragonBuilding = new DragonBuilding();
				if (dataCase_ == DataOneofCase.DragonBuilding)
				{
					dragonBuilding.MergeFrom(DragonBuilding);
				}
				input.ReadMessage(dragonBuilding);
				DragonBuilding = dragonBuilding;
				break;
			}
			case 962u:
			{
				WorldDesert worldDesert = new WorldDesert();
				if (dataCase_ == DataOneofCase.WorldDesert)
				{
					worldDesert.MergeFrom(WorldDesert);
				}
				input.ReadMessage(worldDesert);
				WorldDesert = worldDesert;
				break;
			}
			case 970u:
			{
				WinterStormBuilding winterStormBuilding = new WinterStormBuilding();
				if (dataCase_ == DataOneofCase.WinterStormBuilding)
				{
					winterStormBuilding.MergeFrom(WinterStormBuilding);
				}
				input.ReadMessage(winterStormBuilding);
				WinterStormBuilding = winterStormBuilding;
				break;
			}
			case 978u:
			{
				WorldCityStronghold worldCityStronghold = new WorldCityStronghold();
				if (dataCase_ == DataOneofCase.WorldCityStronghold)
				{
					worldCityStronghold.MergeFrom(WorldCityStronghold);
				}
				input.ReadMessage(worldCityStronghold);
				WorldCityStronghold = worldCityStronghold;
				break;
			}
			case 986u:
			{
				WorldCityAttachment worldCityAttachment = new WorldCityAttachment();
				if (dataCase_ == DataOneofCase.WorldCityAttachment)
				{
					worldCityAttachment.MergeFrom(WorldCityAttachment);
				}
				input.ReadMessage(worldCityAttachment);
				WorldCityAttachment = worldCityAttachment;
				break;
			}
			case 994u:
			{
				QuarantineBuilding quarantineBuilding = new QuarantineBuilding();
				if (dataCase_ == DataOneofCase.QuarantineBuilding)
				{
					quarantineBuilding.MergeFrom(QuarantineBuilding);
				}
				input.ReadMessage(quarantineBuilding);
				QuarantineBuilding = quarantineBuilding;
				break;
			}
			}
		}
	}
}
