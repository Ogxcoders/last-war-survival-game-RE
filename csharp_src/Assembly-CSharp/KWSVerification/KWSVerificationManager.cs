using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace KWSVerification;

public class KWSVerificationManager : MonoBehaviour
{
	private abstract class BaseParameters
	{
		public string method;

		public string airKey;

		public string country;
	}

	private class AccountInfoParameters : BaseParameters
	{
		public string language;

		public int check;

		public string uid;

		public string zone;
	}

	private class VerifyParameters : BaseParameters
	{
		public string language;

		public string email;

		public int age;
	}

	private class CreateAccountParameters : BaseParameters
	{
		public string language;

		public int age;

		public bool needParent;

		public string uid;

		public string zone;

		public float confirmDelayDays;
	}

	private const string BASE_URL = "https://lastwar-serverlist-us-cf-ali.lastwarapp.com/gameservice/kidsverify.php";

	private const int REQUEST_TIMEOUT = 30;

	private const int MIN_REQUEST_INTERVAL = 1000;

	private const int VERIFY_CHECK_INTERVAL = 5;

	private const int MAX_RETRY_COUNT = 3;

	private string _kwsSignKey = "2Y2Q2OGE3OWIzMWViMT";

	private readonly Dictionary<string, long> _lastRequestTime = new Dictionary<string, long>();

	private readonly object _lockObject = new object();

	private static KWSVerificationManager _instance;

