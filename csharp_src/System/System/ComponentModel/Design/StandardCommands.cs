namespace System.ComponentModel.Design;

public class StandardCommands
{
	private static class VSStandardCommands
	{
		internal const int cmdidAlignBottom = 1;

		internal const int cmdidAlignHorizontalCenters = 2;

		internal const int cmdidAlignLeft = 3;

		internal const int cmdidAlignRight = 4;

		internal const int cmdidAlignToGrid = 5;

		internal const int cmdidAlignTop = 6;

		internal const int cmdidAlignVerticalCenters = 7;

		internal const int cmdidArrangeBottom = 8;

		internal const int cmdidArrangeRight = 9;

		internal const int cmdidBringForward = 10;

		internal const int cmdidBringToFront = 11;

		internal const int cmdidCenterHorizontally = 12;

		internal const int cmdidCenterVertically = 13;

		internal const int cmdidCode = 14;

		internal const int cmdidCopy = 15;

		internal const int cmdidCut = 16;

		internal const int cmdidDelete = 17;

		internal const int cmdidFontName = 18;

		internal const int cmdidFontSize = 19;

		internal const int cmdidGroup = 20;

		internal const int cmdidHorizSpaceConcatenate = 21;

		internal const int cmdidHorizSpaceDecrease = 22;

		internal const int cmdidHorizSpaceIncrease = 23;

		internal const int cmdidHorizSpaceMakeEqual = 24;

		internal const int cmdidLockControls = 369;

		internal const int cmdidInsertObject = 25;

		internal const int cmdidPaste = 26;

		internal const int cmdidPrint = 27;

		internal const int cmdidProperties = 28;

		internal const int cmdidRedo = 29;

		internal const int cmdidMultiLevelRedo = 30;

		internal const int cmdidSelectAll = 31;

		internal const int cmdidSendBackward = 32;

		internal const int cmdidSendToBack = 33;

		internal const int cmdidShowTable = 34;

		internal const int cmdidSizeToControl = 35;

		internal const int cmdidSizeToControlHeight = 36;

		internal const int cmdidSizeToControlWidth = 37;

		internal const int cmdidSizeToFit = 38;

		internal const int cmdidSizeToGrid = 39;

		internal const int cmdidSnapToGrid = 40;

		internal const int cmdidTabOrder = 41;

		internal const int cmdidToolbox = 42;

		internal const int cmdidUndo = 43;

		internal const int cmdidMultiLevelUndo = 44;

		internal const int cmdidUngroup = 45;

		internal const int cmdidVertSpaceConcatenate = 46;

		internal const int cmdidVertSpaceDecrease = 47;

		internal const int cmdidVertSpaceIncrease = 48;

		internal const int cmdidVertSpaceMakeEqual = 49;

		internal const int cmdidZoomPercent = 50;

		internal const int cmdidBackColor = 51;

		internal const int cmdidBold = 52;

		internal const int cmdidBorderColor = 53;

		internal const int cmdidBorderDashDot = 54;

		internal const int cmdidBorderDashDotDot = 55;

		internal const int cmdidBorderDashes = 56;

		internal const int cmdidBorderDots = 57;

		internal const int cmdidBorderShortDashes = 58;

		internal const int cmdidBorderSolid = 59;

		internal const int cmdidBorderSparseDots = 60;

		internal const int cmdidBorderWidth1 = 61;

		internal const int cmdidBorderWidth2 = 62;

		internal const int cmdidBorderWidth3 = 63;

		internal const int cmdidBorderWidth4 = 64;

		internal const int cmdidBorderWidth5 = 65;

		internal const int cmdidBorderWidth6 = 66;

		internal const int cmdidBorderWidthHairline = 67;

		internal const int cmdidFlat = 68;

		internal const int cmdidForeColor = 69;

		internal const int cmdidItalic = 70;

		internal const int cmdidJustifyCenter = 71;

		internal const int cmdidJustifyGeneral = 72;

		internal const int cmdidJustifyLeft = 73;

		internal const int cmdidJustifyRight = 74;

		internal const int cmdidRaised = 75;

		internal const int cmdidSunken = 76;

		internal const int cmdidUnderline = 77;

		internal const int cmdidChiseled = 78;

		internal const int cmdidEtched = 79;

		internal const int cmdidShadowed = 80;

		internal const int cmdidCompDebug1 = 81;

		internal const int cmdidCompDebug2 = 82;

		internal const int cmdidCompDebug3 = 83;

		internal const int cmdidCompDebug4 = 84;

		internal const int cmdidCompDebug5 = 85;

		internal const int cmdidCompDebug6 = 86;

		internal const int cmdidCompDebug7 = 87;

		internal const int cmdidCompDebug8 = 88;

		internal const int cmdidCompDebug9 = 89;

		internal const int cmdidCompDebug10 = 90;

		internal const int cmdidCompDebug11 = 91;

		internal const int cmdidCompDebug12 = 92;

		internal const int cmdidCompDebug13 = 93;

		internal const int cmdidCompDebug14 = 94;

		internal const int cmdidCompDebug15 = 95;

		internal const int cmdidExistingSchemaEdit = 96;

		internal const int cmdidFind = 97;

		internal const int cmdidGetZoom = 98;

		internal const int cmdidQueryOpenDesign = 99;

		internal const int cmdidQueryOpenNew = 100;

		internal const int cmdidSingleTableDesign = 101;

		internal const int cmdidSingleTableNew = 102;

		internal const int cmdidShowGrid = 103;

		internal const int cmdidNewTable = 104;

		internal const int cmdidCollapsedView = 105;

		internal const int cmdidFieldView = 106;

		internal const int cmdidVerifySQL = 107;

		internal const int cmdidHideTable = 108;

		internal const int cmdidPrimaryKey = 109;

		internal const int cmdidSave = 110;

		internal const int cmdidSaveAs = 111;

		internal const int cmdidSortAscending = 112;

		internal const int cmdidSortDescending = 113;

		internal const int cmdidAppendQuery = 114;

		internal const int cmdidCrosstabQuery = 115;

		internal const int cmdidDeleteQuery = 116;

		internal const int cmdidMakeTableQuery = 117;

		internal const int cmdidSelectQuery = 118;

		internal const int cmdidUpdateQuery = 119;

		internal const int cmdidParameters = 120;

		internal const int cmdidTotals = 121;

		internal const int cmdidViewCollapsed = 122;

		internal const int cmdidViewFieldList = 123;

		internal const int cmdidViewKeys = 124;

		internal const int cmdidViewGrid = 125;

