using System.Data.ProviderBase;

namespace System.Data.SqlClient;

internal sealed class SqlReferenceCollection : DbReferenceCollection
{
	internal const int DataReaderTag = 1;

	internal const int CommandTag = 2;

	internal const int BulkCopyTag = 3;

	public override void Add(object value, int tag)
	{
		AddItem(value, tag);
	}

	internal void Deactivate()
	{
		Notify(0);
	}

	internal SqlDataReader FindLiveReader(SqlCommand command)
	{
		if (command == null)
		{
			return FindItem(1, (SqlDataReader dataReader) => !dataReader.IsClosed);
		}
		return FindItem(1, (SqlDataReader dataReader) => !dataReader.IsClosed && command == dataReader.Command);
	}

	internal SqlCommand FindLiveCommand(TdsParserStateObject stateObj)
	{
		return FindItem(2, (SqlCommand command) => command.StateObject == stateObj);
	}

	protected override void NotifyItem(int message, int tag, object value)
	{
		switch (tag)
		{
		case 1:
		{
			SqlDataReader sqlDataReader = (SqlDataReader)value;
			if (!sqlDataReader.IsClosed)
			{
				sqlDataReader.CloseReaderFromConnection();
			}
			break;
		}
		case 2:
			((SqlCommand)value).OnConnectionClosed();
			break;
		case 3:
			((SqlBulkCopy)value).OnConnectionClosed();
			break;
		}
	}

	public override void Remove(object value)
	{
		RemoveItem(value);
	}
}
