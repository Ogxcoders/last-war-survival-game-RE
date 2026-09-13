using System.Security.Permissions;

namespace System.ComponentModel;

[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
public delegate void DoWorkEventHandler(object sender, DoWorkEventArgs e);
