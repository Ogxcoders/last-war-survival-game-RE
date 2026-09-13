namespace System.ComponentModel;

public interface ISite : IServiceProvider
{
	IComponent Component { get; }

	IContainer Container { get; }

	bool DesignMode { get; }

	string Name { get; set; }
}
