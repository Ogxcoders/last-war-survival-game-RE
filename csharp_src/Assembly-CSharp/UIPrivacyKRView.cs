using System.Collections.Generic;
using System.Text;
using AIHelp;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPrivacyKRView : UIPrivacyBaseView
{
	private static UIPrivacyKRView _instance;

	private bool agree1;

	private bool agree2;

	private bool agree3;

	private GameObject go;

	private TextMeshProUGUIEx content;

	private Button agree_btn1;

	private GameObject agree_btn1_be_select;

	private TextMeshProUGUIEx agree_tip1;

	private Button agree_btn2;

	private GameObject agree_btn2_be_select;

	private TextMeshProUGUIEx agree_tip2;

	private Button agree_btn3;

	private GameObject agree_btn3_be_select;

	private TextMeshProUGUIEx agree_tip3;

	private Button btn_agree_all;

	private Button btn_enter_game;

	public static UIPrivacyKRView Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new UIPrivacyKRView();
			}
			return _instance;
		}
	}

	public override string GetLoadAssetPath()
	{
		return "Assets/Main/Prefabs/UI/LWUIPrivacy/UIPrivacyKR.prefab";
	}

	public override void OnCreate(GameObject go)
	{
		this.go = go;
		ComponentDefine();
		ReInit();
		RefreshView();
	}

	public override void OnDestroy()
	{
		ComponentDestroy();
		_instance = null;
	}

	private void ComponentDefine()
	{
		content = go.transform.Find("Root/ScrollView/Viewport/Content").GetComponent<TextMeshProUGUIEx>();
		agree_btn1 = go.transform.Find("Root/agreeBtn1").GetComponent<Button>();
		agree_btn1_be_select = go.transform.Find("Root/agreeBtn1/agreeBtn1BeSelect").gameObject;
		agree_tip1 = go.transform.Find("Root/agreeBtn1/tipContent/agreeTip1").GetComponent<TextMeshProUGUIEx>();
		agree_btn2 = go.transform.Find("Root/agreeBtn2").GetComponent<Button>();
		agree_btn2_be_select = go.transform.Find("Root/agreeBtn2/agreeBtn2BeSelect").gameObject;
		agree_tip2 = go.transform.Find("Root/agreeBtn2/tipContent/agreeTip2").GetComponent<TextMeshProUGUIEx>();
		agree_btn3 = go.transform.Find("Root/agreeBtn3").GetComponent<Button>();
		agree_btn3_be_select = go.transform.Find("Root/agreeBtn3/agreeBtn3BeSelect").gameObject;
		agree_tip3 = go.transform.Find("Root/agreeBtn3/tipContent/agreeTip3").GetComponent<TextMeshProUGUIEx>();
		btn_agree_all = go.transform.Find("Root/BtnAgreeAll").GetComponent<Button>();
		btn_enter_game = go.transform.Find("Root/BtnEnterGame").GetComponent<Button>();
		content.onPointerClick = delegate(PointerEventData eventData)
		{
			OnContentPointerClick(eventData, content);
		};
		agree_tip1.onPointerClick = delegate(PointerEventData eventData)
		{
			OnPointerClick(eventData, agree_tip1);
		};
		agree_tip2.onPointerClick = delegate(PointerEventData eventData)
		{
			OnPointerClick(eventData, agree_tip2);
		};
		agree_tip3.onPointerClick = delegate(PointerEventData eventData)
		{
			OnPointerClick(eventData, agree_tip3);
		};
		agree_btn1.onClick.AddListener(delegate
		{
			OnAgreeClick(1);
		});
		agree_btn2.onClick.AddListener(delegate
		{
			OnAgreeClick(2);
		});
		agree_btn3.onClick.AddListener(delegate
		{
			OnAgreeClick(3);
		});
		btn_agree_all.onClick.AddListener(delegate
		{
			OnAgreeAllClick();
		});
		btn_enter_game.onClick.AddListener(delegate
		{
			OnConfirmClick();
		});
	}

	private void ComponentDestroy()
	{
		content.onPointerClick = null;
		agree_tip1.onPointerClick = null;
		agree_tip2.onPointerClick = null;
		agree_tip3.onPointerClick = null;
		agree_btn1.onClick.RemoveAllListeners();
		agree_btn2.onClick.RemoveAllListeners();
		agree_btn3.onClick.RemoveAllListeners();
		btn_agree_all.onClick.RemoveAllListeners();
		btn_enter_game.onClick.RemoveAllListeners();
		content = null;
		agree_btn1 = null;
		agree_btn1_be_select = null;
		agree_tip1 = null;
		agree_btn2 = null;
		agree_btn2_be_select = null;
		agree_tip2 = null;
		agree_btn3 = null;
		agree_btn3_be_select = null;
		agree_tip3 = null;
		btn_agree_all = null;
		btn_enter_game = null;
	}

	private void ReInit()
	{
		string key = "game_start_option1";
		string key2 = "game_start_option2_new";
		string key3 = "game_start_option3_new";
		if (SDKManager.IS_UNITY_STANDALONE() && !SDKManager.IS_UNITY_EDITOR())
		{
			key2 = "game_start_option2_pc";
			key3 = "game_start_option3_pc";
		}
		agree_tip1.text = GameEntry.Localization.GetString(key);
		agree_tip2.text = GameEntry.Localization.GetString(key2);
		agree_tip3.text = GameEntry.Localization.GetString(key3);
	}

	private void RefreshView()
	{
		agree_btn1_be_select.SetActive(agree1);
		agree_btn2_be_select.SetActive(agree2);
		agree_btn3_be_select.SetActive(agree3);
		if (agree1 && agree2 && agree3)
		{
			UIGray.SetGray(btn_enter_game.transform, bGray: false, canClick: true);
		}
		else
		{
			UIGray.SetGray(btn_enter_game.transform, bGray: true, canClick: true);
		}
	}

	private void OnPointerClick(PointerEventData eventData, TMP_Text _textMeshPro)
	{
		string text = null;
		Vector3 position = eventData.position;
		Camera pressEventCamera = eventData.pressEventCamera;
		int num = TMP_TextUtilities.FindIntersectingLink(_textMeshPro, position, pressEventCamera);
		if (num != -1)
		{
			TMP_LinkInfo tMP_LinkInfo = _textMeshPro.textInfo.linkInfo[num];
			text = tMP_LinkInfo.GetLinkID();
		}
		if (!string.IsNullOrEmpty(text))
		{
			SDKManager.OpenURL(text);
		}
	}

	private void OnContentPointerClick(PointerEventData eventData, TMP_Text _textMeshPro)
	{
		string value = null;
		Vector3 position = eventData.position;
		Camera pressEventCamera = eventData.pressEventCamera;
		int num = TMP_TextUtilities.FindIntersectingLink(_textMeshPro, position, pressEventCamera);
		if (num != -1)
		{
			TMP_LinkInfo tMP_LinkInfo = _textMeshPro.textInfo.linkInfo[num];
			value = tMP_LinkInfo.GetLinkID();
		}
		if (!string.IsNullOrEmpty(value) && "OpenAIHelp".Equals(value))
		{
			AIHelpProxy.Show("E009", GameEntry.Localization.GetString("2700006"));
		}
	}

	private void OnAgreeClick(int agreeId)
	{
		switch (agreeId)
		{
		case 1:
			agree1 = !agree1;
			break;
		case 2:
			agree2 = !agree2;
			break;
		case 3:
			agree3 = !agree3;
			break;
		}
		RefreshView();
	}

	private void OnAgreeAllClick()
	{
		agree1 = true;
		agree2 = true;
		agree3 = true;
		RefreshView();
	}

	private void OnConfirmClick()
	{
		if (agree1 && agree2 && agree3)
		{
			RecordPostEvent();
			GameEntry.Event.Fire(EventId.UIPrivacy_Confirm);
			ClosePrivacyView();
		}
	}

	private void RecordPostEvent()
	{
		List<string> list = new List<string>();
		List<TextMeshProUGUIEx> list2 = new List<TextMeshProUGUIEx> { agree_tip1, agree_tip2, agree_tip3 };
		for (int i = 0; i < list2.Count; i++)
		{
			TextMeshProUGUIEx textMeshProUGUIEx = list2[i];
			textMeshProUGUIEx.ForceMeshUpdate();
			TMP_LinkInfo[] linkInfo = textMeshProUGUIEx.textInfo.linkInfo;
			int num = linkInfo.Length;
			for (int j = 0; j < num; j++)
			{
				TMP_LinkInfo tMP_LinkInfo = linkInfo[j];
				string linkID = tMP_LinkInfo.GetLinkID();
				if (!string.IsNullOrEmpty(linkID))
				{
					list.Add(linkID);
				}
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
