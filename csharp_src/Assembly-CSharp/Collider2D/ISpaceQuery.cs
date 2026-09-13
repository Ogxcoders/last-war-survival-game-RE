namespace Collider2D;

public interface ISpaceQuery
{
	void Build(Agent[] agents, int agentCount, int frame);

	void QueryNearAgents(float posX, float posY, float radis, ref int[] queriedAgentIndices, int layerMask, out int queriedCount);

	void DebugLine();

	void Clear();
}
