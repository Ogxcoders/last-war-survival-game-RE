using System;
using GameFramework;
using GameKit.Base;
using SFSLitJson;
using UnityEngine;
using UnityEngine.Networking;
using VEngine;

public class InGameCheckResVersionTools
{
	public enum CheckResult
	{
		Success_NoNeedUpdate,
		Success_NeedUpdate,
		Error,
		Error_UpdateType,
		TimeOut,
		HttpError,
		NoGateway
	}

	private UnityWebRequest _httpRequest;

	private const float TIMEOUT = 5f;

	private float _elapseTime;

	private Action<CheckResult, string> _onCheckFinish;

	public bool Start(Action<CheckResult, string> onCheckFinish)
	{
		_onCheckFinish = onCheckFinish;
		string curFinalGateServer = GameEntry.Network.GetCurFinalGateServer();
		if (!string.IsNullOrEmpty(curFinalGateServer))
		{
			string checkVersionURL = NetworkURLConfig.GetCheckVersionURL(curFinalGateServer);
			_httpRequest = UnityWebRequest.Get(checkVersionURL);
			if (!WebRequestManager.DisableCertificateHandler())
			{
				_httpRequest.certificateHandler = new WebRequestManager.WebRequestCertificateHandler();
			}
			_httpRequest.SendWebRequest();
			_elapseTime = 0f;
			Log.Info($"[InGameCheckResVersionTools] start {GameEntry.Timer.GetLocalSeconds()}   {checkVersionURL}");
			return true;
		}
		CheckFinish(CheckResult.NoGateway, null);
		return false;
	}

	public void Update()
	{
		if (_httpRequest == null)
		{
			return;
		}
		if (_httpRequest.isDone)
		{
			if (_httpRequest.isHttpError || _httpRequest.isNetworkError)
			{
				CheckFinish(CheckResult.HttpError, _httpRequest.error);
			}
			else
			{
				CheckSuccess();
			}
			return;
		}
		_elapseTime += Time.deltaTime;
		if (_elapseTime > 5f)
		{
			CheckFinish(CheckResult.TimeOut, null);
		}
	}

	private void CheckSuccess()
	{
		UnityWebRequest httpRequest = _httpRequest;
		DownloadHandler downloadHandler = httpRequest.downloadHandler;
		try
		{
			JsonData jsonData = JsonMapper.ToObject(downloadHandler.text);
			if (((string)jsonData["updateType"]).ToInt() != 1)
			{
				string[] array = ((string)jsonData["hotUpdateMsg"]).Split(new char[1] { ';' });
				foreach (string text in array)
				{
					if (!string.IsNullOrEmpty(text))
					{
						string[] array2 = text.Split(new char[1] { ',' });
						Log.Info("[InGameCheckResVersionTools]::raw str " + text);
						string text2 = array2[0];
						int num = array2[1].ToInt();
						Manifest manifest = Versions.GetManifest(text2.ToLower());
						if (text2 != "GameRes")
						{
							Log.Info("[InGameCheckResVersionTools]:: Skip check " + text2);
						}
						else if (manifest.version != num)
						{
							Log.Info($"[InGameCheckResVersionTools] version not march: {text2} remote:{num}  local:{manifest.version}");
							CheckFinish(CheckResult.Success_NeedUpdate, $"new:{text2}:{num}.cur:{manifest.name}:{manifest.version}");
							return;
						}
					}
				}
				if (XLuaManager.s_useLwLuaFile && jsonData["lwfile2"] != null)
				{
					int num2 = int.Parse(((string)jsonData["lwfile2"]).Split(new char[1] { '|' })[0]);
					Log.Info($"[InGameCheckResVersionTools]::luafile cur:{ClientConfig.LWLuaVersion}   new:{num2}");
					if (num2 != ClientConfig.LWLuaVersion)
					{
						CheckFinish(CheckResult.Success_NeedUpdate, $"luafile:cur:{ClientConfig.LWLuaVersion}   new:{num2}");
						return;
					}
				}
				string[] array3 = ((string)jsonData["table_version"]).Split(new char[1] { ',' });
				int num3 = int.Parse(array3[0]);
				string text3 = array3[2];
				Log.Info($"[InGameCheckResVersionTools]::tbl_ver cur:{ClientConfig.CURRENT_TABLE_VERSION}   new:{num3}");
				if (num3 != ClientConfig.CURRENT_TABLE_VERSION)
				{
					if (!(text3 == ClientConfig.CURRENT_TABLE_MD5))
					{
						CheckFinish(CheckResult.Success_NeedUpdate, $"tbl_ver:cur:{ClientConfig.CURRENT_TABLE_VERSION}   new:{num3}");
						return;
					}
					Log.Info($"[InGameCheckResVersionTools]::tbl_ver skip update.cause same md5.cur:{ClientConfig.CURRENT_TABLE_VERSION}   new:{num3}");
				}
				CheckFinish(CheckResult.Success_NoNeedUpdate, null);
			}
			else
			{
				CheckFinish(CheckResult.Error_UpdateType, null);
			}
		}
		catch (Exception ex)
		{
			string text4 = downloadHandler.text;
			string url = httpRequest.url;
			string text5 = ex.ToString();
			string text6 = "[InGameCheckResVersionTools] check res version exception:url:" + url + " return:" + text4 + " exc:" + text5;
			Log.Error(text6);
			CheckFinish(CheckResult.Error, text6);
		}
	}

	private void CheckFinish(CheckResult result, string reason = "")
	{
		try
		{
			Log.Info($"[InGameCheckResVersionTools]  CheckFinish:{result.ToString()}  time:{GameEntry.Timer.GetLocalSeconds()} reason:{reason}");
			if (_onCheckFinish != null)
			{
				_onCheckFinish(result, reason);
			}
		}
		catch (Exception ex)
		{
			Log.Error("[InGameCheckResVersionTools] Exception:" + ex.ToString());
		}
	}

	public void Dispose()
	{
		Log.Info("[InGameCheckResVersionTools] Dispose");
		_onCheckFinish = null;
		if (_httpRequest != null)
		{
			_httpRequest.Abort();
			_httpRequest.Dispose();
			_httpRequest = null;
		}
	}
}
