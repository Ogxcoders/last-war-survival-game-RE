using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class NavigationGestureAndroidUtility
{
	private List<Rect> m_DefaultRects = new List<Rect>();

	private bool m_DefaultsFetched;

	private string m_DefaultRectsString = "Fetching...";

	private int m_SDKLevel = -1;

	public bool DefaultsFetched
	{
		get
		{
			return m_DefaultsFetched;
		}
		set
		{
			m_DefaultsFetched = value;
		}
	}

	public string DefaultRectsString
	{
		get
		{
			return m_DefaultRectsString;
		}
		set
		{
			m_DefaultRectsString = value;
		}
	}

	public void ApplyExclusionRects(List<Rect> rectsToApply)
	{
		ApplyExclusionRectsInternal(rectsToApply);
	}

	public List<Rect> FetchSystemRects()
	{
		if (!m_DefaultsFetched)
		{
			FetchSystemRectsInternal();
		}
		return m_DefaultRects;
	}

	private void FetchSystemRectsInternal()
	{
		if (GetSDKLevel() < 29)
		{
			Debug.Log("Gesture exclusion rects not supported on this Android version (API level < 29)");
			m_DefaultRectsString = "Not supported";
			m_DefaultsFetched = true;
			return;
		}
		int height = Screen.height;
		using AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		using AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		if (androidJavaObject == null)
		{
			Debug.LogError("GestureExclusion: Could not get current activity.");
			m_DefaultRectsString = "Could not get current activity";
			m_DefaultsFetched = true;
			return;
		}
		androidJavaObject.Call("runOnUiThread", (AndroidJavaRunnable)delegate
		{
			try
			{
				using AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				using AndroidJavaObject androidJavaObject2 = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity");
				using AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("getWindow", Array.Empty<object>());
				using AndroidJavaObject androidJavaObject4 = androidJavaObject3.Call<AndroidJavaObject>("getDecorView", Array.Empty<object>());
				if (androidJavaObject4 == null)
				{
					Debug.LogError("GestureExclusion: Could not get decor view on UI thread.");
					m_DefaultRectsString = "Could not get decor view on UI thread";
				}
				else
				{
					AndroidJavaObject androidJavaObject5 = androidJavaObject4.Call<AndroidJavaObject>("getSystemGestureExclusionRects", Array.Empty<object>());
					if (androidJavaObject5 != null)
					{
						m_DefaultRects.Clear();
						int num = androidJavaObject5.Call<int>("size", Array.Empty<object>());
						for (int i = 0; i < num; i++)
						{
							using AndroidJavaObject androidJavaObject6 = androidJavaObject5.Call<AndroidJavaObject>("get", new object[1] { i });
							m_DefaultRects.Add(new Rect(androidJavaObject6.Get<int>("left"), height - androidJavaObject6.Get<int>("bottom"), androidJavaObject6.Get<int>("right") - androidJavaObject6.Get<int>("left"), androidJavaObject6.Get<int>("bottom") - androidJavaObject6.Get<int>("top")));
						}
						m_DefaultRectsString = RectsToString(m_DefaultRects);
					}
					else
					{
						m_DefaultRectsString = "None";
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("GestureExclusion: Exception on UI thread while fetching rects: " + ex.Message);
				m_DefaultRectsString = "Exception on UI thread";
			}
			finally
			{
				m_DefaultsFetched = true;
			}
		});
	}

	private void ApplyExclusionRectsInternal(List<Rect> rectsToApply)
	{
		if (rectsToApply == null)
		{
			return;
		}
		int height = Screen.height;
		if (GetSDKLevel() < 29)
		{
			Debug.Log("Gesture exclusion rects not supported on this Android version (API level < 29)");
			return;
		}
		using AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		using AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		if (androidJavaObject == null)
		{
			Debug.LogError("GestureExclusion: Could not get current activity.");
			return;
		}
		AndroidJavaObject javaRectsList = new AndroidJavaObject("java.util.ArrayList");
		IntPtr methodID = AndroidJNIHelper.GetMethodID(javaRectsList.GetRawClass(), "add", "(Ljava/lang/Object;)Z");
		foreach (Rect item in rectsToApply)
		{
			using AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("android.graphics.Rect", (int)item.x, (int)((float)height - item.yMax), (int)item.xMax, (int)((float)height - item.y));
			AndroidJNI.CallBooleanMethod(javaRectsList.GetRawObject(), methodID, new jvalue[1]
			{
				new jvalue
				{
					l = androidJavaObject2.GetRawObject()
				}
			});
		}
		androidJavaObject.Call("runOnUiThread", (AndroidJavaRunnable)delegate
		{
			try
			{
				using AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				using AndroidJavaObject androidJavaObject3 = androidJavaClass2.GetStatic<AndroidJavaObject>("currentActivity");
				using AndroidJavaObject androidJavaObject4 = androidJavaObject3.Call<AndroidJavaObject>("getWindow", Array.Empty<object>());
				using AndroidJavaObject androidJavaObject5 = androidJavaObject4.Call<AndroidJavaObject>("getDecorView", Array.Empty<object>());
				if (androidJavaObject5 == null)
				{
					Debug.LogError("GestureExclusion: Could not get decor view on UI thread.");
				}
				else
				{
					androidJavaObject5.Call("setSystemGestureExclusionRects", javaRectsList);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("GestureExclusion: Exception on UI thread while applying rects: " + ex.Message);
			}
			finally
			{
				if (javaRectsList != null)
				{
					javaRectsList.Dispose();
				}
			}
		});
	}

	private string RectsToString(List<Rect> rects)
	{
		if (rects == null || rects.Count == 0)
		{
			return "None";
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (Rect rect in rects)
		{
			stringBuilder.Append($"R({rect.xMin},{rect.yMin},{rect.width},{rect.height}) ");
		}
		return stringBuilder.ToString();
	}

	public int GetSDKLevel()
	{
		if (m_SDKLevel == -1)
		{
			using AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build$VERSION");
			m_SDKLevel = androidJavaClass.GetStatic<int>("SDK_INT");
		}
		return m_SDKLevel;
	}
}
