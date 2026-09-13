namespace System.ComponentModel;

public class DataErrorsChangedEventArgs : EventArgs
{
	private readonly string _propertyName;

	public virtual string PropertyName => _propertyName;

	public DataErrorsChangedEventArgs(string propertyName)
	{
		_propertyName = propertyName;
	}
}
