using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class WorldPointInfo : IMessage<WorldPointInfo>, IMessage, IEquatable<WorldPointInfo>, IDeepCloneable<WorldPointInfo>
{
	private static readonly MessageParser<WorldPointInfo> _parser = new MessageParser<WorldPointInfo>(() => new WorldPointInfo());

	private UnknownFieldSet _unknownFields;

	public const int IdFieldNumber = 1;

	private int id_;

	public const int PointTypeFieldNumber = 2;

	private int pointType_;

	public const int BuildInfoFieldNumber = 3;

	private BuildInfo buildInfo_;

	public const int RoadInfoFieldNumber = 4;

	private RoadInfo roadInfo_;

	public const int CollectResourceInfoFieldNumber = 5;

	private CollectResourceInfo collectResourceInfo_;

	public const int ResourceInfoFieldNumber = 6;

	private ResourceInfo resourceInfo_;

	public const int ExplorePointInfoFieldNumber = 7;

	private ExplorePointInfo explorePointInfo_;

	public const int SamplePointInfoFieldNumber = 8;

	private SamplePointInfo samplePointInfo_;

	public const int GarbagePointInfoFieldNumber = 9;

	private GarbagePointInfo garbagePointInfo_;

	public const int HeroDispatchMissionPointInfoFieldNumber = 10;

	private HeroDispatchMissionPointInfo heroDispatchMissionPointInfo_;

	public const int TreasurePointInfoFieldNumber = 11;

	private TreasurePointInfo treasurePointInfo_;

	public const int AllianceCollectResInfoFieldNumber = 12;

	private WorldAllianceCollectResPointInfo allianceCollectResInfo_;

	public const int IceSuppliesPointInfoFieldNumber = 13;

	private IceSuppliesPointInfo iceSuppliesPointInfo_;

	public const int GhostReconPointInfoFieldNumber = 14;

	private GhostReconPointInfo ghostReconPointInfo_;

	public const int CityAttachmentFieldNumber = 15;

	private CityAttachment cityAttachment_;

	public const int ZoneMobilizationPointInfoFieldNumber = 16;

	private ZoneMobilizationPointInfo zoneMobilizationPointInfo_;

	public const int SurprisePointFieldNumber = 17;

	private SurprisePointInfo surprisePoint_;

	public const int MeteoritePointFieldNumber = 18;

	private MeteoritePoint meteoritePoint_;

	public const int MonsterChallengeTreasurePointInfoFieldNumber = 19;

	private MonsterChallengeTreasurePointInfo monsterChallengeTreasurePointInfo_;

	public const int ActivityTreasurePointFieldNumber = 20;

	private ActivityTreasurePointInfo activityTreasurePoint_;

	public const int QuarantinePointFieldNumber = 21;

	private QuarantinePointInfo quarantinePoint_;

	public const int StatusFieldNumber = 22;

	private static readonly FieldCodec<Status> _repeated_status_codec = FieldCodec.ForMessage(178u, Protobuf.Status.Parser);

	private readonly RepeatedField<Status> status_ = new RepeatedField<Status>();

	public const int CityCompetitionPointFieldNumber = 23;

	private CityCompetitionPoint cityCompetitionPoint_;

	public const int UuidFieldNumber = 100;

	private long uuid_;

	public const int ExtraInfoFieldNumber = 101;

	private ByteString extraInfo_ = ByteString.Empty;

	public const int ServerIdFieldNumber = 102;

	private int serverId_;

	public const int SrcServerIdFieldNumber = 103;

	private int srcServerId_;

	public const int WorldIdFieldNumber = 104;

	private int worldId_;

	[DebuggerNonUserCode]
	public static MessageParser<WorldPointInfo> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => WorldPointInfoReflection.Descriptor.MessageTypes[38];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public int Id
	{
		get
		{
			return id_;
		}
		set
		{
			id_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int PointType
	{
		get
		{
			return pointType_;
		}
		set
		{
			pointType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public BuildInfo BuildInfo
	{
		get
		{
			return buildInfo_;
		}
		set
		{
			buildInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RoadInfo RoadInfo
	{
		get
		{
			return roadInfo_;
		}
		set
		{
			roadInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CollectResourceInfo CollectResourceInfo
	{
		get
		{
			return collectResourceInfo_;
		}
		set
		{
			collectResourceInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ResourceInfo ResourceInfo
	{
		get
		{
			return resourceInfo_;
		}
		set
		{
			resourceInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ExplorePointInfo ExplorePointInfo
	{
		get
		{
			return explorePointInfo_;
		}
		set
		{
			explorePointInfo_ = value;
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
	public GarbagePointInfo GarbagePointInfo
	{
		get
		{
			return garbagePointInfo_;
		}
		set
		{
			garbagePointInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public HeroDispatchMissionPointInfo HeroDispatchMissionPointInfo
	{
		get
		{
			return heroDispatchMissionPointInfo_;
		}
		set
		{
			heroDispatchMissionPointInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public TreasurePointInfo TreasurePointInfo
	{
		get
		{
			return treasurePointInfo_;
		}
		set
		{
			treasurePointInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WorldAllianceCollectResPointInfo AllianceCollectResInfo
	{
		get
		{
			return allianceCollectResInfo_;
		}
		set
		{
			allianceCollectResInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public IceSuppliesPointInfo IceSuppliesPointInfo
	{
		get
		{
			return iceSuppliesPointInfo_;
		}
		set
		{
			iceSuppliesPointInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public GhostReconPointInfo GhostReconPointInfo
	{
		get
		{
			return ghostReconPointInfo_;
		}
		set
		{
			ghostReconPointInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public CityAttachment CityAttachment
	{
		get
		{
			return cityAttachment_;
		}
		set
		{
			cityAttachment_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ZoneMobilizationPointInfo ZoneMobilizationPointInfo
	{
		get
		{
			return zoneMobilizationPointInfo_;
		}
		set
		{
			zoneMobilizationPointInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public SurprisePointInfo SurprisePoint
	{
		get
		{
			return surprisePoint_;
		}
		set
		{
			surprisePoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MeteoritePoint MeteoritePoint
	{
		get
		{
			return meteoritePoint_;
		}
		set
		{
			meteoritePoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public MonsterChallengeTreasurePointInfo MonsterChallengeTreasurePointInfo
	{
		get
		{
			return monsterChallengeTreasurePointInfo_;
		}
		set
		{
			monsterChallengeTreasurePointInfo_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ActivityTreasurePointInfo ActivityTreasurePoint
	{
		get
		{
			return activityTreasurePoint_;
		}
		set
		{
			activityTreasurePoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public QuarantinePointInfo QuarantinePoint
	{
		get
		{
			return quarantinePoint_;
		}
		set
		{
			quarantinePoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public RepeatedField<Status> Status => status_;

	[DebuggerNonUserCode]
	public CityCompetitionPoint CityCompetitionPoint
	{
		get
		{
			return cityCompetitionPoint_;
		}
		set
		{
			cityCompetitionPoint_ = value;
		}
	}

	[DebuggerNonUserCode]
	public long Uuid
	{
		get
		{
			return uuid_;
		}
		set
		{
			uuid_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ByteString ExtraInfo
	{
		get
		{
			return extraInfo_;
		}
		set
		{
			extraInfo_ = ProtoPreconditions.CheckNotNull(value, "value");
		}
	}

	[DebuggerNonUserCode]
	public int ServerId
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
	public int SrcServerId
	{
		get
		{
			return srcServerId_;
		}
		set
		{
			srcServerId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int WorldId
	{
		get
		{
			return worldId_;
		}
		set
		{
			worldId_ = value;
		}
	}

	[DebuggerNonUserCode]
	public WorldPointInfo()
	{
	}

	[DebuggerNonUserCode]
	public WorldPointInfo(WorldPointInfo other)
		: this()
	{
		id_ = other.id_;
		pointType_ = other.pointType_;
		buildInfo_ = ((other.buildInfo_ != null) ? other.buildInfo_.Clone() : null);
		roadInfo_ = ((other.roadInfo_ != null) ? other.roadInfo_.Clone() : null);
		collectResourceInfo_ = ((other.collectResourceInfo_ != null) ? other.collectResourceInfo_.Clone() : null);
		resourceInfo_ = ((other.resourceInfo_ != null) ? other.resourceInfo_.Clone() : null);
		explorePointInfo_ = ((other.explorePointInfo_ != null) ? other.explorePointInfo_.Clone() : null);
		samplePointInfo_ = ((other.samplePointInfo_ != null) ? other.samplePointInfo_.Clone() : null);
		garbagePointInfo_ = ((other.garbagePointInfo_ != null) ? other.garbagePointInfo_.Clone() : null);
		heroDispatchMissionPointInfo_ = ((other.heroDispatchMissionPointInfo_ != null) ? other.heroDispatchMissionPointInfo_.Clone() : null);
		treasurePointInfo_ = ((other.treasurePointInfo_ != null) ? other.treasurePointInfo_.Clone() : null);
		allianceCollectResInfo_ = ((other.allianceCollectResInfo_ != null) ? other.allianceCollectResInfo_.Clone() : null);
		iceSuppliesPointInfo_ = ((other.iceSuppliesPointInfo_ != null) ? other.iceSuppliesPointInfo_.Clone() : null);
		ghostReconPointInfo_ = ((other.ghostReconPointInfo_ != null) ? other.ghostReconPointInfo_.Clone() : null);
		cityAttachment_ = ((other.cityAttachment_ != null) ? other.cityAttachment_.Clone() : null);
		zoneMobilizationPointInfo_ = ((other.zoneMobilizationPointInfo_ != null) ? other.zoneMobilizationPointInfo_.Clone() : null);
		surprisePoint_ = ((other.surprisePoint_ != null) ? other.surprisePoint_.Clone() : null);
		meteoritePoint_ = ((other.meteoritePoint_ != null) ? other.meteoritePoint_.Clone() : null);
		monsterChallengeTreasurePointInfo_ = ((other.monsterChallengeTreasurePointInfo_ != null) ? other.monsterChallengeTreasurePointInfo_.Clone() : null);
		activityTreasurePoint_ = ((other.activityTreasurePoint_ != null) ? other.activityTreasurePoint_.Clone() : null);
		quarantinePoint_ = ((other.quarantinePoint_ != null) ? other.quarantinePoint_.Clone() : null);
		status_ = other.status_.Clone();
		cityCompetitionPoint_ = ((other.cityCompetitionPoint_ != null) ? other.cityCompetitionPoint_.Clone() : null);
		uuid_ = other.uuid_;
		extraInfo_ = other.extraInfo_;
		serverId_ = other.serverId_;
		srcServerId_ = other.srcServerId_;
		worldId_ = other.worldId_;
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public WorldPointInfo Clone()
	{
		return new WorldPointInfo(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as WorldPointInfo);
	}

	[DebuggerNonUserCode]
	public bool Equals(WorldPointInfo other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (Id != other.Id)
		{
			return false;
		}
		if (PointType != other.PointType)
		{
			return false;
		}
		if (!object.Equals(BuildInfo, other.BuildInfo))
		{
			return false;
		}
		if (!object.Equals(RoadInfo, other.RoadInfo))
		{
			return false;
		}
		if (!object.Equals(CollectResourceInfo, other.CollectResourceInfo))
		{
			return false;
		}
		if (!object.Equals(ResourceInfo, other.ResourceInfo))
		{
			return false;
		}
		if (!object.Equals(ExplorePointInfo, other.ExplorePointInfo))
		{
			return false;
		}
		if (!object.Equals(SamplePointInfo, other.SamplePointInfo))
		{
			return false;
		}
		if (!object.Equals(GarbagePointInfo, other.GarbagePointInfo))
		{
			return false;
		}
		if (!object.Equals(HeroDispatchMissionPointInfo, other.HeroDispatchMissionPointInfo))
		{
			return false;
		}
		if (!object.Equals(TreasurePointInfo, other.TreasurePointInfo))
		{
			return false;
		}
		if (!object.Equals(AllianceCollectResInfo, other.AllianceCollectResInfo))
		{
			return false;
		}
		if (!object.Equals(IceSuppliesPointInfo, other.IceSuppliesPointInfo))
		{
			return false;
		}
		if (!object.Equals(GhostReconPointInfo, other.GhostReconPointInfo))
		{
			return false;
		}
		if (!object.Equals(CityAttachment, other.CityAttachment))
		{
			return false;
		}
		if (!object.Equals(ZoneMobilizationPointInfo, other.ZoneMobilizationPointInfo))
		{
			return false;
		}
		if (!object.Equals(SurprisePoint, other.SurprisePoint))
		{
			return false;
		}
		if (!object.Equals(MeteoritePoint, other.MeteoritePoint))
		{
			return false;
		}
		if (!object.Equals(MonsterChallengeTreasurePointInfo, other.MonsterChallengeTreasurePointInfo))
		{
			return false;
		}
		if (!object.Equals(ActivityTreasurePoint, other.ActivityTreasurePoint))
		{
			return false;
		}
		if (!object.Equals(QuarantinePoint, other.QuarantinePoint))
		{
			return false;
		}
		if (!status_.Equals(other.status_))
		{
			return false;
		}
		if (!object.Equals(CityCompetitionPoint, other.CityCompetitionPoint))
		{
			return false;
		}
		if (Uuid != other.Uuid)
		{
			return false;
		}
		if (ExtraInfo != other.ExtraInfo)
		{
			return false;
		}
		if (ServerId != other.ServerId)
		{
			return false;
		}
		if (SrcServerId != other.SrcServerId)
		{
			return false;
		}
		if (WorldId != other.WorldId)
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (Id != 0)
		{
			num ^= Id.GetHashCode();
		}
		if (PointType != 0)
		{
			num ^= PointType.GetHashCode();
		}
		if (buildInfo_ != null)
		{
			num ^= BuildInfo.GetHashCode();
		}
		if (roadInfo_ != null)
		{
			num ^= RoadInfo.GetHashCode();
		}
		if (collectResourceInfo_ != null)
		{
			num ^= CollectResourceInfo.GetHashCode();
		}
		if (resourceInfo_ != null)
		{
			num ^= ResourceInfo.GetHashCode();
		}
		if (explorePointInfo_ != null)
		{
			num ^= ExplorePointInfo.GetHashCode();
		}
		if (samplePointInfo_ != null)
		{
			num ^= SamplePointInfo.GetHashCode();
		}
		if (garbagePointInfo_ != null)
		{
			num ^= GarbagePointInfo.GetHashCode();
		}
		if (heroDispatchMissionPointInfo_ != null)
		{
			num ^= HeroDispatchMissionPointInfo.GetHashCode();
		}
		if (treasurePointInfo_ != null)
		{
			num ^= TreasurePointInfo.GetHashCode();
		}
		if (allianceCollectResInfo_ != null)
		{
			num ^= AllianceCollectResInfo.GetHashCode();
		}
		if (iceSuppliesPointInfo_ != null)
		{
			num ^= IceSuppliesPointInfo.GetHashCode();
		}
		if (ghostReconPointInfo_ != null)
		{
			num ^= GhostReconPointInfo.GetHashCode();
		}
		if (cityAttachment_ != null)
		{
			num ^= CityAttachment.GetHashCode();
		}
		if (zoneMobilizationPointInfo_ != null)
		{
			num ^= ZoneMobilizationPointInfo.GetHashCode();
		}
		if (surprisePoint_ != null)
		{
			num ^= SurprisePoint.GetHashCode();
		}
		if (meteoritePoint_ != null)
		{
			num ^= MeteoritePoint.GetHashCode();
		}
		if (monsterChallengeTreasurePointInfo_ != null)
		{
			num ^= MonsterChallengeTreasurePointInfo.GetHashCode();
		}
		if (activityTreasurePoint_ != null)
		{
			num ^= ActivityTreasurePoint.GetHashCode();
		}
		if (quarantinePoint_ != null)
		{
			num ^= QuarantinePoint.GetHashCode();
		}
		num ^= status_.GetHashCode();
		if (cityCompetitionPoint_ != null)
		{
			num ^= CityCompetitionPoint.GetHashCode();
		}
		if (Uuid != 0L)
		{
			num ^= Uuid.GetHashCode();
		}
		if (ExtraInfo.Length != 0)
		{
			num ^= ExtraInfo.GetHashCode();
		}
		if (ServerId != 0)
		{
			num ^= ServerId.GetHashCode();
		}
		if (SrcServerId != 0)
		{
			num ^= SrcServerId.GetHashCode();
		}
		if (WorldId != 0)
		{
			num ^= WorldId.GetHashCode();
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
		if (Id != 0)
		{
			output.WriteRawTag(8);
			output.WriteInt32(Id);
		}
		if (PointType != 0)
		{
			output.WriteRawTag(16);
			output.WriteInt32(PointType);
		}
		if (buildInfo_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(BuildInfo);
		}
		if (roadInfo_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(RoadInfo);
		}
		if (collectResourceInfo_ != null)
		{
			output.WriteRawTag(42);
			output.WriteMessage(CollectResourceInfo);
		}
		if (resourceInfo_ != null)
		{
			output.WriteRawTag(50);
			output.WriteMessage(ResourceInfo);
		}
		if (explorePointInfo_ != null)
		{
			output.WriteRawTag(58);
			output.WriteMessage(ExplorePointInfo);
		}
		if (samplePointInfo_ != null)
		{
			output.WriteRawTag(66);
			output.WriteMessage(SamplePointInfo);
		}
		if (garbagePointInfo_ != null)
		{
			output.WriteRawTag(74);
			output.WriteMessage(GarbagePointInfo);
		}
		if (heroDispatchMissionPointInfo_ != null)
		{
			output.WriteRawTag(82);
			output.WriteMessage(HeroDispatchMissionPointInfo);
		}
		if (treasurePointInfo_ != null)
		{
			output.WriteRawTag(90);
			output.WriteMessage(TreasurePointInfo);
		}
		if (allianceCollectResInfo_ != null)
		{
			output.WriteRawTag(98);
			output.WriteMessage(AllianceCollectResInfo);
		}
		if (iceSuppliesPointInfo_ != null)
		{
			output.WriteRawTag(106);
			output.WriteMessage(IceSuppliesPointInfo);
		}
		if (ghostReconPointInfo_ != null)
		{
			output.WriteRawTag(114);
			output.WriteMessage(GhostReconPointInfo);
		}
		if (cityAttachment_ != null)
		{
			output.WriteRawTag(122);
			output.WriteMessage(CityAttachment);
		}
		if (zoneMobilizationPointInfo_ != null)
		{
			output.WriteRawTag(130, 1);
			output.WriteMessage(ZoneMobilizationPointInfo);
		}
		if (surprisePoint_ != null)
		{
			output.WriteRawTag(138, 1);
			output.WriteMessage(SurprisePoint);
		}
		if (meteoritePoint_ != null)
		{
			output.WriteRawTag(146, 1);
			output.WriteMessage(MeteoritePoint);
		}
		if (monsterChallengeTreasurePointInfo_ != null)
		{
			output.WriteRawTag(154, 1);
			output.WriteMessage(MonsterChallengeTreasurePointInfo);
		}
		if (activityTreasurePoint_ != null)
		{
			output.WriteRawTag(162, 1);
			output.WriteMessage(ActivityTreasurePoint);
		}
		if (quarantinePoint_ != null)
		{
			output.WriteRawTag(170, 1);
			output.WriteMessage(QuarantinePoint);
		}
		status_.WriteTo(output, _repeated_status_codec);
		if (cityCompetitionPoint_ != null)
		{
			output.WriteRawTag(186, 1);
			output.WriteMessage(CityCompetitionPoint);
		}
		if (Uuid != 0L)
		{
			output.WriteRawTag(160, 6);
			output.WriteInt64(Uuid);
		}
		if (ExtraInfo.Length != 0)
		{
			output.WriteRawTag(170, 6);
			output.WriteBytes(ExtraInfo);
		}
		if (ServerId != 0)
		{
			output.WriteRawTag(176, 6);
			output.WriteInt32(ServerId);
		}
		if (SrcServerId != 0)
		{
			output.WriteRawTag(184, 6);
			output.WriteInt32(SrcServerId);
		}
		if (WorldId != 0)
		{
			output.WriteRawTag(192, 6);
			output.WriteInt32(WorldId);
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
		if (Id != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(Id);
		}
		if (PointType != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(PointType);
		}
		if (buildInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(BuildInfo);
		}
		if (roadInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(RoadInfo);
		}
		if (collectResourceInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(CollectResourceInfo);
		}
		if (resourceInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ResourceInfo);
		}
		if (explorePointInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ExplorePointInfo);
		}
		if (samplePointInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(SamplePointInfo);
		}
		if (garbagePointInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(GarbagePointInfo);
		}
		if (heroDispatchMissionPointInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(HeroDispatchMissionPointInfo);
		}
		if (treasurePointInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(TreasurePointInfo);
		}
		if (allianceCollectResInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(AllianceCollectResInfo);
		}
		if (iceSuppliesPointInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(IceSuppliesPointInfo);
		}
		if (ghostReconPointInfo_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(GhostReconPointInfo);
		}
		if (cityAttachment_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(CityAttachment);
		}
		if (zoneMobilizationPointInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ZoneMobilizationPointInfo);
		}
		if (surprisePoint_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(SurprisePoint);
		}
		if (meteoritePoint_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MeteoritePoint);
		}
		if (monsterChallengeTreasurePointInfo_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(MonsterChallengeTreasurePointInfo);
		}
		if (activityTreasurePoint_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ActivityTreasurePoint);
		}
		if (quarantinePoint_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(QuarantinePoint);
		}
		num += status_.CalculateSize(_repeated_status_codec);
		if (cityCompetitionPoint_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(CityCompetitionPoint);
		}
		if (Uuid != 0L)
		{
			num += 2 + CodedOutputStream.ComputeInt64Size(Uuid);
		}
		if (ExtraInfo.Length != 0)
		{
			num += 2 + CodedOutputStream.ComputeBytesSize(ExtraInfo);
		}
		if (ServerId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(ServerId);
		}
		if (SrcServerId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(SrcServerId);
		}
		if (WorldId != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(WorldId);
		}
		if (_unknownFields != null)
		{
			num += _unknownFields.CalculateSize();
		}
		return num;
	}

	[DebuggerNonUserCode]
	public void MergeFrom(WorldPointInfo other)
	{
		if (other == null)
		{
			return;
		}
		if (other.Id != 0)
		{
			Id = other.Id;
		}
		if (other.PointType != 0)
		{
			PointType = other.PointType;
		}
		if (other.buildInfo_ != null)
		{
			if (buildInfo_ == null)
			{
				BuildInfo = new BuildInfo();
			}
			BuildInfo.MergeFrom(other.BuildInfo);
		}
		if (other.roadInfo_ != null)
		{
			if (roadInfo_ == null)
			{
				RoadInfo = new RoadInfo();
			}
			RoadInfo.MergeFrom(other.RoadInfo);
		}
		if (other.collectResourceInfo_ != null)
		{
			if (collectResourceInfo_ == null)
			{
				CollectResourceInfo = new CollectResourceInfo();
			}
			CollectResourceInfo.MergeFrom(other.CollectResourceInfo);
		}
		if (other.resourceInfo_ != null)
		{
			if (resourceInfo_ == null)
			{
				ResourceInfo = new ResourceInfo();
			}
			ResourceInfo.MergeFrom(other.ResourceInfo);
		}
		if (other.explorePointInfo_ != null)
		{
			if (explorePointInfo_ == null)
			{
				ExplorePointInfo = new ExplorePointInfo();
			}
			ExplorePointInfo.MergeFrom(other.ExplorePointInfo);
		}
		if (other.samplePointInfo_ != null)
		{
			if (samplePointInfo_ == null)
			{
				SamplePointInfo = new SamplePointInfo();
			}
			SamplePointInfo.MergeFrom(other.SamplePointInfo);
		}
		if (other.garbagePointInfo_ != null)
		{
			if (garbagePointInfo_ == null)
			{
				GarbagePointInfo = new GarbagePointInfo();
			}
			GarbagePointInfo.MergeFrom(other.GarbagePointInfo);
		}
		if (other.heroDispatchMissionPointInfo_ != null)
		{
			if (heroDispatchMissionPointInfo_ == null)
			{
				HeroDispatchMissionPointInfo = new HeroDispatchMissionPointInfo();
			}
			HeroDispatchMissionPointInfo.MergeFrom(other.HeroDispatchMissionPointInfo);
		}
		if (other.treasurePointInfo_ != null)
		{
			if (treasurePointInfo_ == null)
			{
				TreasurePointInfo = new TreasurePointInfo();
			}
			TreasurePointInfo.MergeFrom(other.TreasurePointInfo);
		}
		if (other.allianceCollectResInfo_ != null)
		{
			if (allianceCollectResInfo_ == null)
			{
				AllianceCollectResInfo = new WorldAllianceCollectResPointInfo();
			}
			AllianceCollectResInfo.MergeFrom(other.AllianceCollectResInfo);
		}
		if (other.iceSuppliesPointInfo_ != null)
		{
			if (iceSuppliesPointInfo_ == null)
			{
				IceSuppliesPointInfo = new IceSuppliesPointInfo();
			}
			IceSuppliesPointInfo.MergeFrom(other.IceSuppliesPointInfo);
		}
		if (other.ghostReconPointInfo_ != null)
		{
			if (ghostReconPointInfo_ == null)
			{
				GhostReconPointInfo = new GhostReconPointInfo();
			}
			GhostReconPointInfo.MergeFrom(other.GhostReconPointInfo);
		}
		if (other.cityAttachment_ != null)
		{
			if (cityAttachment_ == null)
			{
				CityAttachment = new CityAttachment();
			}
			CityAttachment.MergeFrom(other.CityAttachment);
		}
		if (other.zoneMobilizationPointInfo_ != null)
		{
			if (zoneMobilizationPointInfo_ == null)
			{
				ZoneMobilizationPointInfo = new ZoneMobilizationPointInfo();
			}
			ZoneMobilizationPointInfo.MergeFrom(other.ZoneMobilizationPointInfo);
		}
		if (other.surprisePoint_ != null)
		{
			if (surprisePoint_ == null)
			{
				SurprisePoint = new SurprisePointInfo();
			}
			SurprisePoint.MergeFrom(other.SurprisePoint);
		}
		if (other.meteoritePoint_ != null)
		{
			if (meteoritePoint_ == null)
			{
				MeteoritePoint = new MeteoritePoint();
			}
			MeteoritePoint.MergeFrom(other.MeteoritePoint);
		}
		if (other.monsterChallengeTreasurePointInfo_ != null)
		{
			if (monsterChallengeTreasurePointInfo_ == null)
			{
				MonsterChallengeTreasurePointInfo = new MonsterChallengeTreasurePointInfo();
			}
			MonsterChallengeTreasurePointInfo.MergeFrom(other.MonsterChallengeTreasurePointInfo);
		}
		if (other.activityTreasurePoint_ != null)
		{
			if (activityTreasurePoint_ == null)
			{
				ActivityTreasurePoint = new ActivityTreasurePointInfo();
			}
			ActivityTreasurePoint.MergeFrom(other.ActivityTreasurePoint);
		}
		if (other.quarantinePoint_ != null)
		{
			if (quarantinePoint_ == null)
			{
				QuarantinePoint = new QuarantinePointInfo();
			}
			QuarantinePoint.MergeFrom(other.QuarantinePoint);
		}
		status_.Add(other.status_);
		if (other.cityCompetitionPoint_ != null)
		{
			if (cityCompetitionPoint_ == null)
			{
				CityCompetitionPoint = new CityCompetitionPoint();
			}
			CityCompetitionPoint.MergeFrom(other.CityCompetitionPoint);
		}
		if (other.Uuid != 0L)
		{
			Uuid = other.Uuid;
		}
		if (other.ExtraInfo.Length != 0)
		{
			ExtraInfo = other.ExtraInfo;
		}
		if (other.ServerId != 0)
		{
			ServerId = other.ServerId;
		}
		if (other.SrcServerId != 0)
		{
			SrcServerId = other.SrcServerId;
		}
		if (other.WorldId != 0)
		{
			WorldId = other.WorldId;
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
				Id = input.ReadInt32();
				break;
			case 16u:
				PointType = input.ReadInt32();
				break;
			case 26u:
				if (buildInfo_ == null)
				{
					BuildInfo = new BuildInfo();
				}
				input.ReadMessage(BuildInfo);
				break;
			case 34u:
				if (roadInfo_ == null)
				{
					RoadInfo = new RoadInfo();
				}
				input.ReadMessage(RoadInfo);
				break;
			case 42u:
				if (collectResourceInfo_ == null)
				{
					CollectResourceInfo = new CollectResourceInfo();
				}
				input.ReadMessage(CollectResourceInfo);
				break;
			case 50u:
				if (resourceInfo_ == null)
				{
					ResourceInfo = new ResourceInfo();
				}
				input.ReadMessage(ResourceInfo);
				break;
			case 58u:
				if (explorePointInfo_ == null)
				{
					ExplorePointInfo = new ExplorePointInfo();
				}
				input.ReadMessage(ExplorePointInfo);
				break;
			case 66u:
				if (samplePointInfo_ == null)
				{
					SamplePointInfo = new SamplePointInfo();
				}
				input.ReadMessage(SamplePointInfo);
				break;
			case 74u:
				if (garbagePointInfo_ == null)
				{
					GarbagePointInfo = new GarbagePointInfo();
				}
				input.ReadMessage(GarbagePointInfo);
				break;
			case 82u:
				if (heroDispatchMissionPointInfo_ == null)
				{
					HeroDispatchMissionPointInfo = new HeroDispatchMissionPointInfo();
				}
				input.ReadMessage(HeroDispatchMissionPointInfo);
				break;
			case 90u:
				if (treasurePointInfo_ == null)
				{
					TreasurePointInfo = new TreasurePointInfo();
				}
				input.ReadMessage(TreasurePointInfo);
				break;
			case 98u:
				if (allianceCollectResInfo_ == null)
				{
					AllianceCollectResInfo = new WorldAllianceCollectResPointInfo();
				}
				input.ReadMessage(AllianceCollectResInfo);
				break;
			case 106u:
				if (iceSuppliesPointInfo_ == null)
				{
					IceSuppliesPointInfo = new IceSuppliesPointInfo();
				}
				input.ReadMessage(IceSuppliesPointInfo);
				break;
			case 114u:
				if (ghostReconPointInfo_ == null)
				{
					GhostReconPointInfo = new GhostReconPointInfo();
				}
				input.ReadMessage(GhostReconPointInfo);
				break;
			case 122u:
				if (cityAttachment_ == null)
				{
					CityAttachment = new CityAttachment();
				}
				input.ReadMessage(CityAttachment);
				break;
			case 130u:
				if (zoneMobilizationPointInfo_ == null)
				{
					ZoneMobilizationPointInfo = new ZoneMobilizationPointInfo();
				}
				input.ReadMessage(ZoneMobilizationPointInfo);
				break;
			case 138u:
				if (surprisePoint_ == null)
				{
					SurprisePoint = new SurprisePointInfo();
				}
				input.ReadMessage(SurprisePoint);
				break;
			case 146u:
				if (meteoritePoint_ == null)
				{
					MeteoritePoint = new MeteoritePoint();
				}
				input.ReadMessage(MeteoritePoint);
				break;
			case 154u:
				if (monsterChallengeTreasurePointInfo_ == null)
				{
					MonsterChallengeTreasurePointInfo = new MonsterChallengeTreasurePointInfo();
				}
				input.ReadMessage(MonsterChallengeTreasurePointInfo);
				break;
			case 162u:
				if (activityTreasurePoint_ == null)
				{
					ActivityTreasurePoint = new ActivityTreasurePointInfo();
				}
				input.ReadMessage(ActivityTreasurePoint);
				break;
			case 170u:
				if (quarantinePoint_ == null)
				{
					QuarantinePoint = new QuarantinePointInfo();
				}
				input.ReadMessage(QuarantinePoint);
				break;
			case 178u:
				status_.AddEntriesFrom(input, _repeated_status_codec);
				break;
			case 186u:
				if (cityCompetitionPoint_ == null)
				{
					CityCompetitionPoint = new CityCompetitionPoint();
				}
				input.ReadMessage(CityCompetitionPoint);
				break;
			case 800u:
				Uuid = input.ReadInt64();
				break;
			case 810u:
				ExtraInfo = input.ReadBytes();
				break;
			case 816u:
				ServerId = input.ReadInt32();
				break;
			case 824u:
				SrcServerId = input.ReadInt32();
				break;
			case 832u:
				WorldId = input.ReadInt32();
				break;
			}
		}
	}
}