		internal const int cmdidInnerJoin = 126;

		internal const int cmdidRightOuterJoin = 127;

		internal const int cmdidLeftOuterJoin = 128;

		internal const int cmdidFullOuterJoin = 129;

		internal const int cmdidUnionJoin = 130;

		internal const int cmdidShowSQLPane = 131;

		internal const int cmdidShowGraphicalPane = 132;

		internal const int cmdidShowDataPane = 133;

		internal const int cmdidShowQBEPane = 134;

		internal const int cmdidSelectAllFields = 135;

		internal const int cmdidOLEObjectMenuButton = 136;

		internal const int cmdidObjectVerbList0 = 137;

		internal const int cmdidObjectVerbList1 = 138;

		internal const int cmdidObjectVerbList2 = 139;

		internal const int cmdidObjectVerbList3 = 140;

		internal const int cmdidObjectVerbList4 = 141;

		internal const int cmdidObjectVerbList5 = 142;

		internal const int cmdidObjectVerbList6 = 143;

		internal const int cmdidObjectVerbList7 = 144;

		internal const int cmdidObjectVerbList8 = 145;

		internal const int cmdidObjectVerbList9 = 146;

		internal const int cmdidConvertObject = 147;

		internal const int cmdidCustomControl = 148;

		internal const int cmdidCustomizeItem = 149;

		internal const int cmdidRename = 150;

		internal const int cmdidImport = 151;

		internal const int cmdidNewPage = 152;

		internal const int cmdidMove = 153;

		internal const int cmdidCancel = 154;

		internal const int cmdidFont = 155;

		internal const int cmdidExpandLinks = 156;

		internal const int cmdidExpandImages = 157;

		internal const int cmdidExpandPages = 158;

		internal const int cmdidRefocusDiagram = 159;

		internal const int cmdidTransitiveClosure = 160;

		internal const int cmdidCenterDiagram = 161;

		internal const int cmdidZoomIn = 162;

		internal const int cmdidZoomOut = 163;

		internal const int cmdidRemoveFilter = 164;

		internal const int cmdidHidePane = 165;

		internal const int cmdidDeleteTable = 166;

		internal const int cmdidDeleteRelationship = 167;

		internal const int cmdidRemove = 168;

		internal const int cmdidJoinLeftAll = 169;

		internal const int cmdidJoinRightAll = 170;

		internal const int cmdidAddToOutput = 171;

		internal const int cmdidOtherQuery = 172;

		internal const int cmdidGenerateChangeScript = 173;

		internal const int cmdidSaveSelection = 174;

		internal const int cmdidAutojoinCurrent = 175;

		internal const int cmdidAutojoinAlways = 176;

		internal const int cmdidEditPage = 177;

		internal const int cmdidViewLinks = 178;

		internal const int cmdidStop = 179;

		internal const int cmdidPause = 180;

		internal const int cmdidResume = 181;

		internal const int cmdidFilterDiagram = 182;

		internal const int cmdidShowAllObjects = 183;

		internal const int cmdidShowApplications = 184;

		internal const int cmdidShowOtherObjects = 185;

		internal const int cmdidShowPrimRelationships = 186;

		internal const int cmdidExpand = 187;

		internal const int cmdidCollapse = 188;

		internal const int cmdidRefresh = 189;

		internal const int cmdidLayout = 190;

		internal const int cmdidShowResources = 191;

		internal const int cmdidInsertHTMLWizard = 192;

		internal const int cmdidShowDownloads = 193;

		internal const int cmdidShowExternals = 194;

		internal const int cmdidShowInBoundLinks = 195;

		internal const int cmdidShowOutBoundLinks = 196;

		internal const int cmdidShowInAndOutBoundLinks = 197;

		internal const int cmdidPreview = 198;

		internal const int cmdidOpen = 261;

		internal const int cmdidOpenWith = 199;

		internal const int cmdidShowPages = 200;

		internal const int cmdidRunQuery = 201;

		internal const int cmdidClearQuery = 202;

		internal const int cmdidRecordFirst = 203;

		internal const int cmdidRecordLast = 204;

		internal const int cmdidRecordNext = 205;

		internal const int cmdidRecordPrevious = 206;

		internal const int cmdidRecordGoto = 207;

		internal const int cmdidRecordNew = 208;

		internal const int cmdidInsertNewMenu = 209;

		internal const int cmdidInsertSeparator = 210;

		internal const int cmdidEditMenuNames = 211;

		internal const int cmdidDebugExplorer = 212;

		internal const int cmdidDebugProcesses = 213;

		internal const int cmdidViewThreadsWindow = 214;

		internal const int cmdidWindowUIList = 215;

		internal const int cmdidNewProject = 216;

		internal const int cmdidOpenProject = 217;

		internal const int cmdidOpenSolution = 218;

		internal const int cmdidCloseSolution = 219;

		internal const int cmdidFileNew = 221;

		internal const int cmdidFileOpen = 222;

		internal const int cmdidFileClose = 223;

		internal const int cmdidSaveSolution = 224;

		internal const int cmdidSaveSolutionAs = 225;

		internal const int cmdidSaveProjectItemAs = 226;

		internal const int cmdidPageSetup = 227;

		internal const int cmdidPrintPreview = 228;

		internal const int cmdidExit = 229;

		internal const int cmdidReplace = 230;

		internal const int cmdidGoto = 231;

		internal const int cmdidPropertyPages = 232;

		internal const int cmdidFullScreen = 233;

		internal const int cmdidProjectExplorer = 234;

		internal const int cmdidPropertiesWindow = 235;

		internal const int cmdidTaskListWindow = 236;

		internal const int cmdidOutputWindow = 237;

		internal const int cmdidObjectBrowser = 238;

		internal const int cmdidDocOutlineWindow = 239;

		internal const int cmdidImmediateWindow = 240;

		internal const int cmdidWatchWindow = 241;

		internal const int cmdidLocalsWindow = 242;

		internal const int cmdidCallStack = 243;

		internal const int cmdidAutosWindow = 747;

		internal const int cmdidThisWindow = 748;

		internal const int cmdidAddNewItem = 220;

		internal const int cmdidAddExistingItem = 244;

		internal const int cmdidNewFolder = 245;

		internal const int cmdidSetStartupProject = 246;

		internal const int cmdidProjectSettings = 247;

		internal const int cmdidProjectReferences = 367;

		internal const int cmdidStepInto = 248;

		internal const int cmdidStepOver = 249;

		internal const int cmdidStepOut = 250;

