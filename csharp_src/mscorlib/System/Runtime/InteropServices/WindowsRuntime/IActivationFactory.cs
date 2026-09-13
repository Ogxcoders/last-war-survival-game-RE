namespace System.Runtime.InteropServices.WindowsRuntime;

[ComImport]
[Guid("00000035-0000-0000-C000-000000000046")]
public interface IActivationFactory
{
	object ActivateInstance();
}
