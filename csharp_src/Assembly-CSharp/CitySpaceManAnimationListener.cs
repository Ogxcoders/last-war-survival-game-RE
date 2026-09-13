using System;
using System.Collections.Generic;
using UnityEngine;

public class CitySpaceManAnimationListener : MonoBehaviour
{
	private class StepInfo
	{
		public Vector3 pos;

		public bool isLeft;
	}

	private float startTick;

	private List<StepInfo> stepList = new List<StepInfo>();

	public Action animation_playBegin { get; set; }

	public Action animation_attackBegin { get; set; }

	public Action animation_attackDone { get; set; }

	public Action animation_playEnd { get; set; }

	public Action animation_placeFlag { get; set; }

	public Action animation_walkLeft { get; set; }

	public Action animation_walkRight { get; set; }

	public Action animation_showTrail { get; set; }

	public void OnAnimationEvent_PlayBegin()
	{
		animation_playBegin?.Invoke();
	}

	public void OnAnimationEvent_AttackBegin()
	{
		animation_attackBegin?.Invoke();
	}

	public void OnAnimationEvent_AttackDone()
	{
		animation_attackDone?.Invoke();
	}

	public void OnAnimationEvent_PlayEnd()
	{
		animation_playEnd?.Invoke();
	}

	public void OnAnimationEvent_PlaceFlag()
	{
		animation_placeFlag?.Invoke();
	}

	public void OnWalkLeft()
	{
		animation_walkLeft?.Invoke();
	}

	public void OnWalkRight()
	{
		animation_walkRight?.Invoke();
	}

	public void OnAnimationEvent_ShowTrail()
	{
		animation_showTrail?.Invoke();
	}
}
