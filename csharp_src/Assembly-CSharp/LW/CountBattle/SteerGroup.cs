using System;
using System.Collections.Generic;
using GameFramework;
using UnityEngine;

namespace LW.CountBattle;

public class SteerGroup : MonoBehaviour
{
	private struct SpawnTask
	{
		public int id;

		public int amount;

		public int point;

		public float radius;

		public float timer;

		public SpawnTask(int amount, int point, float radius, float timer = 0f)
		{
			id = ++_SPAWN_TASK_ID;
			this.amount = amount;
			this.point = point;
			this.radius = radius;
			this.timer = timer;
		}

		public int GetSpawnAmount(float dt, float spawnPerSecond)
		{
			timer += dt;
			int num = Mathf.FloorToInt(timer * spawnPerSecond);
			timer -= (float)num / spawnPerSecond;
			num = Mathf.Min(num, amount);
			amount -= num;
			return num;
		}
	}

	private static int _SPAWN_TASK_ID = 0;

	public static float TICK_INTERVAL = 0.02f;

	private int __unit_inc_id;

	public string groupTag = string.Empty;

	public int spawnPerSecond = 50;

	public float repForceFactor = 10f;

	public float attForceFactor = 0.1f;

	private float _tickTimer;

	private float _tickCD;

	private float _currY;

	private Vector2 _velocity = Vector2.zero;

	private List<SteerUnit> _units = new List<SteerUnit>();

	private List<SpawnTask> _spawnTasks = new List<SpawnTask>();

	private List<SteerCollider> _trapColliders = new List<SteerCollider>();

	private SteerDisplay _display;

	public Action<int, SteerUnit> OnUnitSpawn;

	public Action<int> OnUnitRemoved;

	public Action<int> OnGroupPointChanged;

	public Action<int, int> OnCollideTrap;

	public int GroupPoint { get; set; }

	public Circle GroupCircle { get; set; }

	public float GroupRadius => GroupCircle.radius;

	public float GroupBoundLeft { get; set; }

	public float GroupBoundRight { get; set; }

	public float GroupBoundTop { get; set; }

	public float GroupBoundBottom { get; set; }

	public Box GroupBoundBox
	{
		get
		{
			Vector2 pos = new Vector2((GroupBoundLeft + GroupBoundRight) * 0.5f, (GroupBoundTop + GroupBoundBottom) * 0.5f) + GroupCircle.pos;
			Vector2 size = new Vector2(GroupBoundRight - GroupBoundLeft, GroupBoundTop - GroupBoundBottom);
			return new Box(pos, 0f, size);
		}
	}

	public int UnitCount => _units.Count;

	public bool DisableLogic { get; set; }

	public bool RepSwitch { get; set; }

	public bool AttSwitch { get; set; }

	public Vector3 GroupPos
	{
		get
		{
			if (GroupCircle != null)
			{
				return new Vector3(GroupCircle.pos.x, _currY, GroupCircle.pos.y);
			}
			return Vector3.zero;
		}
	}

	public float GroupPosX
	{
		get
		{
			if (GroupCircle != null)
			{
				return GroupCircle.pos.x;
			}
			return 0f;
		}
	}

	public float GroupPosY => _currY;

	public float GroupPosZ
	{
		get
		{
			if (GroupCircle != null)
			{
				return GroupCircle.pos.y;
			}
			return 0f;
		}
	}

	public List<SteerUnit> Units => _units;

	public SteerGroup EngageGroup { get; set; }

	private void Awake()
	{
		GroupCircle = new Circle(Vector2.zero, 0f, 0f);
		SetPos(base.transform.position.x, base.transform.position.y, base.transform.position.z);
		_display = new SteerDisplay(GroupCircle.pos);
	}

	private void OnDestroy()
	{
		OnUnitSpawn = null;
		foreach (SteerUnit unit in _units)
		{
			unit.Destroy();
		}
		_units = null;
		_spawnTasks = null;
		_trapColliders = null;
		_display = null;
		GroupCircle = null;
	}

	public void SetPosX(float x)
	{
		SetPos(x, _currY, GroupCircle.pos.y);
	}

