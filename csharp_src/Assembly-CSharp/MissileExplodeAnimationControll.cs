using System.Collections.Generic;
using UnityEngine;

public class MissileExplodeAnimationControll
{
	private static MissileExplodeAnimationControll _instance;

	private List<string> _saveList;

	private GameObject _missileAniObj;

	private List<MissileExplodeAnimation> _saveAniList;

	private object m_userData;

	public static MissileExplodeAnimationControll Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new MissileExplodeAnimationControll();
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
			_saveAniList = new List<MissileExplodeAnimation>();
		}
		MissileExplodeAnimation component = Object.Instantiate(_missileAniObj).GetComponent<MissileExplodeAnimation>();
		component.CSShow(m_userData);
		_saveAniList.Add(component);
	}

	public void HideAni(GameObject temp)
	{
		if (_saveAniList != null && _saveAniList.Count > 0)
		{
			MissileExplodeAnimation component = temp.GetComponent<MissileExplodeAnimation>();
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
		_saveAniList = null;
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
