namespace UnityEngine.Experimental.AI;

public struct NavMeshLocation
{
	public PolygonId polygon { get; }

	public Vector3 position { get; }

	internal NavMeshLocation(Vector3 position, PolygonId polygon)
	{
		this.position = position;
		this.polygon = polygon;
	}
}
