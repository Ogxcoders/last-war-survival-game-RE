using System;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class ExposedMethodField : InspectorField
{
	[SerializeField]
	private Button invokeButton;

	protected ExposedMethod boundMethod;

	public override bool SupportsType(Type type)
	{
		return type == typeof(ExposedMethod);
	}

	public override void Initialize()
	{
		base.Initialize();
		invokeButton.onClick.AddListener(InvokeMethod);
	}

	protected override void OnSkinChanged()
	{
		base.OnSkinChanged();
		invokeButton.SetSkinButton(base.Skin);
	}

	protected override void OnDepthChanged()
	{
		((RectTransform)invokeButton.transform).sizeDelta = new Vector2(-base.Skin.IndentAmount * base.Depth, 0f);
	}

	public void SetBoundMethod(ExposedMethod boundMethod)
	{
		this.boundMethod = boundMethod;
		base.NameRaw = boundMethod.Label;
	}

	public void InvokeMethod()
	{
		Refresh();
		if (boundMethod.IsInitializer)
		{
			base.Value = boundMethod.CallAndReturnValue(base.Value);
		}
		else
		{
			boundMethod.Call(base.Value);
		}
	}
}
