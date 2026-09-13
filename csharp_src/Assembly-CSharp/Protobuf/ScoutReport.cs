using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Protobuf;

public sealed class ScoutReport : IMessage<ScoutReport>, IMessage, IEquatable<ScoutReport>, IDeepCloneable<ScoutReport>
{
	[DebuggerNonUserCode]
	public static class Types
	{
		public enum TargetType
		{
			[OriginalName("DEFAULT")]
			Default,
			[OriginalName("MAIN_BUILDING")]
			MainBuilding,
			[OriginalName("STORAGE")]
			Storage,
			[OriginalName("COLLECT_BUILDING")]
			CollectBuilding,
			[OriginalName("TOWER")]
			Tower,
			[OriginalName("BUILDING")]
			Building,
			[OriginalName("MARCH")]
			March,
			[OriginalName("ALLIANCE_CITY")]
			AllianceCity,
			[OriginalName("TRAIN")]
			Train,
			[OriginalName("DRAGON_BUILDING")]
			DragonBuilding,
			[OriginalName("DESERT")]
			Desert,
			[OriginalName("ALLIANCE_BUILD")]
			AllianceBuild,
			[OriginalName("WINTER_STORM_BUILDING")]
			WinterStormBuilding,
			[OriginalName("CITY_STRONGHOLD")]
			CityStronghold,
			[OriginalName("CITY_TRADE")]
			CityTrade,
			[OriginalName("QUARANTINE_BUILDING")]
			QuarantineBuilding
		}
	}

	private static readonly MessageParser<ScoutReport> _parser = new MessageParser<ScoutReport>(() => new ScoutReport());

	private UnknownFieldSet _unknownFields;

	public const int TargetTypeFieldNumber = 101;

	private Types.TargetType targetType_;

	public const int TargetUserFieldNumber = 1;

	private ScoutUser targetUser_;

	public const int UserWallFieldNumber = 2;

	private ScoutUserWall userWall_;

	public const int ResourceFieldNumber = 3;

	private ScoutResource resource_;

	public const int ArmyFieldNumber = 4;

	private ScoutArmy army_;

	public const int TowerFieldNumber = 5;

	private ScoutTower tower_;

	public const int TargetAllianceCityFieldNumber = 6;

	private ScoutAllianceCity targetAllianceCity_;

	public const int AllianceCityWallFieldNumber = 7;

	private ScoutAllianceCityWall allianceCityWall_;

	public const int TargetDesertFieldNumber = 8;

	private ScountDesert targetDesert_;

	public const int TargetAllianceBuildFieldNumber = 9;

	private ScoutAllianceBuild targetAllianceBuild_;

	public const int AllianceBuildWallFieldNumber = 10;

	private ScoutAllianceBuildWall allianceBuildWall_;

	public const int PlayerSeasonBuildFieldNumber = 11;

	private ScoutPlayerSeasonBuild playerSeasonBuild_;

	public const int WinterStormBuildFieldNumber = 12;

	private ScoutWinterStormBuilding winterStormBuild_;

	public const int ScoutCityStrongholdFieldNumber = 13;

	private ScoutCityStronghold scoutCityStronghold_;

	public const int ScoutLevelFieldNumber = 14;

	private int scoutLevel_;

	public const int BattleWeaponFieldNumber = 15;

	private ScoutBattleWeapon battleWeapon_;

	public const int ArmyProcessFieldNumber = 16;

	private ArmyProgress armyProcess_;

	public const int VersionFieldNumber = 17;

	private int version_;

	public const int HideCountFieldNumber = 18;

	private int hideCount_;

	public const int ScoutCityTradeFieldNumber = 19;

	private ScoutCityTrade scoutCityTrade_;

	public const int QuarantineBuildingFieldNumber = 20;

	private ScoutQuarantineBuilding quarantineBuilding_;

	[DebuggerNonUserCode]
	public static MessageParser<ScoutReport> Parser => _parser;

	[DebuggerNonUserCode]
	public static MessageDescriptor Descriptor => LwScoutReportReflection.Descriptor.MessageTypes[0];

	[DebuggerNonUserCode]
	MessageDescriptor IMessage.Descriptor => Descriptor;

