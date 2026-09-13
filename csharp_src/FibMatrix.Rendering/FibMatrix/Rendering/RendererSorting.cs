using UnityEngine;

namespace FibMatrix.Rendering;

[ExecuteAlways]
public class RendererSorting : MonoBehaviour
{
	public int m_SortingOrder;

	public int m_SortingLayerID;

	private MeshRenderer meshRenderer;

	private void OnEnable()
	{
		DoUpdate();
	}

	private void DoUpdate()
	{
		if (meshRenderer == null)
		{
			meshRenderer = GetComponent<MeshRenderer>();
		}
		if (meshRenderer != null)
		{
			meshRenderer.sortingLayerID = m_SortingLayerID;
			meshRenderer.sortingOrder = m_SortingOrder;
		}
	}
}
