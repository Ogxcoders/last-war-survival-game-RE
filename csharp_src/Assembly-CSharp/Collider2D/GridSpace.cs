using System;
using UnityEngine;

namespace Collider2D;

public class GridSpace : ISpaceQuery
{
	private struct GridNode
	{
		internal int frame_;

		internal int begin_;

		internal int next_;

		internal int count_;
	}

	private const float GRID_SIZE = 4f;

	private const float DIVDED_ONE_BY_GRID_SIZE = 0.25f;

	private const int GRID_AGENT_INDICES_STEP = 2;

	private const int GRID_INDICES_COUNT = 60;

	private Agent[] agents_;

	private int agentsCount_;

	private GridNode[] agentTree_;

	private float agentMinPosX;

	private float agentMinPosY;

	private float agentMaxPosX;

	private float agentMaxPosY;

	private int gridWidthCount;

	private int gridHeightCount;

	private int gridTotalCount;

	private const int AGENT_DEFAULT_CAPACITY = 4096;

	private int[] gridAgentIndices_;

	private int curFrame_;

	private const int AGENT_NODE_DEFAULT_CAPACITY = 444;

	public void Build(Agent[] agents, int agentCount, int frame)
	{
		agents_ = agents;
		agentsCount_ = agentCount;
		if (agents_ == null || agentsCount_ <= 0)
		{
			return;
		}
		if (gridAgentIndices_ == null)
		{
			gridAgentIndices_ = new int[4096];
		}
		Agent agent = agents[0];
		agentMinPosX = agent.TransformedAABBMinX;
		agentMinPosY = agent.TransformedAABBMinY;
		agentMaxPosX = agent.TransformedAABBMaxX;
		agentMaxPosY = agent.TransformedAABBMaxY;
		if (agentsCount_ > 1)
		{
			for (int i = 1; i < agentsCount_; i++)
			{
				Agent agent2 = agents_[i];
				float transformedAABBMinX = agent2.TransformedAABBMinX;
				float transformedAABBMinY = agent2.TransformedAABBMinY;
				float transformedAABBMaxX = agent2.TransformedAABBMaxX;
				float transformedAABBMaxY = agent2.TransformedAABBMaxY;
				if (transformedAABBMinX < agentMinPosX)
				{
					agentMinPosX = transformedAABBMinX;
				}
				if (transformedAABBMaxX > agentMaxPosX)
				{
					agentMaxPosX = transformedAABBMaxX;
				}
				if (transformedAABBMinY < agentMinPosY)
				{
					agentMinPosY = transformedAABBMinY;
				}
				if (transformedAABBMaxY > agentMaxPosY)
				{
					agentMaxPosY = transformedAABBMaxY;
				}
			}
		}
		agentMinPosX = Mathf.Floor(agentMinPosX);
		agentMinPosY = Mathf.Floor(agentMinPosY);
		agentMaxPosX = Mathf.Ceil(agentMaxPosX);
		agentMaxPosY = Mathf.Ceil(agentMaxPosY);
		gridWidthCount = Mathf.CeilToInt((agentMaxPosX - agentMinPosX) * 0.25f);
		if (gridWidthCount == 0)
		{
			gridWidthCount = 1;
		}
		gridHeightCount = Mathf.CeilToInt((agentMaxPosY - agentMinPosY) * 0.25f);
		if (gridHeightCount == 0)
		{
			gridHeightCount = 1;
		}
		gridTotalCount = gridWidthCount * gridHeightCount;
		if (agentTree_ == null)
		{
			int num = Mathf.Max(444, gridTotalCount);
			agentTree_ = new GridNode[num];
		}
		else if (gridTotalCount > agentTree_.Length)
		{
			int num2 = agentTree_.Length * 2;
			int num3 = ((num2 < gridTotalCount) ? gridTotalCount : num2);
			agentTree_ = new GridNode[num3];
		}
		int indicesStart = 0;
		for (int j = 0; j < agentsCount_; j++)
		{
			Agent agent3 = agents_[j];
			float transformedAABBMinX2 = agent3.TransformedAABBMinX;
			float transformedAABBMinY2 = agent3.TransformedAABBMinY;
			int num4 = (int)((transformedAABBMinX2 - agentMinPosX) * 0.25f);
			if (num4 < 0)
			{
				num4 = 0;
			}
			else if (num4 >= gridWidthCount)
			{
				num4 = gridWidthCount - 1;
			}
			int num5 = (int)((transformedAABBMinY2 - agentMinPosY) * 0.25f);
			if (num5 < 0)
			{
				num5 = 0;
			}
			else if (num5 >= gridHeightCount)
			{
				num5 = gridHeightCount - 1;
			}
			float transformedAABBMaxX2 = agent3.TransformedAABBMaxX;
			float transformedAABBMaxY2 = agent3.TransformedAABBMaxY;
			int num6 = (int)((transformedAABBMaxX2 - agentMinPosX) * 0.25f);
			if (num6 < 0)
			{
				num6 = 0;
			}
			else if (num6 >= gridWidthCount)
			{
				num6 = gridWidthCount - 1;
			}
			int num7 = (int)((transformedAABBMaxY2 - agentMinPosY) * 0.25f);
			if (num7 < 0)
			{
				num7 = 0;
			}
			else if (num7 >= gridHeightCount)
			{
				num7 = gridHeightCount - 1;
			}
			int layer = agent3.Layer;
			for (int k = num5; k <= num7; k++)
			{
				int num8 = k * gridWidthCount;
				for (int l = num4; l <= num6; l++)
				{
					int gridIndex = num8 + l;
					AddAgentToGrid(j, layer, ref indicesStart, gridIndex, frame);
				}
			}
		}
		curFrame_ = frame;
	}

