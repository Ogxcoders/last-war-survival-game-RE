using System.Collections;
using System.ComponentModel;
using System.Data.Common;

namespace System.Data;

internal sealed class DataColumnPropertyDescriptor : PropertyDescriptor
{
	public override AttributeCollection Attributes
	{
		get
		{
			if (typeof(IList).IsAssignableFrom(PropertyType))
			{
				Attribute[] array = new Attribute[base.Attributes.Count + 1];
				base.Attributes.CopyTo(array, 0);
				array[^1] = new ListBindableAttribute(listBindable: false);
				return new AttributeCollection(array);
			}
			return base.Attributes;
		}
	}

	internal DataColumn Column { get; }

	public override Type ComponentType => typeof(DataRowView);

	public override bool IsReadOnly => Column.ReadOnly;

	public override Type PropertyType => Column.DataType;

	public override bool IsBrowsable
	{
		get
		{
			if (Column.ColumnMapping != MappingType.Hidden)
			{
				return base.IsBrowsable;
			}
			return false;
		}
	}

	internal DataColumnPropertyDescriptor(DataColumn dataColumn)
		: base(dataColumn.ColumnName, null)
	{
		Column = dataColumn;
	}

	public override bool Equals(object other)
	{
		if (other is DataColumnPropertyDescriptor)
		{
			return ((DataColumnPropertyDescriptor)other).Column == Column;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Column.GetHashCode();
	}

	public override bool CanResetValue(object component)
	{
		DataRowView dataRowView = (DataRowView)component;
		if (!Column.IsSqlType)
		{
			return dataRowView.GetColumnValue(Column) != DBNull.Value;
		}
		return !DataStorage.IsObjectNull(dataRowView.GetColumnValue(Column));
	}

	public override object GetValue(object component)
	{
		return ((DataRowView)component).GetColumnValue(Column);
	}

	public override void ResetValue(object component)
	{
		((DataRowView)component).SetColumnValue(Column, DBNull.Value);
	}

	public override void SetValue(object component, object value)
	{
		((DataRowView)component).SetColumnValue(Column, value);
		OnValueChanged(component, EventArgs.Empty);
	}

	public override bool ShouldSerializeValue(object component)
	{
		return false;
	}
}
