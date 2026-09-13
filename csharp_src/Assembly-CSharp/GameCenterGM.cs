using System;
using UnityEngine;
using UnityEngine.UI;

public class GameCenterGM : MonoBehaviour
{
	private GameObject gmPanel;

	private string[] nameSets = new string[6] { "com.fun.lastwar.leaderboard.event.frontline.weekly", "com.fun.lastwar.leaderboard.frontline.chapter.1", "com.fun.lastwar.achievement.build.level.20", "com.fun.lastwar.achievement.frontline.clear.chapter.30", "com.fun.lastwar.achievement.login.days.30", "com.fun.lastwar.achievement.power.100m" };

	private string[] gmPanelButtonNames = new string[14]
	{
		"1-设置名字", "2-设置值", "3-提交排行榜分数", "4-提交成就进度", "5-打开排行榜", "6-打开成就", "7-名字变化", "8-GameCenter登录1-Auto", "9-GameCenter登录2-Bind", "10-teamPlayerID变化",
		"11-清除本地数据", "12-重新生成设备号", "13-日志输出", "14-ReloadGame"
	};

	private InputField nameInputField;

	private GameCenterAuthData authData;

	private string setName = "test.leaderboard.1";

	private string setValue = "100";

	private void Start()
	{
		CleanAuthData();
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
		text.text = "GameCenter";
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
		GameObject gameObject = new GameObject("NameInputField");
		gameObject.transform.SetParent(gmPanel.transform);
		RectTransform rectTransform2 = gameObject.AddComponent<RectTransform>();
		rectTransform2.sizeDelta = new Vector2(200f, 60f);
		rectTransform2.anchorMin = new Vector2(0.5f, 1f);
		rectTransform2.anchorMax = new Vector2(0.5f, 1f);
		rectTransform2.pivot = new Vector2(0.5f, 1f);
		rectTransform2.anchoredPosition = new Vector2(0f, -20 - gmPanelButtonNames.Length * 90);
		gameObject.AddComponent<Image>().color = Color.white;
		nameInputField = gameObject.AddComponent<InputField>();
		GameObject obj = new GameObject("Placeholder");
		obj.transform.SetParent(gameObject.transform);
		Text text = obj.AddComponent<Text>();
		text.text = "输入名字";
		text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		text.fontSize = 24;
		text.fontStyle = FontStyle.Italic;
		text.alignment = TextAnchor.MiddleCenter;
		text.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		RectTransform component = obj.GetComponent<RectTransform>();
		component.sizeDelta = rectTransform2.sizeDelta;
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.pivot = new Vector2(0.5f, 0.5f);
		component.anchoredPosition = Vector2.zero;
		GameObject obj2 = new GameObject("Text");
		obj2.transform.SetParent(gameObject.transform);
		Text text2 = obj2.AddComponent<Text>();
		text2.text = "";
		text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		text2.fontSize = 24;
		text2.alignment = TextAnchor.MiddleCenter;
		text2.color = Color.black;
		RectTransform component2 = obj2.GetComponent<RectTransform>();
		component2.sizeDelta = rectTransform2.sizeDelta;
		component2.anchorMin = Vector2.zero;
		component2.anchorMax = Vector2.one;
		component2.pivot = new Vector2(0.5f, 0.5f);
		component2.anchoredPosition = Vector2.zero;
		nameInputField.textComponent = text2;
		nameInputField.placeholder = text;
		nameInputField.contentType = InputField.ContentType.Standard;
		nameInputField.lineType = InputField.LineType.SingleLine;
		nameInputField.characterLimit = 100;
		nameInputField.text = "test.leaderboard.1";
		for (int i = 0; i < gmPanelButtonNames.Length; i++)
		{
			GameObject gameObject2 = new GameObject("GMPanelBtn" + i);
			gameObject2.transform.SetParent(gmPanel.transform);
			RectTransform rectTransform3 = gameObject2.AddComponent<RectTransform>();
			rectTransform3.sizeDelta = new Vector2(440f, 80f);
			rectTransform3.anchorMin = new Vector2(0.5f, 1f);
			rectTransform3.anchorMax = new Vector2(0.5f, 1f);
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.anchoredPosition = new Vector2(0f, -20 - i * 90);
			gameObject2.AddComponent<Image>().color = Color.white;
			Button button = gameObject2.AddComponent<Button>();
			int idx = i;
			button.onClick.AddListener(delegate
			{
				OnPanelButtonClick(idx);
			});
			GameObject obj3 = new GameObject("Text");
			obj3.transform.SetParent(gameObject2.transform);
			Text text3 = obj3.AddComponent<Text>();
			text3.text = gmPanelButtonNames[i];
			text3.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text3.fontSize = 24;
			text3.fontStyle = FontStyle.Bold;
			text3.alignment = TextAnchor.MiddleCenter;
			text3.color = Color.black;
			RectTransform component3 = obj3.GetComponent<RectTransform>();
			component3.sizeDelta = rectTransform3.sizeDelta;
			component3.anchorMin = Vector2.zero;
			component3.anchorMax = Vector2.one;
			component3.pivot = new Vector2(0.5f, 0.5f);
			component3.anchoredPosition = Vector2.zero;
			component3.offsetMax = Vector2.zero;
			component3.offsetMin = Vector2.zero;
		}
		GameObject gameObject3 = new GameObject("CloseButton");
		gameObject3.transform.SetParent(gmPanel.transform);
		RectTransform rectTransform4 = gameObject3.AddComponent<RectTransform>();
		rectTransform4.sizeDelta = new Vector2(60f, 60f);
		rectTransform4.anchorMin = new Vector2(1f, 1f);
		rectTransform4.anchorMax = new Vector2(1f, 1f);
		rectTransform4.pivot = new Vector2(1f, 1f);
		rectTransform4.anchoredPosition = new Vector2(-10f, -10f);
		gameObject3.AddComponent<Image>().color = new Color(1f, 0f, 0f, 0.8f);
		gameObject3.AddComponent<Button>().onClick.AddListener(delegate
		{
			UnityEngine.Object.Destroy(gmPanel);
			gmPanel = null;
		});
		GameObject obj4 = new GameObject("Text");
		obj4.transform.SetParent(gameObject3.transform);
		Text text4 = obj4.AddComponent<Text>();
		text4.text = "X";
		text4.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
		text4.alignment = TextAnchor.MiddleCenter;
		text4.color = Color.white;
		text4.fontSize = 36;
		RectTransform component4 = obj4.GetComponent<RectTransform>();
		component4.sizeDelta = rectTransform4.sizeDelta;
		component4.anchorMin = Vector2.zero;
		component4.anchorMax = Vector2.one;
		component4.pivot = new Vector2(0.5f, 0.5f);
		component4.anchoredPosition = Vector2.zero;
	}

