using System;
using System.Collections.Generic;
using System.Threading;

namespace ano;

public class AnoInfoPublisher
{
	public const int ANO_INFO_TYPE_DETECT_RESULT = 1;

	public const int ANO_INFO_TYPE_HEARTBEAT = 2;

	private static volatile AnoInfoPublisher mInstance = null;

	private static readonly object mSingletonLock = new object();

	private readonly object padlockReceiver = new object();

	private static List<AnoInfoReceiver> mReceivers = new List<AnoInfoReceiver>();

	private static Thread mAnoInfoPublisherThread = null;

	private static volatile bool mAnoInfoPublisherThreadStarted = false;

	private AnoInfoPublisher()
	{
	}

	public static AnoInfoPublisher getInstance()
	{
		if (mInstance == null)
		{
			lock (mSingletonLock)
			{
				if (mInstance == null)
				{
					mInstance = new AnoInfoPublisher();
				}
			}
		}
		return mInstance;
	}

	public void registAnoInfoReceiver(AnoInfoReceiver receiver)
	{
		if (receiver == null)
		{
			return;
		}
		if (!mAnoInfoPublisherThreadStarted)
		{
			lock (padlockReceiver)
			{
				if (!mAnoInfoPublisherThreadStarted)
				{
					mAnoInfoPublisherThreadStarted = true;
					mAnoInfoPublisherThread = new Thread(getInstance().recvDataThread);
					mAnoInfoPublisherThread.IsBackground = true;
					mAnoInfoPublisherThread.Start();
				}
			}
		}
		lock (padlockReceiver)
		{
			mReceivers.Add(receiver);
		}
	}

	private void broadcastInfo(int id, string info)
	{
		lock (padlockReceiver)
		{
			foreach (AnoInfoReceiver mReceiver in mReceivers)
			{
				mReceiver.onReceive(id, info);
			}
		}
	}

	private void recvDataThread()
	{
		if (openPipe() != 0)
		{
			mAnoInfoPublisherThreadStarted = false;
			return;
		}
		try
		{
			while (true)
			{
				try
				{
					Thread.Sleep(1000);
				}
				catch (Exception)
				{
				}
				string text = recvPipe();
				if (text != null)
				{
					int num = text.IndexOf('|');
					if (num != -1)
					{
						int id = int.Parse(text.Substring(0, num));
						string info = text.Substring(num + 1);
						broadcastInfo(id, info);
						continue;
					}
					break;
				}
				break;
			}
		}
		catch (Exception)
		{
		}
		finally
		{
			closePipe();
			mAnoInfoPublisherThreadStarted = false;
		}
	}

	private static int openPipe()
	{
		string s = AnoSdk.AnoIoctl("ilc_open_pipe");
		int result = -1;
		try
		{
			result = int.Parse(s);
		}
		catch (Exception)
		{
		}
		return result;
	}

	private static void closePipe()
	{
		AnoSdk.AnoIoctl("ilc_close_pipe");
	}

	private static string recvPipe()
	{
		return AnoSdk.AnoIoctl("ilc_recv_pipe");
	}
}
