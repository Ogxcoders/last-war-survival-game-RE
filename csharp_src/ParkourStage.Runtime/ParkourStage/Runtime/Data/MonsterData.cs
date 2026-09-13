namespace ParkourStage.Runtime.Data;

public class MonsterData
{
	public string MonsterId = string.Empty;

	public string MonsterPath = string.Empty;

	public MonsterType MonsterType;

	public string DynamicObjectPath = string.Empty;

	public float AlertRange;

	public int DoorNumber;

	public int PropertyHp;

	public float Size = 1f;

	public TriggerData DeathTrigger;
}
