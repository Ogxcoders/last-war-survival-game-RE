using UnityEngine;
using UnityEngine.UI;

public class UIZendeskPrivacy : UIPrivacyBaseView
{
	private static UIZendeskPrivacy _instance;

	private GameObject _rootGameObject;

	private bool inClose;

	private TextMeshProUGUIEx _textTitle;

	private Button _btnBack;

	private Button _btnMessage;

	private TextMeshProUGUIEx _textMessage;

	private GameObject _compContainer;

	public static UIZendeskPrivacy Instance => _instance ?? (_instance = new UIZendeskPrivacy());

	public override string GetLoadAssetPath()
	{
		return "Assets/Main/Prefabs/UI/UIZendesk/UIZendesk.prefab";
	}

	public override void OnCreate(GameObject go)
	{
		_rootGameObject = go;
		ComponentDefine();
		DataDefine();
	}

	public override void OnDestroy()
	{
		DataDestroy();
		ComponentDestroy();
		_instance = null;
	}

	private T FindComponent<T>(string path) where T : Component
	{
		Transform transform = (_rootGameObject ? _rootGameObject.transform.Find(path) : null);
		if (!transform)
		{
			return null;
		}
		return transform.GetComponent<T>();
	}

	private void ComponentDefine()
	{
		_textTitle = FindComponent<TextMeshProUGUIEx>("Root/TopBar/TextTitle");
		_btnBack = FindComponent<Button>("Root/BottomBar/BtnBack");
		_btnMessage = FindComponent<Button>("Root/BottomBar/BtnMessage");
		_textMessage = FindComponent<TextMeshProUGUIEx>("Root/BottomBar/BtnMessage/TextMessage");
		_compContainer = _rootGameObject.transform.Find("Root/Middle/Container").gameObject;
		_rootGameObject.transform.Find("Root/BottomBar/BtnMessage/dot").gameObject.SetActive(value: false);
		if (_btnBack != null)
		{
			_btnBack.onClick.AddListener(OnBtnBackClick);
		}
		if (_btnMessage != null)
		{
			_btnMessage.onClick.AddListener(OnBtnMessageClick);
		}
	}

	private void ComponentDestroy()
	{
		_textTitle = null;
		_btnBack = null;
		_btnMessage = null;
		_textMessage = null;
		_compContainer = null;
	}

	private void DataDefine()
	{
		if (_textTitle != null)
		{
			_textTitle.text = GameEntry.Localization.GetString("gm_title_helpcenter");
		}
		if (_textMessage != null)
		{
			_textMessage.text = GameEntry.Localization.GetString("gm_btn_contactus");
		}
		inClose = false;
		ZendeskSupportView.Show(_compContainer, "https://firstfungroup.zendesk.com", delegate
		{
			CloseSelf();
		});
	}

	private void DataDestroy()
	{
		ZendeskSupportView.Close();
	}

	private void OnBtnBackClick()
	{
		CloseSelf();
	}

	private void OnBtnMessageClick()
	{
		ZendeskSupportView.ShowMessaging();
		CloseSelf();
	}

	private void CloseSelf()
	{
		if (!inClose)
		{
			inClose = true;
			ClosePrivacyView();
		}
	}
}
