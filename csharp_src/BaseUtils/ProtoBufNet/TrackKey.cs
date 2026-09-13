using System.Collections.Generic;

namespace ProtoBufNet;

public static class TrackKey
{
	public const string lineType = "lineType";

	public const string lineName = "lineName";

	public const string window = "window";

	public const string sendCount = "sendCount";

	public const string sendTimeP90 = "sendTimeP90";

	public const string sendTimeP99 = "sendTimeP99";

	public const string recCount = "recCount";

	public const string recSize = "recSize";

	public const string recTimeTot = "recTimeTot";

	public const string recTimeP90 = "recTimeP90";

	public const string recTimeP99 = "recTimeP99";

	public const string recDecryptTimeP90 = "recDecryptTimeP90";

	public const string recDecryptTimeP99 = "recDecryptTimeP99";

	public const string recUncompressTimeP90 = "recUncompressTimeP90";

	public const string recUncompressTimeP99 = "recUncompressTimeP99";

	public const string recSfsObjTimeP90 = "recSfsObjTimeP90";

	public const string recSfsObjTimeP99 = "recSfsObjTimeP99";

	public const string pinP90 = "pinP90";

	public const string pinP99 = "pinP99";

	public static Dictionary<string, string> keyToTrackStr = new Dictionary<string, string>
	{
		{ "lineType", "s_para1" },
		{ "lineName", "s_para2" }
	};

	public static Dictionary<string, string> keyToTrackNum = new Dictionary<string, string>
	{
		{ "window", "i_para3" },
		{ "sendCount", "int_para1" },
		{ "sendTimeP90", "int_para2" },
		{ "sendTimeP99", "i_gainnum" },
		{ "recCount", "i_gainworkernum" },
		{ "recSize", "i_goodsnum" },
		{ "recTimeTot", "i_isteam" },
		{ "recTimeP90", "i_istopscore" },
		{ "recTimeP99", "i_iswin" },
		{ "recDecryptTimeP90", "i_lotterycount" },
		{ "recDecryptTimeP99", "i_multiple" },
		{ "recUncompressTimeP90", "i_merge_type" },
		{ "recUncompressTimeP99", "i_num" },
		{ "recSfsObjTimeP90", "i_oldwishscore" },
		{ "recSfsObjTimeP99", "i_cost" },
		{ "pinP90", "i_costnum" },
		{ "pinP99", "i_first_size" }
	};
}
