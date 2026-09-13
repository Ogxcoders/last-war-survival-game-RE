using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameKit.Base;
using UnityEngine;

public class SceneLODEffect : MonoBehaviour, ISceneLODNode
{
	[SerializeField]
	[Range(0f, 5f)]
	protected float lodWeight = 1f;

	[SerializeField]
	protected LODType type;

	private float _cost = 1f;

	private float _dynamicCost = 1f;

	private int _currentLODLevel = -1;

	private bool _onEnable;

	[SerializeField]
	protected List<LODEffectObject> objects = new List<LODEffectObject>();

	[SerializeField]
	protected List<LODTimelineObject> timelineObjects = new List<LODTimelineObject>();

	protected void Awake()
	{
		_cost = (float)objects.Count * lodWeight;
		_dynamicCost = _cost;
	}

	protected virtual void OnEnable()
	{
		_onEnable = true;
		SingletonBehaviour<SceneLODManager>.Instance?.AddNode(this);
		_onEnable = false;
	}

	protected virtual void OnDisable()
	{
		SingletonBehaviour<SceneLODManager>.Instance?.RemoveNode(this);
	}

	public LODType GetLODType()
	{
		return type;
	}

	public float GetCost()
	{
		return _cost;
	}

	public float GetDynamicCost()
	{
		if (_dynamicCost < 0f)
		{
			OnLODLevelChanged(_currentLODLevel);
		}
		return _dynamicCost;
	}

	public int CurrentLOD()
	{
		return _currentLODLevel;
	}

	public void UpdateLOD(int level)
	{
		if (_currentLODLevel != level)
		{
			_currentLODLevel = level;
			if (_onEnable)
			{
				StartCoroutine(UpdateLODCoroutine(level));
			}
			else
			{
				OnLODLevelChanged(level);
			}
		}
	}

	private IEnumerator UpdateLODCoroutine(int level)
	{
		yield return null;
		OnLODLevelChanged(level);
	}

	protected virtual void OnLODLevelChanged(int level)
	{
		_dynamicCost = 0f;
		bool flag = false;
		try
		{
			foreach (LODEffectObject @object in objects)
			{
				if (@object.gameObject == null)
				{
					flag = true;
					continue;
				}
				bool flag2 = @object.isMain || (level >= @object.minLOD && level <= @object.maxLOD);
				if (@object.particle != null)
				{
					ParticleSystem.EmissionModule emission = @object.particle.emission;
					emission.enabled = flag2;
					if (flag2)
					{
						if (@object.particle.isStopped && @object.particle.main.loop)
						{
							@object.particle.Play();
						}
					}
					else
					{
						@object.particle.Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
					}
				}
				if (@object.renderer != null)
				{
					@object.renderer.enabled = flag2;
				}
				if (@object.gameObject.activeSelf != flag2)
				{
					@object.gameObject.SetActive(flag2);
				}
				_dynamicCost += (flag2 ? 1 : 0);
			}
		}
		catch (Exception)
		{
			_dynamicCost = 1f;
			flag = true;
		}
		_dynamicCost *= lodWeight;
		if (flag)
		{
			Log.Error("Prefab maybe changed without update SceneLODEffect." + base.gameObject.name);
		}
	}
}
