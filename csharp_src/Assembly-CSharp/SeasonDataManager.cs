using System.Collections.Generic;
using UnityEngine;
using XLua;

public class SeasonDataManager
{
	public sealed class UISkinSetting
	{
		public Color bgColor { get; set; }

		public string bgPath { get; set; }

		public string titlePath { get; set; }

		public string backPath { get; set; }
	}

	private static SeasonDataManager _instance;

	private UISkinSetting _uiSkinSetting;

	private readonly NestedDictionary<string, string, string> dataCache = new NestedDictionary<string, string, string>();

	private readonly Dictionary<int, int> _mNinePalacesIndex2Server = new Dictionary<int, int>();

	private readonly Dictionary<int, int> _mNinePalacesServer2Index = new Dictionary<int, int>();

	private readonly Dictionary<int, SceneSkinMeta> _mNinePalacesIndex2Skin = new Dictionary<int, SceneSkinMeta>();

	private readonly Dictionary<int, SceneSkinMeta> _mNinePalacesServer2Skin = new Dictionary<int, SceneSkinMeta>();

	private static readonly Vector3[] World9BasePos = new Vector3[10];

	private static readonly int[] World9BasePosX = new int[10] { 0, 0, 2000, 4000, 0, 2000, 4000, 0, 2000, 4000 };

	private static readonly int[] World9BasePosZ = new int[10] { 0, 0, 0, 0, 2000, 2000, 2000, 4000, 4000, 4000 };

