using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace System.Data.SqlClient;

[Serializable]
public sealed class SqlException : DbException
{
	private const string OriginalClientConnectionIdKey = "OriginalClientConnectionId";

	private const string RoutingDestinationKey = "RoutingDestination";

	private const int SqlExceptionHResult = -2146232060;

	private SqlErrorCollection _errors;

	private Guid _clientConnectionId = Guid.Empty;

	internal bool _doNotReconnect;

	private const string DEF_MESSAGE = "SQL Exception has occured.";

	public SqlErrorCollection Errors
	{
		get
		{
			if (_errors == null)
			{
				_errors = new SqlErrorCollection();
			}
			return _errors;
		}
	}

	public Guid ClientConnectionId => _clientConnectionId;

	public byte Class
	{
		get
		{
			if (Errors.Count <= 0)
			{
				return 0;
			}
			return Errors[0].Class;
		}
	}

	public int LineNumber
	{
		get
		{
			if (Errors.Count <= 0)
			{
				return 0;
			}
			return Errors[0].LineNumber;
		}
	}

	public int Number
	{
		get
		{
			if (Errors.Count <= 0)
			{
				return 0;
			}
			return Errors[0].Number;
		}
	}

	public string Procedure
	{
		get
		{
			if (Errors.Count <= 0)
			{
				return null;
			}
			return Errors[0].Procedure;
		}
	}

	public string Server
	{
		get
		{
			if (Errors.Count <= 0)
			{
				return null;
			}
			return Errors[0].Server;
		}
	}

	public byte State
	{
		get
		{
			if (Errors.Count <= 0)
			{
				return 0;
			}
			return Errors[0].State;
		}
	}

	public override string Source
	{
		get
		{
			if (Errors.Count <= 0)
			{
				return null;
			}
			return Errors[0].Source;
		}
	}

	public override string Message
	{
		get
		{
			if (Errors.Count == 0)
			{
				return base.Message;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (base.Message != "SQL Exception has occured.")
			{
				stringBuilder.Append(base.Message);
				stringBuilder.Append("\n");
			}
			for (int i = 0; i < Errors.Count - 1; i++)
			{
				stringBuilder.Append(Errors[i].Message);
				stringBuilder.Append("\n");
			}
			stringBuilder.Append(Errors[Errors.Count - 1].Message);
			return stringBuilder.ToString();
		}
	}

	private SqlException(string message, SqlErrorCollection errorCollection, Exception innerException, Guid conId)
		: base(message, innerException)
	{
		base.HResult = -2146232060;
		_errors = errorCollection;
		_clientConnectionId = conId;
	}

	private SqlException(SerializationInfo si, StreamingContext sc)
		: base(si, sc)
	{
		base.HResult = -2146232060;
		SerializationInfoEnumerator enumerator = si.GetEnumerator();
		while (enumerator.MoveNext())
		{
			SerializationEntry current = enumerator.Current;
			if ("ClientConnectionId" == current.Name)
			{
				_clientConnectionId = (Guid)current.Value;
				break;
			}
		}
	}

	public override void GetObjectData(SerializationInfo si, StreamingContext context)
	{
		base.GetObjectData(si, context);
		si.AddValue("Errors", null);
		si.AddValue("ClientConnectionId", _clientConnectionId, typeof(Guid));
		for (int i = 0; i < Errors.Count; i++)
		{
			string key = "SqlError " + (i + 1);
			if (Data.Contains(key))
			{
				Data.Remove(key);
			}
			Data.Add(key, Errors[i].ToString());
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder(base.ToString());
		stringBuilder.AppendLine();
		stringBuilder.AppendFormat(SQLMessage.ExClientConnectionId(), _clientConnectionId);
		if (Errors.Count > 0 && Number != 0)
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat(SQLMessage.ExErrorNumberStateClass(), Number, State, Class);
		}
		if (Data.Contains("OriginalClientConnectionId"))
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat(SQLMessage.ExOriginalClientConnectionId(), Data["OriginalClientConnectionId"]);
		}
		if (Data.Contains("RoutingDestination"))
		{
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat(SQLMessage.ExRoutingDestination(), Data["RoutingDestination"]);
		}
		return stringBuilder.ToString();
	}

	internal static SqlException CreateException(SqlErrorCollection errorCollection, string serverVersion)
	{
		return CreateException(errorCollection, serverVersion, Guid.Empty);
	}

	internal static SqlException CreateException(SqlErrorCollection errorCollection, string serverVersion, SqlInternalConnectionTds internalConnection, Exception innerException = null)
	{
		Guid conId = internalConnection?._clientConnectionId ?? Guid.Empty;
		SqlException ex = CreateException(errorCollection, serverVersion, conId, innerException);
		if (internalConnection != null)
		{
			if (internalConnection.OriginalClientConnectionId != Guid.Empty && internalConnection.OriginalClientConnectionId != internalConnection.ClientConnectionId)
			{
				ex.Data.Add("OriginalClientConnectionId", internalConnection.OriginalClientConnectionId);
			}
			if (!string.IsNullOrEmpty(internalConnection.RoutingDestination))
			{
				ex.Data.Add("RoutingDestination", internalConnection.RoutingDestination);
			}
		}
		return ex;
	}

	internal static SqlException CreateException(SqlErrorCollection errorCollection, string serverVersion, Guid conId, Exception innerException = null)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < errorCollection.Count; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(Environment.NewLine);
			}
			stringBuilder.Append(errorCollection[i].Message);
		}
		if (innerException == null && errorCollection[0].Win32ErrorCode != 0 && errorCollection[0].Win32ErrorCode != -1)
		{
			innerException = new Win32Exception(errorCollection[0].Win32ErrorCode);
		}
		SqlException ex = new SqlException(stringBuilder.ToString(), errorCollection, innerException, conId);
		ex.Data.Add("HelpLink.ProdName", "Microsoft SQL Server");
		if (!string.IsNullOrEmpty(serverVersion))
		{
			ex.Data.Add("HelpLink.ProdVer", serverVersion);
		}
		ex.Data.Add("HelpLink.EvtSrc", "MSSQLServer");
		ex.Data.Add("HelpLink.EvtID", errorCollection[0].Number.ToString(CultureInfo.InvariantCulture));
		ex.Data.Add("HelpLink.BaseHelpUrl", "http://go.microsoft.com/fwlink");
		ex.Data.Add("HelpLink.LinkId", "20476");
		return ex;
	}

	internal SqlException InternalClone()
	{
		SqlException ex = new SqlException(Message, _errors, base.InnerException, _clientConnectionId);
		if (Data != null)
		{
			foreach (DictionaryEntry datum in Data)
			{
				ex.Data.Add(datum.Key, datum.Value);
			}
		}
		ex._doNotReconnect = _doNotReconnect;
		return ex;
	}
}
