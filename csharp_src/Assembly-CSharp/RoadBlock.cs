using UnityEngine;

public class RoadBlock : MonoBehaviour
{
	private FakeModelManager.TempRoadType _type;

	private const float _removePosY = 0.06f;

	private void Update()
	{
	}

	public void UpdateSprite(int pointIndex, FakeModelManager.TempRoadType type, float time = -1f)
	{
		_type = type;
		Vector3 position = SceneManager.World.TileIndexToWorld(pointIndex);
		position.y = ((type == FakeModelManager.TempRoadType.DeleteRoad) ? 0.06f : 0f);
		base.transform.position = position;
		switch (type)
		{
		}
	}

	public void ClickMakeRoad()
	{
	}

	public float GetCurTime()
	{
		return 0f;
	}
}
