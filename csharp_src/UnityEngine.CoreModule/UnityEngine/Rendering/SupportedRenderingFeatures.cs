using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering;

public class SupportedRenderingFeatures
{
	[Flags]
	public enum ReflectionProbeModes
	{
		None = 0,
		Rotation = 1
	}

	[Flags]
	public enum LightmapMixedBakeModes
	{
		None = 0,
		IndirectOnly = 1,
		Subtractive = 2,
		Shadowmask = 4
	}

	private static SupportedRenderingFeatures s_Active = new SupportedRenderingFeatures();

	public static SupportedRenderingFeatures active
	{
		get
		{
			if (s_Active == null)
			{
				s_Active = new SupportedRenderingFeatures();
			}
			return s_Active;
		}
		set
		{
			s_Active = value;
		}
	}

	public ReflectionProbeModes reflectionProbeModes { get; set; } = ReflectionProbeModes.None;

	public LightmapMixedBakeModes defaultMixedLightingModes { get; set; } = LightmapMixedBakeModes.None;

	public LightmapMixedBakeModes mixedLightingModes { get; set; } = LightmapMixedBakeModes.IndirectOnly | LightmapMixedBakeModes.Subtractive | LightmapMixedBakeModes.Shadowmask;

	public LightmapBakeType lightmapBakeTypes { get; set; } = LightmapBakeType.Realtime | LightmapBakeType.Baked | LightmapBakeType.Mixed;

	public LightmapsMode lightmapsModes { get; set; } = LightmapsMode.CombinedDirectional;

	public bool enlighten { get; set; } = true;

	public bool lightProbeProxyVolumes { get; set; } = true;

	public bool motionVectors { get; set; } = true;

	public bool receiveShadows { get; set; } = true;

	public bool reflectionProbes { get; set; } = true;

	public bool rendererPriority { get; set; } = false;

	public bool terrainDetailUnsupported { get; set; } = false;

	public bool overridesEnvironmentLighting { get; set; } = false;

	public bool overridesFog { get; set; } = false;

	public bool overridesOtherLightingSettings { get; set; } = false;

	public bool editableMaterialRenderQueue { get; set; } = true;

	public bool overridesLODBias { get; set; } = false;

	public bool overridesMaximumLODLevel { get; set; } = false;

	public bool autoAmbientProbeBaking { get; set; } = true;

	public bool autoDefaultReflectionProbeBaking { get; set; } = true;

	internal unsafe static MixedLightingMode FallbackMixedLightingMode()
	{
		MixedLightingMode result = default(MixedLightingMode);
		FallbackMixedLightingModeByRef(new IntPtr(&result));
		return result;
	}

	[RequiredByNativeCode]
	internal unsafe static void FallbackMixedLightingModeByRef(IntPtr fallbackModePtr)
	{
		MixedLightingMode* ptr = (MixedLightingMode*)(void*)fallbackModePtr;
		if (active.defaultMixedLightingModes != LightmapMixedBakeModes.None && (active.mixedLightingModes & active.defaultMixedLightingModes) == active.defaultMixedLightingModes)
		{
			switch (active.defaultMixedLightingModes)
			{
			case LightmapMixedBakeModes.Shadowmask:
				*ptr = MixedLightingMode.Shadowmask;
				break;
			case LightmapMixedBakeModes.Subtractive:
				*ptr = MixedLightingMode.Subtractive;
				break;
			default:
				*ptr = MixedLightingMode.IndirectOnly;
				break;
			}
		}
		else if (IsMixedLightingModeSupported(MixedLightingMode.Shadowmask))
		{
			*ptr = MixedLightingMode.Shadowmask;
		}
		else if (IsMixedLightingModeSupported(MixedLightingMode.Subtractive))
		{
			*ptr = MixedLightingMode.Subtractive;
		}
		else
		{
			*ptr = MixedLightingMode.IndirectOnly;
		}
	}

	internal unsafe static bool IsMixedLightingModeSupported(MixedLightingMode mixedMode)
	{
		bool result = default(bool);
		IsMixedLightingModeSupportedByRef(mixedMode, new IntPtr(&result));
		return result;
	}

