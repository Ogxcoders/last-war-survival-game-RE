using System.Collections.Generic;
using UnityEngine;

public class WorldMapEpidemicLine : MonoBehaviour
{
	[SerializeField]
	private LineRenderer _lineRenderer;

	[SerializeField]
	private Material[] materials;

	private const int PointCount = 20;

	private readonly List<Vector3> _allPoints = new List<Vector3>();

	public void ShowLine(Vector3 sPoint, Vector3 ePoint, Vector3 mPoint, int index)
	{
		_allPoints.Clear();
		float num = 20f;
		_lineRenderer.material = materials[index];
		for (int i = 0; i <= 20; i++)
		{
			float num2 = (float)i / num;
			float num3 = Vector3.Distance(sPoint, mPoint);
			Vector3 vector = sPoint + Vector3.Normalize(mPoint - sPoint) * num3 * num2;
			float num4 = Vector3.Distance(mPoint, ePoint);
			Vector3 vector2 = mPoint + Vector3.Normalize(ePoint - mPoint) * num4 * num2;
			float num5 = Vector3.Distance(vector, vector2);
			Vector3 item = vector + Vector3.Normalize(vector2 - vector) * num5 * num2;
			_allPoints.Add(item);
		}
		_lineRenderer.positionCount = _allPoints.Count;
		_lineRenderer.SetPositions(_allPoints.ToArray());
	}

	public void HideLine()
	{
		_allPoints.Clear();
		_lineRenderer.positionCount = 0;
	}
}
