using System;
using System.Collections.Generic;
using ParkourStage.Runtime.Data;
using UnityEngine;

namespace ParkourStage.Runtime.Manager;

public class StageMonsterBornDataEditManager : IStageDataEditManager, IDisposable
{
	private readonly List<MonsterBornEditData> m_EditorDatas = new List<MonsterBornEditData>();

	private readonly List<RowData> m_TmpRowDatas = new List<RowData>();

	private readonly List<MonsterBornEditData> m_ToDestroyEditorDatas = new List<MonsterBornEditData>();

	private IDataLoader m_MonsterDataLoader;

	private IDataLoader m_TriggerDataLoader;

	private IDataLoader m_MonsterBornDataLoader;

	private long m_MaxEditorDataIdNum;

	private string m_MaxEditorDataId;

	private bool m_Inited;

	private bool m_SupportV3;

	private readonly Queue<MonsterBornEditData> m_ToCreateEditorDatas = new Queue<MonsterBornEditData>();

	private const int FRAME_CREATE = 1;

	public bool Inited => m_Inited;

	public void Init(IDataLoader monsterDataLoader, IDataLoader triggerDataLoader, IDataLoader monsterBornDataLoader, string[] monsterBornIds)
	{
		m_TmpRowDatas.Clear();
		m_ToDestroyEditorDatas.Clear();
		m_ToCreateEditorDatas.Clear();
		DestroyAllEditorDatas();
		if (monsterDataLoader == null)
		{
			Debug.LogError("StageEditorBaseMonsterMgr Init failed!,monsterDataLoader is null");
			return;
		}
		if (triggerDataLoader == null)
		{
			Debug.LogError("StageEditorBaseMonsterMgr Init failed!,triggerDataLoader is null");
			return;
		}
		if (monsterBornDataLoader == null)
		{
			Debug.LogError("StageEditorBaseMonsterMgr Init failed!,monsterBornDataLoader is null");
			return;
		}
		m_MonsterDataLoader = monsterDataLoader;
		m_TriggerDataLoader = triggerDataLoader;
		m_MonsterBornDataLoader = monsterBornDataLoader;
		if (monsterBornIds != null)
		{
			foreach (string monsterBornId in monsterBornIds)
			{
				AddEditorDataItem(monsterBornId);
			}
		}
		m_Inited = true;
	}

	public List<MonsterBornEditData> GetMonsterBornEditDatas()
	{
		return m_EditorDatas;
	}

	public int GetTotalCountMonsterNum()
	{
		int num = 0;
		int count = m_EditorDatas.Count;
		for (int i = 0; i < count; i++)
		{
			MonsterBornEditData monsterBornEditData = m_EditorDatas[i];
			if (monsterBornEditData.IsCountingMonster())
			{
				num += monsterBornEditData.GenNumber;
			}
		}
		return num;
	}

	public string GetAllEditorDatasIdParam()
	{
		return MonsterBornDataParamConvert.ConvertMonsterBornListToParam(m_EditorDatas);
	}

	public void AddEditorDataItem(string monsterBornId)
	{
		int rowIndex;
		if (m_MonsterBornDataLoader == null)
		{
			Debug.LogError("AddEditorDataItem failed!,m_MonsterBornDataLoader is null");
		}
		else if (m_MonsterBornDataLoader.ReadRowAllDataById(monsterBornId, in m_TmpRowDatas, out rowIndex))
		{
			MonsterBornEditData monsterBornEditData = new MonsterBornEditData(m_MonsterDataLoader, m_TriggerDataLoader);
			monsterBornEditData.Init(m_TmpRowDatas);
			m_ToCreateEditorDatas.Enqueue(monsterBornEditData);
			m_EditorDatas.Add(monsterBornEditData);
			long.TryParse(monsterBornId, out var result);
			if (result > m_MaxEditorDataIdNum)
			{
				m_MaxEditorDataIdNum = result;
				m_MaxEditorDataId = monsterBornId;
			}
		}
		else
		{
			Debug.LogError("AddEditorDataItem failed!,monsterBornId:" + monsterBornId + ",unable to find monster born data.");
		}
	}

