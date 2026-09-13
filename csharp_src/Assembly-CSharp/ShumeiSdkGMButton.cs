using UnityEngine;
using UnityEngine.UI;

public class ShumeiSdkGMButton : MonoBehaviour
{
	private GameObject gmPanel;

	private string[] gmPanelButtonNames = new string[3] { "1-正常模式-正常获取数美id", "2-null模式-数美id永远为空", "3-查看GM信息" };

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
		text.text = "数美id";
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
			Object.Destroy(gmPanel);
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
			Object.Destroy(gmPanel);
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
		Debug.LogError($"点击了第{idx + 1}个GM面板按钮");
		if (num == 1)
		{
			Debug.LogError("1-正常模式-正常获取数美id");
			PlayerPrefs.SetInt("ShumeiSdkSendNullId", 0);
		}
		if (num == 2)
		{
			Debug.LogError(" 本地UID缓存已清理。请重启游戏以生效。");
			PlayerPrefs.SetInt("ShumeiSdkSendNullId", 1);
		}
		if (num == 3)
		{
			string text = string.Format("状态: {0}", PlayerPrefs.GetInt("ShumeiSdkSendNullId", 0));
			Debug.Log(text);
			GUIUtility.systemCopyBuffer = text;
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
			Text text2 = obj.AddComponent<Text>();
			text2.text = text;
			text2.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text2.alignment = TextAnchor.MiddleLeft;
			text2.color = Color.white;
			text2.fontSize = 22;
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
				Object.Destroy(infoPanel);
			});
			GameObject obj2 = new GameObject("Text");
			obj2.transform.SetParent(gameObject.transform);
			Text text3 = obj2.AddComponent<Text>();
			text3.text = "X";
			text3.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			text3.alignment = TextAnchor.MiddleCenter;
			text3.color = Color.white;
			text3.fontSize = 36;
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
}