	public void SetPosY(float y)
	{
		SetPos(GroupCircle.pos.x, y, GroupCircle.pos.y);
	}

	public void SetPosXZ(float x, float z)
	{
		SetPos(x, _currY, z);
	}

	public void SetPos(float x, float y, float z)
	{
		_currY = y;
		Vector2 vector = new Vector2(x, z);
		Vector2 vec = vector - GroupCircle.pos;
		GroupCircle.pos = vector;
		for (int i = 0; i < _units.Count; i++)
		{
			_units[i].Move(vec);
		}
	}

	public void SetVelocity(float vx, float vz)
	{
		_velocity = new Vector2(vx, vz);
	}

	public bool AddTrapCollider(int id, Collider collider)
	{
		if (collider == null)
		{
			Log.Error("SteerGroup.AddTrapCollider collider is null");
			return false;
		}
		if (FindTrapCollider(id) != null)
		{
			Log.Error("SteerGroup.AddTrapCollider collider id {0} already exist", id);
			return false;
		}
		if (collider is BoxCollider)
		{
			Transform transform = collider.transform;
			BoxCollider boxCollider = collider as BoxCollider;
			Vector2 pos = new Vector2(transform.position.x, transform.position.z);
			float y = transform.rotation.eulerAngles.y;
			Vector2 size = new Vector2(boxCollider.size.x * transform.localScale.x, boxCollider.size.z * transform.localScale.z);
			Box shape = new Box(pos, y, size);
			_trapColliders.Add(new SteerCollider(id, shape));
			return true;
		}
		if (collider is SphereCollider)
		{
			Transform transform2 = collider.transform;
			SphereCollider sphereCollider = collider as SphereCollider;
			Vector2 pos2 = new Vector2(transform2.position.x, transform2.position.z);
			float radius = sphereCollider.radius * transform2.localScale.x;
			Circle shape2 = new Circle(pos2, 0f, radius);
			_trapColliders.Add(new SteerCollider(id, shape2));
			return true;
		}
		Log.Error($"SteerGroup.AddTrapShape Error! unknown collider id:{id} type:{collider.GetType().Name}");
		return false;
	}

	public SteerCollider FindTrapCollider(int id)
	{
		return _trapColliders.Find((SteerCollider collider) => collider.id == id);
	}

	public void UpdateTrapCollider(int id, Collider collider)
	{
		if (collider == null)
		{
			Log.Error($"SteerGroup.UpdateTrapShape Error! collider is null id:{id}");
		}
		else
		{
			UpdateTrapCollider(id, collider.transform.position.x, collider.transform.position.z, collider.transform.rotation.eulerAngles.y);
		}
	}

	public void UpdateTrapCollider(int id, float x, float z, float angle)
	{
		SteerCollider steerCollider = FindTrapCollider(id);
		if (steerCollider != null)
		{
			steerCollider.shape.pos = new Vector2(x, z);
			steerCollider.shape.angle = angle;
		}
		else
		{
			Log.Error($"SteerGroup.UpdateTrapShape Error! unregistered id:{id}");
		}
	}

	public void RemoveTrapCollider(int id)
	{
		for (int i = 0; i < _trapColliders.Count; i++)
		{
			if (_trapColliders[i].id == id)
			{
				_trapColliders.RemoveAt(i);
				break;
			}
		}
	}

	public int Spawn(int amount, int point, float radius)
	{
		SpawnTask item = new SpawnTask(amount, point, radius);
		_spawnTasks.Add(item);
		return item.id;
	}

