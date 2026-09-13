using UnityEngine;

public sealed class WorldDecorationBatchingAssetObject : ScriptableObject
{
	public WorldDecorationBatchingAsset asset;

	public AABB bounds;

	public WorldDecorationTransformInfo[] transforms;

	public int[] lodRanges;
}
