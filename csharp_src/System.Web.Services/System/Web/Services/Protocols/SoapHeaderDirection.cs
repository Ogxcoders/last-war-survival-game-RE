namespace System.Web.Services.Protocols;

[Flags]
public enum SoapHeaderDirection
{
	In = 1,
	InOut = 3,
	Out = 2,
	Fault = 4
}
