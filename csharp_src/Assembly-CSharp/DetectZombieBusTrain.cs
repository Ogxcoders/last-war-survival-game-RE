using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;

public class DetectZombieBusTrain : IDisposable
{
	public readonly List<DetectZombieBus> ZombieBusList = new List<DetectZombieBus>();

	public void Update(ISFSArray arr)
	{
		ZombieBusList.Clear();
		if (arr == null || arr.Size() == 0)
		{
			return;
		}
		for (int i = 0; i < arr.Count; i++)
		{
			ISFSObject sFSObject = arr.GetSFSObject(i);
			if (sFSObject != null)
			{
				DetectZombieBus detectZombieBus = new DetectZombieBus();
				detectZombieBus.Update(sFSObject);
				ZombieBusList.Add(detectZombieBus);
			}
		}
	}

	public void Dispose()
	{
		foreach (DetectZombieBus zombieBus in ZombieBusList)
		{
			zombieBus.Dispose();
		}
		ZombieBusList.Clear();
	}
}
