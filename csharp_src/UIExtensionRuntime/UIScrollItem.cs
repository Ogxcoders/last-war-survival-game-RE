using UnityEngine;
using UnityEngine.UI;

public class UIScrollItem : MonoBehaviour
{
	public Text indexTxt;

	private UIScrollController scroller;

	public int index;

	public int oldIndex = -1;

	public int Index
	{
		get
		{
			return index;
		}
		set
		{
			index = value;
			if (oldIndex == -1)
			{
				oldIndex = index;
			}
			base.transform.localPosition = scroller.GetPosition(index);
			base.gameObject.name = "Scroll" + index;
		}
	}

	public UIScrollController Scroller
	{
		set
		{
			scroller = value;
		}
	}

	private void Awake()
	{
	}

	private void Start()
	{
	}

	public void RefreshItem()
	{
		if ((bool)indexTxt)
		{
			indexTxt.text = index.ToString();
		}
	}
}
