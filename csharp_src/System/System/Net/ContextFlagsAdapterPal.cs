namespace System.Net;

internal static class ContextFlagsAdapterPal
{
	private readonly struct ContextFlagMapping
	{
		public readonly global::Interop.NetSecurityNative.GssFlags GssFlags;

		public readonly ContextFlagsPal ContextFlag;

		public ContextFlagMapping(global::Interop.NetSecurityNative.GssFlags gssFlag, ContextFlagsPal contextFlag)
		{
			GssFlags = gssFlag;
			ContextFlag = contextFlag;
		}
	}

	private static readonly ContextFlagMapping[] s_contextFlagMapping = new ContextFlagMapping[6]
	{
		new ContextFlagMapping(global::Interop.NetSecurityNative.GssFlags.GSS_C_CONF_FLAG, ContextFlagsPal.Confidentiality),
		new ContextFlagMapping(global::Interop.NetSecurityNative.GssFlags.GSS_C_IDENTIFY_FLAG, ContextFlagsPal.AcceptIntegrity),
		new ContextFlagMapping(global::Interop.NetSecurityNative.GssFlags.GSS_C_MUTUAL_FLAG, ContextFlagsPal.MutualAuth),
		new ContextFlagMapping(global::Interop.NetSecurityNative.GssFlags.GSS_C_REPLAY_FLAG, ContextFlagsPal.ReplayDetect),
		new ContextFlagMapping(global::Interop.NetSecurityNative.GssFlags.GSS_C_SEQUENCE_FLAG, ContextFlagsPal.SequenceDetect),
		new ContextFlagMapping(global::Interop.NetSecurityNative.GssFlags.GSS_C_DELEG_FLAG, ContextFlagsPal.Delegate)
	};

	internal static ContextFlagsPal GetContextFlagsPalFromInterop(global::Interop.NetSecurityNative.GssFlags gssFlags, bool isServer)
	{
		ContextFlagsPal contextFlagsPal = ContextFlagsPal.None;
		if ((gssFlags & global::Interop.NetSecurityNative.GssFlags.GSS_C_INTEG_FLAG) != 0)
		{
			contextFlagsPal = (ContextFlagsPal)((int)contextFlagsPal | (isServer ? 131072 : 65536));
		}
		ContextFlagMapping[] array = s_contextFlagMapping;
		for (int i = 0; i < array.Length; i++)
		{
			ContextFlagMapping contextFlagMapping = array[i];
			if ((gssFlags & contextFlagMapping.GssFlags) == contextFlagMapping.GssFlags)
			{
				contextFlagsPal |= contextFlagMapping.ContextFlag;
			}
		}
		return contextFlagsPal;
	}

	internal static global::Interop.NetSecurityNative.GssFlags GetInteropFromContextFlagsPal(ContextFlagsPal flags, bool isServer)
	{
		global::Interop.NetSecurityNative.GssFlags gssFlags = (global::Interop.NetSecurityNative.GssFlags)0u;
		if (isServer)
		{
			if ((flags & ContextFlagsPal.AcceptIntegrity) != ContextFlagsPal.None)
			{
				gssFlags |= global::Interop.NetSecurityNative.GssFlags.GSS_C_INTEG_FLAG;
			}
		}
		else if ((flags & ContextFlagsPal.AcceptStream) != ContextFlagsPal.None)
		{
			gssFlags |= global::Interop.NetSecurityNative.GssFlags.GSS_C_INTEG_FLAG;
		}
		ContextFlagMapping[] array = s_contextFlagMapping;
		for (int i = 0; i < array.Length; i++)
		{
			ContextFlagMapping contextFlagMapping = array[i];
			if ((flags & contextFlagMapping.ContextFlag) == contextFlagMapping.ContextFlag)
			{
				gssFlags |= contextFlagMapping.GssFlags;
			}
		}
		return gssFlags;
	}
}
