namespace Spine.Unity;

public interface ISkeletonComponent : ISpineComponent
{
	SkeletonDataAsset SkeletonDataAsset { get; }

	Skeleton Skeleton { get; }
}
