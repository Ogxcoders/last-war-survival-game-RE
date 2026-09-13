using System;
using System.Collections.Generic;
using System.IO;
using GameFramework;
using GameKit.Base;
using UnityEngine;
using UnityEngine.Networking;

public class UploadImageManager
{
	public enum UploadImageType
	{
		Avatar,
		Chat,
		SeasonPhoto
	}

	private static UploadImageManager _instance;

	private string param_uid_;

	private int param_picVer_;

	private Action<string, string> param_callback_;

	private static bool UseNewPlayerInfo = false;

	private static string PHOTO_UPLOAD_URL_ONLINE_LW = "upload_img.php";

	private static string PHOTO_UPLOAD_URL_ONLINE_LW_AICheck = "upload_img_check.php";

	private static string SEND_PHOTO_UPLOAD_URL = "upload_img_chat.php";

	private static List<string> UPLOAD_URL_LIST = new List<string> { "https://lastwar-upload-us-aws-ali.lastwargame.com/", "https://lastwar-upload-us-gcp-ali.lastwargame.com/" };

	private static int _lastSuccessIndex = 0;

	public int photoResolutionLimit = -1;

	public int photoFileSizeLimit = -1;

	public int suitableResolutionSizeBig = -1;

	public int suitableResolutionSizeSmall = -1;

	public int curPhotoFuncType = -1;

	public string chatPhotoNewPhotoDir = "";

	private Action<string, string, int, string> finishedUploadPhotoCallback_ChatPhoto;

	private Dictionary<string, List<UnityWebRequest>> m_uploadRequests = new Dictionary<string, List<UnityWebRequest>>();

	public string webRequestRecord = "UploadChatSendPhoto_";

	private Action<string, string, int> finishedUploadPhotoCallback_SeasonAlliance;

