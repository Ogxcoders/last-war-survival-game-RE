using System;
using Google.Protobuf.Reflection;

namespace Protobuf;

public static class LwBattleReportReflection
{
	private static FileDescriptor descriptor;

	public static FileDescriptor Descriptor => descriptor;

	static LwBattleReportReflection()
	{
		descriptor = FileDescriptor.FromGeneratedCode(Convert.FromBase64String("ChRMd0JhdHRsZVJlcG9ydC5wcm90bxIIcHJvdG9idWYaEkFybXlVbml0SW5m" + "by5wcm90bxoSQmF0dGxlUmVwb3J0LnByb3RvGgpNYWlsLnByb3RvIr4FChBM" + "d0JhdHRsZVVuaXRTdGF0Eg4KBmRhbWFnZRgBIAEoAhIPCgdpbmp1cmVkGAIg" + "ASgCEg8KB2VuaGFuY2UYAyABKAISDgoGd2Vha2VuGAQgASgCEhIKCmNyaXRE" + "YW1hZ2UYBSABKAISDwoHY3JpdE51bRgGIAEoBRIQCghkaXp6eU51bRgHIAEo" + "BRIXCg9waHlzaWNhbEluanVyZWQYCCABKAISFAoMbWFnaWNJbmp1cmVkGAkg" + "ASgCEhEKCWRvdERhbWFnZRgKIAEoAhIVCg1zcHV0dGVyRGFtYWdlGAsgASgC" + "EhYKDnBoeXNpY2FsRGFtYWdlGAwgASgCEhMKC21hZ2ljRGFtYWdlGA0gASgC" + "EhEKCWRhbWFnZU51bRgOIAEoBRISCgpkZWZlbnNlTnVtGA8gASgFEhUKDWRl" + "ZmVuc2VEYW1hZ2UYECABKAISRgoOc2tpbGxEYW1hZ2VNYXAYESADKAsyLi5w" + "cm90b2J1Zi5Md0JhdHRsZVVuaXRTdGF0LlNraWxsRGFtYWdlTWFwRW50cnkS" + "SAoPc2tpbGxJbmp1cmVkTWFwGBIgAygLMi8ucHJvdG9idWYuTHdCYXR0bGVV" + "bml0U3RhdC5Ta2lsbEluanVyZWRNYXBFbnRyeRIRCglkaXNwZWxOdW0YEyAB" + "KAUSFgoOZGlzcGVsRGVidWZOdW0YFCABKAUSEgoKcmVhbERhbWFnZRgVIAEo" + "AhIUCgxzaGllbGREYW1hZ2UYFiABKAISFwoPaW1tdW5lQnVmZkNvdW50GBcg" + "ASgFGjUKE1NraWxsRGFtYWdlTWFwRW50cnkSCwoDa2V5GAEgASgFEg0KBXZh" + "bHVlGAIgASgCOgI4ARo2ChRTa2lsbEluanVyZWRNYXBFbnRyeRILCgNrZXkY" + "ASABKAUSDQoFdmFsdWUYAiABKAI6AjgBIocGCgxMd0JhdHRsZVVuaXQSDgoG" + "aGVyb0lkGAEgASgFEhEKCWhlcm9MZXZlbBgCIAEoBRINCgVpbmRleBgDIAEo" + "BRIQCghoZXJvVXVpZBgEIAEoAxIwCgpza2lsbEluZm9zGAUgAygLMhwucHJv" + "dG9idWYuSGVyb1NraWxsSW5mb1Byb3RvEisKB2VmZmVjdHMYBiADKAsyGi5w" + "cm90b2J1Zi5CYXR0bGVFZmZlY3RJbmZvEg0KBW1heEhwGAcgASgDEgoKAmhw" + "GAggASgDEhUKDWhwQmVmb3JlU3RhcnQYCSABKAMSDgoGcmFua0x2GAogASgF" + "EjAKCmVxdWlwSW5mb3MYCyADKAsyHC5wcm90b2J1Zi5IZXJvRXF1aXBJbmZv" + "UHJvdG8SKAoEc3RhdBgMIAEoCzIaLnByb3RvYnVmLkx3QmF0dGxlVW5pdFN0" + "YXQSFwoPbWF4U29sZGllckNvdW50GA0gASgFEhQKDHNvbGRpZXJDb3VudBgO" + "IAEoBRIOCgZza2luSWQYDyABKAUSEAoIdW5pdFR5cGUYECABKAUSMAoOc2tp" + "bGxDaGlwSW5mb3MYESADKAsyGC5wcm90b2J1Zi5Ta2lsbENoaXBQcm90bxIT" + "Cgt3ZWFwb25MZXZlbBgSIAEoBRITCgt3ZWFwb25Qb3dlchgTIAEoBRIQCghz" + "dW1tb25JZBgUIAEoBRITCgtleHByaXJlVGltZRgVIAEoBRIMCgRuYW1lGBYg" + "ASgJEjQKEHdlYXBvblN0cmVuZ3RoZW4YFyADKAsyGi5wcm90b2J1Zi5XZWFw" + "b25TdHJlbmd0aGVuEkoKEnNvbGRpZXJCZWZvcmVTdGFydBgYIAMoCzIuLnBy" + "b3RvYnVmLkx3QmF0dGxlVW5pdC5Tb2xkaWVyQmVmb3JlU3RhcnRFbnRyeRpS" + "ChdTb2xkaWVyQmVmb3JlU3RhcnRFbnRyeRILCgNrZXkYASABKAUSJgoFdmFs" + "dWUYAiABKAsyFy5wcm90b2J1Zi5Md1NvbGRpZXJMb3N0OgI4ASI3Cg5Md0Fy" + "bXlVbml0SW5mbxIlCgV1bml0cxgBIAMoCzIWLnByb3RvYnVmLkx3QmF0dGxl" + "VW5pdCKNAQoOVW5pdEJ1ZmZDaGFuZ2USDgoGYnVmZklkGAEgASgFEjEKDWVm" + "ZmVjdENoYW5nZXMYAiADKAsyGi5wcm90b2J1Zi5CYXR0bGVFZmZlY3RJbmZv" + "EhIKCnNraWxsTGV2ZWwYAyABKAUSDgoGcmVtb3ZlGAQgASgIEhQKDGJ1ZmZE" + "dXJhdGlvbhgFIAEoAyLsAgoJVGFyZ2V0SGl0Eg0KBWluZGV4GAEgASgFEgwK" + "BG1pc3MYAiABKAUSDAoEY3JpdBgDIAEoBRIOCgZkYW1hZ2UYBCABKAUSMQoN" + "ZWZmZWN0Q2hhbmdlcxgFIAMoCzIaLnByb3RvYnVmLkJhdHRsZUVmZmVjdElu" + "Zm8SLQoLYnVmZkNoYW5nZXMYBiADKAsyGC5wcm90b2J1Zi5Vbml0QnVmZkNo" + "YW5nZRIUCgxkYW1hZ2VEb3VibGUYByABKAESFAoMc2hpZWxkQ2hhbmdlGAgg" + "ASgCEhMKC3NoaWVsZFRvdGFsGAkgASgCEhUKDWRvdERhbWFnZU9uY2UYCiAB" + "KAUSMgoPZG90RGFtYWdlRGV0YWlsGAsgAygLMhkucHJvdG9idWYuRG90RGFt" + "YWdlRGV0YWlsEg8KB2RlZmVuc2UYDCABKAgSEgoKYXR0YWNrQmFjaxgNIAEo" + "CBIRCglkZWJ1Z0luZm8YDyABKAkiVgoPRG90RGFtYWdlRGV0YWlsEg4KBmJ1" + "ZmZJZBgBIAEoBRIOCgZkYW1hZ2UYAiABKAUSEQoJc3RhcnRUaW1lGAMgASgF" + "EhAKCGRvdFRpbWVzGAQgASgFIs4CCgtGaWdodEFjdGlvbhIKCgJpZBgBIAEo" + "BRINCgVvcmRlchgCIAEoBRIMCgR0aW1lGAMgASgFEhMKC2Nhc3RlckluZGV4" + "GAQgASgFEg0KBXBoYXNlGAUgASgFEg8KB3NraWxsSWQYBiABKAUSJAoHdGFy" + "Z2V0cxgHIAMoCzITLnByb3RvYnVmLlRhcmdldEhpdBITCgtza2lsbEJ1ZmZJ" + "ZBgIIAEoBRISCgpza2lsbExldmVsGAkgASgFEhEKCWRlYnVnSW5mbxgKIAEo" + "CRITCgtidWxsZXRJbmRleBgLIAEoBRIXCg9hY3Rpb25QYXJhbVR5cGUYDCAB" + "KAUSKgoKc3VtbW9uVW5pdBgNIAMoCzIWLnByb3RvYnVmLkx3QmF0dGxlVW5p" + "dBITCgtjYXN0ZXJTdGF0ZRgOIAEoBRIQCghlZmZlY3RJZBgPIAEoBSJGCg5M" + "d0JhdHRsZURldGFpbBIMCgR1dWlkGAEgASgDEiYKB2FjdGlvbnMYAiADKAsy" + "FS5wcm90b2J1Zi5GaWdodEFjdGlvbiKAAQoNTHdTb2xkaWVyTG9zdBIRCglz" + "b2xkaWVySWQYASABKAUSDAoEbG9zdBgCIAEoBRIPCgdpbmp1cmVkGAMgASgF" + "Eg8KB3dvdW5kZWQYBCABKAUSDAoEZGVhZBgFIAEoBRINCgV0b3RhbBgGIAEo" + "BRIPCgdkZWdyYWRlGAcgASgFIjIKDkx3U29sZGllckNvdW50EhEKCXNvbGRp" + "ZXJJZBgBIAEoBRINCgVjb3VudBgCIAEoBSKwBwoSTHdCYXR0bGVQbGF5ZXJT" + "dGF0EigKBHVzZXIYASABKAsyGi5wcm90b2J1Zi5SZXBvcnRQbGF5ZXJJbmZv" + "EhEKCWNvbnRlbnRJZBgCIAEoBRIXCg9tYXhTb2xkaWVyUG93ZXIYAyABKAUS" + "FAoMc29sZGllclBvd2VyGAQgASgFEisKB2VmZmVjdHMYBSADKAsyGi5wcm90" + "b2J1Zi5CYXR0bGVFZmZlY3RJbmZvEhYKDnRvdGFsSGVyb1Bvd2VyGAYgASgF" + "EiwKC3NvbGRpZXJMb3N0GAcgAygLMhcucHJvdG9idWYuTHdTb2xkaWVyTG9z" + "dBIQCghhcm15VHlwZRgIIAEoBRIfChdzb2xkaWVyUG93ZXJCZWZvcmVTdGFy" + "dBgJIAEoBRIWCg5pc0hvc3BpdGFsRnVsbBgKIAEoCBIoCghwcm9ncmVzcxgL" + "IAEoCzIWLnByb3RvYnVmLkFybXlQcm9ncmVzcxIzChJzb2xkaWVyQmVmb3Jl" + "U3RhcnQYDCADKAsyFy5wcm90b2J1Zi5Md1NvbGRpZXJMb3N0EhcKD21heFNv" + "bGRpZXJDb3VudBgNIAEoBRIfChdzb2xkaWVyQ291bnRCZWZvcmVTdGFydBgO" + "IAEoBRIUCgxzb2xkaWVyQ291bnQYDyABKAUSFgoOY2hpcFRvdGFsTGV2ZWwY" + "ECABKAUSEAoIZGVhZFJhdGUYESABKAISMwoLZXh0cmFQb3dlcnMYEiADKAsy" + "Hi5wcm90b2J1Zi5CYXR0bGVFeHRyYVBvd2VySW5mbxJCCgtwb3dlclRhYk1h" + "cBgTIAMoCzItLnByb3RvYnVmLkx3QmF0dGxlUGxheWVyU3RhdC5Qb3dlclRh" + "Yk1hcEVudHJ5EhsKE3dlYXBvblN0cmVuZ1RvdGFsTHYYFCABKAUSLgoKYmF0" + "dGxlQ2FyZBgVIAEoCzIaLnByb3RvYnVmLkJhdHRsZUNhcmRNb2R1bGUSNwoV" + "d291bmRlZFRvUmVtYWluRGV0YWlsGBYgAygLMhgucHJvdG9idWYuTHdTb2xk" + "aWVyQ291bnQSMwoNc29sZGllckVsZXZlbhgXIAEoCzIcLnByb3RvYnVmLlNv" + "bGRpZXJFbGV2ZW5Qcm90bxIXCg9zcGVjaWFsVW5pdFR5cGUYGCABKAUSFgoO" + "YXJteVN0YXJ0UG93ZXIYGSABKAUaMgoQUG93ZXJUYWJNYXBFbnRyeRILCgNr" + "ZXkYASABKAUSDQoFdmFsdWUYAiABKAI6AjgBIsQBChlMd0JhdHRsZVBsYXll" + "ckNvbWJpbmVTdGF0EiwKBnBsYXllchgBIAEoCzIcLnByb3RvYnVmLkx3QmF0" + "dGxlUGxheWVyU3RhdBILCgNtdnAYAiABKAUSHwoXdG90YWxEYW1hZ2VTb2xk" + "aWVyUG93ZXIYAyABKAUSDQoFaW5kZXgYBiABKAUSDAoEc2lkZRgHIAEoBRIT" + "Cgt0b3RhbERhbWFnZRgIIAEoBRIZChF0b3RhbERhbWFnZURvdWJsZRgJIAEo" + "ASJuChJMd0FsbGlhbmNlQ2l0eUluZm8SDgoGY2l0eUlkGAEgASgFEhUKDW1h" + "eER1cmFiaWxpdHkYAiABKAUSEgoKZHVyYWJpbGl0eRgDIAEoBRIdChVkdXJh" + "YmlsaXR5QmVmb3JlU3RhcnQYBCABKAUiRgoVTHdNb25zdGVySW52YXNpb25J" + "bmZvEg4KBmRhbWFnZRgBIAEoBRINCgVjdXJIcBgCIAEoBRIOCgZpc0NyaXQY" + "AyABKAgiQQoSTHdBbGxpYW5jZUJvc3NTYW5kEgsKA3VpZBgBIAEoCRIOCgZk" + "YW1hZ2UYAiABKAMSDgoGaXNDcml0GAMgASgIIuEJCg5Md0JhdHRsZVJlcG9y" + "dBIMCgR1dWlkGAEgASgDEgwKBHR5cGUYAiABKAUSEgoKYmF0dGxlVGltZRgD" + "IAEoAxIyCg9iYXR0bGVQb2ludEluZm8YBCABKAsyGS5wcm90b2J1Zi5CYXR0" + "bGVQb2ludEluZm8SEwoLZmlnaHRSZXN1bHQYBSABKAUSLAoGcGxheWVyGAYg" + "AygLMhwucHJvdG9idWYuTHdCYXR0bGVQbGF5ZXJTdGF0EiUKBXVuaXRzGAgg" + "AygLMhYucHJvdG9idWYuTHdCYXR0bGVVbml0EiAKBnJld2FyZBgKIAEoCzIQ" + "LnByb3RvYnVmLlJld2FyZBIoCgZkZXRhaWwYDCABKAsyGC5wcm90b2J1Zi5M" + "d0JhdHRsZURldGFpbBIWCg53aW5LaWxsU29sZGllchgNIAEoAxIUCgxwbHVu" + "ZGVyVmFsdWUYDiABKAMSFwoPbWF4UGx1bmRlclZhbHVlGA8gASgDEhEKCXRv" + "dGFsVGltZRgQIAEoBRIQCghvdmVyVGltZRgRIAEoBRIPCgdhZGRyZXNzGBMg" + "ASgJEg8KB3ZlcnNpb24YFCABKAUSOgoNY29tYmluZVBsYXllchgVIAMoCzIj" + "LnByb3RvYnVmLkx3QmF0dGxlUGxheWVyQ29tYmluZVN0YXQSLQoFcm91bmQY" + "FiADKAsyHi5wcm90b2J1Zi5Md0JhdHRsZUNvbWJpbmVSb3VuZBIXCg9pc0Nv" + "bWJpbmVSZXBvcnQYFyABKAUSEQoJaXNBbGxLaWxsGBggASgFEjYKEGFsbGlh" + "bmNlQ2l0eUluZm8YHiABKAsyHC5wcm90b2J1Zi5Md0FsbGlhbmNlQ2l0eUlu" + "Zm8SNQoUd2luS2lsbFNvbGRpZXJEZXRhaWwYHyADKAsyFy5wcm90b2J1Zi5M" + "d1NvbGRpZXJMb3N0EjcKFmhvc3BpdGFsRnVsbERlYWREZXRhaWwYICADKAsy" + "Fy5wcm90b2J1Zi5Md1NvbGRpZXJMb3N0Ei8KDnJpY29jaGV0RGV0YWlsGCEg" + "AygLMhcucHJvdG9idWYuTHdTb2xkaWVyTG9zdBI8ChNtb25zdGVySW52YXNp" + "b25JbmZvGCIgASgLMh8ucHJvdG9idWYuTHdNb25zdGVySW52YXNpb25JbmZv" + "EjsKGmF0a011bW15Qmxvd1VwUmVzdWx0RGV0YWlsGCMgAygLMhcucHJvdG9i" + "dWYuTHdTb2xkaWVyTG9zdBI7ChpkZWZNdW1teUJsb3dVcFJlc3VsdERldGFp" + "bBgkIAMoCzIXLnByb3RvYnVmLkx3U29sZGllckxvc3QSNQoUcGx1bmRlck1l" + "dGVvcml0ZUluZm8YJSABKAsyFy5wcm90b2J1Zi5NZXRlb3JpdGVJbmZvEjYK" + "EGFsbGlhbmNlQm9zc1NhbmQYJiADKAsyHC5wcm90b2J1Zi5Md0FsbGlhbmNl" + "Qm9zc1NhbmQSFwoPbW9uc3RlckJ1ZmZMaXN0GCcgAygFEj4KEWNoYW1waW9u" + "RHVlbFNjb3JlGCggAygLMiMucHJvdG9idWYuTHdCYXR0bGVDaGFtcGlvbkR1" + "ZWxTY29yZRIXCg9tYXJjaFRhcmdldFR5cGUYKSABKAUSHgoWcGFyYWxsZWxF" + "eGVjdXRlUHJvY2VzcxgqIAEoBSJUChlMd0JhdHRsZUNoYW1waW9uRHVlbFNj" + "b3JlEhEKCWtpbGxTY29yZRgBIAEoBRISCgphbGl2ZVNjb3JlGAIgASgFEhAK" + "CHdpblNjb3JlGAMgASgFIlQKFEx3QmF0dGxlQ29tYmluZVJvdW5kEigKBmJh" + "dHRsZRgBIAMoCzIYLnByb3RvYnVmLkx3QmF0dGxlUmVwb3J0EhIKCnJvdW5k" + "SW5kZXgYAiABKAUiMQoPTHdDb21tb25Kc29uT3NzEgwKBHR5cGUYASABKAUS" + "EAoIanNvbkRhdGEYAiABKAlCHQobbmV0LmltMzAuYXBzLm1vZGVsLnByb3Rv" + "YnVmYgZwcm90bzM="), new FileDescriptor[3]
		{
			ArmyUnitInfoReflection.Descriptor,
			BattleReportReflection.Descriptor,
			MailReflection.Descriptor
		}, new GeneratedClrTypeInfo(null, null, new GeneratedClrTypeInfo[19]
		{
			new GeneratedClrTypeInfo(typeof(LwBattleUnitStat), LwBattleUnitStat.Parser, new string[23]
			{
				"Damage", "Injured", "Enhance", "Weaken", "CritDamage", "CritNum", "DizzyNum", "PhysicalInjured", "MagicInjured", "DotDamage",
				"SputterDamage", "PhysicalDamage", "MagicDamage", "DamageNum", "DefenseNum", "DefenseDamage", "SkillDamageMap", "SkillInjuredMap", "DispelNum", "DispelDebufNum",
				"RealDamage", "ShieldDamage", "ImmuneBuffCount"
			}, null, null, null, new GeneratedClrTypeInfo[2]),
			new GeneratedClrTypeInfo(typeof(LwBattleUnit), LwBattleUnit.Parser, new string[24]
			{
				"HeroId", "HeroLevel", "Index", "HeroUuid", "SkillInfos", "Effects", "MaxHp", "Hp", "HpBeforeStart", "RankLv",
				"EquipInfos", "Stat", "MaxSoldierCount", "SoldierCount", "SkinId", "UnitType", "SkillChipInfos", "WeaponLevel", "WeaponPower", "SummonId",
				"ExprireTime", "Name", "WeaponStrengthen", "SoldierBeforeStart"
			}, null, null, null, new GeneratedClrTypeInfo[1]),
			new GeneratedClrTypeInfo(typeof(LwArmyUnitInfo), LwArmyUnitInfo.Parser, new string[1] { "Units" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(UnitBuffChange), UnitBuffChange.Parser, new string[5] { "BuffId", "EffectChanges", "SkillLevel", "Remove", "BuffDuration" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(TargetHit), TargetHit.Parser, new string[14]
			{
				"Index", "Miss", "Crit", "Damage", "EffectChanges", "BuffChanges", "DamageDouble", "ShieldChange", "ShieldTotal", "DotDamageOnce",
				"DotDamageDetail", "Defense", "AttackBack", "DebugInfo"
			}, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(DotDamageDetail), DotDamageDetail.Parser, new string[4] { "BuffId", "Damage", "StartTime", "DotTimes" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(FightAction), FightAction.Parser, new string[15]
			{
				"Id", "Order", "Time", "CasterIndex", "Phase", "SkillId", "Targets", "SkillBuffId", "SkillLevel", "DebugInfo",
				"BulletIndex", "ActionParamType", "SummonUnit", "CasterState", "EffectId"
			}, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(LwBattleDetail), LwBattleDetail.Parser, new string[2] { "Uuid", "Actions" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(LwSoldierLost), LwSoldierLost.Parser, new string[7] { "SoldierId", "Lost", "Injured", "Wounded", "Dead", "Total", "Degrade" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(LwSoldierCount), LwSoldierCount.Parser, new string[2] { "SoldierId", "Count" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(LwBattlePlayerStat), LwBattlePlayerStat.Parser, new string[25]
			{
				"User", "ContentId", "MaxSoldierPower", "SoldierPower", "Effects", "TotalHeroPower", "SoldierLost", "ArmyType", "SoldierPowerBeforeStart", "IsHospitalFull",
				"Progress", "SoldierBeforeStart", "MaxSoldierCount", "SoldierCountBeforeStart", "SoldierCount", "ChipTotalLevel", "DeadRate", "ExtraPowers", "PowerTabMap", "WeaponStrengTotalLv",
				"BattleCard", "WoundedToRemainDetail", "SoldierEleven", "SpecialUnitType", "ArmyStartPower"
			}, null, null, null, new GeneratedClrTypeInfo[1]),
			new GeneratedClrTypeInfo(typeof(LwBattlePlayerCombineStat), LwBattlePlayerCombineStat.Parser, new string[7] { "Player", "Mvp", "TotalDamageSoldierPower", "Index", "Side", "TotalDamage", "TotalDamageDouble" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(LwAllianceCityInfo), LwAllianceCityInfo.Parser, new string[4] { "CityId", "MaxDurability", "Durability", "DurabilityBeforeStart" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(LwMonsterInvasionInfo), LwMonsterInvasionInfo.Parser, new string[3] { "Damage", "CurHp", "IsCrit" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(LwAllianceBossSand), LwAllianceBossSand.Parser, new string[3] { "Uid", "Damage", "IsCrit" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(LwBattleReport), LwBattleReport.Parser, new string[33]
			{
				"Uuid", "Type", "BattleTime", "BattlePointInfo", "FightResult", "Player", "Units", "Reward", "Detail", "WinKillSoldier",
				"PlunderValue", "MaxPlunderValue", "TotalTime", "OverTime", "Address", "Version", "CombinePlayer", "Round", "IsCombineReport", "IsAllKill",
				"AllianceCityInfo", "WinKillSoldierDetail", "HospitalFullDeadDetail", "RicochetDetail", "MonsterInvasionInfo", "AtkMummyBlowUpResultDetail", "DefMummyBlowUpResultDetail", "PlunderMeteoriteInfo", "AllianceBossSand", "MonsterBuffList",
				"ChampionDuelScore", "MarchTargetType", "ParallelExecuteProcess"
			}, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(LwBattleChampionDuelScore), LwBattleChampionDuelScore.Parser, new string[3] { "KillScore", "AliveScore", "WinScore" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(LwBattleCombineRound), LwBattleCombineRound.Parser, new string[2] { "Battle", "RoundIndex" }, null, null, null, null),
			new GeneratedClrTypeInfo(typeof(LwCommonJsonOss), LwCommonJsonOss.Parser, new string[2] { "Type", "JsonData" }, null, null, null, null)
		}));
	}
}
