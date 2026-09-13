using System;
using System.Runtime.CompilerServices;
using RVO;
using UnityEngine;

public class LWBattleRVOAgent : MonoBehaviour
{
	public int sid;

	private System.Random m_random = new System.Random();

	public LWBattleRVOManager mgr;

	public float speed;

	public bool active = true;

	private RVO.Vector2 _targetPosition;

	public void Update()
	{
		if (!Simulator.Instance.hasAgent(sid))
		{
			return;
		}
		if (!active)
		{
			Simulator.Instance.setAgentPrefVelocity(sid, new RVO.Vector2(0f, 0f));
			return;
		}
		Simulator instance = Simulator.Instance;
		if (instance.optAgentUpdate)
		{
			instance.getAgentPositionAndPrefVelocity(sid, out var position, out var prefVelocity);
			base.transform.position = new Vector3(position.x(), base.transform.position.y, position.y());
			float num = RVOMath.det(instance.getAgentVelocity(sid), prefVelocity);
			if (num > 0.2f * speed || num < -0.2f * speed)
			{
				RefreshPrefVelocity(position);
			}
			return;
		}
		if (sid >= 0)
		{
			RVO.Vector2 agentPosition = Simulator.Instance.getAgentPosition(sid);
			RVO.Vector2 agentPrefVelocity = Simulator.Instance.getAgentPrefVelocity(sid);
			if (!float.IsNaN(agentPosition.x()) && !float.IsNaN(agentPosition.y()))
			{
				base.transform.position = new Vector3(agentPosition.x(), base.transform.position.y, agentPosition.y());
				if (Math.Abs(agentPrefVelocity.x()) > 0.01f || Math.Abs(agentPrefVelocity.y()) > 0.01f)
				{
					base.transform.forward = new Vector3(agentPrefVelocity.x(), 0f, agentPrefVelocity.y()).normalized;
				}
			}
		}
		RVO.Vector2 vector = _targetPosition - Simulator.Instance.getAgentPosition(sid);
		if (RVOMath.absSq(vector) > 1f)
		{
			vector = RVOMath.normalize(vector);
		}
		vector = speed * vector;
		Simulator.Instance.setAgentPrefVelocity(sid, vector);
		float num2 = (float)m_random.NextDouble() * 2f * MathF.PI;
		float num3 = (float)m_random.NextDouble() * 0.0001f;
		Simulator.Instance.setAgentPrefVelocity(sid, Simulator.Instance.getAgentPrefVelocity(sid) + num3 * new RVO.Vector2((float)Math.Cos(num2), (float)Math.Sin(num2)));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void RefreshPrefVelocity(RVO.Vector2 pos)
	{
		RVO.Vector2 vector = _targetPosition - pos;
		float num = RVOMath.absSq(vector);
		if (!(num < 0.01f))
		{
			float x = UnityEngine.Random.Range(-0.001f, 0.001f) * num;
			float y = UnityEngine.Random.Range(-0.001f, 0.001f) * num;
			RVO.Vector2 prefVelocity = RVOMath.normalize(vector + new RVO.Vector2(x, y)) * speed;
			Simulator.Instance.setAgentPrefVelocity(sid, prefVelocity);
			base.transform.forward = new Vector3(prefVelocity.x(), 0f, prefVelocity.y());
		}
	}

	public void SetActive(bool active)
	{
		this.active = active;
	}

	private void OnDestroy()
	{
		mgr.DeleteAgent(sid);
		mgr = null;
	}

	public void SetTargetPosition(float x, float z)
	{
		active = true;
		if (Simulator.Instance.optAgentUpdate)
		{
			if (!Mathf.Approximately(_targetPosition.x_, x) || !Mathf.Approximately(_targetPosition.y_, z))
			{
				_targetPosition.x_ = x;
				_targetPosition.y_ = z;
				RefreshPrefVelocity(Simulator.Instance.getAgentPosition(sid));
			}
		}
		else
		{
			_targetPosition.x_ = x;
			_targetPosition.y_ = z;
		}
	}

	public void SetCurPosition(float x, float z)
	{
		if (Simulator.Instance.optAgentUpdate)
		{
			RVO.Vector2 vector = new RVO.Vector2(x, z);
			Simulator.Instance.setAgentPosition(sid, vector);
			RefreshPrefVelocity(vector);
		}
		else
		{
			Simulator.Instance.setAgentPosition(sid, new RVO.Vector2(x, z));
		}
	}
}