		internal const int cmdidRunToCursor = 251;

		internal const int cmdidAddWatch = 252;

		internal const int cmdidEditWatch = 253;

		internal const int cmdidQuickWatch = 254;

		internal const int cmdidToggleBreakpoint = 255;

		internal const int cmdidClearBreakpoints = 256;

		internal const int cmdidShowBreakpoints = 257;

		internal const int cmdidSetNextStatement = 258;

		internal const int cmdidShowNextStatement = 259;

		internal const int cmdidEditBreakpoint = 260;

		internal const int cmdidDetachDebugger = 262;

		internal const int cmdidCustomizeKeyboard = 263;

		internal const int cmdidToolsOptions = 264;

		internal const int cmdidNewWindow = 265;

		internal const int cmdidSplit = 266;

		internal const int cmdidCascade = 267;

		internal const int cmdidTileHorz = 268;

		internal const int cmdidTileVert = 269;

		internal const int cmdidTechSupport = 270;

		internal const int cmdidAbout = 271;

		internal const int cmdidDebugOptions = 272;

		internal const int cmdidDeleteWatch = 274;

		internal const int cmdidCollapseWatch = 275;

		internal const int cmdidPbrsToggleStatus = 282;

		internal const int cmdidPropbrsHide = 283;

		internal const int cmdidDockingView = 284;

		internal const int cmdidHideActivePane = 285;

		internal const int cmdidPaneNextTab = 286;

		internal const int cmdidPanePrevTab = 287;

		internal const int cmdidPaneCloseToolWindow = 288;

		internal const int cmdidPaneActivateDocWindow = 289;

		internal const int cmdidDockingViewFloater = 291;

		internal const int cmdidAutoHideWindow = 292;

		internal const int cmdidMoveToDropdownBar = 293;

		internal const int cmdidFindCmd = 294;

		internal const int cmdidStart = 295;

		internal const int cmdidRestart = 296;

		internal const int cmdidAddinManager = 297;

		internal const int cmdidMultiLevelUndoList = 298;

		internal const int cmdidMultiLevelRedoList = 299;

		internal const int cmdidToolboxAddTab = 300;

		internal const int cmdidToolboxDeleteTab = 301;

		internal const int cmdidToolboxRenameTab = 302;

		internal const int cmdidToolboxTabMoveUp = 303;

		internal const int cmdidToolboxTabMoveDown = 304;

		internal const int cmdidToolboxRenameItem = 305;

		internal const int cmdidToolboxListView = 306;

		internal const int cmdidWindowUIGetList = 308;

		internal const int cmdidInsertValuesQuery = 309;

		internal const int cmdidShowProperties = 310;

		internal const int cmdidThreadSuspend = 311;

		internal const int cmdidThreadResume = 312;

		internal const int cmdidThreadSetFocus = 313;

		internal const int cmdidDisplayRadix = 314;

		internal const int cmdidOpenProjectItem = 315;

		internal const int cmdidPaneNextPane = 316;

		internal const int cmdidPanePrevPane = 317;

		internal const int cmdidClearPane = 318;

		internal const int cmdidGotoErrorTag = 319;

		internal const int cmdidTaskListSortByCategory = 320;

		internal const int cmdidTaskListSortByFileLine = 321;

		internal const int cmdidTaskListSortByPriority = 322;

		internal const int cmdidTaskListSortByDefaultSort = 323;

		internal const int cmdidTaskListFilterByNothing = 325;

		internal const int cmdidTaskListFilterByCategoryCodeSense = 326;

		internal const int cmdidTaskListFilterByCategoryCompiler = 327;

		internal const int cmdidTaskListFilterByCategoryComment = 328;

		internal const int cmdidToolboxAddItem = 329;

		internal const int cmdidToolboxReset = 330;

		internal const int cmdidSaveProjectItem = 331;

		internal const int cmdidViewForm = 332;

		internal const int cmdidViewCode = 333;

		internal const int cmdidPreviewInBrowser = 334;

		internal const int cmdidBrowseWith = 336;

		internal const int cmdidSearchSetCombo = 307;

		internal const int cmdidSearchCombo = 337;

		internal const int cmdidEditLabel = 338;

		internal const int cmdidExceptions = 339;

		internal const int cmdidToggleSelMode = 341;

		internal const int cmdidToggleInsMode = 342;

		internal const int cmdidLoadUnloadedProject = 343;

		internal const int cmdidUnloadLoadedProject = 344;

		internal const int cmdidElasticColumn = 345;

		internal const int cmdidHideColumn = 346;

		internal const int cmdidTaskListPreviousView = 347;

		internal const int cmdidZoomDialog = 348;

		internal const int cmdidFindNew = 349;

		internal const int cmdidFindMatchCase = 350;

		internal const int cmdidFindWholeWord = 351;

		internal const int cmdidFindSimplePattern = 276;

		internal const int cmdidFindRegularExpression = 352;

		internal const int cmdidFindBackwards = 353;

		internal const int cmdidFindInSelection = 354;

		internal const int cmdidFindStop = 355;

		internal const int cmdidFindHelp = 356;

		internal const int cmdidFindInFiles = 277;

		internal const int cmdidReplaceInFiles = 278;

		internal const int cmdidNextLocation = 279;

		internal const int cmdidPreviousLocation = 280;

		internal const int cmdidTaskListNextError = 357;

		internal const int cmdidTaskListPrevError = 358;

		internal const int cmdidTaskListFilterByCategoryUser = 359;

		internal const int cmdidTaskListFilterByCategoryShortcut = 360;

		internal const int cmdidTaskListFilterByCategoryHTML = 361;

		internal const int cmdidTaskListFilterByCurrentFile = 362;

		internal const int cmdidTaskListFilterByChecked = 363;

		internal const int cmdidTaskListFilterByUnchecked = 364;

		internal const int cmdidTaskListSortByDescription = 365;

		internal const int cmdidTaskListSortByChecked = 366;

		internal const int cmdidStartNoDebug = 368;

		internal const int cmdidFindNext = 370;

		internal const int cmdidFindPrev = 371;

		internal const int cmdidFindSelectedNext = 372;

		internal const int cmdidFindSelectedPrev = 373;

		internal const int cmdidSearchGetList = 374;

		internal const int cmdidInsertBreakpoint = 375;

		internal const int cmdidEnableBreakpoint = 376;

		internal const int cmdidF1Help = 377;

		internal const int cmdidPropSheetOrProperties = 397;

		internal const int cmdidTshellStep = 398;

