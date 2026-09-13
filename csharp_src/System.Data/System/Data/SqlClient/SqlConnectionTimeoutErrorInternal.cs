using System.Text;

namespace System.Data.SqlClient;

internal class SqlConnectionTimeoutErrorInternal
{
	private SqlConnectionTimeoutPhaseDuration[] _phaseDurations;

	private SqlConnectionTimeoutPhaseDuration[] _originalPhaseDurations;

	private SqlConnectionTimeoutErrorPhase _currentPhase;

	private SqlConnectionInternalSourceType _currentSourceType;

	private bool _isFailoverScenario;

	internal SqlConnectionTimeoutErrorPhase CurrentPhase => _currentPhase;

	public SqlConnectionTimeoutErrorInternal()
	{
		_phaseDurations = new SqlConnectionTimeoutPhaseDuration[9];
		for (int i = 0; i < _phaseDurations.Length; i++)
		{
			_phaseDurations[i] = null;
		}
	}

	public void SetFailoverScenario(bool useFailoverServer)
	{
		_isFailoverScenario = useFailoverServer;
	}

	public void SetInternalSourceType(SqlConnectionInternalSourceType sourceType)
	{
		_currentSourceType = sourceType;
		if (_currentSourceType == SqlConnectionInternalSourceType.RoutingDestination)
		{
			_originalPhaseDurations = _phaseDurations;
			_phaseDurations = new SqlConnectionTimeoutPhaseDuration[9];
			SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.PreLoginBegin);
		}
	}

	internal void ResetAndRestartPhase()
	{
		_currentPhase = SqlConnectionTimeoutErrorPhase.PreLoginBegin;
		for (int i = 0; i < _phaseDurations.Length; i++)
		{
			_phaseDurations[i] = null;
		}
	}

	internal void SetAndBeginPhase(SqlConnectionTimeoutErrorPhase timeoutErrorPhase)
	{
		_currentPhase = timeoutErrorPhase;
		if (_phaseDurations[(int)timeoutErrorPhase] == null)
		{
			_phaseDurations[(int)timeoutErrorPhase] = new SqlConnectionTimeoutPhaseDuration();
		}
		_phaseDurations[(int)timeoutErrorPhase].StartCapture();
	}

	internal void EndPhase(SqlConnectionTimeoutErrorPhase timeoutErrorPhase)
	{
		_phaseDurations[(int)timeoutErrorPhase].StopCapture();
	}

	internal void SetAllCompleteMarker()
	{
		_currentPhase = SqlConnectionTimeoutErrorPhase.Complete;
	}

	internal string GetErrorMessage()
	{
		StringBuilder stringBuilder;
		string text;
		switch (_currentPhase)
		{
		case SqlConnectionTimeoutErrorPhase.PreLoginBegin:
			stringBuilder = new StringBuilder(SQLMessage.Timeout_PreLogin_Begin());
			text = SQLMessage.Duration_PreLogin_Begin(_phaseDurations[1].GetMilliSecondDuration());
			break;
		case SqlConnectionTimeoutErrorPhase.InitializeConnection:
			stringBuilder = new StringBuilder(SQLMessage.Timeout_PreLogin_InitializeConnection());
			text = SQLMessage.Duration_PreLogin_Begin(_phaseDurations[1].GetMilliSecondDuration() + _phaseDurations[2].GetMilliSecondDuration());
			break;
		case SqlConnectionTimeoutErrorPhase.SendPreLoginHandshake:
			stringBuilder = new StringBuilder(SQLMessage.Timeout_PreLogin_SendHandshake());
			text = SQLMessage.Duration_PreLoginHandshake(_phaseDurations[1].GetMilliSecondDuration() + _phaseDurations[2].GetMilliSecondDuration(), _phaseDurations[3].GetMilliSecondDuration());
			break;
		case SqlConnectionTimeoutErrorPhase.ConsumePreLoginHandshake:
			stringBuilder = new StringBuilder(SQLMessage.Timeout_PreLogin_ConsumeHandshake());
			text = SQLMessage.Duration_PreLoginHandshake(_phaseDurations[1].GetMilliSecondDuration() + _phaseDurations[2].GetMilliSecondDuration(), _phaseDurations[3].GetMilliSecondDuration() + _phaseDurations[4].GetMilliSecondDuration());
			break;
		case SqlConnectionTimeoutErrorPhase.LoginBegin:
			stringBuilder = new StringBuilder(SQLMessage.Timeout_Login_Begin());
			text = SQLMessage.Duration_Login_Begin(_phaseDurations[1].GetMilliSecondDuration() + _phaseDurations[2].GetMilliSecondDuration(), _phaseDurations[3].GetMilliSecondDuration() + _phaseDurations[4].GetMilliSecondDuration(), _phaseDurations[5].GetMilliSecondDuration());
			break;
		case SqlConnectionTimeoutErrorPhase.ProcessConnectionAuth:
			stringBuilder = new StringBuilder(SQLMessage.Timeout_Login_ProcessConnectionAuth());
			text = SQLMessage.Duration_Login_ProcessConnectionAuth(_phaseDurations[1].GetMilliSecondDuration() + _phaseDurations[2].GetMilliSecondDuration(), _phaseDurations[3].GetMilliSecondDuration() + _phaseDurations[4].GetMilliSecondDuration(), _phaseDurations[5].GetMilliSecondDuration(), _phaseDurations[6].GetMilliSecondDuration());
			break;
		case SqlConnectionTimeoutErrorPhase.PostLogin:
			stringBuilder = new StringBuilder(SQLMessage.Timeout_PostLogin());
			text = SQLMessage.Duration_PostLogin(_phaseDurations[1].GetMilliSecondDuration() + _phaseDurations[2].GetMilliSecondDuration(), _phaseDurations[3].GetMilliSecondDuration() + _phaseDurations[4].GetMilliSecondDuration(), _phaseDurations[5].GetMilliSecondDuration(), _phaseDurations[6].GetMilliSecondDuration(), _phaseDurations[7].GetMilliSecondDuration());
			break;
		default:
			stringBuilder = new StringBuilder(SQLMessage.Timeout());
			text = null;
			break;
		}
		if (_currentPhase != SqlConnectionTimeoutErrorPhase.Undefined && _currentPhase != SqlConnectionTimeoutErrorPhase.Complete)
		{
			if (_isFailoverScenario)
			{
				stringBuilder.Append("  ");
				stringBuilder.AppendFormat(null, SQLMessage.Timeout_FailoverInfo(), _currentSourceType);
			}
			else if (_currentSourceType == SqlConnectionInternalSourceType.RoutingDestination)
			{
				stringBuilder.Append("  ");
				stringBuilder.AppendFormat(null, SQLMessage.Timeout_RoutingDestination(), _originalPhaseDurations[1].GetMilliSecondDuration() + _originalPhaseDurations[2].GetMilliSecondDuration(), _originalPhaseDurations[3].GetMilliSecondDuration() + _originalPhaseDurations[4].GetMilliSecondDuration(), _originalPhaseDurations[5].GetMilliSecondDuration(), _originalPhaseDurations[6].GetMilliSecondDuration(), _originalPhaseDurations[7].GetMilliSecondDuration());
			}
		}
		if (text != null)
		{
			stringBuilder.Append("  ");
			stringBuilder.Append(text);
		}
		return stringBuilder.ToString();
	}
}
