using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SqlClient;

public sealed class SqlParameterCollection : DbParameterCollection, ICollection, IEnumerable, IList, IDataParameterCollection
{
	private bool _isDirty;

	private static Type s_itemType = typeof(SqlParameter);

	private List<SqlParameter> _items;

	internal bool IsDirty
	{
		get
		{
			return _isDirty;
		}
		set
		{
			_isDirty = value;
		}
	}

	public override bool IsFixedSize => ((IList)InnerList).IsFixedSize;

	public override bool IsReadOnly => ((IList)InnerList).IsReadOnly;

	public new SqlParameter this[int index]
	{
		get
		{
			return (SqlParameter)GetParameter(index);
		}
		set
		{
			SetParameter(index, value);
		}
	}

	public new SqlParameter this[string parameterName]
	{
		get
		{
			return (SqlParameter)GetParameter(parameterName);
		}
		set
		{
			SetParameter(parameterName, value);
		}
	}

	public override int Count
	{
		get
		{
			if (_items == null)
			{
				return 0;
			}
			return _items.Count;
		}
	}

	private List<SqlParameter> InnerList
	{
		get
		{
			List<SqlParameter> list = _items;
			if (list == null)
			{
				list = (_items = new List<SqlParameter>());
			}
			return list;
		}
	}

	public override object SyncRoot => ((ICollection)InnerList).SyncRoot;

	internal SqlParameterCollection()
	{
	}

	public SqlParameter Add(SqlParameter value)
	{
		Add((object)value);
		return value;
	}

	public SqlParameter AddWithValue(string parameterName, object value)
	{
		return Add(new SqlParameter(parameterName, value));
	}

	public SqlParameter Add(string parameterName, SqlDbType sqlDbType)
	{
		return Add(new SqlParameter(parameterName, sqlDbType));
	}

