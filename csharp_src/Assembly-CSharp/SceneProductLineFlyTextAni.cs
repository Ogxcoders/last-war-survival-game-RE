using System.Collections.Generic;
using UnityEngine;

public class SceneProductLineFlyTextAni : MonoBehaviour
{
	[SerializeField]
	private List<Transform> _flyAniList;

	private List<SuperTextMesh> _textList;

	private List<SpriteRenderer> _spriteList;

	private int _listCount;

	private int _curUseListIndex;

	private float _refreshTime;

	private float _waitTime;

	private bool _isPlay;

	private void Awake()
	{
		_isPlay = false;
		_listCount = _flyAniList.Count;
		_refreshTime = GetRefreshDuring();
		_textList = new List<SuperTextMesh>();
		_spriteList = new List<SpriteRenderer>();
		for (int i = 0; i < _listCount; i++)
		{
			Transform obj = _flyAniList[i];
			SuperTextMesh item = obj.Find("num").GetComponent(typeof(SuperTextMesh)) as SuperTextMesh;
			SpriteRenderer item2 = obj.Find("num/icon").GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
			obj.gameObject.SetActive(value: false);
			_textList.Add(item);
			_spriteList.Add(item2);
		}
	}

	public void Init(string txtStr, string iconPath)
	{
		for (int i = 0; i < _listCount; i++)
		{
			_textList[i].text = txtStr;
			_spriteList[i].LoadSprite(iconPath);
			_flyAniList[i].gameObject.SetActive(value: false);
		}
	}

	public void PlayAni()
	{
		_isPlay = true;
	}

	public void StopAni()
	{
		_isPlay = false;
	}

	private void Update()
	{
		if (_isPlay)
		{
			_waitTime += Time.deltaTime;
			if (_waitTime > _refreshTime)
			{
				_waitTime = 0f;
				RefreshValue();
			}
		}
	}

	private void RefreshValue()
	{
		_curUseListIndex++;
		_curUseListIndex %= _listCount;
		_flyAniList[_curUseListIndex].gameObject.SetActive(value: false);
		_flyAniList[_curUseListIndex].gameObject.SetActive(value: true);
	}

	private int GetRefreshDuring()
	{
		return GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetProductFlyTxtAniIntervalTime");
	}
}
