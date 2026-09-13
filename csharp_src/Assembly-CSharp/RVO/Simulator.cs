using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.Collections;

namespace RVO;

public class Simulator
{
	private class Worker
	{
		private ManualResetEvent doneEvent_;

		private int end_;

		private int start_;

		internal Worker(int start, int end, ManualResetEvent doneEvent)
		{
			start_ = start;
			end_ = end;
			doneEvent_ = doneEvent;
		}

		internal void config(int start, int end)
		{
			start_ = start;
			end_ = end;
		}

		internal void step(object obj)
		{
			for (int i = start_; i < end_; i++)
			{
				Instance.agents_[i].computeNeighbors();
				Instance.agents_[i].computeNewVelocity();
			}
			doneEvent_.Set();
		}

		internal void update(object obj)
		{
			for (int i = start_; i < end_; i++)
			{
				Instance.agents_[i].update();
			}
			doneEvent_.Set();
		}
	}

	internal IDictionary<int, int> agentNo2indexDict_;

	internal List<Agent> agents_;

	internal bool agentsDirty_;

	internal List<Obstacle> obstacles_;

	internal KdTree kdTree_;

	internal float timeStep_;

	private static Simulator instance_ = new Simulator();

	private Agent defaultAgent_;

	private ManualResetEvent[] doneEvents_;

	private Worker[] workers_;

	private int numWorkers_;

	private int stepWorkers_;

	private int workerAgentCount_;

	private float globalTime_;

	private static int s_totalID = 0;

	public static Simulator Instance => instance_;

	public bool optAgentUpdate { get; set; }

	public void delAgent(int agentNo)
	{
		if (agentNo2indexDict_.ContainsKey(agentNo))
		{
			agents_[agentNo2indexDict_[agentNo]].needDelete_ = true;
		}
	}

	public bool hasAgent(int agentNo)
	{
		return agentNo2indexDict_.ContainsKey(agentNo);
	}

	private void updateDeleteAgent()
	{
		bool flag = false;
		for (int num = agents_.Count - 1; num >= 0; num--)
		{
			if (agents_[num].needDelete_)
			{
				agents_.RemoveAtSwapBack(num);
				flag = true;
			}
		}
		if (flag)
		{
			onDelAgent();
		}
	}

	public int addAgent(Vector2 position)
	{
		if (defaultAgent_ == null)
		{
			return -1;
		}
		Agent agent = new Agent();
		agent.id_ = s_totalID;
		s_totalID++;
		agent.maxNeighbors_ = defaultAgent_.maxNeighbors_;
		agent.maxSpeed_ = defaultAgent_.maxSpeed_;
		agent.neighborDist_ = defaultAgent_.neighborDist_;
		agent.position_ = position;
		agent.radius_ = defaultAgent_.radius_;
		agent.timeHorizon_ = defaultAgent_.timeHorizon_;
		agent.timeHorizonObst_ = defaultAgent_.timeHorizonObst_;
		agent.velocity_ = defaultAgent_.velocity_;
		agents_.Add(agent);
		onAddAgent();
		return agent.id_;
	}

	private void onDelAgent()
	{
		agentNo2indexDict_.Clear();
		for (int i = 0; i < agents_.Count; i++)
		{
			int id_ = agents_[i].id_;
			agentNo2indexDict_.Add(id_, i);
		}
		agentsDirty_ = true;
	}

	private void onAddAgent()
	{
		if (agents_.Count != 0)
		{
			int num = agents_.Count - 1;
			int id_ = agents_[num].id_;
			agentNo2indexDict_.Add(id_, num);
			agentsDirty_ = true;
		}
	}

	public int addAgent(Vector2 position, float neighborDist, int maxNeighbors, float timeHorizon, float timeHorizonObst, float radius, float maxSpeed, Vector2 velocity)
	{
		Agent agent = new Agent();
		agent.id_ = s_totalID;
		s_totalID++;
		agent.maxNeighbors_ = maxNeighbors;
		agent.maxSpeed_ = maxSpeed;
		agent.neighborDist_ = neighborDist;
		agent.position_ = position;
		agent.radius_ = radius;
		agent.timeHorizon_ = timeHorizon;
		agent.timeHorizonObst_ = timeHorizonObst;
		agent.velocity_ = velocity;
		agents_.Add(agent);
		onAddAgent();
		return agent.id_;
	}

