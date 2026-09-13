using System;
using System.Collections;
using System.Net;
using UnityEngine;

public class UnityPing : MonoBehaviour
{
	private string s_ip = "";

	private Action<int> s_callback;

	private int s_timeout = 2;

	private Coroutine curCoroutine;

	public void CreatePing(string ip, Action<int> callback)
	{
		if (!string.IsNullOrEmpty(ip) && callback != null)
		{
			s_ip = ip;
			s_callback = callback;
			if (curCoroutine != null)
			{
				StopCoroutine(curCoroutine);
			}
			curCoroutine = StartCoroutine(PingConnect());
		}
	}

	private void OnDestroy()
	{
		if (curCoroutine != null)
		{
			StopCoroutine(curCoroutine);
		}
		s_ip = "";
		s_timeout = 2;
		s_callback = null;
	}

	private IEnumerator PingConnect()
	{
		IPAddress[] ip = Dns.GetHostAddresses(s_ip);
		if (ip.Length == 0)
		{
			yield return null;
		}
		string address = ip[0].ToString();
		Ping ping = new Ping(address);
		int addTime = 0;
		int requestCount = s_timeout * 10;
		while (!ping.isDone)
		{
			yield return new WaitForSeconds(0.1f);
			if (addTime > requestCount)
			{
				if (s_callback != null)
				{
					s_callback(ping.time);
				}
				yield break;
			}
			addTime++;
		}
		if (ping.isDone)
		{
			if (s_callback != null)
			{
				s_callback(ping.time);
			}
			yield return null;
		}
	}
}
