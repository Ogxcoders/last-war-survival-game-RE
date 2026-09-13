using System.Collections.Generic;
using UnityEngine;

public class MissileWarningController
{
	private static MissileWarningController _instance;

	private List<string> _saveList;

	private GameObject _missileAniObj;

	private List<MissileWarningAnimation> _saveAniList;

	private object m_userData;

	public static MissileWarningController Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new MissileWarningController();
			}
			return _instance;
		}
	}

	public bool StarAdd(object userData)
	{
		if (_saveList == null)
		{
			_saveList = new List<string>();
		}
		m_userData = userData;
		bool flag = false;
		if (_missileAniObj == null)
		{
			flag = true;
		}
		if (!flag)
		{
			CheckLoad();
		}
		return true;
	}

	private void CheckLoad()
	{
		InitAfterLoad();
	}

	private void InitAfterLoad()
	{
		if (_saveAniList == null)
		{
			_saveAniList = new List<MissileWarningAnimation>();
		}
		MissileWarningAnimation component = Object.Instantiate(_missileAniObj).GetComponent<MissileWarningAnimation>();
		component.CSShow(m_userData);
		_saveAniList.Add(component);
	}

	public void UnInit()
	{
		if (_saveAniList != null && _saveAniList.Count > 0)
		{
			foreach (MissileWarningAnimation saveAni in _saveAniList)
			{
				Object.Destroy(saveAni.gameObject);
			}
			_saveAniList = null;
		}
		if (_saveList != null && _saveList.Count > 0)
		{
			foreach (string save in _saveList)
			{
				_ = save;
			}
			_saveList = null;
		}
		if (_missileAniObj != null)
		{
			_missileAniObj = null;
		}
	}
}