	private void CleanAuthData()
	{
		if (authData == null)
		{
			authData = new GameCenterAuthData();
		}
		authData.displayName = "";
		authData.playerID = "";
		authData.teamPlayerID = "";
		authData.publicKeyUrl = "";
		authData.salt = "";
		authData.signature = "";
		authData.timestamp = "";
	}

	private void OnPanelButtonClick(int idx)
	{
		int num = idx + 1;
		DebugLog($"点击了第{idx + 1}个GM面板按钮");
		if (num == 1)
		{
			if (nameInputField != null && !string.IsNullOrEmpty(nameInputField.text))
			{
				setName = nameInputField.text;
			}
			else
			{
				setName = "test.leaderboard.1";
			}
			DebugLog("设置名字为: " + setName);
		}
		if (num == 2)
		{
			if (nameInputField != null && !string.IsNullOrEmpty(nameInputField.text))
			{
				setValue = nameInputField.text;
			}
			else
			{
				setValue = "100";
			}
			DebugLog("设置值为: " + setValue);
		}
		if (num == 3)
		{
			GameCenterBridge.SubmitScore(setName, long.Parse(setValue));
		}
		if (num == 4)
		{
			GameCenterBridge.ReportAchievement(setName, double.Parse(setValue));
		}
		if (num == 5)
		{
			GameCenterBridge.ShowLeaderboard(setName);
		}
		if (num == 6)
		{
			GameCenterBridge.ShowAchievements();
		}
		if (num == 7)
		{
			int num2 = Array.IndexOf(nameSets, setName);
			if (num2 < 0)
			{
				setName = nameSets[0];
			}
			else
			{
				num2 = (num2 + 1) % nameSets.Length;
				setName = nameSets[num2];
			}
			if (nameInputField != null)
			{
				nameInputField.text = setName;
			}
			DebugLog("设置名字为: " + setName);
		}
		if (num == 8)
		{
			CleanAuthData();
			GameCenterBridge.AutoLogin();
		}
		if (num == 9)
		{
			CleanAuthData();
			GameCenterBridge.Login(delegate(bool result, GameCenterAuthData auth)
			{
				if (result)
				{
					authData = auth;
					DebugLog("GameCenter登录成功: " + authData.displayName + ", " + authData.playerID);
				}
				else
				{
					DebugLog($"GameCenter登录失败: {result}");
				}
			});
		}
		if (num == 10)
		{
			PlayerPrefs.SetString("gm_teamPlayerID_suffix", UnityEngine.Random.Range(1000, 9999).ToString());
		}
		if (num == 11)
		{
			PlayerPrefs.DeleteKey("GameCenterDeclinedByUser");
			PlayerPrefs.DeleteKey("Setting.GooglePlayID");
			CleanAuthData();
			PlayerPrefs.DeleteKey("gm_teamPlayerID_suffix");
			DebugLog("清除本地数据");
		}
		if (num == 12)
		{
			ReGenDeviceUid();
			DebugLog("重新生成设备号");
		}
		if (num == 13)
		{
			string text = (GUIUtility.systemCopyBuffer = string.Format("开关: {0}\n ", PlayerPrefs.GetInt("SettingKeys_GAMECENTER_ON", 0)) + "设置名字: " + setName + "\n 设置值: " + setValue + "\n 本地teamPlayerID后缀: " + PlayerPrefs.GetString("gm_teamPlayerID_suffix", "") + "\n" + string.Format("GAME_CENTER_DECLINED: {0}\n", PlayerPrefs.GetInt("GameCenterDeclinedByUser", 0)) + "AuthData:\n  displayName: " + authData.displayName + "\n  gamePlayerID: " + authData.playerID + "\n  teamPlayerID: " + authData.teamPlayerID + "\n  publicKeyUrl: " + authData.publicKeyUrl + "\n  salt: " + authData.salt + "\n  signature: " + authData.signature + "\n  timestamp: " + authData.timestamp + "\n");
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
			Text text3 = obj.AddComponent<Text>();
			text3.text = text;
			text3.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text3.alignment = TextAnchor.MiddleLeft;
			text3.color = Color.white;
			text3.fontSize = 22;
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
			Text text4 = obj2.AddComponent<Text>();
			text4.text = "X";
			text4.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text4.alignment = TextAnchor.MiddleCenter;
			text4.color = Color.white;
			text4.fontSize = 36;
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
		if (num == 14)
		{
			ApplicationLaunch.Instance.ReloadGame();
			DebugLog("ReloadGame");
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

	private void DebugLog(string message)
	{
		Debug.LogError(message);
	}
}
