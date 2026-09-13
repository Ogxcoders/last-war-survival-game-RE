using System;
using System.Runtime.InteropServices;
using System.Text;
using ano;

public static class AnoSdk
{
	[StructLayout(LayoutKind.Sequential)]
	private class AntiDataInfo
	{
		public ushort anti_data_len;

		public IntPtr anti_data;
	}

	[StructLayout(LayoutKind.Sequential)]
	private class SecScanInfo
	{
		public int sec_scan_status;

		public ushort scan_data_len;

		public IntPtr scan_data;
	}

	public const int AnoSDKCmd_IsEmulator = 10;

	private const int AnoSDKCmd_CommQuery = 18;

	public static void AnoRegistAnoInfoReceiver(AnoInfoReceiver receiver)
	{
		AnoInfoPublisher.getInstance().registAnoInfoReceiver(receiver);
	}

	public static string DecAnoInfo(string info)
	{
		return AnoIoctl($"dec_tss_info:{info}");
	}

	public static void SdkInitEx(int gameId, string appKey)
	{
		AnoSDKInitEx(gameId, appKey);
	}

	public static void AnoUserLogin(int accountType, string openId)
	{
		AnoSDKSetUserInfo(accountType, openId);
	}

	public static void AnoSetGamestatus(GameStatus status)
	{
		switch (status)
		{
		case GameStatus.FRONTEND:
			AnoSDKOnResume();
			break;
		case GameStatus.BACKEND:
			AnoSDKOnPause();
			break;
		}
	}

	public static int AnoSetLocale(int locale_id)
	{
		string param = "SetLocaleId:" + locale_id;
		return (int)AnoSDKIoctl(18, param);
	}

	public static void EnableGameReport()
	{
		AnoSDKIoctl(18, "EnableGameReport");
	}

	public static string Ioctl(int request, string cmd)
	{
		IntPtr intPtr = AnoSDKIoctl(request, cmd);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		AntiDataInfo antiDataInfo = new AntiDataInfo();
		antiDataInfo.anti_data_len = (ushort)Marshal.ReadInt16(intPtr, 0);
		antiDataInfo.anti_data = ReadIntPtr(intPtr, 2);
		byte[] array = new byte[antiDataInfo.anti_data_len];
		Marshal.Copy(antiDataInfo.anti_data, array, 0, antiDataInfo.anti_data_len);
		AnoSDKFree(intPtr);
		return Encoding.ASCII.GetString(array);
	}

	public static string AnoIoctl(string cmd)
	{
		IntPtr intPtr = AnoSDKIoctl(18, cmd);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		AntiDataInfo antiDataInfo = new AntiDataInfo();
		antiDataInfo.anti_data_len = (ushort)Marshal.ReadInt16(intPtr, 0);
		antiDataInfo.anti_data = ReadIntPtr(intPtr, 2);
		byte[] array = new byte[antiDataInfo.anti_data_len];
		Marshal.Copy(antiDataInfo.anti_data, array, 0, antiDataInfo.anti_data_len);
		AnoSDKFree(intPtr);
		return Encoding.ASCII.GetString(array);
	}

	private static bool Is64bit()
	{
		return IntPtr.Size == 8;
	}

	private static bool Is32bit()
	{
		return IntPtr.Size == 4;
	}

	private static IntPtr ReadIntPtr(IntPtr addr, int off)
	{
		IntPtr zero = IntPtr.Zero;
		if (Is64bit())
		{
			long value = Marshal.ReadInt64(addr, off);
			zero = new IntPtr(value);
		}
		else
		{
			int value2 = Marshal.ReadInt32(addr, off);
			zero = new IntPtr(value2);
		}
		return zero;
	}

	public static byte[] AnoGetReportData()
	{
		IntPtr intPtr = AnoSDKGetReportData();
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		ushort num = (ushort)Marshal.ReadInt16(intPtr, 0);
		IntPtr intPtr2 = ReadIntPtr(intPtr, 2);
		if (intPtr2 == IntPtr.Zero)
		{
			AnoSDKDelReportData(intPtr);
			return null;
		}
		byte[] array = new byte[num];
		Marshal.Copy(intPtr2, array, 0, num);
		AnoSDKDelReportData(intPtr);
		return array;
	}

	public static byte[] AnoGetReportData2()
	{
		IntPtr intPtr = AnoSDKGetReportData2();
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		ushort num = (ushort)Marshal.ReadInt16(intPtr, 0);
		IntPtr intPtr2 = ReadIntPtr(intPtr, 2);
		if (intPtr2 == IntPtr.Zero || num > 2048)
		{
			return null;
		}
		byte[] array = new byte[num];
		Marshal.Copy(intPtr2, array, 0, num);
		return array;
	}

	public static byte[] AnoGetReportData4(uint token)
	{
		IntPtr intPtr = AnoSDKGetReportData4(token);
		if (intPtr == IntPtr.Zero)
		{
			return null;
		}
		int num = Marshal.ReadInt32(intPtr, 0);
		ushort num2 = (ushort)Marshal.ReadInt16(intPtr, 4);
		IntPtr intPtr2 = ReadIntPtr(intPtr, 6);
		if (num == 0 || intPtr2 == IntPtr.Zero || num2 == 0)
		{
			AnoSDKDelReportData4(intPtr);
			return null;
		}
		byte[] array = new byte[num2];
		Marshal.Copy(intPtr2, array, 0, num2);
		AnoSDKDelReportData4(intPtr);
		return array;
	}

	public static int AnoOnRecvSignature(string name, byte[] buf, uint buf_len, uint crc)
	{
		return AnoSDKOnRecvSignature(name, buf, buf_len, crc);
	}

	[DllImport("anogs")]
	private static extern void AnoSDKInitEx(int gameId, string appKey);

	[DllImport("anogs")]
	private static extern void AnoSDKSetUserInfo(int accountType, string openId);

	[DllImport("anogs")]
	private static extern void AnoSDKOnPause();

	[DllImport("anogs")]
	private static extern void AnoSDKOnResume();

	[DllImport("anogs")]
	private static extern IntPtr AnoSDKIoctl(int request, string param);

	[DllImport("anogs")]
	private static extern void AnoSDKFree(IntPtr info);

	[DllImport("anogs")]
	private static extern IntPtr AnoSDKGetReportData();

	[DllImport("anogs")]
	public static extern void AnoSDKDelReportData(IntPtr info);

	[DllImport("anogs")]
	private static extern IntPtr AnoSDKGetReportData2();

	[DllImport("anogs")]
	private static extern IntPtr AnoSDKGetReportData4(uint token);

	[DllImport("anogs")]
	private static extern void AnoSDKDelReportData4(IntPtr info);

	[DllImport("anogs")]
	private static extern int AnoSDKOnRecvSignature(string name, byte[] data, uint data_len, uint crc);
}
