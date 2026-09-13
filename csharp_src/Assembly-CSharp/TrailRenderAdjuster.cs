using BitBenderGames;
using UnityEngine;

public class TrailRenderAdjuster : MonoBehaviourWrapped
{
	public int thresholdXZ = 1;

	public int thresholdY;

	public TrailRenderer[] tr;

	private Vector3 _lastPos;

	public void LateUpdate()
	{
		if (tr.Length == 0)
		{
			return;
		}
		Vector3 position = base.Transform.position;
		bool flag = false;
		if (thresholdXZ > 0)
		{
			flag = Mathf.Abs(_lastPos.x - position.x) > (float)thresholdXZ || Mathf.Abs(_lastPos.z - position.z) > (float)thresholdXZ;
		}
		if (thresholdY > 0)
		{
			flag = flag || Mathf.Abs(_lastPos.y - position.y) > (float)thresholdY;
		}
		if (flag)
		{
			int num = tr.Length;
			for (int i = 0; i < num; i++)
			{
				if (tr[i] != null)
				{
					tr[i].Clear();
				}
			}
		}
		_lastPos = position;
	}

	private void Reset()
	{
		tr = GetComponentsInChildren<TrailRenderer>(includeInactive: true);
	}
}
