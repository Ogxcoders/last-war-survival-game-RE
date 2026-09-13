using System.Security;

namespace System.Data.SqlClient;

internal sealed class SqlLogin
{
	internal int timeout;

	internal bool userInstance;

	internal string hostName = "";

	internal string userName = "";

	internal string password = "";

	internal string applicationName = "";

	internal string serverName = "";

	internal string language = "";

	internal string database = "";

	internal string attachDBFilename = "";

	internal bool useReplication;

	internal string newPassword = "";

	internal bool useSSPI;

	internal int packetSize = 8000;

	internal bool readOnlyIntent;

	internal SqlCredential credential;

	internal SecureString newSecurePassword;
}