	[RequiredByNativeCode]
	internal unsafe static void IsMixedLightingModeSupportedByRef(MixedLightingMode mixedMode, IntPtr isSupportedPtr)
	{
		bool* ptr = (bool*)(void*)isSupportedPtr;
		if (!IsLightmapBakeTypeSupported(LightmapBakeType.Mixed))
		{
			*ptr = false;
		}
		else
		{
			*ptr = (mixedMode == MixedLightingMode.IndirectOnly && (active.mixedLightingModes & LightmapMixedBakeModes.IndirectOnly) == LightmapMixedBakeModes.IndirectOnly) || (mixedMode == MixedLightingMode.Subtractive && (active.mixedLightingModes & LightmapMixedBakeModes.Subtractive) == LightmapMixedBakeModes.Subtractive) || (mixedMode == MixedLightingMode.Shadowmask && (active.mixedLightingModes & LightmapMixedBakeModes.Shadowmask) == LightmapMixedBakeModes.Shadowmask);
		}
	}

	internal unsafe static bool IsLightmapBakeTypeSupported(LightmapBakeType bakeType)
	{
		bool result = default(bool);
		IsLightmapBakeTypeSupportedByRef(bakeType, new IntPtr(&result));
		return result;
	}

	[RequiredByNativeCode]
	internal unsafe static void IsLightmapBakeTypeSupportedByRef(LightmapBakeType bakeType, IntPtr isSupportedPtr)
	{
		bool* ptr = (bool*)(void*)isSupportedPtr;
		if (bakeType == LightmapBakeType.Mixed && (!IsLightmapBakeTypeSupported(LightmapBakeType.Baked) || active.mixedLightingModes == LightmapMixedBakeModes.None))
		{
			*ptr = false;
			return;
		}
		*ptr = (active.lightmapBakeTypes & bakeType) == bakeType;
		if (bakeType == LightmapBakeType.Realtime && !active.enlighten && !GraphicsSettings.AllowEnlightenSupportForUpgradedProject())
		{
			*ptr = false;
		}
	}

	internal unsafe static bool IsLightmapsModeSupported(LightmapsMode mode)
	{
		bool result = default(bool);
		IsLightmapsModeSupportedByRef(mode, new IntPtr(&result));
		return result;
	}

	[RequiredByNativeCode]
	internal unsafe static void IsLightmapsModeSupportedByRef(LightmapsMode mode, IntPtr isSupportedPtr)
	{
		bool* ptr = (bool*)(void*)isSupportedPtr;
		*ptr = (active.lightmapsModes & mode) == mode;
	}

	internal unsafe static bool IsLightmapperSupported(int lightmapper)
	{
		bool result = default(bool);
		IsLightmapperSupportedByRef(lightmapper, new IntPtr(&result));
		return result;
	}

	[RequiredByNativeCode]
	internal unsafe static void IsLightmapperSupportedByRef(int lightmapper, IntPtr isSupportedPtr)
	{
		bool* ptr = (bool*)(void*)isSupportedPtr;
		*ptr = ((lightmapper != 0 || active.enlighten || GraphicsSettings.AllowEnlightenSupportForUpgradedProject()) ? true : false);
	}

	[RequiredByNativeCode]
	internal unsafe static void IsAutoAmbientProbeBakingSupported(IntPtr isSupportedPtr)
	{
		bool* ptr = (bool*)(void*)isSupportedPtr;
		*ptr = active.autoAmbientProbeBaking;
	}

	[RequiredByNativeCode]
	internal unsafe static void IsAutoDefaultReflectionProbeBakingSupported(IntPtr isSupportedPtr)
	{
		bool* ptr = (bool*)(void*)isSupportedPtr;
		*ptr = active.autoDefaultReflectionProbeBaking;
	}

	internal unsafe static int FallbackLightmapper()
	{
		int result = default(int);
		FallbackLightmapperByRef(new IntPtr(&result));
		return result;
	}

	[RequiredByNativeCode]
	internal unsafe static void FallbackLightmapperByRef(IntPtr lightmapperPtr)
	{
		int* ptr = (int*)(void*)lightmapperPtr;
		*ptr = 1;
	}
}
