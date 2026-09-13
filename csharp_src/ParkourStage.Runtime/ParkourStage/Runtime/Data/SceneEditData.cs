using System;
using System.Collections.Generic;
using ParkourStage.Runtime.Util;
using UnityEngine;

namespace ParkourStage.Runtime.Data;

public class SceneEditData : IStageEditData, IDisposable
{
	public float Offset;

	private GameObject ScenePrefab;

	private bool m_IsSelected;

	private bool m_ShowPrefabLoaded;

	private bool m_ShowPrefabLoadError;

	private bool m_Destroyed;

	private SceneData m_SceneData = new SceneData();

	public string SceneId => m_SceneData.SceneId;

	public string ScenePrefabPath => m_SceneData.ScenePrefabPath;

	public float Size => m_SceneData.Size;

	public bool Selected => m_IsSelected;

	public bool LoadedError => m_ShowPrefabLoadError;

	public void Init(List<RowData> rowDatas)
	{
		SceneDataParamConvert.ConvertByRowDatas(in m_SceneData, rowDatas);
	}

	public void LoadShowPrefab()
	{
		DestroyShowedPrefab();
		Transform sceneRoot = StagePrefabUtil.GetStagePrefabGenerator().GetSceneRoot();
		StagePrefabUtil.GetStagePrefabGenerator().InstantiatePrefabAsync(ScenePrefabPath, sceneRoot, OnScenePrefabInstantiated);
	}

	private void OnScenePrefabInstantiated(GameObject prefab)
	{
		ScenePrefab = prefab;
		if (ScenePrefab != null)
		{
			ScenePrefab.transform.position = new Vector3(0f, 0f, Offset);
		}
		m_ShowPrefabLoadError = ScenePrefab == null;
		m_ShowPrefabLoaded = true;
	}

	public bool IsDeletedInScene()
	{
		if (m_ShowPrefabLoaded && !m_ShowPrefabLoadError)
		{
			return ScenePrefab == null;
		}
		return false;
	}

	public GameObject GetScenePrefab()
	{
		return ScenePrefab;
	}

	public void SetSelected(bool isSelected)
	{
		m_IsSelected = isSelected;
	}

	public void OnUpdate()
	{
		if (ScenePrefab != null)
		{
			ScenePrefab.transform.position = new Vector3(0f, 0f, Offset);
		}
	}

	public void DrawView()
	{
	}

	public bool IsDestroyed()
	{
		return m_Destroyed;
	}

	public void DestroyShowedPrefab()
	{
		if (ScenePrefab != null)
		{
			StagePrefabUtil.GetStagePrefabGenerator().DestroyInstantiatedPrefab(ScenePrefab);
			ScenePrefab = null;
		}
	}

	public void Dispose()
	{
		m_IsSelected = false;
		DestroyShowedPrefab();
		ScenePrefab = null;
		m_ShowPrefabLoaded = false;
		m_ShowPrefabLoadError = false;
		m_Destroyed = true;
	}
}
