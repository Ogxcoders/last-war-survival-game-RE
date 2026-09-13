using UnityEngine;

namespace FibMatrix.Rendering;

public class ObjectCounter : MonoBehaviour
{
	[ContextMenu("Count")]
	private void Count()
	{
		MeshFilter[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshFilter>();
		MeshFilter[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<MeshFilter>(includeInactive: true);
		int num = 0;
		MeshFilter[] array = componentsInChildren;
		foreach (MeshFilter meshFilter in array)
		{
			if (meshFilter != null && meshFilter.sharedMesh != null)
			{
				num += meshFilter.sharedMesh.vertexCount;
			}
		}
		Debug.Log($"active mesh filter count:{componentsInChildren.Length}, vertex count:{num}");
		num = 0;
		array = componentsInChildren2;
		foreach (MeshFilter meshFilter2 in array)
		{
			if (meshFilter2 != null && meshFilter2.sharedMesh != null)
			{
				num += meshFilter2.sharedMesh.vertexCount;
			}
		}
		Debug.Log($"all mesh filter count:{componentsInChildren2.Length}, vertex count:{num}");
	}
}
