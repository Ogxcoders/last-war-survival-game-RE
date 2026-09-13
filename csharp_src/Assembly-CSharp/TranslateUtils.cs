using System.Threading;
using Sfs2X.Entities.Data;

public static class TranslateUtils
{
	public static readonly object LockerNormal = new object();

	public static readonly object LockerByMail = new object();

	public static readonly object LockerByDialog = new object();

	public static readonly object lockerByChat = new object();

	public static void ThreadMethodNormal(object parameter)
	{
		lock (LockerNormal)
		{
			if (parameter != null)
			{
				ISFSObject iSFSObject = parameter as SFSObject;
				if (iSFSObject != null)
				{
					string originalLang = iSFSObject.TryGetString("original");
					string content = iSFSObject.TryGetString("content");
					string targeLand = iSFSObject.TryGetString("land");
					TranslateManager.Instance.GoogleTransLateNormal(originalLang, content, targeLand);
				}
			}
		}
		Thread.CurrentThread.Abort();
	}

	public static void OnThreadGoogleTransLateNormal(string originalLang, string content, string targeLand)
	{
		Thread thread = new Thread(ThreadMethodNormal);
		ISFSObject iSFSObject = new SFSObject();
		iSFSObject.PutUtfString("original", originalLang);
		iSFSObject.PutUtfString("content", content);
		iSFSObject.PutUtfString("land", targeLand);
		thread.Start(iSFSObject);
	}

	public static void ThreadMethodByMail()
	{
		lock (LockerByMail)
		{
			TranslateManager.Instance.GoogleTranslateByMail();
		}
		Thread.CurrentThread.Abort();
	}

	public static void OnThreadGoogleTranslateByMail()
	{
		new Thread(ThreadMethodByMail).Start();
	}

	public static void ThreadMethodByDialog()
	{
		lock (LockerByDialog)
		{
			TranslateManager.Instance.GoogleTranslateByDialog();
		}
		Thread.CurrentThread.Abort();
	}

	public static void OnThreadGoogleTranslateByDialog()
	{
		new Thread(ThreadMethodByDialog).Start();
	}

	private static void ThreadMethodByChat()
	{
		lock (lockerByChat)
		{
			TranslateManager.Instance.GoogleTranslateByChat();
		}
		Thread.CurrentThread.Abort();
	}

	public static void OnThreadGoogleTranslateByChat()
	{
		new Thread(ThreadMethodByChat).Start();
	}
}
