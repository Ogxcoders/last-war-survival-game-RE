using System;
using UnityEngine;

public class WorldTruckCircleMove
{
	public class Circle
	{
		private float _x;

		private float _z;

		private float _radius;

		public Circle(Vector3 central, float radius)
		{
			_x = central.x;
			_z = central.z;
			_radius = radius;
		}

		public Vector3 GetPos(float angle)
		{
			float f = angle * (MathF.PI / 180f);
			return new Vector3(Mathf.Cos(f) * _radius + _x, 0f, Mathf.Sin(f) * _radius + _z);
		}
	}

	private WorldPeopleTruckBase itemBase;

	private float _nowAngle;

	private bool _finishInit;

	private Circle _circle;

	private float _perSrcRunAngle;

	private float _viaductAngle = 6f;

	private bool _isInner;

	public WorldTruckCircleMove(WorldPeopleTruckBase itemBase)
	{
		this.itemBase = itemBase;
	}

	public void InitCircle(Vector3 central, float radius, float nowAngle, bool isInner)
	{
		_circle = new Circle(central, radius);
		_nowAngle = nowAngle;
		_perSrcRunAngle = 360f / (MathF.PI * 2f * radius);
		_isInner = isInner;
		_finishInit = true;
	}

	private Vector3 genPos(float angle)
	{
		float y = 0f;
		float num = angle % 90f;
		if (num < 0f)
		{
			num += 90f;
		}
		if (num < _viaductAngle || num > 90f - _viaductAngle)
		{
			float time = ((!(num > 90f - _viaductAngle)) ? ((num + _viaductAngle) / (_viaductAngle * 2f)) : ((num - 90f + _viaductAngle) / (_viaductAngle * 2f)));
			y = itemBase.curve.viaductYPos.Evaluate(time) * 1.5f;
		}
		Vector3 pos = _circle.GetPos(angle);
		pos.y = y;
		return pos;
	}

	public void UpdateMove(float deltaTime)
	{
		if (_finishInit)
		{
			float num = deltaTime * itemBase.Speed * _perSrcRunAngle;
			if (!_isInner)
			{
				num = 0f - num;
			}
			_nowAngle = (_nowAngle + num) % 360f;
			if (_nowAngle < 0f)
			{
				_nowAngle += 360f;
			}
			Vector3 position = genPos(_nowAngle);
			Vector3 worldPosition = genPos(_nowAngle + num);
			itemBase.transform.position = position;
			itemBase.transform.LookAt(worldPosition);
		}
	}
}
