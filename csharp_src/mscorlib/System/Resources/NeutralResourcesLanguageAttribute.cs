namespace System.Resources;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class NeutralResourcesLanguageAttribute : Attribute
{
	public string CultureName { get; }

	public UltimateResourceFallbackLocation Location { get; }

	public NeutralResourcesLanguageAttribute(string cultureName)
	{
		if (cultureName == null)
		{
			throw new ArgumentNullException("cultureName");
		}
		CultureName = cultureName;
		Location = UltimateResourceFallbackLocation.MainAssembly;
	}

	public NeutralResourcesLanguageAttribute(string cultureName, UltimateResourceFallbackLocation location)
	{
		if (cultureName == null)
		{
			throw new ArgumentNullException("cultureName");
		}
		if (!Enum.IsDefined(typeof(UltimateResourceFallbackLocation), location))
		{
			throw new ArgumentException(SR.Format("The NeutralResourcesLanguageAttribute specifies an invalid or unrecognized ultimate resource fallback location: \"{0}\".", location));
		}
		CultureName = cultureName;
		Location = location;
	}
}
