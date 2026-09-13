using System;
using System.Collections.Generic;
using System.IO;
using NiceJson;
using UnityEngine;
using UnityEngine.Networking;

namespace Mopsicus.Plugins;

public class MobileInput : MonoBehaviour, IPlugin
{
	public delegate void ShowDelegate(int id, bool isShow, int height, int leftOffset, int rightOffset);

	private const string KEYBOARD_ACTION = "KEYBOARD_ACTION";

	private const string INIT_KEY = "mobileinput_inited";

	public static ShowDelegate OnShowKeyboard = delegate
	{
	};

	private Dictionary<int, MobileInputReceiver> _inputs = new Dictionary<int, MobileInputReceiver>();

	private static MobileInput _instance;

	private JsonObject _data;

	private JsonObject _error;

	private int _counter;

	private List<string> modelList = new List<string> { "SM-S9280", "SM-S9280", "SM-A336B", "SM-G9900", "SM-S928B", "SC-03L", "SM-A336B", "SM-S928N", "SM-A536B", "SM-W9025" };

	public static Action<string> OnTextChangeFromPlatform;

	private static int curId = 0;

	public bool isUnSupportMultiple;

	public string Name => GetType().Name.ToLower();

	public static int CurId
	{
		get
		{
			return curId;
		}
		set
		{
			if (value >= 0)
			{
				curId = value;
			}
		}
	}

	public static MobileInput Plugin => _instance;

	private void Awake()
	{
		if ((object)_instance == null)
		{
			_instance = GetComponent<MobileInput>();
			Init();
		}
	}

	public bool IsUnSupportMultiple()
	{
		return GameEntry.Lua.CallWithReturn<bool>("CSharpCallLuaInterface.GetMobilSupportMultiple");
	}

	private bool FixNavBarHeight(ref int height, ref bool isShow, int navBarHeight)
	{
		if (height <= navBarHeight)
		{
			height = 0;
			isShow = false;
			return true;
		}
		return false;
	}