	public int addObstacle(IList<Vector2> vertices)
	{
		if (vertices.Count < 2)
		{
			return -1;
		}
		int count = obstacles_.Count;
		for (int i = 0; i < vertices.Count; i++)
		{
			Obstacle obstacle = new Obstacle();
			obstacle.point_ = vertices[i];
			if (i != 0)
			{
				obstacle.previous_ = obstacles_[obstacles_.Count - 1];
				obstacle.previous_.next_ = obstacle;
			}
			if (i == vertices.Count - 1)
			{
				obstacle.next_ = obstacles_[count];
				obstacle.next_.previous_ = obstacle;
			}
			obstacle.direction_ = RVOMath.normalize(vertices[(i != vertices.Count - 1) ? (i + 1) : 0] - vertices[i]);
			if (vertices.Count == 2)
			{
				obstacle.convex_ = true;
			}
			else
			{
				obstacle.convex_ = RVOMath.leftOf(vertices[(i == 0) ? (vertices.Count - 1) : (i - 1)], vertices[i], vertices[(i != vertices.Count - 1) ? (i + 1) : 0]) >= 0f;
			}
			obstacle.id_ = obstacles_.Count;
			obstacles_.Add(obstacle);
		}
		return count;
	}

	public void Clear()
	{
		agents_ = new List<Agent>();
		agentsDirty_ = true;
		agentNo2indexDict_ = new Dictionary<int, int>();
		defaultAgent_ = null;
		kdTree_ = new KdTree();
		obstacles_ = new List<Obstacle>();
		globalTime_ = 0f;
		timeStep_ = 0.1f;
		stepWorkers_ = 0;
		SetNumWorkers(1);
	}

	public float doStep()
	{
		updateDeleteAgent();
		stepWorkers_ = (stepWorkers_ + 1) % numWorkers_;
		if (workers_ == null)
		{
			workers_ = new Worker[numWorkers_];
			doneEvents_ = new ManualResetEvent[workers_.Length];
			workerAgentCount_ = getNumAgents();
			for (int i = 0; i < workers_.Length; i++)
			{
				doneEvents_[i] = new ManualResetEvent(initialState: false);
				workers_[i] = new Worker(i * getNumAgents() / workers_.Length, (i + 1) * getNumAgents() / workers_.Length, doneEvents_[i]);
			}
		}
		if (workerAgentCount_ != getNumAgents())
		{
			workerAgentCount_ = getNumAgents();
			for (int j = 0; j < workers_.Length; j++)
			{
				workers_[j].config(j * getNumAgents() / workers_.Length, (j + 1) * getNumAgents() / workers_.Length);
			}
		}
		kdTree_.buildAgentTree();
		agentsDirty_ = false;
		workers_[stepWorkers_].step(null);
		for (int k = 0; k < workers_.Length; k++)
		{
			workers_[k].update(null);
		}
		globalTime_ += timeStep_;
		return globalTime_;
	}

