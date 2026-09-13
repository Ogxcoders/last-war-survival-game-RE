using UnityEngine;
using UnityEngine.UI;

namespace SuperScrollView;

public class ListItem4 : MonoBehaviour
{
	public Text mMsgText;

	public Image mMsgPic;

	public Image mIcon;

	public Image mItemBg;

	public Image mArrow;

	public Text mIndexText;

	private int mItemIndex = -1;

	public int ItemIndex => mItemIndex;

	public void Init()
	{
	}

	public void SetItemData(ChatMsg itemData, int itemIndex)
	{
		mIndexText.text = $"{itemIndex}.{itemData.mSeqId}";
		PersonInfo personInfo = ChatMsgDataSourceMgr.Get.GetPersonInfo(itemData.mPersonId);
		mItemIndex = itemIndex;
		if (itemData.mMsgType == MsgTypeEnum.Str)
		{
			mMsgPic.gameObject.SetActive(value: false);
			mMsgText.gameObject.SetActive(value: true);
			mMsgText.text = itemData.mSrtMsg;
			mMsgText.GetComponent<ContentSizeFitter>().SetLayoutVertical();
			mIcon.sprite = ResManager.Get.GetSpriteByName(personInfo.mHeadIcon);
			Vector2 sizeDelta = mItemBg.GetComponent<RectTransform>().sizeDelta;
			sizeDelta.x = mMsgText.GetComponent<RectTransform>().sizeDelta.x + 20f;
			sizeDelta.y = mMsgText.GetComponent<RectTransform>().sizeDelta.y + 20f;
			mItemBg.GetComponent<RectTransform>().sizeDelta = sizeDelta;
			if (personInfo.mId == 0)
			{
				mItemBg.color = new Color32(160, 231, 90, byte.MaxValue);
				mArrow.color = mItemBg.color;
			}
			else
			{
				mItemBg.color = Color.white;
				mArrow.color = mItemBg.color;
			}
			RectTransform component = base.gameObject.GetComponent<RectTransform>();
			float num = sizeDelta.y;
			if (num < 75f)
			{
				num = 75f;
			}
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num);
		}
		else
		{
			mMsgPic.gameObject.SetActive(value: true);
			mMsgText.gameObject.SetActive(value: false);
			mMsgPic.sprite = ResManager.Get.GetSpriteByName(itemData.mPicMsgSpriteName);
			mMsgPic.SetNativeSize();
			mIcon.sprite = ResManager.Get.GetSpriteByName(personInfo.mHeadIcon);
			Vector2 sizeDelta2 = mItemBg.GetComponent<RectTransform>().sizeDelta;
			sizeDelta2.x = mMsgPic.GetComponent<RectTransform>().sizeDelta.x + 20f;
			sizeDelta2.y = mMsgPic.GetComponent<RectTransform>().sizeDelta.y + 20f;
			mItemBg.GetComponent<RectTransform>().sizeDelta = sizeDelta2;
			if (personInfo.mId == 0)
			{
				mItemBg.color = new Color32(160, 231, 90, byte.MaxValue);
				mArrow.color = mItemBg.color;
			}
			else
			{
				mItemBg.color = Color.white;
				mArrow.color = mItemBg.color;
			}
			RectTransform component2 = base.gameObject.GetComponent<RectTransform>();
			float num2 = sizeDelta2.y;
			if (num2 < 75f)
			{
				num2 = 75f;
			}
			component2.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
		}
	}
}
