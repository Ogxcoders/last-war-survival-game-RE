using System;
using System.Collections.Generic;
using ParkourStage.Runtime.Util;
using TMPro;
using UnityEngine;

namespace ParkourStage.Runtime.Data;

public class MonsterBornEditData : IStageEditData, IDisposable
{
	private bool m_IsSelected;

	private IDataLoader m_MonsterDataLoader;

	private IDataLoader m_TriggerDataLoader;

	private bool m_PrefabLoaded;

	private bool m_ShowPrefabLoadError;

	private readonly MonsterBornData m_MonsterBornData = new MonsterBornData();

	private bool m_Destroyed;

	public string MonsterBornId => m_MonsterBornData.MonsterBornId;

	public MonsterBornType BornType => m_MonsterBornData.BornType;

	public Vector3 Coord => m_MonsterBornData.Coord;

	public float GenZ
	{
		get
		{
			return m_MonsterBornData.GenZ;
		}
		set
		{
			m_MonsterBornData.GenZ = value;
		}
	}

	public int GenNumber
	{
		get
		{
			return m_MonsterBornData.GenNumber;
		}
		set
		{
			m_MonsterBornData.GenNumber = value;
		}
	}

	public int GenDelta
	{
		get
		{
			return m_MonsterBornData.GenDelta;
		}
		set
		{
			m_MonsterBornData.GenDelta = value;
		}
	}

	public float R1
	{
		get
		{
			return m_MonsterBornData.R1;
		}
		set
		{
			m_MonsterBornData.R1 = value;
		}
	}

	public float R2
	{
		get
		{
			return m_MonsterBornData.R2;
		}
		set
		{
			m_MonsterBornData.R2 = value;
		}
	}

	public ViewRuleType ViewType
	{
		get
		{
			return m_MonsterBornData.ViewType;
		}
		set
		{
			m_MonsterBornData.ViewType = value;
		}
	}

	public float ViewOffset
	{
		get
		{
			return m_MonsterBornData.ViewOffset;
		}
		set
		{
			m_MonsterBornData.ViewOffset = value;
		}
	}

	public MoveRuleType MoveType
	{
		get
		{
			return m_MonsterBornData.MoveType;
		}
		set
		{
			m_MonsterBornData.MoveType = value;
		}
	}

	public Vector2 MoveCenterOffset
	{
		get
		{
			return m_MonsterBornData.MoveCenterOffset;
		}
		set
		{
			m_MonsterBornData.MoveCenterOffset = value;
		}
	}

	public string MoveExtraParam
	{
		get
		{
			return m_MonsterBornData.MoveExtraParam;
		}
		set
		{
			m_MonsterBornData.MoveExtraParam = value;
		}
	}

	public bool MoveDirectionFactor
	{
		get
		{
			return m_MonsterBornData.MoveDirectionFactor == 1;
		}
		set
		{
			m_MonsterBornData.MoveDirectionFactor = (value ? 1 : (-1));
		}
	}

	public string MonsterParams => m_MonsterBornData.MonsterParams;

	public string BuffParams => m_MonsterBornData.BuffParams;

	public float ShowedMonsterAlertRange => m_MonsterBornData.ShowedMonsterData?.AlertRange ?? 0f;

	public float MoveEllipseLikeAScaleFactor
	{
		get
		{
			return m_MonsterBornData.MoveEllipseLikeAScaleFactor;
		}
		set
		{
			m_MonsterBornData.MoveEllipseLikeAScaleFactor = value;
		}
	}

	public GameObject ShowedPrefab { get; private set; }

	public bool Selected => m_IsSelected;

	public bool ShowPrefabLoadError => m_ShowPrefabLoadError;

	public MonsterBornEditData(IDataLoader monsterDataLoader, IDataLoader triggerDataLoader)
	{
		m_MonsterDataLoader = monsterDataLoader;
		m_TriggerDataLoader = triggerDataLoader;
	}

	public void Init(List<RowData> rowDatas)
	{
		MonsterBornDataParamConvert.ConvertByRowDatas(in m_MonsterBornData, in rowDatas, m_MonsterDataLoader, m_TriggerDataLoader, checkValid: false);
	}

	public bool IsCountingMonster()
	{
		if (m_MonsterBornData.ShowedMonsterData == null)
		{
			return false;
		}
		MonsterData showedMonsterData = m_MonsterBornData.ShowedMonsterData;
		if (showedMonsterData.MonsterType != MonsterType.Boss && showedMonsterData.MonsterType != MonsterType.Elite && showedMonsterData.MonsterType != MonsterType.Normal)
		{
			return showedMonsterData.MonsterType == MonsterType.SkyBattleNormal;
		}
		return true;
	}

	public void LoadShowPrefab()
	{
		DestroyShowedPrefab();
		if (BornType == MonsterBornType.SingleMonster || BornType == MonsterBornType.TriggerLinePoint || BornType == MonsterBornType.Resource || BornType == MonsterBornType.BuffBall)
		{
			Transform monsterBornRoot = StagePrefabUtil.GetStagePrefabGenerator().GetMonsterBornRoot();
			string prefabPath = ((m_MonsterBornData.ShowedMonsterData != null) ? m_MonsterBornData.ShowedMonsterData.MonsterPath : m_MonsterBornData.ShowedBuffPath);
			StagePrefabUtil.GetStagePrefabGenerator().InstantiatePrefabAsync(prefabPath, monsterBornRoot, OnShowedPrefabInsComplete);
		}
	}

	public bool TryParseMonsterParams(string monsterParams, out string msg)
	{
		return MonsterBornDataParamConvert.ConvertMonsterParam(in m_MonsterBornData, monsterParams, m_MonsterDataLoader, m_TriggerDataLoader, checkValid: true, out msg);
	}