	public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size)
	{
		return Add(new SqlParameter(parameterName, sqlDbType, size));
	}

	public SqlParameter Add(string parameterName, SqlDbType sqlDbType, int size, string sourceColumn)
	{
		return Add(new SqlParameter(parameterName, sqlDbType, size, sourceColumn));
	}

	public void AddRange(SqlParameter[] values)
	{
		AddRange((Array)values);
	}

	public override bool Contains(string value)
	{
		return -1 != IndexOf(value);
	}

	public bool Contains(SqlParameter value)
	{
		return -1 != IndexOf(value);
	}

	public void CopyTo(SqlParameter[] array, int index)
	{
		CopyTo((Array)array, index);
	}

	public int IndexOf(SqlParameter value)
	{
		return IndexOf((object)value);
	}

	public void Insert(int index, SqlParameter value)
	{
		Insert(index, (object)value);
	}

	private void OnChange()
	{
		IsDirty = true;
	}

	public void Remove(SqlParameter value)
	{
		Remove((object)value);
	}

	public override int Add(object value)
	{
		OnChange();
		ValidateType(value);
		Validate(-1, value);
		InnerList.Add((SqlParameter)value);
		return Count - 1;
	}

	public override void AddRange(Array values)
	{
		OnChange();
		if (values == null)
		{
			throw ADP.ArgumentNull("values");
		}
		foreach (object value in values)
		{
			ValidateType(value);
		}
		foreach (SqlParameter value2 in values)
		{
			Validate(-1, value2);
			InnerList.Add(value2);
		}
	}

	private int CheckName(string parameterName)
	{
		int num = IndexOf(parameterName);
		if (num < 0)
		{
			throw ADP.ParametersSourceIndex(parameterName, this, s_itemType);
		}
		return num;
	}

	public override void Clear()
	{
		OnChange();
		List<SqlParameter> innerList = InnerList;
		if (innerList == null)
		{
			return;
		}
		foreach (SqlParameter item in innerList)
		{
			item.ResetParent();
		}
		innerList.Clear();
	}

	public override bool Contains(object value)
	{
		return -1 != IndexOf(value);
	}

	public override void CopyTo(Array array, int index)
	{
		((ICollection)InnerList).CopyTo(array, index);
	}

	public override IEnumerator GetEnumerator()
	{
		return ((IEnumerable)InnerList).GetEnumerator();
	}

	protected override DbParameter GetParameter(int index)
	{
		RangeCheck(index);
		return InnerList[index];
	}

	protected override DbParameter GetParameter(string parameterName)
	{
		int num = IndexOf(parameterName);
		if (num < 0)
		{
			throw ADP.ParametersSourceIndex(parameterName, this, s_itemType);
		}
		return InnerList[num];
	}

	private static int IndexOf(IEnumerable items, string parameterName)
	{
		if (items != null)
		{
			int num = 0;
			foreach (SqlParameter item in items)
			{
				if (parameterName == item.ParameterName)
				{
					return num;
				}
				num++;
			}
			num = 0;
			foreach (SqlParameter item2 in items)
			{
				if (ADP.DstCompare(parameterName, item2.ParameterName) == 0)
				{
					return num;
				}
				num++;
			}
		}
		return -1;
	}

	public override int IndexOf(string parameterName)
	{
		return IndexOf(InnerList, parameterName);
	}

	public override int IndexOf(object value)
	{
		if (value != null)
		{
			ValidateType(value);
			List<SqlParameter> innerList = InnerList;
			if (innerList != null)
			{
				int count = innerList.Count;
				for (int i = 0; i < count; i++)
				{
					if (value == innerList[i])
					{
						return i;
					}
				}
			}
		}
		return -1;
	}

	public override void Insert(int index, object value)
	{
		OnChange();
		ValidateType(value);
		Validate(-1, (SqlParameter)value);
		InnerList.Insert(index, (SqlParameter)value);
	}

	private void RangeCheck(int index)
	{
		if (index < 0 || Count <= index)
		{
			throw ADP.ParametersMappingIndex(index, this);
		}
	}

	public override void Remove(object value)
	{
		OnChange();
		ValidateType(value);
		int num = IndexOf(value);
		if (-1 != num)
		{
			RemoveIndex(num);
		}
		else if (this != ((SqlParameter)value).CompareExchangeParent(null, this))
		{
			throw ADP.CollectionRemoveInvalidObject(s_itemType, this);
		}
	}

	public override void RemoveAt(int index)
	{
		OnChange();
		RangeCheck(index);
		RemoveIndex(index);
	}

	public override void RemoveAt(string parameterName)
	{
		OnChange();
		int index = CheckName(parameterName);
		RemoveIndex(index);
	}

	private void RemoveIndex(int index)
	{
		List<SqlParameter> innerList = InnerList;
		SqlParameter sqlParameter = innerList[index];
		innerList.RemoveAt(index);
		sqlParameter.ResetParent();
	}

	private void Replace(int index, object newValue)
	{
		List<SqlParameter> innerList = InnerList;
		ValidateType(newValue);
		Validate(index, newValue);
		SqlParameter sqlParameter = innerList[index];
		innerList[index] = (SqlParameter)newValue;
		sqlParameter.ResetParent();
	}

	protected override void SetParameter(int index, DbParameter value)
	{
		OnChange();
		RangeCheck(index);
		Replace(index, value);
	}

	protected override void SetParameter(string parameterName, DbParameter value)
	{
		OnChange();
		int num = IndexOf(parameterName);
		if (num < 0)
		{
			throw ADP.ParametersSourceIndex(parameterName, this, s_itemType);
		}
		Replace(num, value);
	}

	private void Validate(int index, object value)
	{
		if (value == null)
		{
			throw ADP.ParameterNull("value", this, s_itemType);
		}
		object obj = ((SqlParameter)value).CompareExchangeParent(this, null);
		if (obj != null)
		{
			if (this != obj)
			{
				throw ADP.ParametersIsNotParent(s_itemType, this);
			}
			if (index != IndexOf(value))
			{
				throw ADP.ParametersIsParent(s_itemType, this);
			}
		}
		string parameterName = ((SqlParameter)value).ParameterName;
		if (parameterName.Length == 0)
		{
			index = 1;
			do
			{
				parameterName = "Parameter" + index.ToString(CultureInfo.CurrentCulture);
				index++;
			}
			while (-1 != IndexOf(parameterName));
			((SqlParameter)value).ParameterName = parameterName;
		}
	}

	private void ValidateType(object value)
	{
		if (value == null)
		{
			throw ADP.ParameterNull("value", this, s_itemType);
		}
		if (!s_itemType.IsInstanceOfType(value))
		{
			throw ADP.InvalidParameterType(this, s_itemType, value);
		}
	}

	public SqlParameter Add(string parameterName, object value)
	{
		return Add(new SqlParameter(parameterName, value));
	}
}
