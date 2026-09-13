public interface IWorldLodWatcher
{
	long Uid { get; }

	void UpdateLod(int lod);
}
