using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopListExample : MonoBehaviour
{
	public DynamicInfinityListRenderer m_Dl;

	public Button m_BtnSetDatas;

	public Button m_BtnMove2Data;

	public Button m_BtnRemoveData;

	public Button m_BtnAddData;

	private void Start()
	{
		m_Dl.InitRendererList(OnSelectHandler, null);
		m_BtnSetDatas.onClick.AddListener(delegate
		{
			List<int> list = new List<int>();
			for (int i = 0; i < 500; i++)
			{
				list.Add(i);
			}
			m_Dl.SetDataProvider(list);
		});
		m_BtnMove2Data.onClick.AddListener(delegate
		{
			if (m_Dl.GetDataProvider() != null)
			{
				m_Dl.LocateRenderItemAtTarget(24, 1f);
			}
			else
			{
				MonoBehaviour.print("先设置数据吧");
			}
		});
		m_BtnRemoveData.onClick.AddListener(delegate
		{
			if (m_Dl.GetDataProvider() != null)
			{
				if (m_Dl.GetDataProvider().Contains(6))
				{
					m_Dl.GetDataProvider().Remove(6);
					m_Dl.RefreshDataProvider();
				}
				else
				{
					MonoBehaviour.print("找不到数据");
				}
			}
			else
			{
				MonoBehaviour.print("先设置数据吧");
			}
		});
		m_BtnAddData.onClick.AddListener(delegate
		{
			if (m_Dl.GetDataProvider() != null)
			{
				m_Dl.GetDataProvider().Add(999);
				m_Dl.RefreshDataProvider();
			}
			else
			{
				MonoBehaviour.print("先设置数据吧");
			}
		});
	}

	private void OnSelectHandler(DynamicInfinityItem item)
	{
		MonoBehaviour.print("on select " + item.ToString());
	}

	private void Update()
	{
	}
}
