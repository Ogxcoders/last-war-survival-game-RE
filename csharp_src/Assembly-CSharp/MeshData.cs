using System;
using UnityEngine;

[Serializable]
public class MeshData
{
	[SerializeField]
	[HideInInspector]
	private byte[] m_Data;

	private Mesh m_Mesh;

	public byte[] Data => m_Data;

	public void SetMesh(Mesh aMesh)
	{
		m_Mesh = aMesh;
		if (aMesh == null)
		{
			m_Data = null;
		}
		else
		{
			m_Data = MeshSerializer.SerializeMesh(m_Mesh);
		}
	}

	public Mesh GetMesh()
	{
		if (m_Mesh == null && m_Data != null)
		{
			m_Mesh = MeshSerializer.DeserializeMesh(m_Data);
		}
		return m_Mesh;
	}
}
