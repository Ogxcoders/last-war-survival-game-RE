using System.Collections.Generic;
using UnityEngine;

namespace SuperScrollView;

public class ChatMsgDataSourceMgr : MonoBehaviour
{
	private Dictionary<int, PersonInfo> mPersonInfoDict = new Dictionary<int, PersonInfo>();

	private List<ChatMsg> mChatMsgList = new List<ChatMsg>();

	private static ChatMsgDataSourceMgr instance = null;

	private static string[] mChatDemoStrList = new string[5] { "Support ListView and GridView.", "Support Infinity Vertical and Horizontal ScrollView.", "Support items in different sizes such as widths or heights. Support items with unknown size at init time.", "Support changing item count and item size at runtime. Support looping items such as spinners. Support item padding.", "Use only one C# script to help the UGUI ScrollRect to support any count items with high performance." };

	private static string[] mChatDemoPicList = new string[4] { "grid_pencil_128_g2", "grid_flower_200_3", "grid_pencil_128_g3", "grid_flower_200_7" };

	public static ChatMsgDataSourceMgr Get
	{
		get
		{
			if (instance == null)
			{
				instance = Object.FindObjectOfType<ChatMsgDataSourceMgr>();
			}
			return instance;
		}
	}

	public int TotalItemCount => mChatMsgList.Count;

	private void Awake()
	{
		Init();
	}

	public PersonInfo GetPersonInfo(int personId)
	{
		PersonInfo value = null;
		if (mPersonInfoDict.TryGetValue(personId, out value))
		{
			return value;
		}
		return null;
	}

	public void Init()
	{
		mPersonInfoDict.Clear();
		PersonInfo personInfo = new PersonInfo();
		personInfo.mHeadIcon = "grid_pencil_128_g8";
		personInfo.mId = 0;
		personInfo.mName = "Jaci";
		mPersonInfoDict.Add(personInfo.mId, personInfo);
		personInfo = new PersonInfo();
		personInfo.mHeadIcon = "grid_pencil_128_g5";
		personInfo.mId = 1;
		personInfo.mName = "Toc";
		mPersonInfoDict.Add(personInfo.mId, personInfo);
		InitChatDataSource();
	}

	public ChatMsg GetChatMsgByIndex(int index)
	{
		if (index < 0 || index >= mChatMsgList.Count)
		{
			return null;
		}
		return mChatMsgList[index];
	}

	private void InitChatDataSource()
	{
		mChatMsgList.Clear();
		mChatMsgList.AddRange(GenChatDataSource(1000, reverse: false));
	}

	private List<ChatMsg> GenChatDataSource(int seqId, bool reverse)
	{
		List<ChatMsg> list = new List<ChatMsg>();
		int num = mChatDemoStrList.Length;
		int num2 = mChatDemoPicList.Length;
		for (int i = 0; i < 20; i++)
		{
			ChatMsg chatMsg = new ChatMsg();
			chatMsg.mSeqId = (reverse ? (--seqId) : (++seqId));
			chatMsg.mMsgType = (MsgTypeEnum)(Random.Range(0, 99) % 2);
			chatMsg.mPersonId = Random.Range(0, 99) % 2;
			chatMsg.mSrtMsg = mChatDemoStrList[Random.Range(0, 99) % num];
			chatMsg.mPicMsgSpriteName = mChatDemoPicList[Random.Range(0, 99) % num2];
			list.Add(chatMsg);
		}
		if (reverse)
		{
			list.Reverse();
		}
		return list;
	}

	public void InsertFront()
	{
		int mSeqId = mChatMsgList[0].mSeqId;
		mChatMsgList.InsertRange(0, GenChatDataSource(mSeqId, reverse: true));
	}

	public void AppendTail()
	{
		int mSeqId = mChatMsgList[mChatMsgList.Count - 1].mSeqId;
		mChatMsgList.AddRange(GenChatDataSource(mSeqId, reverse: false));
	}
}
