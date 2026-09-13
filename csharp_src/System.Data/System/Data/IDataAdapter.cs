namespace System.Data;

public interface IDataAdapter
{
	MissingMappingAction MissingMappingAction { get; set; }

	MissingSchemaAction MissingSchemaAction { get; set; }

	ITableMappingCollection TableMappings { get; }

	DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType);

	int Fill(DataSet dataSet);

	IDataParameter[] GetFillParameters();

	int Update(DataSet dataSet);
}
