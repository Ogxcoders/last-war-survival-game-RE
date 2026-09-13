using System;
using UnityEngine;

namespace EnhancedUI.EnhancedScroller;

public class EnhancedScrollerCellView : MonoBehaviour
{
	public string cellIdentifier;

	[NonSerialized]
	public int cellIndex;

	[NonSerialized]
	public int dataIndex;

	[NonSerialized]
	public bool active;

	public virtual void RefreshCellView()
	{
	}
}
