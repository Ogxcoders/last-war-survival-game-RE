using System;
using System.Collections.Generic;
using UnityEngine;

namespace Collider2D;

public static class Collider2DUtils
{
	private static ISpaceQuery _spaceQuery;

	private static Agent[] _agents;

	private static int _agentCount;

	private static Dictionary<long, int> _agentIdMap;

	private static int[] _queriedAgents;

	private static Transform[] _agentTransforms;

	private static bool _isDebug;

	private static List<int> _sortedList;

	private static void Init()
	{
		if (_agents == null)
		{
			_agents = new Agent[512];
		}
		if (_agentIdMap == null)
		{
			_agentIdMap = new Dictionary<long, int>(512);
		}
		if (_spaceQuery == null)
		{
			_spaceQuery = new GridSpace();
		}
		if (_queriedAgents == null)
		{
			_queriedAgents = new int[100];
		}
		if (_agentTransforms == null)
		{
			_agentTransforms = new Transform[512];
		}
		if (_sortedList == null)
		{
			_sortedList = new List<int>();
		}
	}

	public static bool TryGetAgent(int uid, out Agent agent)
	{
		Init();
		agent = default(Agent);
		if (!_agentIdMap.TryGetValue(uid, out var value))
		{
			return false;
		}
		agent = _agents[value];
		return true;
	}

	public static void SetDebug(bool value)
	{
		_isDebug = value;
	}

	public static bool IsDebug()
	{
		return _isDebug;
	}

