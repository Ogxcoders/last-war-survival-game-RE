namespace UnityEngine.UIElements;

public class UxmlValueBounds : UxmlTypeRestriction
{
	public string min { get; set; }

	public string max { get; set; }

	public bool excludeMin { get; set; }

	public bool excludeMax { get; set; }

	public override bool Equals(UxmlTypeRestriction other)
	{
		if (!(other is UxmlValueBounds uxmlValueBounds))
		{
			return false;
		}
		return min == uxmlValueBounds.min && max == uxmlValueBounds.max && excludeMin == uxmlValueBounds.excludeMin && excludeMax == uxmlValueBounds.excludeMax;
	}
}
