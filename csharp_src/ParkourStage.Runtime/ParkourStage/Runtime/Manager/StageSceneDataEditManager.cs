using System;
using System.Collections.Generic;
using ParkourStage.Runtime.Data;
using ParkourStage.Runtime.Util;
using UnityEngine;

namespace ParkourStage.Runtime.Manager;

public class StageSceneDataEditManager : IStageDataEditManager, IDisposable
{
	private readonly List<SceneEditData> m_SceneDatas = new List<SceneEditData>();

	private readonly List<RowData> m_TmpRowDatas = new List<RowData>();

	private readonly List<SceneEditData> m_ToDestroySceneDatas = new List<SceneEditData>();

	private IDataLoader m_SceneDataLoader;

	private GameObject m_HeadScenePrefab;

	private float m_Offset;

	private bool m_Inited;

	private SceneEditData m_HeadSceneData;

	public bool Inited => m_Inited;

	public void Init(IDataLoader sceneDataLoader, string[] sceneIds, string sceneTrailId)
	{
		DestroySceneDatas();
		DestroyHeadScenePrefab();
		if (sceneDataLoader == null)
		{
			Debug.LogError("StageSceneMgr Load failed!,sceneDataLoader is null");
			return;
		}
		m_SceneDataLoader = sceneDataLoader;
		m_Offset = 0f;
		if (sceneIds != null)
		{
			foreach (string text in sceneIds)
			{
				if (!string.IsNullOrEmpty(text))
				{
					AddSceneItem(text);
				}
			}
		}
		if (sceneTrailId != null && !string.IsNullOrEmpty(sceneTrailId))
		{
			AddSceneItem(sceneTrailId);
		}
		m_Inited = true;
	}

	public void AddSceneItem(string sceneId)
	{
		if (m_SceneDataLoader != null && m_SceneDataLoader.ReadRowAllDataById(sceneId, in m_TmpRowDatas, out var _))
		{
			SceneEditData sceneEditData = new SceneEditData();
			sceneEditData.Init(m_TmpRowDatas);
			sceneEditData.Offset = m_Offset;
			sceneEditData.LoadShowPrefab();
			m_SceneDatas.Add(sceneEditData);
			m_Offset += sceneEditData.Size;
		}
	}

	public void OnUpdate()
	{
		try
		{
			m_ToDestroySceneDatas.Clear();
			float num = 0f;
			for (int i = 0; i < m_SceneDatas.Count; i++)
			{
				SceneEditData sceneEditData = m_SceneDatas[i];
				sceneEditData.Offset = Mathf.Max(0f, sceneEditData.Offset - num);
				if (sceneEditData.IsDeletedInScene())
				{
					m_ToDestroySceneDatas.Add(sceneEditData);
					num += sceneEditData.Size;
				}
				else
				{
					sceneEditData.OnUpdate();
				}
			}
			foreach (SceneEditData toDestroySceneData in m_ToDestroySceneDatas)
			{
				m_Offset = Mathf.Max(0f, m_Offset - toDestroySceneData.Size);
				toDestroySceneData.Dispose();
				m_SceneDatas.Remove(toDestroySceneData);
			}
			int num2 = m_SceneDatas.Count - 1;
			if (num2 >= 0)
			{
				SceneEditData sceneEditData2 = m_SceneDatas[num2];
				if (m_HeadSceneData == null || m_HeadSceneData.ScenePrefabPath == null || !string.Equals(m_HeadSceneData.ScenePrefabPath, sceneEditData2.ScenePrefabPath))
				{
					DestroyHeadScenePrefab();
					m_HeadSceneData = sceneEditData2;
					StagePrefabUtil.GetStagePrefabGenerator().InstantiatePrefabAsync(sceneEditData2.ScenePrefabPath, null, OnHeadScenePrefabInstantiated);
				}
			}
			else
			{
				DestroyHeadScenePrefab();
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	private void OnHeadScenePrefabInstantiated(GameObject prefab)
	{
		m_HeadScenePrefab = prefab;
		float num = ((m_HeadSceneData != null) ? m_HeadSceneData.Size : 1f);
		if (m_HeadScenePrefab != null)
		{
			m_HeadScenePrefab.name = "SceneHead";
			m_HeadScenePrefab.hideFlags = HideFlags.HideInHierarchy;
			m_HeadScenePrefab.transform.position = new Vector3(0f, 0f, -1f * num);
		}
	}

	public bool SaveToDataLoader()
	{
		return true;
	}

	public string GetSceneTrailId()
	{
		if (m_SceneDatas.Count == 0)
		{
			return string.Empty;
		}
		int index = m_SceneDatas.Count - 1;
		return m_SceneDatas[index].SceneId;
	}

	public string GetSceneParams()
	{
		return SceneDataParamConvert.ConvertSceneListToParam(m_SceneDatas);
	}

	public List<SceneEditData> GetSceneEditDatas()
	{
		return m_SceneDatas;
	}

	public void DrawView()
	{
		for (int i = 0; i < m_SceneDatas.Count; i++)
		{
			SceneEditData sceneEditData = m_SceneDatas[i];
			if (!sceneEditData.IsDeletedInScene())
			{
				sceneEditData.DrawView();
			}
		}
	}

	public IStageEditData CheckSelected(GameObject selectedObject)
	{
		if (selectedObject == null)
		{
			return null;
		}
		IStageEditData result = null;
		foreach (SceneEditData sceneData in m_SceneDatas)
		{
			if (sceneData.GetScenePrefab() == selectedObject)
			{
				sceneData.SetSelected(isSelected: true);
				result = sceneData;
			}
			else
			{
				sceneData.SetSelected(isSelected: false);
			}
		}
		return result;
	}

	public void Dispose()
	{
		m_Inited = false;
		m_HeadSceneData = null;
		m_ToDestroySceneDatas.Clear();
		DestroySceneDatas();
		DestroyHeadScenePrefab();
		m_TmpRowDatas.Clear();
		m_SceneDataLoader = null;
		m_Offset = 0f;
	}

	private void DestroyHeadScenePrefab()
	{
		if (m_HeadScenePrefab != null)
		{
			StagePrefabUtil.GetStagePrefabGenerator().DestroyInstantiatedPrefab(m_HeadScenePrefab);
			m_HeadScenePrefab = null;
		}
		m_HeadSceneData = null;
	}

	private void DestroySceneDatas()
	{
		foreach (SceneEditData sceneData in m_SceneDatas)
		{
			sceneData.Dispose();
		}
		m_SceneDatas.Clear();
	}
}
