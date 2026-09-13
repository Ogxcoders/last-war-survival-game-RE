using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Zendesk;

public static class ZendeskSupportView
{
	public enum ViewReturn
	{
		Normal,
		HttpError,
		OtherError
	}

	private static string baseUrl;

	private static WebViewObject webViewObject;

	private static GameObject webViewGameObject;

	private static Action<ViewReturn> whenDone;

	private static RectTransform container;

	private static Canvas canvas;

	private static void UpdateMargins()
	{
		if (webViewObject == null || container == null || canvas == null)
		{
			return;
		}
		Vector3[] array = new Vector3[4];
		container.GetWorldCorners(array);
		Vector3 position = array[1];
		Vector3 position2 = array[3];
		switch (canvas.renderMode)
		{
		case RenderMode.ScreenSpaceCamera:
		case RenderMode.WorldSpace:
		{
			Camera worldCamera = canvas.worldCamera;
			if (worldCamera == null)
			{
				Debug.LogWarning("Canvas' worldCamera is null. The web view may not be positioned correctly.");
				break;
			}
			position = worldCamera.WorldToScreenPoint(position);
			position2 = worldCamera.WorldToScreenPoint(position2);
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		case RenderMode.ScreenSpaceOverlay:
			break;
		}
		int num = Display.main.systemWidth / Screen.width;
		int num2 = Display.main.systemHeight / Screen.height;
		float num3 = position.x * (float)num;
		float num4 = ((float)Screen.height - position.y) * (float)num2;
		float num5 = ((float)Screen.width - position2.x) * (float)num;
		float num6 = position2.y * (float)num2;
		webViewObject.SetMargins((int)num3, (int)num4, (int)num5, (int)num6);
	}

	private static IEnumerator WebviewStart(string startJsCode = "")
	{
		webViewGameObject = new GameObject("WebViewObject");
		webViewObject = webViewGameObject.AddComponent<WebViewObject>();
		string Url = baseUrl;
		Debug.LogWarning("[Webview] Url: " + Url);
		webViewObject.Init(delegate(string msg)
		{
			GameEntry.Event.Fire(EventId.WebViewFireEvent, msg);
			Debug.LogWarning($"[Webview] ----- >>>CallFromJS[{msg}]");
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnError[{msg}]");
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnHttpError[{msg}]");
		}, delegate(string msg)
		{
			if (!string.IsNullOrEmpty(startJsCode))
			{
				webViewObject.EvaluateJS(startJsCode);
			}
			Debug.LogWarning($"[Webview]CallOnLoaded[{msg}]");
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnStarted[{msg}]");
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnHooked[{msg}]");
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnCookies[{msg}]");
		}, delegate(string msg)
		{
			Debug.LogWarning($"[Webview]CallOnQuit[{msg}]");
		});
		while (!webViewObject.IsInitialized())
		{
			yield return null;
		}
		UpdateMargins();
		webViewObject.SetTextZoom(100);
		webViewObject.SetMixedContentMode(0);
		webViewObject.SetVisibility(v: true);
		webViewObject.SetInteractionEnabled(enabled: true);
		if (Url.StartsWith("http"))
		{
			webViewObject.LoadURL(Url.Replace(" ", "%20"));
			yield break;
		}
		string[] array = new string[3] { ".jpg", ".js", ".html" };
		string[] array2 = array;
		foreach (string ext in array2)
		{
			string path = Url.Replace(".html", ext);
			string text = Path.Combine(Application.streamingAssetsPath, path);
			string dst = Path.Combine(Application.temporaryCachePath, path);
			byte[] bytes;
			if (text.Contains("://"))
			{
				UnityWebRequest unityWebRequest = UnityWebRequest.Get(text);
				yield return unityWebRequest.SendWebRequest();
				bytes = unityWebRequest.downloadHandler.data;
			}
			else
			{
				bytes = File.ReadAllBytes(text);
			}
			File.WriteAllBytes(dst, bytes);
			if (ext == ".html")
			{
				webViewObject.LoadURL("file://" + dst.Replace(" ", "%20"));
				break;
			}
		}
	}

	private static void Stop(ViewReturn ret)
	{
		if (webViewObject != null)
		{
			webViewObject.SetVisibility(v: false);
			webViewObject.SetInteractionEnabled(enabled: false);
			UnityEngine.Object.Destroy(webViewGameObject);
			webViewObject = null;
			webViewGameObject = null;
			if (whenDone != null)
			{
				Action<ViewReturn> action = whenDone;
				whenDone = null;
				action(ret);
			}
		}
	}

	public static void ShowMessaging()
	{
		ZendeskInit.unreadMsgCount = 0;
		ZendeskCore.ZendeskMessaging();
	}

	public static void Show(GameObject obj, string url, Action whenDone = null, string startCallJs = "")
	{
		ShowWebView(url, delegate
		{
			if (whenDone != null)
			{
				whenDone();
			}
		}, obj.GetComponent<RectTransform>(), ApplicationLaunch.Instance, startCallJs);
	}

	public static void Close()
	{
		Stop(ViewReturn.Normal);
	}

	public static void ShowWebView(string url, Action<ViewReturn> done, RectTransform con, MonoBehaviour launcher, string startJsCode = "")
	{
		if (webViewObject != null)
		{
			Debug.LogWarning("[Webview] Already showing , close last one first");
			Stop(ViewReturn.OtherError);
		}
		baseUrl = url;
		whenDone = done;
		container = con;
		canvas = container.GetComponentInParent<Canvas>();
		launcher.StartCoroutine(WebviewStart(startJsCode));
	}
}