	public static UploadImageManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new UploadImageManager();
			}
			return _instance;
		}
	}

	private static string PHOTO_UPLOAD_URL
	{
		get
		{
			if (UseNewPlayerInfo)
			{
				return PHOTO_UPLOAD_URL_ONLINE_LW_AICheck;
			}
			return PHOTO_UPLOAD_URL_ONLINE_LW;
		}
	}

	public static void Purge()
	{
		_instance = null;
	}

	private UploadImageManager()
	{
		Log.Info("初始化UploadImageManager ~");
	}

	public string GenAssetKey(string uid, int picVer, bool useBig = false)
	{
		string md5Hash = AESHelper.GetMd5Hash($"{uid}_{picVer}");
		string text = ((uid.Length > 6) ? uid.Substring(uid.Length - 6) : uid);
		string text2 = (useBig ? "_big" : "");
		return text + "/" + md5Hash + text2 + ".jpg";
	}

	public void RequestUploadImage(string uri, string uid, string photo_seq, string filePath, Action<string, string> cb)
	{
		if (!File.Exists(filePath))
		{
			Log.Error("#UploadHeadImage# file " + filePath + " not found??? error!!");
			cb("false", "");
			return;
		}
		byte[] array = File.ReadAllBytes(filePath);
		if (array.Length < 16)
		{
			Debug.LogError("#UploadHeadImage# file " + filePath + " is wrong??? error!!");
			cb("false", "");
			return;
		}
		string md5Hash = AESHelper.GetMd5Hash("x645rGA7rnG5yOZkGijddata0commandNameuser/UploadPhotoparams0gameuid" + uid + "photo_seq" + photo_seq);
		string text = "";
		int num = filePath.LastIndexOf('.');
		if (num != -1)
		{
			text = filePath.Substring(num + 1);
		}
		string fileName = Path.GetFileName(filePath);
		string mimeType = "image/" + text;
		string value = $"{{\n    \"data\":[{{\n        \"commandName\":\"user/UploadPhoto\",\n        \"params\":[{{\n            \"photo_seq\":\"{photo_seq}\",\n            \"gameuid\":\"{uid}\",\n        }}]\n    }}],\n    \"authkey\":\"{md5Hash}\"\n}}";
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("UploadPhoto", value);
		wWWForm.AddField("uid", uid);
		wWWForm.AddField("pvr", photo_seq);
		wWWForm.AddField("funcType", 0);
		wWWForm.AddBinaryData("file", array, fileName, mimeType);
		Log.Info($"#UploadHeadImage#  Start Web Post! byteLen:{array.Length}");
		PostWithFailover(uri, wWWForm, 0, delegate(bool result, UnityWebRequest request, object userData)
		{
			if (request != null)
			{
				string text2 = request.downloadHandler.text;
				if (!result)
				{
					Log.Warning($"#UploadHeadImage# request code: {request.responseCode}, error: {request.error}, response: {text2}");
					cb("false", GameEntry.Localization.GetString("avatar_settings_tips_noservice"));
				}
				else
				{
					Log.Info($"#UploadHeadImage# Web ret:{true}, rspCode:{request.responseCode}, Error:{request.error}, Res:{text2}");
					cb("true", text2);
				}
			}
		}, photo_seq.ToInt());
	}

	public void UploadWithFailoverCore(string apiName, WWWForm form, string filePath, int picVer, int uploadImageType, Action<bool, UnityWebRequest, object> callback)
	{
		try
		{
			if (UPLOAD_URL_LIST == null || UPLOAD_URL_LIST.Count == 0)
			{
				Log.Error("#UploadChatPhoto# UPLOAD_URL_LIST is empty!");
				callback?.Invoke(arg1: false, null, null);
				return;
			}
			int totalLineCount = UPLOAD_URL_LIST.Count;
			int retryCount = 0;
			int currentLineIndex = _lastSuccessIndex;
			TryUploadNext(null, null);
			void TryUploadNext(UnityWebRequest lastRequest, object lastUserData)
			{
				if (retryCount >= totalLineCount)
				{
					Log.Error($"#UploadImage# All upload URLs failed after {totalLineCount} tries. type={uploadImageType}, picVer={picVer}");
					if (lastRequest != null && lastUserData != null)
					{
						callback?.Invoke(arg1: false, lastRequest, lastUserData);
					}
				}
				else
				{
					string text = UPLOAD_URL_LIST[currentLineIndex];
					Log.Info($"#UploadImage# Try line {retryCount + 1}/{totalLineCount} -> {text}");
					string uploadKey = webRequestRecord + uploadImageType + picVer;
					UnityWebRequest item = SingletonBehaviour<WebRequestManager>.Instance.Post(text + apiName, form, delegate(UnityWebRequest resp, bool hasErr, object userData)
					{
						if (resp == null)
						{
							Log.Error($"#UploadImage# Callback received null response. type={uploadImageType}, picVer={picVer}");
							callback?.Invoke(arg1: false, null, userData);
						}
						else if (resp.isDone)
						{
							if (!hasErr && resp.responseCode >= 200 && resp.responseCode < 300)
							{
								Log.Info($"#UploadImage#  Success line {currentLineIndex}: code={resp.responseCode}, type={uploadImageType}, picVer={picVer}");
								_lastSuccessIndex = currentLineIndex;
								callback?.Invoke(arg1: true, resp, userData);
							}
							else
							{
								Log.Warning(string.Format("#UploadImage#  Failed line {0}, code={1}, error={2}, type={3}, picVer={4}", currentLineIndex, resp.responseCode, resp.error ?? "null", uploadImageType, picVer));
								currentLineIndex = (currentLineIndex + 1) % totalLineCount;
								int num = retryCount;
								retryCount = num + 1;
								TryUploadNext(resp, userData);
							}
							if (m_uploadRequests.TryGetValue(uploadKey, out var value))
							{
								value.Remove(resp);
								if (!resp.isDone)
								{
									resp.Abort();
								}
								if (value.Count == 0)
								{
									m_uploadRequests.Remove(uploadKey);
								}
							}
						}
					}, 0, 30, filePath);
					if (!m_uploadRequests.ContainsKey(uploadKey))
					{
						m_uploadRequests[uploadKey] = new List<UnityWebRequest>();
					}
					m_uploadRequests[uploadKey].Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			Log.Error("#UploadImage# Exception in UploadWithFailoverCore: " + ex.Message);
			callback?.Invoke(arg1: false, null, null);
		}
	}

	public void PostWithFailover(string apiName, WWWForm form, int uploadImageType, Action<bool, UnityWebRequest, object> cb, int picVer, string filePath = "")
	{
		UploadWithFailoverCore(apiName, form, filePath, picVer, uploadImageType, cb);
	}

	public void OnUploadImage(int code, string uid, int picVer, Action<string, string> action)
	{
		curPhotoFuncType = code;
		param_uid_ = uid;
		param_picVer_ = picVer;
		param_callback_ = action;
		UseNewPlayerInfo = GameEntry.Lua.CallWithReturn<int, string>("CSharpCallLuaInterface.GetInt", "NewPlayerInfo") == 1;
		GameEntry.GlobalData.isUploadPic = true;
		GameEntry.Sdk.OnUploadPhoto(uid, code, picVer);
	}

	public void OnImagePathOk(string filePath)
	{
		GameEntry.GlobalData.isUploadPic = false;
		if (param_callback_ != null)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				Log.Error("#UploadHeadImage# filePath is null.");
				param_callback_("false", "no file");
			}
			else if (filePath == "-1")
			{
				Log.Info("#UploadHeadImage# ucrop error.");
				param_callback_("false", "no file");
			}
			else
			{
				Debug.Log("#UploadImageManager# GetHeadImgBack filePath:" + filePath);
				int num = param_picVer_ % 1000000;
				UploadHeadImage(photo_seq: (!UseNewPlayerInfo) ? $"{num + 1}" : num.ToString(), uri: PHOTO_UPLOAD_URL, uid: param_uid_, filePath: filePath, cb: param_callback_);
				Log.Info("Upload head url: " + PHOTO_UPLOAD_URL);
			}
		}
	}

	public void UploadHeadImage(string uri, string uid, string photo_seq, string filePath, Action<string, string> cb)
	{
		Log.Info("#UploadHeadImage# step in UploadHeadImage!");
		if (cb == null)
		{
			Log.Error("#UploadHeadImage# no callback??? error!!");
			return;
		}
		GameEntry.Lua.Call("CSharpCallLuaInterface.OnUploadPicStart");
		RequestUploadImage(uri, uid, photo_seq, filePath, cb);
	}

	public static string GetHeadPath(string uid, string picVer)
	{
		string text = uid.Substring(uid.Length - 6, 6);
		string text2 = AESHelper.GetMd5Hash(uid + "_" + picVer) + ".jpg";
		return text + "/" + text2;
	}

	public void DownloadHeadImage(string uri, string uid, string picVer, Action<string, string> cb)
	{
		if (cb == null)
		{
			Log.Error("RequestTranslate no callback??? error!!");
			return;
		}
		string headPath = GetHeadPath(uid, picVer);
		string text = string.Format("{0}{1}{2}", uri, uri.EndsWith("/") ? "" : "/", headPath);
		string text2 = $"{Application.persistentDataPath}/LocalImage/{headPath}";
		string directoryName = Path.GetDirectoryName(text2);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
		Debug.Log("full_uri:" + text);
		SingletonBehaviour<WebRequestManager>.Instance.DownFile(text, text2, delegate(UnityWebRequest request, bool hasErr, object userdata)
		{
			bool flag = true;
			if (hasErr || !request.isDone)
			{
				flag = false;
			}
			cb(flag ? "true" : "false", "");
		});
	}

	public void SetUploadImageLimit(int photoResolutionLimit, int photoFileSizeLimit, int suitableResolutionSizeBig, int suitableResolutionSizeSmall)
	{
		this.photoResolutionLimit = photoResolutionLimit;
		this.photoFileSizeLimit = photoFileSizeLimit;
		this.suitableResolutionSizeBig = suitableResolutionSizeBig;
		this.suitableResolutionSizeSmall = suitableResolutionSizeSmall;
	}

	public void OpenPhotoAlbum(int code)
	{
		curPhotoFuncType = code;
		chatPhotoNewPhotoDir = "";
		int code2 = ((curPhotoFuncType == 3 || curPhotoFuncType == 4) ? 2 : curPhotoFuncType);
		GameEntry.Sdk.OnUploadPhoto_Chat(code2, photoResolutionLimit, photoFileSizeLimit, suitableResolutionSizeBig, suitableResolutionSizeSmall);
	}

	private string GetSelectPhotoPath_WinAndEditor()
	{
		return "";
	}

	private void CompressPhoto_WinAndEditor(string urlPath)
	{
		byte[] data = File.ReadAllBytes(urlPath);
		Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: false);
		texture2D.LoadImage(data);
		float originalWidth = texture2D.width;
		float originalHeight = texture2D.height;
		Vector2 bitmapScaleBig_WinAndEditor = GetBitmapScaleBig_WinAndEditor(originalWidth, originalHeight);
		int num = (int)Math.Floor(bitmapScaleBig_WinAndEditor.x);
		int num2 = (int)Math.Floor(bitmapScaleBig_WinAndEditor.y);
		if (num > photoResolutionLimit || num2 > photoResolutionLimit)
		{
			UIUtils.ShowTips("picture_reject_toast_size", 3f);
			return;
		}
		string text = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
		Texture2D resizedTexture = ScaleTexture_WinAndEditor(texture2D, num, num2);
		if (SavePhotoToCachePath_WinAndEditor(resizedTexture, text + "_big") == "")
		{
			Log.Error("#UploadChatPhoto#  PC平台下压缩大图存储失败！");
			return;
		}
		resizedTexture = MultiScaleTexture_WinAndEditor(texture2D);
		string text2 = SavePhotoToCachePath_WinAndEditor(resizedTexture, text);
		if (text2 == "")
		{
			Log.Error("#UploadChatPhoto#  PC平台下压缩小图存储失败！");
		}
		else
		{
			FinishedSelectSingleImage(text2, num, num2);
		}
	}

	public Vector2 GetBitmapScaleBig_WinAndEditor(float originalWidth, float originalHeight)
	{
		float num = originalWidth / originalHeight;
		float num2;
		float num3;
		if (originalWidth < (float)suitableResolutionSizeBig && originalHeight < (float)suitableResolutionSizeBig)
		{
			num2 = originalWidth;
			num3 = originalHeight;
		}
		else if (originalWidth > (float)suitableResolutionSizeBig && originalHeight > (float)suitableResolutionSizeBig)
		{
			if (num > 1f)
			{
				num3 = suitableResolutionSizeBig;
				num2 = num3 * num;
			}
			else
			{
				num2 = suitableResolutionSizeBig;
				num3 = num2 / num;
			}
		}
		else if (num > 2f || (double)num < 0.5)
		{
			num2 = originalWidth;
			num3 = originalHeight;
		}
		else if (num > 1f)
		{
			num2 = suitableResolutionSizeBig;
			num3 = num2 / num;
		}
		else
		{
			num3 = suitableResolutionSizeBig;
			num2 = num3 * num;
		}
		return new Vector2(num2, num3);
	}

	private string SavePhotoToCachePath_WinAndEditor(Texture2D resizedTexture, string photoName)
	{
		if (resizedTexture == null || resizedTexture.GetNativeTexturePtr() == IntPtr.Zero)
		{
			Log.Error("#UploadChatPhoto#  PC平台下复制原图Texture失败！");
			return "";
		}
		string text = Application.temporaryCachePath + "/ChatUploadPhoto";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		byte[] array = resizedTexture.EncodeToJPG(80);
		if ((float)(array.Length / 1024) > (float)photoFileSizeLimit)
		{
			UIUtils.ShowTips("picture_reject_toast_size1", 3f);
			return "";
		}
		text = text + "/" + photoName + ".jpg";
		File.WriteAllBytes(text, array);
		UnityEngine.Object.Destroy(resizedTexture);
		return text;
	}

	private Texture2D ScaleTexture_WinAndEditor(Texture2D source, int targetWidth, int targetHeight)
	{
		RenderTexture renderTexture = (RenderTexture.active = RenderTexture.GetTemporary(targetWidth, targetHeight));
		Graphics.Blit(source, renderTexture);
		Texture2D texture2D = new Texture2D(targetWidth, targetHeight, TextureFormat.RGBA32, mipChain: false);
		texture2D.ReadPixels(new Rect(0f, 0f, targetWidth, targetHeight), 0, 0);
		texture2D.Apply();
		RenderTexture.active = null;
		RenderTexture.ReleaseTemporary(renderTexture);
		return texture2D;
	}

	private Texture2D MultiScaleTexture_WinAndEditor(Texture2D originalTexture)
	{
		float num = Math.Max(originalTexture.width, originalTexture.height);
		while (num > (float)suitableResolutionSizeSmall)
		{
			num /= 1.8f;
			if (num < (float)suitableResolutionSizeSmall)
			{
				num = suitableResolutionSizeSmall;
			}
			Texture2D texture2D = ScaledBitmapToTargetScale_WinAndEditor(originalTexture, num);
			UnityEngine.Object.Destroy(originalTexture);
			originalTexture = texture2D;
		}
		return originalTexture;
	}

	public Texture2D ScaledBitmapToTargetScale_WinAndEditor(Texture2D texture, float targetEdge)
	{
		float num = texture.width;
		float num2 = texture.height;
		float num3 = 0f;
		float num4 = 0f;
		if (num >= num2)
		{
			float num5 = targetEdge / num;
			num3 = targetEdge;
			num4 = num2 * num5;
		}
		else
		{
			float num6 = targetEdge / num2;
			num4 = targetEdge;
			num3 = num * num6;
		}
		return ScaleTexture_WinAndEditor(texture, (int)Math.Floor(num3), (int)Math.Floor(num4));
	}

	public void FinishedSelectSingleImage(string filePath, int compressedWidth, int compressedHeight)
	{
		if (filePath == "")
		{
			Log.Error(" #UploadChatPhoto#  相册选取照片后返回路径为空！ 甚至还没开始请求最新版本号！");
			return;
		}
		chatPhotoNewPhotoDir = filePath;
		if (curPhotoFuncType == 2)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.FinishedSelectSinglePhotoChat", chatPhotoNewPhotoDir, compressedWidth, compressedHeight);
		}
		else if (curPhotoFuncType == 3)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.FinishedSelectSinglePhotoMoment", chatPhotoNewPhotoDir, compressedWidth, compressedHeight);
		}
		else if (curPhotoFuncType == 4)
		{
			GameEntry.Lua.Call("CSharpCallLuaInterface.FinishedSelectSinglePhotoChatAllianceNotice", chatPhotoNewPhotoDir, compressedWidth, compressedHeight);
		}
	}

	public void StartUploadPhoto(string uid, int lastestPicVer, Action<string, string, int, string> FinishedUploadPhotoAction)
	{
		param_uid_ = uid;
		param_picVer_ = lastestPicVer;
		finishedUploadPhotoCallback_ChatPhoto = FinishedUploadPhotoAction;
		if (finishedUploadPhotoCallback_ChatPhoto != null)
		{
			if (string.IsNullOrEmpty(chatPhotoNewPhotoDir))
			{
				finishedUploadPhotoCallback_ChatPhoto("false", "no file", lastestPicVer, "");
				return;
			}
			int picVer = param_picVer_ % 1000000;
			RequestUploadImage_SendPhoto(SEND_PHOTO_UPLOAD_URL, param_uid_, picVer, chatPhotoNewPhotoDir, finishedUploadPhotoCallback_ChatPhoto);
		}
	}

	public void RequestUploadImage_SendPhoto(string uri, string uid, int picVer, string filePathSmall, Action<string, string, int, string> cb)
	{
		Log.Info("#UploadChatPhoto#  Start UploadChatPhoto!");
		string text = picVer.ToString();
		if (!File.Exists(filePathSmall))
		{
			Log.Error("#UploadChatPhoto# file " + filePathSmall + " not found??? error!!");
			cb("false", "", picVer, filePathSmall);
			return;
		}
		byte[] array = File.ReadAllBytes(filePathSmall);
		if (array.Length < 16)
		{
			Log.Error("#UploadChatPhoto#: " + filePathSmall + " 找不到小图文件");
			cb("false", "", picVer, filePathSmall);
			return;
		}
		string fileName = Path.GetFileName(filePathSmall);
		string text2 = "";
		int num = filePathSmall.LastIndexOf('.');
		if (num != -1)
		{
			text2 = filePathSmall.Substring(num + 1);
		}
		string text3 = "";
		num = filePathSmall.LastIndexOf('/');
		if (num != -1)
		{
			text3 = filePathSmall.Substring(0, num + 1) + Path.GetFileNameWithoutExtension(filePathSmall) + "_big." + text2;
		}
		byte[] array2 = File.ReadAllBytes(text3);
		if (array2.Length < 16)
		{
			Log.Error("#UploadChatPhoto# " + text3 + " 找不到大图文件");
			cb("false", "", picVer, text3);
			return;
		}
		string fileName2 = Path.GetFileName(text3);
		string mimeType = "image/" + text2;
		string value = $"{{\n    \"data\":[{{\n        \"commandName\":\"user/UploadPhoto\",\n        \"params\":[{{\n            \"photo_seq\":\"{text}\",\n            \"gameuid\":\"{uid}\",\n        }}]\n    }}],\n}}";
		string md5Hash = AESHelper.GetMd5Hash(AESHelper.GetMd5Hash($"{uid}_{picVer}"));
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("UploadPhoto", value);
		wWWForm.AddField("uid", uid);
		wWWForm.AddField("pvr", text);
		wWWForm.AddField("funcType", 1);
		wWWForm.AddField("sign", md5Hash);
		wWWForm.AddBinaryData("file", array, fileName, mimeType);
		wWWForm.AddBinaryData("file_big", array2, fileName2, mimeType);
		Log.Info($"#UploadChatPhoto#  Start Web Post! byteLen:{array2.Length}");
		PostWithFailover(uri, wWWForm, 1, delegate(bool result, UnityWebRequest request, object userData)
		{
			if (request != null && userData != null)
			{
				string text4 = request.downloadHandler.text;
				if (!result)
				{
					Log.Error($"#UploadChatPhoto# request code: {request.responseCode}, error: {request.error}, response: {text4}");
					cb("false", "", picVer, userData.ToString());
				}
				else
				{
					Log.Info($"#UploadChatPhoto# Web ret:{true}, rspCode:{request.responseCode}, Error:{request.error}, Res:{text4}");
					cb("true", text4, picVer, userData.ToString());
				}
			}
		}, picVer, filePathSmall);
	}

	public void StartUploadPhoto_SeasonAlliance(string uid, int lastestPicVer, RenderTexture renderTexture, string photoName, Action<string, string, int> FinishedUploadPhotoAction)
	{
		param_uid_ = uid;
		param_picVer_ = lastestPicVer;
		finishedUploadPhotoCallback_SeasonAlliance = FinishedUploadPhotoAction;
		if (finishedUploadPhotoCallback_SeasonAlliance != null)
		{
			int width = renderTexture.width;
			int height = renderTexture.height;
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false);
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
			texture2D.Apply();
			byte[] array = texture2D.EncodeToJPG();
			if (array.Length < 16)
			{
				Log.Error("#SeasonAlliancePhoto#  RT图片转成的二进制问题!!");
				finishedUploadPhotoCallback_SeasonAlliance("false", "no file", lastestPicVer);
			}
			else
			{
				int picVer = param_picVer_ % 1000000;
				RequestUploadImage_SeasonAlliance(PHOTO_UPLOAD_URL_ONLINE_LW_AICheck, param_uid_, picVer, array, photoName, finishedUploadPhotoCallback_SeasonAlliance);
			}
		}
	}

	public void RequestUploadImage_SeasonAlliance(string uri, string uid, int picVer, byte[] photoByte, string photoName, Action<string, string, int> cb)
	{
		string text = picVer.ToString();
		string md5Hash = AESHelper.GetMd5Hash("x645rGA7rnG5yOZkGijddata0commandNameuser/UploadPhotoparams0gameuid" + uid + "photo_seq" + text);
		string mimeType = "image/jpg";
		string value = $"{{\n    \"data\":[{{\n        \"commandName\":\"user/UploadPhoto\",\n        \"params\":[{{\n            \"photo_seq\":\"{text}\",\n            \"gameuid\":\"{uid}\",\n        }}]\n    }}],\n    \"authkey\":\"{md5Hash}\"\n}}";
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("UploadPhoto", value);
		wWWForm.AddField("uid", uid);
		wWWForm.AddField("pvr", text);
		wWWForm.AddField("funcType", 2);
		wWWForm.AddBinaryData("file", photoByte, photoName, mimeType);
		PostWithFailover(uri, wWWForm, 2, delegate(bool result, UnityWebRequest request, object userData)
		{
			if (request != null)
			{
				string text2 = request.downloadHandler.text;
				if (!result)
				{
					Log.Error($"#SeasonAlliancePhoto# request code: {request.responseCode}, error: {request.error}, response: {text2}");
					cb("false", text2, picVer);
				}
				else
				{
					Log.Info($"#SeasonAlliancePhoto# Web ret:{true}, rspCode:{request.responseCode}, Error:{request.error}, Res:{text2}");
					cb("true", text2, picVer);
				}
			}
		}, picVer);
	}

	public void AbortUploadChatPhotoTask(string picVer, int upLoadImgType)
	{
		string key = webRequestRecord + upLoadImgType + picVer;
		if (m_uploadRequests.TryGetValue(key, out var value))
		{
			for (int i = 0; i < value.Count; i++)
			{
				SingletonBehaviour<WebRequestManager>.Instance.ChatSendPhotoAbort(value[i]);
			}
		}
		m_uploadRequests.Remove(key);
	}

	private string GetSelectFolderPath_WinAndEditor()
	{
		return "";
	}

	public void SelectFolderSavePhotoByPath_WinAndEditor(string picPath)
	{
		File.Exists(picPath);
	}

	public void SaveRenderTextureToFolder(RenderTexture rt, string savePath)
	{
		if (!(rt == null) && !string.IsNullOrEmpty(savePath))
		{
			string directoryName = Path.GetDirectoryName(savePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			int width = rt.width;
			int height = rt.height;
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false);
			RenderTexture.active = rt;
			texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
			texture2D.Apply();
			byte[] bytes = texture2D.EncodeToJPG();
			File.WriteAllBytes(savePath, bytes);
			RenderTexture.active = null;
			UnityEngine.Object.Destroy(texture2D);
		}
	}
}
