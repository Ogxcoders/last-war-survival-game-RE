using System;
using System.Collections.Generic;
using FibMatrix;

public class BehaviourTreeManager
{
	private static BehaviourTreeManager instance;

	private ObjectPool<BehaviourTree> treePool;

	private ObjectPool<SimpleActionNode> simpleActionNodePool;

	private ObjectPool<WaitNode> waitNodePool;

	private ObjectPool<SelectorNode> selectorNodePool;

	private ObjectPool<SequenceNode> sequenceNodePool;

	private ObjectPool<ConditionNode> conditionNodePool;

	private ObjectPool<LoopNode> loopNodePool;

	public static BehaviourTreeManager GetInstance()
	{
		if (instance == null)
		{
			instance = new BehaviourTreeManager();
			instance.Init();
		}
		return instance;
	}

	private void Init()
	{
		treePool = new ObjectPool<BehaviourTree>();
		simpleActionNodePool = new ObjectPool<SimpleActionNode>();
		waitNodePool = new ObjectPool<WaitNode>();
		sequenceNodePool = new ObjectPool<SequenceNode>();
		selectorNodePool = new ObjectPool<SelectorNode>();
		conditionNodePool = new ObjectPool<ConditionNode>();
		loopNodePool = new ObjectPool<LoopNode>();
	}

	public void Clear()
	{
		treePool.Release();
		simpleActionNodePool.Release();
		waitNodePool.Release();
		sequenceNodePool.Release();
		selectorNodePool.Release();
		conditionNodePool.Release();
		loopNodePool.Release();
	}

	public void Destroy()
	{
		treePool.Dispose();
		simpleActionNodePool.Dispose();
		waitNodePool.Dispose();
		sequenceNodePool.Dispose();
		selectorNodePool.Dispose();
		conditionNodePool.Dispose();
		loopNodePool.Dispose();
		treePool = null;
		simpleActionNodePool = null;
		waitNodePool = null;
		sequenceNodePool = null;
		selectorNodePool = null;
		conditionNodePool = null;
		loopNodePool = null;
	}

	public BehaviourTree CreateTree(BTNode root)
	{
		return treePool.Allocate().Init(root);
	}

	public void RecycleTree(BehaviourTree tree)
	{
		treePool.Recycle(tree);
	}

	public SimpleActionNode CreateSimpleActionNode(Action action)
	{
		return simpleActionNodePool.Allocate().Init(action);
	}

	public void RecycleSimpleActionNode(SimpleActionNode node)
	{
		simpleActionNodePool.Recycle(node);
	}

	public WaitNode CreateWaitNode(float time)
	{
		return waitNodePool.Allocate().Init(time);
	}

	public void RecycleWaitNode(WaitNode node)
	{
		waitNodePool.Recycle(node);
	}

	public SelectorNode CreateSelectorNode()
	{
		return selectorNodePool.Allocate();
	}

	public SelectorNode CreateSelectorNode(List<BTNode> children)
	{
		return selectorNodePool.Allocate().Init(children);
	}

	public void RecycleSelectorNode(SelectorNode node)
	{
		selectorNodePool.Recycle(node);
	}

	public SequenceNode CreateSequenceNode()
	{
		return sequenceNodePool.Allocate();
	}

	public SequenceNode CreateSequenceNode(List<BTNode> children)
	{
		return sequenceNodePool.Allocate().Init(children);
	}

	public void RecycleSequenceNode(SequenceNode node)
	{
		sequenceNodePool.Recycle(node);
	}

	public ConditionNode CreateConditionNode(Func<bool> conditionFunction)
	{
		return conditionNodePool.Allocate().Init(conditionFunction);
	}

	public void RecycleConditionNode(ConditionNode node)
	{
		conditionNodePool.Recycle(node);
	}

	public LoopNode CreateLoopNode(BTNode child)
	{
		return loopNodePool.Allocate().Init(child);
	}

	public void RecycleLoopNode(LoopNode node)
	{
		loopNodePool.Recycle(node);
	}
}
