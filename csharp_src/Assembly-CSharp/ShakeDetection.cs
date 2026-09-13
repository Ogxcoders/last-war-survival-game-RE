using UnityEngine;

public class ShakeDetection : MonoBehaviour
{
	private float previousSpeed;

	private float SHAKE_THRESHOLD = 13f;

	private float SHAKE_DELAY_TIME = 1f;

	private int SHAKE_TIME_COUNT = 2;

	public float SHAKE_CD;

	public bool mUseShakeDetection = true;

	public bool mIsCrossServer;

	public bool mIsPC;

	public bool mIsDebug;

	public bool mIsDawn;

	private float cd;

	private float preASpeed;

	private float delayTime;

	private float checkTimes;

	private bool mSupportsGyroscope;

	private float timer;

	private const float CHECK_INTERVAL = 0.1f;

	public void Start()
	{
		mSupportsGyroscope = SystemInfo.supportsGyroscope;
		SHAKE_CD = GameEntry.Lua.CallWithReturn<float, string, string>("CSharpCallLuaInterface.GetConfigStr", "s4_disco", "k3");
		SHAKE_THRESHOLD = GameEntry.Lua.CallWithReturn<float, string, string>("CSharpCallLuaInterface.GetConfigStr", "shakeCollectRes_config", "k2");
		SHAKE_DELAY_TIME = GameEntry.Lua.CallWithReturn<float, string, string>("CSharpCallLuaInterface.GetConfigStr", "shakeCollectRes_config", "k3");
		SHAKE_TIME_COUNT = GameEntry.Lua.CallWithReturn<int, string, string>("CSharpCallLuaInterface.GetConfigStr", "shakeCollectRes_config", "k4");
		GameEntry.Event.Subscribe(EventId.OnEnterCrossServer, OnEnterCrossServer);
		GameEntry.Event.Subscribe(EventId.OnQuitCrossServer, OnQuitCrossServer);
		GameEntry.Event.Subscribe(EventId.BloodyNightActivityRefresh, OnBloodyNightActivityRefresh);
		mUseShakeDetection = true;
		mIsDebug = CommonUtils.IsDebug();
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		mIsDawn = IsDawn(curServerId);
		if (mIsDawn)
		{
			StopDetection();
		}
	}

	public bool IsDawn(int targetServerId)
	{
		return GameEntry.Lua.CallWithReturn<bool, int>("CSharpCallLuaInterface.IsDawn", targetServerId);
	}

	public void OnDestroy()
	{
		GameEntry.Event.Unsubscribe(EventId.OnEnterCrossServer, OnEnterCrossServer);
		GameEntry.Event.Unsubscribe(EventId.OnQuitCrossServer, OnQuitCrossServer);
		GameEntry.Event.Unsubscribe(EventId.BloodyNightActivityRefresh, OnBloodyNightActivityRefresh);
	}

	public void StopDetection()
	{
		mUseShakeDetection = false;
	}

	private void OnEnterCrossServer(object obj)
	{
		mIsCrossServer = false;
	}

	private void OnQuitCrossServer(object obj)
	{
		mIsCrossServer = true;
	}

	public void OnBloodyNightActivityRefresh(object serverId)
	{
		if (!(serverId is long))
		{
			return;
		}
		int num = serverId.ToInt();
		int curServerId = GameEntry.Data.Player.GetCurServerId();
		if (curServerId == num)
		{
			mIsDawn = IsDawn(curServerId);
			if (mIsDawn)
			{
				StopDetection();
			}
		}
	}

	private void Update()
	{
		if (!mUseShakeDetection || mIsPC || mIsCrossServer)
		{
			return;
		}
		timer += Time.deltaTime;
		if (!(timer >= 0.1f))
		{
			return;
		}
		timer = 0f;
		float num = (int)Time.time;
		bool flag = IsShaking();
		if (num > cd)
		{
			if (flag || (mIsDebug && Input.GetKeyDown(KeyCode.F12)))
			{
				RequestPlayMainBaseDiscoEffect();
				cd = SHAKE_CD + num;
				if (Vibrator.HapticsSupported())
				{
					Vibrator.Warning();
				}
			}
		}
		else if (flag || (mIsDebug && Input.GetKeyDown(KeyCode.F12)))
		{
			string text = (cd - num).ToString("F2");
			string param = GameEntry.Localization.GetString("season_s4_lighthouse_tips_7", text);
			GameEntry.Lua.Call("CSharpCallLuaInterface.OnWorldBasePlayDiscoShowTips", param);
		}
	}

	public bool IsShaking()
	{
		if (mSupportsGyroscope)
		{
			Gyroscope gyro = Input.gyro;
			gyro.enabled = true;
			float z = gyro.rotationRateUnbiased.z;
			if (Mathf.Abs(z) > SHAKE_THRESHOLD)
			{
				if (previousSpeed == 0f)
				{
					previousSpeed = z;
					delayTime = 0f;
					checkTimes = 0f;
				}
				else if ((previousSpeed > 0f && z < SHAKE_THRESHOLD) || (previousSpeed < 0f && z > SHAKE_THRESHOLD))
				{
					checkTimes += 1f;
				}
				if (checkTimes >= (float)SHAKE_TIME_COUNT)
				{
					previousSpeed = 0f;
					checkTimes = 0f;
					delayTime = 0f;
					return true;
				}
				previousSpeed = z;
			}
			if (previousSpeed != 0f)
			{
				delayTime += 0.1f;
				if (delayTime > SHAKE_DELAY_TIME)
				{
					previousSpeed = 0f;
					checkTimes = 0f;
					delayTime = 0f;
				}
			}
		}
		return false;
	}

	public void RequestPlayMainBaseDiscoEffect()
	{
		GameEntry.Event.Fire(EventId.OnPhoneShakeHappen);
	}
}