	public bool TryParseBuffParams(string buffParams, out string msg)
	{
		return MonsterBornDataParamConvert.ConvertBuffParam(in m_MonsterBornData, buffParams, m_TriggerDataLoader, checkValid: true, out msg);
	}

	private void OnShowedPrefabDynamicPrefabInsComplete(GameObject dynamicObj)
	{
		if (dynamicObj != null)
		{
			dynamicObj.transform.localPosition = Vector3.zero;
			dynamicObj.transform.localRotation = Quaternion.identity;
			dynamicObj.transform.localScale = Vector3.one;
		}
	}

	private void OnShowedPrefabInsComplete(GameObject prefab)
	{
		ShowedPrefab = prefab;
		bool flag = m_MonsterBornData.ShowedMonsterData != null;
		float num = (flag ? m_MonsterBornData.ShowedMonsterData.Size : 1f);
		if (ShowedPrefab != null)
		{
			ShowedPrefab.transform.position = Coord;
			ShowedPrefab.transform.localScale = Vector3.one * num;
			if (flag && (m_MonsterBornData.ShowedMonsterData.MonsterType == MonsterType.NumberDoor || m_MonsterBornData.ShowedMonsterData.MonsterType == MonsterType.DynamicTable || m_MonsterBornData.ShowedMonsterData.MonsterType == MonsterType.Table))
			{
				ShowedPrefab.transform.rotation = Quaternion.identity;
			}
			else
			{
				ShowedPrefab.transform.rotation = Quaternion.LookRotation(new Vector3(0f, 0f, -1f), Vector3.up);
			}
			if (flag)
			{
				if (m_MonsterBornData.ShowedMonsterData.MonsterType == MonsterType.NumberDoor)
				{
					TextMeshPro componentInChildren = ShowedPrefab.GetComponentInChildren<TextMeshPro>();
					if (componentInChildren != null)
					{
						componentInChildren.text = m_MonsterBornData.ShowedMonsterData.DoorNumber.ToString();
					}
				}
				Transform transform = ShowedPrefab.transform.Find("HpText");
				if (transform != null)
				{
					TextMeshPro component = transform.GetComponent<TextMeshPro>();
					if (component != null)
					{
						component.text = m_MonsterBornData.ShowedMonsterData.PropertyHp.ToString();
					}
				}
				if (!string.IsNullOrEmpty(m_MonsterBornData.ShowedMonsterData.DynamicObjectPath))
				{
					Transform transform2 = ShowedPrefab.transform.Find("Node");
					if (transform2 != null)
					{
						StagePrefabUtil.GetStagePrefabGenerator().InstantiatePrefabAsync(m_MonsterBornData.ShowedMonsterData.DynamicObjectPath, transform2, OnShowedPrefabDynamicPrefabInsComplete);
					}
				}
				if (m_MonsterBornData.ShowedMonsterData.DeathTrigger != null && m_MonsterBornData.ShowedMonsterData.DeathTrigger.TriggerType == TriggerEventType.AddEnergy)
				{
					TextMeshPro textMeshPro = new GameObject("TriggerStatus").AddComponent<TextMeshPro>();
					textMeshPro.transform.SetParent(ShowedPrefab.transform);
					textMeshPro.transform.localPosition = new Vector3(0f, 5f, 0f);
					textMeshPro.alignment = TextAlignmentOptions.Bottom;
					textMeshPro.fontSize = 16f;
					textMeshPro.color = Color.cyan;
					textMeshPro.text = m_MonsterBornData.ShowedMonsterData.DeathTrigger.AddEnergyIndex.ToString();
				}
			}
		}
		m_ShowPrefabLoadError = ShowedPrefab == null;
		m_PrefabLoaded = true;
	}

	public MonsterBornData GetMonsterBornData()
	{
		return m_MonsterBornData;
	}

	public bool IsDeletedInScene()
	{
		if (m_PrefabLoaded && !m_ShowPrefabLoadError)
		{
			return ShowedPrefab == null;
		}
		return false;
	}

	public void SetSelected(bool isSelected)
	{
		m_IsSelected = isSelected;
	}

	public GameObject GetShowedPrefab()
	{
		return ShowedPrefab;
	}

	public void OnUpdate()
	{
		if (ShowedPrefab != null && m_IsSelected)
		{
			m_MonsterBornData.Coord = ShowedPrefab.transform.position;
		}
	}

	public void DrawView()
	{
		if (m_IsSelected && (bool)ShowedPrefab && BornType == MonsterBornType.TriggerLinePoint)
		{
			DrawViewUtil.GetViewDrawer().DrawView(this);
		}
	}

	public bool IsDestroyed()
	{
		return m_Destroyed;
	}

	public bool IsShowMoveAndVisibleLine()
	{
		if (m_MonsterBornData.ShowedMonsterData != null)
		{
			if (m_MonsterBornData.ShowedMonsterData.MonsterType != MonsterType.SkyBattleNormal)
			{
				return m_MonsterBornData.ShowedMonsterData.MonsterType == MonsterType.SkyBattleTestNormal;
			}
			return true;
		}
		return false;
	}

	public void DestroyShowedPrefab()
	{
		if (ShowedPrefab != null)
		{
			StagePrefabUtil.GetStagePrefabGenerator().DestroyInstantiatedPrefab(ShowedPrefab);
			ShowedPrefab = null;
		}
	}

	public void Dispose()
	{
		DestroyShowedPrefab();
		m_MonsterDataLoader = null;
		m_TriggerDataLoader = null;
		m_ShowPrefabLoadError = false;
		m_PrefabLoaded = false;
		m_Destroyed = true;
	}
}
