using System.IO;

namespace Microsoft.Win32;

internal static class Win32Native
{
	public class SECURITY_ATTRIBUTES
	{
	}

	internal class WIN32_FIND_DATA
	{
		internal int dwFileAttributes;

		internal string cFileName;
	}

	internal const string ADVAPI32 = "advapi32.dll";

	internal const int ERROR_SUCCESS = 0;

	internal const int ERROR_INVALID_FUNCTION = 1;

	internal const int ERROR_FILE_NOT_FOUND = 2;

	internal const int ERROR_PATH_NOT_FOUND = 3;

	internal const int ERROR_ACCESS_DENIED = 5;

	internal const int ERROR_INVALID_HANDLE = 6;

	internal const int ERROR_NOT_ENOUGH_MEMORY = 8;

	internal const int ERROR_INVALID_DATA = 13;

	internal const int ERROR_INVALID_DRIVE = 15;

	internal const int ERROR_NO_MORE_FILES = 18;

	internal const int ERROR_NOT_READY = 21;

	internal const int ERROR_BAD_LENGTH = 24;

	internal const int ERROR_SHARING_VIOLATION = 32;

	internal const int ERROR_NOT_SUPPORTED = 50;

	internal const int ERROR_FILE_EXISTS = 80;

	internal const int ERROR_INVALID_PARAMETER = 87;

	internal const int ERROR_BROKEN_PIPE = 109;

	internal const int ERROR_CALL_NOT_IMPLEMENTED = 120;

	internal const int ERROR_INSUFFICIENT_BUFFER = 122;

	internal const int ERROR_INVALID_NAME = 123;

	internal const int ERROR_BAD_PATHNAME = 161;

	internal const int ERROR_ALREADY_EXISTS = 183;

	internal const int ERROR_ENVVAR_NOT_FOUND = 203;

	internal const int ERROR_FILENAME_EXCED_RANGE = 206;

	internal const int ERROR_NO_DATA = 232;

	internal const int ERROR_PIPE_NOT_CONNECTED = 233;

	internal const int ERROR_MORE_DATA = 234;

	internal const int ERROR_DIRECTORY = 267;

	internal const int ERROR_OPERATION_ABORTED = 995;

	internal const int ERROR_NOT_FOUND = 1168;

	internal const int ERROR_NO_TOKEN = 1008;

	internal const int ERROR_DLL_INIT_FAILED = 1114;

	internal const int ERROR_NON_ACCOUNT_SID = 1257;

	internal const int ERROR_NOT_ALL_ASSIGNED = 1300;

	internal const int ERROR_UNKNOWN_REVISION = 1305;

	internal const int ERROR_INVALID_OWNER = 1307;

	internal const int ERROR_INVALID_PRIMARY_GROUP = 1308;

	internal const int ERROR_NO_SUCH_PRIVILEGE = 1313;

	internal const int ERROR_PRIVILEGE_NOT_HELD = 1314;

	internal const int ERROR_NONE_MAPPED = 1332;

	internal const int ERROR_INVALID_ACL = 1336;

	internal const int ERROR_INVALID_SID = 1337;

	internal const int ERROR_INVALID_SECURITY_DESCR = 1338;

	internal const int ERROR_BAD_IMPERSONATION_LEVEL = 1346;

	internal const int ERROR_CANT_OPEN_ANONYMOUS = 1347;

	internal const int ERROR_NO_SECURITY_ON_OBJECT = 1350;

	internal const int ERROR_TRUSTED_RELATIONSHIP_FAILURE = 1789;

	internal const FileAttributes FILE_ATTRIBUTE_DIRECTORY = FileAttributes.Directory;

	public static string GetMessage(int hr)
	{
		return "Error " + hr;
	}

	public static int MakeHRFromErrorCode(int errorCode)
	{
		return -2147024896 | errorCode;
	}
}
