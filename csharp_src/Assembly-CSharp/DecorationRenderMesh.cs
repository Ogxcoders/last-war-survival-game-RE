using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class DecorationRenderMesh
{
	public Mesh mesh;

	public int subMeshIndex;

	public Material material;

	public ShadowCastingMode shadowCastingMode;

	public bool receiveShadows;

	public AABB localBounds;

	public int layer;

	public DecorationTransformInfo localTransformInfo;
}
