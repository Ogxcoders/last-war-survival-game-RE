using System.Collections.Generic;
using UnityEngine;

namespace Collider2D;

public class KdTree
{
	private struct AgentTreeNode
	{
		internal int begin_;

		internal int end_;

		internal int left_;

		internal int right_;

		internal float maxX_;

		internal float maxY_;

		internal float minX_;

		internal float minY_;
	}

	private Agent[] agents_;

	private int agentsCount_;

	private Dictionary<long, int> agentIdMap_;

	private const int agentTreeCapacity_ = 1024;

	private AgentTreeNode[] agentTree_;

	private const int MAX_LEAF_SIZE = 10;

	public void Build(Agent[] agents, int agentCount, int frame, Dictionary<long, int> agentIdMap)
	{
		agents_ = agents;
		agentsCount_ = agentCount;
		agentIdMap_ = agentIdMap;
		if (agents_ != null && agentsCount_ > 0)
		{
			int num = agentsCount_ * 2;
			if (agentTree_ == null)
			{
				int num2 = Mathf.Max(1024, num);
				agentTree_ = new AgentTreeNode[num2];
			}
			else if (num > agentTree_.Length)
			{
				int num3 = agentTree_.Length * 2;
				int num4 = ((num3 < num) ? num : num3);
				agentTree_ = new AgentTreeNode[num4];
			}
			BuildAgentTreeRecursive(0, agentsCount_, 0);
		}
	}

	private void BuildAgentTreeRecursive(int begin, int end, int node)
	{
		agentTree_[node].begin_ = begin;
		agentTree_[node].end_ = end;
		agentTree_[node].minX_ = (agentTree_[node].maxX_ = agents_[begin].PosX);
		agentTree_[node].minY_ = (agentTree_[node].maxY_ = agents_[begin].PosY);
		for (int i = begin + 1; i < end; i++)
		{
			float posX = agents_[i].PosX;
			float posY = agents_[i].PosY;
			agentTree_[node].maxX_ = Mathf.Max(agentTree_[node].maxX_, posX);
			agentTree_[node].minX_ = Mathf.Min(agentTree_[node].minX_, posX);
			agentTree_[node].maxY_ = Mathf.Max(agentTree_[node].maxY_, posY);
			agentTree_[node].minY_ = Mathf.Min(agentTree_[node].minY_, posY);
		}
		if (end - begin <= 10)
		{
			return;
		}
		bool flag = agentTree_[node].maxX_ - agentTree_[node].minX_ > agentTree_[node].maxY_ - agentTree_[node].minY_;
		float num = 0.5f * (flag ? (agentTree_[node].maxX_ + agentTree_[node].minX_) : (agentTree_[node].maxY_ + agentTree_[node].minY_));
		int j = begin;
		int num2 = end;
		while (j < num2)
		{
			for (; j < num2 && (flag ? agents_[j].PosX : agents_[j].PosY) < num; j++)
			{
			}
			while (num2 > j && (flag ? agents_[num2 - 1].PosX : agents_[num2 - 1].PosY) >= num)
			{
				num2--;
			}
			if (j < num2)
			{
				Agent agent = agents_[j];
				agents_[j] = agents_[num2 - 1];
				agentIdMap_[agents_[j].Id] = j;
				agents_[num2 - 1] = agent;
				agentIdMap_[agent.Id] = num2 - 1;
				j++;
				num2--;
			}
		}
		int num3 = j - begin;
		if (num3 == 0)
		{
			num3++;
			j++;
			num2++;
		}
		agentTree_[node].left_ = node + 1;
		agentTree_[node].right_ = node + 2 * num3;
		BuildAgentTreeRecursive(begin, j, agentTree_[node].left_);
		BuildAgentTreeRecursive(j, end, agentTree_[node].right_);
	}

	public void QueryNearAgents(Vector2 point, float radis, List<int> agents, int layerMask)
	{
		if (agentTree_ != null)
		{
			float radiusSqr = radis * radis;
			QueryAgentTreeRecursive(point.x, point.y, radiusSqr, 0, agents, layerMask);
		}
	}

