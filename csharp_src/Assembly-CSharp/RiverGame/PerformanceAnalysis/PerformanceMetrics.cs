using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Profiling;

namespace RiverGame.PerformanceAnalysis;

public class PerformanceMetrics
{
	public class Metric<T>
	{
		private Func<T> m_DefaultProvider;

		public Func<T> CustomProvider;

		public T Value
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return (CustomProvider ?? m_DefaultProvider)();
			}
		}

		public Metric(Func<T> defaultProvider)
		{
			m_DefaultProvider = defaultProvider;
		}
	}

	public enum EnEventType
	{
		Default,
		Delay,
		Interval,
		LogicModuleChanged
	}

	public const string DEVICE_PERFORMANCE = "device_performance";

	public const string PD_EVENT_TYPE = "pd_event_type";

	public const string PD_ASSEMBLIES_UPDATED = "pd_assemblies_updated";

	public const string PD_MEM = "pd_mem";

	public const string PD_MEM_NRSV = "pd_mem_nrsv";

	public const string PD_MEM_NUSE = "pd_mem_nuse";

	public const string PD_MEM_MRSV = "pd_mem_mrsv";

	public const string PD_MEM_MUSE = "pd_mem_muse";

	public const string PD_MEM_MNVM = "pd_mem_mnvm";

	public const string PD_MEM_LUVM = "pd_mem_luvm";

	public const string PD_FPS = "pd_fps";

	public const string PD_FPS_0_1 = "pd_fps_0_1";

	public const string PD_FPS_01 = "pd_fps_01";

	public const string PD_FPS_10 = "pd_fps_10";

	public const string PD_FPS_20 = "pd_fps_20";

	public const string PD_FPS_30 = "pd_fps_30";

	public const string PD_FPS_50 = "pd_fps_50";

	public const string PD_FPS_TARGET = "pcurfps";

	public const string PD_FPS_SHOULD_HIGH_TIME = "pd_fps_should_high_time";

	public const string PD_FPS_SHOULD_HIGH_TIME2 = "pd_fps_should_high_time2";

	public const string PD_SMALL_JANK_COUNT = "pd_small_jank_count";

	public const string PD_BIG_JANK_COUNT = "pd_big_jank_count";

	public const string PD_SMALL_JANK_TIME = "pd_small_jank_time";

	public const string PD_BIG_JANK_TIME = "pd_big_jank_time";

	public const string PD_DURATION = "pd_duration";

	public const string PD_FPH = "pd_fph";

	public const string PD_FFR = "pd_ffr";

	public const string PD_DC = "pd_dc";

	public const string PD_DL = "pd_dl";

	public const string PD_QUALITY_LEVEL = "pd_quality_level";

	public const string PD_POWER_SAVING = "pd_power_saving";

	public const string PD_BENCHMARK_LEVEL = "pd_benchmark_level";

	public const string PD_SCORE = "pd_score";

	public const string PD_LOGIC_MODULE_TAG = "pd_logic_module_tag";

	public const string PD_MEM_SPINE_CACHE = "pd_mem_spine_cache";

	public const string PD_SI_DEVICE_MODEL = "pd_si_device_model";

	public const string PD_SI_DEVICE_NAME = "pd_si_device_name";

	public const string PD_SI_PROCESSOR_TYPE = "pd_si_processor_type";

	public const string PD_SI_GRAPHICS_DEVICE_NAME = "pd_si_graphics_device_name";

	public const string PD_SI_GRAPHICS_DEVICE_VERSION = "pd_si_graphics_device_version";

	public const string PD_SI_GRAPHICS_SHADER_LEVEL = "pd_si_graphics_shader_level";

	public const string PD_SI_SUPPORT_INSTANCING = "pd_si_support_instancing";

	public const string PD_SI_OPERATING_SYSTEM = "pd_si_operating_system";

	public const string PD_SI_PLATFORM = "pd_si_platform";

	public const string PD_SI_SYSTEM_MEMORY_SIZE = "pd_si_system_memory_size";

	public const string PD_SI_RESOLUTION_WIDTH = "pd_si_resolution_width";

	public const string PD_SI_RESOLUTION_HEIGHT = "pd_si_resolution_height";

	public const string PD_SI_DPI = "pd_si_dpi";

	public const string PD_SI_BATTERY_LEVEL = "f_para1";

	public const string PD_BATTERY_VOLTAGE = "f_para2";

	public const string PD_BATTERY_CURRENT = "f_para3";

	public const string PD_POWER_NATIVE_SAVE_MODE = "power_3";

	public const string PD_ThermalState = "power_4";

	public const string PD_LOGIC_MODULE_SOURCE_TAG = "source_type";

	internal static EnEventType CurrentEventType = EnEventType.Default;

	internal static EnumToName<EnEventType> EventTypeToNames = new EnumToName<EnEventType>();

	private const double Kibibyte = 1024.0;

	private const double Mebibyte = 1048576.0;

	internal static Metric<string> EventType { get; } = new Metric<string>(() => EventTypeToNames[CurrentEventType]);

	public static Metric<bool> AssembliesUpdated { get; } = new Metric<bool>(() => false);

	public static Metric<int> DeviceLevel { get; } = new Metric<int>(() => -1);

	public static Metric<int> QualityLevel { get; } = new Metric<int>(() => QualitySettings.GetQualityLevel() + 1);

	public static Metric<float> DeviceScore { get; } = new Metric<float>(() => -1f);

	public static Metric<float> BenchmarkLevel { get; } = new Metric<float>(() => Math.Min(float.MaxValue, 0f));

	public static Metric<bool> PowerSaving { get; } = new Metric<bool>(() => false);

	public static Metric<string> LogicModuleTag { get; } = new Metric<string>(() => "default");

	public static Metric<string> LogicModuleSourceTag { get; } = new Metric<string>(() => "default");

	public static string DeviceModel => SystemInfo.deviceModel;

	public static string DeviceName => SystemInfo.deviceName;

	public static string ProcessorType => SystemInfo.processorType;

	public static string GraphicsDeviceName => SystemInfo.graphicsDeviceName;

	public static string GraphicsDeviceVersion => SystemInfo.graphicsDeviceVersion;

	public static int GraphicsShaderLevel => SystemInfo.graphicsShaderLevel;

	public static bool SupportInstancing => SystemInfo.supportsInstancing;

	public static string OperatingSystem => SystemInfo.operatingSystem;

	public static float BatteryLevel => SystemInfo.batteryLevel;

	public static Metric<float> BatteryVoltage { get; } = new Metric<float>(() => -1f);

	public static Metric<float> BatteryCurrent { get; } = new Metric<float>(() => -1f);

	public static Metric<string> PowerNativeSaveMode { get; } = new Metric<string>(() => "false");

	public static Metric<string> Platform { get; } = new Metric<string>(() => "unknown");

	public static Metric<int> ThermalState { get; } = new Metric<int>(() => -1);

	public static float DPI => Screen.dpi;

	public static int SystemMemorySizeInMegabytes => SystemInfo.systemMemorySize;

	public static int ResolutionWidth => Screen.width;

	public static int ResolutionHeight => Screen.height;

	public static Metric<long> CurrentMemoryLong { get; } = new Metric<long>(PerformanceMemoryMetrics.GetMemoryUsageLong);

	public static long MemoryNativeReservedLong => Profiler.GetTotalReservedMemoryLong();

	public static long MemoryNativeUsedLong => Profiler.GetTotalAllocatedMemoryLong();

	public static long MemoryMonoReservedLong => Profiler.GetMonoHeapSizeLong();

	public static long MemoryMonoUsedLong => Profiler.GetMonoUsedSizeLong();

	public static long MemoryMonoVMLong => GC.GetTotalMemory(forceFullCollection: false);

	public static Metric<long> MemoryLuaVMLong { get; } = new Metric<long>(() => 0L);

	public static float CurrentMemoryInKibibyte => (float)((double)CurrentMemoryLong.Value / 1024.0);

	public static float CurrentMemoryInMebibyte => (float)((double)CurrentMemoryLong.Value / 1048576.0);

	public static float MemoryNativeReservedInMebibyte => (float)((double)MemoryNativeReservedLong / 1048576.0);

	public static float MemoryNativeUsedInMebibyte => (float)((double)MemoryNativeUsedLong / 1048576.0);

	public static float MemoryMonoReservedInMebibyte => (float)((double)MemoryMonoReservedLong / 1048576.0);

	public static float MemoryMonoUsedInMebibyte => (float)((double)MemoryMonoUsedLong / 1048576.0);

	public static float MemoryMonoVMInMebibyte => (float)((double)MemoryMonoVMLong / 1048576.0);

	public static float MemoryLuaVMInMebibyte => (float)((double)MemoryLuaVMLong.Value / 1048576.0);

	public static Metric<float> CurrentFPS { get; } = new Metric<float>(() => 0f);

	public static Metric<float> AverageFPS { get; } = new Metric<float>(() => 0f);

	public static Metric<float> FPS0_1 { get; } = new Metric<float>(() => 0f);

	public static Metric<float> FPS01 { get; } = new Metric<float>(() => 0f);

	public static Metric<float> FPS10 { get; } = new Metric<float>(() => 0f);

	public static Metric<float> FPS20 { get; } = new Metric<float>(() => 0f);

	public static Metric<float> FPS30 { get; } = new Metric<float>(() => 0f);

	public static Metric<float> FPS50 { get; } = new Metric<float>(() => 0f);

	public static Metric<float> FPS_Target { get; } = new Metric<float>(() => 0f);

	public static Metric<float> FPSShouldHighTime { get; } = new Metric<float>(() => 0f);

	public static Metric<float> FPSShouldHighTime2 { get; } = new Metric<float>(() => 0f);

	public static Metric<int> SmallJankCount { get; } = new Metric<int>(() => 0);

	public static Metric<int> BigJankCount { get; } = new Metric<int>(() => 0);

	public static Metric<float> SmallJankTime { get; } = new Metric<float>(() => 0f);

	public static Metric<float> BigJankTime { get; } = new Metric<float>(() => 0f);

	public static Metric<float> Duration { get; } = new Metric<float>(() => 0f);

	public static void GetPerformanceMetrics(Dictionary<string, object> tracePerformanceContext)
	{
		tracePerformanceContext["pd_event_type"] = EventType.Value;
		tracePerformanceContext["pd_assemblies_updated"] = AssembliesUpdated.Value;
		tracePerformanceContext["pd_dl"] = DeviceLevel.Value;
		tracePerformanceContext["pd_quality_level"] = QualityLevel.Value;
		tracePerformanceContext["pd_power_saving"] = PowerSaving.Value;
		tracePerformanceContext["pd_score"] = DeviceScore.Value;
		tracePerformanceContext["pd_benchmark_level"] = BenchmarkLevel.Value;
		tracePerformanceContext["pd_logic_module_tag"] = LogicModuleTag.Value;
		tracePerformanceContext["source_type"] = LogicModuleSourceTag.Value;
		tracePerformanceContext["pd_fps"] = AverageFPS.Value;
		tracePerformanceContext["pd_fps_0_1"] = FPS0_1.Value;
		tracePerformanceContext["pd_fps_01"] = FPS01.Value;
		tracePerformanceContext["pd_fps_10"] = FPS10.Value;
		tracePerformanceContext["pd_fps_20"] = FPS20.Value;
		tracePerformanceContext["pd_fps_30"] = FPS30.Value;
		tracePerformanceContext["pd_fps_50"] = FPS50.Value;
		tracePerformanceContext["pcurfps"] = FPS_Target.Value;
		tracePerformanceContext["pd_fps_should_high_time"] = FPSShouldHighTime.Value;
		tracePerformanceContext["pd_fps_should_high_time2"] = FPSShouldHighTime2.Value;
		tracePerformanceContext["pd_small_jank_count"] = SmallJankCount.Value;
		tracePerformanceContext["pd_big_jank_count"] = BigJankCount.Value;
		tracePerformanceContext["pd_small_jank_time"] = SmallJankTime.Value;
		tracePerformanceContext["pd_big_jank_time"] = BigJankTime.Value;
		tracePerformanceContext["pd_duration"] = Duration.Value;
		tracePerformanceContext["pd_mem"] = CurrentMemoryInKibibyte;
		tracePerformanceContext["pd_mem_nrsv"] = MemoryNativeReservedLong;
		tracePerformanceContext["pd_mem_nuse"] = MemoryNativeUsedLong;
		tracePerformanceContext["pd_mem_mrsv"] = MemoryMonoReservedLong;
		tracePerformanceContext["pd_mem_muse"] = MemoryMonoUsedLong;
		tracePerformanceContext["pd_mem_mnvm"] = MemoryMonoVMLong;
		tracePerformanceContext["pd_mem_luvm"] = MemoryLuaVMLong.Value;
		tracePerformanceContext["f_para1"] = BatteryLevel;
		tracePerformanceContext["f_para2"] = BatteryVoltage.Value;
		tracePerformanceContext["f_para3"] = BatteryCurrent.Value;
		tracePerformanceContext["power_3"] = PowerNativeSaveMode.Value;
	}

	public static void GetSystemInfoMetrics(Dictionary<string, object> tracePerformanceContext)
	{
		if (tracePerformanceContext != null)
		{
			tracePerformanceContext["pd_si_device_model"] = DeviceModel;
			tracePerformanceContext["pd_si_device_name"] = DeviceName;
			tracePerformanceContext["pd_si_processor_type"] = ProcessorType;
			tracePerformanceContext["pd_si_graphics_device_name"] = GraphicsDeviceName;
			tracePerformanceContext["pd_si_graphics_device_version"] = GraphicsDeviceVersion;
			tracePerformanceContext["pd_si_graphics_shader_level"] = GraphicsShaderLevel;
			tracePerformanceContext["pd_si_support_instancing"] = SupportInstancing;
			tracePerformanceContext["pd_si_operating_system"] = OperatingSystem;
			tracePerformanceContext["pd_si_platform"] = Platform.Value;
			tracePerformanceContext["pd_si_system_memory_size"] = SystemMemorySizeInMegabytes;
			tracePerformanceContext["pd_si_resolution_width"] = ResolutionWidth;
			tracePerformanceContext["pd_si_resolution_height"] = ResolutionHeight;
			tracePerformanceContext["pd_si_dpi"] = DPI;
		}
	}
}
