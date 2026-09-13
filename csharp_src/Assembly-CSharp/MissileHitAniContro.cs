using System.Collections.Generic;
using UnityEngine;

public class MissileHitAniContro
{
	private static MissileHitAniContro _instance;

	private List<string> _saveList;

	private GameObject _missileAniObj;

	private List<MissileHitAnimation> _saveAniList;

	private object m_userData;

	public static MissileHitAniContro Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new MissileHitAniContro();
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
			_saveAniList = new List<MissileHitAnimation>();
		}
		MissileHitAnimation component = Object.Instantiate(_missileAniObj).GetComponent<MissileHitAnimation>();
		component.CSShow(m_userData);
		_saveAniList.Add(component);
	}

	public void HideAni(GameObject temp)
	{
		if (_saveAniList != null && _saveAniList.Count > 0)
		{
			MissileHitAnimation component = temp.GetComponent<MissileHitAnimation>();
			if (component != null && _saveAniList.Contains(component))
			{
				_saveAniList.Remove(component);
			}
		}
		if (_saveAniList.Count <= 0)
		{
			UnInit();
		}
	}

	public void UnInit()
	{
		if (_saveAniList != null && _saveAniList.Count > 0)
		{
			foreach (MissileHitAnimation saveAni in _saveAniList)
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
