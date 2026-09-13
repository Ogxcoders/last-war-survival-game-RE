using System;
using System.Collections.Generic;

namespace MiniGame.Biubiu;

public class ServiceCommand : ICloneable
{
	public readonly List<ISyncCommand> Commands;

	public int ExecutedIndex = -1;

	public ServiceCommand(List<ISyncCommand> commands = null)
	{
		Commands = commands ?? new List<ISyncCommand>();
	}

	public void QueueEvent(ISyncCommand syncEvent)
	{
		if (Commands.Count > 0 && Commands[Commands.Count - 1].FrameIndex > syncEvent.FrameIndex)
		{
			throw new Exception("FrameIndex is not ascending");
		}
		syncEvent.IsValid = true;
		Commands.Add(syncEvent);
	}

	public bool TryQueueEvent(ISyncCommand syncEvent)
	{
		if (Commands.Count > 0)
		{
			if (Commands[Commands.Count - 1].FrameIndex > syncEvent.FrameIndex)
			{
				return false;
			}
			if (Commands[Commands.Count - 1].FrameIndex == syncEvent.FrameIndex)
			{
				for (int num = Commands.Count - 1; num >= 0; num--)
				{
					ISyncCommand syncCommand = Commands[num];
					if (syncCommand.FrameIndex != syncEvent.FrameIndex)
					{
						break;
					}
					if (syncCommand is CommandCreateBullet commandCreateBullet && syncEvent is CommandCreateBullet commandCreateBullet2 && commandCreateBullet.EntityID == commandCreateBullet2.EntityID)
					{
						return false;
					}
				}
			}
		}
		Commands.Add(syncEvent);
		return true;
	}

	public ISyncCommand PickEvent(int frameIndex)
	{
		int num = ExecutedIndex + 1;
		if (num >= Commands.Count)
		{
			throw new Exception("Command index is out of range");
		}
		ISyncCommand syncCommand = Commands[num];
		if (syncCommand.FrameIndex != frameIndex)
		{
			return null;
		}
		ExecutedIndex = num;
		return syncCommand;
	}

	public bool TryPickEvent(int frameIndex, out ISyncCommand command)
	{
		int num = ExecutedIndex + 1;
		if (num >= Commands.Count)
		{
			command = null;
			return false;
		}
		command = Commands[num];
		if (command.FrameIndex != frameIndex)
		{
			command = null;
			return false;
		}
		ExecutedIndex = num;
		return true;
	}

	public object Clone()
	{
		ServiceCommand serviceCommand = new ServiceCommand();
		for (int i = 0; i < Commands.Count; i++)
		{
			ISyncCommand syncCommand = Commands[i];
			serviceCommand.Commands.Add(syncCommand.Clone());
		}
		serviceCommand.ExecutedIndex = ExecutedIndex;
		return serviceCommand;
	}
}