	public static KWSVerificationManager Instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject obj = new GameObject("KWSVerificationManager");
				_instance = obj.AddComponent<KWSVerificationManager>();
				UnityEngine.Object.DontDestroyOnLoad(obj);
			}
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			_instance = this;
		}
	}

	public void GetAccountInfo(string airKey, string country, string language, int check, string uid, string zone, Action<AccountInfoResponse> onSuccess, Action<string> onError)
	{
		if (ValidateInitialization(onError) && ValidateAirKey(airKey, onError) && ValidateCountryCode(country, onError) && ValidateLanguageCode(language, onError))
		{
			AccountInfoParameters parameters = new AccountInfoParameters
			{
				method = "account_info",
				airKey = airKey,
				country = country,
				language = language,
				check = check,
				uid = uid,
				zone = zone
			};
			StartCoroutine(SendRequest(parameters, onSuccess, onError));
		}
	}

	public void VerifyAge(string airKey, string country, string language, string email, int age, Action<VerifyResponse> onSuccess, Action<string> onError)
	{
		if (ValidateInitialization(onError) && ValidateAirKey(airKey, onError) && ValidateCountryCode(country, onError) && ValidateLanguageCode(language, onError) && ValidateAge(age, onError) && (string.IsNullOrEmpty(email) || ValidateEmail(email, onError)) && CheckRequestFrequency("verify"))
		{
			VerifyParameters parameters = new VerifyParameters
			{
				method = "verify",
				airKey = airKey,
				country = country,
				language = language,
				email = email,
				age = age
			};
			StartCoroutine(SendRequest(parameters, onSuccess, onError));
		}
	}

	public void CreateAccount(string airKey, string country, string language, int age, bool needParent, string uid, string zone, float confirmDelayDays, Action<CreateAccountResponse> onSuccess, Action<string> onError)
	{
		if (ValidateInitialization(onError) && ValidateAirKey(airKey, onError) && ValidateCountryCode(country, onError) && ValidateLanguageCode(language, onError) && ValidateAge(age, onError) && ValidateConfirmDelayDays(confirmDelayDays, onError) && CheckRequestFrequency("create_account"))
		{
			CreateAccountParameters parameters = new CreateAccountParameters
			{
				method = "create_account",
				airKey = airKey,
				country = country,
				language = language,
				age = age,
				needParent = needParent,
				uid = uid,
				zone = zone,
				confirmDelayDays = confirmDelayDays
			};
			StartCoroutine(SendRequest(parameters, onSuccess, onError));
		}
	}

	public Coroutine StartVerificationPolling(string airKey, string country, string language, string uid, string zone, Action<AccountInfoResponse> onVerified, Action<string> onError, int maxAttempts = 60)
	{
		return StartCoroutine(PollVerificationStatus(airKey, country, language, uid, zone, onVerified, onError, maxAttempts));
	}

	public void StopVerificationPolling(Coroutine pollingCoroutine)
	{
		if (pollingCoroutine != null)
		{
			StopCoroutine(pollingCoroutine);
		}
	}

	private IEnumerator SendRequest<TParam, TResponse>(TParam parameters, Action<TResponse> onSuccess, Action<string> onError) where TParam : BaseParameters where TResponse : BaseResponse
	{
		int retryCount = 0;
		while (retryCount < 3)
		{
			yield return SendRequestInternal(parameters, onSuccess, onError, delegate(bool success)
			{
				if (!success)
				{
					retryCount++;
					if (retryCount >= 3)
					{
						onError?.Invoke($"Request failed after {3} attempts");
					}
				}
				else
				{
					retryCount = 3;
				}
			});
			if (retryCount < 3)
			{
				yield return new WaitForSeconds(1f * (float)retryCount);
			}
		}
	}

	private IEnumerator SendRequestInternal<TParam, TResponse>(TParam parameters, Action<TResponse> onSuccess, Action<string> onError, Action<bool> onComplete) where TParam : BaseParameters where TResponse : BaseResponse
	{
		long currentTimestamp = GetCurrentTimestamp();
		string sign = GenerateSign(parameters.method, parameters.airKey, parameters.country, currentTimestamp);
		if (parameters.method == "account_info")
		{
			if (!(parameters is AccountInfoParameters accountInfoParameters))
			{
				onError?.Invoke("Invalid parameters for account_info method");
				onComplete?.Invoke(obj: false);
				yield break;
			}
			sign = GenerateSign_AccountInfo(accountInfoParameters.method, accountInfoParameters.airKey, accountInfoParameters.country, accountInfoParameters.check, currentTimestamp);
		}
		Dictionary<string, string> parameters2 = BuildUrlParameters(parameters, currentTimestamp, sign);
		string text = BuildUrl("https://lastwar-serverlist-us-cf-ali.lastwarapp.com/gameservice/kidsverify.php", parameters2);
		DebugLog("Sending request: " + parameters.method + " \n Request URL: " + text);
		using UnityWebRequest request = UnityWebRequest.Get(text);
		request.timeout = 30;
		yield return request.SendWebRequest();
		bool flag = false;
		string text2 = null;
		if (request.isNetworkError || request.isHttpError)
		{
			flag = true;
			text2 = request.error;
		}
		if (!flag)
		{
			string text3 = request.downloadHandler.text;
			DebugLog("Response: " + text3);
			try
			{
				TResponse val = JsonUtility.FromJson<TResponse>(text3);
				if (val.code == 0)
				{
					onSuccess?.Invoke(val);
					onComplete?.Invoke(obj: true);
					yield break;
				}
				string text4 = $"Server Error: Code={val.code}, Message={val.message}";
				Debug.LogError("[COPPA] [KWS] " + text4);
				onError?.Invoke(text4);
				onComplete?.Invoke(obj: false);
			}
			catch (Exception ex)
			{
				string text5 = "JSON Parse Error: " + ex.Message;
				Debug.LogError("[COPPA] [KWS] " + text5);
				onError?.Invoke(text5);
				onComplete?.Invoke(obj: false);
			}
		}
		else
		{
			string text6 = "Network Error: " + text2;
			Debug.LogError("[COPPA] [KWS] " + text6);
			onError?.Invoke(text6);
			onComplete?.Invoke(obj: false);
		}
	}

	private IEnumerator PollVerificationStatus(string airKey, string country, string language, string uid, string zone, Action<AccountInfoResponse> onVerified, Action<string> onError, int maxAttempts)
	{
		int attempts = 0;
		while (attempts < maxAttempts)
		{
			bool requestCompleted = false;
			bool verificationSuccess = false;
			string verificationError = null;
			GetAccountInfo(airKey, country, language, 0, uid, zone, delegate(AccountInfoResponse response)
			{
				requestCompleted = true;
				if (response.data?.accountInfo != null)
				{
					string parentConfirmed = response.data.accountInfo.parentConfirmed;
					if (parentConfirmed == "1")
					{
						verificationSuccess = true;
						onVerified?.Invoke(response);
					}
					else if (parentConfirmed == "-1")
					{
						verificationError = "Parent verification failed";
					}
				}
			}, delegate(string error)
			{
				requestCompleted = true;
				Debug.LogWarning("[COPPA] [KWS] Polling error: " + error);
			});
			while (!requestCompleted)
			{
				yield return null;
			}
			if (verificationSuccess)
			{
				yield break;
			}
			if (!string.IsNullOrEmpty(verificationError))
			{
				onError?.Invoke(verificationError);
				yield break;
			}
			attempts++;
			if (attempts < maxAttempts)
			{
				yield return new WaitForSeconds(5f);
			}
		}
		onError?.Invoke("Verification timeout - no response received");
	}

	private bool ValidateInitialization(Action<string> onError)
	{
		if (string.IsNullOrEmpty(_kwsSignKey))
		{
			string obj = "KWSVerificationManager not initialized. Call Initialize() first.";
			onError?.Invoke(obj);
			return false;
		}
		return true;
	}

	private bool ValidateAirKey(string airKey, Action<string> onError)
	{
		if (string.IsNullOrEmpty(airKey))
		{
			string obj = "AirKey cannot be null or empty";
			onError?.Invoke(obj);
			return false;
		}
		if (!airKey.StartsWith("lwDid_") || airKey.Length < 10)
		{
			string obj2 = "Invalid AirKey format";
			onError?.Invoke(obj2);
			return false;
		}
		return true;
	}

	private bool ValidateCountryCode(string country, Action<string> onError)
	{
		if (string.IsNullOrEmpty(country) || country.Length != 2)
		{
			string obj = "Invalid country code";
			onError?.Invoke(obj);
			return false;
		}
		if (!Regex.IsMatch(country, "^[A-Z]{2}$"))
		{
			string obj2 = "Country code must be 2 uppercase letters";
			onError?.Invoke(obj2);
			return false;
		}
		return true;
	}

	private bool ValidateLanguageCode(string language, Action<string> onError)
	{
		if (string.IsNullOrEmpty(language))
		{
			string obj = "Invalid language code";
			onError?.Invoke(obj);
			return false;
		}
		return true;
	}

	private bool ValidateEmail(string email, Action<string> onError)
	{
		if (string.IsNullOrEmpty(email))
		{
			return true;
		}
		string pattern = "^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$";
		if (!Regex.IsMatch(email, pattern))
		{
			string obj = "Invalid email format";
			onError?.Invoke(obj);
			return false;
		}
		if (email.Length > 255)
		{
			string obj2 = "Email too long";
			onError?.Invoke(obj2);
			return false;
		}
		return true;
	}

	private bool ValidateAge(int age, Action<string> onError)
	{
		if (age < 0 || age > 61)
		{
			string obj = "Age must be between 3 and 61";
			onError?.Invoke(obj);
			return false;
		}
		return true;
	}

	private bool ValidateConfirmDelayDays(float days, Action<string> onError)
	{
		if (days < 0f || days > 365f)
		{
			string obj = "ConfirmDelayDays must be between 0 and 365";
			onError?.Invoke(obj);
			return false;
		}
		return true;
	}

	private bool CheckRequestFrequency(string method)
	{
		lock (_lockObject)
		{
			long currentTimestamp = GetCurrentTimestamp();
			if (_lastRequestTime.ContainsKey(method))
			{
				long num = currentTimestamp - _lastRequestTime[method];
				if (num < 1000)
				{
					DebugLog($"Request too frequent. Please wait {(float)(1000 - num) / 1000f:F1} seconds.");
					return false;
				}
			}
			_lastRequestTime[method] = currentTimestamp;
			return true;
		}
	}

	private string GenerateSign(string method, string airKey, string country, long ts)
	{
		string input = method + airKey + country + ts + _kwsSignKey;
		return GetMD5Hash(input);
	}

	private string GenerateSign_AccountInfo(string method, string airKey, string country, int check, long ts)
	{
		string input = method + airKey + country + check + ts + _kwsSignKey;
		return GetMD5Hash(input);
	}

	private string GetMD5Hash(string input)
	{
		using MD5 mD = MD5.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] array = mD.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	private long GetCurrentTimestamp()
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return (long)(DateTime.UtcNow - dateTime).TotalMilliseconds;
	}

	private Dictionary<string, string> BuildUrlParameters<T>(T parameters, long ts, string sign) where T : BaseParameters
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		FieldInfo[] fields = typeof(T).GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			object value = fieldInfo.GetValue(parameters);
			if (value == null)
			{
				continue;
			}
			if (value is bool)
			{
				dictionary[fieldInfo.Name] = value.ToString().ToLower();
				continue;
			}
			string value2 = value.ToString();
			if (!string.IsNullOrEmpty(value2))
			{
				dictionary[fieldInfo.Name] = value2;
			}
		}
		dictionary["ts"] = ts.ToString();
		dictionary["sign"] = sign;
		return dictionary;
	}

	private string BuildUrl(string baseUrl, Dictionary<string, string> parameters)
	{
		if (parameters == null || parameters.Count == 0)
		{
			return baseUrl;
		}
		StringBuilder stringBuilder = new StringBuilder(baseUrl);
		stringBuilder.Append("?");
		bool flag = true;
		foreach (KeyValuePair<string, string> parameter in parameters)
		{
			if (!flag)
			{
				stringBuilder.Append("&");
			}
			stringBuilder.Append(UrlEncode(parameter.Key));
			stringBuilder.Append("=");
			stringBuilder.Append(UrlEncode(parameter.Value));
			flag = false;
		}
		return stringBuilder.ToString();
	}

	private string UrlEncode(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		try
		{
			return UnityWebRequest.EscapeURL(str);
		}
		catch
		{
			return Uri.EscapeDataString(str);
		}
	}

	private void DebugLog(string message)
	{
	}
}
