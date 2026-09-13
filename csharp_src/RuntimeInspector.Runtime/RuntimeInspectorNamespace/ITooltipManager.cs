using UnityEngine;

namespace RuntimeInspectorNamespace;

public interface ITooltipManager
{
	UISkin Skin { get; }

	Canvas Canvas { get; }

	float TooltipDelay { get; }
}
