using UnityEngine;

public sealed class WorldDecorationBatchingAsset : ScriptableObject
{
	public WorldDecorationRenderMesh[] meshes;

	public AABB localBounds;
}
