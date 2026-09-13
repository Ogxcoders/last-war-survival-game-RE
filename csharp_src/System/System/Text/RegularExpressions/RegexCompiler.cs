using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Text.RegularExpressions;

internal abstract class RegexCompiler
{
	private sealed class BacktrackNote
	{
		internal int _codepos;

		internal int _flags;

		internal Label _label;

		public BacktrackNote(int flags, Label label, int codepos)
		{
			_codepos = codepos;
			_flags = flags;
			_label = label;
		}
	}

	private static FieldInfo s_textbegF;

	private static FieldInfo s_textendF;

	private static FieldInfo s_textstartF;

	private static FieldInfo s_textposF;

	private static FieldInfo s_textF;

	private static FieldInfo s_trackposF;

	private static FieldInfo s_trackF;

	private static FieldInfo s_stackposF;

	private static FieldInfo s_stackF;

	private static FieldInfo s_trackcountF;

	private static MethodInfo s_ensurestorageM;

	private static MethodInfo s_captureM;

	private static MethodInfo s_transferM;

	private static MethodInfo s_uncaptureM;

	private static MethodInfo s_ismatchedM;

	private static MethodInfo s_matchlengthM;

	private static MethodInfo s_matchindexM;

	private static MethodInfo s_isboundaryM;

	private static MethodInfo s_isECMABoundaryM;

	private static MethodInfo s_chartolowerM;

	private static MethodInfo s_getcharM;

	private static MethodInfo s_crawlposM;

	private static MethodInfo s_charInSetM;

	private static MethodInfo s_getCurrentCulture;

	private static MethodInfo s_getInvariantCulture;

	private static MethodInfo s_checkTimeoutM;

	protected ILGenerator _ilg;

	private LocalBuilder _textstartV;

	private LocalBuilder _textbegV;

	private LocalBuilder _textendV;

	private LocalBuilder _textposV;

	private LocalBuilder _textV;

	private LocalBuilder _trackposV;

	private LocalBuilder _trackV;

	private LocalBuilder _stackposV;

	private LocalBuilder _stackV;

	private LocalBuilder _tempV;

	private LocalBuilder _temp2V;

	private LocalBuilder _temp3V;

	protected RegexCode _code;

	protected int[] _codes;

	protected string[] _strings;

	protected RegexPrefix? _fcPrefix;

	protected RegexBoyerMoore _bmPrefix;

	protected int _anchors;

	private Label[] _labels;

	private BacktrackNote[] _notes;

	private int _notecount;

	protected int _trackcount;

	private Label _backtrack;

	private int _regexopcode;

	private int _codepos;

	private int _backpos;

	protected RegexOptions _options;

	private int[] _uniquenote;

	private int[] _goto;

	private const int Stackpop = 0;

	private const int Stackpop2 = 1;

	private const int Stackpop3 = 2;

	private const int Capback = 3;

	private const int Capback2 = 4;

	private const int Branchmarkback2 = 5;

	private const int Lazybranchmarkback2 = 6;

	private const int Branchcountback2 = 7;

	private const int Lazybranchcountback2 = 8;

	private const int Forejumpback = 9;

	private const int Uniquecount = 10;

	static RegexCompiler()
	{
		s_textbegF = RegexRunnerField("runtextbeg");
		s_textendF = RegexRunnerField("runtextend");
		s_textstartF = RegexRunnerField("runtextstart");
		s_textposF = RegexRunnerField("runtextpos");
		s_textF = RegexRunnerField("runtext");
		s_trackposF = RegexRunnerField("runtrackpos");
		s_trackF = RegexRunnerField("runtrack");
		s_stackposF = RegexRunnerField("runstackpos");
		s_stackF = RegexRunnerField("runstack");
		s_trackcountF = RegexRunnerField("runtrackcount");
		s_ensurestorageM = RegexRunnerMethod("EnsureStorage");
		s_captureM = RegexRunnerMethod("Capture");
		s_transferM = RegexRunnerMethod("TransferCapture");
		s_uncaptureM = RegexRunnerMethod("Uncapture");
		s_ismatchedM = RegexRunnerMethod("IsMatched");
		s_matchlengthM = RegexRunnerMethod("MatchLength");
		s_matchindexM = RegexRunnerMethod("MatchIndex");
		s_isboundaryM = RegexRunnerMethod("IsBoundary");
		s_charInSetM = RegexRunnerMethod("CharInClass");
		s_isECMABoundaryM = RegexRunnerMethod("IsECMABoundary");
		s_crawlposM = RegexRunnerMethod("Crawlpos");
		s_checkTimeoutM = RegexRunnerMethod("CheckTimeout");
		s_chartolowerM = typeof(char).GetMethod("ToLower", new Type[2]
		{
			typeof(char),
			typeof(CultureInfo)
		});
		s_getcharM = typeof(string).GetMethod("get_Chars", new Type[1] { typeof(int) });
		s_getCurrentCulture = typeof(CultureInfo).GetMethod("get_CurrentCulture");
		s_getInvariantCulture = typeof(CultureInfo).GetMethod("get_InvariantCulture");
	}

