using System;
using System.Collections.Generic;

namespace RVO;

internal class KdTree
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

	private struct FloatPair
	{
		private float a_;

		private float b_;

		internal FloatPair(float a, float b)
		{
			a_ = a;
			b_ = b;
		}

		public static bool operator <(FloatPair pair1, FloatPair pair2)
		{
			if (!(pair1.a_ < pair2.a_))
			{
				if (!(pair2.a_ < pair1.a_))
				{
					return pair1.b_ < pair2.b_;
				}
				return false;
			}
			return true;
		}

		public static bool operator <=(FloatPair pair1, FloatPair pair2)
		{
			if (pair1.a_ != pair2.a_ || pair1.b_ != pair2.b_)
			{
				return pair1 < pair2;
			}
			return true;
		}

		public static bool operator >(FloatPair pair1, FloatPair pair2)
		{
			return !(pair1 <= pair2);
		}

		public static bool operator >=(FloatPair pair1, FloatPair pair2)
		{
			return !(pair1 < pair2);
		}
	}

	private class ObstacleTreeNode
	{
		internal Obstacle obstacle_;

		internal ObstacleTreeNode left_;

		internal ObstacleTreeNode right_;
	}

	private const int MAX_LEAF_SIZE = 10;

	private Agent[] agents_;

	private AgentTreeNode[] agentTree_;

	private ObstacleTreeNode obstacleTree_;

	internal void buildAgentTree()
	{
		int count = Simulator.Instance.agents_.Count;
		if (agents_ == null || Simulator.Instance.agentsDirty_)
		{
			if (agents_ == null || agents_.Length < count)
			{
				int num = Math.Max(64, count * 3 / 2);
				agents_ = new Agent[num];
				agentTree_ = new AgentTreeNode[2 * num];
				for (int i = 0; i < agentTree_.Length; i++)
				{
					agentTree_[i] = default(AgentTreeNode);
				}
			}
			Simulator.Instance.agents_.CopyTo(agents_);
		}
		if (count != 0)
		{
			buildAgentTreeRecursive(0, count, 0);
		}
	}

	internal void buildObstacleTree()
	{
		obstacleTree_ = new ObstacleTreeNode();
		IList<Obstacle> list = new List<Obstacle>(Simulator.Instance.obstacles_.Count);
		for (int i = 0; i < Simulator.Instance.obstacles_.Count; i++)
		{
			list.Add(Simulator.Instance.obstacles_[i]);
		}
		obstacleTree_ = buildObstacleTreeRecursive(list);
	}

	internal void computeAgentNeighbors(Agent agent, ref float rangeSq)
	{
		queryAgentTreeRecursive(agent, ref rangeSq, 0);
	}

	internal void computeObstacleNeighbors(Agent agent, float rangeSq)
	{
		queryObstacleTreeRecursive(agent, rangeSq, obstacleTree_);
	}

	internal bool queryVisibility(Vector2 q1, Vector2 q2, float radius)
	{
		return queryVisibilityRecursive(q1, q2, radius, obstacleTree_);
	}

	internal int queryNearAgent(Vector2 point, float radius)
	{
		float rangeSq = float.MaxValue;
		int agentNo = -1;
		queryAgentTreeRecursive(point, ref rangeSq, ref agentNo, 0);
		if (rangeSq < radius * radius)
		{
			return agentNo;
		}
		return -1;
	}

	private void buildAgentTreeRecursive(int begin, int end, int node)
	{
		agentTree_[node].begin_ = begin;
		agentTree_[node].end_ = end;
		agentTree_[node].minX_ = (agentTree_[node].maxX_ = agents_[begin].position_.x_);
		agentTree_[node].minY_ = (agentTree_[node].maxY_ = agents_[begin].position_.y_);
		for (int i = begin + 1; i < end; i++)
		{
			agentTree_[node].maxX_ = Math.Max(agentTree_[node].maxX_, agents_[i].position_.x_);
			agentTree_[node].minX_ = Math.Min(agentTree_[node].minX_, agents_[i].position_.x_);
			agentTree_[node].maxY_ = Math.Max(agentTree_[node].maxY_, agents_[i].position_.y_);
			agentTree_[node].minY_ = Math.Min(agentTree_[node].minY_, agents_[i].position_.y_);
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
			for (; j < num2 && (flag ? agents_[j].position_.x_ : agents_[j].position_.y_) < num; j++)
			{
			}
			while (num2 > j && (flag ? agents_[num2 - 1].position_.x_ : agents_[num2 - 1].position_.y_) >= num)
			{
				num2--;
			}
			if (j < num2)
			{
				Agent agent = agents_[j];
				agents_[j] = agents_[num2 - 1];
				agents_[num2 - 1] = agent;
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
		buildAgentTreeRecursive(begin, j, agentTree_[node].left_);
		buildAgentTreeRecursive(j, end, agentTree_[node].right_);
	}

	private ObstacleTreeNode buildObstacleTreeRecursive(IList<Obstacle> obstacles)
	{
		if (obstacles.Count == 0)
		{
			return null;
		}
		ObstacleTreeNode obstacleTreeNode = new ObstacleTreeNode();
		int num = 0;
		int num2 = obstacles.Count;
		int num3 = obstacles.Count;
		for (int i = 0; i < obstacles.Count; i++)
		{
			int num4 = 0;
			int num5 = 0;
			Obstacle obstacle = obstacles[i];
			Obstacle next_ = obstacle.next_;
			for (int j = 0; j < obstacles.Count; j++)
			{
				if (i != j)
				{
					Obstacle obstacle2 = obstacles[j];
					Obstacle next_2 = obstacle2.next_;
					float num6 = RVOMath.leftOf(obstacle.point_, next_.point_, obstacle2.point_);
					float num7 = RVOMath.leftOf(obstacle.point_, next_.point_, next_2.point_);
					if (num6 >= -1E-05f && num7 >= -1E-05f)
					{
						num4++;
					}
					else if (num6 <= 1E-05f && num7 <= 1E-05f)
					{
						num5++;
					}
					else
					{
						num4++;
						num5++;
					}
					if (new FloatPair(Math.Max(num4, num5), Math.Min(num4, num5)) >= new FloatPair(Math.Max(num2, num3), Math.Min(num2, num3)))
					{
						break;
					}
				}
			}
			if (new FloatPair(Math.Max(num4, num5), Math.Min(num4, num5)) < new FloatPair(Math.Max(num2, num3), Math.Min(num2, num3)))
			{
				num2 = num4;
				num3 = num5;
				num = i;
			}
		}
		IList<Obstacle> list = new List<Obstacle>(num2);
		for (int k = 0; k < num2; k++)
		{
			list.Add(null);
		}
		IList<Obstacle> list2 = new List<Obstacle>(num3);
		for (int l = 0; l < num3; l++)
		{
			list2.Add(null);
		}
		int num8 = 0;
		int num9 = 0;
		int num10 = num;
		Obstacle obstacle3 = obstacles[num10];
		Obstacle next_3 = obstacle3.next_;
		for (int m = 0; m < obstacles.Count; m++)
		{
			if (num10 == m)
			{
				continue;
			}
			Obstacle obstacle4 = obstacles[m];
			Obstacle next_4 = obstacle4.next_;
			float num11 = RVOMath.leftOf(obstacle3.point_, next_3.point_, obstacle4.point_);
			float num12 = RVOMath.leftOf(obstacle3.point_, next_3.point_, next_4.point_);
			if (num11 >= -1E-05f && num12 >= -1E-05f)
			{
				list[num8++] = obstacles[m];
				continue;
			}
			if (num11 <= 1E-05f && num12 <= 1E-05f)
			{
				list2[num9++] = obstacles[m];
				continue;
			}
			float num13 = RVOMath.det(next_3.point_ - obstacle3.point_, obstacle4.point_ - obstacle3.point_) / RVOMath.det(next_3.point_ - obstacle3.point_, obstacle4.point_ - next_4.point_);
			Vector2 point_ = obstacle4.point_ + num13 * (next_4.point_ - obstacle4.point_);
			Obstacle obstacle5 = new Obstacle();
			obstacle5.point_ = point_;
			obstacle5.previous_ = obstacle4;
			obstacle5.next_ = next_4;
			obstacle5.convex_ = true;
			obstacle5.direction_ = obstacle4.direction_;
			obstacle5.id_ = Simulator.Instance.obstacles_.Count;
			Simulator.Instance.obstacles_.Add(obstacle5);
			obstacle4.next_ = obstacle5;
			next_4.previous_ = obstacle5;
			if (num11 > 0f)
			{
				list[num8++] = obstacle4;
				list2[num9++] = obstacle5;
			}
			else
			{
				list2[num9++] = obstacle4;
				list[num8++] = obstacle5;
			}
		}
		obstacleTreeNode.obstacle_ = obstacle3;
		obstacleTreeNode.left_ = buildObstacleTreeRecursive(list);
		obstacleTreeNode.right_ = buildObstacleTreeRecursive(list2);
		return obstacleTreeNode;
	}

	private void queryAgentTreeRecursive(Vector2 position, ref float rangeSq, ref int agentNo, int node)
	{
		if (agentTree_[node].end_ - agentTree_[node].begin_ <= 10)
		{
			for (int i = agentTree_[node].begin_; i < agentTree_[node].end_; i++)
			{
				float num = RVOMath.absSq(position - agents_[i].position_);
				if (num < rangeSq)
				{
					rangeSq = num;
					agentNo = agents_[i].id_;
				}
			}
			return;
		}
		float num2 = RVOMath.sqr(Math.Max(0f, agentTree_[agentTree_[node].left_].minX_ - position.x_)) + RVOMath.sqr(Math.Max(0f, position.x_ - agentTree_[agentTree_[node].left_].maxX_)) + RVOMath.sqr(Math.Max(0f, agentTree_[agentTree_[node].left_].minY_ - position.y_)) + RVOMath.sqr(Math.Max(0f, position.y_ - agentTree_[agentTree_[node].left_].maxY_));
		float num3 = RVOMath.sqr(Math.Max(0f, agentTree_[agentTree_[node].right_].minX_ - position.x_)) + RVOMath.sqr(Math.Max(0f, position.x_ - agentTree_[agentTree_[node].right_].maxX_)) + RVOMath.sqr(Math.Max(0f, agentTree_[agentTree_[node].right_].minY_ - position.y_)) + RVOMath.sqr(Math.Max(0f, position.y_ - agentTree_[agentTree_[node].right_].maxY_));
		if (num2 < num3)
		{
			if (num2 < rangeSq)
			{
				queryAgentTreeRecursive(position, ref rangeSq, ref agentNo, agentTree_[node].left_);
				if (num3 < rangeSq)
				{
					queryAgentTreeRecursive(position, ref rangeSq, ref agentNo, agentTree_[node].right_);
				}
			}
		}
		else if (num3 < rangeSq)
		{
			queryAgentTreeRecursive(position, ref rangeSq, ref agentNo, agentTree_[node].right_);
			if (num2 < rangeSq)
			{
				queryAgentTreeRecursive(position, ref rangeSq, ref agentNo, agentTree_[node].left_);
			}
		}
	}

	private void queryAgentTreeRecursive(Agent agent, ref float rangeSq, int node)
	{
		if (agentTree_[node].end_ - agentTree_[node].begin_ <= 10)
		{
			for (int i = agentTree_[node].begin_; i < agentTree_[node].end_; i++)
			{
				agent.insertAgentNeighbor(agents_[i], ref rangeSq);
			}
			return;
		}
		float num = RVOMath.sqr(Math.Max(0f, agentTree_[agentTree_[node].left_].minX_ - agent.position_.x_)) + RVOMath.sqr(Math.Max(0f, agent.position_.x_ - agentTree_[agentTree_[node].left_].maxX_)) + RVOMath.sqr(Math.Max(0f, agentTree_[agentTree_[node].left_].minY_ - agent.position_.y_)) + RVOMath.sqr(Math.Max(0f, agent.position_.y_ - agentTree_[agentTree_[node].left_].maxY_));
		float num2 = RVOMath.sqr(Math.Max(0f, agentTree_[agentTree_[node].right_].minX_ - agent.position_.x_)) + RVOMath.sqr(Math.Max(0f, agent.position_.x_ - agentTree_[agentTree_[node].right_].maxX_)) + RVOMath.sqr(Math.Max(0f, agentTree_[agentTree_[node].right_].minY_ - agent.position_.y_)) + RVOMath.sqr(Math.Max(0f, agent.position_.y_ - agentTree_[agentTree_[node].right_].maxY_));
		if (num < num2)
		{
			if (num < rangeSq)
			{
				queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].left_);
				if (num2 < rangeSq)
				{
					queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].right_);
				}
			}
		}
		else if (num2 < rangeSq)
		{
			queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].right_);
			if (num < rangeSq)
			{
				queryAgentTreeRecursive(agent, ref rangeSq, agentTree_[node].left_);
			}
		}
	}

	private void queryObstacleTreeRecursive(Agent agent, float rangeSq, ObstacleTreeNode node)
	{
		if (node == null)
		{
			return;
		}
		Obstacle obstacle_ = node.obstacle_;
		Obstacle next_ = obstacle_.next_;
		float num = RVOMath.leftOf(obstacle_.point_, next_.point_, agent.position_);
		queryObstacleTreeRecursive(agent, rangeSq, (num >= 0f) ? node.left_ : node.right_);
		if (RVOMath.sqr(num) / RVOMath.absSq(next_.point_ - obstacle_.point_) < rangeSq)
		{
			if (num < 0f)
			{
				agent.insertObstacleNeighbor(node.obstacle_, rangeSq);
			}
			queryObstacleTreeRecursive(agent, rangeSq, (num >= 0f) ? node.right_ : node.left_);
		}
	}

	private bool queryVisibilityRecursive(Vector2 q1, Vector2 q2, float radius, ObstacleTreeNode node)
	{
		if (node == null)
		{
			return true;
		}
		Obstacle obstacle_ = node.obstacle_;
		Obstacle next_ = obstacle_.next_;
		float num = RVOMath.leftOf(obstacle_.point_, next_.point_, q1);
		float num2 = RVOMath.leftOf(obstacle_.point_, next_.point_, q2);
		float num3 = 1f / RVOMath.absSq(next_.point_ - obstacle_.point_);
		if (num >= 0f && num2 >= 0f)
		{
			if (queryVisibilityRecursive(q1, q2, radius, node.left_))
			{
				if (!(RVOMath.sqr(num) * num3 >= RVOMath.sqr(radius)) || !(RVOMath.sqr(num2) * num3 >= RVOMath.sqr(radius)))
				{
					return queryVisibilityRecursive(q1, q2, radius, node.right_);
				}
				return true;
			}
			return false;
		}
		if (num <= 0f && num2 <= 0f)
		{
			if (queryVisibilityRecursive(q1, q2, radius, node.right_))
			{
				if (!(RVOMath.sqr(num) * num3 >= RVOMath.sqr(radius)) || !(RVOMath.sqr(num2) * num3 >= RVOMath.sqr(radius)))
				{
					return queryVisibilityRecursive(q1, q2, radius, node.left_);
				}
				return true;
			}
			return false;
		}
		if (num >= 0f && num2 <= 0f)
		{
			if (queryVisibilityRecursive(q1, q2, radius, node.left_))
			{
				return queryVisibilityRecursive(q1, q2, radius, node.right_);
			}
			return false;
		}
		float num4 = RVOMath.leftOf(q1, q2, obstacle_.point_);
		float num5 = RVOMath.leftOf(q1, q2, next_.point_);
		float num6 = 1f / RVOMath.absSq(q2 - q1);
		if (num4 * num5 >= 0f && RVOMath.sqr(num4) * num6 > RVOMath.sqr(radius) && RVOMath.sqr(num5) * num6 > RVOMath.sqr(radius) && queryVisibilityRecursive(q1, q2, radius, node.left_))
		{
			return queryVisibilityRecursive(q1, q2, radius, node.right_);
		}
		return false;
	}
}
