using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SuperScrollView;

public class ChatMsgListViewDemoScript : MonoBehaviour
{
	public LoopListView2 mLoopListView;

	private Button mScrollToButton;

	private InputField mScrollToInput;

	private Button mBackButton;

	private Button mInsertFrontButton;

	private Button mAppendTailButton;

	private Toggle mAutoFillToggle;

	private bool mAutoFill;

	private void Start()
	{
		mLoopListView.SetStopAdjustVelocity(stop: true);
		mLoopListView.SetSmoothDraggingInertia(value: true);
		mLoopListView.InitListView(0, OnGetItemByIndex);
		mScrollToButton = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToButton").GetComponent<Button>();
		mScrollToInput = GameObject.Find("ButtonPanel/buttonGroup2/ScrollToInputField").GetComponent<InputField>();
		mScrollToButton.onClick.AddListener(OnJumpBtnClicked);
		mBackButton = GameObject.Find("ButtonPanel/BackButton").GetComponent<Button>();
		mBackButton.onClick.AddListener(OnBackBtnClicked);
		mInsertFrontButton = GameObject.Find("ButtonPanel/InsertFrontButton").GetComponent<Button>();
		mInsertFrontButton.onClick.AddListener(OnInsertFrontButton);
		mAppendTailButton = GameObject.Find("ButtonPanel/AppendTailButton").GetComponent<Button>();
		mAppendTailButton.onClick.AddListener(OnAppendTailButton);
		mAutoFillToggle = GameObject.Find("ButtonPanel/AutoFillToggle").GetComponent<Toggle>();
		mAutoFillToggle.onValueChanged.AddListener(OnAutoFillToggleValueChanged);
		mLoopListView.SetListItemCount_Mod(ChatMsgDataSourceMgr.Get.TotalItemCount, resetPos: false);
	}

	private void OnBackBtnClicked()
	{
		SceneManager.LoadScene("Menu");
	}

	private void OnInsertFrontButton()
	{
		ChatMsgDataSourceMgr.Get.InsertFront();
		mLoopListView.InsertFront_Mod(ChatMsgDataSourceMgr.Get.TotalItemCount);
	}

	private void OnAppendTailButton()
	{
		ChatMsgDataSourceMgr.Get.AppendTail();
		mLoopListView.AppendTail_Mod(ChatMsgDataSourceMgr.Get.TotalItemCount);
	}

	private void OnAutoFillToggleValueChanged(bool value)
	{
		mAutoFill = value;
	}

	private void OnJumpBtnClicked()
	{
		int result = 0;
		if (int.TryParse(mScrollToInput.text, out result) && result >= 0)
		{
			mLoopListView.MovePanelToItemIndex(result, 0f);
		}
	}

	private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
	{
		if (index < 0 || index >= ChatMsgDataSourceMgr.Get.TotalItemCount)
		{
			return null;
		}
		if (mAutoFill)
		{
			if (index < 10)
			{
				Invoke("OnInsertFrontButton", 0.1f);
			}
			if (ChatMsgDataSourceMgr.Get.TotalItemCount - index < 10)
			{
				Invoke("OnAppendTailButton", 0.1f);
			}
		}
		ChatMsg chatMsgByIndex = ChatMsgDataSourceMgr.Get.GetChatMsgByIndex(index);
		if (chatMsgByIndex == null)
		{
			return null;
		}
		LoopListViewItem2 loopListViewItem = null;
		loopListViewItem = ((chatMsgByIndex.mPersonId != 0) ? listView.NewListViewItem("ItemPrefab2") : listView.NewListViewItem("ItemPrefab1"));
		ListItem4 component = loopListViewItem.GetComponent<ListItem4>();
		if (!loopListViewItem.IsInitHandlerCalled)
		{
			loopListViewItem.IsInitHandlerCalled = true;
			component.Init();
		}
		component.SetItemData(chatMsgByIndex, index);
		return loopListViewItem;
	}
}
