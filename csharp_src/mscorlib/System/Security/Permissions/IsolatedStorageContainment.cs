namespace System.Security.Permissions;

public enum IsolatedStorageContainment
{
	None = 0,
	DomainIsolationByUser = 16,
	ApplicationIsolationByUser = 21,
	AssemblyIsolationByUser = 32,
	DomainIsolationByMachine = 48,
	AssemblyIsolationByMachine = 64,
	ApplicationIsolationByMachine = 69,
	DomainIsolationByRoamingUser = 80,
	AssemblyIsolationByRoamingUser = 96,
	ApplicationIsolationByRoamingUser = 101,
	AdministerIsolatedStorageByUser = 112,
	UnrestrictedIsolatedStorage = 240
}
