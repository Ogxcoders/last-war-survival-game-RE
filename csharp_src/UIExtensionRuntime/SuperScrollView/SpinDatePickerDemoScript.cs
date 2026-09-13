using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView;

public class SpinDatePickerDemoScript : MonoBehaviour
{
	public LoopListView2 mLoopListViewMonth;

	public LoopListView2 mLoopListViewDay;

	public LoopListView2 mLoopListViewHour;

	public Button mBackButton;

	private static int[] mMonthDayCountArray = new int[12]
	{
		31, 28, 31, 30, 31, 30, 31, 31, 30, 31,
		30, 31
	};

	private static string[] mMonthNameArray = new string[12]
	{
		"Jan.", "Feb.", "Mar.", "Apr.", "May.", "Jun.", "Jul.", "Aug.", "Sep.", "Oct.",
		"Nov.", "Dec."
	};

	private int mCurSelectedMonth = 2;

	private int mCurSelectedDay = 2;

	private int mCurSelectedHour = 2;

	public int CurSelectedMonth => mCurSelectedMonth;

	public int CurSelectedDay => mCurSelectedDay;

	public int CurSelectedHour => mCurSelectedHour;

	private void Start()
	{
		mLoopListViewMonth.InitListView(-1, OnGetItemByIndexForMonth);
		mLoopListViewDay.InitListView(-1, OnGetItemByIndexForDay);
		mLoopListViewHour.InitListView(-1, OnGetItemByIndexForHour);
		mLoopListViewMonth.mOnSnapItemFinished = OnMonthSnapTargetFinished;
		mLoopListViewMonth.mOnSnapNearestChanged = OnMonthSnapTargetChanged;
		mLoopListViewDay.mOnSnapNearestChanged = OnDaySnapTargetChanged;
		mLoopListViewHour.mOnSnapNearestChanged = OnHourSnapTargetChanged;
		mBackButton.onClick.AddListener(OnBackBtnClicked);
	}

	private void OnBackBtnClicked()
	{
		SceneManager.LoadScene("Menu");
	}

	private LoopListViewItem2 OnGetItemByIndexForHour(LoopListView2 listView, int index)
	{
		LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemPrefab1");
		ListItem7 component = loopListViewItem.GetComponent<ListItem7>();
		if (!loopListViewItem.IsInitHandlerCalled)
		{
			loopListViewItem.IsInitHandlerCalled = true;
			component.Init();
		}
		int num = 1;
		int num2 = 24;
		int num3 = 0;
		num3 = ((index < 0) ? (num2 + (index + 1) % num2 - 1) : (index % num2));
		num3 = (component.Value = num3 + num);
		component.mText.text = num3.ToString();
		return loopListViewItem;
	}

	private LoopListViewItem2 OnGetItemByIndexForMonth(LoopListView2 listView, int index)
	{
		LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemPrefab1");
		ListItem7 component = loopListViewItem.GetComponent<ListItem7>();
		if (!loopListViewItem.IsInitHandlerCalled)
		{
			loopListViewItem.IsInitHandlerCalled = true;
			component.Init();
		}
		int num = 1;
		int num2 = 12;
		int num3 = 0;
		num3 = ((index < 0) ? (num2 + (index + 1) % num2 - 1) : (index % num2));
		num3 = (component.Value = num3 + num);
		component.mText.text = mMonthNameArray[num3 - 1];
		return loopListViewItem;
	}

	private LoopListViewItem2 OnGetItemByIndexForDay(LoopListView2 listView, int index)
	{
		LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemPrefab1");
		ListItem7 component = loopListViewItem.GetComponent<ListItem7>();
		if (!loopListViewItem.IsInitHandlerCalled)
		{
			loopListViewItem.IsInitHandlerCalled = true;
			component.Init();
		}
		int num = 1;
		int num2 = mMonthDayCountArray[mCurSelectedMonth - 1];
		int num3 = 0;
		num3 = ((index < 0) ? (num2 + (index + 1) % num2 - 1) : (index % num2));
		num3 = (component.Value = num3 + num);
		component.mText.text = num3.ToString();
		return loopListViewItem;
	}

	private void OnMonthSnapTargetChanged(LoopListView2 listView, LoopListViewItem2 item)
	{
		int indexInShownItemList = listView.GetIndexInShownItemList(item);
		if (indexInShownItemList >= 0)
		{
			ListItem7 component = item.GetComponent<ListItem7>();
			mCurSelectedMonth = component.Value;
			OnListViewSnapTargetChanged(listView, indexInShownItemList);
		}
	}

	private void OnDaySnapTargetChanged(LoopListView2 listView, LoopListViewItem2 item)
	{
		int indexInShownItemList = listView.GetIndexInShownItemList(item);
		if (indexInShownItemList >= 0)
		{
			ListItem7 component = item.GetComponent<ListItem7>();
			mCurSelectedDay = component.Value;
			OnListViewSnapTargetChanged(listView, indexInShownItemList);
		}
	}

	private void OnHourSnapTargetChanged(LoopListView2 listView, LoopListViewItem2 item)
	{
		int indexInShownItemList = listView.GetIndexInShownItemList(item);
		if (indexInShownItemList >= 0)
		{
			ListItem7 component = item.GetComponent<ListItem7>();
			mCurSelectedHour = component.Value;
			OnListViewSnapTargetChanged(listView, indexInShownItemList);
		}
	}

	private void OnMonthSnapTargetFinished(LoopListView2 listView, LoopListViewItem2 item)
	{
		int firstItemIndex = mLoopListViewDay.GetShownItemByIndex(0).GetComponent<ListItem7>().Value - 1;
		mLoopListViewDay.RefreshAllShownItemWithFirstIndex(firstItemIndex);
	}

	private void OnListViewSnapTargetChanged(LoopListView2 listView, int targetIndex)
	{
		int shownItemCount = listView.ShownItemCount;
		for (int i = 0; i < shownItemCount; i++)
		{
			ListItem7 component = listView.GetShownItemByIndex(i).GetComponent<ListItem7>();
			if (i == targetIndex)
			{
				component.mText.color = Color.red;
			}
			else
			{
				component.mText.color = Color.black;
			}
		}
	}
}