	public static SeasonDataManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SeasonDataManager();
			}
			return _instance;
		}
	}

	public UISkinSetting SkinSetting => _uiSkinSetting;

	public static void Purge()
	{
		_instance = null;
	}

	public void UpdateUISkin(Color bgColor, string bgPath, string titlePath, string backPath)
	{
		if (_uiSkinSetting == null)
		{
			_uiSkinSetting = new UISkinSetting();
		}
		_uiSkinSetting.bgColor = bgColor;
		_uiSkinSetting.bgPath = bgPath;
		_uiSkinSetting.titlePath = titlePath;
		_uiSkinSetting.backPath = backPath;
	}

	public void SetData(string k1, string k2, string data)
	{
		dataCache.Add(k1, k2, data);
	}

	public string GetData(string k1, string k2, string defaultData)
	{
		if (!dataCache.TryGetValue(k1, k2, out var value))
		{
			return defaultData;
		}
		return value;
	}

	public static NewMarchType CheckCrossMarchType(NewMarchType theTargetType, ref bool globalArmy)
	{
		switch (theTargetType)
		{
		case NewMarchType.CROSS_NORMAL:
			globalArmy = true;
			return NewMarchType.NORMAL;
		case NewMarchType.CROSS_ASSEMBLY_MARCH:
			globalArmy = true;
			return NewMarchType.ASSEMBLY_MARCH;
		case NewMarchType.CROSS_SCOUT:
			globalArmy = true;
			return NewMarchType.SCOUT;
		default:
			globalArmy = false;
			return theTargetType;
		}
	}

	public static MarchTargetType CheckCrossMarchTargetType(MarchTargetType theTargetType, ref bool globalArmy)
	{
		switch (theTargetType)
		{
		case MarchTargetType.CROSS_COLLECT:
			globalArmy = true;
			return MarchTargetType.COLLECT;
		case MarchTargetType.CROSS_ATTACK_ARMY_COLLECT:
			globalArmy = true;
			return MarchTargetType.ATTACK_ARMY_COLLECT;
		case MarchTargetType.CROSS_ATTACK_MONSTER:
			globalArmy = true;
			return MarchTargetType.ATTACK_MONSTER;
		case MarchTargetType.CROSS_DETECT_TREASURE:
			globalArmy = true;
			return MarchTargetType.DETECT_TREASURE;
		case MarchTargetType.CROSS_ATTACK_CITY:
			globalArmy = true;
			return MarchTargetType.ATTACK_CITY;
		case MarchTargetType.CROSS_SCOUT_CITY:
			globalArmy = true;
			return MarchTargetType.SCOUT_CITY;
		case MarchTargetType.CROSS_ASSISTANCE_CITY:
			globalArmy = true;
			return MarchTargetType.ASSISTANCE_CITY;
		case MarchTargetType.CROSS_BACK_HOME:
			globalArmy = true;
			return MarchTargetType.BACK_HOME;
		case MarchTargetType.CROSS_DIRECT_ATTACK_ACT_BOSS:
			globalArmy = true;
			return MarchTargetType.DIRECT_ATTACK_ACT_BOSS;
		case MarchTargetType.CROSS_ATTACK_CITY_STRONGHOLD:
			globalArmy = true;
			return MarchTargetType.ATTACK_CITY_STRONGHOLD;
		case MarchTargetType.CROSS_ASSISTANCE_CITY_STRONGHOLD:
			globalArmy = true;
			return MarchTargetType.ASSISTANCE_CITY_STRONGHOLD;
		case MarchTargetType.CROSS_ATTACK_ALLIANCE_CITY:
			globalArmy = true;
			return MarchTargetType.ATTACK_ALLIANCE_CITY;
		case MarchTargetType.CROSS_ASSISTANCE_ALLIANCE_CITY:
			globalArmy = true;
			return MarchTargetType.ASSISTANCE_ALLIANCE_CITY;
		case MarchTargetType.CROSS_DIRECT_ATTACK_ACT_BERSERK_BOSS:
			globalArmy = true;
			return MarchTargetType.DIRECT_ATTACK_ACT_BERSERK_BOSS;
		default:
			globalArmy = false;
			return theTargetType;
		}
	}

	public bool InSeasonBigMapMode()
	{
		SceneSkinMeta curSkinMeta = SceneSkinManager.Instance.GetCurSkinMeta();
		if (curSkinMeta != null && curSkinMeta.IsNineNationMode())
		{
			return _mNinePalacesIndex2Server.Count == 9;
		}
		return false;
	}

	public void UpdateNinePalacesSkin(int serverId, int mapIndex, SceneSkinMeta meta)
	{
		_mNinePalacesIndex2Skin[mapIndex] = meta;
		_mNinePalacesServer2Skin[serverId] = meta;
	}

	public SceneSkinMeta GetWorldSkinByIndex(int mapIndex)
	{
		if (_mNinePalacesIndex2Skin.TryGetValue(mapIndex, out var value))
		{
			return value;
		}
		return null;
	}

	public SceneSkinMeta GetWorldSkinByServerId(int serverId)
	{
		if (_mNinePalacesServer2Skin.TryGetValue(serverId, out var value))
		{
			return value;
		}
		return null;
	}

	public void UpdateNinePalacesData(LuaTable info)
	{
		_mNinePalacesIndex2Server.Clear();
		_mNinePalacesServer2Index.Clear();
		if (info != null)
		{
			info.ForEach(delegate(int nIndex, int nServerId)
			{
				_mNinePalacesIndex2Server[nIndex] = nServerId;
				_mNinePalacesServer2Index[nServerId] = nIndex;
			});
			if (SceneManager.IsInWorld() && SceneManager.World != null)
			{
				SceneManager.World.SetWorldSize((_mNinePalacesServer2Index.Count == 9) ? 3000 : 1000);
			}
		}
	}

	public int GetNinePalacesServer(int index)
	{
		if (index > 0 && _mNinePalacesIndex2Server.TryGetValue(index, out var value))
		{
			return value;
		}
		return 0;
	}

	public bool InNinePalacesList(int serverId)
	{
		return _mNinePalacesServer2Index.ContainsKey(serverId);
	}

	public int GetNinePalacesIndex(int serverId)
	{
		if (_mNinePalacesServer2Index.TryGetValue(serverId, out var value))
		{
			return value;
		}
		return 1;
	}

	public void CleanNinePalacesData()
	{
		_mNinePalacesIndex2Server.Clear();
		_mNinePalacesServer2Index.Clear();
		if (SceneManager.IsInWorld() && SceneManager.World != null)
		{
			SceneManager.World.SetWorldSize(1000);
		}
	}

	public Vector3 NormalizedWorldPos(Vector3 realWorldPos, int serverId)
	{
		int ninePalacesIndex = GetNinePalacesIndex(serverId);
		if (ninePalacesIndex > 1)
		{
			return new Vector3(realWorldPos.x - (float)World9BasePosX[ninePalacesIndex], realWorldPos.y, realWorldPos.z - (float)World9BasePosZ[ninePalacesIndex]);
		}
		return realWorldPos;
	}

	public int GetServerIdFromWorldPos(Vector3 worldPos)
	{
		if (InSeasonBigMapMode())
		{
			int num = Mathf.Clamp((int)worldPos.x / 2, 0, 2999) / 1000;
			int num2 = Mathf.Clamp((int)worldPos.z / 2, 0, 2999) / 1000;
			return GetNinePalacesServer(num + 1 + 3 * num2);
		}
		return 0;
	}

	public int GetNinePalacesIndexFromWorldPos(Vector3 worldPos)
	{
		if (InSeasonBigMapMode())
		{
			int num = Mathf.Clamp((int)worldPos.x / 2, 0, 2999) / 1000;
			int num2 = Mathf.Clamp((int)worldPos.z / 2, 0, 2999) / 1000;
			return num + 1 + 3 * num2;
		}
		return 1;
	}

	public Vector3 GetWorldBasePos(int tileX, int tileY)
	{
		if (InSeasonBigMapMode())
		{
			int num = Mathf.Clamp(tileX, 0, 2999) / 1000;
			int num2 = Mathf.Clamp(tileY, 0, 2999) / 1000;
			return GetWorldBasePosByIndex(num + 1 + 3 * num2);
		}
		return Vector3.zero;
	}

	public Vector3 GetWorldBasePos(int serverId)
	{
		if (serverId <= 0)
		{
			return Vector3.zero;
		}
		return GetWorldBasePosByIndex(GetNinePalacesIndex(serverId));
	}

	public Vector3 GetWorldBasePosByIndex(int mapIndex)
	{
		if (mapIndex > 1)
		{
			Vector3 result = World9BasePos[mapIndex];
			result.Set(World9BasePosX[mapIndex], 0f, World9BasePosZ[mapIndex]);
			return result;
		}
		return Vector3.zero;
	}
}
