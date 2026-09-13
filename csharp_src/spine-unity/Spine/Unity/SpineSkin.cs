namespace Spine.Unity;

public class SpineSkin : SpineAttributeBase
{
	public bool defaultAsEmptyString;

	public SpineSkin(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false, bool defaultAsEmptyString = false)
	{
		base.startsWith = startsWith;
		base.dataField = dataField;
		base.includeNone = includeNone;
		base.fallbackToTextField = fallbackToTextField;
		this.defaultAsEmptyString = defaultAsEmptyString;
	}
}
