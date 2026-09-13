using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using FibMatrix;

namespace RiverBISDK;

public class TextFile
{
	private static ReaderWriterLockSlim _logWriteLock = new ReaderWriterLockSlim();

	private static int _writedCount = 0;

	private static int _readCount = 0;

	public static void InitTextFile()
	{
		BIManagerCore.instance.Dlog("TextFile init info:" + BIConfig.logFilePath);
	}

	public static void WriteTextFile(ArrayList dataList, Func<object, string> dataSerializeFunc)
	{
		if (dataList == null)
		{
			return;
		}
		try
		{
			_logWriteLock.EnterWriteLock();
			string directoryName = Path.GetDirectoryName(BIConfig.logFilePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using StreamWriter streamWriter = new StreamWriter(BIConfig.logFilePath, append: true);
			foreach (object data in dataList)
			{
				string value = dataSerializeFunc(data);
				if (!string.IsNullOrEmpty(value))
				{
					streamWriter.WriteLine(value);
				}
			}
			_writedCount++;
			streamWriter.Close();
		}
		catch (Exception exception)
		{
			Logger.Error(exception);
		}
		finally
		{
			_logWriteLock.ExitWriteLock();
			if (_writedCount > BIConfig.fileMaxLine)
			{
				ReadTextFile();
			}
		}
	}

	public static void ReadTextFile()
	{
		try
		{
			_logWriteLock.EnterWriteLock();
			_writedCount = 0;
			NetManager netManager = BIManagerCore.instance.GetNetManager();
			if (netManager == null || !File.Exists(BIConfig.logFilePath))
			{
				return;
			}
			using (StreamReader streamReader = new StreamReader(BIConfig.logFilePath, Encoding.UTF8))
			{
				string info;
				while ((info = streamReader.ReadLine()) != null)
				{
					PackParams packParams = new PackParams();
					try
					{
						packParams.UpdatePackParams(info);
						netManager.PushQueueHttps(packParams);
					}
					catch (Exception exception)
					{
						Logger.Error(exception);
					}
				}
				streamReader.Close();
			}
			_readCount++;
			using FileStream fileStream = File.Open(BIConfig.logFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			fileStream.SetLength(0L);
			fileStream.Close();
		}
		catch (Exception exception2)
		{
			Logger.Error(exception2);
		}
		finally
		{
			_logWriteLock.ExitWriteLock();
		}
	}

	public static string GetTempIdFromFile()
	{
		try
		{
			if (File.Exists(BIConfig.tempIdFilePath))
			{
				string text = File.ReadAllText(BIConfig.tempIdFilePath).Trim();
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
			}
		}
		catch (Exception)
		{
		}
		return null;
	}

	public static void WriteTempIdToFile(string tempId)
	{
		try
		{
			string directoryName = Path.GetDirectoryName(BIConfig.tempIdFilePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			File.WriteAllText(BIConfig.tempIdFilePath, tempId);
		}
		catch (Exception)
		{
		}
	}
}
