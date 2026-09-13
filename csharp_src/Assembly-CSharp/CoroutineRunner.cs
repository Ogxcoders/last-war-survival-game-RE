using System;
using System.Collections;
using UnityEngine;
using XLua;

[Hotfix(HotfixFlag.Stateless)]
public class CoroutineRunner : MonoBehaviour
{
	public void YieldAndCallback(object toYield, Action callback)
	{
		StartCoroutine(CoBody(toYield, callback));
	}

	private IEnumerator CoBody(object toYield, Action callback)
	{
		if (toYield is IEnumerator)
		{
			yield return StartCoroutine((IEnumerator)toYield);
		}
		else
		{
			yield return toYield;
		}
		callback();
	}
}
