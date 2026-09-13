using System.Text;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Protobuf;
using Sfs2X.Entities.Data;

public class PointInfo
{
	public const int WEREWOLF_STATUS_ID = 704102;

	public const int WEREWOLF_BASE_SKIN_ID = -89757;

	public const string WEREWOLF_SKIN_PREFAB = "Assets/Main/SeasonRes/S4/Prefabs/Building/A_build_werewolf_world.prefab";

	public const string DEFAULT_BUILD_MODEL_PATH = "Assets/Main/Prefabs/Building/building_10100035_world.prefab";

	public const int WEREWOLF_TITLE_SKIN_ID = 0;

	public const int WEREWOLF_EFFECT_SKIN_ID = 0;

	public int pointIndex;

	public int mainIndex;

	public WorldPointType pointType;

	public string ownerUid;

	public int tileSize;

	public long uuid;

	public byte[] extraInfo;

	public int serverId;

	public int srcServerId;

	public int worldId = -1;

	public ThermalConductor thermalConductor;

	public RepeatedField<Status> status;

	private const int StatusTypeCityVirus = 39;

	public int LodLayerMask => pointType switch
	{
		WorldPointType.WorldMonster => 224, 
		WorldPointType.WorldResource => 224, 
		_ => 0, 
	};

	public bool isMainPoint => mainIndex == pointIndex;

	public int PointType => (int)pointType;

	protected WorldPointManager PointManager
	{
		get
		{
			WorldScene worldScene = SceneManager.World as WorldScene;
			if (worldScene == null)
			{
				return null;
			}
			return worldScene.PointManager;
		}
	}

	public virtual bool CheckClickIsValid(int lod)
	{
		return false;
	}

	public PointInfo()
	{
	}

	public PointInfo(WorldPointInfo pi)
	{
		uuid = pi.Uuid;
		pointIndex = pi.Id;
		mainIndex = pi.Id;
		pointType = (WorldPointType)pi.PointType;
		tileSize = 1;
		serverId = pi.ServerId;
		srcServerId = pi.SrcServerId;
		worldId = pi.WorldId;
		ByteString byteString = pi.ExtraInfo;
		if (byteString != null)
		{
			extraInfo = byteString.ToByteArray();
		}
		status = pi.Status;
	}

	public virtual PointInfo Clone()
	{
		PointInfo pointInfo = new PointInfo();
		BaseClone(pointInfo);
		return pointInfo;
	}

	protected void BaseClone(PointInfo p)
	{
		p.pointIndex = pointIndex;
		p.mainIndex = mainIndex;
		p.pointType = pointType;
		p.ownerUid = ownerUid;
		p.tileSize = tileSize;
		p.uuid = uuid;
		p.extraInfo = extraInfo;
		p.serverId = serverId;
		p.srcServerId = srcServerId;
		p.worldId = worldId;
		p.status = status;
	}

	public bool IsMine()
	{
		return ownerUid == GameEntry.Data.Player.Uid;
	}

	public virtual PlayerType GetPlayerType()
	{
		return PlayerType.PlayerNone;
	}

	public bool IsFrozen()
	{
		if (thermalConductor != null)
		{
			return thermalConductor.hp > 0;
		}
		return false;
	}

	public bool IsOverHeat()
	{
		if (thermalConductor != null)
		{
			return thermalConductor.phase == 3;
		}
		return false;
	}

	public string Description(StringBuilder sb = null)
	{
		sb = sb ?? new StringBuilder();
		sb.AppendLine("<color=yellow>===WorldPointInfo===</color>");
		sb.AppendLine($"PointIndex:{pointIndex}");
		sb.AppendLine($"WorldPosition:{SceneManager.World.TileIndexToWorld(pointIndex)}");
		sb.AppendLine($"TilePosition:{SceneManager.World.IndexToTilePos(pointIndex)}");
		sb.AppendLine($"MainIndex:{mainIndex}");
		sb.AppendLine("ownerUid:" + ownerUid);
		sb.AppendLine($"serverId:{serverId}");
		sb.AppendLine($"srcServerId:{srcServerId}");
		sb.AppendLine($"size:{tileSize}");
		sb.AppendLine($"uuid:{uuid}");
		sb.AppendLine("PointType:" + pointType);
		OnDescription(sb);
		return sb.ToString();
	}

