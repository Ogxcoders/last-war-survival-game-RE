using System;
using System.Collections.Generic;
using System.Diagnostics;
using Sirenix.OdinInspector.Internal;

namespace Sirenix.OdinInspector;

[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
public class TabGroupAttribute : PropertyGroupAttribute, ISubGroupProviderAttribute
{
	[Conditional("UNITY_EDITOR")]
	public class TabSubGroupAttribute : PropertyGroupAttribute
	{
		public TabSubGroupAttribute(string groupId, float order)
			: base(groupId, order)
		{
		}
	}

	public const string DEFAULT_NAME = "_DefaultTabGroup";

	public string TabName;

	public bool UseFixedHeight;

	public bool Paddingless;

	public bool HideTabGroupIfTabGroupOnlyHasOneTab;

	public List<string> Tabs { get; private set; }

	public TabGroupAttribute(string tab, bool useFixedHeight = false, float order = 0f)
		: this("_DefaultTabGroup", tab, useFixedHeight, order)
	{
	}

	public TabGroupAttribute(string group, string tab, bool useFixedHeight = false, float order = 0f)
		: base(group, order)
	{
		TabName = tab;
		UseFixedHeight = useFixedHeight;
		Tabs = new List<string>();
		if (tab != null)
		{
			Tabs.Add(tab);
		}
		Tabs = new List<string>(Tabs);
	}

	protected override void CombineValuesWith(PropertyGroupAttribute other)
	{
		base.CombineValuesWith(other);
		TabGroupAttribute tabGroupAttribute = other as TabGroupAttribute;
		if (tabGroupAttribute.TabName != null)
		{
			UseFixedHeight = UseFixedHeight || tabGroupAttribute.UseFixedHeight;
			Paddingless = Paddingless || tabGroupAttribute.Paddingless;
			HideTabGroupIfTabGroupOnlyHasOneTab = HideTabGroupIfTabGroupOnlyHasOneTab || tabGroupAttribute.HideTabGroupIfTabGroupOnlyHasOneTab;
			if (!Tabs.Contains(tabGroupAttribute.TabName))
			{
				Tabs.Add(tabGroupAttribute.TabName);
			}
		}
	}

	IList<PropertyGroupAttribute> ISubGroupProviderAttribute.GetSubGroupAttributes()
	{
		int num = 0;
		List<PropertyGroupAttribute> list = new List<PropertyGroupAttribute>(Tabs.Count);
		foreach (string tab in Tabs)
		{
			list.Add(new TabSubGroupAttribute(GroupID + "/" + tab, num++));
		}
		return list;
	}

	string ISubGroupProviderAttribute.RepathMemberAttribute(PropertyGroupAttribute attr)
	{
		TabGroupAttribute tabGroupAttribute = (TabGroupAttribute)attr;
		return GroupID + "/" + tabGroupAttribute.TabName;
	}
}