		internal const int cmdidTshellRun = 399;

		internal const int cmdidMarkerCmd0 = 400;

		internal const int cmdidMarkerCmd1 = 401;

		internal const int cmdidMarkerCmd2 = 402;

		internal const int cmdidMarkerCmd3 = 403;

		internal const int cmdidMarkerCmd4 = 404;

		internal const int cmdidMarkerCmd5 = 405;

		internal const int cmdidMarkerCmd6 = 406;

		internal const int cmdidMarkerCmd7 = 407;

		internal const int cmdidMarkerCmd8 = 408;

		internal const int cmdidMarkerCmd9 = 409;

		internal const int cmdidMarkerLast = 409;

		internal const int cmdidMarkerEnd = 410;

		internal const int cmdidReloadProject = 412;

		internal const int cmdidUnloadProject = 413;

		internal const int cmdidDetachAttachOutline = 420;

		internal const int cmdidShowHideOutline = 421;

		internal const int cmdidSyncOutline = 422;

		internal const int cmdidRunToCallstCursor = 423;

		internal const int cmdidNoCmdsAvailable = 424;

		internal const int cmdidContextWindow = 427;

		internal const int cmdidAlias = 428;

		internal const int cmdidGotoCommandLine = 429;

		internal const int cmdidEvaluateExpression = 430;

		internal const int cmdidImmediateMode = 431;

		internal const int cmdidEvaluateStatement = 432;

		internal const int cmdidFindResultWindow1 = 433;

		internal const int cmdidFindResultWindow2 = 434;

		internal const int cmdidWindow1 = 570;

		internal const int cmdidWindow2 = 571;

		internal const int cmdidWindow3 = 572;

		internal const int cmdidWindow4 = 573;

		internal const int cmdidWindow5 = 574;

		internal const int cmdidWindow6 = 575;

		internal const int cmdidWindow7 = 576;

		internal const int cmdidWindow8 = 577;

		internal const int cmdidWindow9 = 578;

		internal const int cmdidWindow10 = 579;

		internal const int cmdidWindow11 = 580;

		internal const int cmdidWindow12 = 581;

		internal const int cmdidWindow13 = 582;

		internal const int cmdidWindow14 = 583;

		internal const int cmdidWindow15 = 584;

		internal const int cmdidWindow16 = 585;

		internal const int cmdidWindow17 = 586;

		internal const int cmdidWindow18 = 587;

		internal const int cmdidWindow19 = 588;

		internal const int cmdidWindow20 = 589;

		internal const int cmdidWindow21 = 590;

		internal const int cmdidWindow22 = 591;

		internal const int cmdidWindow23 = 592;

		internal const int cmdidWindow24 = 593;

		internal const int cmdidWindow25 = 594;

		internal const int cmdidMoreWindows = 595;

		internal const int cmdidTaskListTaskHelp = 598;

		internal const int cmdidClassView = 599;

		internal const int cmdidMRUProj1 = 600;

		internal const int cmdidMRUProj2 = 601;

		internal const int cmdidMRUProj3 = 602;

		internal const int cmdidMRUProj4 = 603;

		internal const int cmdidMRUProj5 = 604;

		internal const int cmdidMRUProj6 = 605;

		internal const int cmdidMRUProj7 = 606;

		internal const int cmdidMRUProj8 = 607;

		internal const int cmdidMRUProj9 = 608;

		internal const int cmdidMRUProj10 = 609;

		internal const int cmdidMRUProj11 = 610;

		internal const int cmdidMRUProj12 = 611;

		internal const int cmdidMRUProj13 = 612;

		internal const int cmdidMRUProj14 = 613;

		internal const int cmdidMRUProj15 = 614;

		internal const int cmdidMRUProj16 = 615;

		internal const int cmdidMRUProj17 = 616;

		internal const int cmdidMRUProj18 = 617;

		internal const int cmdidMRUProj19 = 618;

		internal const int cmdidMRUProj20 = 619;

		internal const int cmdidMRUProj21 = 620;

		internal const int cmdidMRUProj22 = 621;

		internal const int cmdidMRUProj23 = 622;

		internal const int cmdidMRUProj24 = 623;

		internal const int cmdidMRUProj25 = 624;

		internal const int cmdidSplitNext = 625;

		internal const int cmdidSplitPrev = 626;

		internal const int cmdidCloseAllDocuments = 627;

		internal const int cmdidNextDocument = 628;

		internal const int cmdidPrevDocument = 629;

		internal const int cmdidTool1 = 630;

		internal const int cmdidTool2 = 631;

		internal const int cmdidTool3 = 632;

		internal const int cmdidTool4 = 633;

		internal const int cmdidTool5 = 634;

		internal const int cmdidTool6 = 635;

		internal const int cmdidTool7 = 636;

		internal const int cmdidTool8 = 637;

		internal const int cmdidTool9 = 638;

		internal const int cmdidTool10 = 639;

		internal const int cmdidTool11 = 640;

		internal const int cmdidTool12 = 641;

		internal const int cmdidTool13 = 642;

		internal const int cmdidTool14 = 643;

		internal const int cmdidTool15 = 644;

		internal const int cmdidTool16 = 645;

		internal const int cmdidTool17 = 646;

		internal const int cmdidTool18 = 647;

		internal const int cmdidTool19 = 648;

		internal const int cmdidTool20 = 649;

		internal const int cmdidTool21 = 650;

		internal const int cmdidTool22 = 651;

		internal const int cmdidTool23 = 652;

		internal const int cmdidTool24 = 653;

		internal const int cmdidExternalCommands = 654;

		internal const int cmdidPasteNextTBXCBItem = 655;

		internal const int cmdidToolboxShowAllTabs = 656;

		internal const int cmdidProjectDependencies = 657;

		internal const int cmdidCloseDocument = 658;

		internal const int cmdidToolboxSortItems = 659;

		internal const int cmdidViewBarView1 = 660;

		internal const int cmdidViewBarView2 = 661;

		internal const int cmdidViewBarView3 = 662;

		internal const int cmdidViewBarView4 = 663;

		internal const int cmdidViewBarView5 = 664;

		internal const int cmdidViewBarView6 = 665;

		internal const int cmdidViewBarView7 = 666;

		internal const int cmdidViewBarView8 = 667;

		internal const int cmdidViewBarView9 = 668;

		internal const int cmdidViewBarView10 = 669;

		internal const int cmdidViewBarView11 = 670;

		internal const int cmdidViewBarView12 = 671;

		internal const int cmdidViewBarView13 = 672;

