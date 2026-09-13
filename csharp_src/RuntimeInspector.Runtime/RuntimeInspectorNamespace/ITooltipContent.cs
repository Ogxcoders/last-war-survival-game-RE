namespace RuntimeInspectorNamespace;

public interface ITooltipContent
{
	bool IsActive { get; }

	string TooltipText { get; }
}