	public void OnData(JsonObject data)
	{
		_data = data;
		try
		{
			JsonObject jsonObject = (JsonObject)JsonNode.ParseJsonString(data["data"]);
			string text = jsonObject["msg"];
			int num = 0;
			if (text == "KEYBOARD_ACTION")
			{
				bool isShow = jsonObject["show"];
				int height = (int)((float)jsonObject["height"] * (float)Screen.height);
				int leftOffset = (int)((float)jsonObject["lratio"] * (float)Screen.width);
				int rightOffset = (int)((float)jsonObject["rratio"] * (float)Screen.width);
				if (jsonObject.ContainsKey("extraMsg"))
				{
					JsonObject jsonObject2 = (JsonObject)JsonNode.ParseJsonString(jsonObject["extraMsg"]);
					int num2 = jsonObject2["heightMax"];
					int num3 = jsonObject2["rectbottom"];
					num = jsonObject2["navHeight"];
					int num4 = height;
					string deviceModel = SystemInfo.deviceModel;
					if (ClientSwitch.IsOn(11) && jsonObject2.ContainsKey("DecorViewSizeY"))
					{
						if (ClientSwitch.IsOn(27))
						{
							bool flag = false;
							for (int i = 0; i < modelList.Count; i++)
							{
								if (deviceModel.Contains(modelList[i]))
								{
									int num5 = jsonObject2["DecorViewSizeY"];
									height = (int)((float)(num5 - num3) * 1f / (float)num5 * (float)Screen.height);
									FixNavBarHeight(ref height, ref isShow, num);
									if (isShow)
									{
										Debug.LogWarning(string.Format("OnShowKeyboard deviceModel: [{0}], [{1}, {2}], [{3}, {4}], {5}", height, num4, jsonObject["height"], Screen.width, Screen.height, jsonObject["extraMsg"]));
									}
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								int num6 = jsonObject2["DecorViewSizeY"];
								int num7 = ((num2 > num6) ? num2 : num6);
								height = (int)((float)(num7 - num3) * 1f / (float)num7 * (float)Screen.height);
								FixNavBarHeight(ref height, ref isShow, num);
								if (isShow)
								{
									Debug.LogWarning(string.Format("OnShowKeyboard FitAll: [{0}], [{1}, {2}], [{3}, {4}], {5}", height, num4, jsonObject["height"], Screen.width, Screen.height, jsonObject["extraMsg"]));
								}
							}
						}
						else if (deviceModel.Contains("SM-S928B"))
						{
							int num8 = jsonObject2["DecorViewSizeY"];
							height = (int)((float)(num8 - num3) * 1f / (float)num8 * (float)Screen.height);
							FixNavBarHeight(ref height, ref isShow, num);
							if (isShow)
							{
								Debug.LogWarning(string.Format("OnShowKeyboard S928B: [{0}], [{1}, {2}], [{3}, {4}], {5}", height, num4, jsonObject["height"], Screen.width, Screen.height, jsonObject["extraMsg"]));
							}
						}
						else
						{
							int num9 = jsonObject2["DecorViewSizeY"];
							int num10 = ((num2 > num9) ? num2 : num9);
							height = (int)((float)(num10 - num3) * 1f / (float)num10 * (float)Screen.height);
							FixNavBarHeight(ref height, ref isShow, num);
							if (isShow)
							{
								Debug.LogWarning(string.Format("OnShowKeyboard FitAll: [{0}], [{1}, {2}], [{3}, {4}], {5}", height, num4, jsonObject["height"], Screen.width, Screen.height, jsonObject["extraMsg"]));
							}
						}
					}
					else if (deviceModel.Contains("samsung") || deviceModel.Contains("SM-") || deviceModel.Contains("Galaxy"))
					{
						height = (int)((float)(num2 - num3) * 1f / (float)num2 * (float)Screen.height);
					}
				}
				if (isUnSupportMultiple)
				{
					if (_inputs.ContainsKey(curId))
					{
						_inputs[curId]?.OnShowKeyboard?.Invoke(curId, isShow, height, leftOffset, rightOffset);
					}
				}
				else
				{
					OnShowKeyboard?.Invoke(0, isShow, height, num, rightOffset);
				}
			}
			else if (!jsonObject.ContainsKey("id"))
			{
				Debug.LogError($"{GetType().Name} plugin OnData error: {jsonObject.ToJsonString()}");
			}
			else if (!isUnSupportMultiple || !(jsonObject["id"] == "none"))
			{
				int num11 = jsonObject["id"];
				if (_inputs.ContainsKey(num11))
				{
					GetReceiver(num11).Send(jsonObject);
				}
			}
			_data = null;
		}
		catch (Exception ex)
		{
			Debug.LogError($"{GetType().Name} plugin OnData error: {ex.Message}");
		}
	}

	public void OnError(JsonObject data)
	{
		Debug.LogError($"{GetType().Name} plugin OnError: {data.ToJsonPrettyPrintString()}");
		_error = data;
		try
		{
			_error = null;
		}
		catch (Exception ex)
		{
			Debug.LogError($"{GetType().Name} plugin OnError error: {ex.Message}");
		}
	}

	public static int Register(MobileInputReceiver receiver)
	{
		int counter = _instance._counter;
		_instance._counter++;
		_instance._inputs[counter] = receiver;
		return counter;
	}

	public static void RemoveReceiver(int id)
	{
		_instance._inputs.Remove(id);
	}

	public static MobileInputReceiver GetReceiver(int id)
	{
		return _instance._inputs[id];
	}

	public static void Execute(int id, JsonObject data)
	{
		data["id"] = id;
		string text = data.ToJsonString();
		using AndroidJavaClass androidJavaClass = new AndroidJavaClass($"ru.mopsicus.{_instance.Name}.Plugin");
		androidJavaClass.CallStatic("execute", id, text);
	}

	public static void Init()
	{
		if (PlayerPrefs.GetInt("mobileinput_inited", 0) == 0)
		{
			string streamingAssetsPath = Application.streamingAssetsPath;
			if (Directory.Exists(streamingAssetsPath))
			{
				string[] files = Directory.GetFiles(streamingAssetsPath, "*.ttf");
				for (int i = 0; i < files.Length; i++)
				{
					PrepareFontsAssets(Path.GetFileName(files[i]));
				}
			}
			PlayerPrefs.SetInt("mobileinput_inited", 1);
			PlayerPrefs.Save();
		}
		using AndroidJavaClass androidJavaClass = new AndroidJavaClass($"ru.mopsicus.{_instance.Name}.Plugin");
		androidJavaClass.CallStatic("init");
	}

	public static void Destroy()
	{
		using AndroidJavaClass androidJavaClass = new AndroidJavaClass($"ru.mopsicus.{_instance.Name}.Plugin");
		androidJavaClass.CallStatic("destroy");
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus)
		{
			if (_data != null)
			{
				OnData(_data);
			}
			else if (_error != null)
			{
				OnError(_error);
			}
		}
	}

	private static void PrepareFontsAssets(string fileName)
	{
		string dataPath = Application.dataPath;
		string path = $"{Application.persistentDataPath}/{fileName}";
		using UnityWebRequest unityWebRequest = UnityWebRequest.Get($"jar:file://{dataPath}!/assets/{fileName}");
		unityWebRequest.SendWebRequest();
		while (!unityWebRequest.isDone)
		{
		}
		File.WriteAllBytes(path, unityWebRequest.downloadHandler.data);
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (focusStatus)
		{
			return;
		}
		foreach (MobileInputReceiver value in _instance._inputs.Values)
		{
			value.Hide();
		}
	}
}
