using UnityEngine;
using UnityEngine.UI;

public class MultiKillMono : MonoBehaviour
{
	[SerializeField]
	private UIPlayerHead head;

	[SerializeField]
	private Image headFrame;

	[SerializeField]
	private TextMeshProUGUIEx tmp;

	[SerializeField]
	private Image num1;

	[SerializeField]
	private Image num2;

	[SerializeField]
	public SimpleAnimation Anim;

	[SerializeField]
	private TextMeshProUGUIEx squadNum;

	[SerializeField]
	private GameObject plus;

	private float countdown;

	private static readonly string[] NUMBER = new string[10] { "Assets/Main/Sprites/UI/UIMultiKill/number_0.png", "Assets/Main/Sprites/UI/UIMultiKill/number_1.png", "Assets/Main/Sprites/UI/UIMultiKill/number_2.png", "Assets/Main/Sprites/UI/UIMultiKill/number_3.png", "Assets/Main/Sprites/UI/UIMultiKill/number_4.png", "Assets/Main/Sprites/UI/UIMultiKill/number_5.png", "Assets/Main/Sprites/UI/UIMultiKill/number_6.png", "Assets/Main/Sprites/UI/UIMultiKill/number_7.png", "Assets/Main/Sprites/UI/UIMultiKill/number_8.png", "Assets/Main/Sprites/UI/UIMultiKill/number_9.png" };

	private static readonly Color MyColor = new Color(0.46f, 0.93f, 0.18f, 1f);

	private static readonly Color AllyColor = new Color(0.2f, 0.78f, 0.81f, 1f);

	private static readonly Color EnemyColor = new Color(0.89f, 0.23f, 0.2f, 1f);

	public MultiKillBubbleData Data;

	private MultiKillPoint point;

	public void SetData(MultiKillPoint point, MultiKillBubbleData data)
	{
		this.point = point;
		Data = data;
		RefreshView();
	}

	public void RefreshView()
	{
		head.SetData(Data.uid, Data.pic, Data.picVer);
		string spritePath = GameEntry.Lua.CallWithReturn<string, int, long>("CSharpCallLuaInterface.GetHeadFrame", Data.headSkinId, Data.headSkinET);
		headFrame.LoadSprite(spritePath);
		string text = Data.name;
		tmp.text = UIUtils.FormatServerAllianceName(Data.serverId, Data.abbr, text);
		squadNum.text = Data.squadNo.ToString();
		SetColor();
		if (Data.killNum < 10)
		{
			num1.LoadSprite(NUMBER[Data.killNum]);
			num2.gameObject.SetActive(value: false);
			plus.SetActive(value: false);
			return;
		}
		int num;
		if (Data.killNum <= 99)
		{
			num = Data.killNum;
			plus.SetActive(value: false);
		}
		else
		{
			num = 99;
			plus.SetActive(value: true);
		}
		num1.LoadSprite(NUMBER[num / 10]);
		num2.gameObject.SetActive(value: true);
		num2.LoadSprite(NUMBER[num % 10]);
	}

	private void SetColor()
	{
		if (Data.uid == GameEntry.Data.Player.Uid)
		{
			tmp.color = MyColor;
		}
		else if (!string.IsNullOrEmpty(Data.allianceId) && Data.allianceId == GameEntry.Data.Player.GetAllianceId())
		{
			tmp.color = AllyColor;
		}
		else
		{
			tmp.color = EnemyColor;
		}
	}

	public void Dispose()
	{
		Data = null;
		point = null;
	}

	private void Update()
	{
		if (!(countdown <= 0f))
		{
			countdown -= Time.deltaTime;
			if (countdown <= 0f)
			{
				point?.AddDELTask(Data);
			}
		}
	}
}
