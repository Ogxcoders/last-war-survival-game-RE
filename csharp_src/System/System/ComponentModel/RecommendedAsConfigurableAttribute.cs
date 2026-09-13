namespace System.ComponentModel;

[AttributeUsage(AttributeTargets.Property)]
[Obsolete("Use System.ComponentModel.SettingsBindableAttribute instead to work with the new settings model.")]
public class RecommendedAsConfigurableAttribute : Attribute
{
	public static readonly RecommendedAsConfigurableAttribute No = new RecommendedAsConfigurableAttribute(recommendedAsConfigurable: false);

	public static readonly RecommendedAsConfigurableAttribute Yes = new RecommendedAsConfigurableAttribute(recommendedAsConfigurable: true);

	public static readonly RecommendedAsConfigurableAttribute Default = No;

	public bool RecommendedAsConfigurable { get; }

	public RecommendedAsConfigurableAttribute(bool recommendedAsConfigurable)
	{
		RecommendedAsConfigurable = recommendedAsConfigurable;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		if (obj is RecommendedAsConfigurableAttribute recommendedAsConfigurableAttribute)
		{
			return recommendedAsConfigurableAttribute.RecommendedAsConfigurable == RecommendedAsConfigurable;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool IsDefaultAttribute()
	{
		return !RecommendedAsConfigurable;
	}
}
