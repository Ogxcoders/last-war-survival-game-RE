using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIChatTheme : MonoBehaviour
{
	public List<ImageNode> ImageNodes;

	public List<TextNode> TextNodes;

	private List<TextNode> _tmpTextNodes;

	public List<UIChatTheme> ThemeNodes;

	public ThemeMode CurrentMode = ThemeMode.Normal;

	private void Awake()
	{
		UIChatTheme[] componentsInParent = GetComponentsInParent<UIChatTheme>();
		foreach (UIChatTheme uIChatTheme in componentsInParent)
		{
			if (!uIChatTheme.ThemeNodes.Contains(this))
			{
				uIChatTheme.ThemeNodes.Add(this);
			}
		}
	}

	private void OnEnable()
	{
		CurrentMode = (ThemeMode)GameEntry.Setting.PlayerPrefsGetInt("CHAT_THEME_TYPE", 1);
		RefreshView();
	}

	private void OnDisable()
	{
	}

	private void OnDestroy()
	{
		UIChatTheme[] componentsInParent = GetComponentsInParent<UIChatTheme>();
		foreach (UIChatTheme uIChatTheme in componentsInParent)
		{
			if (uIChatTheme.ThemeNodes.Contains(this))
			{
				uIChatTheme.ThemeNodes.Remove(this);
			}
		}
	}

	public void CollectNode()
	{
		if (ThemeNodes == null)
		{
			ThemeNodes = new List<UIChatTheme>();
		}
		if (ImageNodes == null)
		{
			ImageNodes = new List<ImageNode>();
		}
		if (TextNodes == null)
		{
			TextNodes = new List<TextNode>();
		}
		if (_tmpTextNodes == null)
		{
			_tmpTextNodes = new List<TextNode>();
		}
		ThemeNodes.Clear();
		ImageNodes.Clear();
		_tmpTextNodes.Clear();
		_tmpTextNodes.AddRange(TextNodes);
		TextNodes.Clear();
		CollectFromNode(base.transform);
	}

	public void CollectFromNode(Transform current)
	{
		if (current != base.transform && current.GetComponent<UIChatTheme>() != null)
		{
			current.GetComponent<UIChatTheme>().CollectNode();
			return;
		}
		Image component = current.GetComponent<Image>();
		if (component != null)
		{
			ImageNode imageNode = new ImageNode(component);
			if (imageNode.parseSuccess)
			{
				ImageNodes.Add(imageNode);
			}
		}
		TextMeshProUGUI component2 = current.GetComponent<TextMeshProUGUI>();
		if (component2 != null)
		{
			TextNode textNode = null;
			foreach (TextNode tmpTextNode in _tmpTextNodes)
			{
				if (tmpTextNode.component == component2)
				{
					textNode = tmpTextNode;
					textNode.UpdateOrigin();
					break;
				}
			}
			if (textNode == null)
			{
				textNode = new TextNode(component2);
			}
			if (textNode.parseSuccess)
			{
				TextNodes.Add(textNode);
			}
		}
		for (int i = 0; i < current.childCount; i++)
		{
			CollectFromNode(current.GetChild(i));
		}
	}

	public void RefreshViewByMode(ThemeMode mode)
	{
		foreach (UIChatTheme themeNode in ThemeNodes)
		{
			if (!(themeNode == this))
			{
				themeNode.RefreshViewByMode(mode);
			}
		}
		if (mode != CurrentMode)
		{
			CurrentMode = mode;
			RefreshView();
		}
	}

	public void RefreshView()
	{
		foreach (ImageNode imageNode in ImageNodes)
		{
			imageNode.ConvertTheme(CurrentMode);
		}
		foreach (TextNode textNode in TextNodes)
		{
			textNode.ConvertTheme(CurrentMode);
		}
	}
}
