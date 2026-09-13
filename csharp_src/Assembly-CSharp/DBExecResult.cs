using System.Collections.Generic;

public class DBExecResult
{
	public int error;

	public int errorcode;

	public string errormsg;

	public string errorsql;

	public int change_rows;

	public long last_insertid;

	public int col_count;

	public DBColumn[] cols;

	public List<DBAnyValue[]> values;
}
