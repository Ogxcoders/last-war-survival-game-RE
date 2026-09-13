namespace Mono.Security.Interface;

public class ValidationResult
{
	private bool trusted;

	private bool user_denied;

	private int error_code;

	private MonoSslPolicyErrors? policy_errors;

	public bool Trusted => trusted;

	public bool UserDenied => user_denied;

	public int ErrorCode => error_code;

	public MonoSslPolicyErrors? PolicyErrors => policy_errors;

	public ValidationResult(bool trusted, bool user_denied, int error_code, MonoSslPolicyErrors? policy_errors)
	{
		this.trusted = trusted;
		this.user_denied = user_denied;
		this.error_code = error_code;
		this.policy_errors = policy_errors;
	}

	internal ValidationResult(bool trusted, bool user_denied, int error_code)
	{
		this.trusted = trusted;
		this.user_denied = user_denied;
		this.error_code = error_code;
	}
}