	private void RemoveEditorDataItem(MonsterBornEditData monsterBornData)
	{
		m_EditorDatas.Remove(monsterBornData);
		long.TryParse(monsterBornData.MonsterBornId, out var result);
		if (result == m_MaxEditorDataIdNum)
		{
			m_MaxEditorDataIdNum = 0L;
			int count = m_EditorDatas.Count;
			for (int i = 0; i < count; i++)
			{
				MonsterBornEditData monsterBornEditData = m_EditorDatas[i];
				long.TryParse(monsterBornEditData.MonsterBornId, out var result2);
				if (result2 > m_MaxEditorDataIdNum)
				{
					m_MaxEditorDataIdNum = result2;
					m_MaxEditorDataId = monsterBornEditData.MonsterBornId;
				}
			}
		}
		monsterBornData.Dispose();
	}

	public long GetMaxMonsterBornIdNum()
	{
		return m_MaxEditorDataIdNum;
	}

	public MonsterBornData GetNextOneMonsterTypeBornData(long stageBasicId, string monsterId, Vector3 point)
	{
		MonsterBornData monsterBornData = GetNextOneNewMonsterBornData(stageBasicId, point);
		if (monsterBornData != null)
		{
			monsterBornData.BornType = MonsterBornType.TriggerLinePoint;
			string value = monsterId + "|10000";
			MonsterBornDataParamConvert.ConvertMonsterParam(in monsterBornData, value, m_MonsterDataLoader, m_TriggerDataLoader, checkValid: false, out var _);
		}
		return monsterBornData;
	}

	public MonsterBornData GetNextOneBuffTypeBornData(long stageBasicId, string triggerId, Vector3 point)
	{
		MonsterBornData monsterBornData = GetNextOneNewMonsterBornData(stageBasicId, point);
		if (monsterBornData != null)
		{
			monsterBornData.BornType = MonsterBornType.BuffBall;
			string value = triggerId + "|10000";
			MonsterBornDataParamConvert.ConvertBuffParam(in monsterBornData, value, m_TriggerDataLoader, checkValid: false, out var _);
		}
		return monsterBornData;
	}

	private MonsterBornData GetNextOneNewMonsterBornData(long stageBasicId, Vector3 point)
	{
		if (!m_Inited)
		{
			Debug.LogError("StageEditorMonsterBornDataMgr GetOneNewMonsterBornData failed! Init first!");
			return null;
		}
		long num = Math.Max(GetMaxMonsterBornIdNum() + 1, stageBasicId);
		return new MonsterBornData
		{
			MonsterBornId = num.ToString(),
			Coord = point,
			GenZ = Math.Max(point.z - 45f, 10f)
		};
	}

	public bool AddMonsterBornData(MonsterBornData info)
	{
		if (m_MonsterBornDataLoader == null)
		{
			Debug.LogError("AddMonsterBornData failed!,m_MonsterBornDataLoader is null");
			return false;
		}
		if (info == null)
		{
			Debug.LogError("AddMonsterBornData failed!,info is null");
			return false;
		}
		m_MonsterBornDataLoader.GetEmptyRowData(in m_TmpRowDatas);
		MonsterBornDataParamConvert.ConvertDataToRowData(in info, in m_TmpRowDatas, m_SupportV3);
		int num = m_MonsterBornDataLoader.ContainsId(info.MonsterBornId);
		if (num != -1)
		{
			return m_MonsterBornDataLoader.WriteRowData(num, in m_TmpRowDatas);
		}
		int rowCount = m_MonsterBornDataLoader.RowCount;
		if (string.IsNullOrEmpty(m_MaxEditorDataId))
		{
			num = rowCount + 1;
			return m_MonsterBornDataLoader.WriteRowData(num, in m_TmpRowDatas);
		}
		int num2 = m_MonsterBornDataLoader.ContainsId(m_MaxEditorDataId);
		if (num2 == -1)
		{
			num = rowCount + 1;
			return m_MonsterBornDataLoader.WriteRowData(num, in m_TmpRowDatas);
		}
		if (num2 + 1 > m_MonsterBornDataLoader.RowCount)
		{
			num = rowCount + 1;
			return m_MonsterBornDataLoader.WriteRowData(num, in m_TmpRowDatas);
		}
		return m_MonsterBornDataLoader.InsertNewRowData(num2 + 1, in m_TmpRowDatas);
	}

