using System;

namespace MiniGame.Core.Server;

public interface IGameLogic : IDisposable
{
	bool AddPlayer(IGameLogicSession player);

	bool DelPlayer(IGameLogicSession player);

	bool AddOBPlayer(IGameLogicSession player);

	bool DelOBPlayer(IGameLogicSession player);

	bool IsValid();

	bool IsSettlement();

	IGameLogicSettlement GetSettlement();

	void Update(float elapsed);

	void HandleMessage(IGameLogicSession session, int op, object data);

	bool IsOpCode(Type type, int op);

	bool IsOpCode<Type>(int op);
}
