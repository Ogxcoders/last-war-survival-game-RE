using System.Collections.Generic;
using UnityEngine;

namespace LW.CountBattle;

public class SteerDisplay
{
	private struct SyncState
	{
		public Vector3 position;

		public float timestamp;

		public SyncState(Vector3 position, float timestamp)
		{
			this.position = position;
			this.timestamp = timestamp;
		}
	}

	public static int AUTO_INC_ID = 0;

	public static int DELAY_TICKS = 3;

	private List<SyncState> _buffer = new List<SyncState>();

	private float _syncTimer;

	private float _firstTickTimeFix = 1f;

	private int _id = ++AUTO_INC_ID;

	private Vector3 _position = Vector3.zero;

	public Vector3 Position => _position;

	public SteerDisplay(Vector3 pos0, float firstTickTimeFix = 0f)
	{
		Push(pos0, 0f);
		_firstTickTimeFix = firstTickTimeFix;
	}

	public void Push(Vector3 position, float timestamp)
	{
		_buffer.Add(new SyncState(position, timestamp));
	}

	public void Update(float dt)
	{
		if (_buffer.Count < DELAY_TICKS)
		{
			return;
		}
		_syncTimer += dt - _firstTickTimeFix;
		_firstTickTimeFix = 0f;
		float num = (_syncTimer - _buffer[0].timestamp) / (_buffer[1].timestamp - _buffer[0].timestamp);
		bool flag = false;
		if (num > 1f)
		{
			int num2 = (int)num;
			num -= (float)num2;
			while (num2 > 0)
			{
				if (_buffer.Count <= 2)
				{
					DELAY_TICKS++;
					flag = true;
					break;
				}
				_buffer.RemoveAt(0);
				num2--;
			}
		}
		if (flag)
		{
			_position = _buffer[0].position;
		}
		else if (num < 1f)
		{
			_position = Vector3.Lerp(_buffer[0].position, _buffer[1].position, num);
		}
		else if (num == 1f)
		{
			_position = _buffer[1].position;
			_buffer.RemoveAt(0);
		}
	}
}
