namespace System.Data;

internal sealed class DataError
{
	internal struct ColumnError
	{
		internal DataColumn _column;

		internal string _error;
	}

	private string _rowError = string.Empty;

	private int _count;

	private ColumnError[] _errorList;

	internal const int initialCapacity = 1;

	internal string Text
	{
		get
		{
			return _rowError;
		}
		set
		{
			SetText(value);
		}
	}

	internal bool HasErrors
	{
		get
		{
			if (_rowError.Length == 0)
			{
				return _count != 0;
			}
			return true;
		}
	}

	internal DataError()
	{
	}

	internal DataError(string rowError)
	{
		SetText(rowError);
	}

	internal void SetColumnError(DataColumn column, string error)
	{
		if (error == null || error.Length == 0)
		{
			Clear(column);
			return;
		}
		if (_errorList == null)
		{
			_errorList = new ColumnError[1];
		}
		int num = IndexOf(column);
		_errorList[num]._column = column;
		_errorList[num]._error = error;
		column._errors++;
		if (num == _count)
		{
			_count++;
		}
	}

	internal string GetColumnError(DataColumn column)
	{
		for (int i = 0; i < _count; i++)
		{
			if (_errorList[i]._column == column)
			{
				return _errorList[i]._error;
			}
		}
		return string.Empty;
	}

	internal void Clear(DataColumn column)
	{
		if (_count == 0)
		{
			return;
		}
		for (int i = 0; i < _count; i++)
		{
			if (_errorList[i]._column == column)
			{
				Array.Copy(_errorList, i + 1, _errorList, i, _count - i - 1);
				_count--;
				column._errors--;
			}
		}
	}

	internal void Clear()
	{
		for (int i = 0; i < _count; i++)
		{
			_errorList[i]._column._errors--;
		}
		_count = 0;
		_rowError = string.Empty;
	}

	internal DataColumn[] GetColumnsInError()
	{
		DataColumn[] array = new DataColumn[_count];
		for (int i = 0; i < _count; i++)
		{
			array[i] = _errorList[i]._column;
		}
		return array;
	}

	private void SetText(string errorText)
	{
		if (errorText == null)
		{
			errorText = string.Empty;
		}
		_rowError = errorText;
	}

	internal int IndexOf(DataColumn column)
	{
		for (int i = 0; i < _count; i++)
		{
			if (_errorList[i]._column == column)
			{
				return i;
			}
		}
		if (_count >= _errorList.Length)
		{
			ColumnError[] array = new ColumnError[Math.Min(_count * 2, column.Table.Columns.Count)];
			Array.Copy(_errorList, 0, array, 0, _count);
			_errorList = array;
		}
		return _count;
	}
}