	public virtual void OnDescription(StringBuilder sb)
	{
	}

	public bool IsHasStatusOn(int type)
	{
		if (this.status != null && this.status.Count > 0)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			for (int i = 0; i < this.status.Count; i++)
			{
				Status status = this.status[i];
				string templateData = GameEntry.ConfigCache.GetTemplateData("lw_status", status.Id, "type2");
				int result = -1;
				if (int.TryParse(templateData, out result) && type == result && serverTime < status.ExpireTime)
				{
					return true;
				}
			}
		}
		return false;
	}

	public long GetStatusExpireTime(int type)
	{
		if (this.status != null && this.status.Count > 0)
		{
			long serverTime = GameEntry.Timer.GetServerTime();
			for (int i = 0; i < this.status.Count; i++)
			{
				Status status = this.status[i];
				string templateData = GameEntry.ConfigCache.GetTemplateData("lw_status", status.Id, "type2");
				int result = -1;
				if (int.TryParse(templateData, out result) && type == result && serverTime < status.ExpireTime)
				{
					return status.ExpireTime;
				}
			}
		}
		return 0L;
	}

	public long GetStatusExpireTimeById(int id)
	{
		if (status != null && status.Count > 0)
		{
			foreach (Status item in status)
			{
				if (item.Id == id)
				{
					return item.ExpireTime;
				}
			}
		}
		return 0L;
	}

	protected virtual void BeforeSetPointStatus()
	{
	}

	protected virtual void OnAddPointState(Status status)
	{
	}

	public void SetStatus(ISFSObject msg)
	{
		BeforeSetPointStatus();
		if (this.status == null)
		{
			this.status = new RepeatedField<Status>();
		}
		this.status.Clear();
		ISFSArray iSFSArray = msg.TryGetArray("list");
		if (iSFSArray == null)
		{
			return;
		}
		foreach (ISFSObject item in iSFSArray)
		{
			Status status = new Status
			{
				Id = item.TryGetInt("stateId"),
				ExpireTime = item.TryGetLong("overTime"),
				Layer = item.TryGetInt("layer")
			};
			this.status.Add(status);
			OnAddPointState(status);
			if (int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_status", status.Id, "type2"), out var result) && result == 39)
			{
				GameEntry.Event.Fire(EventId.AllianceCityVirusRefresh, pointIndex);
			}
		}
	}

	public Status GetStatusByType(int type)
	{
		if (status == null)
		{
			return null;
		}
		foreach (Status item in status)
		{
			if (int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_status", item.Id, "type2"), out var result) && result == type)
			{
				return item;
			}
		}
		return null;
	}

	public int GetCityVirusLayer()
	{
		if (status == null)
		{
			return 0;
		}
		foreach (Status item in status)
		{
			if (int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_status", item.Id, "type2"), out var result) && result == 39)
			{
				return CalcVirusLevel(item.Id, item.Layer, item.ExpireTime);
			}
		}
		return 0;
	}

	private static int CalcVirusLevel(int statusId, int layer, long endTime)
	{
		if (layer <= 0 || endTime <= 0)
		{
			return 0;
		}
		if (!int.TryParse(GameEntry.ConfigCache.GetTemplateData("lw_status", statusId, "time"), out var result))
		{
			result = 900;
		}
		long serverTime = GameEntry.Timer.GetServerTime();
		result *= 1000;
		if (serverTime > endTime)
		{
			if (layer == 1)
			{
				return 0;
			}
			do
			{
				endTime += result;
				layer--;
			}
			while (serverTime > endTime && layer > 0);
		}
		return layer;
	}
}
