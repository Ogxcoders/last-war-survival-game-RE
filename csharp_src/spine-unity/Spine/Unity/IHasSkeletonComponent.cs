namespace Spine.Unity;

public interface IHasSkeletonComponent : ISpineComponent
{
	ISkeletonComponent SkeletonComponent { get; }
}
