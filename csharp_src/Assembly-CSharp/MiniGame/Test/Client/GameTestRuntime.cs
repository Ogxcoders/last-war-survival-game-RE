using System;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Test.Client;

public class GameTestRuntime : MonoBehaviour
{
	private GUIStyle _labelStyle;

	private GameTestLoader _loader;

	public EGameWorldState WorldState
	{
		get
		{
			return _loader?.GameWorld?.State ?? EGameWorldState.Uninitialized;
		}
		private set
		{
			throw new NotSupportedException("WorldState is readonly");
		}
	}

	public int ViewTickCount { get; set; }

	private void Update()
	{
		_loader?.GameWorld?.Update(Time.deltaTime);
	}

	private void OnGUI()
	{
		if (_loader != null)
		{
			if (_labelStyle == null)
			{
				_labelStyle = new GUIStyle(GUI.skin.label);
				_labelStyle.alignment = TextAnchor.MiddleCenter;
				_labelStyle.fontSize = 40;
				_labelStyle.normal.textColor = Color.green;
			}
			GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), GetDisplayText(), _labelStyle);
		}
	}

	private string GetDisplayText()
	{
		float runningTime = _loader.RunningTime;
		float num = _loader.GameWorld?.Env.PrepareTime.AsFloat ?? 0f;
		float num2 = _loader.GameWorld?.Env.GameTime.AsFloat ?? 0f;
		float num3 = _loader.GameWorld?.Env.LogicTime.AsFloat ?? 0f;
		float num4 = _loader.GameWorld?.Env.SettlementTime.AsFloat ?? 0f;
		return $"WorldState : {WorldState} \n ViewTickCount : {ViewTickCount} \n startTime : {runningTime} \n prepareTime : {num} \n gameTime : {num2} \n logicTime : {num3} \n settlementTime : {num4}";
	}

	public void Enter()
	{
		if (_loader != null)
		{
			_loader.Dispose();
			_loader = null;
		}
		_loader = new GameTestLoader();
		StartCoroutine(_loader.StartGame("\"Map/BiuBiu_1.txt\"", this));
	}

	public void Exit()
	{
		StartCoroutine(_loader.ExitGame());
	}

	public void TogglePause()
	{
		if (_loader != null && _loader.GameWorld != null)
		{
			if (!_loader.GameWorld.IsPaused)
			{
				_loader.GameWorld.Pause();
			}
			else
			{
				_loader.GameWorld.Resume();
			}
		}
	}

	private void OnDestroy()
	{
		StopAllCoroutines();
		_loader.Dispose();
	}
}
