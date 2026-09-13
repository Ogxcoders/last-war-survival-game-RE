using System.Collections.Generic;
using UnityEngine;

namespace RuntimeInspectorNamespace;

public class PseudoSceneSourceTransform : MonoBehaviour
{
	[SerializeField]
	private RuntimeHierarchy m_hierarchy;

	[SerializeField]
	private string m_sceneName;

	[SerializeField]
	private bool m_hideOnDisable;

	private HashSet<Transform> childrenCurrent = new HashSet<Transform>();

	private HashSet<Transform> childrenNew = new HashSet<Transform>();

	private bool updateChildren;

	private bool isEnabled = true;

	private bool isQuitting;

	public RuntimeHierarchy Hierarchy
	{
		get
		{
			return m_hierarchy;
		}
		set
		{
			if (m_hierarchy != value)
			{
				RemoveChildrenFromScene();
				m_hierarchy = value;
				AddChildrenToScene();
			}
		}
	}

	public string SceneName
	{
		get
		{
			return m_sceneName;
		}
		set
		{
			if (m_sceneName != value)
			{
				RemoveChildrenFromScene();
				m_sceneName = value;
				AddChildrenToScene();
			}
		}
	}

	public bool HideOnDisable
	{
		get
		{
			return m_hideOnDisable;
		}
		set
		{
			if (m_hideOnDisable == value)
			{
				return;
			}
			m_hideOnDisable = value;
			if (!isEnabled)
			{
				if (value)
				{
					RemoveChildrenFromScene();
				}
				else
				{
					AddChildrenToScene();
				}
			}
		}
	}

	private bool ShouldUpdateChildren
	{
		get
		{
			if ((isEnabled || !m_hideOnDisable) && (bool)Hierarchy)
			{
				return !string.IsNullOrEmpty(m_sceneName);
			}
			return false;
		}
	}

	private void OnEnable()
	{
		isEnabled = true;
		updateChildren = true;
	}

	private void OnDisable()
	{
		if (!isQuitting)
		{
			isEnabled = false;
			if (m_hideOnDisable)
			{
				RemoveChildrenFromScene();
			}
		}
	}

	private void OnApplicationQuit()
	{
		isQuitting = true;
	}

	private void OnTransformChildrenChanged()
	{
		updateChildren = true;
	}

	private void Update()
	{
		if (!updateChildren)
		{
			return;
		}
		updateChildren = false;
		if (!ShouldUpdateChildren)
		{
			return;
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			childrenNew.Add(child);
			if (!childrenCurrent.Remove(child))
			{
				Hierarchy.AddToPseudoScene(m_sceneName, child);
			}
		}
		RemoveChildrenFromScene();
		HashSet<Transform> hashSet = childrenCurrent;
		childrenCurrent = childrenNew;
		childrenNew = hashSet;
	}

	private void AddChildrenToScene()
	{
		if (!ShouldUpdateChildren)
		{
			return;
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			if (childrenCurrent.Add(child))
			{
				Hierarchy.AddToPseudoScene(m_sceneName, child);
			}
		}
	}

	private void RemoveChildrenFromScene()
	{
		if (!Hierarchy || string.IsNullOrEmpty(m_sceneName))
		{
			return;
		}
		foreach (Transform item in childrenCurrent)
		{
			if ((bool)item)
			{
				Hierarchy.RemoveFromPseudoScene(m_sceneName, item, deleteSceneIfEmpty: true);
			}
		}
		childrenCurrent.Clear();
	}
}
