using System.ComponentModel;
using System.Data.Common;
using System.Globalization;

namespace System.Data;

internal static class ExceptionBuilder
{
	private static void TraceException(string trace, Exception e)
	{
		if (e != null)
		{
			DataCommonEventSource.Log.Trace(trace, e);
		}
	}

	internal static Exception TraceExceptionAsReturnValue(Exception e)
	{
		TraceException("<comm.ADP.TraceException|ERR|THROW> '{0}'", e);
		return e;
	}

	internal static Exception TraceExceptionForCapture(Exception e)
	{
		TraceException("<comm.ADP.TraceException|ERR|CATCH> '{0}'", e);
		return e;
	}

	internal static Exception TraceExceptionWithoutRethrow(Exception e)
	{
		TraceException("<comm.ADP.TraceException|ERR|CATCH> '{0}'", e);
		return e;
	}

	internal static Exception _Argument(string error)
	{
		return TraceExceptionAsReturnValue(new ArgumentException(error));
	}

	internal static Exception _Argument(string paramName, string error)
	{
		return TraceExceptionAsReturnValue(new ArgumentException(error));
	}

	internal static Exception _Argument(string error, Exception innerException)
	{
		return TraceExceptionAsReturnValue(new ArgumentException(error, innerException));
	}

	private static Exception _ArgumentNull(string paramName, string msg)
	{
		return TraceExceptionAsReturnValue(new ArgumentNullException(paramName, msg));
	}

	internal static Exception _ArgumentOutOfRange(string paramName, string msg)
	{
		return TraceExceptionAsReturnValue(new ArgumentOutOfRangeException(paramName, msg));
	}

	private static Exception _IndexOutOfRange(string error)
	{
		return TraceExceptionAsReturnValue(new IndexOutOfRangeException(error));
	}

	private static Exception _InvalidOperation(string error)
	{
		return TraceExceptionAsReturnValue(new InvalidOperationException(error));
	}

	private static Exception _InvalidEnumArgumentException(string error)
	{
		return TraceExceptionAsReturnValue(new InvalidEnumArgumentException(error));
	}

	private static Exception _InvalidEnumArgumentException<T>(T value)
	{
		return _InvalidEnumArgumentException(global::SR.Format("The {0} enumeration value, {1}, is invalid.", typeof(T).Name, value.ToString()));
	}

	private static void ThrowDataException(string error, Exception innerException)
	{
		throw TraceExceptionAsReturnValue(new DataException(error, innerException));
	}

	private static Exception _Data(string error)
	{
		return TraceExceptionAsReturnValue(new DataException(error));
	}

	private static Exception _Constraint(string error)
	{
		return TraceExceptionAsReturnValue(new ConstraintException(error));
	}

	private static Exception _InvalidConstraint(string error)
	{
		return TraceExceptionAsReturnValue(new InvalidConstraintException(error));
	}

	private static Exception _DeletedRowInaccessible(string error)
	{
		return TraceExceptionAsReturnValue(new DeletedRowInaccessibleException(error));
	}

	private static Exception _DuplicateName(string error)
	{
		return TraceExceptionAsReturnValue(new DuplicateNameException(error));
	}

	private static Exception _InRowChangingEvent(string error)
	{
		return TraceExceptionAsReturnValue(new InRowChangingEventException(error));
	}

	private static Exception _MissingPrimaryKey(string error)
	{
		return TraceExceptionAsReturnValue(new MissingPrimaryKeyException(error));
	}

	private static Exception _NoNullAllowed(string error)
	{
		return TraceExceptionAsReturnValue(new NoNullAllowedException(error));
	}

	private static Exception _ReadOnly(string error)
	{
		return TraceExceptionAsReturnValue(new ReadOnlyException(error));
	}

	private static Exception _RowNotInTable(string error)
	{
		return TraceExceptionAsReturnValue(new RowNotInTableException(error));
	}

	private static Exception _VersionNotFound(string error)
	{
		return TraceExceptionAsReturnValue(new VersionNotFoundException(error));
	}

	public static Exception ArgumentNull(string paramName)
	{
		return _ArgumentNull(paramName, global::SR.Format("'{0}' argument cannot be null.", paramName));
	}

	public static Exception ArgumentOutOfRange(string paramName)
	{
		return _ArgumentOutOfRange(paramName, global::SR.Format("'{0}' argument is out of range.", paramName));
	}

	public static Exception BadObjectPropertyAccess(string error)
	{
		return _InvalidOperation(global::SR.Format("Property not accessible because '{0}'.", error));
	}

	public static Exception ArgumentContainsNull(string paramName)
	{
		return _Argument(paramName, global::SR.Format("'{0}' argument contains null value.", paramName));
	}

	public static Exception CannotModifyCollection()
	{
		return _Argument("Collection itself is not modifiable.");
	}

	public static Exception CaseInsensitiveNameConflict(string name)
	{
		return _Argument(global::SR.Format("The given name '{0}' matches at least two names in the collection object with different cases, but does not match either of them with the same case.", name));
	}

	public static Exception NamespaceNameConflict(string name)
	{
		return _Argument(global::SR.Format("The given name '{0}' matches at least two names in the collection object with different namespaces.", name));
	}

