namespace Spine.Unity;

public enum UpdateMode
{
	Nothing = 0,
	OnlyAnimationStatus = 1,
	OnlyEventTimelines = 4,
	EverythingExceptMesh = 2,
	FullUpdate = 3
}
