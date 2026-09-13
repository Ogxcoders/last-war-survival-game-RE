using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RefundNoticeController : MonoBehaviour
{
	public enum NoticeTheme
	{
		Dark,
		Light
	}

	public NoticeTheme noticeTheme;

	private TextMeshProUGUIEx _txtNotice;

	private Button _btnOpenRules;

	private void Start()
	{
		if (!GameEntry.Sdk.IsKoreaRegion())
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		_txtNotice = base.transform.Find("Text").GetComponent<TextMeshProUGUIEx>();
		_txtNotice.onPointerClick = OnPrivacyLinkClicked;
		string text = GameEntry.Localization.GetString("refund_policy_short_a");
		string text2 = GameEntry.Localization.GetString("refund_policy_short_b");
		if (noticeTheme == NoticeTheme.Dark)
		{
			_txtNotice.color = new Color(0.553f, 0.545f, 0.545f);
			_txtNotice.text = text + "<link=http://your-refund-policy-url.com><color=#adadad><u>" + text2 + "</u></color></link>";
		}
		else
		{
			_txtNotice.color = new Color(0.918f, 0.902f, 0.925f);
			_txtNotice.text = text + "<link=http://your-refund-policy-url.com><color=#ffffff><u>" + text2 + "</u></color></link>";
		}
	}

	private void OnPrivacyLinkClicked(PointerEventData eventData)
	{
		int num = TMP_TextUtilities.FindIntersectingLink(_txtNotice, eventData.position, eventData.pressEventCamera);
		if (num != -1 && !string.IsNullOrEmpty(_txtNotice.textInfo.linkInfo[num].GetLinkID()))
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.OpenKR_RefundDetailMessage");
		}
	}
}