	private void QueryAgentTreeRecursive(float posX, float posY, float radiusSqr, int node, List<int> queryAgents, int layerMask)
	{
		AgentTreeNode agentTreeNode = agentTree_[node];
		int end_ = agentTreeNode.end_;
		int begin_ = agentTreeNode.begin_;
		int left_ = agentTreeNode.left_;
		int right_ = agentTreeNode.right_;
		if (end_ - begin_ <= 10)
		{
			for (int i = begin_; i < end_; i += 2)
			{
				Agent agent = agents_[i];
				float posX2 = agent.PosX;
				float posY2 = agent.PosY;
				int layer = agent.Layer;
				float num = posX - posX2;
				float num2 = posY - posY2;
				float num3 = num * num + num2 * num2;
				bool flag = (layerMask & layer) != 0;
				if (num3 < radiusSqr && flag)
				{
					queryAgents.Add(agent.Id);
				}
				int num4 = i + 1;
				if (num4 < end_)
				{
					Agent agent2 = agents_[num4];
					float posX3 = agent2.PosX;
					float posY3 = agent2.PosY;
					float num5 = posX - posX3;
					float num6 = posY - posY3;
					float num7 = num5 * num5 + num6 * num6;
					bool flag2 = (layerMask & agent2.Layer) != 0;
					if (num7 < radiusSqr && flag2)
					{
						queryAgents.Add(agent2.Id);
					}
				}
			}
			return;
		}
		AgentTreeNode agentTreeNode2 = agentTree_[left_];
		float num8 = agentTreeNode2.minX_ - posX;
		num8 = ((num8 > 0f) ? num8 : 0f);
		float num9 = num8 * num8;
		float maxX_ = agentTreeNode2.maxX_;
		float num10 = posX - maxX_;
		num10 = ((num10 > 0f) ? num10 : 0f);
		float num11 = num10 * num10;
		float num12 = agentTreeNode2.minY_ - posY;
		num12 = ((num12 > 0f) ? num12 : 0f);
		float num13 = num12 * num12;
		float maxY_ = agentTreeNode2.maxY_;
		float num14 = posY - maxY_;
		num14 = ((num14 > 0f) ? num14 : 0f);
		float num15 = num14 * num14;
		float num16 = num9 + num11 + num13 + num15;
		AgentTreeNode agentTreeNode3 = agentTree_[right_];
		float num17 = agentTreeNode3.minX_ - posX;
		num17 = ((num17 > 0f) ? num17 : 0f);
		float num18 = num17 * num17;
		float maxX_2 = agentTreeNode3.maxX_;
		float num19 = posX - maxX_2;
		num19 = ((num19 > 0f) ? num19 : 0f);
		float num20 = num19 * num19;
		float num21 = agentTreeNode3.minY_ - posY;
		num21 = ((num21 > 0f) ? num21 : 0f);
		float num22 = num21 * num21;
		float maxY_2 = agentTreeNode3.maxY_;
		float num23 = posY - maxY_2;
		num23 = ((num23 > 0f) ? num23 : 0f);
		float num24 = num23 * num23;
		float num25 = num18 + num20 + num22 + num24;
		if (num16 < num25)
		{
			if (num16 < radiusSqr)
			{
				QueryAgentTreeRecursive(posX, posY, radiusSqr, left_, queryAgents, layerMask);
				if (num25 < radiusSqr)
				{
					QueryAgentTreeRecursive(posX, posY, radiusSqr, right_, queryAgents, layerMask);
				}
			}
		}
		else if (num25 < radiusSqr)
		{
			QueryAgentTreeRecursive(posX, posY, radiusSqr, right_, queryAgents, layerMask);
			if (num16 < radiusSqr)
			{
				QueryAgentTreeRecursive(posX, posY, radiusSqr, left_, queryAgents, layerMask);
			}
		}
	}

	public void DebugLine()
	{
		DebugKDTree(0);
	}

	private void DebugKDTree(int nodeIndex)
	{
		if (agentTree_ != null && agentTree_[nodeIndex].end_ != 0)
		{
			int begin_ = agentTree_[nodeIndex].begin_;
			if (agentTree_[nodeIndex].end_ - begin_ > 10)
			{
				Vector3 vector = new Vector3(agentTree_[nodeIndex].minX_, 0f, agentTree_[nodeIndex].minY_);
				Vector3 vector2 = new Vector3(agentTree_[nodeIndex].maxX_, 0f, agentTree_[nodeIndex].minY_);
				Vector3 vector3 = new Vector3(agentTree_[nodeIndex].minX_, 0f, agentTree_[nodeIndex].maxY_);
				Vector3 vector4 = new Vector3(agentTree_[nodeIndex].maxX_, 0f, agentTree_[nodeIndex].maxY_);
				Debug.DrawLine(vector, vector2, Color.red);
				Debug.DrawLine(vector2, vector4, Color.red);
				Debug.DrawLine(vector4, vector3, Color.red);
				Debug.DrawLine(vector3, vector, Color.red);
				DebugKDTree(agentTree_[nodeIndex].left_);
				DebugKDTree(agentTree_[nodeIndex].right_);
			}
		}
	}

	public void Clear()
	{
		agents_ = null;
		agentIdMap_ = null;
	}
}
