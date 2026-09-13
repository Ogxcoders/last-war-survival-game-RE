namespace System.Data.SqlClient;

internal sealed class Row
{
	private object[] _dataFields;

	internal object[] DataFields => _dataFields;

	internal object this[int index] => _dataFields[index];

	internal Row(int rowCount)
	{
		_dataFields = new object[rowCount];
	}
}