		internal const int cmdidViewBarView14 = 673;

		internal const int cmdidViewBarView15 = 674;

		internal const int cmdidViewBarView16 = 675;

		internal const int cmdidViewBarView17 = 676;

		internal const int cmdidViewBarView18 = 677;

		internal const int cmdidViewBarView19 = 678;

		internal const int cmdidViewBarView20 = 679;

		internal const int cmdidViewBarView21 = 680;

		internal const int cmdidViewBarView22 = 681;

		internal const int cmdidViewBarView23 = 682;

		internal const int cmdidViewBarView24 = 683;

		internal const int cmdidSolutionCfg = 684;

		internal const int cmdidSolutionCfgGetList = 685;

		internal const int cmdidManageIndexes = 675;

		internal const int cmdidManageRelationships = 676;

		internal const int cmdidManageConstraints = 677;

		internal const int cmdidTaskListCustomView1 = 678;

		internal const int cmdidTaskListCustomView2 = 679;

		internal const int cmdidTaskListCustomView3 = 680;

		internal const int cmdidTaskListCustomView4 = 681;

		internal const int cmdidTaskListCustomView5 = 682;

		internal const int cmdidTaskListCustomView6 = 683;

		internal const int cmdidTaskListCustomView7 = 684;

		internal const int cmdidTaskListCustomView8 = 685;

		internal const int cmdidTaskListCustomView9 = 686;

		internal const int cmdidTaskListCustomView10 = 687;

		internal const int cmdidTaskListCustomView11 = 688;

		internal const int cmdidTaskListCustomView12 = 689;

		internal const int cmdidTaskListCustomView13 = 690;

		internal const int cmdidTaskListCustomView14 = 691;

		internal const int cmdidTaskListCustomView15 = 692;

		internal const int cmdidTaskListCustomView16 = 693;

		internal const int cmdidTaskListCustomView17 = 694;

		internal const int cmdidTaskListCustomView18 = 695;

		internal const int cmdidTaskListCustomView19 = 696;

		internal const int cmdidTaskListCustomView20 = 697;

		internal const int cmdidTaskListCustomView21 = 698;

		internal const int cmdidTaskListCustomView22 = 699;

		internal const int cmdidTaskListCustomView23 = 700;

		internal const int cmdidTaskListCustomView24 = 701;

		internal const int cmdidTaskListCustomView25 = 702;

		internal const int cmdidTaskListCustomView26 = 703;

		internal const int cmdidTaskListCustomView27 = 704;

		internal const int cmdidTaskListCustomView28 = 705;

		internal const int cmdidTaskListCustomView29 = 706;

		internal const int cmdidTaskListCustomView30 = 707;

		internal const int cmdidTaskListCustomView31 = 708;

		internal const int cmdidTaskListCustomView32 = 709;

		internal const int cmdidTaskListCustomView33 = 710;

		internal const int cmdidTaskListCustomView34 = 711;

		internal const int cmdidTaskListCustomView35 = 712;

		internal const int cmdidTaskListCustomView36 = 713;

		internal const int cmdidTaskListCustomView37 = 714;

		internal const int cmdidTaskListCustomView38 = 715;

		internal const int cmdidTaskListCustomView39 = 716;

		internal const int cmdidTaskListCustomView40 = 717;

		internal const int cmdidTaskListCustomView41 = 718;

		internal const int cmdidTaskListCustomView42 = 719;

		internal const int cmdidTaskListCustomView43 = 720;

		internal const int cmdidTaskListCustomView44 = 721;

		internal const int cmdidTaskListCustomView45 = 722;

		internal const int cmdidTaskListCustomView46 = 723;

		internal const int cmdidTaskListCustomView47 = 724;

		internal const int cmdidTaskListCustomView48 = 725;

		internal const int cmdidTaskListCustomView49 = 726;

		internal const int cmdidTaskListCustomView50 = 727;

		internal const int cmdidObjectSearch = 728;

		internal const int cmdidCommandWindow = 729;

		internal const int cmdidCommandWindowMarkMode = 730;

		internal const int cmdidLogCommandWindow = 731;

		internal const int cmdidShell = 732;

		internal const int cmdidSingleChar = 733;

		internal const int cmdidZeroOrMore = 734;

		internal const int cmdidOneOrMore = 735;

		internal const int cmdidBeginLine = 736;

		internal const int cmdidEndLine = 737;

		internal const int cmdidBeginWord = 738;

		internal const int cmdidEndWord = 739;

		internal const int cmdidCharInSet = 740;

		internal const int cmdidCharNotInSet = 741;

		internal const int cmdidOr = 742;

		internal const int cmdidEscape = 743;

		internal const int cmdidTagExp = 744;

		internal const int cmdidPatternMatchHelp = 745;

		internal const int cmdidRegExList = 746;

		internal const int cmdidDebugReserved1 = 747;

		internal const int cmdidDebugReserved2 = 748;

		internal const int cmdidDebugReserved3 = 749;

		internal const int cmdidWildZeroOrMore = 754;

		internal const int cmdidWildSingleChar = 755;

		internal const int cmdidWildSingleDigit = 756;

		internal const int cmdidWildCharInSet = 757;

		internal const int cmdidWildCharNotInSet = 758;

		internal const int cmdidFindWhatText = 759;

		internal const int cmdidTaggedExp1 = 760;

		internal const int cmdidTaggedExp2 = 761;

		internal const int cmdidTaggedExp3 = 762;

		internal const int cmdidTaggedExp4 = 763;

		internal const int cmdidTaggedExp5 = 764;

		internal const int cmdidTaggedExp6 = 765;

		internal const int cmdidTaggedExp7 = 766;

		internal const int cmdidTaggedExp8 = 767;

		internal const int cmdidTaggedExp9 = 768;

		internal const int cmdidEditorWidgetClick = 769;

		internal const int cmdidCmdWinUpdateAC = 770;

		internal const int cmdidSlnCfgMgr = 771;

		internal const int cmdidAddNewProject = 772;

		internal const int cmdidAddExistingProject = 773;

		internal const int cmdidAddNewSolutionItem = 774;

		internal const int cmdidAddExistingSolutionItem = 775;

		internal const int cmdidAutoHideContext1 = 776;

		internal const int cmdidAutoHideContext2 = 777;

		internal const int cmdidAutoHideContext3 = 778;

		internal const int cmdidAutoHideContext4 = 779;

		internal const int cmdidAutoHideContext5 = 780;

		internal const int cmdidAutoHideContext6 = 781;

