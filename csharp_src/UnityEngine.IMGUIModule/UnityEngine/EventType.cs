using System;
using System.ComponentModel;

namespace UnityEngine;

public enum EventType
{
	MouseDown = 0,
	MouseUp = 1,
	MouseMove = 2,
	MouseDrag = 3,
	KeyDown = 4,
	KeyUp = 5,
	ScrollWheel = 6,
	Repaint = 7,
	Layout = 8,
	DragUpdated = 9,
	DragPerform = 10,
	DragExited = 15,
	Ignore = 11,
	Used = 12,
	ValidateCommand = 13,
	ExecuteCommand = 14,
	ContextClick = 16,
	MouseEnterWindow = 20,
	MouseLeaveWindow = 21,
	[Obsolete("Use MouseDown instead (UnityUpgradable) -> MouseDown", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	mouseDown = 0,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use MouseUp instead (UnityUpgradable) -> MouseUp", true)]
	mouseUp = 1,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use MouseMove instead (UnityUpgradable) -> MouseMove", true)]
	mouseMove = 2,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use MouseDrag instead (UnityUpgradable) -> MouseDrag", true)]
	mouseDrag = 3,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use KeyDown instead (UnityUpgradable) -> KeyDown", true)]
	keyDown = 4,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use KeyUp instead (UnityUpgradable) -> KeyUp", true)]
	keyUp = 5,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use ScrollWheel instead (UnityUpgradable) -> ScrollWheel", true)]
	scrollWheel = 6,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use Repaint instead (UnityUpgradable) -> Repaint", true)]
	repaint = 7,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use Layout instead (UnityUpgradable) -> Layout", true)]
	layout = 8,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use DragUpdated instead (UnityUpgradable) -> DragUpdated", true)]
	dragUpdated = 9,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use DragPerform instead (UnityUpgradable) -> DragPerform", true)]
	dragPerform = 10,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use Ignore instead (UnityUpgradable) -> Ignore", true)]
	ignore = 11,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use Used instead (UnityUpgradable) -> Used", true)]
	used = 12
}
