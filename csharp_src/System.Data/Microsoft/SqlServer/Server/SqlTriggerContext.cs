using System.Data.Common;
using System.Data.SqlTypes;

namespace Microsoft.SqlServer.Server;

public sealed class SqlTriggerContext
{
	private TriggerAction _triggerAction;

	private bool[] _columnsUpdated;

	private SqlXml _eventInstanceData;

	public int ColumnCount
	{
		get
		{
			int result = 0;
			if (_columnsUpdated != null)
			{
				result = _columnsUpdated.Length;
			}
			return result;
		}
	}

	public SqlXml EventData => _eventInstanceData;

	public TriggerAction TriggerAction => _triggerAction;

	internal SqlTriggerContext(TriggerAction triggerAction, bool[] columnsUpdated, SqlXml eventInstanceData)
	{
		_triggerAction = triggerAction;
		_columnsUpdated = columnsUpdated;
		_eventInstanceData = eventInstanceData;
	}

	public bool IsUpdatedColumn(int columnOrdinal)
	{
		if (_columnsUpdated != null)
		{
			return _columnsUpdated[columnOrdinal];
		}
		throw ADP.IndexOutOfRange(columnOrdinal);
	}
}