		internal const int cmdidAutoHideContext7 = 782;

		internal const int cmdidAutoHideContext8 = 783;

		internal const int cmdidAutoHideContext9 = 784;

		internal const int cmdidAutoHideContext10 = 785;

		internal const int cmdidAutoHideContext11 = 786;

		internal const int cmdidAutoHideContext12 = 787;

		internal const int cmdidAutoHideContext13 = 788;

		internal const int cmdidAutoHideContext14 = 789;

		internal const int cmdidAutoHideContext15 = 790;

		internal const int cmdidAutoHideContext16 = 791;

		internal const int cmdidAutoHideContext17 = 792;

		internal const int cmdidAutoHideContext18 = 793;

		internal const int cmdidAutoHideContext19 = 794;

		internal const int cmdidAutoHideContext20 = 795;

		internal const int cmdidAutoHideContext21 = 796;

		internal const int cmdidAutoHideContext22 = 797;

		internal const int cmdidAutoHideContext23 = 798;

		internal const int cmdidAutoHideContext24 = 799;

		internal const int cmdidAutoHideContext25 = 800;

		internal const int cmdidAutoHideContext26 = 801;

		internal const int cmdidAutoHideContext27 = 802;

		internal const int cmdidAutoHideContext28 = 803;

		internal const int cmdidAutoHideContext29 = 804;

		internal const int cmdidAutoHideContext30 = 805;

		internal const int cmdidAutoHideContext31 = 806;

		internal const int cmdidAutoHideContext32 = 807;

		internal const int cmdidAutoHideContext33 = 808;

		internal const int cmdidShellNavBackward = 809;

		internal const int cmdidShellNavForward = 810;

		internal const int cmdidShellNavigate1 = 811;

		internal const int cmdidShellNavigate2 = 812;

		internal const int cmdidShellNavigate3 = 813;

		internal const int cmdidShellNavigate4 = 814;

		internal const int cmdidShellNavigate5 = 815;

		internal const int cmdidShellNavigate6 = 816;

		internal const int cmdidShellNavigate7 = 817;

		internal const int cmdidShellNavigate8 = 818;

		internal const int cmdidShellNavigate9 = 819;

		internal const int cmdidShellNavigate10 = 820;

		internal const int cmdidShellNavigate11 = 821;

		internal const int cmdidShellNavigate12 = 822;

		internal const int cmdidShellNavigate13 = 823;

		internal const int cmdidShellNavigate14 = 824;

		internal const int cmdidShellNavigate15 = 825;

		internal const int cmdidShellNavigate16 = 826;

		internal const int cmdidShellNavigate17 = 827;

		internal const int cmdidShellNavigate18 = 828;

		internal const int cmdidShellNavigate19 = 829;

		internal const int cmdidShellNavigate20 = 830;

		internal const int cmdidShellNavigate21 = 831;

		internal const int cmdidShellNavigate22 = 832;

		internal const int cmdidShellNavigate23 = 833;

		internal const int cmdidShellNavigate24 = 834;

		internal const int cmdidShellNavigate25 = 835;

		internal const int cmdidShellNavigate26 = 836;

		internal const int cmdidShellNavigate27 = 837;

		internal const int cmdidShellNavigate28 = 838;

		internal const int cmdidShellNavigate29 = 839;

		internal const int cmdidShellNavigate30 = 840;

		internal const int cmdidShellNavigate31 = 841;

		internal const int cmdidShellNavigate32 = 842;

		internal const int cmdidShellNavigate33 = 843;

		internal const int cmdidShellWindowNavigate1 = 844;

		internal const int cmdidShellWindowNavigate2 = 845;

		internal const int cmdidShellWindowNavigate3 = 846;

		internal const int cmdidShellWindowNavigate4 = 847;

		internal const int cmdidShellWindowNavigate5 = 848;

		internal const int cmdidShellWindowNavigate6 = 849;

		internal const int cmdidShellWindowNavigate7 = 850;

		internal const int cmdidShellWindowNavigate8 = 851;

		internal const int cmdidShellWindowNavigate9 = 852;

		internal const int cmdidShellWindowNavigate10 = 853;

		internal const int cmdidShellWindowNavigate11 = 854;

		internal const int cmdidShellWindowNavigate12 = 855;

		internal const int cmdidShellWindowNavigate13 = 856;

		internal const int cmdidShellWindowNavigate14 = 857;

		internal const int cmdidShellWindowNavigate15 = 858;

		internal const int cmdidShellWindowNavigate16 = 859;

		internal const int cmdidShellWindowNavigate17 = 860;

		internal const int cmdidShellWindowNavigate18 = 861;

		internal const int cmdidShellWindowNavigate19 = 862;

		internal const int cmdidShellWindowNavigate20 = 863;

		internal const int cmdidShellWindowNavigate21 = 864;

		internal const int cmdidShellWindowNavigate22 = 865;

		internal const int cmdidShellWindowNavigate23 = 866;

		internal const int cmdidShellWindowNavigate24 = 867;

		internal const int cmdidShellWindowNavigate25 = 868;

		internal const int cmdidShellWindowNavigate26 = 869;

		internal const int cmdidShellWindowNavigate27 = 870;

		internal const int cmdidShellWindowNavigate28 = 871;

		internal const int cmdidShellWindowNavigate29 = 872;

		internal const int cmdidShellWindowNavigate30 = 873;

		internal const int cmdidShellWindowNavigate31 = 874;

		internal const int cmdidShellWindowNavigate32 = 875;

		internal const int cmdidShellWindowNavigate33 = 876;

		internal const int cmdidOBSDoFind = 877;

		internal const int cmdidOBSMatchCase = 878;

		internal const int cmdidOBSMatchSubString = 879;

		internal const int cmdidOBSMatchWholeWord = 880;

		internal const int cmdidOBSMatchPrefix = 881;

		internal const int cmdidBuildSln = 882;

		internal const int cmdidRebuildSln = 883;

		internal const int cmdidDeploySln = 884;

		internal const int cmdidCleanSln = 885;

		internal const int cmdidBuildSel = 886;

		internal const int cmdidRebuildSel = 887;

		internal const int cmdidDeploySel = 888;

		internal const int cmdidCleanSel = 889;

		internal const int cmdidCancelBuild = 890;

		internal const int cmdidBatchBuildDlg = 891;

		internal const int cmdidBuildCtx = 892;

		internal const int cmdidRebuildCtx = 893;

		internal const int cmdidDeployCtx = 894;

		internal const int cmdidCleanCtx = 895;

