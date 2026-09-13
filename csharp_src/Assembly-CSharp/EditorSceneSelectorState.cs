using System;
using System.Collections;
using UnityEngine;

public class EditorSceneSelectorState : LoadingStateBase
{
	private class UnityUpdater : MonoBehaviour
	{
		public Action OnGUICallback;

		private void OnGUI()
		{
			OnGUICallback?.Invoke();
		}
	}

	private GameObject _gameObject;

	private bool showDropdown;

	private string[] options = new string[4] { "Option 1", "Option 2", "Option 3", "Option 4" };

	private int selectedOptionIndex;

	private float offsetX => Screen.width / 3;

	private float offsetY => Screen.height / 10;

	private float buttonWidth => Screen.width / 3;

	private float buttonHeight => 50f;

	public EditorSceneSelectorState(AppStartupLoading startupLoading)
		: base(startupLoading)
	{
		_startupLoading = startupLoading;
	}

	public override void OnEnter(params object[] args)
	{
		Debug.Log("EditorSceneSelectorState.OnEnter");
		_gameObject = new GameObject("SceneSelector");
		_gameObject.AddComponent<UnityUpdater>().OnGUICallback = OnGUI;
		YieldUtils.StartCoroutine(DelayedAction());
	}

	private IEnumerator DelayedAction()
	{
		yield return new WaitForSecondsRealtime(1f);
		_startupLoading.SetState(LoadingState.None);
		EditorSceneBuilding.Init();
	}

	public override void OnExit()
	{
		_startupLoading.CloseUILoading();
		_startupLoading.ClosePrivacyView();
		UnityEngine.Object.Destroy(_gameObject);
		_gameObject = null;
	}

	public override void OnUpdate()
	{
	}

	public void OnGUI()
	{
		if (GUI.Button(new Rect(offsetX, offsetY, buttonWidth, buttonHeight), options[selectedOptionIndex]))
		{
			showDropdown = !showDropdown;
		}
		if (!showDropdown)
		{
			return;
		}
		for (int i = 0; i < options.Length; i++)
		{
			if (GUI.Button(new Rect(offsetX, offsetY + (float)i * buttonHeight, buttonWidth, buttonHeight), options[i]))
			{
				selectedOptionIndex = i;
				showDropdown = false;
				if (selectedOptionIndex == options.Length - 1)
				{
					_startupLoading.SetState(LoadingState.None);
					EditorSceneBuilding.Init();
				}
			}
		}
	}
}