	private void AddAgentToGrid(int agentIndex, int agentLayer, ref int indicesStart, int gridIndex, int frame)
	{
		if (frame > agentTree_[gridIndex].frame_)
		{
			agentTree_[gridIndex].frame_ = frame;
			agentTree_[gridIndex].begin_ = indicesStart;
			agentTree_[gridIndex].next_ = indicesStart;
			agentTree_[gridIndex].count_ = 60;
			indicesStart += 60;
			int num = gridAgentIndices_.Length;
			if (indicesStart >= num)
			{
				int num2 = num * 2;
				int[] destinationArray = new int[(indicesStart > num2) ? indicesStart : num2];
				Array.Copy(gridAgentIndices_, 0, destinationArray, 0, num);
				gridAgentIndices_ = destinationArray;
			}
		}
		int next_ = agentTree_[gridIndex].next_;
		int begin_ = agentTree_[gridIndex].begin_;
		int count_ = agentTree_[gridIndex].count_;
		int num3 = next_ + 2;
		agentTree_[gridIndex].next_ = num3;
		int num4 = begin_ + count_;
		if (num3 >= num4)
		{
			agentTree_[gridIndex].count_ = count_ * 2;
			int length = indicesStart;
			int num5 = indicesStart - 2;
			indicesStart += count_;
			int num6 = gridAgentIndices_.Length;
			if (indicesStart >= num6)
			{
				int num7 = num6 * 2;
				int[] destinationArray2 = new int[(indicesStart >= num7) ? indicesStart : num7];
				Array.Copy(gridAgentIndices_, 0, destinationArray2, 0, length);
				gridAgentIndices_ = destinationArray2;
			}
			for (int num8 = num5; num8 >= num4; num8 -= 2)
			{
				int num9 = num8 + count_;
				gridAgentIndices_[num9] = gridAgentIndices_[num8];
				gridAgentIndices_[num9 + 1] = gridAgentIndices_[num8 + 1];
			}
			for (int i = 0; i < agentTree_.Length; i++)
			{
				GridNode gridNode = agentTree_[i];
				if (gridNode.frame_ == frame && gridNode.begin_ >= num4 && i != gridIndex)
				{
					agentTree_[i].begin_ = gridNode.begin_ + count_;
					agentTree_[i].next_ = gridNode.next_ + count_;
				}
			}
		}
		gridAgentIndices_[next_] = agentIndex;
		gridAgentIndices_[next_ + 1] = agentLayer;
	}

