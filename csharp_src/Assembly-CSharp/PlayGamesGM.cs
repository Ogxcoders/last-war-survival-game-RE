using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayGamesGM : MonoBehaviour
{
	private GameObject gmPanel;

	private string[] nameSets = new string[9] { "com.fun.lastwar.leaderboard.event.frontline.weekly", "com.fun.lastwar.leaderboard.frontline.chapter.1", "com.fun.lastwar.achievement.build.level.20", "com.fun.lastwar.achievement.frontline.clear.chapter.30", "com.fun.lastwar.achievement.login.days.30", "com.fun.lastwar.achievement.power.100m", "PGS_TestInvoke1", "PGS_TestInvoke2", "PGS_TestInvoke3" };

	private string[] gmPanelButtonNames = new string[14]
	{
		"1-设置名字", "2-设置值", "3-提交排行榜分数", "4-提交成就进度", "5-打开排行榜", "6-打开成就", "7-名字变化", "8-PlayGames登录1-Auto", "9-PlayGames登录2-Bind", "10-随机变化PlayerId的GoogleId",
		"11-调用SetName原生方法", "12-开启debug开关", "13-日志输出", "14-清除所有GM痕迹"
	};

	private InputField nameInputField;

	private PlayGamesAuthData authData;

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
		text.text = "PlayGames";
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
		rectTransform2.sizeDelta = new Vector2(500f, 60f);
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
			authData = new PlayGamesAuthData();
		}
		authData.authCode = "";
		authData.playerId = "";
		authData.displayName = "";
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
			PlayGamesBridge.SubmitScore(setName, long.Parse(setValue));
		}
		if (num == 4)
		{
			PlayGamesBridge.ReportAchievement(setName, double.Parse(setValue));
		}
		if (num == 5)
		{
			PlayGamesBridge.ShowLeaderboard(setName);
		}
		if (num == 6)
		{
			PlayGamesBridge.ShowAchievements();
		}
		if (num == 7)
		{
			CleanAuthData();
			DebugLog("清除本地数据");
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
			PlayGamesBridge.AutoLogin();
		}
		if (num == 9)
		{
			CleanAuthData();
			PlayGamesBridge.Login(delegate(PlayGamesAuthData auth)
			{
				if (auth.success)
				{
					authData = auth;
					DebugLog("GameCenter登录成功: " + authData.displayName + ", " + authData.playerId);
				}
				else
				{
					DebugLog("GameCenter登录失败: " + auth.error);
				}
			});
		}
		if (num == 10)
		{
			string text = Guid.NewGuid().ToString();
			PlayerPrefs.SetString("PlayGamesTestingPlayerId", text);
			DebugLog("设置随机PlayerId的GoogleId: " + text);
		}
		if (num == 11)
		{
			DebugLog("调用SetName原生方法:" + setName + "setValue:" + setValue);
			GameEntry.Sdk.SendDataToNative(setName, setValue);
		}
		if (num == 12)
		{
			DebugLog("Lua测试GetGoogleId");
			PlayerPrefs.SetInt("PGS_DEBUG_ON", 1);
		}
		if (num == 13)
		{
			string text2 = (GUIUtility.systemCopyBuffer = "设置名字: " + setName + "\n 设置值: " + setValue + "\n " + string.Format("测试开关: {0}\n ", PlayerPrefs.GetInt("PGS_DEBUG_ON", 0)) + "测试PlayerId: " + PlayerPrefs.GetString("PlayGamesTestingPlayerId", " null") + "\n AuthData:\n  authCode: " + authData.authCode + "\n  playerId: " + authData.playerId + "\n  displayName: " + authData.displayName + "\n" + $"  success: {authData.success}\n" + $"  code: {authData.code}\n" + "  error: " + authData.error + "\n");
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
			text4.text = text2;
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
			DebugLog("已查看GM信息，请查看控制台输出。");
		}
		if (num == 14)
		{
			CleanAuthData();
			PlayerPrefs.DeleteKey("PlayGamesTestingPlayerId");
			PlayerPrefs.DeleteKey("PGS_DEBUG_ON");
			PlayerPrefs.DeleteKey("LastWarFirstEnterGame");
			DebugLog("已清除所有GM痕迹");
		}
	}

	private void DebugLog(string message)
	{
		Debug.LogError("[GooglePlayManager]" + message);
	}
}
