using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView;

internal class MenuSceneScript : MonoBehaviour
{
	public Transform mButtonPanelTf;

	private SceneNameInfo[] mSceneNameArray = new SceneNameInfo[20]
	{
		new SceneNameInfo("Chat Message List", "ChatMsgListViewDemo"),
		new SceneNameInfo("Horizontal Gallery", "HorizontalGalleryDemo"),
		new SceneNameInfo("Vertical Gallery", "VerticalGalleryDemo"),
		new SceneNameInfo("GridView", "GridViewDemo"),
		new SceneNameInfo("PageView", "PageViewDemo"),
		new SceneNameInfo("TreeView", "TreeViewDemo"),
		new SceneNameInfo("Spin Date Picker", "SpinDatePickerDemo"),
		new SceneNameInfo("Pull Down To Refresh", "PullAndRefreshDemo"),
		new SceneNameInfo("TreeView\nWith Sticky Head", "TreeViewWithStickyHeadDemo"),
		new SceneNameInfo("Change Item Height", "ChangeItemHeightDemo"),
		new SceneNameInfo("Pull Up To Load More", "PullAndLoadMoreDemo"),
		new SceneNameInfo("Click Load More", "ClickAndLoadMoreDemo"),
		new SceneNameInfo("Select And Delete", "DeleteItemDemo"),
		new SceneNameInfo("Top To Bottom", "TopToBottomDemo"),
		new SceneNameInfo("Left To Right", "LeftToRightDemo"),
		new SceneNameInfo("GridView Select Delete ", "GridViewDeleteItemDemo"),
		new SceneNameInfo("Bottom To Top", "BottomToTopDemo"),
		new SceneNameInfo("Right To Left", "RightToLeftDemo"),
		new SceneNameInfo("Responsive GridView", "ResponsiveGridViewDemo"),
		new SceneNameInfo("TreeView\nWith Children Indent", "TreeViewWithChildrenIndentDemo")
	};

	private void Start()
	{
		CreateFpsDisplyObj();
		int childCount = mButtonPanelTf.childCount;
		for (int i = 0; i < childCount; i++)
		{
			SceneNameInfo sceneNameInfo = mSceneNameArray[i];
			mButtonPanelTf.GetChild(i).GetComponent<Button>().transform.Find("Text").GetComponent<Text>().text = sceneNameInfo.mName;
		}
	}

	private void CreateFpsDisplyObj()
	{
		if (!(Object.FindObjectOfType<FPSDisplay>() != null))
		{
			GameObject obj = new GameObject();
			obj.name = "FPSDisplay";
			obj.AddComponent<FPSDisplay>();
			Object.DontDestroyOnLoad(obj);
		}
	}
}
