using System;
using System.Collections;
using System.Collections.Generic;
using GameKit.Base;
using UnityEngine;

public class SoftReferencePrefab : MonoBehaviour
{
	public List<string> AsyncPrefabPaths = new List<string>();

	public List<string> AsyncPrefabGuids = new List<string>();

	public Action<GameObject> LoadedCallback;

	public const string PrefabRecordFilePath = "Assets/Editor/UseSoftRef.json";

	private Transform _transform;

	private ITimer _disposeTimer;

	private Coroutine _autoDisableCoroutine;

	private bool _isUseAutoClosePs;

	private float _psDuration;

	public void Awake()
	{
		_transform = base.gameObject.transform;
		if (TryGetComponent<SoftReferencePrefabExtParam>(out var component))
		{
			_isUseAutoClosePs = true;
			_psDuration = component.psDuration;
		}
	}

	private IEnumerator AutoDisableRoutine()
	{
		yield return new WaitForSeconds(_psDuration);
		if ((bool)base.gameObject)
		{
			base.gameObject.SetActive(value: false);
		}
		_autoDisableCoroutine = null;
	}

	private void ProcessParticleSystem()
	{
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
		if (componentsInChildren != null && componentsInChildren.Length != 0)
		{
			if (_autoDisableCoroutine != null)
			{
				StopCoroutine(_autoDisableCoroutine);
				_autoDisableCoroutine = null;
			}
			_autoDisableCoroutine = StartCoroutine(AutoDisableRoutine());
		}
	}

	private void OnSpawnComplete(GameObject spawnGo)
	{
		if (spawnGo == null)
		{
			return;
		}
		if (_transform == null || !base.isActiveAndEnabled)
		{
			spawnGo.GameObjectRecycle();
			return;
		}
		spawnGo.transform.SetParent(_transform);
		spawnGo.transform.localPosition = Vector3.zero;
		spawnGo.transform.localRotation = Quaternion.identity;
		spawnGo.transform.localScale = Vector3.one;
		LoadedCallback?.Invoke(spawnGo);
		if (_isUseAutoClosePs && _psDuration > 0f)
		{
			ProcessParticleSystem();
		}
	}

	public void OnEnable()
	{
		RecycleGameObjects();
		if (AsyncPrefabPaths == null)
		{
			return;
		}
		foreach (string asyncPrefabPath in AsyncPrefabPaths)
		{
			if (GameEntry.Resource.HasAsset(asyncPrefabPath))
			{
				SoftReferencePrefabManager.Instance.InitPrefabPool(asyncPrefabPath);
			}
			SoftReferencePrefabManager.Instance.SpawnChildObjectByPath(asyncPrefabPath, OnSpawnComplete);
		}
	}

	private void CancelRecycleTimer()
	{
		if (_disposeTimer != null)
		{
			GameEntry.Timer.CancelTimer(_disposeTimer);
			_disposeTimer = null;
		}
	}

	public void OnDisable()
	{
		if (_autoDisableCoroutine != null)
		{
			StopCoroutine(_autoDisableCoroutine);
			_autoDisableCoroutine = null;
		}
		CancelRecycleTimer();
		_disposeTimer = GameEntry.Timer.RegisterTimer(0.1f, delegate
		{
			if (this != null && base.gameObject != null)
			{
				if (!base.isActiveAndEnabled)
				{
					RecycleGameObjects();
				}
				if (_isUseAutoClosePs)
				{
					GameObject rootParent = base.gameObject.GetRootParent();
					if ((rootParent == GameEntry.Resource.ObjectPoolRootTrans.gameObject || rootParent == SingletonBehaviour<SingletonParent>.Instance.transform.gameObject) && base.gameObject.activeSelf)
					{
						base.gameObject.SetActive(value: false);
					}
				}
			}
		});
		LoadedCallback = null;
	}

	private void RecycleGameObjects()
	{
		foreach (Transform item in _transform)
		{
			item.gameObject.GameObjectRecycle();
		}
	}

	private void HideGameObjects()
	{
		foreach (Transform item in _transform)
		{
			item.gameObject.SetActive(value: false);
		}
	}

	public void OnDestroy()
	{
		CancelRecycleTimer();
		if (_autoDisableCoroutine != null)
		{
			StopCoroutine(_autoDisableCoroutine);
			_autoDisableCoroutine = null;
		}
	}
}