		internal const int cmdidMRUFile1 = 900;

		internal const int cmdidMRUFile2 = 901;

		internal const int cmdidMRUFile3 = 902;

		internal const int cmdidMRUFile4 = 903;

		internal const int cmdidMRUFile5 = 904;

		internal const int cmdidMRUFile6 = 905;

		internal const int cmdidMRUFile7 = 906;

		internal const int cmdidMRUFile8 = 907;

		internal const int cmdidMRUFile9 = 908;

		internal const int cmdidMRUFile10 = 909;

		internal const int cmdidMRUFile11 = 910;

		internal const int cmdidMRUFile12 = 911;

		internal const int cmdidMRUFile13 = 912;

		internal const int cmdidMRUFile14 = 913;

		internal const int cmdidMRUFile15 = 914;

		internal const int cmdidMRUFile16 = 915;

		internal const int cmdidMRUFile17 = 916;

		internal const int cmdidMRUFile18 = 917;

		internal const int cmdidMRUFile19 = 918;

		internal const int cmdidMRUFile20 = 919;

		internal const int cmdidMRUFile21 = 920;

		internal const int cmdidMRUFile22 = 921;

		internal const int cmdidMRUFile23 = 922;

		internal const int cmdidMRUFile24 = 923;

		internal const int cmdidMRUFile25 = 924;

		internal const int cmdidGotoDefn = 925;

		internal const int cmdidGotoDecl = 926;

		internal const int cmdidBrowseDefn = 927;

		internal const int cmdidShowMembers = 928;

		internal const int cmdidShowBases = 929;

		internal const int cmdidShowDerived = 930;

		internal const int cmdidShowDefns = 931;

		internal const int cmdidShowRefs = 932;

		internal const int cmdidShowCallers = 933;

		internal const int cmdidShowCallees = 934;

		internal const int cmdidDefineSubset = 935;

		internal const int cmdidSetSubset = 936;

		internal const int cmdidCVGroupingNone = 950;

		internal const int cmdidCVGroupingSortOnly = 951;

		internal const int cmdidCVGroupingGrouped = 952;

		internal const int cmdidCVShowPackages = 953;

		internal const int cmdidQryManageIndexes = 954;

		internal const int cmdidBrowseComponent = 955;

		internal const int cmdidPrintDefault = 956;

		internal const int cmdidBrowseDoc = 957;

		internal const int cmdidStandardMax = 1000;

		internal const int cmdidFormsFirst = 24576;

		internal const int cmdidFormsLast = 28671;

		internal const int cmdidVBEFirst = 32768;

		internal const int msotcidBookmarkWellMenu = 32769;

		internal const int cmdidZoom200 = 32770;

		internal const int cmdidZoom150 = 32771;

		internal const int cmdidZoom100 = 32772;

		internal const int cmdidZoom75 = 32773;

		internal const int cmdidZoom50 = 32774;

		internal const int cmdidZoom25 = 32775;

		internal const int cmdidZoom10 = 32784;

		internal const int msotcidZoomWellMenu = 32785;

		internal const int msotcidDebugPopWellMenu = 32786;

		internal const int msotcidAlignWellMenu = 32787;

		internal const int msotcidArrangeWellMenu = 32788;

		internal const int msotcidCenterWellMenu = 32789;

		internal const int msotcidSizeWellMenu = 32790;

		internal const int msotcidHorizontalSpaceWellMenu = 32791;

		internal const int msotcidVerticalSpaceWellMenu = 32800;

		internal const int msotcidDebugWellMenu = 32801;

		internal const int msotcidDebugMenuVB = 32802;

		internal const int msotcidStatementBuilderWellMenu = 32803;

		internal const int msotcidProjWinInsertMenu = 32804;

		internal const int msotcidToggleMenu = 32805;

		internal const int msotcidNewObjInsertWellMenu = 32806;

		internal const int msotcidSizeToWellMenu = 32807;

		internal const int msotcidCommandBars = 32808;

		internal const int msotcidVBOrderMenu = 32809;

		internal const int msotcidMSOnTheWeb = 32810;

		internal const int msotcidVBDesignerMenu = 32816;

		internal const int msotcidNewProjectWellMenu = 32817;

		internal const int msotcidProjectWellMenu = 32818;

		internal const int msotcidVBCode1ContextMenu = 32819;

		internal const int msotcidVBCode2ContextMenu = 32820;

		internal const int msotcidVBWatchContextMenu = 32821;

		internal const int msotcidVBImmediateContextMenu = 32822;

		internal const int msotcidVBLocalsContextMenu = 32823;

		internal const int msotcidVBFormContextMenu = 32824;

		internal const int msotcidVBControlContextMenu = 32825;

		internal const int msotcidVBProjWinContextMenu = 32826;

		internal const int msotcidVBProjWinContextBreakMenu = 32827;

		internal const int msotcidVBPreviewWinContextMenu = 32828;

		internal const int msotcidVBOBContextMenu = 32829;

		internal const int msotcidVBForms3ContextMenu = 32830;

		internal const int msotcidVBForms3ControlCMenu = 32831;

		internal const int msotcidVBForms3ControlCMenuGroup = 32832;

		internal const int msotcidVBForms3ControlPalette = 32833;

		internal const int msotcidVBForms3ToolboxCMenu = 32834;

		internal const int msotcidVBForms3MPCCMenu = 32835;

		internal const int msotcidVBForms3DragDropCMenu = 32836;

		internal const int msotcidVBToolBoxContextMenu = 32837;

		internal const int msotcidVBToolBoxGroupContextMenu = 32838;

		internal const int msotcidVBPropBrsHostContextMenu = 32839;

		internal const int msotcidVBPropBrsContextMenu = 32840;

		internal const int msotcidVBPalContextMenu = 32841;

		internal const int msotcidVBProjWinProjectContextMenu = 32842;

		internal const int msotcidVBProjWinFormContextMenu = 32843;

		internal const int msotcidVBProjWinModClassContextMenu = 32844;

		internal const int msotcidVBProjWinRelDocContextMenu = 32845;

		internal const int msotcidVBDockedWindowContextMenu = 32846;

		internal const int msotcidVBShortCutForms = 32847;

		internal const int msotcidVBShortCutCodeWindows = 32848;

		internal const int msotcidVBShortCutMisc = 32849;

		internal const int msotcidVBBuiltInMenus = 32850;

		internal const int msotcidPreviewWinFormPos = 32851;

		internal const int msotcidVBAddinFirst = 33280;
	}

