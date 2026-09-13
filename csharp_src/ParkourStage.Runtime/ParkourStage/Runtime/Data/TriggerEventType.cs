namespace ParkourStage.Runtime.Data;

public enum TriggerEventType
{
	None = 0,
	CastSkill = 2,
	AddBuff = 10,
	AddSingleHeroBuff = 11,
	AddSkill = 20,
	AddSingleHeroSkill = 21,
	AddSingleHeroIdBuff = 22,
	AddEnergy = 23,
	AddSingleHeroIdSkill = 24,
	ReplaceSingleHeroNormalAttack = 32,
	ReplaceHeroIdNormalAttack = 34,
	ReplaceHeroIdAppearance = 35,
	ReplaceHeroIdNormalBullet = 36,
	ReplaceHeroIdActiveBullet = 37,
	AddHero = 40,
	AddTrialHero = 41,
	RemoveHero = 50,
	SaveHero = 60,
	SaveWorker = 70,
	GetGoods = 80,
	ThreeChoices = 90,
	CountdownGetGoods = 100,
	AlwaysGetGoods = 110,
	SummonMonsterBatch = 120
}
