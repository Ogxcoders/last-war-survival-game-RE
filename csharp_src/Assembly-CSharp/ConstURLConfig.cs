using System.Collections.Generic;

public static class ConstURLConfig
{
	public static readonly string[] cdns = new string[2] { "https://lastwar-cdn.akamaized.net/hotupdate/", "https://lastwar-cdn.lastwarapp.com/hotupdate/" };

	public const string debugDownloadURL_ = "http://lw-local-s188.rivergame.net/hotupdate/";

	public const string package_name_debug = "com.fun.lastwar.debug";

	public const string package_name_gp = "com.fun.lastwar.gp";

	public const string package_name_ios = "com.lastwar.ios";

	public const string package_name_pc = "com.lastwar.pc";

	public const string checkVersionFormFormat = "/gameservice/getlsu3dversion.php?packageName={0}&platform={1}&appVersion={2}&gm={3}&server={4}&uid={5}&deviceId={6}&table_env={7}&returnJson=1";

	public const string checkVersionFormFormatWithBuildId = "/gameservice/getlsu3dversion.php?packageName={0}&platform={1}&appVersion={2}&gm={3}&server={4}&uid={5}&deviceId={6}&table_env={7}&buildId={8}&returnJson=1";

	public static readonly string[] onlineCheckVersionHostList = new string[3] { "https://lastwar-serverlist-us-cf-ali.lastwarapp.com", "https://lastwar-serverlist-us-aws-ali.lastwargame.com", "https://lastwar-serverlist-us-gcp-ali.lastwargame.com" };

	public static readonly string[] online_ea_CheckVersionHostList = new string[2] { "https://lastwar-serverlist-ea-us-aws.lastwargame.com", "https://lastwar-serverlist-ea-us-gcp.lastwargame.com" };

	public static readonly string[] pressureTestCheckVersionHostList = new string[1] { "https://yace-lastwar-serverlist-va-ali.lastwargame.com" };

	public static readonly string[] awsCheckVersionHostList = new string[1] { "https://lastwar-server-list-aws-test-1.lastwargame.com" };

	public static readonly string[] vietnamCheckVersionHostList = new string[1] { "https://lastwar-game-vietnam.lastwargame.com" };

	public static readonly string[] tencentCheckVersionHostList = new string[1] { "https://lastwar-serverlist-cn-1.rivergame.net" };

	public static readonly string[] GCPCheckVersionHostList = new string[1] { "https://lastwar-serverlist-gcp-test-1.lastwargame.com" };

	public static readonly string[] LocalCheckVersionHostList = new string[1] { "http://lw-local-gm.rivergame.net:8988" };

	public static Dictionary<URLGroupType, string[]> GetServerListConfigDic = new Dictionary<URLGroupType, string[]>
	{
		{
			URLGroupType.Online,
			onlineCheckVersionHostList
		},
		{
			URLGroupType.PressureTest,
			pressureTestCheckVersionHostList
		},
		{
			URLGroupType.AWS,
			awsCheckVersionHostList
		},
		{
			URLGroupType.Vietnam,
			vietnamCheckVersionHostList
		},
		{
			URLGroupType.Tencent,
			tencentCheckVersionHostList
		},
		{
			URLGroupType.GCP,
			GCPCheckVersionHostList
		},
		{
			URLGroupType.Local,
			LocalCheckVersionHostList
		}
	};

	public static readonly string debugGateServer = "http://lw-local-gm.rivergame.net:8989";

	public static readonly string debugNoticeServer = "http://lw-local-gm.rivergame.net:8988";

	public static readonly string onlineGateServer_CN = "http://lw-local-gm.rivergame.net:8989/gameservice/getserverlist.php";

	public static string OnlineBattleReportCDN = "https://lastwar-fight-report.akamaized.net/report/";

	public static string DebugBattleReportCDN = "https://lastwar-fight-report-office.oss-cn-beijing.aliyuncs.com/report/";

	public static string PressureTestBattleReportCDN = "https://yace-lastwar-fight-report.oss-us-east-1.aliyuncs.com/report/";

	public static string TencentBattleReportCDN = "https://lastwar-fight-test-cn-1256721769.cos.ap-beijing.myqcloud.com/report/";

	public static string AwsBattleReportCDN = "https://lastwar-fight-report-yace.s3-accelerate.amazonaws.com/report/";

	public static string OnlineDownloadBattleReportCDN = "https://lastwar-fight-report.akamaized.net/reportFrame/";

	public static string DebugDownloadBattleReportCDN = "https://lastwar-fight-report-office.oss-cn-beijing.aliyuncs.com/reportFrame/";

	public static string PressureDownloadTestBattleReportCDN = "https://yace-lastwar-fight-report.oss-us-east-1.aliyuncs.com/reportFrame/";

	public static string TencentDownloadBattleReportCDN = "https://lastwar-fight-test-cn-1256721769.cos.ap-beijing.myqcloud.com/reportFrame/";

	public static string AwsDownloadBattleReportCDN = "https://lastwar-fight-report-yace.s3-accelerate.amazonaws.com/reportFrame/";

	public static string OnlineMailRankDataCDN = "https://lastwar-fight-report.akamaized.net/rankMail/";

	public static string DebugMailRankDataCDN = "https://lastwar-fight-report-office.oss-cn-beijing.aliyuncs.com/rankMail/";

	public static string PressureTestMailRankDataCDN = "https://yace-lastwar-fight-report.oss-us-east-1.aliyuncs.com/rankMail/";

	public static string TencentMailRankDataCDN = "https://lastwar-fight-test-cn-1256721769.cos.ap-beijing.myqcloud.com/rankMail/";

