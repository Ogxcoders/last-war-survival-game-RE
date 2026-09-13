namespace UnityEngine.Rendering;

internal class DebugActionState
{
	private enum DebugActionKeyType
	{
		Button,
		Axis,
		Key
	}

	private DebugActionKeyType m_Type;

	private string[] m_PressedButtons;

	private string m_PressedAxis = "";

	private KeyCode[] m_PressedKeys;

	private bool[] m_TriggerPressedUp;

	private float m_Timer;

	internal bool runningAction { get; private set; }

	internal float actionState { get; private set; }

	private void Trigger(int triggerCount, float state)
	{
		actionState = state;
		runningAction = true;
		m_Timer = 0f;
		m_TriggerPressedUp = new bool[triggerCount];
		for (int i = 0; i < m_TriggerPressedUp.Length; i++)
		{
			m_TriggerPressedUp[i] = false;
		}
	}

	public void TriggerWithButton(string[] buttons, float state)
	{
		m_Type = DebugActionKeyType.Button;
		m_PressedButtons = buttons;
		m_PressedAxis = "";
		Trigger(buttons.Length, state);
	}

	public void TriggerWithAxis(string axis, float state)
	{
		m_Type = DebugActionKeyType.Axis;
		m_PressedAxis = axis;
		Trigger(1, state);
	}

	public void TriggerWithKey(KeyCode[] keys, float state)
	{
		m_Type = DebugActionKeyType.Key;
		m_PressedKeys = keys;
		m_PressedAxis = "";
		Trigger(keys.Length, state);
	}

	private void Reset()
	{
		runningAction = false;
		m_Timer = 0f;
		m_TriggerPressedUp = null;
	}

	public void Update(DebugActionDesc desc)
	{
		actionState = 0f;
		if (m_TriggerPressedUp == null)
		{
			return;
		}
		m_Timer += Time.deltaTime;
		for (int i = 0; i < m_TriggerPressedUp.Length; i++)
		{
			if (m_Type == DebugActionKeyType.Button)
			{
				m_TriggerPressedUp[i] |= Input.GetButtonUp(m_PressedButtons[i]);
			}
			else if (m_Type == DebugActionKeyType.Axis)
			{
				m_TriggerPressedUp[i] |= Mathf.Approximately(Input.GetAxis(m_PressedAxis), 0f);
			}
			else
			{
				m_TriggerPressedUp[i] |= Input.GetKeyUp(m_PressedKeys[i]);
			}
		}
		bool flag = true;
		bool[] triggerPressedUp = m_TriggerPressedUp;
		foreach (bool flag2 in triggerPressedUp)
		{
			flag = flag && flag2;
		}
		if (flag || (m_Timer > desc.repeatDelay && desc.repeatMode == DebugActionRepeatMode.Delay))
		{
			Reset();
		}
	}
}
