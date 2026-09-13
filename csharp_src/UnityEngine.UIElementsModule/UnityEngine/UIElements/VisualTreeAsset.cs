#define UNITY_ASSERTIONS
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements;

[Serializable]
public class VisualTreeAsset : ScriptableObject
{
	[Serializable]
	internal struct UsingEntry
	{
		internal static readonly IComparer<UsingEntry> comparer = new UsingEntryComparer();

		[SerializeField]
		public string alias;

		[SerializeField]
		public string path;

		[SerializeField]
		public VisualTreeAsset asset;

		public UsingEntry(string alias, string path)
		{
			this.alias = alias;
			this.path = path;
			asset = null;
		}

		public UsingEntry(string alias, VisualTreeAsset asset)
		{
			this.alias = alias;
			path = null;
			this.asset = asset;
		}
	}

	private class UsingEntryComparer : IComparer<UsingEntry>
	{
		public int Compare(UsingEntry x, UsingEntry y)
		{
			return string.CompareOrdinal(x.alias, y.alias);
		}
	}

	[Serializable]
	internal struct SlotDefinition
	{
		[SerializeField]
		public string name;

		[SerializeField]
		public int insertionPointId;
	}

	[Serializable]
	internal struct SlotUsageEntry
	{
		[SerializeField]
		public string slotName;

		[SerializeField]
		public int assetId;

		public SlotUsageEntry(string slotName, int assetId)
		{
			this.slotName = slotName;
			this.assetId = assetId;
		}
	}

	private static StylePropertyReader s_StylePropertyReader = new StylePropertyReader();

	private static readonly Dictionary<string, VisualElement> s_TemporarySlotInsertionPoints = new Dictionary<string, VisualElement>();

	[SerializeField]
	private List<UsingEntry> m_Usings;

	[SerializeField]
	internal StyleSheet inlineSheet;

	[SerializeField]
	private List<VisualElementAsset> m_VisualElementAssets;

	[SerializeField]
	private List<TemplateAsset> m_TemplateAssets;

	[SerializeField]
	private List<SlotDefinition> m_Slots;

	[SerializeField]
	private int m_ContentContainerId;

	[SerializeField]
	private int m_ContentHash;

	internal List<VisualElementAsset> visualElementAssets
	{
		get
		{
			return m_VisualElementAssets;
		}
		set
		{
			m_VisualElementAssets = value;
		}
	}

	internal List<TemplateAsset> templateAssets
	{
		get
		{
			return m_TemplateAssets;
		}
		set
		{
			m_TemplateAssets = value;
		}
	}

	internal List<SlotDefinition> slots
	{
		get
		{
			return m_Slots;
		}
		set
		{
			m_Slots = value;
		}
	}

	internal int contentContainerId
	{
		get
		{
			return m_ContentContainerId;
		}
		set
		{
			m_ContentContainerId = value;
		}
	}

	internal int contentHash
	{
		get
		{
			return m_ContentHash;
		}
		set
		{
			m_ContentHash = value;
		}
	}

	internal int GetNextChildSerialNumber()
	{
		int num = m_VisualElementAssets?.Count ?? 0;
		return num + (m_TemplateAssets?.Count ?? 0);
	}

	public TemplateContainer CloneTree()
	{
		TemplateContainer templateContainer = new TemplateContainer(base.name);
		CloneTree(templateContainer);
		return templateContainer;
	}

	public TemplateContainer CloneTree(string bindingPath)
	{
		TemplateContainer templateContainer = CloneTree();
		templateContainer.bindingPath = bindingPath;
		return templateContainer;
	}

	public void CloneTree(VisualElement target)
	{
		try
		{
			CloneTree(target, s_TemporarySlotInsertionPoints);
		}
		finally
		{
			s_TemporarySlotInsertionPoints.Clear();
		}
	}

	internal void CloneTree(VisualElement target, Dictionary<string, VisualElement> slotInsertionPoints)
	{
		CloneTree(target, slotInsertionPoints, null);
	}

