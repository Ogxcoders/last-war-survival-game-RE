using System;
using System.Collections.Generic;
using ParkourStage.Runtime.Data;
using UnityEngine;

namespace ParkourStage.Runtime.Manager;

public class StageDatasEditManager : IStageDataEditManager, IDisposable
{
	private readonly StageSceneDataEditManager m_SceneDataEditManager = new StageSceneDataEditManager();

	private readonly StageMonsterBornDataEditManager m_MonsterBornDataEditManager = new StageMonsterBornDataEditManager();

	private IDataLoader m_MonsterDataLoader;

	private IDataLoader m_SceneDataLoader;

	private IDataLoader m_MonsterBornDataLoader;

	private IDataLoader m_TriggerDataLoader;

	private IDataLoader m_StageDataLoader;

	private bool m_Inited;

	private readonly List<RowData> m_TmpRowData = new List<RowData>();

	private readonly StageData m_StageData = new StageData();

	private static StageDatasEditManager ms_Instance;

	public string[] SceneIds => m_StageData.SceneIds;

	public string SceneTrailId => m_StageData.SceneTrailId;

	public string[] MonsterBornIds => m_StageData.MonsterBornIds;

	public string StageId => m_StageData.StageId;

	public long StageMonsterBornBasicId => m_StageData.StageMonsterBornBasicId;

	public bool Inited => m_Inited;

	public static StageDatasEditManager Instance
	{
		get
		{
			if (ms_Instance == null)
			{
				ms_Instance = new StageDatasEditManager();
			}
			return ms_Instance;
		}
	}

	private StageDatasEditManager()
	{
	}

	public bool Init(string stageId, IDataLoader stageDataLoader, IDataLoader monsterDataLoader, IDataLoader sceneDataLoader, IDataLoader triggerDataLoader, IDataLoader monsterBornDataLoader)
	{
		m_StageDataLoader = stageDataLoader;
		m_MonsterDataLoader = monsterDataLoader;
		m_SceneDataLoader = sceneDataLoader;
		m_MonsterBornDataLoader = monsterBornDataLoader;
		m_TriggerDataLoader = triggerDataLoader;
		m_Inited = false;
		if (m_StageDataLoader.ReadRowAllDataById(stageId, in m_TmpRowData, out var _))
		{
			StageDataParamConvert.ConvertByRowDatas(in m_StageData, m_TmpRowData);
			m_SceneDataEditManager.Init(m_SceneDataLoader, SceneIds, SceneTrailId);
			m_MonsterBornDataEditManager.Init(m_MonsterDataLoader, m_TriggerDataLoader, m_MonsterBornDataLoader, MonsterBornIds);
			m_Inited = true;
			return true;
		}
		Debug.LogError("StageEditorDataMgr Init error! : " + stageId);
		return false;
	}

	public void SupportV3(bool v3Support)
	{
		m_MonsterBornDataEditManager.SupportV3(v3Support);
	}

	public bool SaveToDataLoader()
	{
		if (!m_Inited)
		{
			return false;
		}
		if (!m_MonsterBornDataEditManager.SaveToDataLoader())
		{
			return false;
		}
		if (!m_StageDataLoader.ReadRowAllDataById(StageId, in m_TmpRowData, out var rowIndex))
		{
			return false;
		}
		StageDataParamConvert.ConvertDataToRowData(m_MonsterBornDataEditManager.GetAllEditorDatasIdParam(), m_SceneDataEditManager.GetSceneParams(), m_SceneDataEditManager.GetSceneTrailId(), in m_TmpRowData);
		return m_StageDataLoader.WriteRowData(rowIndex, in m_TmpRowData);
	}

	public void AddSceneItem(string sceneId)
	{
		if (!m_Inited)
		{
			Debug.LogError("StageData AddScene failed! Init first!");
		}
		m_SceneDataEditManager.AddSceneItem(sceneId);
	}

	public List<SceneEditData> GetSceneEditDatas()
	{
		return m_SceneDataEditManager.GetSceneEditDatas();
	}

	public List<MonsterBornEditData> GetMonsterBornEditDatas()
	{
		return m_MonsterBornDataEditManager.GetMonsterBornEditDatas();
	}

	public int GetTotalCountMonsterNum()
	{
		return m_MonsterBornDataEditManager.GetTotalCountMonsterNum();
	}

	public bool PutMonsterBorn(MonsterBornData info)
	{
		if (!m_Inited)
		{
			Debug.LogError("StageData PutMonsterBorn failed! Init first!");
			return false;
		}
		bool num = m_MonsterBornDataEditManager.AddMonsterBornData(info);
		if (num)
		{
			m_MonsterBornDataEditManager.AddEditorDataItem(info.MonsterBornId);
		}
		return num;
	}

	public MonsterBornData GetNextOneMonsterTypeBornData(string monsterId, Vector3 point)
	{
		return m_MonsterBornDataEditManager.GetNextOneMonsterTypeBornData(m_StageData.StageMonsterBornBasicId, monsterId, point);
	}

	public MonsterBornData GetNextOneBuffTypeBornData(string triggerId, Vector3 point)
	{
		return m_MonsterBornDataEditManager.GetNextOneBuffTypeBornData(m_StageData.StageMonsterBornBasicId, triggerId, point);
	}

	public void OnUpdate()
	{
		m_SceneDataEditManager.OnUpdate();
		m_MonsterBornDataEditManager.OnUpdate();
	}

	public void DrawView()
	{
		m_SceneDataEditManager.DrawView();
		m_MonsterBornDataEditManager.DrawView();
	}

	public IStageEditData CheckSelected(GameObject selectedObject)
	{
		if (selectedObject == null)
		{
			return null;
		}
		IStageEditData stageEditData = null;
		stageEditData = m_SceneDataEditManager.CheckSelected(selectedObject);
		if (stageEditData == null)
		{
			stageEditData = m_MonsterBornDataEditManager.CheckSelected(selectedObject);
		}
		return stageEditData;
	}

	public void Dispose()
	{
		m_TmpRowData.Clear();
		m_SceneDataEditManager.Dispose();
		m_MonsterBornDataEditManager.Dispose();
		m_MonsterDataLoader = null;
		m_StageDataLoader = null;
		m_SceneDataLoader = null;
		m_Inited = false;
	}
}
