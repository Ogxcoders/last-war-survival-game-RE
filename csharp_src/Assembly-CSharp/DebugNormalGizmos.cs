using UnityEngine;

[ExecuteInEditMode]
public class DebugNormalGizmos : MonoBehaviour
{
	private Mesh mesh;

	private void Awake()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		if (component != null)
		{
			mesh = component.sharedMesh;
		}
	}

	private void OnDrawGizmos()
	{
		if (mesh != null)
		{
			Vector3[] normals = mesh.normals;
			Vector3[] vertices = mesh.vertices;
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 vector = base.transform.TransformPoint(vertices[i]);
				Vector3 to = vector + normals[i];
				Gizmos.DrawLine(vector, to);
			}
		}
	}
}