	public static Exception InvalidOffsetLength()
	{
		return _Argument("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
	}

	public static Exception ColumnNotInTheTable(string column, string table)
	{
		return _Argument(global::SR.Format("Column '{0}' does not belong to table {1}.", column, table));
	}

	public static Exception ColumnNotInAnyTable()
	{
		return _Argument("Column must belong to a table.");
	}

	public static Exception ColumnOutOfRange(int index)
	{
		return _IndexOutOfRange(global::SR.Format("Cannot find column {0}.", index.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception ColumnOutOfRange(string column)
	{
		return _IndexOutOfRange(global::SR.Format("Cannot find column {0}.", column));
	}

	public static Exception CannotAddColumn1(string column)
	{
		return _Argument(global::SR.Format("Column '{0}' already belongs to this DataTable.", column));
	}

	public static Exception CannotAddColumn2(string column)
	{
		return _Argument(global::SR.Format("Column '{0}' already belongs to another DataTable.", column));
	}

	public static Exception CannotAddColumn3()
	{
		return _Argument("Cannot have more than one SimpleContent columns in a DataTable.");
	}

	public static Exception CannotAddColumn4(string column)
	{
		return _Argument(global::SR.Format("Cannot add a SimpleContent column to a table containing element columns or nested relations.", column));
	}

	public static Exception CannotAddDuplicate(string column)
	{
		return _DuplicateName(global::SR.Format("A column named '{0}' already belongs to this DataTable.", column));
	}

	public static Exception CannotAddDuplicate2(string table)
	{
		return _DuplicateName(global::SR.Format("Cannot add a column named '{0}': a nested table with the same name already belongs to this DataTable.", table));
	}

	public static Exception CannotAddDuplicate3(string table)
	{
		return _DuplicateName(global::SR.Format("A column named '{0}' already belongs to this DataTable: cannot set a nested table name to the same name.", table));
	}

	public static Exception CannotRemoveColumn()
	{
		return _Argument("Cannot remove a column that doesn't belong to this table.");
	}

	public static Exception CannotRemovePrimaryKey()
	{
		return _Argument("Cannot remove this column, because it's part of the primary key.");
	}

	public static Exception CannotRemoveChildKey(string relation)
	{
		return _Argument(global::SR.Format("Cannot remove this column, because it is part of the parent key for relationship {0}.", relation));
	}

	public static Exception CannotRemoveConstraint(string constraint, string table)
	{
		return _Argument(global::SR.Format("Cannot remove this column, because it is a part of the constraint {0} on the table {1}.", constraint, table));
	}

	public static Exception CannotRemoveExpression(string column, string expression)
	{
		return _Argument(global::SR.Format("Cannot remove this column, because it is part of an expression: {0} = {1}.", column, expression));
	}

	public static Exception ColumnNotInTheUnderlyingTable(string column, string table)
	{
		return _Argument(global::SR.Format("Column '{0}' does not belong to underlying table '{1}'.", column, table));
	}

	public static Exception InvalidOrdinal(string name, int ordinal)
	{
		return _ArgumentOutOfRange(name, global::SR.Format("Ordinal '{0}' exceeds the maximum number.", ordinal.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception AddPrimaryKeyConstraint()
	{
		return _Argument("Cannot add primary key constraint since primary key is already set for the table.");
	}

	public static Exception NoConstraintName()
	{
		return _Argument("Cannot change the name of a constraint to empty string when it is in the ConstraintCollection.");
	}

	public static Exception ConstraintViolation(string constraint)
	{
		return _Constraint(global::SR.Format("Cannot enforce constraints on constraint {0}.", constraint));
	}

	public static Exception ConstraintNotInTheTable(string constraint)
	{
		return _Argument(global::SR.Format("Constraint '{0}' does not belong to this DataTable.", constraint));
	}

	public static string KeysToString(object[] keys)
	{
		string text = string.Empty;
		for (int i = 0; i < keys.Length; i++)
		{
			text = text + Convert.ToString(keys[i], null) + ((i < keys.Length - 1) ? ", " : string.Empty);
		}
		return text;
	}

	public static string UniqueConstraintViolationText(DataColumn[] columns, object[] values)
	{
		if (columns.Length > 1)
		{
			string text = string.Empty;
			for (int i = 0; i < columns.Length; i++)
			{
				text = text + columns[i].ColumnName + ((i < columns.Length - 1) ? ", " : "");
			}
			return global::SR.Format("Column '{0}' is constrained to be unique.  Value '{1}' is already present.", text, KeysToString(values));
		}
		return global::SR.Format("Column '{0}' is constrained to be unique.  Value '{1}' is already present.", columns[0].ColumnName, Convert.ToString(values[0], null));
	}

	public static Exception ConstraintViolation(DataColumn[] columns, object[] values)
	{
		return _Constraint(UniqueConstraintViolationText(columns, values));
	}

	public static Exception ConstraintOutOfRange(int index)
	{
		return _IndexOutOfRange(global::SR.Format("Cannot find constraint {0}.", index.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception DuplicateConstraint(string constraint)
	{
		return _Data(global::SR.Format("Constraint matches constraint named {0} already in collection.", constraint));
	}

	public static Exception DuplicateConstraintName(string constraint)
	{
		return _DuplicateName(global::SR.Format("A Constraint named '{0}' already belongs to this DataTable.", constraint));
	}

	public static Exception NeededForForeignKeyConstraint(UniqueConstraint key, ForeignKeyConstraint fk)
	{
		return _Argument(global::SR.Format("Cannot remove unique constraint '{0}'. Remove foreign key constraint '{1}' first.", key.ConstraintName, fk.ConstraintName));
	}

	public static Exception UniqueConstraintViolation()
	{
		return _Argument("These columns don't currently have unique values.");
	}

	public static Exception ConstraintForeignTable()
	{
		return _Argument("These columns don't point to this table.");
	}

	public static Exception ConstraintParentValues()
	{
		return _Argument("This constraint cannot be enabled as not all values have corresponding parent values.");
	}

	public static Exception ConstraintAddFailed(DataTable table)
	{
		return _InvalidConstraint(global::SR.Format("This constraint cannot be added since ForeignKey doesn't belong to table {0}.", table.TableName));
	}

	public static Exception ConstraintRemoveFailed()
	{
		return _Argument("Cannot remove a constraint that doesn't belong to this table.");
	}

	public static Exception FailedCascadeDelete(string constraint)
	{
		return _InvalidConstraint(global::SR.Format("Cannot delete this row because constraints are enforced on relation {0}, and deleting this row will strand child rows.", constraint));
	}

	public static Exception FailedCascadeUpdate(string constraint)
	{
		return _InvalidConstraint(global::SR.Format("Cannot make this change because constraints are enforced on relation {0}, and changing this value will strand child rows.", constraint));
	}

	public static Exception FailedClearParentTable(string table, string constraint, string childTable)
	{
		return _InvalidConstraint(global::SR.Format("Cannot clear table {0} because ForeignKeyConstraint {1} enforces constraints and there are child rows in {2}.", table, constraint, childTable));
	}

	public static Exception ForeignKeyViolation(string constraint, object[] keys)
	{
		return _InvalidConstraint(global::SR.Format("ForeignKeyConstraint {0} requires the child key values ({1}) to exist in the parent table.", constraint, KeysToString(keys)));
	}

	public static Exception RemoveParentRow(ForeignKeyConstraint constraint)
	{
		return _InvalidConstraint(global::SR.Format("Cannot remove this row because it has child rows, and constraints on relation {0} are enforced.", constraint.ConstraintName));
	}

	public static string MaxLengthViolationText(string columnName)
	{
		return global::SR.Format("Column '{0}' exceeds the MaxLength limit.", columnName);
	}

	public static string NotAllowDBNullViolationText(string columnName)
	{
		return global::SR.Format("Column '{0}' does not allow DBNull.Value.", columnName);
	}

	public static Exception CantAddConstraintToMultipleNestedTable(string tableName)
	{
		return _Argument(global::SR.Format("Cannot add constraint to DataTable '{0}' which is a child table in two nested relations.", tableName));
	}

	public static Exception AutoIncrementAndExpression()
	{
		return _Argument("Cannot set AutoIncrement property for a computed column.");
	}

	public static Exception AutoIncrementAndDefaultValue()
	{
		return _Argument("Cannot set AutoIncrement property for a column with DefaultValue set.");
	}

	public static Exception AutoIncrementSeed()
	{
		return _Argument("AutoIncrementStep must be a non-zero value.");
	}

	public static Exception CantChangeDataType()
	{
		return _Argument("Cannot change DataType of a column once it has data.");
	}

	public static Exception NullDataType()
	{
		return _Argument("Column requires a valid DataType.");
	}

	public static Exception ColumnNameRequired()
	{
		return _Argument("ColumnName is required when it is part of a DataTable.");
	}

	public static Exception DefaultValueAndAutoIncrement()
	{
		return _Argument("Cannot set a DefaultValue on an AutoIncrement column.");
	}

	public static Exception DefaultValueDataType(string column, Type defaultType, Type columnType, Exception inner)
	{
		if (column.Length != 0)
		{
			return _Argument(global::SR.Format("The DefaultValue for column {0} is of type {1} and cannot be converted to {2}.", column, defaultType.FullName, columnType.FullName), inner);
		}
		return _Argument(global::SR.Format("The DefaultValue for the column is of type {0} and cannot be converted to {1}.", defaultType.FullName, columnType.FullName), inner);
	}

	public static Exception DefaultValueColumnDataType(string column, Type defaultType, Type columnType, Exception inner)
	{
		return _Argument(global::SR.Format("The DefaultValue for column {0} is of type {1}, but the column is of type {2}.", column, defaultType.FullName, columnType.FullName), inner);
	}

	public static Exception ExpressionAndUnique()
	{
		return _Argument("Cannot create an expression on a column that has AutoIncrement or Unique.");
	}

	public static Exception ExpressionAndReadOnly()
	{
		return _Argument("Cannot set expression because column cannot be made ReadOnly.");
	}

	public static Exception ExpressionAndConstraint(DataColumn column, Constraint constraint)
	{
		return _Argument(global::SR.Format("Cannot set Expression property on column {0}, because it is a part of a constraint.", column.ColumnName, constraint.ConstraintName));
	}

	public static Exception ExpressionInConstraint(DataColumn column)
	{
		return _Argument(global::SR.Format("Cannot create a constraint based on Expression column {0}.", column.ColumnName));
	}

	public static Exception ExpressionCircular()
	{
		return _Argument("Cannot set Expression property due to circular reference in the expression.");
	}

	public static Exception NonUniqueValues(string column)
	{
		return _InvalidConstraint(global::SR.Format("Column '{0}' contains non-unique values.", column));
	}

	public static Exception NullKeyValues(string column)
	{
		return _Data(global::SR.Format("Column '{0}' has null values in it.", column));
	}

	public static Exception NullValues(string column)
	{
		return _NoNullAllowed(global::SR.Format("Column '{0}' does not allow nulls.", column));
	}

	public static Exception ReadOnlyAndExpression()
	{
		return _ReadOnly("Cannot change ReadOnly property for the expression column.");
	}

	public static Exception ReadOnly(string column)
	{
		return _ReadOnly(global::SR.Format("Column '{0}' is read only.", column));
	}

	public static Exception UniqueAndExpression()
	{
		return _Argument("Cannot change Unique property for the expression column.");
	}

	public static Exception SetFailed(object value, DataColumn column, Type type, Exception innerException)
	{
		return _Argument(innerException.Message + global::SR.Format("Couldn't store <{0}> in {1} Column.  Expected type is {2}.", value.ToString(), column.ColumnName, type.Name), innerException);
	}

	public static Exception CannotSetToNull(DataColumn column)
	{
		return _Argument(global::SR.Format("Cannot set Column '{0}' to be null. Please use DBNull instead.", column.ColumnName));
	}

	public static Exception LongerThanMaxLength(DataColumn column)
	{
		return _Argument(global::SR.Format("Cannot set column '{0}'. The value violates the MaxLength limit of this column.", column.ColumnName));
	}

	public static Exception CannotSetMaxLength(DataColumn column, int value)
	{
		return _Argument(global::SR.Format("Cannot set Column '{0}' property MaxLength to '{1}'. There is at least one string in the table longer than the new limit.", column.ColumnName, value.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception CannotSetMaxLength2(DataColumn column)
	{
		return _Argument(global::SR.Format("Cannot set Column '{0}' property MaxLength. The Column is SimpleContent.", column.ColumnName));
	}

	public static Exception CannotSetSimpleContentType(string columnName, Type type)
	{
		return _Argument(global::SR.Format("Cannot set Column '{0}' property DataType to {1}. The Column is SimpleContent.", columnName, type));
	}

	public static Exception CannotSetSimpleContent(string columnName, Type type)
	{
		return _Argument(global::SR.Format("Cannot set Column '{0}' property MappingType to SimpleContent. The Column DataType is {1}.", columnName, type));
	}

	public static Exception CannotChangeNamespace(string columnName)
	{
		return _Argument(global::SR.Format("Cannot change the Column '{0}' property Namespace. The Column is SimpleContent.", columnName));
	}

	public static Exception HasToBeStringType(DataColumn column)
	{
		return _Argument(global::SR.Format("MaxLength applies to string data type only. You cannot set Column '{0}' property MaxLength to be non-negative number.", column.ColumnName));
	}

	public static Exception AutoIncrementCannotSetIfHasData(string typeName)
	{
		return _Argument(global::SR.Format("Cannot change AutoIncrement of a DataColumn with type '{0}' once it has data.", typeName));
	}

	public static Exception INullableUDTwithoutStaticNull(string typeName)
	{
		return _Argument(global::SR.Format("Type '{0}' does not contain static Null property or field.", typeName));
	}

	public static Exception IComparableNotImplemented(string typeName)
	{
		return _Data(global::SR.Format(" Type '{0}' does not implement IComparable interface. Comparison cannot be done.", typeName));
	}

	public static Exception UDTImplementsIChangeTrackingButnotIRevertible(string typeName)
	{
		return _InvalidOperation(global::SR.Format("Type '{0}' does not implement IRevertibleChangeTracking; therefore can not proceed with RejectChanges().", typeName));
	}

	public static Exception SetAddedAndModifiedCalledOnnonUnchanged()
	{
		return _InvalidOperation("SetAdded and SetModified can only be called on DataRows with Unchanged DataRowState.");
	}

	public static Exception InvalidDataColumnMapping(Type type)
	{
		return _Argument(global::SR.Format("DataColumn with type '{0}' is a complexType. Can not serialize value of a complex type as Attribute", type.AssemblyQualifiedName));
	}

	public static Exception CannotSetDateTimeModeForNonDateTimeColumns()
	{
		return _InvalidOperation("The DateTimeMode can be set only on DataColumns of type DateTime.");
	}

	public static Exception InvalidDateTimeMode(DataSetDateTime mode)
	{
		return _InvalidEnumArgumentException(mode);
	}

	public static Exception CantChangeDateTimeMode(DataSetDateTime oldValue, DataSetDateTime newValue)
	{
		return _InvalidOperation(global::SR.Format("Cannot change DateTimeMode from '{0}' to '{1}' once the table has data.", oldValue.ToString(), newValue.ToString()));
	}

	public static Exception ColumnTypeNotSupported()
	{
		return ADP.NotSupported("DataSet does not support System.Nullable<>.");
	}

	public static Exception SetFailed(string name)
	{
		return _Data(global::SR.Format("Cannot set {0}.", name));
	}

	public static Exception SetDataSetFailed()
	{
		return _Data("Cannot change DataSet on a DataViewManager that's already the default view for a DataSet.");
	}

	public static Exception SetRowStateFilter()
	{
		return _Data("RowStateFilter cannot show ModifiedOriginals and ModifiedCurrents at the same time.");
	}

	public static Exception CanNotSetDataSet()
	{
		return _Data("Cannot change DataSet property once it is set.");
	}

	public static Exception CanNotUseDataViewManager()
	{
		return _Data("DataSet must be set prior to using DataViewManager.");
	}

	public static Exception CanNotSetTable()
	{
		return _Data("Cannot change Table property once it is set.");
	}

	public static Exception CanNotUse()
	{
		return _Data("DataTable must be set prior to using DataView.");
	}

	public static Exception CanNotBindTable()
	{
		return _Data("Cannot bind to DataTable with no name.");
	}

	public static Exception SetTable()
	{
		return _Data("Cannot change Table property on a DefaultView or a DataView coming from a DataViewManager.");
	}

	public static Exception SetIListObject()
	{
		return _Argument("Cannot set an object into this list.");
	}

	public static Exception AddNewNotAllowNull()
	{
		return _Data("Cannot call AddNew on a DataView where AllowNew is false.");
	}

	public static Exception NotOpen()
	{
		return _Data("DataView is not open.");
	}

	public static Exception CreateChildView()
	{
		return _Argument("The relation is not parented to the table to which this DataView points.");
	}

	public static Exception CanNotDelete()
	{
		return _Data("Cannot delete on a DataSource where AllowDelete is false.");
	}

	public static Exception CanNotEdit()
	{
		return _Data("Cannot edit on a DataSource where AllowEdit is false.");
	}

	public static Exception GetElementIndex(int index)
	{
		return _IndexOutOfRange(global::SR.Format("Index {0} is either negative or above rows count.", index.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception AddExternalObject()
	{
		return _Argument("Cannot add external objects to this list.");
	}

	public static Exception CanNotClear()
	{
		return _Argument("Cannot clear this list.");
	}

	public static Exception InsertExternalObject()
	{
		return _Argument("Cannot insert external objects to this list.");
	}

	public static Exception RemoveExternalObject()
	{
		return _Argument("Cannot remove objects not in the list.");
	}

	public static Exception PropertyNotFound(string property, string table)
	{
		return _Argument(global::SR.Format("{0} is neither a DataColumn nor a DataRelation for table {1}.", property, table));
	}

	public static Exception ColumnToSortIsOutOfRange(string column)
	{
		return _Argument(global::SR.Format("Cannot find column {0}.", column));
	}

	public static Exception KeyTableMismatch()
	{
		return _InvalidConstraint("Cannot create a Key from Columns that belong to different tables.");
	}

	public static Exception KeyNoColumns()
	{
		return _InvalidConstraint("Cannot have 0 columns.");
	}

	public static Exception KeyTooManyColumns(int cols)
	{
		return _InvalidConstraint(global::SR.Format("Cannot have more than {0} columns.", cols.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception KeyDuplicateColumns(string columnName)
	{
		return _InvalidConstraint(global::SR.Format("Cannot create a Key when the same column is listed more than once: '{0}'", columnName));
	}

	public static Exception RelationDataSetMismatch()
	{
		return _InvalidConstraint("Cannot have a relationship between tables in different DataSets.");
	}

	public static Exception NoRelationName()
	{
		return _Argument("RelationName is required when it is part of a DataSet.");
	}

	public static Exception ColumnsTypeMismatch()
	{
		return _InvalidConstraint("Parent Columns and Child Columns don't have type-matching columns.");
	}

	public static Exception KeyLengthMismatch()
	{
		return _Argument("ParentColumns and ChildColumns should be the same length.");
	}

	public static Exception KeyLengthZero()
	{
		return _Argument("ParentColumns and ChildColumns must not be zero length.");
	}

	public static Exception ForeignRelation()
	{
		return _Argument("This relation should connect two tables in this DataSet to be added to this DataSet.");
	}

	public static Exception KeyColumnsIdentical()
	{
		return _InvalidConstraint("ParentKey and ChildKey are identical.");
	}

	public static Exception RelationForeignTable(string t1, string t2)
	{
		return _InvalidConstraint(global::SR.Format("GetChildRows requires a row whose Table is {0}, but the specified row's Table is {1}.", t1, t2));
	}

	public static Exception GetParentRowTableMismatch(string t1, string t2)
	{
		return _InvalidConstraint(global::SR.Format("GetParentRow requires a row whose Table is {0}, but the specified row's Table is {1}.", t1, t2));
	}

	public static Exception SetParentRowTableMismatch(string t1, string t2)
	{
		return _InvalidConstraint(global::SR.Format("SetParentRow requires a child row whose Table is {0}, but the specified row's Table is {1}.", t1, t2));
	}

	public static Exception RelationForeignRow()
	{
		return _Argument("The row doesn't belong to the same DataSet as this relation.");
	}

	public static Exception RelationNestedReadOnly()
	{
		return _Argument("Cannot set the 'Nested' property to false for this relation.");
	}

	public static Exception TableCantBeNestedInTwoTables(string tableName)
	{
		return _Argument(global::SR.Format("The same table '{0}' cannot be the child table in two nested relations.", tableName));
	}

	public static Exception LoopInNestedRelations(string tableName)
	{
		return _Argument(global::SR.Format("The table ({0}) cannot be the child table to itself in nested relations.", tableName));
	}

	public static Exception RelationDoesNotExist()
	{
		return _Argument("This relation doesn't belong to this relation collection.");
	}

	public static Exception ParentRowNotInTheDataSet()
	{
		return _Argument("This relation and child row don't belong to same DataSet.");
	}

	public static Exception ParentOrChildColumnsDoNotHaveDataSet()
	{
		return _InvalidConstraint("Cannot create a DataRelation if Parent or Child Columns are not in a DataSet.");
	}

	public static Exception InValidNestedRelation(string childTableName)
	{
		return _InvalidOperation(global::SR.Format("Nested table '{0}' which inherits its namespace cannot have multiple parent tables in different namespaces.", childTableName));
	}

	public static Exception InvalidParentNamespaceinNestedRelation(string childTableName)
	{
		return _InvalidOperation(global::SR.Format("Nested table '{0}' with empty namespace cannot have multiple parent tables in different namespaces.", childTableName));
	}

	public static Exception RowNotInTheDataSet()
	{
		return _Argument("The row doesn't belong to the same DataSet as this relation.");
	}

	public static Exception RowNotInTheTable()
	{
		return _RowNotInTable("Cannot perform this operation on a row not in the table.");
	}

	public static Exception EditInRowChanging()
	{
		return _InRowChangingEvent("Cannot change a proposed value in the RowChanging event.");
	}

	public static Exception EndEditInRowChanging()
	{
		return _InRowChangingEvent("Cannot call EndEdit() inside an OnRowChanging event.");
	}

	public static Exception BeginEditInRowChanging()
	{
		return _InRowChangingEvent("Cannot call BeginEdit() inside the RowChanging event.");
	}

	public static Exception CancelEditInRowChanging()
	{
		return _InRowChangingEvent("Cannot call CancelEdit() inside an OnRowChanging event.  Throw an exception to cancel this update.");
	}

	public static Exception DeleteInRowDeleting()
	{
		return _InRowChangingEvent("Cannot call Delete inside an OnRowDeleting event.  Throw an exception to cancel this delete.");
	}

	public static Exception ValueArrayLength()
	{
		return _Argument("Input array is longer than the number of columns in this table.");
	}

	public static Exception NoCurrentData()
	{
		return _VersionNotFound("There is no Current data to access.");
	}

	public static Exception NoOriginalData()
	{
		return _VersionNotFound("There is no Original data to access.");
	}

	public static Exception NoProposedData()
	{
		return _VersionNotFound("There is no Proposed data to access.");
	}

	public static Exception RowRemovedFromTheTable()
	{
		return _RowNotInTable("This row has been removed from a table and does not have any data.  BeginEdit() will allow creation of new data in this row.");
	}

	public static Exception DeletedRowInaccessible()
	{
		return _DeletedRowInaccessible("Deleted row information cannot be accessed through the row.");
	}

	public static Exception RowAlreadyDeleted()
	{
		return _DeletedRowInaccessible("Cannot delete this row since it's already deleted.");
	}

	public static Exception RowEmpty()
	{
		return _Argument("This row is empty.");
	}

	public static Exception InvalidRowVersion()
	{
		return _Data("Version must be Original, Current, or Proposed.");
	}

	public static Exception RowOutOfRange()
	{
		return _IndexOutOfRange("The given DataRow is not in the current DataRowCollection.");
	}

	public static Exception RowOutOfRange(int index)
	{
		return _IndexOutOfRange(global::SR.Format("There is no row at position {0}.", index.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception RowInsertOutOfRange(int index)
	{
		return _IndexOutOfRange(global::SR.Format("The row insert position {0} is invalid.", index.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception RowInsertTwice(int index, string tableName)
	{
		return _IndexOutOfRange(global::SR.Format("The rowOrder value={0} has been found twice for table named '{1}'.", index.ToString(CultureInfo.InvariantCulture), tableName));
	}

	public static Exception RowInsertMissing(string tableName)
	{
		return _IndexOutOfRange(global::SR.Format("Values are missing in the rowOrder sequence for table '{0}'.", tableName));
	}

	public static Exception RowAlreadyRemoved()
	{
		return _Data("Cannot remove a row that's already been removed.");
	}

	public static Exception MultipleParents()
	{
		return _Data("A child row has multiple parents.");
	}

	public static Exception InvalidRowState(DataRowState state)
	{
		return _InvalidEnumArgumentException(state);
	}

	public static Exception InvalidRowBitPattern()
	{
		return _Argument("Unrecognized row state bit pattern.");
	}

	internal static Exception SetDataSetNameToEmpty()
	{
		return _Argument("Cannot change the name of the DataSet to an empty string.");
	}

	internal static Exception SetDataSetNameConflicting(string name)
	{
		return _Argument(global::SR.Format("The name '{0}' is invalid. A DataSet cannot have the same name of the DataTable.", name));
	}

	public static Exception DataSetUnsupportedSchema(string ns)
	{
		return _Argument(global::SR.Format("The schema namespace is invalid. Please use this one instead: {0}.", ns));
	}

	public static Exception MergeMissingDefinition(string obj)
	{
		return _Argument(global::SR.Format("Target DataSet missing definition for {0}.", obj));
	}

	public static Exception TablesInDifferentSets()
	{
		return _Argument("Cannot create a relation between tables in different DataSets.");
	}

	public static Exception RelationAlreadyExists()
	{
		return _Argument("A relation already exists for these child columns.");
	}

	public static Exception RowAlreadyInOtherCollection()
	{
		return _Argument("This row already belongs to another table.");
	}

	public static Exception RowAlreadyInTheCollection()
	{
		return _Argument("This row already belongs to this table.");
	}

	public static Exception TableMissingPrimaryKey()
	{
		return _MissingPrimaryKey("Table doesn't have a primary key.");
	}

	public static Exception RecordStateRange()
	{
		return _Argument("The RowStates parameter must be set to a valid combination of values from the DataViewRowState enumeration.");
	}

	public static Exception IndexKeyLength(int length, int keyLength)
	{
		if (length != 0)
		{
			return _Argument(global::SR.Format("Expecting {0} value(s) for the key being indexed, but received {1} value(s).", length.ToString(CultureInfo.InvariantCulture), keyLength.ToString(CultureInfo.InvariantCulture)));
		}
		return _Argument("Find finds a row based on a Sort order, and no Sort order is specified.");
	}

	public static Exception RemovePrimaryKey(DataTable table)
	{
		if (table.TableName.Length != 0)
		{
			return _Argument(global::SR.Format("Cannot remove unique constraint since it's the primary key of table {0}.", table.TableName));
		}
		return _Argument("Cannot remove unique constraint since it's the primary key of a table.");
	}

	public static Exception RelationAlreadyInOtherDataSet()
	{
		return _Argument("This relation already belongs to another DataSet.");
	}

	public static Exception RelationAlreadyInTheDataSet()
	{
		return _Argument("This relation already belongs to this DataSet.");
	}

	public static Exception RelationNotInTheDataSet(string relation)
	{
		return _Argument(global::SR.Format("Relation {0} does not belong to this DataSet.", relation));
	}

	public static Exception RelationOutOfRange(object index)
	{
		return _IndexOutOfRange(global::SR.Format("Cannot find relation {0}.", Convert.ToString(index, null)));
	}

	public static Exception DuplicateRelation(string relation)
	{
		return _DuplicateName(global::SR.Format("A Relation named '{0}' already belongs to this DataSet.", relation));
	}

	public static Exception RelationTableNull()
	{
		return _Argument("Cannot create a collection on a null table.");
	}

	public static Exception RelationDataSetNull()
	{
		return _Argument("Cannot create a collection on a null table.");
	}

	public static Exception RelationTableWasRemoved()
	{
		return _Argument("The table this collection displays relations for has been removed from its DataSet.");
	}

	public static Exception ParentTableMismatch()
	{
		return _Argument("Cannot add a relation to this table's ChildRelation collection where this table isn't the parent table.");
	}

	public static Exception ChildTableMismatch()
	{
		return _Argument("Cannot add a relation to this table's ParentRelation collection where this table isn't the child table.");
	}

	public static Exception EnforceConstraint()
	{
		return _Constraint("Failed to enable constraints. One or more rows contain values violating non-null, unique, or foreign-key constraints.");
	}

	public static Exception CaseLocaleMismatch()
	{
		return _Argument("Cannot add a DataRelation or Constraint that has different Locale or CaseSensitive settings between its parent and child tables.");
	}

	public static Exception CannotChangeCaseLocale()
	{
		return CannotChangeCaseLocale(null);
	}

	public static Exception CannotChangeCaseLocale(Exception innerException)
	{
		return _Argument("Cannot change CaseSensitive or Locale property. This change would lead to at least one DataRelation or Constraint to have different Locale or CaseSensitive settings between its related tables.", innerException);
	}

	public static Exception CannotChangeSchemaSerializationMode()
	{
		return _InvalidOperation("SchemaSerializationMode property can be set only if it is overridden by derived DataSet.");
	}

	public static Exception InvalidSchemaSerializationMode(Type enumType, string mode)
	{
		return _InvalidEnumArgumentException(global::SR.Format("The {0} enumeration value, {1}, is invalid.", enumType.Name, mode));
	}

	public static Exception InvalidRemotingFormat(SerializationFormat mode)
	{
		return _InvalidEnumArgumentException(mode);
	}

	public static Exception TableForeignPrimaryKey()
	{
		return _Argument("PrimaryKey columns do not belong to this table.");
	}

	public static Exception TableCannotAddToSimpleContent()
	{
		return _Argument("Cannot add a nested relation or an element column to a table containing a SimpleContent column.");
	}

	public static Exception NoTableName()
	{
		return _Argument("TableName is required when it is part of a DataSet.");
	}

	public static Exception MultipleTextOnlyColumns()
	{
		return _Argument("DataTable already has a simple content column.");
	}

	public static Exception InvalidSortString(string sort)
	{
		return _Argument(global::SR.Format(" {0} isn't a valid Sort string entry.", sort));
	}

	public static Exception DuplicateTableName(string table)
	{
		return _DuplicateName(global::SR.Format("A DataTable named '{0}' already belongs to this DataSet.", table));
	}

	public static Exception DuplicateTableName2(string table, string ns)
	{
		return _DuplicateName(global::SR.Format("A DataTable named '{0}' with the same Namespace '{1}' already belongs to this DataSet.", table, ns));
	}

	public static Exception SelfnestedDatasetConflictingName(string table)
	{
		return _DuplicateName(global::SR.Format("The table ({0}) cannot be the child table to itself in a nested relation: the DataSet name conflicts with the table name.", table));
	}

	public static Exception DatasetConflictingName(string table)
	{
		return _DuplicateName(global::SR.Format("The name '{0}' is invalid. A DataTable cannot have the same name of the DataSet.", table));
	}

	public static Exception TableAlreadyInOtherDataSet()
	{
		return _Argument("DataTable already belongs to another DataSet.");
	}

	public static Exception TableAlreadyInTheDataSet()
	{
		return _Argument("DataTable already belongs to this DataSet.");
	}

	public static Exception TableOutOfRange(int index)
	{
		return _IndexOutOfRange(global::SR.Format("Cannot find table {0}.", index.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception TableNotInTheDataSet(string table)
	{
		return _Argument(global::SR.Format("Table {0} does not belong to this DataSet.", table));
	}

	public static Exception TableInRelation()
	{
		return _Argument("Cannot remove a table that has existing relations.  Remove relations first.");
	}

	public static Exception TableInConstraint(DataTable table, Constraint constraint)
	{
		return _Argument(global::SR.Format("Cannot remove table {0}, because it referenced in ForeignKeyConstraint {1}.  Remove the constraint first.", table.TableName, constraint.ConstraintName));
	}

	public static Exception CanNotSerializeDataTableHierarchy()
	{
		return _InvalidOperation("Cannot serialize the DataTable. A DataTable being used in one or more DataColumn expressions is not a descendant of current DataTable.");
	}

	public static Exception CanNotRemoteDataTable()
	{
		return _InvalidOperation("This DataTable can only be remoted as part of DataSet. One or more Expression Columns has reference to other DataTable(s).");
	}

	public static Exception CanNotSetRemotingFormat()
	{
		return _Argument("Cannot have different remoting format property value for DataSet and DataTable.");
	}

	public static Exception CanNotSerializeDataTableWithEmptyName()
	{
		return _InvalidOperation("Cannot serialize the DataTable. DataTable name is not set.");
	}

	public static Exception TableNotFound(string tableName)
	{
		return _Argument(global::SR.Format("DataTable '{0}' does not match to any DataTable in source.", tableName));
	}

	public static Exception AggregateException(AggregateType aggregateType, Type type)
	{
		return _Data(global::SR.Format("Invalid usage of aggregate function {0}() and Type: {1}.", aggregateType.ToString(), type.Name));
	}

	public static Exception InvalidStorageType(TypeCode typecode)
	{
		return _Data(global::SR.Format("Invalid storage type: {0}.", typecode.ToString()));
	}

	public static Exception RangeArgument(int min, int max)
	{
		return _Argument(global::SR.Format("Min ({0}) must be less than or equal to max ({1}) in a Range object.", min.ToString(CultureInfo.InvariantCulture), max.ToString(CultureInfo.InvariantCulture)));
	}

	public static Exception NullRange()
	{
		return _Data("This is a null range.");
	}

	public static Exception NegativeMinimumCapacity()
	{
		return _Argument("MinimumCapacity must be non-negative.");
	}

	public static Exception ProblematicChars(char charValue)
	{
		ushort num = charValue;
		return _Argument(global::SR.Format("The DataSet Xml persistency does not support the value '{0}' as Char value, please use Byte storage instead.", "0x" + num.ToString("X", CultureInfo.InvariantCulture)));
	}

	public static Exception StorageSetFailed()
	{
		return _Argument("Type of value has a mismatch with column type");
	}

	public static Exception SimpleTypeNotSupported()
	{
		return _Data("DataSet doesn't support 'union' or 'list' as simpleType.");
	}

	public static Exception MissingAttribute(string attribute)
	{
		return MissingAttribute(string.Empty, attribute);
	}

	public static Exception MissingAttribute(string element, string attribute)
	{
		return _Data(global::SR.Format("Invalid {0} syntax: missing required '{1}' attribute.", element, attribute));
	}

	public static Exception InvalidAttributeValue(string name, string value)
	{
		return _Data(global::SR.Format("Value '{1}' is invalid for attribute '{0}'.", name, value));
	}

	public static Exception AttributeValues(string name, string value1, string value2)
	{
		return _Data(global::SR.Format("The value of attribute '{0}' should be '{1}' or '{2}'.", name, value1, value2));
	}

	public static Exception ElementTypeNotFound(string name)
	{
		return _Data(global::SR.Format("Cannot find ElementType name='{0}'.", name));
	}

	public static Exception RelationParentNameMissing(string rel)
	{
		return _Data(global::SR.Format("Parent table name is missing in relation '{0}'.", rel));
	}

	public static Exception RelationChildNameMissing(string rel)
	{
		return _Data(global::SR.Format("Child table name is missing in relation '{0}'.", rel));
	}

	public static Exception RelationTableKeyMissing(string rel)
	{
		return _Data(global::SR.Format("Parent table key is missing in relation '{0}'.", rel));
	}

	public static Exception RelationChildKeyMissing(string rel)
	{
		return _Data(global::SR.Format("Child table key is missing in relation '{0}'.", rel));
	}

	public static Exception UndefinedDatatype(string name)
	{
		return _Data(global::SR.Format("Undefined data type: '{0}'.", name));
	}

	public static Exception DatatypeNotDefined()
	{
		return _Data("Data type not defined.");
	}

	public static Exception MismatchKeyLength()
	{
		return _Data("Invalid Relation definition: different length keys.");
	}

	public static Exception InvalidField(string name)
	{
		return _Data(global::SR.Format("Invalid XPath selection inside field node. Cannot find: {0}.", name));
	}

	public static Exception InvalidSelector(string name)
	{
		return _Data(global::SR.Format("Invalid XPath selection inside selector node: {0}.", name));
	}

	public static Exception CircularComplexType(string name)
	{
		return _Data(global::SR.Format("DataSet doesn't allow the circular reference in the ComplexType named '{0}'.", name));
	}

	public static Exception CannotInstantiateAbstract(string name)
	{
		return _Data(global::SR.Format("DataSet cannot instantiate an abstract ComplexType for the node {0}.", name));
	}

	public static Exception InvalidKey(string name)
	{
		return _Data(global::SR.Format("Invalid 'Key' node inside constraint named: {0}.", name));
	}

	public static Exception DiffgramMissingTable(string name)
	{
		return _Data(global::SR.Format("Cannot load diffGram. Table '{0}' is missing in the destination dataset.", name));
	}

	public static Exception DiffgramMissingSQL()
	{
		return _Data("Cannot load diffGram. The 'sql' node is missing.");
	}

	public static Exception DuplicateConstraintRead(string str)
	{
		return _Data(global::SR.Format("The constraint name {0} is already used in the schema.", str));
	}

	public static Exception ColumnTypeConflict(string name)
	{
		return _Data(global::SR.Format("Column name '{0}' is defined for different mapping types.", name));
	}

	public static Exception CannotConvert(string name, string type)
	{
		return _Data(global::SR.Format(" Cannot convert '{0}' to type '{1}'.", name, type));
	}

	public static Exception MissingRefer(string name)
	{
		return _Data(global::SR.Format("Missing '{0}' part in '{1}' constraint named '{2}'.", "refer", "keyref", name));
	}

	public static Exception InvalidPrefix(string name)
	{
		return _Data(global::SR.Format("Prefix '{0}' is not valid, because it contains special characters.", name));
	}

	public static Exception CanNotDeserializeObjectType()
	{
		return _InvalidOperation("Unable to proceed with deserialization. Data does not implement IXMLSerializable, therefore polymorphism is not supported.");
	}

	public static Exception IsDataSetAttributeMissingInSchema()
	{
		return _Data("IsDataSet attribute is missing in input Schema.");
	}

	public static Exception TooManyIsDataSetAtributeInSchema()
	{
		return _Data("Cannot determine the DataSet Element. IsDataSet attribute exist more than once.");
	}

	public static Exception NestedCircular(string name)
	{
		return _Data(global::SR.Format("Circular reference in self-nested table '{0}'.", name));
	}

	public static Exception MultipleParentRows(string tableQName)
	{
		return _Data(global::SR.Format("Cannot proceed with serializing DataTable '{0}'. It contains a DataRow which has multiple parent rows on the same Foreign Key.", tableQName));
	}

	public static Exception PolymorphismNotSupported(string typeName)
	{
		return _InvalidOperation(global::SR.Format("Type '{0}' does not implement IXmlSerializable interface therefore can not proceed with serialization.", typeName));
	}

	public static Exception DataTableInferenceNotSupported()
	{
		return _InvalidOperation("DataTable does not support schema inference from Xml.");
	}

	internal static void ThrowMultipleTargetConverter(Exception innerException)
	{
		ThrowDataException((innerException != null) ? "An error occurred with the multiple target converter while writing an Xml Schema.  See the inner exception for details." : "An error occurred with the multiple target converter while writing an Xml Schema.  A null or empty string was returned.", innerException);
	}

	public static Exception DuplicateDeclaration(string name)
	{
		return _Data(global::SR.Format("Duplicated declaration '{0}'.", name));
	}

	public static Exception FoundEntity()
	{
		return _Data("DataSet cannot expand entities. Use XmlValidatingReader and set the EntityHandling property accordingly.");
	}

	public static Exception MergeFailed(string name)
	{
		return _Data(name);
	}

	public static Exception ConvertFailed(Type type1, Type type2)
	{
		return _Data(global::SR.Format(" Cannot convert object of type '{0}' to object of type '{1}'.", type1.FullName, type2.FullName));
	}

	public static Exception InvalidDataTableReader(string tableName)
	{
		return _InvalidOperation(global::SR.Format("DataTableReader is invalid for current DataTable '{0}'.", tableName));
	}

	public static Exception DataTableReaderSchemaIsInvalid(string tableName)
	{
		return _InvalidOperation(global::SR.Format("Schema of current DataTable '{0}' in DataTableReader has changed, DataTableReader is invalid.", tableName));
	}

	public static Exception CannotCreateDataReaderOnEmptyDataSet()
	{
		return _Argument("DataTableReader Cannot be created. There is no DataTable in DataSet.");
	}

	public static Exception DataTableReaderArgumentIsEmpty()
	{
		return _Argument("Cannot create DataTableReader. Argument is Empty.");
	}

	public static Exception ArgumentContainsNullValue()
	{
		return _Argument("Cannot create DataTableReader. Arguments contain null value.");
	}

	public static Exception InvalidCurrentRowInDataTableReader()
	{
		return _DeletedRowInaccessible("Current DataRow is either in Deleted or Detached state.");
	}

	public static Exception EmptyDataTableReader(string tableName)
	{
		return _DeletedRowInaccessible(global::SR.Format("Current DataTable '{0}' is empty. There is no DataRow in DataTable.", tableName));
	}

	internal static Exception InvalidDuplicateNamedSimpleTypeDelaration(string stName, string errorStr)
	{
		return _Argument(global::SR.Format("Simple type '{0}' has already be declared with different '{1}'.", stName, errorStr));
	}

	internal static Exception InternalRBTreeError(RBTreeError internalError)
	{
		return _InvalidOperation(global::SR.Format("DataTable internal index is corrupted: '{0}'.", (int)internalError));
	}

	public static Exception EnumeratorModified()
	{
		return _InvalidOperation("Collection was modified; enumeration operation might not execute.");
	}
}
