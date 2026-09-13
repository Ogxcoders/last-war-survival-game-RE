namespace System.Security;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class SecurityRulesAttribute : Attribute
{
	private SecurityRuleSet m_ruleSet;

	private bool m_skipVerificationInFullTrust;

	public bool SkipVerificationInFullTrust
	{
		get
		{
			return m_skipVerificationInFullTrust;
		}
		set
		{
			m_skipVerificationInFullTrust = value;
		}
	}

	public SecurityRuleSet RuleSet => m_ruleSet;

	public SecurityRulesAttribute(SecurityRuleSet ruleSet)
	{
		m_ruleSet = ruleSet;
	}
}
