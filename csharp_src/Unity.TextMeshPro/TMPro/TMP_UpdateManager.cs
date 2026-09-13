using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro;

public class TMP_UpdateManager
{
	private static TMP_UpdateManager s_Instance;

	private readonly HashSet<int> m_LayoutQueueLookup = new HashSet<int>();

	private readonly List<TMP_Text> m_LayoutRebuildQueue = new List<TMP_Text>();

	private readonly HashSet<int> m_GraphicQueueLookup = new HashSet<int>();

	private readonly List<TMP_Text> m_GraphicRebuildQueue = new List<TMP_Text>();

	private readonly HashSet<int> m_InternalUpdateLookup = new HashSet<int>();

	private readonly List<TMP_Text> m_InternalUpdateQueue = new List<TMP_Text>();

	private readonly HashSet<int> m_CullingUpdateLookup = new HashSet<int>();

	private readonly List<TMP_Text> m_CullingUpdateQueue = new List<TMP_Text>();

	private static ProfilerMarker k_RegisterTextObjectForUpdateMarker = new ProfilerMarker("TMP.RegisterTextObjectForUpdate");

	private static ProfilerMarker k_RegisterTextElementForGraphicRebuildMarker = new ProfilerMarker("TMP.RegisterTextElementForGraphicRebuild");

	private static ProfilerMarker k_RegisterTextElementForCullingUpdateMarker = new ProfilerMarker("TMP.RegisterTextElementForCullingUpdate");

	private static ProfilerMarker k_UnregisterTextObjectForUpdateMarker = new ProfilerMarker("TMP.UnregisterTextObjectForUpdate");

	private static ProfilerMarker k_UnregisterTextElementForGraphicRebuildMarker = new ProfilerMarker("TMP.UnregisterTextElementForGraphicRebuild");

	private static TMP_UpdateManager instance
	{
		get
		{
			if (s_Instance == null)
			{
				s_Instance = new TMP_UpdateManager();
			}
			return s_Instance;
		}
	}

	private TMP_UpdateManager()
	{
		Canvas.willRenderCanvases += DoRebuilds;
	}

	internal static void RegisterTextObjectForUpdate(TMP_Text textObject)
	{
		instance.InternalRegisterTextObjectForUpdate(textObject);
	}

	private void InternalRegisterTextObjectForUpdate(TMP_Text textObject)
	{
		int instanceID = textObject.GetInstanceID();
		if (!m_InternalUpdateLookup.Contains(instanceID))
		{
			m_InternalUpdateLookup.Add(instanceID);
			m_InternalUpdateQueue.Add(textObject);
		}
	}

	public static void RegisterTextElementForLayoutRebuild(TMP_Text element)
	{
		instance.InternalRegisterTextElementForLayoutRebuild(element);
	}

	private void InternalRegisterTextElementForLayoutRebuild(TMP_Text element)
	{
		int instanceID = element.GetInstanceID();
		if (!m_LayoutQueueLookup.Contains(instanceID))
		{
			m_LayoutQueueLookup.Add(instanceID);
			m_LayoutRebuildQueue.Add(element);
		}
	}

	public static void RegisterTextElementForGraphicRebuild(TMP_Text element)
	{
		instance.InternalRegisterTextElementForGraphicRebuild(element);
	}

	private void InternalRegisterTextElementForGraphicRebuild(TMP_Text element)
	{
		int instanceID = element.GetInstanceID();
		if (!m_GraphicQueueLookup.Contains(instanceID))
		{
			m_GraphicQueueLookup.Add(instanceID);
			m_GraphicRebuildQueue.Add(element);
		}
	}

	public static void RegisterTextElementForCullingUpdate(TMP_Text element)
	{
		instance.InternalRegisterTextElementForCullingUpdate(element);
	}

	private void InternalRegisterTextElementForCullingUpdate(TMP_Text element)
	{
		int instanceID = element.GetInstanceID();
		if (!m_CullingUpdateLookup.Contains(instanceID))
		{
			m_CullingUpdateLookup.Add(instanceID);
			m_CullingUpdateQueue.Add(element);
		}
	}

	private void OnCameraPreCull()
	{
		DoRebuilds();
	}

	private void DoRebuilds()
	{
		for (int i = 0; i < m_InternalUpdateQueue.Count; i++)
		{
			m_InternalUpdateQueue[i].InternalUpdate();
		}
		for (int j = 0; j < m_LayoutRebuildQueue.Count; j++)
		{
			m_LayoutRebuildQueue[j].Rebuild(CanvasUpdate.Prelayout);
		}
		if (m_LayoutRebuildQueue.Count > 0)
		{
			m_LayoutRebuildQueue.Clear();
			m_LayoutQueueLookup.Clear();
		}
		for (int k = 0; k < m_GraphicRebuildQueue.Count; k++)
		{
			m_GraphicRebuildQueue[k].Rebuild(CanvasUpdate.PreRender);
		}
		if (m_GraphicRebuildQueue.Count > 0)
		{
			m_GraphicRebuildQueue.Clear();
			m_GraphicQueueLookup.Clear();
		}
		for (int l = 0; l < m_CullingUpdateQueue.Count; l++)
		{
			m_CullingUpdateQueue[l].UpdateCulling();
		}
		if (m_CullingUpdateQueue.Count > 0)
		{
			m_CullingUpdateQueue.Clear();
			m_CullingUpdateLookup.Clear();
		}
	}

	internal static void UnRegisterTextObjectForUpdate(TMP_Text textObject)
	{
		instance.InternalUnRegisterTextObjectForUpdate(textObject);
	}

	public static void UnRegisterTextElementForRebuild(TMP_Text element)
	{
		instance.InternalUnRegisterTextElementForGraphicRebuild(element);
		instance.InternalUnRegisterTextElementForLayoutRebuild(element);
		instance.InternalUnRegisterTextObjectForUpdate(element);
	}

	private void InternalUnRegisterTextElementForGraphicRebuild(TMP_Text element)
	{
		int instanceID = element.GetInstanceID();
		m_GraphicRebuildQueue.Remove(element);
		m_GraphicQueueLookup.Remove(instanceID);
	}

	private void InternalUnRegisterTextElementForLayoutRebuild(TMP_Text element)
	{
		int instanceID = element.GetInstanceID();
		m_LayoutRebuildQueue.Remove(element);
		m_LayoutQueueLookup.Remove(instanceID);
	}

	private void InternalUnRegisterTextObjectForUpdate(TMP_Text textObject)
	{
		int instanceID = textObject.GetInstanceID();
		m_InternalUpdateQueue.Remove(textObject);
		m_InternalUpdateLookup.Remove(instanceID);
	}
}
