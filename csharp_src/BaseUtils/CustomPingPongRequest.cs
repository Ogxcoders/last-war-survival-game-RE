using BaseUtils;
using Sfs2X;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;

public class CustomPingPongRequest : BaseRequest
{
	public CustomPingPongRequest()
		: base(RequestType.PingPong)
	{
	}

	public override void Validate(SmartFox sfs)
	{
	}

	public override void Execute(SmartFox sfs)
	{
		SFSObject sFSObject = new SFSObject();
		sFSObject.PutUtfString("appVersion", "1.0.211");
		sFSObject.PutUtfString("pfId", "");
		sFSObject.PutUtfString("androidDid", "f84860f342011xxxx667b442a0251f0a");
		sFSObject.PutUtfString("afuid", "1714554715325-2686560xxx180113866");
		string val = PlayerPrefs.GetString("DEVICE_ID", "");
		sFSObject.PutUtfString("deviceId", val);
		sFSObject.PutUtfString("uuid", "500c0cd5-xxxx-4e74-bc63-e761441a64be_n3d1714554740611");
		sFSObject.PutUtfString("serverId", "499");
		sFSObject.PutUtfString("packageSign", "870dafxxxx073b3a60fxxxxfc6eb22845a1a8497");
		sFSObject.PutUtfString("resVersion", "1672.1293.1528.1013.xxxx.1098_84B_5334.5345");
		sFSObject.PutInt("delete_account_status", 0);
		sFSObject.PutUtfString("CoreV", "3fcf05fe3253b132a2926xxxx12c2345f34218312000c96bbfb0619e88f9f707");
		sFSObject.PutUtfString("googleName", "");
		sFSObject.PutUtfString("SecurityCode", "16b75bb7e04f7xxxx0ef77ebb6fb5cdd");
		sFSObject.PutUtfString("deeplinkParams", "");
		sFSObject.PutUtfString("simOp", "44051");
		sFSObject.PutInt("lat", 0);
		sFSObject.PutUtfString("OneCode", "14c6912061797505d6101xxxxxd89217a077a90151929547c5f7e000d0024981");
		sFSObject.PutUtfString("dataConfigMd5", "49913a6cc6cc5xxxxx9bcec87bea83ea219_df8d7e54f6ddb02828435bcd2aefc2370_1.0.211");
		sFSObject.PutUtfString("fromCountry", "JP");
		SFSObject val2 = new SFSObject();
		sFSObject.PutSFSObject("configVersion", val2);
		sFSObject.PutInt("netType", 2);
		sFSObject.PutInt("gmLogin", 0);
		sFSObject.PutUtfString("mt", "Model:CPH1893,SDKVersion:28,SYSVersion:9,xxxxx:OPPO,SimProvider:44051,NetWork:4G");
		sFSObject.PutInt("google_available", 1);
		sFSObject.PutUtfString("cmdBaseTime", "zxzxzxzxzxzx");
		sFSObject.PutUtfString("gameUid", "xxxxxxxxxxxxzzzxxxzzx");
		sFSObject.PutUtfString("firebaseId", "1f74dzxzxzx166267d2e19caf53f421c5");
		sFSObject.PutUtfString("distinct_id", "9acb5b0c-zxzx-463f-ab1c-cc072552d8db");
		sFSObject.PutInt("_id", 1);
		sFSObject.PutInt("configNumber", 0);
		sFSObject.PutUtfString("psh", "08a926bd21fdea8774zxzxfb99c78072");
		sFSObject.PutUtfString("gaid", "500c0cd5-745d-zxzx-zxzx-zxzxzx41a64be");
		sFSObject.PutUtfString("AndroidID", "unknown");
		sFSObject.PutUtfString("platform", "1");
		sFSObject.PutUtfString("phone_screen", "1080*2340");
		sFSObject.PutUtfString("osVersion", "Android OS 9 / API-28 (zxzx.190414.001/1625220276)");
		sFSObject.PutInt("forbidden_froce_merge", 1);
		sFSObject.PutUtfString("packageName", "com.fun.lastwar.gp");
		sFSObject.PutUtfString("lang", "ja");
		sFSObject.PutUtfString("IMEI", "sdm660 qcom OPPO arm64-v8a  zxzxzxzx OPPO unknown CPH1893 1080 2132 480 OPPO/CPH1893/CPH1893:9/PKQ1.190414.001/2021070000:user/release-keys OPPO CPH1893 ");
		sFSObject.PutUtfString("ta", "{\"#carrier\":\"KDDI\", \"#os\":\"ccccxx\", \"#device_id\":\"feb8c2cc90305c9a\", \"#screen_height\":2340, \"#bundle_id\":\"com.fun.lastwar.gp\", \"#device_model\":\"CPH1893\", \"#screen_width\":1080, \"#system_language\":\"ja\", \"#install_time\":\"2024-05-01 18:11:34.765\", \"#simulator\":false, \"#manufacturer\":\"OPPO\", \"#os_version\":\"9\", \"#app_version\":\"1.0.211\", \"#network_type\":\"4G\", \"#zone_offset\":9, \"#ram\":\"1.0/3.6\", \"#disk\":\"25.5/106.4\", \"#fps\":60, \"#device_type\":\"Phone\", \"lw_first_launch\":false, \"lw_game_session_id\":\"34e97520-045a-4431-8bbb-dbd96zxzx98c6\", \"lw_device_id\":\"500c0cd5-zxzx-zxzx-zxzx-zxzxzx441a64be_n3d\", \"lw_zone\":\"APS499\", \"lw_version\":\"1.0.211\", \"lw_res_version\":\"1672.1293.1528.1013.1025.1098_84B_5334.5338\", \"lw_buildcode\":\"803\", \"lw_net\":\"4g\", \"lw_line\":\"lastwar-game-us-gcp.laszxzxgame.com\", \"lw_platform\":\"market_global\", \"lw_main_level\":11, \"pd_dl\":1, \"lw_ab\":\"0\", \"lw_alliance_id\":\"b8241262c158zxzx016436c4aad79\", \"lw_power\":16zx863}");
		sFSObject.PutUtfString("versionCode", "803");
		sFSObject.PutUtfString("phone_model", "OPPO zxzxzxzx");
		sFSObject.PutUtfString("KCPMode", "0");
		sFSObject.PutUtfString("googlePlay", "");
		sFSObject.PutUtfString("parseRegisterId", "edXzgp_sQSCDkBWVuqUxky:zxzxzxbFV5rvt2Jo9zxzxgcd0Le8hXx-XIAREbXdMfFsgjS5AMyEaqoBqpuVtYPc1S34E0DkRf9OvZMWju1j5RKKXIE1ZX7-m-37A0d5kWOb4NiLpHneO1T4CARQTy9lQ7Iyv-tGNVsU");
		sFSObject.PutUtfString("pf", "market_global");
		sFSObject.PutUtfString("device_string", "Model=OPPO CPH1893|Memory=3711|Vendor=Qualcomm|Procezxzx=1958|Graphics=Adreno (TM) 512|DeviceType=OpenGL3.2-ASTC-ETC2-ASTC_4x4-ASTC_6x6|");
		sFSObject.PutUtfString("simOpName", "KDDI");
		sFSObject.PutLong("clientTime", RealTimer.elapsedMilliseconds);
		sfso.PutSFSObject(LoginRequest.KEY_PARAMS, sFSObject);
	}
}
