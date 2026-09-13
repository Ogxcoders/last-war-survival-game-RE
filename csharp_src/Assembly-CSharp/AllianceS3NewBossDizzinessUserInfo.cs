using System;
using Sfs2X.Entities.Data;

public class AllianceS3NewBossDizzinessUserInfo : IDisposable
{
	public string name;

	public string pic;

	public int picVer;

	public string abbr;

	public int headSkinId;

	public long headSkinET;

	public int chatBubbleId;

	public long chatBubbleET;

	public void Update(ISFSObject q)
	{
		if (q != null)
		{
			name = q.TryGetString("name");
			pic = q.TryGetString("pic");
			picVer = q.TryGetInt("picVer");
			abbr = q.TryGetString("abbr");
			headSkinId = q.TryGetInt("headSkinId");
			headSkinET = q.TryGetLong("headSkinET");
			chatBubbleId = q.TryGetInt("chatBubbleId");
			chatBubbleET = q.TryGetLong("chatBubbleET");
		}
	}

	public void Dispose()
	{
	}
}
