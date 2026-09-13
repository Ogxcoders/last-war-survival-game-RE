using System;
using System.Collections.Generic;
using System.IO;
using RVO;
using UnityEngine;
using VEngine;

public class LWBattleRVOManager
{
	private int _configCount;

	private int _finishConfigCount;

	private RVO.Vector2 _targetPosition;

	public RVO.Vector2 targetPosition => _targetPosition;

	public void InitLW(float timeStep, float neighborDist, int maxNeighbors, float timeHorizon, float timeHorizonObst, float radius, float maxSpeed, int step = 1, bool agentOpt = false)
	{
		Simulator.Instance.Clear();
		Simulator.Instance.SetNumWorkers(step);
		Simulator.Instance.optAgentUpdate = agentOpt;
		Simulator.Instance.setAgentDefaults(neighborDist, maxNeighbors, timeHorizon, timeHorizonObst, radius, maxSpeed, new RVO.Vector2(0f, 0f));
		_configCount = 0;
		_finishConfigCount = 0;
	}

	public void Append(string configPath, float offset)
	{
		_configCount++;
		Asset req = GameEntry.Resource.LoadAssetAsync(configPath, typeof(TextAsset));
		Asset asset = req;
		asset.completed = (Action<Asset>)Delegate.Combine(asset.completed, (Action<Asset>)delegate
		{
			using (BinaryReader binaryReader = new BinaryReader(new MemoryStream((req.asset as TextAsset).bytes)))
			{
				int num = binaryReader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					IList<RVO.Vector2> vertices = new List<RVO.Vector2>
					{
						new RVO.Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle() + offset),
						new RVO.Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle() + offset),
						new RVO.Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle() + offset),
						new RVO.Vector2(binaryReader.ReadSingle(), binaryReader.ReadSingle() + offset)
					};
					Simulator.Instance.addObstacle(vertices);
				}
			}
			_finishConfigCount++;
			if (_finishConfigCount == _configCount)
			{
				Simulator.Instance.processObstacles();
			}
		});
	}

	public void Destory()
	{
		Simulator.Instance.Clear();
	}

	public void SyncTargetPosition(float x, float z)
	{
		_targetPosition.x_ = x;
		_targetPosition.y_ = z;
	}

	public void Update(float x, float z)
	{
		Simulator.Instance.setTimeStep(Time.deltaTime);
		Simulator.Instance.doStep();
		SyncTargetPosition(x, z);
	}

	public int AddAgent(Vector3 position, GameObject gameObject, float speed, float radius)
	{
		RVO.Vector2 position2 = default(RVO.Vector2);
		position2.x_ = position.x;
		position2.y_ = position.z;
		int num = Simulator.Instance.addAgent(position2);
		LWBattleRVOAgent lWBattleRVOAgent = gameObject.GetComponent<LWBattleRVOAgent>();
		if (lWBattleRVOAgent == null)
		{
			lWBattleRVOAgent = gameObject.AddComponent<LWBattleRVOAgent>();
		}
		lWBattleRVOAgent.sid = num;
		lWBattleRVOAgent.mgr = this;
		lWBattleRVOAgent.speed = speed;
		Simulator.Instance.setAgentRadius(num, radius);
		Simulator.Instance.setAgentMaxSpeed(num, speed * 2f);
		return num;
	}

	public void DeleteAgent(int sid)
	{
		Simulator.Instance.delAgent(sid);
	}
}