	public static string AwsMailRankDataCDN = "https://lastwar-fight-report-yace.s3-accelerate.amazonaws.com/rankMail/";

	public static string OnlineBattleReportAddressCDN = "https://lastwar-fight-report.akamaized.net/";

	public static string DebugBattleReportAddressCDN = "https://lastwar-fight-report-office.oss-cn-beijing.aliyuncs.com/";

	public static string PressureTestBattleReportAddressCDN = "https://yace-lastwar-fight-report.oss-us-east-1.aliyuncs.com/";

	public static string TencentBattleReportAddressCDN = "https://lastwar-fight-test-cn-1256721769.cos.ap-beijing.myqcloud.com/";

	public static string AmazonBattleReportAddressCDN = "https://lastwar-fight-report-yace.s3-accelerate.amazonaws.com/";

	public static string AmazonOfficialBattleReportAddressCDN = "https://lastwar-fight-report-s3.akamaized.net/";

	public static string OnlineBattleReportAddressBackupCDN1 = "https://lastwar-fight-report-us.oss-accelerate.aliyuncs.com/";

	public static string AmazonOfficialBattleReportAddressBackupCDN1 = "https://lastwar-fight-report-s3.s3-accelerate.amazonaws.com/";

	public static string OnlineBattleReportAddressPrefix = "ali://";

	public static string DebugBattleReportAddressPrefix = "ali_local://";

	public static string PressureTestBattleReportAddressPrefix = "ali_pt://";

	public static string TencentBattleReportAddressPrefix = "tencent://";

	public static string AmazonBattleReportAddressPrefix = "aws_pt://";

	public static string AmazonOfficialBattleReportAddressPrefix = "aws://";

	public static Dictionary<URLGroupType, string> BattleReportConfigDic = new Dictionary<URLGroupType, string>
	{
		{
			URLGroupType.Online,
			OnlineBattleReportCDN
		},
		{
			URLGroupType.PressureTest,
			PressureTestBattleReportCDN
		},
		{
			URLGroupType.Tencent,
			TencentBattleReportCDN
		},
		{
			URLGroupType.Local,
			DebugBattleReportCDN
		},
		{
			URLGroupType.AWS,
			AwsBattleReportCDN
		}
	};

	public static Dictionary<URLGroupType, string> BattleReportDownloadConfigDic = new Dictionary<URLGroupType, string>
	{
		{
			URLGroupType.Online,
			OnlineDownloadBattleReportCDN
		},
		{
			URLGroupType.PressureTest,
			PressureDownloadTestBattleReportCDN
		},
		{
			URLGroupType.Tencent,
			TencentDownloadBattleReportCDN
		},
		{
			URLGroupType.Local,
			DebugDownloadBattleReportCDN
		},
		{
			URLGroupType.AWS,
			AwsDownloadBattleReportCDN
		}
	};

	public static Dictionary<URLGroupType, string> MailRankDataConfigDic = new Dictionary<URLGroupType, string>
	{
		{
			URLGroupType.Online,
			OnlineMailRankDataCDN
		},
		{
			URLGroupType.PressureTest,
			PressureTestMailRankDataCDN
		},
		{
			URLGroupType.Tencent,
			TencentMailRankDataCDN
		},
		{
			URLGroupType.Local,
			DebugMailRankDataCDN
		},
		{
			URLGroupType.AWS,
			AwsMailRankDataCDN
		}
	};

	public static Dictionary<BattleReportOSSURLType, string> BattleReportAddressConfigDic = new Dictionary<BattleReportOSSURLType, string>
	{
		{
			BattleReportOSSURLType.Online,
			OnlineBattleReportAddressCDN
		},
		{
			BattleReportOSSURLType.PressureTest,
			PressureTestBattleReportAddressCDN
		},
		{
			BattleReportOSSURLType.Tencent,
			TencentBattleReportAddressCDN
		},
		{
			BattleReportOSSURLType.Local,
			DebugBattleReportAddressCDN
		},
		{
			BattleReportOSSURLType.Amazon,
			AmazonBattleReportAddressCDN
		},
		{
			BattleReportOSSURLType.AmazonOfficial,
			AmazonOfficialBattleReportAddressCDN
		}
	};

	public static Dictionary<BattleReportOSSURLType, string> BattleReportAddressSubMap = new Dictionary<BattleReportOSSURLType, string>
	{
		{
			BattleReportOSSURLType.Online,
			OnlineBattleReportAddressPrefix
		},
		{
			BattleReportOSSURLType.PressureTest,
			PressureTestBattleReportAddressPrefix
		},
		{
			BattleReportOSSURLType.Tencent,
			TencentBattleReportAddressPrefix
		},
		{
			BattleReportOSSURLType.Local,
			DebugBattleReportAddressPrefix
		},
		{
			BattleReportOSSURLType.Amazon,
			AmazonBattleReportAddressPrefix
		},
		{
			BattleReportOSSURLType.AmazonOfficial,
			AmazonOfficialBattleReportAddressPrefix
		}
	};

	public static Dictionary<BattleReportOSSURLType, string[]> BattleReportAddressBackupMap = new Dictionary<BattleReportOSSURLType, string[]>
	{
		{
			BattleReportOSSURLType.Online,
			new string[1] { OnlineBattleReportAddressBackupCDN1 }
		},
		{
			BattleReportOSSURLType.AmazonOfficial,
			new string[1] { AmazonOfficialBattleReportAddressBackupCDN1 }
		}
	};

	public static string onlineDownloadURL_ => cdns[0];

	public static string TableDownloadURL => onlineDownloadURL_;
}
