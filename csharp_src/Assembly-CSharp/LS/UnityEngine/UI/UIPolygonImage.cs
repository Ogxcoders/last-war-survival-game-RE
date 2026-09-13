using UnityEngine;
using UnityEngine.UI;

namespace LS.UnityEngine.UI;

[AddComponentMenu("UI/UIPolygonImage", 31)]
[RequireComponent(typeof(PolygonCollider2D))]
public class UIPolygonImage : Image
{
	private PolygonCollider2D _polygon;

	private PolygonCollider2D polygon
	{
		get
		{
			if (_polygon == null)
			{
				_polygon = GetComponent<PolygonCollider2D>();
			}
			return _polygon;
		}
	}

	public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
	{
		return polygon.OverlapPoint(eventCamera.ScreenToWorldPoint(screenPoint));
	}
}
