using UnityEngine;

namespace FibMatrix.Rendering;

[ExecuteAlways]
public class FrustumCulling : MonoBehaviour
{
	private MeshFilter meshFilter;

	public bool enable = true;

	public float sizeNotBeCulled = 1024f;

	private Bounds bounds => meshFilter.mesh.bounds;

	private void Start()
	{
		OnStateChanged();
	}

	private void OnStateChanged()
	{
		if (meshFilter == null)
		{
			meshFilter = GetComponent<MeshFilter>();
		}
		if (meshFilter != null && meshFilter.mesh != null)
		{
			if (enable)
			{
				BoundsUtilities.ReCalculateBounds(meshFilter.mesh);
				return;
			}
			Bounds bounds = default(Bounds);
			bounds.SetMinMax(Vector3.one * (0f - sizeNotBeCulled), Vector3.one * sizeNotBeCulled);
			meshFilter.mesh.bounds = bounds;
		}
	}
}
