using System;

namespace Leopotam.EcsLite;

public interface IEcsPool
{
	void Resize(int capacity);

	bool Has(int entity);

	void Del(int entity);

	void AddRaw(int entity, object dataRaw);

	object GetRaw(int entity);

	void SetRaw(int entity, object dataRaw);

	int GetId();

	Type GetComponentType();

	void Copy(int srcEntity, int dstEntity);

	object TakeSnapshot(int entity, object env);

	void RestoreSnapshot(int entity, object data, object env, bool overwrite);

	bool IsSnapshotEqual(object lhs, object rhs);

	void RestoreSnapshotId(short id);
}
