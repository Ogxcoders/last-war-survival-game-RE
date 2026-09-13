namespace System.Configuration;

public enum ConfigurationAllowExeDefinition
{
	MachineOnly = 0,
	MachineToApplication = 100,
	MachineToLocalUser = 300,
	MachineToRoamingUser = 200
}