	public void SupportV3(bool v3Support)
	{
		m_SupportV3 = v3Support;
	}

	private void WriteMonsterBornData(MonsterBornData info)
	{
		if (info == null)
		{
			Debug.LogError("WriteMonsterBornData failed!,info is null");
			return;
		}
		int num = m_MonsterBornDataLoader.ContainsId(info.MonsterBornId);
		if (num == -1)
		{
			Debug.LogError("WriteMonsterBornData failed!,Id :" + info.MonsterBornId + " is not exist");
			return;
		}
		m_MonsterBornDataLoader.GetEmptyRowData(in m_TmpRowDatas);
		MonsterBornDataParamConvert.ConvertDataToRowData(in info, in m_TmpRowDatas, m_SupportV3);
		m_MonsterBornDataLoader.WriteRowData(num, in m_TmpRowDatas);
	}

	public bool SaveToDataLoader()
	{
		if (m_MonsterBornDataLoader == null)
		{
			Debug.LogError("WriteMonsterBornData failed!,m_MonsterBornDataLoader is null");
			return false;
		}
		foreach (MonsterBornEditData editorData in m_EditorDatas)
		{
			MonsterBornData monsterBornData = editorData.GetMonsterBornData();
			WriteMonsterBornData(monsterBornData);
		}
		return true;
	}

	public void OnUpdate()
	{
		try
		{
			m_ToDestroyEditorDatas.Clear();
			foreach (MonsterBornEditData editorData in m_EditorDatas)
			{
				if (editorData.IsDeletedInScene())
				{
					m_ToDestroyEditorDatas.Add(editorData);
				}
				else
				{
					editorData.OnUpdate();
				}
			}
			foreach (MonsterBornEditData toDestroyEditorData in m_ToDestroyEditorDatas)
			{
				RemoveEditorDataItem(toDestroyEditorData);
			}
			int num = 1;
			while (num > 0 && m_ToCreateEditorDatas.Count > 0)
			{
				m_ToCreateEditorDatas.Dequeue().LoadShowPrefab();
				num--;
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}

	public IStageEditData CheckSelected(GameObject selectedObject)
	{
		if (selectedObject == null)
		{
			return null;
		}
		IStageEditData result = null;
		foreach (MonsterBornEditData editorData in m_EditorDatas)
		{
			if (editorData.GetShowedPrefab() == selectedObject)
			{
				editorData.SetSelected(isSelected: true);
				result = editorData;
			}
			else
			{
				editorData.SetSelected(isSelected: false);
			}
		}
		return result;
	}

	public void DrawView()
	{
		foreach (MonsterBornEditData editorData in m_EditorDatas)
		{
			if (!editorData.IsDeletedInScene())
			{
				editorData.DrawView();
			}
		}
	}

	public void Dispose()
	{
		m_MaxEditorDataIdNum = 0L;
		m_MaxEditorDataId = string.Empty;
		m_Inited = false;
		m_ToDestroyEditorDatas.Clear();
		m_ToCreateEditorDatas.Clear();
		DestroyAllEditorDatas();
		m_TmpRowDatas.Clear();
		m_MonsterDataLoader = null;
		m_MonsterBornDataLoader = null;
		m_TriggerDataLoader = null;
		m_SupportV3 = false;
	}

	private void DestroyAllEditorDatas()
	{
		foreach (MonsterBornEditData editorData in m_EditorDatas)
		{
			editorData.Dispose();
		}
		m_EditorDatas.Clear();
	}
}