	public int getAgentAgentNeighbor(int agentNo, int neighborNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].agentNeighbors_[neighborNo].Value.id_;
	}

	public int getAgentMaxNeighbors(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].maxNeighbors_;
	}

	public float getAgentMaxSpeed(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].maxSpeed_;
	}

	public float getAgentNeighborDist(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].neighborDist_;
	}

	public int getAgentNumAgentNeighbors(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].agentNeighbors_.Count;
	}

	public int getAgentNumObstacleNeighbors(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].obstacleNeighbors_.Count;
	}

	public int getAgentObstacleNeighbor(int agentNo, int neighborNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].obstacleNeighbors_[neighborNo].Value.id_;
	}

	public IList<Line> getAgentOrcaLines(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].orcaLines_;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2 getAgentPosition(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].position_;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2 getAgentPrefVelocity(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].prefVelocity_;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void getAgentPositionAndPrefVelocity(int agentNo, out Vector2 position, out Vector2 prefVelocity)
	{
		Agent agent = agents_[agentNo2indexDict_[agentNo]];
		position = agent.position_;
		prefVelocity = agent.prefVelocity_;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float getAgentRadius(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].radius_;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float getAgentTimeHorizon(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].timeHorizon_;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public float getAgentTimeHorizonObst(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].timeHorizonObst_;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vector2 getAgentVelocity(int agentNo)
	{
		return agents_[agentNo2indexDict_[agentNo]].velocity_;
	}

	public float getGlobalTime()
	{
		return globalTime_;
	}

	public int getNumAgents()
	{
		return agents_.Count;
	}

	public int getNumObstacleVertices()
	{
		return obstacles_.Count;
	}

	public int GetNumWorkers()
	{
		return numWorkers_;
	}

	public Vector2 getObstacleVertex(int vertexNo)
	{
		return obstacles_[vertexNo].point_;
	}

	public int getNextObstacleVertexNo(int vertexNo)
	{
		return obstacles_[vertexNo].next_.id_;
	}

	public int getPrevObstacleVertexNo(int vertexNo)
	{
		return obstacles_[vertexNo].previous_.id_;
	}

	public float getTimeStep()
	{
		return timeStep_;
	}

	public void processObstacles()
	{
		kdTree_.buildObstacleTree();
	}

	public bool queryVisibility(Vector2 point1, Vector2 point2, float radius)
	{
		return kdTree_.queryVisibility(point1, point2, radius);
	}

	public int queryNearAgent(Vector2 point, float radius)
	{
		if (getNumAgents() == 0)
		{
			return -1;
		}
		return kdTree_.queryNearAgent(point, radius);
	}

	public void setAgentDefaults(float neighborDist, int maxNeighbors, float timeHorizon, float timeHorizonObst, float radius, float maxSpeed, Vector2 velocity)
	{
		if (defaultAgent_ == null)
		{
			defaultAgent_ = new Agent();
		}
		defaultAgent_.maxNeighbors_ = maxNeighbors;
		defaultAgent_.maxSpeed_ = maxSpeed;
		defaultAgent_.neighborDist_ = neighborDist;
		defaultAgent_.radius_ = radius;
		defaultAgent_.timeHorizon_ = timeHorizon;
		defaultAgent_.timeHorizonObst_ = timeHorizonObst;
		defaultAgent_.velocity_ = velocity;
	}

	public void setAgentMaxNeighbors(int agentNo, int maxNeighbors)
	{
		agents_[agentNo2indexDict_[agentNo]].maxNeighbors_ = maxNeighbors;
	}

	public void setAgentMaxSpeed(int agentNo, float maxSpeed)
	{
		agents_[agentNo2indexDict_[agentNo]].maxSpeed_ = maxSpeed;
	}

	public void setAgentNeighborDist(int agentNo, float neighborDist)
	{
		agents_[agentNo2indexDict_[agentNo]].neighborDist_ = neighborDist;
	}

	public void setAgentPosition(int agentNo, Vector2 position)
	{
		agents_[agentNo2indexDict_[agentNo]].position_ = position;
	}

	public void setAgentMoveWeight(int agentNo, float weight)
	{
		agents_[agentNo2indexDict_[agentNo]].SetWeight(weight);
	}

	public void setAgentPrefVelocity(int agentNo, Vector2 prefVelocity)
	{
		agents_[agentNo2indexDict_[agentNo]].prefVelocity_ = prefVelocity;
	}

	public void setAgentRadius(int agentNo, float radius)
	{
		agents_[agentNo2indexDict_[agentNo]].radius_ = radius;
	}

	public void setAgentTimeHorizon(int agentNo, float timeHorizon)
	{
		agents_[agentNo2indexDict_[agentNo]].timeHorizon_ = timeHorizon;
	}

	public void setAgentTimeHorizonObst(int agentNo, float timeHorizonObst)
	{
		agents_[agentNo2indexDict_[agentNo]].timeHorizonObst_ = timeHorizonObst;
	}

	public void setAgentVelocity(int agentNo, Vector2 velocity)
	{
		agents_[agentNo2indexDict_[agentNo]].velocity_ = velocity;
	}

	public void setGlobalTime(float globalTime)
	{
		globalTime_ = globalTime;
	}

	public void SetNumWorkers(int numWorkers)
	{
		numWorkers_ = numWorkers;
		workers_ = null;
		workerAgentCount_ = 0;
	}

	public void setTimeStep(float timeStep)
	{
		timeStep_ = timeStep;
	}

	private Simulator()
	{
		Clear();
	}
}
