using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

namespace RuntimeInspectorNamespace;

public class GameObjectField : ExpandableInspectorField
{
	private string currentTag;

	private Getter isActiveGetter;

	private Getter nameGetter;

	private Getter tagGetter;

	private Setter isActiveSetter;

	private Setter nameSetter;

	private Setter tagSetter;

	private PropertyInfo layerProp;

	private readonly List<Component> components = new List<Component>(8);

	private readonly List<bool> componentsExpandedStates = new List<bool>();

	private Type[] addComponentTypes;

	internal static ExposedMethod addComponentMethod = new ExposedMethod(typeof(GameObjectField).GetMethod("AddComponentButtonClicked", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), new RuntimeInspectorButtonAttribute("Add Component", isInitializer: false, ButtonVisibility.InitializedObjects), isExtensionMethod: false);

	internal static ExposedMethod removeComponentMethod = new ExposedMethod(typeof(GameObjectField).GetMethod("RemoveComponentButtonClicked", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic), new RuntimeInspectorButtonAttribute("Remove Component", isInitializer: false, ButtonVisibility.InitializedObjects), isExtensionMethod: true);

	protected override int Length => components.Count + 4;

	public override void Initialize()
	{
		base.Initialize();
		isActiveGetter = () => ((GameObject)base.Value).activeSelf;
		isActiveSetter = delegate(object value)
		{
			((GameObject)base.Value).SetActive((bool)value);
		};
		nameGetter = () => ((GameObject)base.Value).name;
		nameSetter = delegate(object value)
		{
			((GameObject)base.Value).name = (string)value;
			base.NameRaw = base.Value.GetNameWithType();
			RuntimeHierarchy connectedHierarchy = base.Inspector.ConnectedHierarchy;
			if ((bool)connectedHierarchy)
			{
				connectedHierarchy.RefreshNameOf(((GameObject)base.Value).transform);
			}
		};
		tagGetter = delegate
		{
			GameObject gameObject = (GameObject)base.Value;
			if (!gameObject.CompareTag(currentTag))
			{
				currentTag = gameObject.tag;
			}
			return currentTag;
		};
		tagSetter = delegate(object value)
		{
			((GameObject)base.Value).tag = (string)value;
		};
		layerProp = typeof(GameObject).GetProperty("layer");
	}

	public override bool SupportsType(Type type)
	{
		return type == typeof(GameObject);
	}

	protected override void OnBound(MemberInfo variable)
	{
		base.OnBound(variable);
		currentTag = ((GameObject)base.Value).tag;
	}

	protected override void OnUnbound()
	{
		base.OnUnbound();
		components.Clear();
		componentsExpandedStates.Clear();
	}

	protected override void ClearElements()
	{
		componentsExpandedStates.Clear();
		for (int i = 0; i < elements.Count; i++)
		{
			if (elements[i] is ExpandableInspectorField && (bool)(elements[i].Value as UnityEngine.Object))
			{
				componentsExpandedStates.Add(((ExpandableInspectorField)elements[i]).IsExpanded);
			}
		}
		base.ClearElements();
	}

	protected override void GenerateElements()
	{
		if (components.Count == 0)
		{
			return;
		}
		CreateDrawer(typeof(bool), "Is Active", isActiveGetter, isActiveSetter);
		StringField stringField = CreateDrawer(typeof(string), "Name", nameGetter, nameSetter) as StringField;
		StringField stringField2 = CreateDrawer(typeof(string), "Tag", tagGetter, tagSetter) as StringField;
		CreateDrawerForVariable(layerProp, "Layer");
		int i = 0;
		int num = 0;
		for (; i < components.Count; i++)
		{
			InspectorField inspectorField = CreateDrawerForComponent(components[i]);
			if ((bool)(inspectorField as ExpandableInspectorField) && num < componentsExpandedStates.Count && componentsExpandedStates[num++])
			{
				((ExpandableInspectorField)inspectorField).IsExpanded = true;
			}
		}
		if ((bool)stringField)
		{
			stringField.SetterMode = StringField.Mode.OnSubmit;
		}
		if ((bool)stringField2)
		{
			stringField2.SetterMode = StringField.Mode.OnSubmit;
		}
		if (base.Inspector.ShowAddComponentButton)
		{
			CreateExposedMethodButton(addComponentMethod, () => this, delegate
			{
			});
		}
		componentsExpandedStates.Clear();
	}

	public override void Refresh()
	{
		components.Clear();
		GameObject gameObject = base.Value as GameObject;
		if ((bool)gameObject)
		{
			gameObject.GetComponents(components);
			for (int num = components.Count - 1; num >= 0; num--)
			{
				if (!components[num])
				{
					components.RemoveAt(num);
				}
			}
			if (base.Inspector.ComponentFilter != null)
			{
				base.Inspector.ComponentFilter(gameObject, components);
			}
		}
		base.Refresh();
	}

	[Preserve]
	private void AddComponentButtonClicked()
	{
		GameObject target = (GameObject)base.Value;
		if (!target)
		{
			return;
		}
		if (addComponentTypes == null)
		{
			List<Type> list = new List<Type>(128);
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (assembly.IsDynamic)
				{
					continue;
				}
				try
				{
					Type[] exportedTypes = assembly.GetExportedTypes();
					foreach (Type type in exportedTypes)
					{
						if (typeof(Component).IsAssignableFrom(type) && !type.IsGenericType && !type.IsAbstract)
						{
							list.Add(type);
						}
					}
				}
				catch (NotSupportedException)
				{
				}
				catch (FileNotFoundException)
				{
				}
				catch (Exception ex3)
				{
					Debug.LogError("Couldn't search assembly for Component types: " + assembly.GetName().Name + "\n" + ex3.ToString());
				}
			}
			addComponentTypes = list.ToArray();
		}
		ObjectReferencePicker.Instance.Skin = base.Inspector.Skin;
		ObjectReferencePicker instance = ObjectReferencePicker.Instance;
		ObjectReferencePicker.ReferenceCallback onSelectionConfirmed = delegate(object obj)
		{
			if (obj != null && (bool)target && (bool)base.Inspector && base.Inspector.InspectedObject as GameObject == target)
			{
				target.AddComponent((Type)obj);
				base.Inspector.Refresh();
			}
		};
		ObjectReferencePicker.NameGetter referenceNameGetter = (object obj) => ((Type)obj).FullName;
		ObjectReferencePicker.NameGetter referenceDisplayNameGetter = (object obj) => ((Type)obj).FullName;
		object[] references = addComponentTypes;
		instance.Show(null, onSelectionConfirmed, referenceNameGetter, referenceDisplayNameGetter, references, null, includeNullReference: false, "Add Component", base.Inspector.Canvas);
	}

	[Preserve]
	private static void RemoveComponentButtonClicked(ExpandableInspectorField componentDrawer)
	{
		if ((bool)componentDrawer && (bool)componentDrawer.Inspector)
		{
			Component component = componentDrawer.Value as Component;
			if ((bool)component && !(component is Transform))
			{
				componentDrawer.StartCoroutine(RemoveComponentCoroutine(component, componentDrawer.Inspector));
			}
		}
	}

	private static IEnumerator RemoveComponentCoroutine(Component component, RuntimeInspector inspector)
	{
		UnityEngine.Object.Destroy(component);
		yield return null;
		inspector.Refresh();
		inspector.EnsureScrollViewIsWithinBounds();
	}
}
