using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class WebViewObject : MonoBehaviour
{
	private Action<string> onJS;

	private Action<string> onError;

	private Action<string> onHttpError;

	private Action<string> onStarted;

	private Action<string> onLoaded;

	private Action<string> onHooked;

	private Action<string> onCookies;

	private Action<string> onQuit;

	private Action<string> onUpdate;

	private bool paused;

	private bool visibility;

	private bool alertDialogEnabled;

	private bool scrollBounceEnabled;

	private int mMarginLeft;

	private int mMarginTop;

	private int mMarginRight;

	private int mMarginBottom;

	private bool mMarginRelative;

	private float mMarginLeftComputed;

	private float mMarginTopComputed;

	private float mMarginRightComputed;

	private float mMarginBottomComputed;

	private bool mMarginRelativeComputed;

	private AndroidJavaObject webView;

	private bool mVisibility;

	private int mKeyboardVisibleHeight;

	private float mResumedTimestamp;

	private int mLastScreenHeight;

	private int mRequestPermissionPhase;

	public bool IsKeyboardVisible => mKeyboardVisibleHeight > 0;

	private void OnApplicationPause(bool paused)
	{
		this.paused = paused;
		if (webView != null)
		{
			webView.Call("OnApplicationPause", paused);
		}
	}

	private void Update()
	{
		if (paused || webView == null)
		{
			return;
		}
		if (mResumedTimestamp != 0f && Time.realtimeSinceStartup - mResumedTimestamp > 0.5f)
		{
			mResumedTimestamp = 0f;
			webView.Call("SetVisibility", mVisibility);
		}
		if (Screen.height != mLastScreenHeight)
		{
			mLastScreenHeight = Screen.height;
			webView.Call("EvaluateJS", "(function() {var e = document.activeElement; if (e != null && e.tagName.toLowerCase() != 'body') {e.blur(); e.focus();}})()");
		}
		while (webView != null)
		{
			string text = webView.Call<string>("GetMessage", Array.Empty<object>());
			if (text == null)
			{
				break;
			}
			int num = text.IndexOf(':', 0);
			if (num != -1)
			{
				switch (text.Substring(0, num))
				{
				case "CallFromJS":
					CallFromJS(text.Substring(num + 1));
					break;
				case "CallOnError":
					CallOnError(text.Substring(num + 1));
					break;
				case "CallOnHttpError":
					CallOnHttpError(text.Substring(num + 1));
					break;
				case "CallOnLoaded":
					CallOnLoaded(text.Substring(num + 1));
					break;
				case "CallOnStarted":
					CallOnStarted(text.Substring(num + 1));
					break;
				case "CallOnHooked":
					CallOnHooked(text.Substring(num + 1));
					break;
				case "CallOnCookies":
					CallOnCookies(text.Substring(num + 1));
					break;
				case "SetKeyboardVisible":
					SetKeyboardVisible(text.Substring(num + 1));
					break;
				case "RequestFileChooserPermissions":
					RequestFileChooserPermissions();
					break;
				}
			}
		}
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (CanGoBack())
			{
				GoBack();
			}
			else if (onQuit != null)
			{
				onQuit("goBack");
			}
		}
		if (onUpdate != null)
		{
			onUpdate("");
		}
	}

	public void SetKeyboardVisible(string keyboardVisibleHeight)
	{
		if (!BottomAdjustmentDisabled())
		{
			int num = mKeyboardVisibleHeight;
			int num2 = int.Parse(keyboardVisibleHeight);
			if (num != num2)
			{
				mKeyboardVisibleHeight = num2;
				SetMargins(mMarginLeft, mMarginTop, mMarginRight, mMarginBottom, mMarginRelative);
			}
		}
	}

	public void RequestFileChooserPermissions()
	{
		List<string> list = new List<string>();
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build$VERSION"))
		{
			if (androidJavaClass.GetStatic<int>("SDK_INT") >= 33)
			{
				if (!Permission.HasUserAuthorizedPermission("android.permission.READ_MEDIA_IMAGES"))
				{
					list.Add("android.permission.READ_MEDIA_IMAGES");
				}
				if (!Permission.HasUserAuthorizedPermission("android.permission.READ_MEDIA_VIDEO"))
				{
					list.Add("android.permission.READ_MEDIA_VIDEO");
				}
				if (!Permission.HasUserAuthorizedPermission("android.permission.READ_MEDIA_AUDIO"))
				{
					list.Add("android.permission.READ_MEDIA_AUDIO");
				}
			}
			else
			{
				if (!Permission.HasUserAuthorizedPermission("android.permission.READ_EXTERNAL_STORAGE"))
				{
					list.Add("android.permission.READ_EXTERNAL_STORAGE");
				}
				if (!Permission.HasUserAuthorizedPermission("android.permission.WRITE_EXTERNAL_STORAGE"))
				{
					list.Add("android.permission.WRITE_EXTERNAL_STORAGE");
				}
			}
		}
		if (!Permission.HasUserAuthorizedPermission("android.permission.CAMERA"))
		{
			list.Add("android.permission.CAMERA");
		}
		if (list.Count > 0)
		{
			StartCoroutine(RequestFileChooserPermissionsCoroutine(list.ToArray()));
		}
		else
		{
			StartCoroutine(CallOnRequestFileChooserPermissionsResult(granted: true));
		}
	}

	private IEnumerator RequestFileChooserPermissionsCoroutine(string[] permissions)
	{
		foreach (string permission in permissions)
		{
			mRequestPermissionPhase = 0;
			Permission.RequestUserPermission(permission);
			for (int j = 0; j < 8; j++)
			{
				if (mRequestPermissionPhase != 0)
				{
					break;
				}
				yield return new WaitForSeconds(0.25f);
			}
			if (mRequestPermissionPhase != 0)
			{
				while (mRequestPermissionPhase == 1)
				{
					yield return new WaitForSeconds(0.3f);
				}
			}
		}
		yield return new WaitForSeconds(0.3f);
		int num = 0;
		for (int k = 0; k < permissions.Length; k++)
		{
			if (Permission.HasUserAuthorizedPermission(permissions[k]))
			{
				num++;
			}
		}
		StartCoroutine(CallOnRequestFileChooserPermissionsResult(num == permissions.Length));
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			if (mRequestPermissionPhase == 1)
			{
				mRequestPermissionPhase = 2;
			}
		}
		else if (mRequestPermissionPhase == 0)
		{
			mRequestPermissionPhase = 1;
		}
	}

	private IEnumerator CallOnRequestFileChooserPermissionsResult(bool granted)
	{
		for (int i = 0; i < 3; i++)
		{
			yield return null;
		}
		webView.Call("OnRequestFileChooserPermissionsResult", granted);
	}

	public int AdjustBottomMargin(int bottom)
	{
		if (BottomAdjustmentDisabled())
		{
			return bottom;
		}
		if (mKeyboardVisibleHeight <= 0)
		{
			return bottom;
		}
		int num = 0;
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			using AndroidJavaObject androidJavaObject2 = androidJavaObject.Get<AndroidJavaObject>("mUnityPlayer");
			using AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getView", Array.Empty<object>());
			using AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("android.graphics.Rect");
			if (androidJavaObject3.Call<bool>("getGlobalVisibleRect", new object[1] { androidJavaObject4 }))
			{
				int num2 = androidJavaObject4.Get<int>("bottom");
				androidJavaObject3.Call("getWindowVisibleDisplayFrame", androidJavaObject4);
				int num3 = androidJavaObject4.Get<int>("bottom");
				num = num2 - num3;
			}
		}
		if (bottom <= num)
		{
			return num;
		}
		return bottom;
	}

	private bool BottomAdjustmentDisabled()
	{
		if (Screen.fullScreen)
		{
			if (Screen.autorotateToLandscapeLeft || Screen.autorotateToLandscapeRight)
			{
				if (!Screen.autorotateToPortrait)
				{
					return Screen.autorotateToPortraitUpsideDown;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	private void Awake()
	{
		alertDialogEnabled = true;
		scrollBounceEnabled = true;
		mMarginLeftComputed = -9999f;
		mMarginTopComputed = -9999f;
		mMarginRightComputed = -9999f;
		mMarginBottomComputed = -9999f;
	}

	public static bool IsWebViewAvailable()
	{
		using AndroidJavaObject androidJavaObject = new AndroidJavaObject("net.gree.unitywebview.CWebViewPlugin");
		return androidJavaObject.CallStatic<bool>("IsWebViewAvailable", Array.Empty<object>());
	}

	public bool IsInitialized()
	{
		if (webView == null)
		{
			return false;
		}
		return webView.Call<bool>("IsInitialized", Array.Empty<object>());
	}

	public void Init(Action<string> cb = null, Action<string> err = null, Action<string> httpErr = null, Action<string> ld = null, Action<string> started = null, Action<string> hooked = null, Action<string> cookies = null, Action<string> quit = null, Action<string> update = null, bool transparent = false, bool zoom = true, string ua = "", int radius = 0, int androidForceDarkMode = 0, bool enableWKWebView = true, int wkContentMode = 0, bool wkAllowsLinkPreview = true, bool wkAllowsBackForwardNavigationGestures = true, bool separated = false)
	{
		onJS = cb;
		onError = err;
		onHttpError = httpErr;
		onStarted = started;
		onLoaded = ld;
		onHooked = hooked;
		onCookies = cookies;
		onQuit = quit;
		onUpdate = update;
		webView = new AndroidJavaObject("net.gree.unitywebview.CWebViewPlugin");
		webView.Call("Init", base.name, transparent, zoom, androidForceDarkMode, ua, radius);
	}

	protected virtual void OnDestroy()
	{
		if (webView != null)
		{
			webView.Call("Destroy");
			webView.Dispose();
			webView = null;
		}
	}

	public void Pause()
	{
		if (webView != null)
		{
			webView.Call("Pause");
		}
	}

	public void Resume()
	{
		if (webView != null)
		{
			webView.Call("Resume");
		}
	}

	public void SetCenterPositionWithScale(Vector2 center, Vector2 scale)
	{
		float num = ((float)Screen.width - scale.x) / 2f + center.x;
		float num2 = (float)Screen.width - (num + scale.x);
		float num3 = ((float)Screen.height - scale.y) / 2f + center.y;
		float num4 = (float)Screen.height - (num3 + scale.y);
		SetMargins((int)num, (int)num4, (int)num2, (int)num3);
	}

	public void SetMargins(int left, int top, int right, int bottom, bool relative = false)
	{
		if (webView == null)
		{
			return;
		}
		mMarginLeft = left;
		mMarginTop = top;
		mMarginRight = right;
		mMarginBottom = bottom;
		mMarginRelative = relative;
		float num5;
		float num6;
		float num7;
		float num8;
		if (relative)
		{
			float num = Screen.width;
			float num2 = Screen.height;
			int num3 = Display.main.systemWidth;
			int num4 = Display.main.systemHeight;
			if (!Screen.fullScreen)
			{
				using AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				using AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				using AndroidJavaObject androidJavaObject2 = androidJavaObject.Get<AndroidJavaObject>("mUnityPlayer");
				using AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getView", Array.Empty<object>());
				using AndroidJavaObject androidJavaObject4 = new AndroidJavaObject("android.graphics.Rect");
				androidJavaObject3.Call("getDrawingRect", androidJavaObject4);
				num3 = androidJavaObject4.Call<int>("width", Array.Empty<object>());
				num4 = androidJavaObject4.Call<int>("height", Array.Empty<object>());
			}
			num5 = (float)left / num * (float)num3;
			num6 = (float)top / num2 * (float)num4;
			num7 = (float)right / num * (float)num3;
			num8 = AdjustBottomMargin((int)((float)bottom / num2 * (float)num4));
		}
		else
		{
			num5 = left;
			num6 = top;
			num7 = right;
			num8 = AdjustBottomMargin(bottom);
		}
		bool flag = relative;
		if (num5 != mMarginLeftComputed || num6 != mMarginTopComputed || num7 != mMarginRightComputed || num8 != mMarginBottomComputed || flag != mMarginRelativeComputed)
		{
			mMarginLeftComputed = num5;
			mMarginTopComputed = num6;
			mMarginRightComputed = num7;
			mMarginBottomComputed = num8;
			mMarginRelativeComputed = flag;
			webView.Call("SetMargins", (int)num5, (int)num6, (int)num7, (int)num8);
		}
	}

	public void SetVisibility(bool v)
	{
		if (GetVisibility() && !v)
		{
			EvaluateJS("if (document && document.activeElement) document.activeElement.blur();");
		}
		if (webView != null)
		{
			mVisibility = v;
			webView.Call("SetVisibility", v);
			visibility = v;
		}
	}

	public bool GetVisibility()
	{
		return visibility;
	}

	public void SetScrollbarsVisibility(bool v)
	{
		if (webView != null)
		{
			webView.Call("SetScrollbarsVisibility", v);
		}
	}

	public void SetInteractionEnabled(bool enabled)
	{
		if (webView != null)
		{
			webView.Call("SetInteractionEnabled", enabled);
		}
	}

	public void SetAlertDialogEnabled(bool e)
	{
		if (webView != null)
		{
			webView.Call("SetAlertDialogEnabled", e);
			alertDialogEnabled = e;
		}
	}

	public bool GetAlertDialogEnabled()
	{
		return alertDialogEnabled;
	}

	public void SetScrollBounceEnabled(bool e)
	{
		scrollBounceEnabled = e;
	}

	public bool GetScrollBounceEnabled()
	{
		return scrollBounceEnabled;
	}

	public void SetCameraAccess(bool allowed)
	{
		if (webView != null)
		{
			webView.Call("SetCameraAccess", allowed);
		}
	}

	public void SetMicrophoneAccess(bool allowed)
	{
		if (webView != null)
		{
			webView.Call("SetMicrophoneAccess", allowed);
		}
	}

	public bool SetURLPattern(string allowPattern, string denyPattern, string hookPattern)
	{
		if (webView == null)
		{
			return false;
		}
		return webView.Call<bool>("SetURLPattern", new object[3] { allowPattern, denyPattern, hookPattern });
	}

	public void LoadURL(string url)
	{
		if (!string.IsNullOrEmpty(url) && webView != null)
		{
			webView.Call("LoadURL", url);
		}
	}

	public void LoadHTML(string html, string baseUrl)
	{
		if (!string.IsNullOrEmpty(html))
		{
			if (string.IsNullOrEmpty(baseUrl))
			{
				baseUrl = "";
			}
			if (webView != null)
			{
				webView.Call("LoadHTML", html, baseUrl);
			}
		}
	}

	public void EvaluateJS(string js)
	{
		if (webView != null)
		{
			webView.Call("EvaluateJS", js);
		}
	}

	public int Progress()
	{
		if (webView == null)
		{
			return 0;
		}
		return webView.Get<int>("progress");
	}

	public bool CanGoBack()
	{
		if (webView == null)
		{
			return false;
		}
		return webView.Get<bool>("canGoBack");
	}

	public bool CanGoForward()
	{
		if (webView == null)
		{
			return false;
		}
		return webView.Get<bool>("canGoForward");
	}

	public void GoBack()
	{
		if (webView != null)
		{
			webView.Call("GoBack");
		}
	}

	public void GoForward()
	{
		if (webView != null)
		{
			webView.Call("GoForward");
		}
	}

	public void Reload()
	{
		if (webView != null)
		{
			webView.Call("Reload");
		}
	}

	public void CallOnError(string error)
	{
		if (onError != null)
		{
			onError(error);
		}
	}

	public void CallOnHttpError(string error)
	{
		if (onHttpError != null)
		{
			onHttpError(error);
		}
	}

	public void CallOnStarted(string url)
	{
		if (onStarted != null)
		{
			onStarted(url);
		}
	}

	public void CallOnLoaded(string url)
	{
		if (onLoaded != null)
		{
			onLoaded(url);
		}
	}

	public void CallFromJS(string message)
	{
		if (onJS != null)
		{
			onJS(message);
		}
	}

	public void CallOnHooked(string message)
	{
		if (onHooked != null)
		{
			onHooked(message);
		}
	}

	public void CallOnCookies(string cookies)
	{
		if (onCookies != null)
		{
			onCookies(cookies);
		}
	}

	public void AddCustomHeader(string headerKey, string headerValue)
	{
		if (webView != null)
		{
			webView.Call("AddCustomHeader", headerKey, headerValue);
		}
	}

	public string GetCustomHeaderValue(string headerKey)
	{
		if (webView == null)
		{
			return null;
		}
		return webView.Call<string>("GetCustomHeaderValue", new object[1] { headerKey });
	}

	public void RemoveCustomHeader(string headerKey)
	{
		if (webView != null)
		{
			webView.Call("RemoveCustomHeader", headerKey);
		}
	}

	public void ClearCustomHeader()
	{
		if (webView != null)
		{
			webView.Call("ClearCustomHeader");
		}
	}

	public void ClearCookies()
	{
		if (webView != null)
		{
			webView.Call("ClearCookies");
		}
	}

	public void SaveCookies()
	{
		if (webView != null)
		{
			webView.Call("SaveCookies");
		}
	}

	public void GetCookies(string url)
	{
		if (webView != null)
		{
			webView.Call("GetCookies", url);
		}
	}

	public void SetBasicAuthInfo(string userName, string password)
	{
		if (webView != null)
		{
			webView.Call("SetBasicAuthInfo", userName, password);
		}
	}

	public void ClearCache(bool includeDiskFiles)
	{
		if (webView != null)
		{
			webView.Call("ClearCache", includeDiskFiles);
		}
	}

	public void SetTextZoom(int textZoom)
	{
		if (webView != null)
		{
			webView.Call("SetTextZoom", textZoom);
		}
	}

	public void SetMixedContentMode(int mode)
	{
		if (webView != null)
		{
			webView.Call("SetMixedContentMode", mode);
		}
	}
}