	private static class ShellGuids
	{
		internal static readonly Guid VSStandardCommandSet97 = new Guid("{5efc7975-14bc-11cf-9b2b-00aa00573819}");

		internal static readonly Guid guidDsdCmdId = new Guid("{1F0FD094-8e53-11d2-8f9c-0060089fc486}");

		internal static readonly Guid SID_SOleComponentUIManager = new Guid("{5efc7974-14bc-11cf-9b2b-00aa00573819}");

		internal static readonly Guid GUID_VSTASKCATEGORY_DATADESIGNER = new Guid("{6B32EAED-13BB-11d3-A64F-00C04F683820}");

		internal static readonly Guid GUID_PropertyBrowserToolWindow = new Guid(-285584864, -7528, 4560, new byte[8] { 143, 120, 0, 160, 201, 17, 0, 87 });
	}

	private static readonly Guid s_standardCommandSet = ShellGuids.VSStandardCommandSet97;

	private static readonly Guid s_ndpCommandSet = new Guid("{74D21313-2AEE-11d1-8BFB-00A0C90F26F7}");

	private const int cmdidDesignerVerbFirst = 8192;

	private const int cmdidDesignerVerbLast = 8448;

	private const int cmdidArrangeIcons = 12298;

	private const int cmdidLineupIcons = 12299;

	private const int cmdidShowLargeIcons = 12300;

	public static readonly CommandID AlignBottom = new CommandID(s_standardCommandSet, 1);

	public static readonly CommandID AlignHorizontalCenters = new CommandID(s_standardCommandSet, 2);

	public static readonly CommandID AlignLeft = new CommandID(s_standardCommandSet, 3);

	public static readonly CommandID AlignRight = new CommandID(s_standardCommandSet, 4);

	public static readonly CommandID AlignToGrid = new CommandID(s_standardCommandSet, 5);

	public static readonly CommandID AlignTop = new CommandID(s_standardCommandSet, 6);

	public static readonly CommandID AlignVerticalCenters = new CommandID(s_standardCommandSet, 7);

	public static readonly CommandID ArrangeBottom = new CommandID(s_standardCommandSet, 8);

	public static readonly CommandID ArrangeRight = new CommandID(s_standardCommandSet, 9);

	public static readonly CommandID BringForward = new CommandID(s_standardCommandSet, 10);

	public static readonly CommandID BringToFront = new CommandID(s_standardCommandSet, 11);

	public static readonly CommandID CenterHorizontally = new CommandID(s_standardCommandSet, 12);

	public static readonly CommandID CenterVertically = new CommandID(s_standardCommandSet, 13);

	public static readonly CommandID ViewCode = new CommandID(s_standardCommandSet, 333);

	public static readonly CommandID DocumentOutline = new CommandID(s_standardCommandSet, 239);

	public static readonly CommandID Copy = new CommandID(s_standardCommandSet, 15);

	public static readonly CommandID Cut = new CommandID(s_standardCommandSet, 16);

	public static readonly CommandID Delete = new CommandID(s_standardCommandSet, 17);

	public static readonly CommandID Group = new CommandID(s_standardCommandSet, 20);

	public static readonly CommandID HorizSpaceConcatenate = new CommandID(s_standardCommandSet, 21);

	public static readonly CommandID HorizSpaceDecrease = new CommandID(s_standardCommandSet, 22);

	public static readonly CommandID HorizSpaceIncrease = new CommandID(s_standardCommandSet, 23);

	public static readonly CommandID HorizSpaceMakeEqual = new CommandID(s_standardCommandSet, 24);

	public static readonly CommandID Paste = new CommandID(s_standardCommandSet, 26);

	public static readonly CommandID Properties = new CommandID(s_standardCommandSet, 28);

	public static readonly CommandID Redo = new CommandID(s_standardCommandSet, 29);

	public static readonly CommandID MultiLevelRedo = new CommandID(s_standardCommandSet, 30);

	public static readonly CommandID SelectAll = new CommandID(s_standardCommandSet, 31);

	public static readonly CommandID SendBackward = new CommandID(s_standardCommandSet, 32);

	public static readonly CommandID SendToBack = new CommandID(s_standardCommandSet, 33);

	public static readonly CommandID SizeToControl = new CommandID(s_standardCommandSet, 35);

	public static readonly CommandID SizeToControlHeight = new CommandID(s_standardCommandSet, 36);

	public static readonly CommandID SizeToControlWidth = new CommandID(s_standardCommandSet, 37);

	public static readonly CommandID SizeToFit = new CommandID(s_standardCommandSet, 38);

	public static readonly CommandID SizeToGrid = new CommandID(s_standardCommandSet, 39);

	public static readonly CommandID SnapToGrid = new CommandID(s_standardCommandSet, 40);

	public static readonly CommandID TabOrder = new CommandID(s_standardCommandSet, 41);

	public static readonly CommandID Undo = new CommandID(s_standardCommandSet, 43);

	public static readonly CommandID MultiLevelUndo = new CommandID(s_standardCommandSet, 44);

	public static readonly CommandID Ungroup = new CommandID(s_standardCommandSet, 45);

	public static readonly CommandID VertSpaceConcatenate = new CommandID(s_standardCommandSet, 46);

	public static readonly CommandID VertSpaceDecrease = new CommandID(s_standardCommandSet, 47);

	public static readonly CommandID VertSpaceIncrease = new CommandID(s_standardCommandSet, 48);

	public static readonly CommandID VertSpaceMakeEqual = new CommandID(s_standardCommandSet, 49);

	public static readonly CommandID ShowGrid = new CommandID(s_standardCommandSet, 103);

	public static readonly CommandID ViewGrid = new CommandID(s_standardCommandSet, 125);

	public static readonly CommandID Replace = new CommandID(s_standardCommandSet, 230);

	public static readonly CommandID PropertiesWindow = new CommandID(s_standardCommandSet, 235);

	public static readonly CommandID LockControls = new CommandID(s_standardCommandSet, 369);

	public static readonly CommandID F1Help = new CommandID(s_standardCommandSet, 377);

	public static readonly CommandID ArrangeIcons = new CommandID(s_ndpCommandSet, 12298);

	public static readonly CommandID LineupIcons = new CommandID(s_ndpCommandSet, 12299);

	public static readonly CommandID ShowLargeIcons = new CommandID(s_ndpCommandSet, 12300);

	public static readonly CommandID VerbFirst = new CommandID(s_ndpCommandSet, 8192);

	public static readonly CommandID VerbLast = new CommandID(s_ndpCommandSet, 8448);
}
