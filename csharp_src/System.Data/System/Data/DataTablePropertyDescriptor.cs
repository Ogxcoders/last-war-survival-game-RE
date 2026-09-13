using System.ComponentModel;

namespace System.Data;

internal sealed class DataTablePropertyDescriptor : PropertyDescriptor
{
	public DataTable Table { get; }

	public override Type ComponentType => typeof(DataRowView);

	public override bool IsReadOnly => false;

	public override Type PropertyType => typeof(IBindingList);

	internal DataTablePropertyDescriptor(DataTable dataTable)
		: base(dataTable.TableName, null)
	{
		Table = dataTable;
	}

	public override bool Equals(object other)
	{
		if (other is DataTablePropertyDescriptor)
		{
			return ((DataTablePropertyDescriptor)other).Table == Table;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Table.GetHashCode();
	}

	public override bool CanResetValue(object component)
	{
		return false;
	}

	public override object GetValue(object component)
	{
		return ((DataViewManagerListItemTypeDescriptor)component).GetDataView(Table);
	}

	public override void ResetValue(object component)
	{
	}

	public override void SetValue(object component, object value)
	{
	}

	public override bool ShouldSerializeValue(object component)
	{
		return false;
	}
}