	public static void AddAgent(int uid, int colliderId, Collider unityCollider)
	{
		if (unityCollider == null)
		{
			Debug.LogError($"unityCollider is null! agentId:{uid}");
			return;
		}
		Init();
		bool flag = false;
		Transform transform = unityCollider.transform;
		if (_agentIdMap.TryGetValue(uid, out var value))
		{
			_agents[value].ColliderId = colliderId;
			if (_agentTransforms[value] != transform)
			{
				flag = true;
				_agentTransforms[value] = transform;
			}
		}
		else
		{
			value = _agentCount++;
			_agentIdMap[uid] = value;
			_agents[value] = new Agent
			{
				Id = uid,
				ColliderId = colliderId
			};
			_agentTransforms[value] = transform;
			if (_agentCount >= _agents.Length)
			{
				Agent[] array = new Agent[_agents.Length << 1];
				Array.Copy(_agents, array, _agents.Length);
				_agents = array;
			}
			if (_agentCount >= _agentTransforms.Length)
			{
				Transform[] array2 = new Transform[_agentTransforms.Length << 1];
				Array.Copy(_agentTransforms, array2, _agentTransforms.Length);
				_agentTransforms = array2;
			}
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		float colliderCenterX = 0f;
		float colliderCenterY = 0f;
		float colliderExtendX = 0.5f;
		float colliderExtendY = 0.5f;
		Collider2DType collider2DType = Collider2DType.Circle;
		if (unityCollider is BoxCollider boxCollider)
		{
			colliderCenterX = boxCollider.center.x;
			colliderCenterY = boxCollider.center.z;
			colliderExtendX = boxCollider.size.x * 0.5f;
			colliderExtendY = boxCollider.size.z * 0.5f;
			collider2DType = Collider2DType.Cube;
		}
		else if (unityCollider is CapsuleCollider capsuleCollider)
		{
			colliderCenterX = capsuleCollider.center.x;
			colliderCenterY = capsuleCollider.center.z;
			colliderExtendX = (colliderExtendY = capsuleCollider.radius);
			collider2DType = Collider2DType.Circle;
		}
		else if (unityCollider is SphereCollider sphereCollider)
		{
			colliderCenterX = sphereCollider.center.x;
			colliderCenterY = sphereCollider.center.z;
			colliderExtendX = (colliderExtendY = sphereCollider.radius);
			collider2DType = Collider2DType.Circle;
		}
		_agents[value].SetColliderData(collider2DType, colliderCenterX, colliderCenterY, colliderExtendX, colliderExtendY, unityCollider.gameObject.layer);
		if (_isDebug)
		{
			transform.TryGetComponent<AgentDebug>(out var component);
			if (component == null)
			{
				component = transform.gameObject.AddComponent<AgentDebug>();
			}
			component.enabled = true;
			component.Uid = uid;
		}
	}

	public static void RemoveAgent(long uid)
	{
		if (_agentIdMap == null || !_agentIdMap.TryGetValue(uid, out var value))
		{
			return;
		}
		if (_isDebug)
		{
			_agentTransforms[value].TryGetComponent<AgentDebug>(out var component);
			if (component != null)
			{
				component.enabled = false;
				component.Uid = -1;
			}
		}
		int num = _agentCount - 1;
		if (value < num)
		{
			Agent agent = _agents[num];
			_agents[value] = agent;
			Transform transform = _agentTransforms[num];
			_agentTransforms[value] = transform;
			_agentIdMap[agent.Id] = value;
		}
		_agentCount--;
		_agentIdMap.Remove(uid);
	}

	public static void UpdateAgents()
	{
		if (_agentTransforms != null)
		{
			int num = _agentTransforms.Length;
			for (int i = 0; i < num; i++)
			{
				Transform transform = _agentTransforms[i];
				_agents[i].ResetByTransform(transform);
			}
		}
	}

	public static void Build(int frame)
	{
		if (_spaceQuery != null)
		{
			_spaceQuery.Build(_agents, _agentCount, frame);
			if (_isDebug)
			{
				_spaceQuery.DebugLine();
			}
		}
	}

	public static int OverlapFastCapsule2DCollider(Vector2 startPoint, Vector2 endPoint, float capsuleRadius, int layerMask, ref int[] detectedColliderIds)
	{
		if (_spaceQuery == null)
		{
			return 0;
		}
		_sortedList.Clear();
		float num = float.PositiveInfinity;
		float num2 = 0f;
		int num3 = 0;
		float x = startPoint.x;
		float y = startPoint.y;
		float x2 = endPoint.x;
		float y2 = endPoint.y;
		float num4 = x2 - x;
		float num5 = y2 - y;
		float num6 = num4 * num4 + num5 * num5;
		float posX = x + num4 * 0.5f;
		float posY = y + num5 * 0.5f;
		float radis = Mathf.Sqrt(num6) * 0.5f + capsuleRadius;
		_spaceQuery.QueryNearAgents(posX, posY, radis, ref _queriedAgents, layerMask, out var queriedCount);
		for (int i = 0; i < queriedCount; i++)
		{
			int num7 = _queriedAgents[i];
			Agent agent = _agents[num7];
			bool flag = false;
			float num8 = 0f;
			if (agent.ColliderType == Collider2DType.Circle)
			{
				float transformedColliderCenterX = agent.TransformedColliderCenterX;
				float transformedColliderCenterY = agent.TransformedColliderCenterY;
				float transformedColliderExtendX = agent.TransformedColliderExtendX;
				float num9 = transformedColliderCenterX - x2;
				float num10 = transformedColliderCenterY - y2;
				float num11 = transformedColliderExtendX + capsuleRadius;
				float num12 = num11 * num11;
				if (num6 <= float.Epsilon)
				{
					num8 = num9 * num9 + num10 * num10;
					flag = num8 < num12;
					num8 = Mathf.Abs(num9) + Mathf.Abs(num10);
				}
				else
				{
					float num13 = 0f;
					float num14 = num9 * num4 + num10 * num5;
					if (num14 > 0f)
					{
						num13 = num9 * num9 + num10 * num10;
					}
					else if (num14 + num6 < 0f)
					{
						float num15 = transformedColliderCenterX - x;
						float num16 = transformedColliderCenterY - y;
						num13 = num15 * num15 + num16 * num16;
						num8 = Mathf.Abs(num15) + Mathf.Abs(num16);
					}
					else
					{
						float num17 = num14 / num6;
						float num18 = x2 + num4 * num17;
						float num19 = y2 + num5 * num17;
						float num20 = transformedColliderCenterX - num18;
						float num21 = transformedColliderCenterY - num19;
						num13 = num20 * num20 + num21 * num21;
						num8 = Mathf.Abs(num20) + Mathf.Abs(num21);
					}
					flag = num13 < num12;
				}
			}
			else if (agent.ColliderType == Collider2DType.Cube)
			{
				float num22 = agent.TransformedAABBMinX - capsuleRadius;
				float num23 = agent.TransformedAABBMinY - capsuleRadius;
				float num24 = agent.TransformedAABBMaxX + capsuleRadius;
				float num25 = agent.TransformedAABBMaxY + capsuleRadius;
				float num26 = x2 - x;
				float num27 = y2 - y;
				float num28 = Math.Abs(num26);
				float num29 = Math.Abs(num27);
				if ((!(num28 < float.Epsilon) || (!(x < num22) && !(x > num24))) && (!(num29 < float.Epsilon) || (!(y < num23) && !(y > num25))))
				{
					float num30 = 1f / num26;
					float num31 = (num22 - x) * num30;
					float num32 = (num24 - x) * num30;
					if (num31 > num32)
					{
						float num33 = num31;
						num31 = num32;
						num32 = num33;
					}
					float num34 = ((num31 < 0f) ? 0f : num31);
					float num35 = ((num32 > 1f) ? 1f : num32);
					if (num34 <= num35)
					{
						float num36 = 1f / num27;
						num31 = (num23 - y) * num36;
						num32 = (num25 - y) * num36;
						if (num31 > num32)
						{
							float num37 = num31;
							num31 = num32;
							num32 = num37;
						}
						float num38 = ((num31 < 0f) ? 0f : num31);
						num35 = ((num32 > 1f) ? 1f : num32);
						if (num38 <= num35)
						{
							float transformedColliderCenterX2 = agent.TransformedColliderCenterX;
							float transformedColliderCenterY2 = agent.TransformedColliderCenterY;
							float f = transformedColliderCenterX2 - x2;
							float f2 = transformedColliderCenterY2 - y2;
							num8 = Mathf.Abs(f) + Mathf.Abs(f2);
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				if (num8 < num)
				{
					num = num8;
					_sortedList.Insert(0, agent.ColliderId);
				}
				else if (num8 > num2)
				{
					num2 = num8;
					_sortedList.Insert(num3, agent.ColliderId);
				}
				else
				{
					_sortedList.Insert(num3 / 2, agent.ColliderId);
				}
				num3++;
			}
		}
		if (num3 >= detectedColliderIds.Length)
		{
			int num39 = detectedColliderIds.Length * 2;
			int[] array = new int[(num3 >= num39) ? num3 : num39];
			detectedColliderIds = array;
		}
		for (int j = 0; j < num3; j++)
		{
			detectedColliderIds[j] = _sortedList[j];
		}
		return num3;
	}

	public static int OverlapCircle2DCollider(Vector2 circleCenter, Vector2 targetPos, float circleRadius, int layerMask, ref int[] detectedColliderIds)
	{
		if (_spaceQuery == null)
		{
			return 0;
		}
		_sortedList.Clear();
		float num = float.PositiveInfinity;
		float num2 = 0f;
		int num3 = 0;
		float x = circleCenter.x;
		float y = circleCenter.y;
		float x2 = targetPos.x;
		float y2 = targetPos.y;
		_spaceQuery.QueryNearAgents(x, y, circleRadius, ref _queriedAgents, layerMask, out var queriedCount);
		for (int i = 0; i < queriedCount; i++)
		{
			int num4 = _queriedAgents[i];
			Agent agent = _agents[num4];
			bool flag = false;
			float num5 = 0f;
			if (agent.ColliderType == Collider2DType.Circle)
			{
				float num6 = circleRadius + agent.TransformedColliderExtendX;
				float num7 = x - agent.TransformedColliderCenterX;
				float num8 = y - agent.TransformedColliderCenterY;
				float f = x2 - agent.TransformedColliderCenterX;
				float f2 = y2 - agent.TransformedColliderCenterY;
				num5 = num7 * num7 + num8 * num8;
				flag = num5 <= num6 * num6;
				num5 = Mathf.Abs(f) + Mathf.Abs(f2);
			}
			else if (agent.ColliderType == Collider2DType.Cube)
			{
				float transformedAABBMinX = agent.TransformedAABBMinX;
				float transformedAABBMinY = agent.TransformedAABBMinY;
				float transformedAABBMaxX = agent.TransformedAABBMaxX;
				float transformedAABBMaxY = agent.TransformedAABBMaxY;
				float num9 = x;
				if (transformedAABBMaxX < num9)
				{
					num9 = transformedAABBMaxX;
				}
				if (transformedAABBMinX > num9)
				{
					num9 = transformedAABBMinX;
				}
				float num10 = y;
				if (transformedAABBMaxY < num10)
				{
					num10 = transformedAABBMaxY;
				}
				if (transformedAABBMinY > num10)
				{
					num10 = transformedAABBMinY;
				}
				float num11 = x - num9;
				float num12 = y - num10;
				num5 = num11 * num11 + num12 * num12;
				flag = num5 < circleRadius * circleRadius;
				float f3 = x2 - agent.TransformedColliderCenterX;
				float f4 = y2 - agent.TransformedColliderCenterY;
				num5 = Mathf.Abs(f3) + Mathf.Abs(f4);
			}
			if (flag)
			{
				if (num5 < num)
				{
					num = num5;
					_sortedList.Insert(0, agent.ColliderId);
				}
				else if (num5 > num2)
				{
					num2 = num5;
					_sortedList.Insert(num3, agent.ColliderId);
				}
				else
				{
					_sortedList.Insert(num3 / 2, agent.ColliderId);
				}
				num3++;
			}
		}
		if (num3 >= detectedColliderIds.Length)
		{
			int num13 = detectedColliderIds.Length * 2;
			int[] array = new int[(num3 >= num13) ? num3 : num13];
			detectedColliderIds = array;
		}
		for (int j = 0; j < num3; j++)
		{
			detectedColliderIds[j] = _sortedList[j];
		}
		return num3;
	}

	public static int OverlapAABB2DCollider(float aAABBMinX, float aAABBMinY, float aAABBMaxX, float aAABBMaxY, int layerMask, ref int[] detectedColliderIds)
	{
		if (_spaceQuery == null)
		{
			return 0;
		}
		int num = 0;
		float num2 = (aAABBMaxX - aAABBMinX) * 0.5f;
		float num3 = (aAABBMaxY - aAABBMinY) * 0.5f;
		float radis = Mathf.Sqrt(num2 * num2 + num3 * num3);
		_spaceQuery.QueryNearAgents(aAABBMinX + num2, aAABBMinY + num3, radis, ref _queriedAgents, layerMask, out var queriedCount);
		if (queriedCount == 0)
		{
			return 0;
		}
		for (int i = 0; i < queriedCount; i++)
		{
			int num4 = _queriedAgents[i];
			Agent agent = _agents[num4];
			bool flag = false;
			if (agent.ColliderType == Collider2DType.Circle)
			{
				flag = IntersectDetect2D.AABBIntersectCircle2D(aAABBMinX, aAABBMinY, aAABBMaxX, aAABBMaxY, agent.TransformedColliderCenterX, agent.TransformedColliderCenterY, agent.TransformedColliderExtendX);
			}
			else if (agent.ColliderType == Collider2DType.Cube)
			{
				flag = IntersectDetect2D.AABBIntersectAABB2D(agent.TransformedAABBMinX, agent.TransformedAABBMinY, agent.TransformedAABBMaxX, agent.TransformedAABBMaxY, aAABBMinX, aAABBMinY, aAABBMaxX, aAABBMaxY);
			}
			if (flag)
			{
				detectedColliderIds[num++] = agent.ColliderId;
				if (num >= detectedColliderIds.Length)
				{
					int num5 = detectedColliderIds.Length;
					int num6 = num5 * 2;
					int[] array = new int[(num >= num6) ? num : num6];
					Array.Copy(detectedColliderIds, 0, array, 0, num5);
					detectedColliderIds = array;
				}
			}
		}
		return num;
	}

	public static void Clear()
	{
		if (_agents != null)
		{
			Array.Clear(_agents, 0, _agents.Length);
		}
		_agentCount = 0;
		if (_agentTransforms != null)
		{
			Array.Clear(_agentTransforms, 0, _agentTransforms.Length);
		}
		if (_queriedAgents != null)
		{
			Array.Clear(_queriedAgents, 0, _queriedAgents.Length);
		}
		_agentIdMap?.Clear();
		_spaceQuery?.Clear();
	}
}
