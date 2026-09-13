using UnityEngine;

public class DecorationRenderAsset : ScriptableObject
{
	public int guid;

	public string assetPath;

	public DecorationLodMesh[] lodMeshes;

	public DecorationRenderState[] states;
}
