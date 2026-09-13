using System.Collections;
using System.Xml.Xsl.XsltOld.Debugger;

namespace System.Xml.Xsl.XsltOld;

internal class DbgCompiler : Compiler
{
	private class ApplyImportsActionDbg : ApplyImportsAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class ApplyTemplatesActionDbg : ApplyTemplatesAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class AttributeActionDbg : AttributeAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class AttributeSetActionDbg : AttributeSetAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class CallTemplateActionDbg : CallTemplateAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class CommentActionDbg : CommentAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class CopyActionDbg : CopyAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class CopyOfActionDbg : CopyOfAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class ElementActionDbg : ElementAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class ForEachActionDbg : ForEachAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.PushDebuggerStack();
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
			if (frame.State == -1)
			{
				processor.PopDebuggerStack();
			}
		}
	}

	private class IfActionDbg : IfAction
	{
		private DbgData dbgData;

		internal IfActionDbg(ConditionType type)
			: base(type)
		{
		}

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class MessageActionDbg : MessageAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class NewInstructionActionDbg : NewInstructionAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class NumberActionDbg : NumberAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class ProcessingInstructionActionDbg : ProcessingInstructionAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class RootActionDbg : RootAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
			string builtInTemplatesUri = compiler.Debugger.GetBuiltInTemplatesUri();
			if (builtInTemplatesUri != null && builtInTemplatesUri.Length != 0)
			{
				compiler.AllowBuiltInMode = true;
				builtInSheet = compiler.RootAction.CompileImport(compiler, compiler.ResolveUri(builtInTemplatesUri), int.MaxValue);
				compiler.AllowBuiltInMode = false;
			}
			dbgData.ReplaceVariables(((DbgCompiler)compiler).GlobalVariables);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.PushDebuggerStack();
				processor.OnInstructionExecute();
				processor.PushDebuggerStack();
			}
			base.Execute(processor, frame);
			if (frame.State == -1)
			{
				processor.PopDebuggerStack();
				processor.PopDebuggerStack();
			}
		}
	}

	private class SortActionDbg : SortAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class TemplateActionDbg : TemplateAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.PushDebuggerStack();
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
			if (frame.State == -1)
			{
				processor.PopDebuggerStack();
			}
		}
	}

	private class TextActionDbg : TextAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class UseAttributeSetsActionDbg : UseAttributeSetsAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class ValueOfActionDbg : ValueOfAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class VariableActionDbg : VariableAction
	{
		private DbgData dbgData;

		internal VariableActionDbg(VariableType type)
			: base(type)
		{
		}

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
			((DbgCompiler)compiler).DefineVariable(this);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class WithParamActionDbg : WithParamAction
	{
		private DbgData dbgData;

		internal override DbgData GetDbgData(ActionFrame frame)
		{
			return dbgData;
		}

		internal override void Compile(Compiler compiler)
		{
			dbgData = new DbgData(compiler);
			base.Compile(compiler);
		}

		internal override void Execute(Processor processor, ActionFrame frame)
		{
			if (frame.State == 0)
			{
				processor.OnInstructionExecute();
			}
			base.Execute(processor, frame);
		}
	}

	private class BeginEventDbg : BeginEvent
	{
		private DbgData dbgData;

		internal override DbgData DbgData => dbgData;

		public BeginEventDbg(Compiler compiler)
			: base(compiler)
		{
			dbgData = new DbgData(compiler);
		}

		public override bool Output(Processor processor, ActionFrame frame)
		{
			OnInstructionExecute(processor);
			return base.Output(processor, frame);
		}
	}

	private class TextEventDbg : TextEvent
	{
		private DbgData dbgData;

		internal override DbgData DbgData => dbgData;

		public TextEventDbg(Compiler compiler)
			: base(compiler)
		{
			dbgData = new DbgData(compiler);
		}

		public override bool Output(Processor processor, ActionFrame frame)
		{
			OnInstructionExecute(processor);
			return base.Output(processor, frame);
		}
	}

	private IXsltDebugger debugger;

	private ArrayList globalVars = new ArrayList();

	private ArrayList localVars = new ArrayList();

	private VariableAction[] globalVarsCache;

	private VariableAction[] localVarsCache;

	public override IXsltDebugger Debugger => debugger;

	public virtual VariableAction[] GlobalVariables
	{
		get
		{
			if (globalVarsCache == null)
			{
				globalVarsCache = (VariableAction[])globalVars.ToArray(typeof(VariableAction));
			}
			return globalVarsCache;
		}
	}

	public virtual VariableAction[] LocalVariables
	{
		get
		{
			if (localVarsCache == null)
			{
				localVarsCache = (VariableAction[])localVars.ToArray(typeof(VariableAction));
			}
			return localVarsCache;
		}
	}

	public DbgCompiler(IXsltDebugger debugger)
	{
		this.debugger = debugger;
	}

	private void DefineVariable(VariableAction variable)
	{
		if (variable.IsGlobal)
		{
			for (int i = 0; i < globalVars.Count; i++)
			{
				VariableAction variableAction = (VariableAction)globalVars[i];
				if (variableAction.Name == variable.Name)
				{
					if (variable.Stylesheetid < variableAction.Stylesheetid)
					{
						globalVars[i] = variable;
						globalVarsCache = null;
					}
					return;
				}
			}
			globalVars.Add(variable);
			globalVarsCache = null;
		}
		else
		{
			localVars.Add(variable);
			localVarsCache = null;
		}
	}

	private void UnDefineVariables(int count)
	{
		if (count != 0)
		{
			localVars.RemoveRange(localVars.Count - count, count);
			localVarsCache = null;
		}
	}

	internal override void PopScope()
	{
		UnDefineVariables(base.ScopeManager.CurrentScope.GetVeriablesCount());
		base.PopScope();
	}

	public override ApplyImportsAction CreateApplyImportsAction()
	{
		ApplyImportsActionDbg applyImportsActionDbg = new ApplyImportsActionDbg();
		applyImportsActionDbg.Compile(this);
		return applyImportsActionDbg;
	}

	public override ApplyTemplatesAction CreateApplyTemplatesAction()
	{
		ApplyTemplatesActionDbg applyTemplatesActionDbg = new ApplyTemplatesActionDbg();
		applyTemplatesActionDbg.Compile(this);
		return applyTemplatesActionDbg;
	}

	public override AttributeAction CreateAttributeAction()
	{
		AttributeActionDbg attributeActionDbg = new AttributeActionDbg();
		attributeActionDbg.Compile(this);
		return attributeActionDbg;
	}

	public override AttributeSetAction CreateAttributeSetAction()
	{
		AttributeSetActionDbg attributeSetActionDbg = new AttributeSetActionDbg();
		attributeSetActionDbg.Compile(this);
		return attributeSetActionDbg;
	}

	public override CallTemplateAction CreateCallTemplateAction()
	{
		CallTemplateActionDbg callTemplateActionDbg = new CallTemplateActionDbg();
		callTemplateActionDbg.Compile(this);
		return callTemplateActionDbg;
	}

	public override ChooseAction CreateChooseAction()
	{
		ChooseAction chooseAction = new ChooseAction();
		chooseAction.Compile(this);
		return chooseAction;
	}

	public override CommentAction CreateCommentAction()
	{
		CommentActionDbg commentActionDbg = new CommentActionDbg();
		commentActionDbg.Compile(this);
		return commentActionDbg;
	}

	public override CopyAction CreateCopyAction()
	{
		CopyActionDbg copyActionDbg = new CopyActionDbg();
		copyActionDbg.Compile(this);
		return copyActionDbg;
	}

	public override CopyOfAction CreateCopyOfAction()
	{
		CopyOfActionDbg copyOfActionDbg = new CopyOfActionDbg();
		copyOfActionDbg.Compile(this);
		return copyOfActionDbg;
	}

	public override ElementAction CreateElementAction()
	{
		ElementActionDbg elementActionDbg = new ElementActionDbg();
		elementActionDbg.Compile(this);
		return elementActionDbg;
	}

	public override ForEachAction CreateForEachAction()
	{
		ForEachActionDbg forEachActionDbg = new ForEachActionDbg();
		forEachActionDbg.Compile(this);
		return forEachActionDbg;
	}

	public override IfAction CreateIfAction(IfAction.ConditionType type)
	{
		IfActionDbg ifActionDbg = new IfActionDbg(type);
		ifActionDbg.Compile(this);
		return ifActionDbg;
	}

	public override MessageAction CreateMessageAction()
	{
		MessageActionDbg messageActionDbg = new MessageActionDbg();
		messageActionDbg.Compile(this);
		return messageActionDbg;
	}

	public override NewInstructionAction CreateNewInstructionAction()
	{
		NewInstructionActionDbg newInstructionActionDbg = new NewInstructionActionDbg();
		newInstructionActionDbg.Compile(this);
		return newInstructionActionDbg;
	}

	public override NumberAction CreateNumberAction()
	{
		NumberActionDbg numberActionDbg = new NumberActionDbg();
		numberActionDbg.Compile(this);
		return numberActionDbg;
	}

	public override ProcessingInstructionAction CreateProcessingInstructionAction()
	{
		ProcessingInstructionActionDbg processingInstructionActionDbg = new ProcessingInstructionActionDbg();
		processingInstructionActionDbg.Compile(this);
		return processingInstructionActionDbg;
	}

	public override void CreateRootAction()
	{
		base.RootAction = new RootActionDbg();
		base.RootAction.Compile(this);
	}

	public override SortAction CreateSortAction()
	{
		SortActionDbg sortActionDbg = new SortActionDbg();
		sortActionDbg.Compile(this);
		return sortActionDbg;
	}

	public override TemplateAction CreateTemplateAction()
	{
		TemplateActionDbg templateActionDbg = new TemplateActionDbg();
		templateActionDbg.Compile(this);
		return templateActionDbg;
	}

	public override TemplateAction CreateSingleTemplateAction()
	{
		TemplateActionDbg templateActionDbg = new TemplateActionDbg();
		templateActionDbg.CompileSingle(this);
		return templateActionDbg;
	}

	public override TextAction CreateTextAction()
	{
		TextActionDbg textActionDbg = new TextActionDbg();
		textActionDbg.Compile(this);
		return textActionDbg;
	}

	public override UseAttributeSetsAction CreateUseAttributeSetsAction()
	{
		UseAttributeSetsActionDbg useAttributeSetsActionDbg = new UseAttributeSetsActionDbg();
		useAttributeSetsActionDbg.Compile(this);
		return useAttributeSetsActionDbg;
	}

	public override ValueOfAction CreateValueOfAction()
	{
		ValueOfActionDbg valueOfActionDbg = new ValueOfActionDbg();
		valueOfActionDbg.Compile(this);
		return valueOfActionDbg;
	}

	public override VariableAction CreateVariableAction(VariableType type)
	{
		VariableActionDbg variableActionDbg = new VariableActionDbg(type);
		variableActionDbg.Compile(this);
		return variableActionDbg;
	}

	public override WithParamAction CreateWithParamAction()
	{
		WithParamActionDbg withParamActionDbg = new WithParamActionDbg();
		withParamActionDbg.Compile(this);
		return withParamActionDbg;
	}

	public override BeginEvent CreateBeginEvent()
	{
		return new BeginEventDbg(this);
	}

	public override TextEvent CreateTextEvent()
	{
		return new TextEventDbg(this);
	}
}
