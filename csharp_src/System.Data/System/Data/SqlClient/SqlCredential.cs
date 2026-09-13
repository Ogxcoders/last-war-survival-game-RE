using System.Security;

namespace System.Data.SqlClient;

[Serializable]
public sealed class SqlCredential
{
	private string uid = "";

	private SecureString pwd;

	public string UserId => uid;

	public SecureString Password => pwd;

	public SqlCredential(string userId, SecureString password)
	{
		if (userId == null)
		{
			throw new ArgumentNullException("userId");
		}
		if (password == null)
		{
			throw new ArgumentNullException("password");
		}
		uid = userId;
		pwd = password;
	}
}
