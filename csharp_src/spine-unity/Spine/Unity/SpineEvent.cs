namespace Spine.Unity;

public class SpineEvent : SpineAttributeBase
{
	public bool audioOnly;

	public SpineEvent(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false, bool audioOnly = false)
	{
		base.startsWith = startsWith;
		base.dataField = dataField;
		base.includeNone = includeNone;
		base.fallbackToTextField = fallbackToTextField;
		this.audioOnly = audioOnly;
	}
}
