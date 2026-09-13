public interface IWorldDelayDestroyObject
{
	bool CanDestroy { get; }

	void DestroyImmediate();
}
