using System.Data.Common;
using System.Globalization;

namespace System.Data.Sql;

public sealed class SqlDataSourceEnumerator : DbDataSourceEnumerator
{
	private static readonly SqlDataSourceEnumerator SingletonInstance = new SqlDataSourceEnumerator();

	internal const string ServerName = "ServerName";

	internal const string InstanceName = "InstanceName";

	internal const string IsClustered = "IsClustered";

	internal const string Version = "Version";

	private long timeoutTime;

	private static string _Version = "Version:";

	private static string _Cluster = "Clustered:";

	private static int _clusterLength = _Cluster.Length;

	private static int _versionLength = _Version.Length;

	public static SqlDataSourceEnumerator Instance => SingletonInstance;

	private SqlDataSourceEnumerator()
	{
	}

	public override DataTable GetDataSources()
	{
		timeoutTime = 0L;
		throw new NotImplementedException();
	}

	private static DataTable ParseServerEnumString(string serverInstances)
	{
		DataTable dataTable = new DataTable("SqlDataSources");
		dataTable.Locale = CultureInfo.InvariantCulture;
		dataTable.Columns.Add("ServerName", typeof(string));
		dataTable.Columns.Add("InstanceName", typeof(string));
		dataTable.Columns.Add("IsClustered", typeof(string));
		dataTable.Columns.Add("Version", typeof(string));
		DataRow dataRow = null;
		string text = null;
		string text2 = null;
		string text3 = null;
		string value = null;
		string[] array = serverInstances.Split('\0');
		for (int i = 0; i < array.Length; i++)
		{
			string text4 = array[i].Trim('\0');
			if (text4.Length == 0)
			{
				continue;
			}
			string[] array2 = text4.Split(';');
			foreach (string text5 in array2)
			{
				if (text == null)
				{
					string[] array3 = text5.Split('\\');
					foreach (string text6 in array3)
					{
						if (text == null)
						{
							text = text6;
						}
						else
						{
							text2 = text6;
						}
					}
				}
				else if (text3 == null)
				{
					text3 = text5.Substring(_clusterLength);
				}
				else
				{
					value = text5.Substring(_versionLength);
				}
			}
			string text7 = "ServerName='" + text + "'";
			if (!ADP.IsEmpty(text2))
			{
				text7 = text7 + " AND InstanceName='" + text2 + "'";
			}
			if (dataTable.Select(text7).Length == 0)
			{
				dataRow = dataTable.NewRow();
				dataRow[0] = text;
				dataRow[1] = text2;
				dataRow[2] = text3;
				dataRow[3] = value;
				dataTable.Rows.Add(dataRow);
			}
			text = null;
			text2 = null;
			text3 = null;
			value = null;
		}
		foreach (DataColumn column in dataTable.Columns)
		{
			column.ReadOnly = true;
		}
		return dataTable;
	}
}