	private static FieldInfo RegexRunnerField(string fieldname)
	{
		return typeof(RegexRunner).GetField(fieldname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
	}

	private static MethodInfo RegexRunnerMethod(string methname)
	{
		return typeof(RegexRunner).GetMethod(methname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
	}

	internal static RegexRunnerFactory Compile(RegexCode code, RegexOptions options)
	{
		return new RegexLWCGCompiler().FactoryInstanceFromCode(code, options);
	}

	private int AddBacktrackNote(int flags, Label l, int codepos)
	{
		if (_notes == null || _notecount >= _notes.Length)
		{
			BacktrackNote[] array = new BacktrackNote[(_notes == null) ? 16 : (_notes.Length * 2)];
			if (_notes != null)
			{
				Array.Copy(_notes, 0, array, 0, _notecount);
			}
			_notes = array;
		}
		_notes[_notecount] = new BacktrackNote(flags, l, codepos);
		return _notecount++;
	}

	private int AddTrack()
	{
		return AddTrack(128);
	}

	private int AddTrack(int flags)
	{
		return AddBacktrackNote(flags, DefineLabel(), _codepos);
	}

	private int AddGoto(int destpos)
	{
		if (_goto[destpos] == -1)
		{
			_goto[destpos] = AddBacktrackNote(0, _labels[destpos], destpos);
		}
		return _goto[destpos];
	}

	private int AddUniqueTrack(int i)
	{
		return AddUniqueTrack(i, 128);
	}

	private int AddUniqueTrack(int i, int flags)
	{
		if (_uniquenote[i] == -1)
		{
			_uniquenote[i] = AddTrack(flags);
		}
		return _uniquenote[i];
	}

	private Label DefineLabel()
	{
		return _ilg.DefineLabel();
	}

	private void MarkLabel(Label l)
	{
		_ilg.MarkLabel(l);
	}

	private int Operand(int i)
	{
		return _codes[_codepos + i + 1];
	}

	private bool IsRtl()
	{
		return (_regexopcode & 0x40) != 0;
	}

	private bool IsCi()
	{
		return (_regexopcode & 0x200) != 0;
	}

	private int Code()
	{
		return _regexopcode & 0x3F;
	}

	private void Ldstr(string str)
	{
		_ilg.Emit(OpCodes.Ldstr, str);
	}

	private void Ldc(int i)
	{
		if (i <= 127 && i >= -128)
		{
			_ilg.Emit(OpCodes.Ldc_I4_S, (byte)i);
		}
		else
		{
			_ilg.Emit(OpCodes.Ldc_I4, i);
		}
	}

	private void LdcI8(long i)
	{
		if (i <= int.MaxValue && i >= int.MinValue)
		{
			Ldc((int)i);
			_ilg.Emit(OpCodes.Conv_I8);
		}
		else
		{
			_ilg.Emit(OpCodes.Ldc_I8, i);
		}
	}

	private void Dup()
	{
		_ilg.Emit(OpCodes.Dup);
	}

	private void Ret()
	{
		_ilg.Emit(OpCodes.Ret);
	}

	private void Pop()
	{
		_ilg.Emit(OpCodes.Pop);
	}

	private void Add()
	{
		_ilg.Emit(OpCodes.Add);
	}

	private void Add(bool negate)
	{
		if (negate)
		{
			_ilg.Emit(OpCodes.Sub);
		}
		else
		{
			_ilg.Emit(OpCodes.Add);
		}
	}

	private void Sub()
	{
		_ilg.Emit(OpCodes.Sub);
	}

	private void Sub(bool negate)
	{
		if (negate)
		{
			_ilg.Emit(OpCodes.Add);
		}
		else
		{
			_ilg.Emit(OpCodes.Sub);
		}
	}

	private void Ldloc(LocalBuilder lt)
	{
		_ilg.Emit(OpCodes.Ldloc_S, lt);
	}

	private void Stloc(LocalBuilder lt)
	{
		_ilg.Emit(OpCodes.Stloc_S, lt);
	}

	private void Ldthis()
	{
		_ilg.Emit(OpCodes.Ldarg_0);
	}

	private void Ldthisfld(FieldInfo ft)
	{
		Ldthis();
		_ilg.Emit(OpCodes.Ldfld, ft);
	}

	private void Mvfldloc(FieldInfo ft, LocalBuilder lt)
	{
		Ldthisfld(ft);
		Stloc(lt);
	}

	private void Mvlocfld(LocalBuilder lt, FieldInfo ft)
	{
		Ldthis();
		Ldloc(lt);
		Stfld(ft);
	}

	private void Stfld(FieldInfo ft)
	{
		_ilg.Emit(OpCodes.Stfld, ft);
	}

	private void Callvirt(MethodInfo mt)
	{
		_ilg.Emit(OpCodes.Callvirt, mt);
	}

	private void Call(MethodInfo mt)
	{
		_ilg.Emit(OpCodes.Call, mt);
	}

	private void Newobj(ConstructorInfo ct)
	{
		_ilg.Emit(OpCodes.Newobj, ct);
	}

	private void BrfalseFar(Label l)
	{
		_ilg.Emit(OpCodes.Brfalse, l);
	}

	private void BrtrueFar(Label l)
	{
		_ilg.Emit(OpCodes.Brtrue, l);
	}

	private void BrFar(Label l)
	{
		_ilg.Emit(OpCodes.Br, l);
	}

	private void BleFar(Label l)
	{
		_ilg.Emit(OpCodes.Ble, l);
	}

	private void BltFar(Label l)
	{
		_ilg.Emit(OpCodes.Blt, l);
	}

	private void BgeFar(Label l)
	{
		_ilg.Emit(OpCodes.Bge, l);
	}

	private void BgtFar(Label l)
	{
		_ilg.Emit(OpCodes.Bgt, l);
	}

	private void BneFar(Label l)
	{
		_ilg.Emit(OpCodes.Bne_Un, l);
	}

	private void BeqFar(Label l)
	{
		_ilg.Emit(OpCodes.Beq, l);
	}

	private void Brfalse(Label l)
	{
		_ilg.Emit(OpCodes.Brfalse_S, l);
	}

	private void Br(Label l)
	{
		_ilg.Emit(OpCodes.Br_S, l);
	}

	private void Ble(Label l)
	{
		_ilg.Emit(OpCodes.Ble_S, l);
	}

	private void Blt(Label l)
	{
		_ilg.Emit(OpCodes.Blt_S, l);
	}

	private void Bge(Label l)
	{
		_ilg.Emit(OpCodes.Bge_S, l);
	}

	private void Bgt(Label l)
	{
		_ilg.Emit(OpCodes.Bgt_S, l);
	}

	private void Bgtun(Label l)
	{
		_ilg.Emit(OpCodes.Bgt_Un_S, l);
	}

	private void Bne(Label l)
	{
		_ilg.Emit(OpCodes.Bne_Un_S, l);
	}

	private void Beq(Label l)
	{
		_ilg.Emit(OpCodes.Beq_S, l);
	}

	private void Ldlen()
	{
		_ilg.Emit(OpCodes.Ldlen);
	}

	private void Rightchar()
	{
		Ldloc(_textV);
		Ldloc(_textposV);
		Callvirt(s_getcharM);
	}

	private void Rightcharnext()
	{
		Ldloc(_textV);
		Ldloc(_textposV);
		Dup();
		Ldc(1);
		Add();
		Stloc(_textposV);
		Callvirt(s_getcharM);
	}

	private void Leftchar()
	{
		Ldloc(_textV);
		Ldloc(_textposV);
		Ldc(1);
		Sub();
		Callvirt(s_getcharM);
	}

	private void Leftcharnext()
	{
		Ldloc(_textV);
		Ldloc(_textposV);
		Ldc(1);
		Sub();
		Dup();
		Stloc(_textposV);
		Callvirt(s_getcharM);
	}

	private void Track()
	{
		ReadyPushTrack();
		Ldc(AddTrack());
		DoPush();
	}

	private void Trackagain()
	{
		ReadyPushTrack();
		Ldc(_backpos);
		DoPush();
	}

	private void PushTrack(LocalBuilder lt)
	{
		ReadyPushTrack();
		Ldloc(lt);
		DoPush();
	}

	private void TrackUnique(int i)
	{
		ReadyPushTrack();
		Ldc(AddUniqueTrack(i));
		DoPush();
	}

	private void TrackUnique2(int i)
	{
		ReadyPushTrack();
		Ldc(AddUniqueTrack(i, 256));
		DoPush();
	}

	private void ReadyPushTrack()
	{
		_ilg.Emit(OpCodes.Ldloc_S, _trackV);
		_ilg.Emit(OpCodes.Ldloc_S, _trackposV);
		_ilg.Emit(OpCodes.Ldc_I4_1);
		_ilg.Emit(OpCodes.Sub);
		_ilg.Emit(OpCodes.Dup);
		_ilg.Emit(OpCodes.Stloc_S, _trackposV);
	}

	private void PopTrack()
	{
		_ilg.Emit(OpCodes.Ldloc_S, _trackV);
		_ilg.Emit(OpCodes.Ldloc_S, _trackposV);
		_ilg.Emit(OpCodes.Dup);
		_ilg.Emit(OpCodes.Ldc_I4_1);
		_ilg.Emit(OpCodes.Add);
		_ilg.Emit(OpCodes.Stloc_S, _trackposV);
		_ilg.Emit(OpCodes.Ldelem_I4);
	}

	private void TopTrack()
	{
		_ilg.Emit(OpCodes.Ldloc_S, _trackV);
		_ilg.Emit(OpCodes.Ldloc_S, _trackposV);
		_ilg.Emit(OpCodes.Ldelem_I4);
	}

	private void PushStack(LocalBuilder lt)
	{
		ReadyPushStack();
		_ilg.Emit(OpCodes.Ldloc_S, lt);
		DoPush();
	}

	internal void ReadyReplaceStack(int i)
	{
		_ilg.Emit(OpCodes.Ldloc_S, _stackV);
		_ilg.Emit(OpCodes.Ldloc_S, _stackposV);
		if (i != 0)
		{
			Ldc(i);
			_ilg.Emit(OpCodes.Add);
		}
	}

	private void ReadyPushStack()
	{
		_ilg.Emit(OpCodes.Ldloc_S, _stackV);
		_ilg.Emit(OpCodes.Ldloc_S, _stackposV);
		_ilg.Emit(OpCodes.Ldc_I4_1);
		_ilg.Emit(OpCodes.Sub);
		_ilg.Emit(OpCodes.Dup);
		_ilg.Emit(OpCodes.Stloc_S, _stackposV);
	}

	private void TopStack()
	{
		_ilg.Emit(OpCodes.Ldloc_S, _stackV);
		_ilg.Emit(OpCodes.Ldloc_S, _stackposV);
		_ilg.Emit(OpCodes.Ldelem_I4);
	}

	private void PopStack()
	{
		_ilg.Emit(OpCodes.Ldloc_S, _stackV);
		_ilg.Emit(OpCodes.Ldloc_S, _stackposV);
		_ilg.Emit(OpCodes.Dup);
		_ilg.Emit(OpCodes.Ldc_I4_1);
		_ilg.Emit(OpCodes.Add);
		_ilg.Emit(OpCodes.Stloc_S, _stackposV);
		_ilg.Emit(OpCodes.Ldelem_I4);
	}

	private void PopDiscardStack()
	{
		PopDiscardStack(1);
	}

	private void PopDiscardStack(int i)
	{
		_ilg.Emit(OpCodes.Ldloc_S, _stackposV);
		Ldc(i);
		_ilg.Emit(OpCodes.Add);
		_ilg.Emit(OpCodes.Stloc_S, _stackposV);
	}

	private void DoReplace()
	{
		_ilg.Emit(OpCodes.Stelem_I4);
	}

	private void DoPush()
	{
		_ilg.Emit(OpCodes.Stelem_I4);
	}

	private void Back()
	{
		_ilg.Emit(OpCodes.Br, _backtrack);
	}

	private void Goto(int i)
	{
		if (i < _codepos)
		{
			Label l = DefineLabel();
			Ldloc(_trackposV);
			Ldc(_trackcount * 4);
			Ble(l);
			Ldloc(_stackposV);
			Ldc(_trackcount * 3);
			BgtFar(_labels[i]);
			MarkLabel(l);
			ReadyPushTrack();
			Ldc(AddGoto(i));
			DoPush();
			BrFar(_backtrack);
		}
		else
		{
			BrFar(_labels[i]);
		}
	}

	private int NextCodepos()
	{
		return _codepos + RegexCode.OpcodeSize(_codes[_codepos]);
	}

	private Label AdvanceLabel()
	{
		return _labels[NextCodepos()];
	}

	private void Advance()
	{
		_ilg.Emit(OpCodes.Br, AdvanceLabel());
	}

	private void CallToLower()
	{
		if ((_options & RegexOptions.CultureInvariant) != RegexOptions.None)
		{
			Call(s_getInvariantCulture);
		}
		else
		{
			Call(s_getCurrentCulture);
		}
		Call(s_chartolowerM);
	}

	private void GenerateForwardSection()
	{
		_labels = new Label[_codes.Length];
		_goto = new int[_codes.Length];
		for (int i = 0; i < _codes.Length; i += RegexCode.OpcodeSize(_codes[i]))
		{
			_goto[i] = -1;
			_labels[i] = _ilg.DefineLabel();
		}
		_uniquenote = new int[10];
		for (int j = 0; j < 10; j++)
		{
			_uniquenote[j] = -1;
		}
		Mvfldloc(s_textF, _textV);
		Mvfldloc(s_textstartF, _textstartV);
		Mvfldloc(s_textbegF, _textbegV);
		Mvfldloc(s_textendF, _textendV);
		Mvfldloc(s_textposF, _textposV);
		Mvfldloc(s_trackF, _trackV);
		Mvfldloc(s_trackposF, _trackposV);
		Mvfldloc(s_stackF, _stackV);
		Mvfldloc(s_stackposF, _stackposV);
		_backpos = -1;
		for (int i = 0; i < _codes.Length; i += RegexCode.OpcodeSize(_codes[i]))
		{
			MarkLabel(_labels[i]);
			_codepos = i;
			_regexopcode = _codes[i];
			GenerateOneCode();
		}
	}

	private void GenerateMiddleSection()
	{
		DefineLabel();
		MarkLabel(_backtrack);
		Mvlocfld(_trackposV, s_trackposF);
		Mvlocfld(_stackposV, s_stackposF);
		Ldthis();
		Callvirt(s_ensurestorageM);
		Mvfldloc(s_trackposF, _trackposV);
		Mvfldloc(s_stackposF, _stackposV);
		Mvfldloc(s_trackF, _trackV);
		Mvfldloc(s_stackF, _stackV);
		PopTrack();
		Label[] array = new Label[_notecount];
		for (int i = 0; i < _notecount; i++)
		{
			array[i] = _notes[i]._label;
		}
		_ilg.Emit(OpCodes.Switch, array);
	}

	private void GenerateBacktrackSection()
	{
		for (int i = 0; i < _notecount; i++)
		{
			BacktrackNote backtrackNote = _notes[i];
			if (backtrackNote._flags != 0)
			{
				_ilg.MarkLabel(backtrackNote._label);
				_codepos = backtrackNote._codepos;
				_backpos = i;
				_regexopcode = _codes[backtrackNote._codepos] | backtrackNote._flags;
				GenerateOneCode();
			}
		}
	}

	protected void GenerateFindFirstChar()
	{
		_textposV = DeclareInt();
		_textV = DeclareString();
		_tempV = DeclareInt();
		_temp2V = DeclareInt();
		if ((_anchors & 0x35) != 0)
		{
			if (!_code.RightToLeft)
			{
				if ((_anchors & 1) != 0)
				{
					Label l = DefineLabel();
					Ldthisfld(s_textposF);
					Ldthisfld(s_textbegF);
					Ble(l);
					Ldthis();
					Ldthisfld(s_textendF);
					Stfld(s_textposF);
					Ldc(0);
					Ret();
					MarkLabel(l);
				}
				if ((_anchors & 4) != 0)
				{
					Label l2 = DefineLabel();
					Ldthisfld(s_textposF);
					Ldthisfld(s_textstartF);
					Ble(l2);
					Ldthis();
					Ldthisfld(s_textendF);
					Stfld(s_textposF);
					Ldc(0);
					Ret();
					MarkLabel(l2);
				}
				if ((_anchors & 0x10) != 0)
				{
					Label l3 = DefineLabel();
					Ldthisfld(s_textposF);
					Ldthisfld(s_textendF);
					Ldc(1);
					Sub();
					Bge(l3);
					Ldthis();
					Ldthisfld(s_textendF);
					Ldc(1);
					Sub();
					Stfld(s_textposF);
					MarkLabel(l3);
				}
				if ((_anchors & 0x20) != 0)
				{
					Label l4 = DefineLabel();
					Ldthisfld(s_textposF);
					Ldthisfld(s_textendF);
					Bge(l4);
					Ldthis();
					Ldthisfld(s_textendF);
					Stfld(s_textposF);
					MarkLabel(l4);
				}
			}
			else
			{
				if ((_anchors & 0x20) != 0)
				{
					Label l5 = DefineLabel();
					Ldthisfld(s_textposF);
					Ldthisfld(s_textendF);
					Bge(l5);
					Ldthis();
					Ldthisfld(s_textbegF);
					Stfld(s_textposF);
					Ldc(0);
					Ret();
					MarkLabel(l5);
				}
				if ((_anchors & 0x10) != 0)
				{
					Label l6 = DefineLabel();
					Label l7 = DefineLabel();
					Ldthisfld(s_textposF);
					Ldthisfld(s_textendF);
					Ldc(1);
					Sub();
					Blt(l6);
					Ldthisfld(s_textposF);
					Ldthisfld(s_textendF);
					Beq(l7);
					Ldthisfld(s_textF);
					Ldthisfld(s_textposF);
					Callvirt(s_getcharM);
					Ldc(10);
					Beq(l7);
					MarkLabel(l6);
					Ldthis();
					Ldthisfld(s_textbegF);
					Stfld(s_textposF);
					Ldc(0);
					Ret();
					MarkLabel(l7);
				}
				if ((_anchors & 4) != 0)
				{
					Label l8 = DefineLabel();
					Ldthisfld(s_textposF);
					Ldthisfld(s_textstartF);
					Bge(l8);
					Ldthis();
					Ldthisfld(s_textbegF);
					Stfld(s_textposF);
					Ldc(0);
					Ret();
					MarkLabel(l8);
				}
				if ((_anchors & 1) != 0)
				{
					Label l9 = DefineLabel();
					Ldthisfld(s_textposF);
					Ldthisfld(s_textbegF);
					Ble(l9);
					Ldthis();
					Ldthisfld(s_textbegF);
					Stfld(s_textposF);
					MarkLabel(l9);
				}
			}
			Ldc(1);
			Ret();
		}
		else if (_bmPrefix != null && _bmPrefix.NegativeUnicode == null)
		{
			LocalBuilder tempV = _tempV;
			LocalBuilder tempV2 = _tempV;
			LocalBuilder temp2V = _temp2V;
			Label label = DefineLabel();
			Label l10 = DefineLabel();
			Label l11 = DefineLabel();
			Label l12 = DefineLabel();
			DefineLabel();
			Label l13 = DefineLabel();
			int num;
			int index;
			if (!_code.RightToLeft)
			{
				num = -1;
				index = _bmPrefix.Pattern.Length - 1;
			}
			else
			{
				num = _bmPrefix.Pattern.Length;
				index = 0;
			}
			int i = _bmPrefix.Pattern[index];
			Mvfldloc(s_textF, _textV);
			if (!_code.RightToLeft)
			{
				Ldthisfld(s_textendF);
			}
			else
			{
				Ldthisfld(s_textbegF);
			}
			Stloc(temp2V);
			Ldthisfld(s_textposF);
			if (!_code.RightToLeft)
			{
				Ldc(_bmPrefix.Pattern.Length - 1);
				Add();
			}
			else
			{
				Ldc(_bmPrefix.Pattern.Length);
				Sub();
			}
			Stloc(_textposV);
			Br(l12);
			MarkLabel(label);
			if (!_code.RightToLeft)
			{
				Ldc(_bmPrefix.Pattern.Length);
			}
			else
			{
				Ldc(-_bmPrefix.Pattern.Length);
			}
			MarkLabel(l10);
			Ldloc(_textposV);
			Add();
			Stloc(_textposV);
			MarkLabel(l12);
			Ldloc(_textposV);
			Ldloc(temp2V);
			if (!_code.RightToLeft)
			{
				BgeFar(l11);
			}
			else
			{
				BltFar(l11);
			}
			Rightchar();
			if (_bmPrefix.CaseInsensitive)
			{
				CallToLower();
			}
			Dup();
			Stloc(tempV);
			Ldc(i);
			BeqFar(l13);
			Ldloc(tempV);
			Ldc(_bmPrefix.LowASCII);
			Sub();
			Dup();
			Stloc(tempV);
			Ldc(_bmPrefix.HighASCII - _bmPrefix.LowASCII);
			Bgtun(label);
			Label[] array = new Label[_bmPrefix.HighASCII - _bmPrefix.LowASCII + 1];
			for (int j = _bmPrefix.LowASCII; j <= _bmPrefix.HighASCII; j++)
			{
				if (_bmPrefix.NegativeASCII[j] == num)
				{
					array[j - _bmPrefix.LowASCII] = label;
				}
				else
				{
					array[j - _bmPrefix.LowASCII] = DefineLabel();
				}
			}
			Ldloc(tempV);
			_ilg.Emit(OpCodes.Switch, array);
			for (int j = _bmPrefix.LowASCII; j <= _bmPrefix.HighASCII; j++)
			{
				if (_bmPrefix.NegativeASCII[j] != num)
				{
					MarkLabel(array[j - _bmPrefix.LowASCII]);
					Ldc(_bmPrefix.NegativeASCII[j]);
					BrFar(l10);
				}
			}
			MarkLabel(l13);
			Ldloc(_textposV);
			Stloc(tempV2);
			for (int j = _bmPrefix.Pattern.Length - 2; j >= 0; j--)
			{
				Label l14 = DefineLabel();
				int num2 = (_code.RightToLeft ? (_bmPrefix.Pattern.Length - 1 - j) : j);
				Ldloc(_textV);
				Ldloc(tempV2);
				Ldc(1);
				Sub(_code.RightToLeft);
				Dup();
				Stloc(tempV2);
				Callvirt(s_getcharM);
				if (_bmPrefix.CaseInsensitive)
				{
					CallToLower();
				}
				Ldc(_bmPrefix.Pattern[num2]);
				Beq(l14);
				Ldc(_bmPrefix.Positive[num2]);
				BrFar(l10);
				MarkLabel(l14);
			}
			Ldthis();
			Ldloc(tempV2);
			if (_code.RightToLeft)
			{
				Ldc(1);
				Add();
			}
			Stfld(s_textposF);
			Ldc(1);
			Ret();
			MarkLabel(l11);
			Ldthis();
			if (!_code.RightToLeft)
			{
				Ldthisfld(s_textendF);
			}
			else
			{
				Ldthisfld(s_textbegF);
			}
			Stfld(s_textposF);
			Ldc(0);
			Ret();
		}
		else if (!_fcPrefix.HasValue)
		{
			Ldc(1);
			Ret();
		}
		else
		{
			LocalBuilder temp2V2 = _temp2V;
			_ = _tempV;
			Label l15 = DefineLabel();
			Label l16 = DefineLabel();
			Label l17 = DefineLabel();
			Label l18 = DefineLabel();
			Label l19 = DefineLabel();
			Mvfldloc(s_textposF, _textposV);
			Mvfldloc(s_textF, _textV);
			if (!_code.RightToLeft)
			{
				Ldthisfld(s_textendF);
				Ldloc(_textposV);
			}
			else
			{
				Ldloc(_textposV);
				Ldthisfld(s_textbegF);
			}
			Sub();
			Stloc(temp2V2);
			Ldloc(temp2V2);
			Ldc(0);
			BleFar(l18);
			MarkLabel(l15);
			Ldloc(temp2V2);
			Ldc(1);
			Sub();
			Stloc(temp2V2);
			if (_code.RightToLeft)
			{
				Leftcharnext();
			}
			else
			{
				Rightcharnext();
			}
			if (_fcPrefix.GetValueOrDefault().CaseInsensitive)
			{
				CallToLower();
			}
			if (!RegexCharClass.IsSingleton(_fcPrefix.GetValueOrDefault().Prefix))
			{
				Ldstr(_fcPrefix.GetValueOrDefault().Prefix);
				Call(s_charInSetM);
				BrtrueFar(l16);
			}
			else
			{
				Ldc(RegexCharClass.SingletonChar(_fcPrefix.GetValueOrDefault().Prefix));
				Beq(l16);
			}
			MarkLabel(l19);
			Ldloc(temp2V2);
			Ldc(0);
			if (!RegexCharClass.IsSingleton(_fcPrefix.GetValueOrDefault().Prefix))
			{
				BgtFar(l15);
			}
			else
			{
				Bgt(l15);
			}
			Ldc(0);
			BrFar(l17);
			MarkLabel(l16);
			Ldloc(_textposV);
			Ldc(1);
			Sub(_code.RightToLeft);
			Stloc(_textposV);
			Ldc(1);
			MarkLabel(l17);
			Mvlocfld(_textposV, s_textposF);
			Ret();
			MarkLabel(l18);
			Ldc(0);
			Ret();
		}
	}

	protected void GenerateInitTrackCount()
	{
		Ldthis();
		Ldc(_trackcount);
		Stfld(s_trackcountF);
		Ret();
	}

	private LocalBuilder DeclareInt()
	{
		return _ilg.DeclareLocal(typeof(int));
	}

	private LocalBuilder DeclareIntArray()
	{
		return _ilg.DeclareLocal(typeof(int[]));
	}

	private LocalBuilder DeclareString()
	{
		return _ilg.DeclareLocal(typeof(string));
	}

	protected void GenerateGo()
	{
		_textposV = DeclareInt();
		_textV = DeclareString();
		_trackposV = DeclareInt();
		_trackV = DeclareIntArray();
		_stackposV = DeclareInt();
		_stackV = DeclareIntArray();
		_tempV = DeclareInt();
		_temp2V = DeclareInt();
		_temp3V = DeclareInt();
		_textbegV = DeclareInt();
		_textendV = DeclareInt();
		_textstartV = DeclareInt();
		_labels = null;
		_notes = null;
		_notecount = 0;
		_backtrack = DefineLabel();
		GenerateForwardSection();
		GenerateMiddleSection();
		GenerateBacktrackSection();
	}

	private void GenerateOneCode()
	{
		Ldthis();
		Callvirt(s_checkTimeoutM);
		switch (_regexopcode)
		{
		case 40:
			Mvlocfld(_textposV, s_textposF);
			Ret();
			break;
		case 22:
			Back();
			break;
		case 38:
			Goto(Operand(0));
			break;
		case 37:
			Ldthis();
			Ldc(Operand(0));
			Callvirt(s_ismatchedM);
			BrfalseFar(_backtrack);
			break;
		case 23:
			PushTrack(_textposV);
			Track();
			break;
		case 151:
			PopTrack();
			Stloc(_textposV);
			Goto(Operand(0));
			break;
		case 30:
			ReadyPushStack();
			Ldc(-1);
			DoPush();
			TrackUnique(0);
			break;
		case 31:
			PushStack(_textposV);
			TrackUnique(0);
			break;
		case 158:
		case 159:
			PopDiscardStack();
			Back();
			break;
		case 33:
			ReadyPushTrack();
			PopStack();
			Dup();
			Stloc(_textposV);
			DoPush();
			Track();
			break;
		case 161:
			ReadyPushStack();
			PopTrack();
			DoPush();
			Back();
			break;
		case 32:
			if (Operand(1) != -1)
			{
				Ldthis();
				Ldc(Operand(1));
				Callvirt(s_ismatchedM);
				BrfalseFar(_backtrack);
			}
			PopStack();
			Stloc(_tempV);
			if (Operand(1) != -1)
			{
				Ldthis();
				Ldc(Operand(0));
				Ldc(Operand(1));
				Ldloc(_tempV);
				Ldloc(_textposV);
				Callvirt(s_transferM);
			}
			else
			{
				Ldthis();
				Ldc(Operand(0));
				Ldloc(_tempV);
				Ldloc(_textposV);
				Callvirt(s_captureM);
			}
			PushTrack(_tempV);
			if (Operand(0) != -1 && Operand(1) != -1)
			{
				TrackUnique(4);
			}
			else
			{
				TrackUnique(3);
			}
			break;
		case 160:
			ReadyPushStack();
			PopTrack();
			DoPush();
			Ldthis();
			Callvirt(s_uncaptureM);
			if (Operand(0) != -1 && Operand(1) != -1)
			{
				Ldthis();
				Callvirt(s_uncaptureM);
			}
			Back();
			break;
		case 24:
		{
			LocalBuilder tempV9 = _tempV;
			Label l20 = DefineLabel();
			PopStack();
			Dup();
			Stloc(tempV9);
			PushTrack(tempV9);
			Ldloc(_textposV);
			Beq(l20);
			PushTrack(_textposV);
			PushStack(_textposV);
			Track();
			Goto(Operand(0));
			MarkLabel(l20);
			TrackUnique2(5);
			break;
		}
		case 152:
			PopTrack();
			Stloc(_textposV);
			PopStack();
			Pop();
			TrackUnique2(5);
			Advance();
			break;
		case 280:
			ReadyPushStack();
			PopTrack();
			DoPush();
			Back();
			break;
		case 25:
		{
			LocalBuilder tempV8 = _tempV;
			Label l17 = DefineLabel();
			Label l18 = DefineLabel();
			Label l19 = DefineLabel();
			PopStack();
			Dup();
			Stloc(tempV8);
			Ldloc(tempV8);
			Ldc(-1);
			Beq(l18);
			PushTrack(tempV8);
			Br(l19);
			MarkLabel(l18);
			PushTrack(_textposV);
			MarkLabel(l19);
			Ldloc(_textposV);
			Beq(l17);
			PushTrack(_textposV);
			Track();
			Br(AdvanceLabel());
			MarkLabel(l17);
			ReadyPushStack();
			Ldloc(tempV8);
			DoPush();
			TrackUnique2(6);
			break;
		}
		case 153:
			PopTrack();
			Stloc(_textposV);
			PushStack(_textposV);
			TrackUnique2(6);
			Goto(Operand(0));
			break;
		case 281:
			ReadyReplaceStack(0);
			PopTrack();
			DoReplace();
			Back();
			break;
		case 26:
			ReadyPushStack();
			Ldc(-1);
			DoPush();
			ReadyPushStack();
			Ldc(Operand(0));
			DoPush();
			TrackUnique(1);
			break;
		case 27:
			PushStack(_textposV);
			ReadyPushStack();
			Ldc(Operand(0));
			DoPush();
			TrackUnique(1);
			break;
		case 154:
		case 155:
			PopDiscardStack(2);
			Back();
			break;
		case 28:
		{
			LocalBuilder tempV7 = _tempV;
			LocalBuilder temp2V4 = _temp2V;
			Label l15 = DefineLabel();
			Label l16 = DefineLabel();
			PopStack();
			Stloc(tempV7);
			PopStack();
			Dup();
			Stloc(temp2V4);
			PushTrack(temp2V4);
			Ldloc(_textposV);
			Bne(l15);
			Ldloc(tempV7);
			Ldc(0);
			Bge(l16);
			MarkLabel(l15);
			Ldloc(tempV7);
			Ldc(Operand(1));
			Bge(l16);
			PushStack(_textposV);
			ReadyPushStack();
			Ldloc(tempV7);
			Ldc(1);
			Add();
			DoPush();
			Track();
			Goto(Operand(0));
			MarkLabel(l16);
			PushTrack(tempV7);
			TrackUnique2(7);
			break;
		}
		case 156:
		{
			LocalBuilder tempV4 = _tempV;
			Label l10 = DefineLabel();
			PopStack();
			Ldc(1);
			Sub();
			Dup();
			Stloc(tempV4);
			Ldc(0);
			Blt(l10);
			PopStack();
			Stloc(_textposV);
			PushTrack(tempV4);
			TrackUnique2(7);
			Advance();
			MarkLabel(l10);
			ReadyReplaceStack(0);
			PopTrack();
			DoReplace();
			PushStack(tempV4);
			Back();
			break;
		}
		case 284:
			PopTrack();
			Stloc(_tempV);
			ReadyPushStack();
			PopTrack();
			DoPush();
			PushStack(_tempV);
			Back();
			break;
		case 29:
		{
			LocalBuilder tempV3 = _tempV;
			LocalBuilder temp2V2 = _temp2V;
			Label l9 = DefineLabel();
			DefineLabel();
			_ = ref _labels[NextCodepos()];
			PopStack();
			Stloc(tempV3);
			PopStack();
			Stloc(temp2V2);
			Ldloc(tempV3);
			Ldc(0);
			Bge(l9);
			PushTrack(temp2V2);
			PushStack(_textposV);
			ReadyPushStack();
			Ldloc(tempV3);
			Ldc(1);
			Add();
			DoPush();
			TrackUnique2(8);
			Goto(Operand(0));
			MarkLabel(l9);
			PushTrack(temp2V2);
			PushTrack(tempV3);
			PushTrack(_textposV);
			Track();
			break;
		}
		case 157:
		{
			Label l11 = DefineLabel();
			LocalBuilder tempV5 = _tempV;
			PopTrack();
			Stloc(_textposV);
			PopTrack();
			Dup();
			Stloc(tempV5);
			Ldc(Operand(1));
			Bge(l11);
			Ldloc(_textposV);
			TopTrack();
			Beq(l11);
			PushStack(_textposV);
			ReadyPushStack();
			Ldloc(tempV5);
			Ldc(1);
			Add();
			DoPush();
			TrackUnique2(8);
			Goto(Operand(0));
			MarkLabel(l11);
			ReadyPushStack();
			PopTrack();
			DoPush();
			PushStack(tempV5);
			Back();
			break;
		}
		case 285:
			ReadyReplaceStack(1);
			PopTrack();
			DoReplace();
			ReadyReplaceStack(0);
			TopStack();
			Ldc(1);
			Sub();
			DoReplace();
			Back();
			break;
		case 34:
			ReadyPushStack();
			Ldthisfld(s_trackF);
			Ldlen();
			Ldloc(_trackposV);
			Sub();
			DoPush();
			ReadyPushStack();
			Ldthis();
			Callvirt(s_crawlposM);
			DoPush();
			TrackUnique(1);
			break;
		case 162:
			PopDiscardStack(2);
			Back();
			break;
		case 35:
		{
			Label l3 = DefineLabel();
			Label l4 = DefineLabel();
			PopStack();
			Ldthisfld(s_trackF);
			Ldlen();
			PopStack();
			Sub();
			Stloc(_trackposV);
			Dup();
			Ldthis();
			Callvirt(s_crawlposM);
			Beq(l4);
			MarkLabel(l3);
			Ldthis();
			Callvirt(s_uncaptureM);
			Dup();
			Ldthis();
			Callvirt(s_crawlposM);
			Bne(l3);
			MarkLabel(l4);
			Pop();
			Back();
			break;
		}
		case 36:
			PopStack();
			Stloc(_tempV);
			Ldthisfld(s_trackF);
			Ldlen();
			PopStack();
			Sub();
			Stloc(_trackposV);
			PushTrack(_tempV);
			TrackUnique(9);
			break;
		case 164:
		{
			Label l5 = DefineLabel();
			Label l6 = DefineLabel();
			PopTrack();
			Dup();
			Ldthis();
			Callvirt(s_crawlposM);
			Beq(l6);
			MarkLabel(l5);
			Ldthis();
			Callvirt(s_uncaptureM);
			Dup();
			Ldthis();
			Callvirt(s_crawlposM);
			Bne(l5);
			MarkLabel(l6);
			Pop();
			Back();
			break;
		}
		case 14:
		{
			Label l7 = _labels[NextCodepos()];
			Ldloc(_textposV);
			Ldloc(_textbegV);
			Ble(l7);
			Leftchar();
			Ldc(10);
			BneFar(_backtrack);
			break;
		}
		case 15:
		{
			Label l = _labels[NextCodepos()];
			Ldloc(_textposV);
			Ldloc(_textendV);
			Bge(l);
			Rightchar();
			Ldc(10);
			BneFar(_backtrack);
			break;
		}
		case 16:
		case 17:
			Ldthis();
			Ldloc(_textposV);
			Ldloc(_textbegV);
			Ldloc(_textendV);
			Callvirt(s_isboundaryM);
			if (Code() == 16)
			{
				BrfalseFar(_backtrack);
			}
			else
			{
				BrtrueFar(_backtrack);
			}
			break;
		case 41:
		case 42:
			Ldthis();
			Ldloc(_textposV);
			Ldloc(_textbegV);
			Ldloc(_textendV);
			Callvirt(s_isECMABoundaryM);
			if (Code() == 41)
			{
				BrfalseFar(_backtrack);
			}
			else
			{
				BrtrueFar(_backtrack);
			}
			break;
		case 18:
			Ldloc(_textposV);
			Ldloc(_textbegV);
			BgtFar(_backtrack);
			break;
		case 19:
			Ldloc(_textposV);
			Ldthisfld(s_textstartF);
			BneFar(_backtrack);
			break;
		case 20:
			Ldloc(_textposV);
			Ldloc(_textendV);
			Ldc(1);
			Sub();
			BltFar(_backtrack);
			Ldloc(_textposV);
			Ldloc(_textendV);
			Bge(_labels[NextCodepos()]);
			Rightchar();
			Ldc(10);
			BneFar(_backtrack);
			break;
		case 21:
			Ldloc(_textposV);
			Ldloc(_textendV);
			BltFar(_backtrack);
			break;
		case 9:
		case 10:
		case 11:
		case 73:
		case 74:
		case 75:
		case 521:
		case 522:
		case 523:
		case 585:
		case 586:
		case 587:
			Ldloc(_textposV);
			if (!IsRtl())
			{
				Ldloc(_textendV);
				BgeFar(_backtrack);
				Rightcharnext();
			}
			else
			{
				Ldloc(_textbegV);
				BleFar(_backtrack);
				Leftcharnext();
			}
			if (IsCi())
			{
				CallToLower();
			}
			if (Code() == 11)
			{
				Ldstr(_strings[Operand(0)]);
				Call(s_charInSetM);
				BrfalseFar(_backtrack);
				break;
			}
			Ldc(Operand(0));
			if (Code() == 9)
			{
				BneFar(_backtrack);
			}
			else
			{
				BeqFar(_backtrack);
			}
			break;
		case 12:
		case 524:
		{
			string text2 = _strings[Operand(0)];
			Ldc(text2.Length);
			Ldloc(_textendV);
			Ldloc(_textposV);
			Sub();
			BgtFar(_backtrack);
			for (int i = 0; i < text2.Length; i++)
			{
				Ldloc(_textV);
				Ldloc(_textposV);
				if (i != 0)
				{
					Ldc(i);
					Add();
				}
				Callvirt(s_getcharM);
				if (IsCi())
				{
					CallToLower();
				}
				Ldc(text2[i]);
				BneFar(_backtrack);
			}
			Ldloc(_textposV);
			Ldc(text2.Length);
			Add();
			Stloc(_textposV);
			break;
		}
		case 76:
		case 588:
		{
			string text = _strings[Operand(0)];
			Ldc(text.Length);
			Ldloc(_textposV);
			Ldloc(_textbegV);
			Sub();
			BgtFar(_backtrack);
			int num3 = text.Length;
			while (num3 > 0)
			{
				num3--;
				Ldloc(_textV);
				Ldloc(_textposV);
				Ldc(text.Length - num3);
				Sub();
				Callvirt(s_getcharM);
				if (IsCi())
				{
					CallToLower();
				}
				Ldc(text[num3]);
				BneFar(_backtrack);
			}
			Ldloc(_textposV);
			Ldc(text.Length);
			Sub();
			Stloc(_textposV);
			break;
		}
		case 13:
		case 77:
		case 525:
		case 589:
		{
			LocalBuilder tempV = _tempV;
			LocalBuilder temp2V = _temp2V;
			Label l2 = DefineLabel();
			Ldthis();
			Ldc(Operand(0));
			Callvirt(s_ismatchedM);
			if ((_options & RegexOptions.ECMAScript) != RegexOptions.None)
			{
				Brfalse(AdvanceLabel());
			}
			else
			{
				BrfalseFar(_backtrack);
			}
			Ldthis();
			Ldc(Operand(0));
			Callvirt(s_matchlengthM);
			Dup();
			Stloc(tempV);
			if (!IsRtl())
			{
				Ldloc(_textendV);
				Ldloc(_textposV);
			}
			else
			{
				Ldloc(_textposV);
				Ldloc(_textbegV);
			}
			Sub();
			BgtFar(_backtrack);
			Ldthis();
			Ldc(Operand(0));
			Callvirt(s_matchindexM);
			if (!IsRtl())
			{
				Ldloc(tempV);
				Add(IsRtl());
			}
			Stloc(temp2V);
			Ldloc(_textposV);
			Ldloc(tempV);
			Add(IsRtl());
			Stloc(_textposV);
			MarkLabel(l2);
			Ldloc(tempV);
			Ldc(0);
			Ble(AdvanceLabel());
			Ldloc(_textV);
			Ldloc(temp2V);
			Ldloc(tempV);
			if (IsRtl())
			{
				Ldc(1);
				Sub();
				Dup();
				Stloc(tempV);
			}
			Sub(IsRtl());
			Callvirt(s_getcharM);
			if (IsCi())
			{
				CallToLower();
			}
			Ldloc(_textV);
			Ldloc(_textposV);
			Ldloc(tempV);
			if (!IsRtl())
			{
				Dup();
				Ldc(1);
				Sub();
				Stloc(tempV);
			}
			Sub(IsRtl());
			Callvirt(s_getcharM);
			if (IsCi())
			{
				CallToLower();
			}
			Beq(l2);
			Back();
			break;
		}
		case 0:
		case 1:
		case 2:
		case 64:
		case 65:
		case 66:
		case 512:
		case 513:
		case 514:
		case 576:
		case 577:
		case 578:
		{
			LocalBuilder tempV10 = _tempV;
			Label l21 = DefineLabel();
			int num4 = Operand(1);
			if (num4 == 0)
			{
				break;
			}
			Ldc(num4);
			if (!IsRtl())
			{
				Ldloc(_textendV);
				Ldloc(_textposV);
			}
			else
			{
				Ldloc(_textposV);
				Ldloc(_textbegV);
			}
			Sub();
			BgtFar(_backtrack);
			Ldloc(_textposV);
			Ldc(num4);
			Add(IsRtl());
			Stloc(_textposV);
			Ldc(num4);
			Stloc(tempV10);
			MarkLabel(l21);
			Ldloc(_textV);
			Ldloc(_textposV);
			Ldloc(tempV10);
			if (IsRtl())
			{
				Ldc(1);
				Sub();
				Dup();
				Stloc(tempV10);
				Add();
			}
			else
			{
				Dup();
				Ldc(1);
				Sub();
				Stloc(tempV10);
				Sub();
			}
			Callvirt(s_getcharM);
			if (IsCi())
			{
				CallToLower();
			}
			if (Code() == 2)
			{
				Ldstr(_strings[Operand(0)]);
				Call(s_charInSetM);
				BrfalseFar(_backtrack);
			}
			else
			{
				Ldc(Operand(0));
				if (Code() == 0)
				{
					BneFar(_backtrack);
				}
				else
				{
					BeqFar(_backtrack);
				}
			}
			Ldloc(tempV10);
			Ldc(0);
			if (Code() == 2)
			{
				BgtFar(l21);
			}
			else
			{
				Bgt(l21);
			}
			break;
		}
		case 3:
		case 4:
		case 5:
		case 67:
		case 68:
		case 69:
		case 515:
		case 516:
		case 517:
		case 579:
		case 580:
		case 581:
		{
			LocalBuilder tempV6 = _tempV;
			LocalBuilder temp2V3 = _temp2V;
			Label l12 = DefineLabel();
			Label l13 = DefineLabel();
			int num2 = Operand(1);
			if (num2 == 0)
			{
				break;
			}
			if (!IsRtl())
			{
				Ldloc(_textendV);
				Ldloc(_textposV);
			}
			else
			{
				Ldloc(_textposV);
				Ldloc(_textbegV);
			}
			Sub();
			if (num2 != int.MaxValue)
			{
				Label l14 = DefineLabel();
				Dup();
				Ldc(num2);
				Blt(l14);
				Pop();
				Ldc(num2);
				MarkLabel(l14);
			}
			Dup();
			Stloc(temp2V3);
			Ldc(1);
			Add();
			Stloc(tempV6);
			MarkLabel(l12);
			Ldloc(tempV6);
			Ldc(1);
			Sub();
			Dup();
			Stloc(tempV6);
			Ldc(0);
			if (Code() == 5)
			{
				BleFar(l13);
			}
			else
			{
				Ble(l13);
			}
			if (IsRtl())
			{
				Leftcharnext();
			}
			else
			{
				Rightcharnext();
			}
			if (IsCi())
			{
				CallToLower();
			}
			if (Code() == 5)
			{
				Ldstr(_strings[Operand(0)]);
				Call(s_charInSetM);
				BrtrueFar(l12);
			}
			else
			{
				Ldc(Operand(0));
				if (Code() == 3)
				{
					Beq(l12);
				}
				else
				{
					Bne(l12);
				}
			}
			Ldloc(_textposV);
			Ldc(1);
			Sub(IsRtl());
			Stloc(_textposV);
			MarkLabel(l13);
			Ldloc(temp2V3);
			Ldloc(tempV6);
			Ble(AdvanceLabel());
			ReadyPushTrack();
			Ldloc(temp2V3);
			Ldloc(tempV6);
			Sub();
			Ldc(1);
			Sub();
			DoPush();
			ReadyPushTrack();
			Ldloc(_textposV);
			Ldc(1);
			Sub(IsRtl());
			DoPush();
			Track();
			break;
		}
		case 131:
		case 132:
		case 133:
		case 195:
		case 196:
		case 197:
		case 643:
		case 644:
		case 645:
		case 707:
		case 708:
		case 709:
			PopTrack();
			Stloc(_textposV);
			PopTrack();
			Stloc(_tempV);
			Ldloc(_tempV);
			Ldc(0);
			BleFar(AdvanceLabel());
			ReadyPushTrack();
			Ldloc(_tempV);
			Ldc(1);
			Sub();
			DoPush();
			ReadyPushTrack();
			Ldloc(_textposV);
			Ldc(1);
			Sub(IsRtl());
			DoPush();
			Trackagain();
			Advance();
			break;
		case 6:
		case 7:
		case 8:
		case 70:
		case 71:
		case 72:
		case 518:
		case 519:
		case 520:
		case 582:
		case 583:
		case 584:
		{
			LocalBuilder tempV2 = _tempV;
			int num = Operand(1);
			if (num != 0)
			{
				if (!IsRtl())
				{
					Ldloc(_textendV);
					Ldloc(_textposV);
				}
				else
				{
					Ldloc(_textposV);
					Ldloc(_textbegV);
				}
				Sub();
				if (num != int.MaxValue)
				{
					Label l8 = DefineLabel();
					Dup();
					Ldc(num);
					Blt(l8);
					Pop();
					Ldc(num);
					MarkLabel(l8);
				}
				Dup();
				Stloc(tempV2);
				Ldc(0);
				Ble(AdvanceLabel());
				ReadyPushTrack();
				Ldloc(tempV2);
				Ldc(1);
				Sub();
				DoPush();
				PushTrack(_textposV);
				Track();
			}
			break;
		}
		case 134:
		case 135:
		case 136:
		case 198:
		case 199:
		case 200:
		case 646:
		case 647:
		case 648:
		case 710:
		case 711:
		case 712:
			PopTrack();
			Stloc(_textposV);
			PopTrack();
			Stloc(_temp2V);
			if (!IsRtl())
			{
				Rightcharnext();
			}
			else
			{
				Leftcharnext();
			}
			if (IsCi())
			{
				CallToLower();
			}
			if (Code() == 8)
			{
				Ldstr(_strings[Operand(0)]);
				Call(s_charInSetM);
				BrfalseFar(_backtrack);
			}
			else
			{
				Ldc(Operand(0));
				if (Code() == 6)
				{
					BneFar(_backtrack);
				}
				else
				{
					BeqFar(_backtrack);
				}
			}
			Ldloc(_temp2V);
			Ldc(0);
			BleFar(AdvanceLabel());
			ReadyPushTrack();
			Ldloc(_temp2V);
			Ldc(1);
			Sub();
			DoPush();
			PushTrack(_textposV);
			Trackagain();
			Advance();
			break;
		default:
			throw new NotImplementedException("Unimplemented state.");
		}
	}
}
