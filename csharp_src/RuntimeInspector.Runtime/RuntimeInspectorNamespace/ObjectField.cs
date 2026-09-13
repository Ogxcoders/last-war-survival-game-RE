using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace RuntimeInspectorNamespace;

public class ObjectField : ExpandableInspectorField
{
	[SerializeField]
	private Button initializeObjectButton;

	private bool elementsInitialized;

	private IRuntimeInspectorCustomEditor customEditor;

	protected override int Length
	{
		get
		{
			if (base.Value.IsNull())
			{
				if (!initializeObjectButton.gameObject.activeSelf)
				{
					return -1;
				}
				return 0;
			}
			if (initializeObjectButton.gameObject.activeSelf)
			{
				return -1;
			}
			if (!elementsInitialized)
			{
				elementsInitialized = true;
				return -1;
			}
			return elements.Count;
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		initializeObjectButton.onClick.AddListener(InitializeObject);
	}

	public override bool SupportsType(Type type)
	{
		return true;
	}

	protected override void OnBound(MemberInfo variable)
	{
		elementsInitialized = false;
		base.OnBound(variable);
	}

	protected override void GenerateElements()
	{
		if (base.Value.IsNull())
		{
			initializeObjectButton.gameObject.SetActive(CanInitializeNewObject());
			return;
		}
		initializeObjectButton.gameObject.SetActive(value: false);
		if ((customEditor = RuntimeInspectorUtils.GetCustomEditor(base.Value.GetType())) != null)
		{
			customEditor.GenerateElements(this);
		}
		else
		{
			CreateDrawersForVariables();
		}
	}

	protected override void ClearElements()
	{
		base.ClearElements();
		if (customEditor != null)
		{
			customEditor.Cleanup();
			customEditor = null;
		}
	}

	protected override void OnSkinChanged()
	{
		base.OnSkinChanged();
		initializeObjectButton.SetSkinButton(base.Skin);
	}

	public override void Refresh()
	{
		base.Refresh();
		if (customEditor != null)
		{
			customEditor.Refresh();
		}
	}

	public void CreateDrawersForVariables(params string[] variables)
	{
		if (variables == null || variables.Length == 0)
		{
			foreach (MemberInfo item in base.Inspector.GetExposedVariablesForType(base.Value.GetType()))
			{
				CreateDrawerForVariable(item);
			}
			return;
		}
		foreach (MemberInfo item2 in base.Inspector.GetExposedVariablesForType(base.Value.GetType()))
		{
			if (Array.IndexOf(variables, item2.Name) >= 0)
			{
				CreateDrawerForVariable(item2);
			}
		}
	}

	public void CreateDrawersForVariablesExcluding(params string[] variablesToExclude)
	{
		if (variablesToExclude == null || variablesToExclude.Length == 0)
		{
			foreach (MemberInfo item in base.Inspector.GetExposedVariablesForType(base.Value.GetType()))
			{
				CreateDrawerForVariable(item);
			}
			return;
		}
		foreach (MemberInfo item2 in base.Inspector.GetExposedVariablesForType(base.Value.GetType()))
		{
			if (Array.IndexOf(variablesToExclude, item2.Name) < 0)
			{
				CreateDrawerForVariable(item2);
			}
		}
	}

	private bool CanInitializeNewObject()
	{
		if (base.BoundVariableType.IsAbstract || base.BoundVariableType.IsInterface)
		{
			return false;
		}
		if (typeof(ScriptableObject).IsAssignableFrom(base.BoundVariableType))
		{
			return true;
		}
		if (typeof(UnityEngine.Object).IsAssignableFrom(base.BoundVariableType))
		{
			return false;
		}
		if (base.BoundVariableType.IsArray)
		{
			return false;
		}
		if (base.BoundVariableType.IsGenericType && base.BoundVariableType.GetGenericTypeDefinition() == typeof(List<>))
		{
			return false;
		}
		return true;
	}

	private void InitializeObject()
	{
		if (CanInitializeNewObject())
		{
			base.Value = base.BoundVariableType.Instantiate();
			RegenerateElements();
			base.IsExpanded = true;
		}
	}
}
