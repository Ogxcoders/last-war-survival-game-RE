namespace System.Data.SqlClient;

internal struct FederatedAuthenticationFeatureExtensionData
{
	internal TdsEnums.FedAuthLibrary libraryType;

	internal bool fedAuthRequiredPreLoginResponse;

	internal byte[] accessToken;
}
