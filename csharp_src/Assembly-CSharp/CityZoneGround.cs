using System.Collections.Generic;
using UnityEngine;

public class CityZoneGround
{
	private InstanceRequest _instanceRequest;

	private string _path;

	private Transform _parent;

	private Mesh _mesh;

	private MeshFilter _meshFilter;

	private List<Vector3> _vertices;

	private List<int> _triangles;

	private List<Vector2> _uvs;

	public void InitGroundData(Transform parent, string path)
	{
		_parent = parent;
		if (_path != path && _instanceRequest != null)
		{
			_instanceRequest.Destroy();
			_instanceRequest = null;
		}
		_path = path;
	}

	public void ShowMesh(List<Vector3> vertices, List<Vector2> uv, List<int> triangles)
	{
		_vertices = vertices;
		_uvs = uv;
		_triangles = triangles;
		if (_instanceRequest == null && !string.IsNullOrEmpty(_path))
		{
			_instanceRequest = GameEntry.Resource.InstantiateAsync(_path);
			_instanceRequest.completed += delegate
			{
				if (_instanceRequest.gameObject != null)
				{
					GameObject gameObject = _instanceRequest.gameObject;
					gameObject.transform.SetParent(_parent);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localRotation = Quaternion.identity;
					gameObject.transform.localScale = Vector3.one;
					_meshFilter = gameObject.GetComponent<MeshFilter>();
					_mesh = new Mesh();
					RebuildMesh();
				}
			};
		}
		else if (_instanceRequest?.gameObject != null && _meshFilter != null && _mesh != null)
		{
			RebuildMesh();
		}
	}

	private void RebuildMesh()
	{
		_mesh.Clear();
		_mesh.vertices = _vertices.ToArray();
		_mesh.triangles = _triangles.ToArray();
		_mesh.uv = _uvs.ToArray();
		_mesh.RecalculateNormals();
		_mesh.RecalculateBounds();
		_meshFilter.mesh = _mesh;
	}

	public void UnloadGround()
	{
		if (_instanceRequest != null)
		{
			_instanceRequest.Destroy();
			_instanceRequest = null;
		}
		_path = "";
		_parent = null;
		_mesh = null;
		_meshFilter = null;
	}
}