	[DebuggerNonUserCode]
	public Types.TargetType TargetType
	{
		get
		{
			return targetType_;
		}
		set
		{
			targetType_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutUser TargetUser
	{
		get
		{
			return targetUser_;
		}
		set
		{
			targetUser_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutUserWall UserWall
	{
		get
		{
			return userWall_;
		}
		set
		{
			userWall_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutResource Resource
	{
		get
		{
			return resource_;
		}
		set
		{
			resource_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutArmy Army
	{
		get
		{
			return army_;
		}
		set
		{
			army_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutTower Tower
	{
		get
		{
			return tower_;
		}
		set
		{
			tower_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutAllianceCity TargetAllianceCity
	{
		get
		{
			return targetAllianceCity_;
		}
		set
		{
			targetAllianceCity_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutAllianceCityWall AllianceCityWall
	{
		get
		{
			return allianceCityWall_;
		}
		set
		{
			allianceCityWall_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScountDesert TargetDesert
	{
		get
		{
			return targetDesert_;
		}
		set
		{
			targetDesert_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutAllianceBuild TargetAllianceBuild
	{
		get
		{
			return targetAllianceBuild_;
		}
		set
		{
			targetAllianceBuild_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutAllianceBuildWall AllianceBuildWall
	{
		get
		{
			return allianceBuildWall_;
		}
		set
		{
			allianceBuildWall_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutPlayerSeasonBuild PlayerSeasonBuild
	{
		get
		{
			return playerSeasonBuild_;
		}
		set
		{
			playerSeasonBuild_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutWinterStormBuilding WinterStormBuild
	{
		get
		{
			return winterStormBuild_;
		}
		set
		{
			winterStormBuild_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutCityStronghold ScoutCityStronghold
	{
		get
		{
			return scoutCityStronghold_;
		}
		set
		{
			scoutCityStronghold_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int ScoutLevel
	{
		get
		{
			return scoutLevel_;
		}
		set
		{
			scoutLevel_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutBattleWeapon BattleWeapon
	{
		get
		{
			return battleWeapon_;
		}
		set
		{
			battleWeapon_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ArmyProgress ArmyProcess
	{
		get
		{
			return armyProcess_;
		}
		set
		{
			armyProcess_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int Version
	{
		get
		{
			return version_;
		}
		set
		{
			version_ = value;
		}
	}

	[DebuggerNonUserCode]
	public int HideCount
	{
		get
		{
			return hideCount_;
		}
		set
		{
			hideCount_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutCityTrade ScoutCityTrade
	{
		get
		{
			return scoutCityTrade_;
		}
		set
		{
			scoutCityTrade_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutQuarantineBuilding QuarantineBuilding
	{
		get
		{
			return quarantineBuilding_;
		}
		set
		{
			quarantineBuilding_ = value;
		}
	}

	[DebuggerNonUserCode]
	public ScoutReport()
	{
	}

	[DebuggerNonUserCode]
	public ScoutReport(ScoutReport other)
		: this()
	{
		targetType_ = other.targetType_;
		targetUser_ = ((other.targetUser_ != null) ? other.targetUser_.Clone() : null);
		userWall_ = ((other.userWall_ != null) ? other.userWall_.Clone() : null);
		resource_ = ((other.resource_ != null) ? other.resource_.Clone() : null);
		army_ = ((other.army_ != null) ? other.army_.Clone() : null);
		tower_ = ((other.tower_ != null) ? other.tower_.Clone() : null);
		targetAllianceCity_ = ((other.targetAllianceCity_ != null) ? other.targetAllianceCity_.Clone() : null);
		allianceCityWall_ = ((other.allianceCityWall_ != null) ? other.allianceCityWall_.Clone() : null);
		targetDesert_ = ((other.targetDesert_ != null) ? other.targetDesert_.Clone() : null);
		targetAllianceBuild_ = ((other.targetAllianceBuild_ != null) ? other.targetAllianceBuild_.Clone() : null);
		allianceBuildWall_ = ((other.allianceBuildWall_ != null) ? other.allianceBuildWall_.Clone() : null);
		playerSeasonBuild_ = ((other.playerSeasonBuild_ != null) ? other.playerSeasonBuild_.Clone() : null);
		winterStormBuild_ = ((other.winterStormBuild_ != null) ? other.winterStormBuild_.Clone() : null);
		scoutCityStronghold_ = ((other.scoutCityStronghold_ != null) ? other.scoutCityStronghold_.Clone() : null);
		scoutLevel_ = other.scoutLevel_;
		battleWeapon_ = ((other.battleWeapon_ != null) ? other.battleWeapon_.Clone() : null);
		armyProcess_ = ((other.armyProcess_ != null) ? other.armyProcess_.Clone() : null);
		version_ = other.version_;
		hideCount_ = other.hideCount_;
		scoutCityTrade_ = ((other.scoutCityTrade_ != null) ? other.scoutCityTrade_.Clone() : null);
		quarantineBuilding_ = ((other.quarantineBuilding_ != null) ? other.quarantineBuilding_.Clone() : null);
		_unknownFields = UnknownFieldSet.Clone(other._unknownFields);
	}

	[DebuggerNonUserCode]
	public ScoutReport Clone()
	{
		return new ScoutReport(this);
	}

	[DebuggerNonUserCode]
	public override bool Equals(object other)
	{
		return Equals(other as ScoutReport);
	}

	[DebuggerNonUserCode]
	public bool Equals(ScoutReport other)
	{
		if (other == null)
		{
			return false;
		}
		if (other == this)
		{
			return true;
		}
		if (TargetType != other.TargetType)
		{
			return false;
		}
		if (!object.Equals(TargetUser, other.TargetUser))
		{
			return false;
		}
		if (!object.Equals(UserWall, other.UserWall))
		{
			return false;
		}
		if (!object.Equals(Resource, other.Resource))
		{
			return false;
		}
		if (!object.Equals(Army, other.Army))
		{
			return false;
		}
		if (!object.Equals(Tower, other.Tower))
		{
			return false;
		}
		if (!object.Equals(TargetAllianceCity, other.TargetAllianceCity))
		{
			return false;
		}
		if (!object.Equals(AllianceCityWall, other.AllianceCityWall))
		{
			return false;
		}
		if (!object.Equals(TargetDesert, other.TargetDesert))
		{
			return false;
		}
		if (!object.Equals(TargetAllianceBuild, other.TargetAllianceBuild))
		{
			return false;
		}
		if (!object.Equals(AllianceBuildWall, other.AllianceBuildWall))
		{
			return false;
		}
		if (!object.Equals(PlayerSeasonBuild, other.PlayerSeasonBuild))
		{
			return false;
		}
		if (!object.Equals(WinterStormBuild, other.WinterStormBuild))
		{
			return false;
		}
		if (!object.Equals(ScoutCityStronghold, other.ScoutCityStronghold))
		{
			return false;
		}
		if (ScoutLevel != other.ScoutLevel)
		{
			return false;
		}
		if (!object.Equals(BattleWeapon, other.BattleWeapon))
		{
			return false;
		}
		if (!object.Equals(ArmyProcess, other.ArmyProcess))
		{
			return false;
		}
		if (Version != other.Version)
		{
			return false;
		}
		if (HideCount != other.HideCount)
		{
			return false;
		}
		if (!object.Equals(ScoutCityTrade, other.ScoutCityTrade))
		{
			return false;
		}
		if (!object.Equals(QuarantineBuilding, other.QuarantineBuilding))
		{
			return false;
		}
		return object.Equals(_unknownFields, other._unknownFields);
	}

	[DebuggerNonUserCode]
	public override int GetHashCode()
	{
		int num = 1;
		if (TargetType != Types.TargetType.Default)
		{
			num ^= TargetType.GetHashCode();
		}
		if (targetUser_ != null)
		{
			num ^= TargetUser.GetHashCode();
		}
		if (userWall_ != null)
		{
			num ^= UserWall.GetHashCode();
		}
		if (resource_ != null)
		{
			num ^= Resource.GetHashCode();
		}
		if (army_ != null)
		{
			num ^= Army.GetHashCode();
		}
		if (tower_ != null)
		{
			num ^= Tower.GetHashCode();
		}
		if (targetAllianceCity_ != null)
		{
			num ^= TargetAllianceCity.GetHashCode();
		}
		if (allianceCityWall_ != null)
		{
			num ^= AllianceCityWall.GetHashCode();
		}
		if (targetDesert_ != null)
		{
			num ^= TargetDesert.GetHashCode();
		}
		if (targetAllianceBuild_ != null)
		{
			num ^= TargetAllianceBuild.GetHashCode();
		}
		if (allianceBuildWall_ != null)
		{
			num ^= AllianceBuildWall.GetHashCode();
		}
		if (playerSeasonBuild_ != null)
		{
			num ^= PlayerSeasonBuild.GetHashCode();
		}
		if (winterStormBuild_ != null)
		{
			num ^= WinterStormBuild.GetHashCode();
		}
		if (scoutCityStronghold_ != null)
		{
			num ^= ScoutCityStronghold.GetHashCode();
		}
		if (ScoutLevel != 0)
		{
			num ^= ScoutLevel.GetHashCode();
		}
		if (battleWeapon_ != null)
		{
			num ^= BattleWeapon.GetHashCode();
		}
		if (armyProcess_ != null)
		{
			num ^= ArmyProcess.GetHashCode();
		}
		if (Version != 0)
		{
			num ^= Version.GetHashCode();
		}
		if (HideCount != 0)
		{
			num ^= HideCount.GetHashCode();
		}
		if (scoutCityTrade_ != null)
		{
			num ^= ScoutCityTrade.GetHashCode();
		}
		if (quarantineBuilding_ != null)
		{
			num ^= QuarantineBuilding.GetHashCode();
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
		if (targetUser_ != null)
		{
			output.WriteRawTag(10);
			output.WriteMessage(TargetUser);
		}
		if (userWall_ != null)
		{
			output.WriteRawTag(18);
			output.WriteMessage(UserWall);
		}
		if (resource_ != null)
		{
			output.WriteRawTag(26);
			output.WriteMessage(Resource);
		}
		if (army_ != null)
		{
			output.WriteRawTag(34);
			output.WriteMessage(Army);
		}
		if (tower_ != null)
		{
			output.WriteRawTag(42);
			output.WriteMessage(Tower);
		}
		if (targetAllianceCity_ != null)
		{
			output.WriteRawTag(50);
			output.WriteMessage(TargetAllianceCity);
		}
		if (allianceCityWall_ != null)
		{
			output.WriteRawTag(58);
			output.WriteMessage(AllianceCityWall);
		}
		if (targetDesert_ != null)
		{
			output.WriteRawTag(66);
			output.WriteMessage(TargetDesert);
		}
		if (targetAllianceBuild_ != null)
		{
			output.WriteRawTag(74);
			output.WriteMessage(TargetAllianceBuild);
		}
		if (allianceBuildWall_ != null)
		{
			output.WriteRawTag(82);
			output.WriteMessage(AllianceBuildWall);
		}
		if (playerSeasonBuild_ != null)
		{
			output.WriteRawTag(90);
			output.WriteMessage(PlayerSeasonBuild);
		}
		if (winterStormBuild_ != null)
		{
			output.WriteRawTag(98);
			output.WriteMessage(WinterStormBuild);
		}
		if (scoutCityStronghold_ != null)
		{
			output.WriteRawTag(106);
			output.WriteMessage(ScoutCityStronghold);
		}
		if (ScoutLevel != 0)
		{
			output.WriteRawTag(112);
			output.WriteInt32(ScoutLevel);
		}
		if (battleWeapon_ != null)
		{
			output.WriteRawTag(122);
			output.WriteMessage(BattleWeapon);
		}
		if (armyProcess_ != null)
		{
			output.WriteRawTag(130, 1);
			output.WriteMessage(ArmyProcess);
		}
		if (Version != 0)
		{
			output.WriteRawTag(136, 1);
			output.WriteInt32(Version);
		}
		if (HideCount != 0)
		{
			output.WriteRawTag(144, 1);
			output.WriteInt32(HideCount);
		}
		if (scoutCityTrade_ != null)
		{
			output.WriteRawTag(154, 1);
			output.WriteMessage(ScoutCityTrade);
		}
		if (quarantineBuilding_ != null)
		{
			output.WriteRawTag(162, 1);
			output.WriteMessage(QuarantineBuilding);
		}
		if (TargetType != Types.TargetType.Default)
		{
			output.WriteRawTag(168, 6);
			output.WriteEnum((int)TargetType);
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
		if (TargetType != Types.TargetType.Default)
		{
			num += 2 + CodedOutputStream.ComputeEnumSize((int)TargetType);
		}
		if (targetUser_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(TargetUser);
		}
		if (userWall_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(UserWall);
		}
		if (resource_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Resource);
		}
		if (army_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Army);
		}
		if (tower_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(Tower);
		}
		if (targetAllianceCity_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(TargetAllianceCity);
		}
		if (allianceCityWall_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(AllianceCityWall);
		}
		if (targetDesert_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(TargetDesert);
		}
		if (targetAllianceBuild_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(TargetAllianceBuild);
		}
		if (allianceBuildWall_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(AllianceBuildWall);
		}
		if (playerSeasonBuild_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(PlayerSeasonBuild);
		}
		if (winterStormBuild_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(WinterStormBuild);
		}
		if (scoutCityStronghold_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(ScoutCityStronghold);
		}
		if (ScoutLevel != 0)
		{
			num += 1 + CodedOutputStream.ComputeInt32Size(ScoutLevel);
		}
		if (battleWeapon_ != null)
		{
			num += 1 + CodedOutputStream.ComputeMessageSize(BattleWeapon);
		}
		if (armyProcess_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ArmyProcess);
		}
		if (Version != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(Version);
		}
		if (HideCount != 0)
		{
			num += 2 + CodedOutputStream.ComputeInt32Size(HideCount);
		}
		if (scoutCityTrade_ != null)
		{
			num += 2 + CodedOutputStream.ComputeMessageSize(ScoutCityTrade);
		}
		if (quarantineBuilding_ != null)
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
	public void MergeFrom(ScoutReport other)
	{
		if (other == null)
		{
			return;
		}
		if (other.TargetType != Types.TargetType.Default)
		{
			TargetType = other.TargetType;
		}
		if (other.targetUser_ != null)
		{
			if (targetUser_ == null)
			{
				TargetUser = new ScoutUser();
			}
			TargetUser.MergeFrom(other.TargetUser);
		}
		if (other.userWall_ != null)
		{
			if (userWall_ == null)
			{
				UserWall = new ScoutUserWall();
			}
			UserWall.MergeFrom(other.UserWall);
		}
		if (other.resource_ != null)
		{
			if (resource_ == null)
			{
				Resource = new ScoutResource();
			}
			Resource.MergeFrom(other.Resource);
		}
		if (other.army_ != null)
		{
			if (army_ == null)
			{
				Army = new ScoutArmy();
			}
			Army.MergeFrom(other.Army);
		}
		if (other.tower_ != null)
		{
			if (tower_ == null)
			{
				Tower = new ScoutTower();
			}
			Tower.MergeFrom(other.Tower);
		}
		if (other.targetAllianceCity_ != null)
		{
			if (targetAllianceCity_ == null)
			{
				TargetAllianceCity = new ScoutAllianceCity();
			}
			TargetAllianceCity.MergeFrom(other.TargetAllianceCity);
		}
		if (other.allianceCityWall_ != null)
		{
			if (allianceCityWall_ == null)
			{
				AllianceCityWall = new ScoutAllianceCityWall();
			}
			AllianceCityWall.MergeFrom(other.AllianceCityWall);
		}
		if (other.targetDesert_ != null)
		{
			if (targetDesert_ == null)
			{
				TargetDesert = new ScountDesert();
			}
			TargetDesert.MergeFrom(other.TargetDesert);
		}
		if (other.targetAllianceBuild_ != null)
		{
			if (targetAllianceBuild_ == null)
			{
				TargetAllianceBuild = new ScoutAllianceBuild();
			}
			TargetAllianceBuild.MergeFrom(other.TargetAllianceBuild);
		}
		if (other.allianceBuildWall_ != null)
		{
			if (allianceBuildWall_ == null)
			{
				AllianceBuildWall = new ScoutAllianceBuildWall();
			}
			AllianceBuildWall.MergeFrom(other.AllianceBuildWall);
		}
		if (other.playerSeasonBuild_ != null)
		{
			if (playerSeasonBuild_ == null)
			{
				PlayerSeasonBuild = new ScoutPlayerSeasonBuild();
			}
			PlayerSeasonBuild.MergeFrom(other.PlayerSeasonBuild);
		}
		if (other.winterStormBuild_ != null)
		{
			if (winterStormBuild_ == null)
			{
				WinterStormBuild = new ScoutWinterStormBuilding();
			}
			WinterStormBuild.MergeFrom(other.WinterStormBuild);
		}
		if (other.scoutCityStronghold_ != null)
		{
			if (scoutCityStronghold_ == null)
			{
				ScoutCityStronghold = new ScoutCityStronghold();
			}
			ScoutCityStronghold.MergeFrom(other.ScoutCityStronghold);
		}
		if (other.ScoutLevel != 0)
		{
			ScoutLevel = other.ScoutLevel;
		}
		if (other.battleWeapon_ != null)
		{
			if (battleWeapon_ == null)
			{
				BattleWeapon = new ScoutBattleWeapon();
			}
			BattleWeapon.MergeFrom(other.BattleWeapon);
		}
		if (other.armyProcess_ != null)
		{
			if (armyProcess_ == null)
			{
				ArmyProcess = new ArmyProgress();
			}
			ArmyProcess.MergeFrom(other.ArmyProcess);
		}
		if (other.Version != 0)
		{
			Version = other.Version;
		}
		if (other.HideCount != 0)
		{
			HideCount = other.HideCount;
		}
		if (other.scoutCityTrade_ != null)
		{
			if (scoutCityTrade_ == null)
			{
				ScoutCityTrade = new ScoutCityTrade();
			}
			ScoutCityTrade.MergeFrom(other.ScoutCityTrade);
		}
		if (other.quarantineBuilding_ != null)
		{
			if (quarantineBuilding_ == null)
			{
				QuarantineBuilding = new ScoutQuarantineBuilding();
			}
			QuarantineBuilding.MergeFrom(other.QuarantineBuilding);
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
				if (targetUser_ == null)
				{
					TargetUser = new ScoutUser();
				}
				input.ReadMessage(TargetUser);
				break;
			case 18u:
				if (userWall_ == null)
				{
					UserWall = new ScoutUserWall();
				}
				input.ReadMessage(UserWall);
				break;
			case 26u:
				if (resource_ == null)
				{
					Resource = new ScoutResource();
				}
				input.ReadMessage(Resource);
				break;
			case 34u:
				if (army_ == null)
				{
					Army = new ScoutArmy();
				}
				input.ReadMessage(Army);
				break;
			case 42u:
				if (tower_ == null)
				{
					Tower = new ScoutTower();
				}
				input.ReadMessage(Tower);
				break;
			case 50u:
				if (targetAllianceCity_ == null)
				{
					TargetAllianceCity = new ScoutAllianceCity();
				}
				input.ReadMessage(TargetAllianceCity);
				break;
			case 58u:
				if (allianceCityWall_ == null)
				{
					AllianceCityWall = new ScoutAllianceCityWall();
				}
				input.ReadMessage(AllianceCityWall);
				break;
			case 66u:
				if (targetDesert_ == null)
				{
					TargetDesert = new ScountDesert();
				}
				input.ReadMessage(TargetDesert);
				break;
			case 74u:
				if (targetAllianceBuild_ == null)
				{
					TargetAllianceBuild = new ScoutAllianceBuild();
				}
				input.ReadMessage(TargetAllianceBuild);
				break;
			case 82u:
				if (allianceBuildWall_ == null)
				{
					AllianceBuildWall = new ScoutAllianceBuildWall();
				}
				input.ReadMessage(AllianceBuildWall);
				break;
			case 90u:
				if (playerSeasonBuild_ == null)
				{
					PlayerSeasonBuild = new ScoutPlayerSeasonBuild();
				}
				input.ReadMessage(PlayerSeasonBuild);
				break;
			case 98u:
				if (winterStormBuild_ == null)
				{
					WinterStormBuild = new ScoutWinterStormBuilding();
				}
				input.ReadMessage(WinterStormBuild);
				break;
			case 106u:
				if (scoutCityStronghold_ == null)
				{
					ScoutCityStronghold = new ScoutCityStronghold();
				}
				input.ReadMessage(ScoutCityStronghold);
				break;
			case 112u:
				ScoutLevel = input.ReadInt32();
				break;
			case 122u:
				if (battleWeapon_ == null)
				{
					BattleWeapon = new ScoutBattleWeapon();
				}
				input.ReadMessage(BattleWeapon);
				break;
			case 130u:
				if (armyProcess_ == null)
				{
					ArmyProcess = new ArmyProgress();
				}
				input.ReadMessage(ArmyProcess);
				break;
			case 136u:
				Version = input.ReadInt32();
				break;
			case 144u:
				HideCount = input.ReadInt32();
				break;
			case 154u:
				if (scoutCityTrade_ == null)
				{
					ScoutCityTrade = new ScoutCityTrade();
				}
				input.ReadMessage(ScoutCityTrade);
				break;
			case 162u:
				if (quarantineBuilding_ == null)
				{
					QuarantineBuilding = new ScoutQuarantineBuilding();
				}
				input.ReadMessage(QuarantineBuilding);
				break;
			case 808u:
				TargetType = (Types.TargetType)input.ReadEnum();
				break;
			}
		}
	}
}
