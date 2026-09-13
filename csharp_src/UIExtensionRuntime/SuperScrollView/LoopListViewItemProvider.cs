using UnityEngine;

namespace SuperScrollView;

public class LoopListViewItemProvider : MonoBehaviour
{
	protected LoopListView2 mHost;

	public void SetHost(LoopListView2 host)
	{
		mHost = host;
	}

	public virtual bool AddItemPool(string itemPrefabName)
	{
		return false;
	}
}
