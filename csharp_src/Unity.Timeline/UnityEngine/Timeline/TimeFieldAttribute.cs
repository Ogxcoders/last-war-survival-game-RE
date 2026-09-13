namespace UnityEngine.Timeline;

internal class TimeFieldAttribute : PropertyAttribute
{
	public enum UseEditMode
	{
		None,
		ApplyEditMode
	}

	public UseEditMode useEditMode { get; }

	public TimeFieldAttribute(UseEditMode useEditMode = UseEditMode.ApplyEditMode)
	{
		this.useEditMode = useEditMode;
	}
}
