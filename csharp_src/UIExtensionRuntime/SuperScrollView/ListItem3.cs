using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView;

public class ListItem3 : MonoBehaviour
{
	public Text mNameText;

	public Image mIcon;

	public Text mDescText;

	private int mItemIndex = -1;

	public Toggle mToggle;

	public void Init()
	{
		mToggle.onValueChanged.AddListener(OnToggleValueChanged);
	}

	private void OnToggleValueChanged(bool check)
	{
		ItemData itemDataByIndex = DataSourceMgr.Get.GetItemDataByIndex(mItemIndex);
		if (itemDataByIndex != null)
		{
			itemDataByIndex.mChecked = check;
		}
	}

	public void SetItemData(ItemData itemData, int itemIndex)
	{
		mItemIndex = itemIndex;
		mNameText.text = itemData.mName;
		mDescText.text = itemData.mDesc;
		mIcon.sprite = ResManager.Get.GetSpriteByName(itemData.mIcon);
		mToggle.isOn = itemData.mChecked;
	}
}
