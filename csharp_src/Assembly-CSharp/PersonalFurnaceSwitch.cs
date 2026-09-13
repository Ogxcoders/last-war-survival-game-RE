using UnityEngine;

public class PersonalFurnaceSwitch : CityBuildingSwitchBase
{
	protected override void OnInit()
	{
		base.OnInit();
		DoModelLogic(null);
	}

	protected override void OnRelease()
	{
		base.OnRelease();
		for (int i = 0; i < _models.Count; i++)
		{
			_models[i].SetActive(i == 0);
		}
	}

	public override void DoModelLogic(object userData)
	{
		int num = GameEntry.Lua.CallWithReturn<int>("CSharpCallLuaInterface.GetCityFurnaceState");
		GameObject obj = null;
		switch (num)
		{
		case -1:
		case 0:
			obj = _models[0];
			break;
		case 1:
			obj = _models[1];
			break;
		case 2:
			obj = _models[2];
			break;
		}
		_changeModelCall?.Invoke(obj);
	}
}
