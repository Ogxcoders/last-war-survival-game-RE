using System.Collections.Generic;
using System.Diagnostics;

namespace System.Data.SqlClient;

internal class SessionData
{
	internal const int _maxNumberOfSessionStates = 256;

	internal uint _tdsVersion;

	internal bool _encrypted;

	internal string _database;

	internal SqlCollation _collation;

	internal string _language;

	internal string _initialDatabase;

	internal SqlCollation _initialCollation;

	internal string _initialLanguage;

	internal byte _unrecoverableStatesCount;

	internal Dictionary<string, Tuple<string, string>> _resolvedAliases;

	internal SessionStateRecord[] _delta = new SessionStateRecord[256];

	internal bool _deltaDirty;

	internal byte[][] _initialState = new byte[256][];

	public SessionData(SessionData recoveryData)
	{
		_initialDatabase = recoveryData._initialDatabase;
		_initialCollation = recoveryData._initialCollation;
		_initialLanguage = recoveryData._initialLanguage;
		_resolvedAliases = recoveryData._resolvedAliases;
		for (int i = 0; i < 256; i++)
		{
			if (recoveryData._initialState[i] != null)
			{
				_initialState[i] = (byte[])recoveryData._initialState[i].Clone();
			}
		}
	}

	public SessionData()
	{
		_resolvedAliases = new Dictionary<string, Tuple<string, string>>(2);
	}

	public void Reset()
	{
		_database = null;
		_collation = null;
		_language = null;
		if (_deltaDirty)
		{
			_delta = new SessionStateRecord[256];
			_deltaDirty = false;
		}
		_unrecoverableStatesCount = 0;
	}

	[Conditional("DEBUG")]
	public void AssertUnrecoverableStateCountIsCorrect()
	{
		byte b = 0;
		SessionStateRecord[] delta = _delta;
		foreach (SessionStateRecord sessionStateRecord in delta)
		{
			if (sessionStateRecord != null && !sessionStateRecord._recoverable)
			{
				b++;
			}
		}
	}
}
