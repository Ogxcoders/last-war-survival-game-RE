using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class InputHelper
{
	public class InputState
	{
		public Vector3 mousePosition;

		public bool mouse0Down;

		public bool mouse1Down;

		public bool mouse2Down;

		public List<Touch> touchPoints;
	}

	private static InputState[] m_InputStates = new InputState[2];

	public static int LastInputStatesIndex
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (Time.frameCount + 2 - 1) % 2;
		}
	}

	public static int CurrInputStatesIndex
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (Time.frameCount + 2) % 2;
		}
	}

	public static InputState LastInput => m_InputStates[LastInputStatesIndex];

	public static InputState CurrInput => m_InputStates[CurrInputStatesIndex];

	public static void RecordInputState()
	{
		m_InputStates[CurrInputStatesIndex] = m_InputStates[CurrInputStatesIndex] ?? new InputState();
		InputState inputState = m_InputStates[CurrInputStatesIndex];
		inputState.mousePosition = Input.mousePosition;
		inputState.mouse0Down = Input.GetMouseButton(0);
		inputState.mouse1Down = Input.GetMouseButton(1);
		inputState.mouse2Down = Input.GetMouseButton(2);
		inputState.touchPoints = inputState.touchPoints ?? new List<Touch>(Mathf.Max(Input.touchCount, 4));
		inputState.touchPoints.Clear();
		for (int i = 0; i < Input.touchCount; i++)
		{
			inputState.touchPoints.Add(Input.GetTouch(i));
		}
	}

	public static bool IsPressed(InputState input)
	{
		if (input != null)
		{
			if (!Application.isMobilePlatform)
			{
				if (!input.mouse0Down && !input.mouse1Down)
				{
					return input.mouse2Down;
				}
				return true;
			}
			List<Touch> touchPoints = input.touchPoints;
			if (touchPoints == null)
			{
				return false;
			}
			return touchPoints.Count > 0;
		}
		return false;
	}
}
