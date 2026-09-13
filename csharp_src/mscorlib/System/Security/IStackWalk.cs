namespace System.Security;

public interface IStackWalk
{
	void Assert();

	void Demand();

	void Deny();

	void PermitOnly();
}
