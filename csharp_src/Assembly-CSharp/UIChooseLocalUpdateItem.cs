using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChooseLocalUpdateItem : MonoBehaviour
{
	public Text inputTitle;

	public InputField input;

	public GameObject checkPass;

	public GameObject checkFailed;

	public Button selectButton;

	private UIChooseLocalUpdate _parent;

	private Action<string> _onSetInputValue;

	public List<string> options { get; private set; }

	public void Init(UIChooseLocalUpdate parent, string name, List<string> options, Action<string> onSetInputValue)
	{
		_parent = parent;
		this.options = options;
		inputTitle.text = name;
		_onSetInputValue = onSetInputValue;
		input.onValueChanged.AddListener(OnTableEnvInputValueChange);
		selectButton.onClick.AddListener(OnSelectBtnClick);
	}

	public void SetInputValue(string value)
	{
		input.text = value;
	}

	public bool CheckValue(string value)
	{
		return options.Contains(value);
	}

	private void OnTableEnvInputValueChange(string arg0)
	{
		bool flag = CheckValue(arg0);
		checkPass.SetActive(flag);
		checkFailed.SetActive(!flag);
		_onSetInputValue?.Invoke(arg0);
	}

	private void OnSelectBtnClick()
	{
		_parent.OpenSelectPop(this);
	}
}
