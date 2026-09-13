using Protobuf;
using UnityEngine;

public class WorldAllianceBuildFightMode
{
	private const string battleAnimPath = "Assets/_Art_LastWar/Effect/Prefab/dafuw/Eff_dafuw_jiaozhan_X_S1.prefab";

	private GameObject rootNode;

	private Transform battleRoot1;

	private Transform battleRoot2;

	private InstanceRequest eff1;

	private InstanceRequest eff2;

	public WorldAllianceBuildFightMode(GameObject root)
	{
		rootNode = root;
	}

	public void UpdateStatus(WorldOutpostTowerPoint pt, OutpostTowerInfo extraData)
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (pt != null && pt.battleStartTime <= serverTime && pt.protectTime <= serverTime && rootNode != null)
		{
			if (battleRoot1 == null)
			{
				battleRoot1 = rootNode.transform.Find("Icon/CollectResourceIconSprite");
			}
			if (battleRoot2 == null)
			{
				battleRoot2 = rootNode.transform.Find("ModelGo/Normal/battle");
			}
			if (eff1 == null && battleRoot1 != null)
			{
				eff1 = WorldPointObject.AsyncLoad("Assets/_Art_LastWar/Effect/Prefab/dafuw/Eff_dafuw_jiaozhan_X_S1.prefab", battleRoot1, Vector3.zero, new Vector3(0.2f, 0.2f, 0.2f), Quaternion.identity);
			}
			if (eff2 == null && battleRoot2 != null)
			{
				eff2 = WorldPointObject.AsyncLoad("Assets/_Art_LastWar/Effect/Prefab/dafuw/Eff_dafuw_jiaozhan_X_S1.prefab", battleRoot2, Vector3.zero, new Vector3(0.15f, 0.15f, 0.15f), Quaternion.identity);
			}
		}
		else
		{
			Destroy();
		}
	}

	public void UpdateStatus(WorldOutpostPoint pt, OutpostInfo extraData)
	{
		long serverTime = GameEntry.Timer.GetServerTime();
		if (pt != null && pt.battleStartTime <= serverTime && pt.protectTime <= serverTime && rootNode != null)
		{
			if (battleRoot1 == null)
			{
				battleRoot1 = rootNode.transform.Find("Icon/CollectResourceIconSprite");
			}
			if (battleRoot2 == null)
			{
				battleRoot2 = rootNode.transform.Find("ModelGo/Normal/battle");
			}
			if (eff1 == null && battleRoot1 != null)
			{
				eff1 = WorldPointObject.AsyncLoad("Assets/_Art_LastWar/Effect/Prefab/dafuw/Eff_dafuw_jiaozhan_X_S1.prefab", battleRoot1, Vector3.zero, new Vector3(0.2f, 0.2f, 0.2f), Quaternion.identity);
			}
			if (eff2 == null && battleRoot2 != null)
			{
				eff2 = WorldPointObject.AsyncLoad("Assets/_Art_LastWar/Effect/Prefab/dafuw/Eff_dafuw_jiaozhan_X_S1.prefab", battleRoot2, Vector3.zero, new Vector3(0.15f, 0.15f, 0.15f), Quaternion.identity);
			}
		}
		else
		{
			Destroy();
		}
	}

	public void UpdateStatus(AllianceBuildPointInfo pt, AllianceBuildingPointInfo extraData)
	{
		if (pt != null && pt.fightState == 1 && rootNode != null)
		{
			if (battleRoot1 == null)
			{
				battleRoot1 = rootNode.transform.Find("Icon/CollectResourceIconSprite");
			}
			if (battleRoot2 == null)
			{
				battleRoot2 = rootNode.transform.Find("ModelGo/Normal/battle");
			}
			if (eff1 == null && battleRoot1 != null)
			{
				eff1 = WorldPointObject.AsyncLoad("Assets/_Art_LastWar/Effect/Prefab/dafuw/Eff_dafuw_jiaozhan_X_S1.prefab", battleRoot1, Vector3.zero, new Vector3(0.2f, 0.2f, 0.2f), Quaternion.identity);
			}
			if (eff2 == null && battleRoot2 != null)
			{
				eff2 = WorldPointObject.AsyncLoad("Assets/_Art_LastWar/Effect/Prefab/dafuw/Eff_dafuw_jiaozhan_X_S1.prefab", battleRoot2, Vector3.zero, new Vector3(0.15f, 0.15f, 0.15f), Quaternion.identity);
			}
		}
		else
		{
			Destroy();
		}
	}

	public void Destroy()
	{
		if (eff1 != null)
		{
			eff1.Destroy();
		}
		if (eff2 != null)
		{
			eff2.Destroy();
		}
	}
}
