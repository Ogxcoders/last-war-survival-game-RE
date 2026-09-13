using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AIHelp;
using KWSVerification;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPrivacyCoppaView : UIPrivacyBaseView
{
	private static UIPrivacyCoppaView _instance;

	private GameObject _rootGameObject;

	private Dictionary<string, GameObject> _pageGameObjects;

	private TextMeshProUGUIEx _ageSelectionContentText;

	private Button _ageSelectionConfirmButton;

	private Slider _ageSlider;

	private Button _ageDecrementButton;

	private Button _ageIncrementButton;

	private TextMeshProUGUIEx _ageDisplayText;

	private int _currentSelectedAge = 3;

	private bool _isAgeUnselected = true;

	private bool _isFirstDrag = true;

	private int _dragCount;

	private bool _isIncrementButtonPressed;

	private ITimer _incrementLongPressTimer;

	private const float LONG_PRESS_INITIAL_DELAY = 0.2f;

	private const float LONG_PRESS_REPEAT_INTERVAL = 0.025f;

	private TextMeshProUGUIEx _confirmationAgeText;

	private Button _confirmationCancelButton;

	private Button _confirmationConfirmButton;

	private TMP_InputField _emailInputField;

	private Button _emailConfirmButton;

	private GameObject _emailErrorObject;

	private TextMeshProUGUIEx _parentConfirmationText;

	private Button _parentCancelButton;

	private Button _parentConfirmButton;

	private Button _contactUsButton;

	private Button _resendEmailButton;

	private TextMeshProUGUIEx _resendEmailText;

	private TextMeshProUGUIEx _resendButtonText;

	private GameObject _emailSentSuccessObject;

	private ITimer _resendCooldownTimer;

	private GameObject _closeButtonObject;

	private Button _closeButton;

	private Coroutine _verificationPollingCoroutine;

	public static UIPrivacyCoppaView Instance => _instance ?? (_instance = new UIPrivacyCoppaView());

	public override string GetLoadAssetPath()
	{
		return "Assets/Main/Prefabs/UI/LWUIPrivacy/UIPrivacyCoppa.prefab";
	}

	public override void OnCreate(GameObject go)
	{
		PrivacyFuncUtil.Instance.LogEvent("COPPA_DIALOG_OPEN", new Dictionary<string, object> { 
		{
			"s_type",
			PlayerPrefs.GetString("PRIVACY_COPPA_OPEN_FROM", "")
		} });
		_rootGameObject = go;
		InitializeComponents();
		InitializeView();
	}

	public override void OnDestroy()
	{
		PrivacyFuncUtil.Instance.LogEvent("COPPA_DIALOG_CLOSE", null);
		CleanupComponents();
		_instance = null;
	}

	private void InitializeComponents()
	{
		InitializePageObjects();
		InitializeAgeSelectionPage();
		InitializeConfirmationPage();
		InitializeEmailInputPage();
		InitializeParentConfirmationPage();
		InitializeResendEmailPage();
		InitializeCommonComponents();
	}

	private void InitializePageObjects()
	{
		_pageGameObjects = new Dictionary<string, GameObject>
		{
			["age1_go"] = _rootGameObject.transform.Find("Root/Age1").gameObject,
			["confirm2_go"] = _rootGameObject.transform.Find("Root/Confirm2").gameObject,
			["mail3_go"] = _rootGameObject.transform.Find("Root/Mail3").gameObject,
			["parent4_go"] = _rootGameObject.transform.Find("Root/Parent4").gameObject,
			["resend5_go"] = _rootGameObject.transform.Find("Root/Resend5").gameObject
		};
	}

	private void InitializeAgeSelectionPage()
	{
		string text = "Root/Age1/";
		_ageSelectionContentText = FindComponent<TextMeshProUGUIEx>(text + "Text2");
		_ageSelectionContentText.onPointerClick = OnPrivacyLinkClicked;
		_ageSelectionConfirmButton = FindComponent<Button>(text + "GoBtn1/LW_Btn_Common_New");
		_ageSelectionConfirmButton.onClick.AddListener(OnAgeSelectionConfirmed);
		_ageSlider = FindComponent<Slider>(text + "Slider");
		_ageSlider.minValue = 3f;
		_ageSlider.maxValue = 61f;
		_ageSlider.onValueChanged.AddListener(OnAgeSliderChanged);
		AddPointerUpListener(_ageSlider.gameObject, OnAgeSliderRelease);
		_ageDecrementButton = FindComponent<Button>(text + "subBtn");
		_ageDecrementButton.onClick.AddListener(delegate
		{
			AdjustAge(-1);
		});
		_ageIncrementButton = FindComponent<Button>(text + "addBtn");
		_ageIncrementButton.onClick.AddListener(delegate
		{
			AdjustAge(1);
		});
		AddButtonPressListener(_ageIncrementButton, OnIncrementButtonPressed, OnIncrementButtonReleased);
		_ageDisplayText = FindComponent<TextMeshProUGUIEx>(text + "Slider/HandleSlideArea/Handle/Image/TextAge");
	}

	private void AddPointerUpListener(GameObject go, UnityAction action)
	{
		EventTrigger eventTrigger = go.GetComponent<EventTrigger>();
		if (eventTrigger == null)
		{
			eventTrigger = go.AddComponent<EventTrigger>();
		}
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerUp;
		entry.callback.AddListener(delegate
		{
			action();
		});
		eventTrigger.triggers.Add(entry);
	}

	private void AddButtonPressListener(Button button, UnityAction onPressed, UnityAction onReleased)
	{
		EventTrigger eventTrigger = button.gameObject.GetComponent<EventTrigger>();
		if (eventTrigger == null)
		{
			eventTrigger = button.gameObject.AddComponent<EventTrigger>();
		}
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerDown;
		entry.callback.AddListener(delegate
		{
			onPressed();
		});
		eventTrigger.triggers.Add(entry);
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerUp;
		entry2.callback.AddListener(delegate
		{
			onReleased();
		});
		eventTrigger.triggers.Add(entry2);
	}

	private void OnIncrementButtonPressed()
	{
		if (!_ageIncrementButton.enabled)
		{
			return;
		}
		_isIncrementButtonPressed = true;
		CancelIncrementTimer();
		_incrementLongPressTimer = GameEntry.Timer.RegisterTimerRepeat(0.2f, 0.025f, delegate
		{
			if (_isIncrementButtonPressed)
			{
				AdjustAge(1);
			}
		});
	}

	private void OnIncrementButtonReleased()
	{
		_isIncrementButtonPressed = false;
		CancelIncrementTimer();
	}

	private void CancelIncrementTimer()
	{
		if (_incrementLongPressTimer != null)
		{
			GameEntry.Timer.CancelTimer(_incrementLongPressTimer);
			_incrementLongPressTimer = null;
		}
	}

	private void InitializeConfirmationPage()
	{
		string text = "Root/Confirm2/";
		_confirmationAgeText = FindComponent<TextMeshProUGUIEx>(text + "TextAge");
		_confirmationCancelButton = FindComponent<Button>(text + "GoBtn1/LW_Btn_Common_New");
		_confirmationCancelButton.onClick.AddListener(delegate
		{
			ShowPage("age1_go");
		});
		_confirmationConfirmButton = FindComponent<Button>(text + "GoBtn2/LW_Btn_Common_New");
		_confirmationConfirmButton.onClick.AddListener(OnAgeConfirmationConfirmed);
	}

	private void InitializeEmailInputPage()
	{
		string text = "Root/Mail3/";
		_emailInputField = FindComponent<TMP_InputField>(text + "InputFieldMail");
		_emailInputField.onValueChanged.AddListener(OnEmailInputChanged);
		_emailConfirmButton = FindComponent<Button>(text + "GoBtn1/LW_Btn_Common_New");
		_emailConfirmButton.onClick.AddListener(OnEmailConfirmed);
		_emailErrorObject = _rootGameObject.transform.Find(text + "TextMailError").gameObject;
	}

	private void InitializeParentConfirmationPage()
	{
		string text = "Root/Parent4/";
		_parentConfirmationText = FindComponent<TextMeshProUGUIEx>(text + "TextParent");
		_parentCancelButton = FindComponent<Button>(text + "GoBtn1/LW_Btn_Common_New");
		_parentCancelButton.onClick.AddListener(delegate
		{
			ShowPage("mail3_go");
		});
		_parentConfirmButton = FindComponent<Button>(text + "GoBtn2/LW_Btn_Common_New");
		_parentConfirmButton.onClick.AddListener(OnParentConfirmationConfirmed);
	}

	private void InitializeResendEmailPage()
	{
		string text = "Root/Resend5/";
		_emailSentSuccessObject = _rootGameObject.transform.Find(text + "Text2").gameObject;
		_contactUsButton = FindComponent<Button>(text + "GoBtn1/LW_Btn_Common_New");
		_contactUsButton.onClick.AddListener(OnContactUsClicked);
		_resendEmailButton = FindComponent<Button>(text + "GoBtn2/LW_Btn_Common_New");
		_resendEmailButton.onClick.AddListener(OnResendEmailClicked);
		_resendEmailText = FindComponent<TextMeshProUGUIEx>(text + "Image/TextMail");
		_resendButtonText = FindComponent<TextMeshProUGUIEx>(text + "GoBtn2/LW_Btn_Common_New/LW_Btn_Common_New_Base/BtnText");
	}

	private void InitializeCommonComponents()
	{
		_closeButtonObject = _rootGameObject.transform.Find("Root/LW_Btn_Close").gameObject;
		_closeButton = _closeButtonObject.GetComponent<Button>();
		_closeButton.onClick.AddListener(delegate
		{
			ClosePrivacyView();
		});
		string text = PlayerPrefs.GetString("PRIVACY_COPPA_OPEN_FROM", "Loading");
		_closeButtonObject.SetActive(text == "MainUI");
		_ageSelectionContentText.gameObject.SetActive(text == "Loading");
	}

	private void InitializeView()
	{
		string text = PlayerPrefs.GetString("PRIVACY_COPPA_CURRENT_STATE", "non_us_regions");
		DebugLog("Initializing COPPA view with state: " + text);
		if (!(text == "us_uncertified_need_mail"))
		{
			if (text == "us_uncertified_mail_pending")
			{
				_currentSelectedAge = PrivacyFuncUtil.Instance.GetCoppaAge();
				_emailInputField.text = PrivacyFuncUtil.Instance.GetCoppaEmail();
				_resendEmailText.text = _emailInputField.text;
				ShowPage("resend5_go");
				InitResendCooldown();
				StartVerificationPolling();
			}
			else
			{
				ShowPage("age1_go");
				ResetAgeSelection();
			}
		}
		else
		{
			_currentSelectedAge = PrivacyFuncUtil.Instance.GetCoppaAge();
			ShowPage("mail3_go");
		}
	}

	private void ShowPage(string pageName)
	{
		foreach (KeyValuePair<string, GameObject> pageGameObject in _pageGameObjects)
		{
			pageGameObject.Value.SetActive(pageGameObject.Key == pageName);
		}
	}

	private void ResetAgeSelection()
	{
		_ageSelectionContentText.text = GameEntry.Localization.GetString("coppa_selectAge_tips");
		_isAgeUnselected = true;
		_isFirstDrag = true;
		_dragCount = 0;
		UpdateAgeDisplay(3);
	}

	private void OnAgeSliderChanged(float value)
	{
		if (_isAgeUnselected)
		{
			_isAgeUnselected = false;
		}
		float num = Mathf.Pow((value - 3f) / 58f, 0.65f);
		int age = Mathf.RoundToInt(3f + num * 58f);
		UpdateAgeDisplay(age);
	}

	private void OnAgeSliderRelease()
	{
		if (_dragCount < 2)
		{
			_dragCount++;
			if (_currentSelectedAge < 13)
			{
				int num = UnityEngine.Random.Range(1, 4);
				int num2 = 12 + num;
				if (num2 > 61)
				{
					num2 = 61;
				}
				_isAgeUnselected = false;
				UpdateAgeDisplay(num2);
			}
		}
		if (_isFirstDrag)
		{
			_isFirstDrag = false;
			if (_currentSelectedAge <= 15)
			{
				UIGray.SetGray(_ageDecrementButton.transform, bGray: true);
			}
		}
	}

	private void AdjustAge(int delta)
	{
		int num = Mathf.Clamp(_currentSelectedAge + delta, 3, 61);
		if (num != _currentSelectedAge)
		{
			_isAgeUnselected = false;
			_isFirstDrag = false;
			UpdateAgeDisplay(num);
		}
	}

	private void UpdateAgeDisplay(int age)
	{
		_currentSelectedAge = age;
		if (_isAgeUnselected)
		{
			_ageDisplayText.text = "-";
			UIGray.SetGray(_ageDecrementButton.transform, bGray: true);
			UIGray.SetGray(_ageIncrementButton.transform, bGray: true);
			UIGray.SetGray(_ageSelectionConfirmButton.transform, bGray: true);
		}
		else
		{
			if (age == 61)
			{
				_ageDisplayText.text = $"{61}+";
			}
			else
			{
				_ageDisplayText.text = age.ToString();
			}
			UIGray.SetGray(_ageDecrementButton.transform, age <= 3, age > 3);
			UIGray.SetGray(_ageIncrementButton.transform, age >= 61, age < 61);
			UIGray.SetGray(_ageSelectionConfirmButton.transform, bGray: false, canClick: true);
		}
		_ageSlider.onValueChanged.RemoveListener(OnAgeSliderChanged);
		_ageSlider.value = age;
		_ageSlider.onValueChanged.AddListener(OnAgeSliderChanged);
	}

	private void OnAgeSelectionConfirmed()
	{
		if (_currentSelectedAge <= 12)
		{
			DebugLog($"Selected age {_currentSelectedAge} (child), showing confirmation");
			_confirmationAgeText.text = _currentSelectedAge.ToString();
			ShowPage("confirm2_go");
		}
		else
		{
			DebugLog($"Selected age {_currentSelectedAge} (adult), creating account");
			ConfirmAge_CreateAccountOrVerify(_currentSelectedAge, isAdult: true);
		}
	}

	private void OnAgeConfirmationConfirmed()
	{
		ConfirmAge_CreateAccountOrVerify(_currentSelectedAge, isAdult: false);
	}

	private void OnEmailInputChanged(string email)
	{
		bool flag = string.IsNullOrEmpty(email);
		bool flag2 = !flag && Regex.IsMatch(email, "^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$");
		_emailErrorObject.SetActive(!flag && !flag2);
		UIGray.SetGray(_emailConfirmButton.transform, !flag2, flag2);
	}

	private void OnEmailConfirmed()
	{
		string text = _emailInputField.text;
		if (ValidateEmail(text))
		{
			PrivacyFuncUtil.Instance.LogEvent("COPPA_EMAIL_INPUT", new Dictionary<string, object> { { "info", text } });
			ShowPage("parent4_go");
			_parentConfirmationText.text = GameEntry.Localization.GetString("coppa_parentEmail_tips2", text);
		}
	}

	private bool ValidateEmail(string email)
	{
		if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, "^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$"))
		{
			_emailErrorObject.SetActive(value: true);
			DebugLog("Invalid email format");
			return false;
		}
		return true;
	}

	private void OnParentConfirmationConfirmed()
	{
		ShowPage("resend5_go");
		SendEmailVerification();
		StartVerificationPolling();
	}

	private void SendEmailVerification()
	{
		string text = _emailInputField.text;
		DebugLog("Sending email verification to: " + text);
		ShowEmailSentSuccess();
		_resendEmailText.text = text;
		StartResendCooldown();
		SubmitAgeVerification(_currentSelectedAge, text);
	}

	private void ShowEmailSentSuccess()
	{
		_emailSentSuccessObject.SetActive(value: true);
	}

	private void InitResendCooldown()
	{
		if (long.TryParse(PlayerPrefs.GetString("PRIVACY_COPPA_NEXT_RESEND_TIME", "0"), out var result))
		{
			long num = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			int remainingSeconds = (int)Math.Max(0L, result - num);
			if (remainingSeconds > 0)
			{
				UIGray.SetGray(_resendEmailButton.transform, bGray: true);
				UpdateResendButtonText(remainingSeconds);
				_resendCooldownTimer = GameEntry.Timer.RegisterTimerRepeat(1f, 1f, delegate
				{
					remainingSeconds--;
					UpdateResendButtonText(remainingSeconds);
					if (remainingSeconds <= 0)
					{
						EndResendCooldown();
					}
				});
			}
			else
			{
				UpdateResendButtonText(0);
			}
		}
		else
		{
			UpdateResendButtonText(0);
		}
	}

	private void StartResendCooldown()
	{
		PlayerPrefs.SetString("PRIVACY_COPPA_NEXT_RESEND_TIME", (DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 180).ToString());
		PlayerPrefs.Save();
		int remainingSeconds = 180;
		UIGray.SetGray(_resendEmailButton.transform, bGray: true);
		UpdateResendButtonText(remainingSeconds);
		CancelResendTimer();
		_resendCooldownTimer = GameEntry.Timer.RegisterTimerRepeat(1f, 1f, delegate
		{
			remainingSeconds--;
			UpdateResendButtonText(remainingSeconds);
			if (remainingSeconds < 176)
			{
				_emailSentSuccessObject.SetActive(value: false);
			}
			if (remainingSeconds <= 0)
			{
				EndResendCooldown();
			}
		});
	}

	private void UpdateResendButtonText(int seconds)
	{
		_resendButtonText.text = ((seconds > 0) ? string.Format("{0}({1})", GameEntry.Localization.GetString("280117"), seconds) : GameEntry.Localization.GetString("280117"));
	}

	private void EndResendCooldown()
	{
		UIGray.SetGray(_resendEmailButton.transform, bGray: false, canClick: true);
		CancelResendTimer();
	}

	private void CancelResendTimer()
	{
		if (_resendCooldownTimer != null)
		{
			GameEntry.Timer.CancelTimer(_resendCooldownTimer);
			_resendCooldownTimer = null;
		}
	}

	private void OnResendEmailClicked()
	{
		SendEmailVerification();
	}

	private void OnContactUsClicked()
	{
		AIHelpProxy.Show("E019", GameEntry.Localization.GetString("290045"));
	}

	private void ConfirmAge_CreateAccountOrVerify(int age, bool isAdult)
	{
		AccountData accountData = PrivacyFuncUtil.Instance.GetAccountData();
		if (accountData == null || accountData.accountInfo == null || string.IsNullOrEmpty(accountData.accountInfo.airKey))
		{
			ConfirmAge_CreateAccount(age, isAdult);
		}
		else
		{
			ConfirmAge_Verify(age, isAdult);
		}
		PrivacyFuncUtil.Instance.LogEvent("COPPA_AGE_CONFIRM", new Dictionary<string, object> { { "i_common_num", age } });
	}

	private void ConfirmAge_CreateAccount(int age, bool isAdult)
	{
		KWSVerificationManager.Instance.CreateAccount(PrivacyFuncUtil.Instance.GetAirKey(), PrivacyFuncUtil.Instance.GetCountry(), PrivacyFuncUtil.Instance.GetLanguage(), age, !isAdult, PrivacyFuncUtil.Instance.GetUid(considerNewPlayerFlag: true), PrivacyFuncUtil.Instance.GetZone(considerNewPlayerFlag: true), PrivacyFuncUtil.Instance.GetConfirmDelayDays(), delegate(CreateAccountResponse response)
		{
			DebugLog($"ConfirmAge_CreateAccount successfully. Adult: {isAdult}");
			PrivacyFuncUtil.Instance.SaveAccountData(response.data);
			ConfirmAgeProceed(isAdult);
		}, delegate(string error)
		{
			HandleError("CreateAccount", error);
			PrivacyFuncUtil.Instance.LogEvent_CoppaStateException(error);
		});
	}

	private void ConfirmAge_Verify(int age, bool isAdult)
	{
		KWSVerificationManager.Instance.VerifyAge(PrivacyFuncUtil.Instance.GetAirKey(), PrivacyFuncUtil.Instance.GetCountry(), PrivacyFuncUtil.Instance.GetLanguage(), "", age, delegate
		{
			DebugLog("ConfirmAge_Verify submitted successfully");
			AccountData accountData = PrivacyFuncUtil.Instance.GetAccountData();
			accountData.accountInfo.age = age.ToString();
			accountData.accountInfo.email = "";
			PrivacyFuncUtil.Instance.SaveAccountData(accountData);
			ConfirmAgeProceed(isAdult);
		}, delegate(string error)
		{
			HandleError("VerifyAge", error);
			PrivacyFuncUtil.Instance.LogEvent_CoppaStateException(error);
		});
	}

	private void ConfirmAgeProceed(bool isAdult)
	{
		if (isAdult)
		{
			CompleteVerification("us_certified_adult");
		}
		else
		{
			ShowPage("mail3_go");
		}
	}

	private void SubmitAgeVerification(int age, string parentEmail)
	{
		PrivacyFuncUtil.Instance.LogEvent("COPPA_EMAIL_SEND", new Dictionary<string, object>
		{
			{ "i_common_num", age },
			{ "info", parentEmail }
		});
		KWSVerificationManager.Instance.VerifyAge(PrivacyFuncUtil.Instance.GetAirKey(), PrivacyFuncUtil.Instance.GetCountry(), PrivacyFuncUtil.Instance.GetLanguage(), parentEmail, age, delegate
		{
			DebugLog("Age verification submitted successfully");
			AccountData accountData = PrivacyFuncUtil.Instance.GetAccountData();
			accountData.accountInfo.age = age.ToString();
			accountData.accountInfo.email = parentEmail;
			PrivacyFuncUtil.Instance.SaveAccountData(accountData);
			PrivacyFuncUtil.Instance.LogEvent("COPPA_EMAIL_SUCCESS", new Dictionary<string, object>
			{
				{ "i_common_num", age },
				{ "info", parentEmail }
			});
		}, delegate(string error)
		{
			HandleError("VerifyAge", error);
			PrivacyFuncUtil.Instance.LogEvent("COPPA_EMAIL_FAIL", null);
			PrivacyFuncUtil.Instance.LogEvent_CoppaStateException(error);
		});
	}

	private void StartVerificationPolling()
	{
		DebugLog("Starting verification status polling...");
		if (_verificationPollingCoroutine != null)
		{
			KWSVerificationManager.Instance.StopVerificationPolling(_verificationPollingCoroutine);
			_verificationPollingCoroutine = null;
		}
		_verificationPollingCoroutine = KWSVerificationManager.Instance.StartVerificationPolling(PrivacyFuncUtil.Instance.GetAirKey(), PrivacyFuncUtil.Instance.GetCountry(), PrivacyFuncUtil.Instance.GetLanguage(), PrivacyFuncUtil.Instance.GetUid(considerNewPlayerFlag: true), PrivacyFuncUtil.Instance.GetZone(considerNewPlayerFlag: true), delegate(AccountInfoResponse response)
		{
			DebugLog("Parent verification completed!");
			PrivacyFuncUtil.Instance.SaveAccountData(response.data);
			CompleteVerification("us_certified_children");
			PrivacyFuncUtil.Instance.LogEvent("COPPA_PARENT_CONFIRM", new Dictionary<string, object>
			{
				{
					"i_common_num",
					response.data.accountInfo.age
				},
				{
					"info",
					response.data.accountInfo.email
				}
			});
		}, delegate(string error)
		{
			HandleError("Verification", error);
			PrivacyFuncUtil.Instance.LogEvent_CoppaStateException(error);
		}, 600);
	}

	public void CompleteVerification(string coppaState)
	{
		DebugLog("Completing COPPA verification with state: " + coppaState);
		PlayerPrefs.SetString("PRIVACY_COPPA_CURRENT_STATE", coppaState);
		RecordPrivacyAcceptanceEvent();
		GameEntry.Event.Fire(EventId.UIPrivacy_Confirm);
		ClosePrivacyView();
	}

	private void RecordPrivacyAcceptanceEvent()
	{
		List<string> links = ExtractPrivacyLinksFromContent();
		string prop = BuildPrivacyEventData(links);
		PostEventLog.Track("DMA_AGREE_RECORD", prop);
	}

	private List<string> ExtractPrivacyLinksFromContent()
	{
		List<string> list = new List<string>();
		_ageSelectionContentText.ForceMeshUpdate();
		TMP_LinkInfo[] linkInfo = _ageSelectionContentText.textInfo.linkInfo;
		for (int i = 0; i < linkInfo.Length; i++)
		{
			string linkID = linkInfo[i].GetLinkID();
			if (!string.IsNullOrEmpty(linkID))
			{
				list.Add(linkID);
			}
		}
		return list;
	}

	private string BuildPrivacyEventData(List<string> links)
	{
		if (links.Count == 0)
		{
			return "{}";
		}
		StringBuilder stringBuilder = new StringBuilder("{");
		for (int i = 0; i < Math.Min(links.Count, 4); i++)
		{
			if (i > 0)
			{
				stringBuilder.Append(",");
			}
			stringBuilder.Append($"\"s_para{i + 1}\":\"{links[i]}\"");
		}
		stringBuilder.Append("}");
		return stringBuilder.ToString();
	}

	private void HandleError(string where, string error)
	{
		Debug.LogError("[COPPA] " + where + " error occurred: " + error);
		PlayerPrefs.SetString("PRIVACY_COPPA_CURRENT_STATE", "non_us_regions");
		GameEntry.Event.Fire(EventId.UIPrivacy_Confirm);
		ClosePrivacyView();
	}

	private void OnPrivacyLinkClicked(PointerEventData eventData)
	{
		int num = TMP_TextUtilities.FindIntersectingLink(_ageSelectionContentText, eventData.position, eventData.pressEventCamera);
		if (num != -1)
		{
			string linkID = _ageSelectionContentText.textInfo.linkInfo[num].GetLinkID();
			if (!string.IsNullOrEmpty(linkID))
			{
				SDKManager.OpenURL(linkID);
			}
		}
	}

	private void CleanupComponents()
	{
		if (_verificationPollingCoroutine != null)
		{
			KWSVerificationManager.Instance.StopVerificationPolling(_verificationPollingCoroutine);
			_verificationPollingCoroutine = null;
		}
		CancelResendTimer();
		RemoveAllListeners();
		ClearReferences();
		CancelIncrementTimer();
	}

	private void RemoveAllListeners()
	{
		_ageSelectionContentText.onPointerClick = null;
		_ageSelectionConfirmButton.onClick.RemoveAllListeners();
		_ageSlider.onValueChanged.RemoveAllListeners();
		_ageDecrementButton.onClick.RemoveAllListeners();
		_ageIncrementButton.onClick.RemoveAllListeners();
		_confirmationCancelButton.onClick.RemoveAllListeners();
		_confirmationConfirmButton.onClick.RemoveAllListeners();
		_emailInputField.onValueChanged.RemoveAllListeners();
		_emailConfirmButton.onClick.RemoveAllListeners();
		_parentCancelButton.onClick.RemoveAllListeners();
		_parentConfirmButton.onClick.RemoveAllListeners();
		_contactUsButton.onClick.RemoveAllListeners();
		_resendEmailButton.onClick.RemoveAllListeners();
		_closeButton.onClick.RemoveAllListeners();
	}

	private void ClearReferences()
	{
		_pageGameObjects?.Clear();
		_pageGameObjects = null;
		_rootGameObject = null;
		_ageSelectionContentText = null;
		_ageSelectionConfirmButton = null;
		_ageSlider = null;
		_ageDecrementButton = null;
		_ageIncrementButton = null;
		_ageDisplayText = null;
		_confirmationAgeText = null;
		_confirmationCancelButton = null;
		_confirmationConfirmButton = null;
		_emailInputField = null;
		_emailConfirmButton = null;
		_emailErrorObject = null;
		_parentConfirmationText = null;
		_parentCancelButton = null;
		_parentConfirmButton = null;
		_contactUsButton = null;
		_resendEmailButton = null;
		_resendEmailText = null;
		_resendButtonText = null;
		_emailSentSuccessObject = null;
		_closeButtonObject = null;
		_closeButton = null;
	}

	private T FindComponent<T>(string path) where T : Component
	{
		return _rootGameObject.transform.Find(path).GetComponent<T>();
	}

	private void DebugLog(string message)
	{
	}
}
