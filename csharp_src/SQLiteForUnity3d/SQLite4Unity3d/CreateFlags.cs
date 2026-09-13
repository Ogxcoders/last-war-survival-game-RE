using System;

namespace SQLite4Unity3d;

[Flags]
public enum CreateFlags
{
	None = 0,
	ImplicitPK = 1,
	ImplicitIndex = 2,
	AllImplicit = 3,
	AutoIncPK = 4
}
