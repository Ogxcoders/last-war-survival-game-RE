using UnityEngine;

public class BasementCollider : MonoBehaviour, ITouchObjectClickHandler, ITouchObject
{
	private int theBuildId;

	private int thePointIndex;

	private CityBuilding theCityBuilding;

	public float Priority => 1f;

	public Vector2Int TilePos
	{
		get
		{
			if (theCityBuilding != null)
			{
				return theCityBuilding.TilePos;
			}
			return Vector2Int.zero;
		}
	}

	public WorldPreviewType PreviewType => WorldPreviewType.Default;

	public PointInfo GetPointInfo()
	{
		if (theCityBuilding != null)
		{
			return theCityBuilding.GetPointInfo();
		}
		return null;
	}

	public bool OnClick()
	{
		if (theCityBuilding != null && theBuildId != 0 && thePointIndex != 0)
		{
			GameEntry.Lua.Call("UIUtil.OnClickCollider", thePointIndex, theBuildId);
			return true;
		}
		return false;
	}

	public void CSInit(CityBuilding cityBuilding, int buildId, int pointIndex)
	{
		theCityBuilding = cityBuilding;
		theBuildId = buildId;
		thePointIndex = pointIndex;
	}
}
