namespace UnityEngine.UIElements;

public class UxmlValueMatches : UxmlTypeRestriction
{
	public string regex { get; set; }

	public override bool Equals(UxmlTypeRestriction other)
	{
		if (!(other is UxmlValueMatches uxmlValueMatches))
		{
			return false;
		}
		return regex == uxmlValueMatches.regex;
	}
}
