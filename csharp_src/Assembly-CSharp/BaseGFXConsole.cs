using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseGFXConsole : MonoBehaviour
{
	private bool bShowConsole = true;

	private List<BaseGFXPanel> panelList;

	private string[] panelNameArr;

	private int currPanelIdx;

	private Rect windowRect;

	private Vector2 scrollPosi;

	private float screenWidth;

	private float screenHeight;

	private float resFactor = 1f;

	private float fixedWidth = 330f;

	private bool firstShow = true;

	public static BaseGFXConsole Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
		panelList = new List<BaseGFXPanel>();
		Initialize();
	}

	protected virtual void Initialize()
	{
	}

	protected virtual void OnShowConsole()
	{
	}

	protected virtual void OnHideConsole()
	{
	}

	protected void AddPanel(BaseGFXPanel panel)
	{
		panelList.Add(panel);
		panel.Init();
	}

	private void OnGUI()
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		resFactor = screenHeight / 750f;
		MakeBigGUISkin();
		if (GUI.Button(new Rect(300f, screenHeight / 2f - 200f * resFactor, 150f * resFactor, 80f * resFactor), "渲染控制台"))
		{
			bShowConsole = !bShowConsole;
			if (bShowConsole)
			{
				OnShowConsole();
			}
			else
			{
				OnHideConsole();
			}
		}
		if (GUI.Button(new Rect(300f, screenHeight / 2f - 100f * resFactor, 150f * resFactor, 80f * resFactor), "Login"))
		{
			ApplicationLaunch.Instance.Loading.ReConnect();
		}
		if (GUI.Button(new Rect(300f, screenHeight / 2f + 0f * resFactor, 150f * resFactor, 80f * resFactor), "GC Collect"))
		{
			GameEntry.Lua.Env.FullGc();
			GameEntry.Resource.CollectGarbage();
			GC.Collect();
		}
		if (GUI.Button(new Rect(300f, screenHeight / 2f + 100f * resFactor, 150f * resFactor, 80f * resFactor), "Debug Cache"))
		{
			GameEntry.Resource.DebugOutput();
		}
		if (GUI.Button(new Rect(300f, screenHeight / 2f + 200f * resFactor, 150f * resFactor, 80f * resFactor), "LoadCount"))
		{
			GameEntry.Resource.DebugLoadCount();
		}
		if (bShowConsole)
		{
			float num = fixedWidth * resFactor;
			windowRect = new Rect(screenWidth - num, 0f, num, screenHeight);
			windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "");
			if (firstShow)
			{
				firstShow = false;
				OnShowConsole();
			}
		}
	}

	private void DoMyWindow(int windowID)
	{
		scrollPosi = GUILayout.BeginScrollView(scrollPosi);
		DrawHead();
		GUILayout.Space(30f);
		DrawBody();
		GUILayout.EndScrollView();
	}

	private void DrawHead()
	{
		if (panelNameArr == null)
		{
			InitPanelNameArray();
		}
		int num = currPanelIdx;
		currPanelIdx = GUILayout.SelectionGrid(currPanelIdx, panelNameArr, 3);
		if (num != currPanelIdx)
		{
			panelList[currPanelIdx].Init();
			Debug.Log("变化了" + currPanelIdx);
		}
	}

	private void InitPanelNameArray()
	{
		List<string> list = new List<string>();
		foreach (BaseGFXPanel panel in panelList)
		{
			list.Add(panel.name);
		}
		panelNameArr = list.ToArray();
	}

	private void DrawBody()
	{
		panelList[currPanelIdx].DrawGUI();
	}

	private void MakeBigGUISkin()
	{
		int fontSize = Mathf.CeilToInt(20f * resFactor);
		float fixedHeight = 40f * resFactor;
		GUI.skin.button.fixedHeight = fixedHeight;
		GUI.skin.button.fontSize = fontSize;
		GUI.skin.label.fontSize = fontSize;
		GUI.skin.textField.fontSize = fontSize;
		GUI.skin.textField.fixedHeight = fixedHeight;
		GUI.skin.horizontalSlider.fixedHeight = fixedHeight;
		GUI.skin.horizontalSliderThumb.fixedHeight = fixedHeight;
		GUI.skin.horizontalSliderThumb.fixedWidth = fixedHeight;
		GUI.skin.verticalScrollbar.fixedWidth = fixedHeight;
		GUI.skin.verticalScrollbarThumb.fixedWidth = fixedHeight;
		GUI.skin.verticalScrollbarThumb.fixedHeight = fixedHeight;
		GUI.skin.toggle.fixedHeight = fixedHeight;
		GUI.skin.toggle.fontSize = fontSize;
	}
}
