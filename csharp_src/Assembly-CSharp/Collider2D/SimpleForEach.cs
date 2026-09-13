using System.Collections.Generic;
using UnityEngine;

namespace Collider2D;

public class SimpleForEach
{
	private Agent[] agents_;

	private int agentsCount_;

	private Dictionary<long, int> agentIdMap_;

	public void Build(Agent[] agents, int agentCount, int frame, Dictionary<long, int> agentIdMap)
	{
		agents_ = agents;
		agentsCount_ = agentCount;
		agentIdMap_ = agentIdMap;
	}

	public void DebugLine()
	{
	}

	public void QueryNearAgents(Vector2 point, float radis, List<int> agents, int layerMask)
	{
		float num = radis * radis;
		float x = point.x;
		float y = point.y;
		for (int i = 0; i < agentsCount_; i++)
		{
			Agent agent = agents_[i];
			float posX = agent.PosX;
			float posY = agent.PosY;
			int layer = agent.Layer;
			float num2 = x - posX;
			float num3 = y - posY;
			float num4 = num2 * num2 + num3 * num3;
			bool flag = (layerMask & layer) != 0;
			if (num4 < num && flag)
			{
				agents.Add(agent.Id);
			}
		}
	}

	public void Clear()
	{
		agents_ = null;
		agentIdMap_ = null;
	}
}
