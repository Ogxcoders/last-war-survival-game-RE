using System;
using Google.Protobuf.Reflection;

namespace Protobuf;

public static class BattleRoundPushReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static BattleRoundPushReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChVCYXR0bGVSb3VuZFB1c2gucHJvdG8SCHByb3RvYnVmGhJCYXR0bGVSZXBv" + "cnQucHJvdG8iMQoPUm91bmRTdGF0dXNJbmZvEhAKCHN0YXR1c0lkGAEgASgF" + "EgwKBHRpbWUYAiABKAUiZgoXU2ltcGxlQ29tYmF0VW5pdFB1c2hPYmoSLAoI" + "YXJteUluZm8YASABKAsyGi5wcm90b2J1Zi5TaW1wbGVDb21iYXRVbml0EgwK" + "BHR5cGUYAiABKAUSDwoHdG9wVXVpZBgDIAEoAyL+AQoSU2ltcGxlU2VsZkFy" + "bXlJbmZvEjMKCGFybXlJbmZvGAEgASgLMiEucHJvdG9idWYuU2ltcGxlQ29t" + "YmF0VW5pdFB1c2hPYmoSNQoKdGFyZ2V0SW5mbxgCIAEoCzIhLnByb3RvYnVm" + "LlNpbXBsZUNvbWJhdFVuaXRQdXNoT2JqEgwKBGh1cnQYAyABKAUSDAoEaGVh" + "bBgEIAEoBRISCgpzaGllbGRIdXJ0GAUgASgFEg4KBnNoaWVsZBgGIAEoBRIN" + "CgVhbmdlchgHIAEoBRItCgpzdGF0dXNJbmZvGAggAygLMhkucHJvdG9idWYu" + "Um91bmRTdGF0dXNJbmZvInsKE0NvbWJpbmVTZWxmQXJteUluZm8SLQoHbWVt" + "YmVycxgBIAMoCzIcLnByb3RvYnVmLlNpbXBsZVNlbGZBcm15SW5mbxI1Cgp0" + "YXJnZXRJbmZvGAIgASgLMiEucHJvdG9idWYuU2ltcGxlQ29tYmF0VW5pdFB1" + "c2hPYmoibgoTQmFzZVJvdW5kUmVwb3J0UHVzaBIuCgtyb3VuZFJlcG9ydBgB" + "IAEoCzIZLnByb3RvYnVmLkJhc2VSb3VuZFJlcG9ydBITCgt0cmlnZ2VyVXVp" + "ZBgCIAEoAxISCgp0YXJnZXRVdWlkGAMgASgDInIKFUVmZmVjdFJvdW5kUmVw" + "b3J0UHVzaBIwCgtyb3VuZFJlcG9ydBgBIAEoCzIbLnByb3RvYnVmLkVmZmVj" + "dFJvdW5kUmVwb3J0EhMKC3RyaWdnZXJVdWlkGAIgASgDEhIKCnRhcmdldFV1" + "aWQYAyABKAMi2AEKE0JhdHRsZVJvdW5kUHVzaEluZm8SDAoEdHlwZRgBIAEo" + "BRI0Cg5zaW1wbGVBcm15SW5mbxgCIAEoCzIcLnByb3RvYnVmLlNpbXBsZVNl" + "bGZBcm15SW5mbxI2Cg9jb21iaW5lQXJteUluZm8YAyABKAsyHS5wcm90b2J1" + "Zi5Db21iaW5lU2VsZkFybXlJbmZvEhAKCG91dFJhbmdlGAQgASgIEjMKDHJv" + "dW5kUmVwb3J0cxgFIAMoCzIdLnByb3RvYnVmLkJhc2VSb3VuZFJlcG9ydFB1" + "c2hCHQobbmV0LmltMzAuYXBzLm1vZGVsLnByb3RvYnVmYgZwcm90bzM="), new FileDescriptor[1] { BattleReportReflection.Descriptor }, new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[7]
		{
			new GeneratedClrTypeInfo(typeof(RoundStatusInfo), RoundStatusInfo.Parser, new string[2] { "StatusId", "Time" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(SimpleCombatUnitPushObj), SimpleCombatUnitPushObj.Parser, new string[3] { "ArmyInfo", "Type", "TopUuid" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(SimpleSelfArmyInfo), SimpleSelfArmyInfo.Parser, new string[8] { "ArmyInfo", "TargetInfo", "Hurt", "Heal", "ShieldHurt", "Shield", "Anger", "StatusInfo" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(CombineSelfArmyInfo), CombineSelfArmyInfo.Parser, new string[2] { "Members", "TargetInfo" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(BaseRoundReportPush), BaseRoundReportPush.Parser, new string[3] { "RoundReport", "TriggerUuid", "TargetUuid" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(EffectRoundReportPush), EffectRoundReportPush.Parser, new string[3] { "RoundReport", "TriggerUuid", "TargetUuid" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(BattleRoundPushInfo), BattleRoundPushInfo.Parser, new string[5] { "Type", "SimpleArmyInfo", "CombineArmyInfo", "OutRange", "RoundReports" }, null, null, null, null)
		}));
	}
}
