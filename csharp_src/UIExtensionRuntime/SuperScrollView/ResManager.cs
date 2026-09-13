using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView;

public class ResManager : MonoBehaviour
{
	public Sprite[] spriteObjArray;

	private static ResManager instance;

	private string[] mWordList;

	private Dictionary<string, Sprite> spriteObjDict = new Dictionary<string, Sprite>();

	public static ResManager Get
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType<ResManager>();
			}
			return instance;
		}
	}

	public int SpriteCount => spriteObjArray.Length;

	private void InitData()
	{
		spriteObjDict.Clear();
		Sprite[] array = spriteObjArray;
		foreach (Sprite sprite in array)
		{
			spriteObjDict[sprite.name] = sprite;
		}
	}

	private void Awake()
	{
		instance = null;
		InitData();
	}

	public Sprite GetSpriteByName(string spriteName)
	{
		Sprite value = null;
		if (spriteObjDict.TryGetValue(spriteName, out value))
		{
			return value;
		}
		return null;
	}

	public string GetRandomSpriteName()
	{
		int max = spriteObjArray.Length;
		int num = Random.Range(0, max);
		return spriteObjArray[num].name;
	}

	public Sprite GetSpriteByIndex(int index)
	{
		if (index < 0 || index >= spriteObjArray.Length)
		{
			return null;
		}
		return spriteObjArray[index];
	}

	public string GetSpriteNameByIndex(int index)
	{
		if (index < 0 || index >= spriteObjArray.Length)
		{
			return "";
		}
		return spriteObjArray[index].name;
	}
}
