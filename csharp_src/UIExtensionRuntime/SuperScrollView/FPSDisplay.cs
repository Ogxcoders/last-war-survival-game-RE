using UnityEngine;

namespace SuperScrollView;

public class FPSDisplay : MonoBehaviour
{
	private float deltaTime;

	private GUIStyle mStyle;

	private void Awake()
	{
		mStyle = new GUIStyle();
		mStyle.alignment = TextAnchor.UpperLeft;
		mStyle.normal.background = null;
		mStyle.fontSize = 25;
		mStyle.normal.textColor = new Color(0f, 1f, 0f, 1f);
	}

	private void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}
}
