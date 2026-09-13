using System;
using Sfs2X.Entities.Data;

public class DetectZombieBus : IDisposable
{
	public int busId;

	public int type;

	public string param;

	public int isPass;

	public void Update(ISFSObject obj)
	{
		if (obj != null)
		{
			busId = obj.TryGetInt("busId");
			type = obj.TryGetInt("type");
			param = obj.TryGetString("param");
			isPass = obj.TryGetInt("isPass");
		}
	}

	public void Dispose()
	{
		busId = 0;
		type = 0;
		param = null;
		isPass = 0;
	}
}
