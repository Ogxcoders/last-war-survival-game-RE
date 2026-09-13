public interface ISceneLODNode
{
	LODType GetLODType();

	int CurrentLOD();

	void UpdateLOD(int level);

	float GetCost();

	float GetDynamicCost();
}
