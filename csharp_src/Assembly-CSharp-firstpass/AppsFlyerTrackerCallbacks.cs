using System;
using UnityEngine;

public class AppsFlyerTrackerCallbacks : MonoBehaviour
{
	private void Start()
	{
		printCallback("AppsFlyerTrackerCallbacks on Start");
	}

	private void Update()
	{
	}

	public void didReceiveConversionData(string conversionData)
	{
		printCallback("AppsFlyerTrackerCallbacks:: got conversion data = " + conversionData);
	}

	public void didReceiveConversionDataWithError(string error)
	{
		printCallback("AppsFlyerTrackerCallbacks:: got conversion data error = " + error);
	}

	public void didFinishValidateReceipt(string validateResult)
	{
		printCallback("AppsFlyerTrackerCallbacks:: got didFinishValidateReceipt  = " + validateResult);
	}

	public void didFinishValidateReceiptWithError(string error)
	{
		printCallback("AppsFlyerTrackerCallbacks:: got idFinishValidateReceiptWithError error = " + error);
	}

	public void onAppOpenAttribution(string validateResult)
	{
		printCallback("AppsFlyerTrackerCallbacks:: got onAppOpenAttribution  = " + validateResult);
	}

	public void onAppOpenAttributionFailure(string error)
	{
		printCallback("AppsFlyerTrackerCallbacks:: got onAppOpenAttributionFailure error = " + error);
	}

	public void onInAppBillingSuccess()
	{
		printCallback("AppsFlyerTrackerCallbacks:: got onInAppBillingSuccess succcess");
	}

	public void onInAppBillingFailure(string error)
	{
		printCallback("AppsFlyerTrackerCallbacks:: got onInAppBillingFailure error = " + error);
	}

	public void onInviteLinkGenerated(string link)
	{
		printCallback("AppsFlyerTrackerCallbacks:: generated userInviteLink " + link);
	}

	public void onOpenStoreLinkGenerated(string link)
	{
		printCallback("onOpenStoreLinkGenerated:: generated store link " + link);
		if (BaseVersionCompare(Application.version, "1.0.286") >= 0)
		{
			Application.OpenURL(link);
		}
		else
		{
			Application.OpenURL(link);
		}
	}

	public static int BaseVersionCompare(string a, string b)
	{
		if (string.IsNullOrEmpty(a))
		{
			if (string.IsNullOrEmpty(b))
			{
				return 0;
			}
			return -1;
		}
		if (string.IsNullOrEmpty(b))
		{
			return 1;
		}
		string[] array = a.Split(new char[1] { '.' });
		string[] array2 = b.Split(new char[1] { '.' });
		int num = array.Length;
		int num2 = array2.Length;
		int num3 = Math.Max(num, num2);
		for (int i = 0; i < num3; i++)
		{
			string text = ((i < num) ? array[i] : null);
			string text2 = ((i < num2) ? array2[i] : null);
			int num4 = 0;
			int num5 = 0;
			if (!string.IsNullOrEmpty(text))
			{
				num4 = int.Parse(text);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				num5 = int.Parse(text2);
			}
			if (num4 > num5)
			{
				return 1;
			}
			if (num4 < num5)
			{
				return -1;
			}
		}
		return 0;
	}

	private void printCallback(string str)
	{
		Debug.Log(str);
	}
}
