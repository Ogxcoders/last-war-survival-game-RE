using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPrivacyView : UIPrivacyBaseView
{
	private static UIPrivacyView _instance;

	private GameObject go;

	private TextMeshProUGUIEx content_text;

	private Button playBtn;

	private TextMeshProUGUIEx playBtn_text;

	public static UIPrivacyView Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new UIPrivacyView();
			}
			return _instance;
		}
	}

	public override string GetLoadAssetPath()
	{
		return "Assets/Main/Prefabs/UI/LWUIPrivacy/UIPrivacy.prefab";
	}

	public override void OnCreate(GameObject go)
	{
		this.go = go;
		ComponentDefine();
		ReInit();
	}

	public override void OnDestroy()
	{
		ComponentDestroy();
		_instance = null;
	}

	private void ComponentDefine()
	{
		content_text = go.transform.Find("BtnGo/Content").GetComponent<TextMeshProUGUIEx>();
		playBtn = go.transform.Find("BtnGo/PlayBtn").GetComponent<Button>();
		playBtn_text = go.transform.Find("BtnGo/PlayBtn/BtnText").GetComponent<TextMeshProUGUIEx>();
		content_text.onPointerClick = delegate(PointerEventData eventData)
		{
			OnPointerClick(eventData);
		};
		playBtn.onClick.AddListener(delegate
		{
			OnConfirmClick();
		});
	}

	private void ComponentDestroy()
	{
		content_text.onPointerClick = null;
		playBtn.onClick.RemoveAllListeners();
		content_text = null;
		playBtn_text = null;
		playBtn = null;
		go = null;
	}

	private void ReInit()
	{
		string text = "game_start_notice001_new";
		text = ((!PrivacyFuncUtil.Instance.IsPrivacyConfirmed()) ? "game_start_notice001_new" : "game_start_notice002_new");
		content_text.text = GameEntry.Localization.GetString(text);
		playBtn_text.text = GameEntry.Localization.GetString("100833");
	}

	private void OnPointerClick(PointerEventData eventData)
	{
		string text = null;
		Vector3 position = eventData.position;
		Camera pressEventCamera = eventData.pressEventCamera;
		TMP_Text tMP_Text = content_text;
		int num = TMP_TextUtilities.FindIntersectingLink(tMP_Text, position, pressEventCamera);
		if (num != -1)
		{
			TMP_LinkInfo tMP_LinkInfo = tMP_Text.textInfo.linkInfo[num];
			text = tMP_LinkInfo.GetLinkID();
		}
		if (!string.IsNullOrEmpty(text))
		{
			SDKManager.OpenURL(text);
		}
	}

	private void OnConfirmClick()
	{
		RecordPostEvent();
		GameEntry.Event.Fire(EventId.UIPrivacy_Confirm);
		ClosePrivacyView();
	}

	private void RecordPostEvent()
	{
		List<string> list = new List<string>();
		content_text.ForceMeshUpdate();
		TMP_LinkInfo[] linkInfo = content_text.textInfo.linkInfo;
		int num = linkInfo.Length;
		for (int i = 0; i < num; i++)
		{
			TMP_LinkInfo tMP_LinkInfo = linkInfo[i];
			string linkID = tMP_LinkInfo.GetLinkID();
			if (!string.IsNullOrEmpty(linkID))
			{
				list.Add(linkID);
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		if (list.Count > 0)
		{
			stringBuilder.Append('{');
			if (list.Count >= 1)
			{
				stringBuilder.Append("\"s_para1\":\"");
				stringBuilder.Append(list[0]);
				stringBuilder.Append("\"");
			}
			if (list.Count >= 2)
			{
				stringBuilder.Append(",\"s_para2\":\"");
				stringBuilder.Append(list[1]);
				stringBuilder.Append("\"");
			}
			if (list.Count >= 3)
			{
				stringBuilder.Append(",\"s_para3\":\"");
				stringBuilder.Append(list[2]);
				stringBuilder.Append("\"");
			}
			if (list.Count >= 4)
			{
				stringBuilder.Append(",\"s_para4\":\"");
				stringBuilder.Append(list[3]);
				stringBuilder.Append("\"");
			}
			stringBuilder.Append('}');
		}
		PostEventLog.Track("DMA_AGREE_RECORD", stringBuilder.ToString());
	}
}
