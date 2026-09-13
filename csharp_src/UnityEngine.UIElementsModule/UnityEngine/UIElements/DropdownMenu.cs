using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

public class DropdownMenu
{
	private List<DropdownMenuItem> menuItems = new List<DropdownMenuItem>();

	private DropdownMenuEventInfo m_DropdownMenuEventInfo;

	public List<DropdownMenuItem> MenuItems()
	{
		return menuItems;
	}

	public void AppendAction(string actionName, Action<DropdownMenuAction> action, Func<DropdownMenuAction, DropdownMenuAction.Status> actionStatusCallback, object userData = null)
	{
		DropdownMenuAction item = new DropdownMenuAction(actionName, action, actionStatusCallback, userData);
		menuItems.Add(item);
	}

	public void AppendAction(string actionName, Action<DropdownMenuAction> action, DropdownMenuAction.Status status = DropdownMenuAction.Status.Normal)
	{
		if (status == DropdownMenuAction.Status.Normal)
		{
			AppendAction(actionName, action, DropdownMenuAction.AlwaysEnabled);
			return;
		}
		if (status == DropdownMenuAction.Status.Disabled)
		{
			AppendAction(actionName, action, DropdownMenuAction.AlwaysDisabled);
			return;
		}
		AppendAction(actionName, action, (DropdownMenuAction e) => status);
	}

	public void InsertAction(int atIndex, string actionName, Action<DropdownMenuAction> action, Func<DropdownMenuAction, DropdownMenuAction.Status> actionStatusCallback, object userData = null)
	{
		DropdownMenuAction item = new DropdownMenuAction(actionName, action, actionStatusCallback, userData);
		menuItems.Insert(atIndex, item);
	}

	public void InsertAction(int atIndex, string actionName, Action<DropdownMenuAction> action, DropdownMenuAction.Status status = DropdownMenuAction.Status.Normal)
	{
		if (status == DropdownMenuAction.Status.Normal)
		{
			InsertAction(atIndex, actionName, action, DropdownMenuAction.AlwaysEnabled);
			return;
		}
		if (status == DropdownMenuAction.Status.Disabled)
		{
			InsertAction(atIndex, actionName, action, DropdownMenuAction.AlwaysDisabled);
			return;
		}
		InsertAction(atIndex, actionName, action, (DropdownMenuAction e) => status);
	}

	public void AppendSeparator(string subMenuPath = null)
	{
		if (menuItems.Count > 0 && !(menuItems[menuItems.Count - 1] is DropdownMenuSeparator))
		{
			DropdownMenuSeparator item = new DropdownMenuSeparator(subMenuPath ?? string.Empty);
			menuItems.Add(item);
		}
	}

	public void InsertSeparator(string subMenuPath, int atIndex)
	{
		if (atIndex > 0 && atIndex <= menuItems.Count && !(menuItems[atIndex - 1] is DropdownMenuSeparator))
		{
			DropdownMenuSeparator item = new DropdownMenuSeparator(subMenuPath ?? string.Empty);
			menuItems.Insert(atIndex, item);
		}
	}

	public void RemoveItemAt(int index)
	{
		menuItems.RemoveAt(index);
	}

	public void PrepareForDisplay(EventBase e)
	{
		m_DropdownMenuEventInfo = ((e != null) ? new DropdownMenuEventInfo(e) : null);
		if (menuItems.Count == 0)
		{
			return;
		}
		foreach (DropdownMenuItem menuItem in menuItems)
		{
			if (menuItem is DropdownMenuAction dropdownMenuAction)
			{
				dropdownMenuAction.UpdateActionStatus(m_DropdownMenuEventInfo);
			}
		}
		if (menuItems[menuItems.Count - 1] is DropdownMenuSeparator)
		{
			menuItems.RemoveAt(menuItems.Count - 1);
		}
	}
}
