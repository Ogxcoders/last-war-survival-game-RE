using System;
using FibMatrix;
using UnityEngine;

public class GlobalMonobehaviourDispatcher
{
	private static GlobalMonobehaviourDispatcher _instance;

	private int _currentFrameCount = -1;

	private float _currentTime = -1f;

	private float _smoothDeltaTime = -1f;

	private bool _onApplicationFocusListened;

	private Action<bool> _onApplicationFocusHandler;

	private bool _onApplicationPauseListened;

	private Action<bool> _onApplicationPauseHandler;

	private bool _onApplicationQuitListened;

	private Action _onApplicationQuitHandler;

	private bool _onNativePluginMessageListened;

	private Action<string, string> _onNativePluginMessageHandler;

	public static GlobalMonobehaviourDispatcher instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GlobalMonobehaviourDispatcher();
			}
			return _instance;
		}
	}

	public static int currentFrameCount
	{
		get
		{
			int num = instance._currentFrameCount;
			if (num < 0)
			{
				num = (instance._currentFrameCount = Time.frameCount);
			}
			return num;
		}
	}

	public static float currentTime
	{
		get
		{
			float num = instance._currentTime;
			if (num < 0f)
			{
				num = (instance._currentTime = Time.time);
			}
			return num;
		}
	}

	public static float smoothDeltaTime
	{
		get
		{
			float num = instance._smoothDeltaTime;
			if (num < 0f)
			{
				num = (instance._smoothDeltaTime = Time.smoothDeltaTime);
			}
			return num;
		}
	}

	public static event Action<bool> onApplicationFocus
	{
		add
		{
			instance.AddApplicationFocusListener(value);
		}
		remove
		{
			instance.RemoveApplicationFocusListener(value);
		}
	}

	public static event Action<bool> onApplicationPause
	{
		add
		{
			instance.AddApplicationPauseListener(value);
		}
		remove
		{
			instance.RemoveApplicationPauseListener(value);
		}
	}

	public static event Action onApplicationQuit
	{
		add
		{
			instance.AddApplicationQuitListener(value);
		}
		remove
		{
			instance.RemoveApplicationQuitListener(value);
		}
	}

	public static event Action<string, string> onNativePluginMessage
	{
		add
		{
			instance.AddNativePluginMessageListener(value);
		}
		remove
		{
			instance.RemoveNativePluginMessageListener(value);
		}
	}

	private void AddApplicationFocusListener(Action<bool> value)
	{
		if (!_onApplicationFocusListened)
		{
			_onApplicationFocusListened = true;
		}
		_onApplicationFocusHandler = (Action<bool>)Delegate.Combine(_onApplicationFocusHandler, value);
	}

	private void RemoveApplicationFocusListener(Action<bool> value)
	{
		if (_onApplicationFocusListened)
		{
			_onApplicationFocusHandler = (Action<bool>)Delegate.Remove(_onApplicationFocusHandler, value);
		}
	}

	private void OnApplicationFocusListener(bool focus)
	{
		if (_onApplicationFocusHandler != null)
		{
			try
			{
				_onApplicationFocusHandler(focus);
			}
			catch (Exception exception)
			{
				FibMatrix.Logger.Error(exception);
			}
		}
	}

	private void AddApplicationPauseListener(Action<bool> value)
	{
		if (!_onApplicationPauseListened)
		{
			_onApplicationPauseListened = true;
		}
		_onApplicationPauseHandler = (Action<bool>)Delegate.Combine(_onApplicationPauseHandler, value);
	}

	private void RemoveApplicationPauseListener(Action<bool> value)
	{
		if (_onApplicationPauseListened)
		{
			_onApplicationPauseHandler = (Action<bool>)Delegate.Remove(_onApplicationPauseHandler, value);
		}
	}

	public void OnApplicationPauseListener(bool paused)
	{
		if (_onApplicationPauseHandler != null)
		{
			try
			{
				_onApplicationPauseHandler(paused);
			}
			catch (Exception exception)
			{
				FibMatrix.Logger.Error(exception);
			}
		}
	}

	private void AddApplicationQuitListener(Action value)
	{
		if (!_onApplicationQuitListened)
		{
			_onApplicationQuitListened = true;
		}
		_onApplicationQuitHandler = (Action)Delegate.Combine(_onApplicationQuitHandler, value);
	}

	private void RemoveApplicationQuitListener(Action value)
	{
		if (_onApplicationQuitListened)
		{
			_onApplicationQuitHandler = (Action)Delegate.Remove(_onApplicationQuitHandler, value);
		}
	}

	public void OnApplicationQuitListener()
	{
		if (_onApplicationQuitHandler != null)
		{
			try
			{
				_onApplicationQuitHandler();
			}
			catch (Exception exception)
			{
				FibMatrix.Logger.Error(exception);
			}
		}
	}

	private void AddNativePluginMessageListener(Action<string, string> value)
	{
		if (!_onNativePluginMessageListened)
		{
			_onNativePluginMessageListened = true;
		}
		_onNativePluginMessageHandler = (Action<string, string>)Delegate.Combine(_onNativePluginMessageHandler, value);
	}

	private void RemoveNativePluginMessageListener(Action<string, string> value)
	{
		if (_onNativePluginMessageListened)
		{
			_onNativePluginMessageHandler = (Action<string, string>)Delegate.Remove(_onNativePluginMessageHandler, value);
		}
	}

	private void OnNativePluginMessageListener(string name, string content)
	{
		if (_onNativePluginMessageHandler != null)
		{
			try
			{
				_onNativePluginMessageHandler(name, content);
			}
			catch (Exception exception)
			{
				FibMatrix.Logger.Error(exception);
			}
		}
	}
}
