using System.Security.Permissions;

namespace System.ComponentModel;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
