namespace Leopotam.EcsLite;

public interface IEcsAutoSnapshot<T> where T : struct
{
	object TakeSnapshot(ref T c, EcsWorld world, int entity, object env);

	void RestoreSnapshot(ref T c, EcsWorld world, int entity, object data, object env);

	bool IsSnapshotEqual(object a, object b, EcsWorld world);
}
