using System;
using System.Collections.Generic;

public class CustomDataManager
{
	private readonly Dictionary<string, BaseDataContainer> m_Datas = new Dictionary<string, BaseDataContainer>();

	public DCPlayer Player { get; private set; }

	public DCBuilding Building { get; private set; }

	public DCFog Fog { get; private set; }

	public DCRoad Road { get; private set; }

	private T AddData<T>() where T : BaseDataContainer
	{
		string name = typeof(T).Name;
		T val = Activator.CreateInstance(typeof(T)) as T;
		m_Datas[name] = val;
		return val;
	}

	public CustomDataManager()
	{
		Reset();
	}

	public void Release()
	{
		foreach (BaseDataContainer value in m_Datas.Values)
		{
			value.Release();
		}
		m_Datas.Clear();
	}

	public void Reset()
	{
		Player = AddData<DCPlayer>();
		Building = AddData<DCBuilding>();
		Fog = AddData<DCFog>();
		Road = AddData<DCRoad>();
	}
}
