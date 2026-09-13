namespace MiniGame.Biubiu.Client;

public class DataUIEnemyController : DataUIEntityController
{
	public override void Die()
	{
		PlayAnimation("dead01", 0);
		base.Die();
	}
}
