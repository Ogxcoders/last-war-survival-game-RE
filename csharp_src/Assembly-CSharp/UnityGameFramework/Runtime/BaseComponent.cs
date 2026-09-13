using UnityEngine;

namespace UnityGameFramework.Runtime;

[DisallowMultipleComponent]
public sealed class BaseComponent : GameFrameworkComponent
{
	private float m_GameSpeedBeforePause = 1f;

	[SerializeField]
	private int m_FrameRate = 50;

	[SerializeField]
	private float m_GameSpeed = 1f;

	[SerializeField]
	private bool m_RunInBackground = true;

	[SerializeField]
	private bool m_NeverSleep = true;

	public int FrameRate
	{
		get
		{
			return m_FrameRate;
		}
		set
		{
			Application.targetFrameRate = (m_FrameRate = value);
		}
	}

	public float GameSpeed
	{
		get
		{
			return m_GameSpeed;
		}
		set
		{
			Time.timeScale = (m_GameSpeed = ((value >= 0f) ? value : 0f));
		}
	}

	public bool IsGamePaused => m_GameSpeed <= 0f;

	public bool IsNormalGameSpeed => m_GameSpeed == 1f;

	public bool RunInBackground
	{
		get
		{
			return m_RunInBackground;
		}
		set
		{
			Application.runInBackground = (m_RunInBackground = value);
		}
	}

	public bool NeverSleep
	{
		get
		{
			return m_NeverSleep;
		}
		set
		{
			m_NeverSleep = value;
			Screen.sleepTimeout = (value ? (-1) : (-2));
		}
	}

	protected override void Awake()
	{
		base.Awake();
		Application.targetFrameRate = m_FrameRate;
		Time.timeScale = m_GameSpeed;
		Application.runInBackground = m_RunInBackground;
		Screen.sleepTimeout = (m_NeverSleep ? (-1) : (-2));
	}

	public void PauseGame()
	{
		if (!IsGamePaused)
		{
			m_GameSpeedBeforePause = GameSpeed;
			GameSpeed = 0f;
		}
	}

	public void ResumeGame()
	{
		if (IsGamePaused)
		{
			GameSpeed = m_GameSpeedBeforePause;
		}
	}

	public void ResetNormalGameSpeed()
	{
		if (!IsNormalGameSpeed)
		{
			GameSpeed = 1f;
		}
	}
}