	public void DebugLine()
	{
		if (agentTree_ == null)
		{
			return;
		}
		for (int i = 0; i < gridHeightCount; i++)
		{
			int num = i * gridWidthCount;
			for (int j = 0; j < gridWidthCount; j++)
			{
				int num2 = num + j;
				if (agentTree_[num2].frame_ >= curFrame_)
				{
					Vector3 vector = new Vector3(agentMinPosX + (float)j * 4f, 0f, agentMinPosY + (float)i * 4f);
					Vector3 vector2 = new Vector3(agentMinPosX + (float)j * 4f + 4f, 0f, agentMinPosY + (float)i * 4f);
					Vector3 vector3 = new Vector3(agentMinPosX + (float)j * 4f, 0f, agentMinPosY + (float)i * 4f + 4f);
					Vector3 vector4 = new Vector3(agentMinPosX + (float)j * 4f + 4f, 0f, agentMinPosY + (float)i * 4f + 4f);
					Debug.DrawLine(vector, vector2, Color.red);
					Debug.DrawLine(vector2, vector4, Color.red);
					Debug.DrawLine(vector4, vector3, Color.red);
					Debug.DrawLine(vector3, vector, Color.red);
				}
			}
		}
	}

	public void QueryNearAgents(float posX, float posY, float radis, ref int[] queriedAgentIndices, int layerMask, out int queriedCount)
	{
		queriedCount = 0;
		if (agentTree_ == null)
		{
			return;
		}
		float num = posX - radis - agentMinPosX;
		float num2 = posY - radis - agentMinPosY;
		float num3 = posX + radis - agentMinPosX;
		float num4 = posY + radis - agentMinPosY;
		int num5 = 0;
		if (num > 0f)
		{
			num5 = (int)(num * 0.25f);
		}
		int num6 = 0;
		if (num3 > 0f)
		{
			int num7 = (int)(num3 * 0.25f);
			int num8 = gridWidthCount - 1;
			num6 = ((num7 > num8) ? num8 : num7);
		}
		int num9 = 0;
		if (num2 > 0f)
		{
			num9 = (int)(num2 * 0.25f);
		}
		int num10 = 0;
		if (num4 > 0f)
		{
			int num11 = (int)(num4 * 0.25f);
			int num12 = gridHeightCount - 1;
			num10 = ((num11 > num12) ? num12 : num11);
		}
		for (int i = num9; i <= num10; i++)
		{
			int num13 = i * gridWidthCount;
			for (int j = num5; j <= num6; j++)
			{
				int num14 = num13 + j;
				GridNode gridNode = agentTree_[num14];
				if (gridNode.frame_ < curFrame_)
				{
					continue;
				}
				int next_ = gridNode.next_;
				for (int k = gridNode.begin_; k < next_; k += 2)
				{
					int num15 = gridAgentIndices_[k];
					int num16 = gridAgentIndices_[k + 1];
					bool flag = false;
					for (int l = 0; l < queriedCount; l++)
					{
						if (queriedAgentIndices[l] == num15)
						{
							flag = true;
							break;
						}
					}
					if (!flag && (layerMask & num16) != 0)
					{
						queriedAgentIndices[queriedCount++] = num15;
						if (queriedCount >= queriedAgentIndices.Length)
						{
							int num17 = queriedAgentIndices.Length;
							int num18 = num17 * 2;
							int[] array = new int[(queriedCount >= num18) ? queriedCount : num18];
							Array.Copy(queriedAgentIndices, 0, array, 0, num17);
							queriedAgentIndices = array;
						}
					}
				}
			}
		}
	}

	public void Clear()
	{
		if (agentTree_ != null)
		{
			Array.Clear(agentTree_, 0, agentTree_.Length);
		}
		if (gridAgentIndices_ != null)
		{
			Array.Clear(gridAgentIndices_, 0, gridAgentIndices_.Length);
		}
		agents_ = null;
	}
}
