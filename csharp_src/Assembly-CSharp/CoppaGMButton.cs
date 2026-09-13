using System;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

public class CoppaGMButton : MonoBehaviour
{
	private GameObject gmPanel;

	private string[] debugStates = new string[3] { "non_us_regions", "us_certified_adult", "us_certified_children" };

	private string[] gmPanelButtonNames = new string[13]
	{
		"1-进入游戏-非美国地区,无需认证", "2-进入游戏-美国地区,认证完,age>13", "3-进入游戏-美国地区,认证完,age<=13", "4-重新生成设备ID-变成新设备了", "5-清理本地uid缓存-变成新玩家", "6-设备国家设置为美国", "7-设备国家设置为非美国", "8-IP国家设置为美国", "9-IP国家设置为非美国", "10-老用户宽松期限设为5分钟",
		"11-ClientSwitch.ENABLE_COPPA_VERIFY开关切换", "12-清除所有GM痕迹；完全真实环境", "13-查看GM信息"
	};

	private void Start()
	{
		GameObject gameObject = new GameObject("GMCanvas");
		gameObject.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
		gameObject.AddComponent<CanvasScaler>();
		gameObject.AddComponent<GraphicRaycaster>();
		GameObject gameObject2 = new GameObject("GMButton");
		gameObject2.transform.SetParent(gameObject.transform);
		RectTransform rectTransform = gameObject2.AddComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(60f, 40f);
		rectTransform.anchorMin = new Vector2(0f, 0.5f);
		rectTransform.anchorMax = new Vector2(0f, 0.5f);
		rectTransform.pivot = new Vector2(0f, 0.5f);
		rectTransform.anchoredPosition = new Vector2(10f, 0f);
		gameObject2.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.5f);
		gameObject2.AddComponent<Button>().onClick.AddListener(OnGMButtonClick);
		GameObject obj = new GameObject("Text");
		obj.transform.SetParent(gameObject2.transform);
		Text text = obj.AddComponent<Text>();
		text.text = "Coppa";
		text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		text.alignment = TextAnchor.MiddleCenter;
		text.color = Color.white;
		RectTransform component = obj.GetComponent<RectTransform>();
		component.sizeDelta = rectTransform.sizeDelta;
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.pivot = new Vector2(0.5f, 0.5f);
		component.anchoredPosition = Vector2.zero;
	}

	private void OnGMButtonClick()
	{
		if (gmPanel != null)
		{
			UnityEngine.Object.Destroy(gmPanel);
			gmPanel = null;
			return;
		}
		gmPanel = new GameObject("GMPanel");
		gmPanel.transform.SetParent(GameObject.Find("GMCanvas").transform);
		RectTransform rectTransform = gmPanel.AddComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(500f, gmPanelButtonNames.Length * 90 + 30);
		rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
		rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
		rectTransform.pivot = new Vector2(0.5f, 0.5f);
		rectTransform.anchoredPosition = Vector2.zero;
		gmPanel.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.8f);
		for (int i = 0; i < gmPanelButtonNames.Length; i++)
		{
			GameObject gameObject = new GameObject("GMPanelBtn" + i);
			gameObject.transform.SetParent(gmPanel.transform);
			RectTransform rectTransform2 = gameObject.AddComponent<RectTransform>();
			rectTransform2.sizeDelta = new Vector2(440f, 80f);
			rectTransform2.anchorMin = new Vector2(0.5f, 1f);
			rectTransform2.anchorMax = new Vector2(0.5f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.anchoredPosition = new Vector2(0f, -20 - i * 90);
			gameObject.AddComponent<Image>().color = Color.white;
			Button button = gameObject.AddComponent<Button>();
			int idx = i;
			button.onClick.AddListener(delegate
			{
				OnPanelButtonClick(idx);
			});
			GameObject obj = new GameObject("Text");
			obj.transform.SetParent(gameObject.transform);
			Text text = obj.AddComponent<Text>();
			text.text = gmPanelButtonNames[i];
			text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text.fontSize = 24;
			text.fontStyle = FontStyle.Bold;
			text.alignment = TextAnchor.MiddleCenter;
			text.color = Color.black;
			RectTransform component = obj.GetComponent<RectTransform>();
			component.sizeDelta = rectTransform2.sizeDelta;
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.pivot = new Vector2(0.5f, 0.5f);
			component.anchoredPosition = Vector2.zero;
			component.offsetMax = Vector2.zero;
			component.offsetMin = Vector2.zero;
		}
		GameObject gameObject2 = new GameObject("CloseButton");
		gameObject2.transform.SetParent(gmPanel.transform);
		RectTransform rectTransform3 = gameObject2.AddComponent<RectTransform>();
		rectTransform3.sizeDelta = new Vector2(60f, 60f);
		rectTransform3.anchorMin = new Vector2(1f, 1f);
		rectTransform3.anchorMax = new Vector2(1f, 1f);
		rectTransform3.pivot = new Vector2(1f, 1f);
		rectTransform3.anchoredPosition = new Vector2(-10f, -10f);
		gameObject2.AddComponent<Image>().color = new Color(1f, 0f, 0f, 0.8f);
		gameObject2.AddComponent<Button>().onClick.AddListener(delegate
		{
			UnityEngine.Object.Destroy(gmPanel);
			gmPanel = null;
		});
		GameObject obj2 = new GameObject("Text");
		obj2.transform.SetParent(gameObject2.transform);
		Text text2 = obj2.AddComponent<Text>();
		text2.text = "X";
		text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		text2.alignment = TextAnchor.MiddleCenter;
		text2.color = Color.white;
		text2.fontSize = 36;
		RectTransform component2 = obj2.GetComponent<RectTransform>();
		component2.sizeDelta = rectTransform3.sizeDelta;
		component2.anchorMin = Vector2.zero;
		component2.anchorMax = Vector2.one;
		component2.pivot = new Vector2(0.5f, 0.5f);
		component2.anchoredPosition = Vector2.zero;
	}

	private void OnPanelButtonClick(int idx)
	{
		int num = idx + 1;
		Debug.Log($"点击了第{idx + 1}个GM面板按钮");
		if (num <= 3)
		{
			UIPrivacyCoppaView.Instance.CompleteVerification(debugStates[idx]);
			if (gmPanel != null)
			{
				UnityEngine.Object.Destroy(gmPanel);
				gmPanel = null;
			}
		}
		if (num == 4)
		{
			ReGenDeviceUid();
			Debug.Log("设备ID已重新生成，重新进入游戏后将触发COPPA认证流程。");
		}
		if (num == 5)
		{
			Log.Info("[AT]ClearGUID_CoppaGM");
			PlayerPrefs.DeleteKey("Setting.GAME_UID");
			PlayerPrefs.DeleteKey("SERVER_ZONE");
			Debug.Log(" 本地UID缓存已清理。请重启游戏以生效。");
		}
		if (num == 6)
		{
			GameEntry.Setting.SetString("PRIVACY_COPPA_GM_COUNTRY", "US");
			Debug.Log("国家已强制设置为美国。");
		}
		if (num == 7)
		{
			GameEntry.Setting.SetString("PRIVACY_COPPA_GM_COUNTRY", "PT");
			Debug.Log("国家已强制设置为非美国。");
		}
		if (num == 8)
		{
			GameEntry.Setting.SetString("PRIVACY_COPPA_GM_IPCOUNTRY", "US");
			Debug.Log("IP已强制设置为美国。");
		}
		if (num == 9)
		{
			GameEntry.Setting.SetString("PRIVACY_COPPA_GM_IPCOUNTRY", "PT");
			Debug.Log("IP已强制设置为非美国。");
		}
		if (num == 10)
		{
			GameEntry.Setting.SetInt("PRIVACY_COPPA_GM_DELAYDAY", 1);
			Debug.Log("老用户宽松期限已设置为5分钟。");
		}
		if (num == 11)
		{
			int num2 = ((GameEntry.Setting.GetInt($"CLIENT_SWITCH_CACHE_ON_{14}", 0) == 0) ? 1 : 0);
			GameEntry.Setting.SetInt($"CLIENT_SWITCH_CACHE_ON_{14}", num2);
			Debug.Log($"ClientSwitch.ENABLE_COPPA_VERIFY开关已切换为{num2}。请重启游戏以生效。");
		}
		if (num == 12)
		{
			PlayerPrefs.DeleteKey("PRIVACY_COPPA_GM_COUNTRY");
			PlayerPrefs.DeleteKey("PRIVACY_COPPA_GM_IPCOUNTRY");
			PlayerPrefs.DeleteKey("PRIVACY_COPPA_ACCOUNT_INFO");
			PlayerPrefs.DeleteKey("PRIVACY_COPPA_CURRENT_STATE");
			PlayerPrefs.DeleteKey("PRIVACY_COPPA_NEW_PLAYER");
			PlayerPrefs.DeleteKey("PRIVACY_COPPA_GM_DELAYDAY");
			PlayerPrefs.DeleteKey($"CLIENT_SWITCH_CACHE_ON_{14}");
			Debug.Log("所有GM痕迹已清除。请重启游戏以生效。");
		}
		if (num == 13)
		{
			string text = GameEntry.Setting.GetString("Setting.GAME_UID", "");
			string text2 = ((GameEntry.Setting.GetInt("PRIVACY_COPPA_GM_DELAYDAY", 0) == 1) ? "5分钟" : "90天");
			string text3 = "当前GM状态: " + GameEntry.Setting.GetString("PRIVACY_COPPA_CURRENT_STATE", "未设置") + "\nGM国家: " + GameEntry.Setting.GetString("PRIVACY_COPPA_GM_COUNTRY", "未设置") + "\nGM_IP国家: " + GameEntry.Setting.GetString("PRIVACY_COPPA_GM_IPCOUNTRY", "未设置") + "\n" + $"年龄: {PrivacyFuncUtil.Instance.GetCoppaAge()}\n" + "邮箱: " + PrivacyFuncUtil.Instance.GetCoppaEmail() + "\n延时标志: " + text2 + " \n" + string.Format("新玩家标志: {0}\n", GameEntry.Setting.GetBool("PRIVACY_COPPA_NEW_PLAYER", defaultValue: false)) + "uid:  " + text + " \n \n" + $"IsGrayDevice: {GrayUtils.IsGrayDevice()} \n" + $"ClientSwitch.ENABLE_COPPA_VERIFY: {GameEntry.Setting.GetInt($"CLIENT_SWITCH_CACHE_ON_{14}", 0)}\n" + string.Format("账号kids: {0}  (0未验证;1成年;2小孩) \n", PlayerPrefs.GetInt("PRIVACY_COPPA_ACCOUNT_STATE", 0)) + "GetCountry: " + PrivacyFuncUtil.Instance.GetCountry() + " \n设备国家: " + PrivacyFuncUtil.Instance.Country + " \nIP国家: " + PrivacyFuncUtil.Instance.IPCountry + " \n设备号: " + PrivacyFuncUtil.Instance.GetAirKey() + "\n \n账号信息: " + GameEntry.Setting.GetString("PRIVACY_COPPA_ACCOUNT_INFO", "未设置");
			Debug.Log(text3);
			GUIUtility.systemCopyBuffer = text3;
			GameObject infoPanel = new GameObject("InfoPanel");
			infoPanel.transform.SetParent(GameObject.Find("GMCanvas").transform);
			RectTransform rectTransform = infoPanel.AddComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(500f, 760f);
			rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = Vector2.zero;
			infoPanel.AddComponent<Image>().color = new Color(0.32f, 0.32f, 0.32f, 1f);
			GameObject obj = new GameObject("InfoText");
			obj.transform.SetParent(infoPanel.transform);
			Text text4 = obj.AddComponent<Text>();
			text4.text = text3;
			text4.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text4.alignment = TextAnchor.MiddleLeft;
			text4.color = Color.white;
			text4.fontSize = 22;
			RectTransform component = obj.GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(380f, 280f);
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.pivot = new Vector2(0.5f, 0.5f);
			component.anchoredPosition = Vector2.zero;
			component.offsetMax = Vector2.zero;
			component.offsetMin = Vector2.zero;
			GameObject gameObject = new GameObject("CloseInfoButton");
			gameObject.transform.SetParent(infoPanel.transform);
			RectTransform rectTransform2 = gameObject.AddComponent<RectTransform>();
			rectTransform2.sizeDelta = new Vector2(60f, 60f);
			rectTransform2.anchorMin = new Vector2(1f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(1f, 1f);
			rectTransform2.anchoredPosition = new Vector2(-10f, -10f);
			gameObject.AddComponent<Image>().color = new Color(1f, 0f, 0f, 0.8f);
			gameObject.AddComponent<Button>().onClick.AddListener(delegate
			{
				UnityEngine.Object.Destroy(infoPanel);
			});
			GameObject obj2 = new GameObject("Text");
			obj2.transform.SetParent(gameObject.transform);
			Text text5 = obj2.AddComponent<Text>();
			text5.text = "X";
			text5.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text5.alignment = TextAnchor.MiddleCenter;
			text5.color = Color.white;
			text5.fontSize = 36;
			RectTransform component2 = obj2.GetComponent<RectTransform>();
			component2.sizeDelta = rectTransform2.sizeDelta;
			component2.anchorMin = Vector2.zero;
			component2.anchorMax = Vector2.one;
			component2.pivot = new Vector2(0.5f, 0.5f);
			component2.anchoredPosition = Vector2.zero;
			component2.offsetMax = Vector2.zero;
			component2.offsetMin = Vector2.zero;
			Debug.Log("已查看GM信息，请查看控制台输出。");
		}
	}

	public void ReGenDeviceUid()
	{
		string text = SystemInfo.deviceUniqueIdentifier + UnityEngine.Random.Range(0, 1000000);
		Debug.Log("ReGenDeviceUid1: " + text);
		text = text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
		text = ((!CommonUtils.IsDebug()) ? (text + "_n3d") : (text + "_3d"));
		Debug.Log("ReGenDeviceUid2: " + text);
		GameEntry.Setting.SetString("DEVICE_ID", text);
	}
}
