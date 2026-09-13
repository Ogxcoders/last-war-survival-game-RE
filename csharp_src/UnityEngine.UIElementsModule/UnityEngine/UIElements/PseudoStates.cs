using System;

namespace UnityEngine.UIElements;

[Flags]
internal enum PseudoStates
{
	Active = 1,
	Hover = 2,
	Checked = 8,
	Disabled = 0x20,
	Focus = 0x40,
	Root = 0x80
}