	private void __Spawn(int taskId, int amount, int point, float radius, float firstTickTimeFix)
	{
		for (int i = 0; i < amount; i++)
		{
			Vector2 pos = GroupCircle.pos;
			Vector2 vector = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360)) * Vector3.up;
			pos += vector * UnityEngine.Random.Range(0f, GroupCircle.radius * 0.5f);
			SteerUnit steerUnit = new SteerUnit(++__unit_inc_id, new Vector3(pos.x, _currY, pos.y), point, radius, firstTickTimeFix);
			_units.Add(steerUnit);
			OnUnitSpawn?.Invoke(taskId, steerUnit);
		}
	}

	public void RemoveUnit(SteerUnit unit)
	{
		int num = _units.IndexOf(unit);
		if (num >= 0 && num < _units.Count)
		{
			_units.RemoveAt(num);
			OnUnitRemoved?.Invoke(unit.id);
		}
	}

	public void Vibrate()
	{
		Handheld.Vibrate();
	}

	private void Update()
	{
		if (DisableLogic)
		{
			return;
		}
		float deltaTime = Time.deltaTime;
		_tickCD += deltaTime;
		while (_tickCD >= TICK_INTERVAL)
		{
			_tickCD -= TICK_INTERVAL;
			Tick(TICK_INTERVAL);
		}
		if (_tickCD > 0f)
		{
			Tick(TICK_INTERVAL);
			_tickCD -= TICK_INTERVAL;
		}
		float firstTickTimeFix = 0f - _tickCD;
		for (int num = _spawnTasks.Count - 1; num >= 0; num--)
		{
			SpawnTask value = _spawnTasks[num];
			int spawnAmount = value.GetSpawnAmount(deltaTime, spawnPerSecond);
			__Spawn(value.id, spawnAmount, value.point, value.radius, firstTickTimeFix);
			if (value.amount <= 0)
			{
				_spawnTasks.RemoveAt(num);
			}
			else
			{
				_spawnTasks[num] = value;
			}
		}
		_display.Update(deltaTime);
		base.transform.position = _display.Position;
		int groupPoint = GroupPoint;
		GroupPoint = 0;
		for (int i = 0; i < _units.Count; i++)
		{
			_units[i].Update(deltaTime);
			GroupPoint += _units[i].point;
		}
		if (groupPoint != GroupPoint)
		{
			OnGroupPointChanged?.Invoke(GroupPoint);
		}
	}

	private int __SortUnits(SteerUnit a, SteerUnit b)
	{
		if (a == null || b == null)
		{
			return 0;
		}
		if (b.initPoint == a.initPoint)
		{
			if (a.sqrMag2Center == b.sqrMag2Center)
			{
				if (a.id == b.id)
				{
					return 0;
				}
				if (a.id <= b.id)
				{
					return -1;
				}
				return 1;
			}
			if (!(a.sqrMag2Center > b.sqrMag2Center))
			{
				return -1;
			}
			return 1;
		}
		if (b.initPoint <= a.initPoint)
		{
			return -1;
		}
		return 1;
	}

	private void Tick(float dt)
	{
		_tickTimer += dt;
		GroupMotion(dt);
		_display.Push(new Vector3(GroupCircle.pos.x, _currY, GroupCircle.pos.y), _tickTimer);
		CalcBound();
		TrapDetection();
		if (EngageGroup != null && EngageGroup.GroupCircle.Overlap(GroupCircle))
		{
			EngageDetection();
		}
		_units.Sort(__SortUnits);
		if (RepSwitch)
		{
			Repulsion();
		}
		if (AttSwitch)
		{
			Attraction();
		}
		foreach (SteerUnit unit in _units)
		{
			unit.Move(unit.velocity);
			unit.velocity = Vector2.zero;
		}
		CalcBound();
		foreach (SteerUnit unit2 in _units)
		{
			unit2.Tick(dt, _currY);
		}
	}

	private void GroupMotion(float dt)
	{
		Vector2 vector = _velocity * dt;
		GroupCircle.pos += vector;
		foreach (SteerUnit unit in _units)
		{
			if (!unit.IsDead)
			{
				unit.Move(vector);
			}
		}
	}

	private void CalcBound()
	{
		float[] array = new float[4];
		float num = -1f;
		float num2 = 0f;
		foreach (SteerUnit unit in _units)
		{
			if (!unit.IsDead)
			{
				Vector2 vector = unit.pos - GroupCircle.pos;
				float sqrMagnitude = vector.sqrMagnitude;
				if (sqrMagnitude > num)
				{
					num = sqrMagnitude;
					num2 = unit.shape.radius;
				}
				if (vector.x - unit.shape.radius < array[0])
				{
					array[0] = vector.x - unit.shape.radius;
				}
				if (vector.x + unit.shape.radius > array[1])
				{
					array[1] = vector.x + unit.shape.radius;
				}
				if (vector.y + unit.shape.radius < array[2])
				{
					array[2] = vector.y + unit.shape.radius;
				}
				if (vector.y - unit.shape.radius > array[3])
				{
					array[3] = vector.y - unit.shape.radius;
				}
				unit.sqrMag2Center = sqrMagnitude;
			}
		}
		num = Mathf.Max(num, 0f);
		GroupCircle.radius = Mathf.Sqrt(num) + num2;
		GroupBoundLeft = array[0];
		GroupBoundRight = array[1];
		GroupBoundTop = array[2];
		GroupBoundBottom = array[3];
	}

	private void Repulsion()
	{
		for (int i = 0; i < _units.Count; i++)
		{
			SteerUnit steerUnit = _units[i];
			if (steerUnit.IsDead)
			{
				continue;
			}
			for (int j = i + 1; j < _units.Count; j++)
			{
				SteerUnit steerUnit2 = _units[j];
				if (steerUnit2.IsDead)
				{
					continue;
				}
				Vector2 vector = steerUnit.pos - steerUnit2.pos;
				float sqrMagnitude = vector.sqrMagnitude;
				if (sqrMagnitude < Mathf.Pow(steerUnit.shape.radius + steerUnit2.shape.radius, 2f))
				{
					float num = (float)Math.Sqrt(sqrMagnitude);
					Vector2 vector2 = (((double)num > 1E-05) ? (vector / num) : Vector2.zero);
					if (vector2 == Vector2.zero)
					{
						vector2 = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360)) * Vector3.up;
					}
					steerUnit2.velocity -= vector2 * (steerUnit.shape.radius + steerUnit2.shape.radius - num) * repForceFactor;
				}
			}
		}
	}

	private void Attraction()
	{
		for (int i = 0; i < _units.Count; i++)
		{
			SteerUnit steerUnit = _units[i];
			if (!steerUnit.IsDead)
			{
				Vector2 vector = GroupCircle.pos - steerUnit.pos;
				Vector2 normalized = vector.normalized;
				float sqrMagnitude = vector.sqrMagnitude;
				steerUnit.velocity += normalized * Mathf.Min(sqrMagnitude, 1f) * attForceFactor;
			}
		}
	}

	private void TrapDetection()
	{
		for (int num = _trapColliders.Count - 1; num >= 0; num--)
		{
			SteerCollider steerCollider = _trapColliders[num];
			if (steerCollider.shape.Overlap(GroupCircle))
			{
				for (int num2 = _units.Count - 1; num2 >= 0; num2--)
				{
					SteerUnit steerUnit = _units[num2];
					if (!steerUnit.IsDead && steerCollider.shape.Overlap(steerUnit.shape))
					{
						OnCollideTrap?.Invoke(steerCollider.id, steerUnit.id);
					}
				}
			}
		}
	}

	private void EngageDetection()
	{
		for (int num = _units.Count - 1; num >= 0; num--)
		{
			SteerUnit steerUnit = _units[num];
			if (!steerUnit.IsDead)
			{
				for (int num2 = EngageGroup.Units.Count - 1; num2 >= 0; num2--)
				{
					SteerUnit steerUnit2 = EngageGroup.Units[num2];
					if (!steerUnit2.IsDead && steerUnit.shape.Overlap(steerUnit2.shape))
					{
						int point = steerUnit.point;
						int point2 = steerUnit2.point;
						steerUnit.point -= point2;
						steerUnit2.point -= point;
						if (steerUnit2.point <= 0)
						{
							steerUnit2.Kill();
							EngageGroup.RemoveUnit(steerUnit2);
						}
						if (steerUnit.point <= 0)
						{
							steerUnit.Kill();
							RemoveUnit(steerUnit);
							break;
						}
					}
				}
			}
		}
	}
}
