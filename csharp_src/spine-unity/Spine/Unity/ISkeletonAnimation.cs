namespace Spine.Unity;

public interface ISkeletonAnimation : ISpineComponent
{
	Skeleton Skeleton { get; }

	event UpdateBonesDelegate UpdateLocal;

	event UpdateBonesDelegate UpdateWorld;

	event UpdateBonesDelegate UpdateComplete;
}
