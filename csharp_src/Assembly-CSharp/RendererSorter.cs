using System;
using UnityEngine;

public class RendererSorter : MonoBehaviour
{
	[Serializable]
	public struct SortInfo
	{
		[SerializeField]
		public UnityEngine.Object target;

		[SerializeField]
		public int offset;

		public SortInfo(UnityEngine.Object target, int offset)
		{
			this.target = target;
			this.offset = offset;
		}
	}

	[SerializeField]
	private SortInfo[] m_sortInfos;

	[SerializeField]
	private int m_baseOrder;

	public int BaseOrder
	{
		get
		{
			return m_baseOrder;
		}
		set
		{
			if (m_baseOrder != value)
			{
				m_baseOrder = value;
				Sort();
			}
		}
	}

	private void Sort()
	{
		if (m_sortInfos == null)
		{
			return;
		}
		for (int i = 0; i < m_sortInfos.Length; i++)
		{
			if (m_sortInfos[i].target != null)
			{
				if (m_sortInfos[i].target is TextMeshProEx)
				{
					(m_sortInfos[i].target as TextMeshProEx).sortingOrder = m_baseOrder + m_sortInfos[i].offset;
				}
				else if (m_sortInfos[i].target is Renderer)
				{
					(m_sortInfos[i].target as Renderer).sortingOrder = m_baseOrder + m_sortInfos[i].offset;
				}
				else if (m_sortInfos[i].target is SuperTextMesh)
				{
					(m_sortInfos[i].target as SuperTextMesh).SetOrderInLayer(m_baseOrder + m_sortInfos[i].offset);
				}
			}
		}
	}

	private void Awake()
	{
		Sort();
	}
}