	internal void CloneTree(VisualElement target, Dictionary<string, VisualElement> slotInsertionPoints, List<TemplateAsset.AttributeOverride> attributeOverrides)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if ((visualElementAssets == null || visualElementAssets.Count <= 0) && (templateAssets == null || templateAssets.Count <= 0))
		{
			return;
		}
		Dictionary<int, List<VisualElementAsset>> dictionary = new Dictionary<int, List<VisualElementAsset>>();
		int num = ((visualElementAssets != null) ? visualElementAssets.Count : 0);
		int num2 = ((templateAssets != null) ? templateAssets.Count : 0);
		for (int i = 0; i < num + num2; i++)
		{
			VisualElementAsset visualElementAsset = ((i < num) ? visualElementAssets[i] : templateAssets[i - num]);
			if (!dictionary.TryGetValue(visualElementAsset.parentId, out var value))
			{
				value = new List<VisualElementAsset>();
				dictionary.Add(visualElementAsset.parentId, value);
			}
			value.Add(visualElementAsset);
		}
		if (!dictionary.TryGetValue(0, out var value2) || value2 == null)
		{
			return;
		}
		value2.Sort(CompareForOrder);
		foreach (VisualElementAsset item in value2)
		{
			Assert.IsNotNull(item);
			VisualElement child = CloneSetupRecursively(item, dictionary, new CreationContext(slotInsertionPoints, attributeOverrides, this, target));
			target.hierarchy.Add(child);
		}
	}

	private VisualElement CloneSetupRecursively(VisualElementAsset root, Dictionary<int, List<VisualElementAsset>> idToChildren, CreationContext context)
	{
		VisualElement visualElement = Create(root, context);
		if (root.id == context.visualTreeAsset.contentContainerId)
		{
			if (context.target is TemplateContainer)
			{
				((TemplateContainer)context.target).SetContentContainer(visualElement);
			}
			else
			{
				Debug.LogError("Trying to clone a VisualTreeAsset with a custom content container into a element which is not a template container");
			}
		}
		if (context.slotInsertionPoints != null && TryGetSlotInsertionPoint(root.id, out var slotName))
		{
			context.slotInsertionPoints.Add(slotName, visualElement);
		}
		if (root.classes != null)
		{
			for (int i = 0; i < root.classes.Length; i++)
			{
				visualElement.AddToClassList(root.classes[i]);
			}
		}
		if (root.ruleIndex != -1)
		{
			if (inlineSheet == null)
			{
				Debug.LogWarning("VisualElementAsset has a RuleIndex but no inlineStyleSheet");
			}
			else
			{
				StyleRule rule = inlineSheet.rules[root.ruleIndex];
				VisualElementStylesData visualElementStylesData = new VisualElementStylesData(isShared: false);
				visualElement.SetInlineStyles(visualElementStylesData);
				s_StylePropertyReader.SetInlineContext(inlineSheet, rule, root.ruleIndex);
				visualElementStylesData.ApplyProperties(s_StylePropertyReader, null);
			}
		}
		TemplateAsset templateAsset = root as TemplateAsset;
		if (idToChildren.TryGetValue(root.id, out var value))
		{
			value.Sort(CompareForOrder);
			foreach (VisualElementAsset childVea in value)
			{
				VisualElement visualElement2 = CloneSetupRecursively(childVea, idToChildren, context);
				if (visualElement2 == null)
				{
					continue;
				}
				if (templateAsset == null)
				{
					visualElement.Add(visualElement2);
					continue;
				}
				int num = ((templateAsset.slotUsages == null) ? (-1) : templateAsset.slotUsages.FindIndex((SlotUsageEntry u) => u.assetId == childVea.id));
				if (num != -1)
				{
					string slotName2 = templateAsset.slotUsages[num].slotName;
					Assert.IsFalse(string.IsNullOrEmpty(slotName2), "a lost name should not be null or empty, this probably points to an importer or serialization bug");
					if (context.slotInsertionPoints == null || !context.slotInsertionPoints.TryGetValue(slotName2, out var value2))
					{
						Debug.LogErrorFormat("Slot '{0}' was not found. Existing slots: {1}", slotName2, (context.slotInsertionPoints == null) ? string.Empty : string.Join(", ", context.slotInsertionPoints.Keys.ToArray()));
						visualElement.Add(visualElement2);
					}
					else
					{
						value2.Add(visualElement2);
					}
				}
				else
				{
					visualElement.Add(visualElement2);
				}
			}
		}
		if (templateAsset != null && context.slotInsertionPoints != null)
		{
			context.slotInsertionPoints.Clear();
		}
		return visualElement;
	}

	private static int CompareForOrder(VisualElementAsset a, VisualElementAsset b)
	{
		return a.orderInDocument.CompareTo(b.orderInDocument);
	}

	internal bool TryGetSlotInsertionPoint(int insertionPointId, out string slotName)
	{
		if (m_Slots == null)
		{
			slotName = null;
			return false;
		}
		for (int i = 0; i < m_Slots.Count; i++)
		{
			SlotDefinition slotDefinition = m_Slots[i];
			if (slotDefinition.insertionPointId == insertionPointId)
			{
				slotName = slotDefinition.name;
				return true;
			}
		}
		slotName = null;
		return false;
	}

	internal VisualTreeAsset ResolveTemplate(string templateName)
	{
		if (m_Usings == null || m_Usings.Count == 0)
		{
			return null;
		}
		int num = m_Usings.BinarySearch(new UsingEntry(templateName, string.Empty), UsingEntry.comparer);
		if (num < 0)
		{
			return null;
		}
		if ((bool)m_Usings[num].asset)
		{
			return m_Usings[num].asset;
		}
		string path = m_Usings[num].path;
		return Panel.LoadResource(path, typeof(VisualTreeAsset), GUIUtility.pixelsPerPoint) as VisualTreeAsset;
	}

	internal static VisualElement Create(VisualElementAsset asset, CreationContext ctx)
	{
		if (!VisualElementFactoryRegistry.TryGetValue(asset.fullTypeName, out var factoryList))
		{
			if (!asset.fullTypeName.StartsWith("UnityEngine.Experimental.UIElements.") && !asset.fullTypeName.StartsWith("UnityEditor.Experimental.UIElements."))
			{
				Debug.LogErrorFormat("Element '{0}' has no registered factory method.", asset.fullTypeName);
				return new Label($"Unknown type: '{asset.fullTypeName}'");
			}
			string fullTypeName = asset.fullTypeName.Replace(".Experimental.UIElements", ".UIElements");
			if (!VisualElementFactoryRegistry.TryGetValue(fullTypeName, out factoryList))
			{
				Debug.LogErrorFormat("Element '{0}' has no registered factory method.", asset.fullTypeName);
				return new Label($"Unknown type: '{asset.fullTypeName}'");
			}
		}
		IUxmlFactory uxmlFactory = null;
		foreach (IUxmlFactory item in factoryList)
		{
			if (item.AcceptsAttributeBag(asset, ctx))
			{
				uxmlFactory = item;
				break;
			}
		}
		if (uxmlFactory == null)
		{
			Debug.LogErrorFormat("Element '{0}' has a no factory that accept the set of XML attributes specified.", asset.fullTypeName);
			return new Label($"Type with no factory: '{asset.fullTypeName}'");
		}
		if (uxmlFactory is UxmlRootElementFactory)
		{
			return null;
		}
		VisualElement visualElement = uxmlFactory.Create(asset, ctx);
		if (visualElement == null)
		{
			Debug.LogErrorFormat("The factory of Visual Element Type '{0}' has returned a null object", asset.fullTypeName);
			return new Label($"The factory of Visual Element Type '{asset.fullTypeName}' has returned a null object");
		}
		if (asset.classes != null)
		{
			for (int i = 0; i < asset.classes.Length; i++)
			{
				visualElement.AddToClassList(asset.classes[i]);
			}
		}
		if (asset.stylesheetPaths != null)
		{
			for (int j = 0; j < asset.stylesheetPaths.Count; j++)
			{
				visualElement.AddStyleSheetPath(asset.stylesheetPaths[j]);
			}
		}
		if (asset.stylesheets != null)
		{
			for (int k = 0; k < asset.stylesheets.Count; k++)
			{
				visualElement.styleSheets.Add(asset.stylesheets[k]);
			}
		}
		return visualElement;
	}

	public override int GetHashCode()
	{
		return contentHash;
	}
}
