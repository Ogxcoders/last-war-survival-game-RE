using UnityEngine.Bindings;

namespace UnityEngine.TextCore;

[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
internal enum TextOverflowMode
{
	Overflow,
	Ellipsis,
	Masking,
	Truncate,
	ScrollRect,
	Page,
	Linked
}
