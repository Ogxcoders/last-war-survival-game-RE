using Protobuf;
using UnityEngine;

public class WorldAllianceBuildS4 : MonoBehaviour
{
	[SerializeField]
	public SpriteRenderer lodIcon;

	[SerializeField]
	public GameObject normalBuild;

	[SerializeField]
	public GameObject normalBuildEffect;

	[SerializeField]
	public GameObject guardianTower;

	private bool guardianTowerModeInit;

	private bool guardianTowerModeLast;

	public void UpdateStatus(WorldScene world, AllianceBuildPointInfo pt, AllianceBuildingPointInfo extraData)
	{
		if (pt != null && pt.buildId == 400000)
		{
			bool flag = pt.IsGuardianTower();
			if (guardianTowerModeInit && flag == guardianTowerModeLast)
			{
				return;
			}
			normalBuild?.SetActive(!flag);
			guardianTower?.SetActive(flag);
			normalBuildEffect?.SetActive(!flag && extraData.State == 0);
			if (lodIcon != null)
			{
				if (flag)
				{
					lodIcon.LoadSprite("Assets/Main/SeasonRes/Shared/Sprites/AllianceGovernmentSkill/Ambassador/mjc_S4_daditu_dianta_03.png");
				}
				else
				{
					lodIcon.LoadSprite("Assets/Main/Sprites/LodIcon/zyf_daditu_lianmengqianxian_01_white.png");
					UpdateGuardianTowerIcon(world, pt, isGuardianTower: false);
				}
			}
			guardianTowerModeInit = true;
			guardianTowerModeLast = flag;
		}
		else if (pt != null && (pt.buildId == 402000 || pt.buildId == 403000 || pt.buildId == 404000))
		{
			bool flag2 = pt.IsGuardianTower();
			if (!guardianTowerModeInit || flag2 != guardianTowerModeLast)
			{
				normalBuild?.SetActive(!flag2);
				guardianTower?.SetActive(flag2);
				normalBuildEffect?.SetActive(!flag2 && extraData.State == 0);
				UpdateGuardianTowerIcon(world, pt, flag2);
				guardianTowerModeInit = true;
				guardianTowerModeLast = flag2;
			}
		}
	}

	private void UpdateGuardianTowerIcon(WorldScene world, AllianceBuildPointInfo pt, bool isGuardianTower)
	{
		if (pt == null)
		{
			return;
		}
		if (pt.buildId == 400000)
		{
			if (lodIcon != null)
			{
				Vector2Int v = world.IndexToTilePos(pt.mainIndex);
				isGuardianTower = IsGuardianTower(world, 402000, v) || IsGuardianTower(world, 403000, v) || IsGuardianTower(world, 404000, v);
				if (isGuardianTower)
				{
					lodIcon.LoadSprite("Assets/Main/SeasonRes/Shared/Sprites/AllianceGovernmentSkill/Ambassador/mjc_S4_daditu_dianta_03.png");
				}
				else
				{
					lodIcon.LoadSprite("Assets/Main/Sprites/LodIcon/zyf_daditu_lianmengqianxian_01_white.png");
				}
			}
		}
		else
		{
			if (!isGuardianTower)
			{
				return;
			}
			int k = 0;
			int v2 = 0;
			string tabName = "alliance_res_build";
			string templateData = GameEntry.ConfigCache.GetTemplateData(tabName, pt.buildId, "relative_position");
			if (templateData.IsNullOrEmpty() || !templateData.Split_to_ii(';', out k, out v2))
			{
				return;
			}
			Vector2Int vector2Int = world.IndexToTilePos(pt.mainIndex);
			int pointIndex = world.TilePosToIndex(new Vector2Int(vector2Int.x - k, vector2Int.y - v2));
			PointInfo pointInfo = world.GetPointInfo(pointIndex);
			if (pointInfo != null && world.GetObjectByUuid(pointInfo.uuid) is WorldAllianceBuildObject worldAllianceBuildObject)
			{
				WorldAllianceBuildS4 worldAllianceBuildS = worldAllianceBuildObject.GetGuardianTower();
				if (worldAllianceBuildS != null)
				{
					worldAllianceBuildS.ChangeLodIcon(isGuardianTower: true);
				}
			}
		}
	}

	public void ChangeLodIcon(bool isGuardianTower)
	{
		if (lodIcon != null)
		{
			if (isGuardianTower)
			{
				lodIcon.LoadSprite("Assets/Main/SeasonRes/Shared/Sprites/AllianceGovernmentSkill/Ambassador/mjc_S4_daditu_dianta_03.png");
			}
			else
			{
				lodIcon.LoadSprite("Assets/Main/Sprites/LodIcon/zyf_daditu_lianmengqianxian_01_white.png");
			}
		}
	}

	public void Destroy()
	{
		guardianTowerModeInit = false;
		guardianTowerModeLast = false;
	}

	private static bool IsGuardianTower(WorldScene world, int buildId, Vector2Int v2)
	{
		int k = 0;
		int v3 = 0;
		string tabName = "alliance_res_build";
		string templateData = GameEntry.ConfigCache.GetTemplateData(tabName, buildId, "relative_position");
		if (!templateData.IsNullOrEmpty() && templateData.Split_to_ii(';', out k, out v3))
		{
			int pointIndex = world.TilePosToIndex(new Vector2Int(v2.x + k, v2.y + v3));
			if (world.GetPointInfo(pointIndex) is AllianceBuildPointInfo allianceBuildPointInfo)
			{
				return allianceBuildPointInfo.IsGuardianTower();
			}
		}
		return false;
	}
}
