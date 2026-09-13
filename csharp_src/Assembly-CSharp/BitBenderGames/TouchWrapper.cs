using System.Collections.Generic;
using UnityEngine;

namespace BitBenderGames;

public class TouchWrapper
{
	private static readonly WrappedTouch m_FirstWrappedTouches = new WrappedTouch();

	public static int TouchCount => Input.touchCount;

	public static WrappedTouch Touch0
	{
		get
		{
			if (TouchCount > 0)
			{
				return WrappedTouch.FromTouch(Input.touches[0]);
			}
			return null;
		}
	}

	public static bool IsFingerDown => TouchCount > 0;

	public static List<WrappedTouch> Touches => GetTouchesFromInputTouches();

	public static Vector2 AverageTouchPos => GetAverageTouchPosFromInputTouches();

	private static List<WrappedTouch> GetTouchesFromInputTouches()
	{
		List<WrappedTouch> list = new List<WrappedTouch>();
		Touch[] touches = Input.touches;
		foreach (Touch touch in touches)
		{
			list.Add(WrappedTouch.FromTouch(touch));
		}
		return list;
	}

	public static WrappedTouch GetFirstWrappedTouch()
	{
		if (TouchCount == 0)
		{
			m_FirstWrappedTouches.FingerId = -1;
		}
		else
		{
			Touch touch = Input.touches[0];
			m_FirstWrappedTouches.Position = touch.position;
			m_FirstWrappedTouches.FingerId = touch.fingerId;
		}
		return m_FirstWrappedTouches;
	}

	private static Vector2 GetAverageTouchPosFromInputTouches()
	{
		Vector2 zero = Vector2.zero;
		if (Input.touches != null && Input.touches.Length != 0)
		{
			Touch[] touches = Input.touches;
			foreach (Touch touch in touches)
			{
				zero += touch.position;
			}
			zero /= (float)Input.touches.Length;
		}
		return zero;
	}
}
