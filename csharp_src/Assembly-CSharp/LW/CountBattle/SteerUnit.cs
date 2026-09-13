using System;
using UnityEngine;

namespace LW.CountBattle;

public class SteerUnit
{
	public int id;

	public int initPoint;

	public int point;

	public Circle shape;

	public Vector2 velocity;

	public float sqrMag2Center;

	private bool _isDead;

	private float _tickTimer;

	private SteerDisplay _display;

	public Action OnDead;

	public Vector2 pos => shape.pos;

	public float radius => shape.radius;

	public Vector3 DisplayPos => _display.Position;

	public bool IsDead => _isDead;

	public SteerUnit(int id, Vector3 pos0, int point, float radius, float firstTickTimeFix)
	{
		this.id = id;
		initPoint = point;
		this.point = point;
		shape = new Circle(new Vector2(pos0.x, pos0.z), 0f, radius);
		_display = new SteerDisplay(pos0, firstTickTimeFix);
	}

	public void Destroy()
	{
		shape = null;
		OnDead = null;
		_display = null;
	}

	public void SetPos(float x, float z)
	{
		if (shape != null)
		{
			Vector2 vector = new Vector2(x, z);
			shape.pos = vector;
		}
	}

	public void Move(Vector2 vec)
	{
		if (shape != null)
		{
			shape.pos += vec;
		}
	}

	public void Kill()
	{
		if (!_isDead)
		{
			_isDead = true;
			OnDead?.Invoke();
		}
	}

	public void Tick(float dt, float y)
	{
		_tickTimer += dt;
		_display.Push(new Vector3(shape.pos.x, y, shape.pos.y), _tickTimer);
	}

	public void Update(float dt)
	{
		_display.Update(dt);
	}

	public void Sync(Transform transform)
	{
		if (!(transform == null))
		{
			transform.position = _display.Position;
		}
	}
}
