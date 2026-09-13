using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ArabicSupport;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;

public class EmojiText : Text
{
	private const string regex1 = "[#*0-9]\\uFE0F?\\u20E3|©\\uFE0F?|[®\\u203C\\u2049\\u2122\\u2139\\u2194-\\u2199\\u21A9\\u21AA]\\uFE0F?|[\\u231A\\u231B]|[\\u2328\\u23CF]\\uFE0F?|[\\u23E9-\\u23EC]|[\\u23ED-\\u23EF]\\uFE0F?|\\u23F0|[\\u23F1\\u23F2]\\uFE0F?|\\u23F3|[\\u23F8-\\u23FA\\u24C2\\u25AA\\u25AB\\u25B6\\u25C0\\u25FB\\u25FC]\\uFE0F?|[\\u25FD\\u25FE]|[\\u2600-\\u2604\\u260E\\u2611]\\uFE0F?|[\\u2614\\u2615]|\\u2618\\uFE0F?|\\u261D(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|[\\u2620\\u2622\\u2623\\u2626\\u262A\\u262E\\u262F\\u2638-\\u263A\\u2640\\u2642]\\uFE0F?|[\\u2648-\\u2653]|[\\u265F\\u2660\\u2663\\u2665\\u2666\\u2668\\u267B\\u267E]\\uFE0F?|\\u267F|\\u2692\\uFE0F?|\\u2693|[\\u2694-\\u2697\\u2699\\u269B\\u269C\\u26A0]\\uFE0F?|\\u26A1|\\u26A7\\uFE0F?|[\\u26AA\\u26AB]|[\\u26B0\\u26B1]\\uFE0F?|[\\u26BD\\u26BE\\u26C4\\u26C5]|\\u26C8\\uFE0F?|\\u26CE|[\\u26CF\\u26D1\\u26D3]\\uFE0F?|\\u26D4|\\u26E9\\uFE0F?|\\u26EA|[\\u26F0\\u26F1]\\uFE0F?|[\\u26F2\\u26F3]|\\u26F4\\uFE0F?|\\u26F5|[\\u26F7\\u26F8]\\uFE0F?|\\u26F9(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\u26FA\\u26FD]|\\u2702\\uFE0F?|\\u2705|[\\u2708\\u2709]\\uFE0F?|[\\u270A\\u270B](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\u270C\\u270D](?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|\\u270F\\uFE0F?|[\\u2712\\u2714\\u2716\\u271D\\u2721]\\uFE0F?|\\u2728|[\\u2733\\u2734\\u2744\\u2747]\\uFE0F?|[\\u274C\\u274E\\u2753-\\u2755\\u2757]|\\u2763\\uFE0F?|\\u2764(?:\\u200D(?:\\uD83D\\uDD25|\\uD83E\\uDE79)|\\uFE0F(?:\\u200D(?:\\uD83D\\uDD25|\\uD83E\\uDE79))?)?|[\\u2795-\\u2797]|\\u27A1\\uFE0F?|[\\u27B0\\u27BF]|[\\u2934\\u2935\\u2B05-\\u2B07]\\uFE0F?|[\\u2B1B\\u2B1C\\u2B50\\u2B55]|[\\u3030\\u303D\\u3297\\u3299]\\uFE0F?|\\uD83C(?:[\\uDC04\\uDCCF]|[\\uDD70\\uDD71\\uDD7E\\uDD7F]\\uFE0F?|[\\uDD8E\\uDD91-\\uDD9A]|\\uDDE6\\uD83C[\\uDDE8-\\uDDEC\\uDDEE\\uDDF1\\uDDF2\\uDDF4\\uDDF6-\\uDDFA\\uDDFC\\uDDFD\\uDDFF]|\\uDDE7\\uD83C[\\uDDE6\\uDDE7\\uDDE9-\\uDDEF\\uDDF1-\\uDDF4\\uDDF6-\\uDDF9\\uDDFB\\uDDFC\\uDDFE\\uDDFF]|\\uDDE8\\uD83C[\\uDDE6\\uDDE8\\uDDE9\\uDDEB-\\uDDEE\\uDDF0-\\uDDF5\\uDDF7\\uDDFA-\\uDDFF]|\\uDDE9\\uD83C[\\uDDEA\\uDDEC\\uDDEF\\uDDF0\\uDDF2\\uDDF4\\uDDFF]|\\uDDEA\\uD83C[\\uDDE6\\uDDE8\\uDDEA\\uDDEC\\uDDED\\uDDF7-\\uDDFA]|\\uDDEB\\uD83C[\\uDDEE-\\uDDF0\\uDDF2\\uDDF4\\uDDF7]|\\uDDEC\\uD83C[\\uDDE6\\uDDE7\\uDDE9-\\uDDEE\\uDDF1-\\uDDF3\\uDDF5-\\uDDFA\\uDDFC\\uDDFE]|\\uDDED\\uD83C[\\uDDF0\\uDDF2\\uDDF3\\uDDF7\\uDDF9\\uDDFA]|\\uDDEE\\uD83C[\\uDDE8-\\uDDEA\\uDDF1-\\uDDF4\\uDDF6-\\uDDF9]|\\uDDEF\\uD83C[\\uDDEA\\uDDF2\\uDDF4\\uDDF5]|\\uDDF0\\uD83C[\\uDDEA\\uDDEC-\\uDDEE\\uDDF2\\uDDF3\\uDDF5\\uDDF7\\uDDFC\\uDDFE\\uDDFF]|\\uDDF1\\uD83C[\\uDDE6-\\uDDE8\\uDDEE\\uDDF0\\uDDF7-\\uDDFB\\uDDFE]|\\uDDF2\\uD83C[\\uDDE6\\uDDE8-\\uDDED\\uDDF0-\\uDDFF]|\\uDDF3\\uD83C[\\uDDE6\\uDDE8\\uDDEA-\\uDDEC\\uDDEE\\uDDF1\\uDDF4\\uDDF5\\uDDF7\\uDDFA\\uDDFF]|\\uDDF4\\uD83C\\uDDF2|\\uDDF5\\uD83C[\\uDDE6\\uDDEA-\\uDDED\\uDDF0-\\uDDF3\\uDDF7-\\uDDF9\\uDDFC\\uDDFE]|\\uDDF6\\uD83C\\uDDE6|\\uDDF7\\uD83C[\\uDDEA\\uDDF4\\uDDF8\\uDDFA\\uDDFC]|\\uDDF8\\uD83C[\\uDDE6-\\uDDEA\\uDDEC-\\uDDF4\\uDDF7-\\uDDF9\\uDDFB\\uDDFD-\\uDDFF]|\\uDDF9\\uD83C[\\uDDE6\\uDDE8\\uDDE9\\uDDEB-\\uDDED\\uDDEF-\\uDDF4\\uDDF7\\uDDF9\\uDDFB\\uDDFC\\uDDFF]|\\uDDFA\\uD83C[\\uDDE6\\uDDEC\\uDDF2\\uDDF3\\uDDF8\\uDDFE\\uDDFF]|\\uDDFB\\uD83C[\\uDDE6\\uDDE8\\uDDEA\\uDDEC\\uDDEE\\uDDF3\\uDDFA]|\\uDDFC\\uD83C[\\uDDEB\\uDDF8]|\\uDDFD\\uD83C\\uDDF0|\\uDDFE\\uD83C[\\uDDEA\\uDDF9]|\\uDDFF\\uD83C[\\uDDE6\\uDDF2\\uDDFC]|\\uDE01|\\uDE02\\uFE0F?|[\\uDE1A\\uDE2F\\uDE32-\\uDE36]|\\uDE37\\uFE0F?|[\\uDE38-\\uDE3A\\uDE50\\uDE51\\uDF00-\\uDF20]|[\\uDF21\\uDF24-\\uDF2C]\\uFE0F?|[\\uDF2D-\\uDF35]|\\uDF36\\uFE0F?|[\\uDF37-\\uDF7C]|\\uDF7D\\uFE0F?|[\\uDF7E-\\uDF84]|\\uDF85(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDF86-\\uDF93]|[\\uDF96\\uDF97\\uDF99-\\uDF9B\\uDF9E\\uDF9F]\\uFE0F?|[\\uDFA0-\\uDFC1]|\\uDFC2(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDFC3\\uDFC4](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFC5\\uDFC6]|\\uDFC7(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDFC8\\uDFC9]|\\uDFCA(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFCB\\uDFCC](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFCD\\uDFCE]\\uFE0F?|[\\uDFCF-\\uDFD3]|[\\uDFD4-\\uDFDF]\\uFE0F?|[\\uDFE0-\\uDFF0]|\\uDFF3(?:\\u200D(?:\\u26A7\\uFE0F?|\\uD83C\\uDF08)|\\uFE0F(?:\\u200D(?:\\u26A7\\uFE0F?|\\uD83C\\uDF08))?)?|\\uDFF4(?:\\u200D\\u2620\\uFE0F?|\\uDB40\\uDC67\\uDB40\\uDC62\\uDB40(?:\\uDC65\\uDB40\\uDC6E\\uDB40\\uDC67|\\uDC73\\uDB40\\uDC63\\uDB40\\uDC74|\\uDC77\\uDB40\\uDC6C\\uDB40\\uDC73)\\uDB40\\uDC7F)?|[\\uDFF5\\uDFF7]\\uFE0F?|[\\uDFF8-\\uDFFF])|\\uD83D(?:[\\uDC00-\\uDC07]|\\uDC08(?:\\u200D\\u2B1B)?|[\\uDC09-\\uDC14]|\\uDC15(?:\\u200D\\uD83E\\uDDBA)?|[\\uDC16-\\uDC3A]|\\uDC3B(?:\\u200D\\u2744\\uFE0F?)?|[\\uDC3C-\\uDC3E]|\\uDC3F\\uFE0F?|\\uDC40|\\uDC41(?:\\u200D\\uD83D\\uDDE8\\uFE0F?|\\uFE0F(?:\\u200D\\uD83D\\uDDE8\\uFE0F?)?)?|[\\uDC42\\uDC43](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC44\\uDC45]|[\\uDC46-\\uDC50](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC51-\\uDC65]|[\\uDC66\\uDC67](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC68(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?|[\\uDC68\\uDC69]\\u200D\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?)|[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92])|\\uD83E[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFC-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB-\\uDFFE]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|\\uDC69(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?[\\uDC68\\uDC69]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?|\\uDC69\\u200D\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?)|[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92])|\\uD83E[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFC-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFE]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|\\uDC6A|[\\uDC6B-\\uDC6D](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC6E(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC6F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDC70\\uDC71](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC72(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC73(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDC74-\\uDC76](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC77(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC78(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC79-\\uDC7B]|\\uDC7C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC7D-\\uDC80]|[\\uDC81\\uDC82](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC83(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC84|\\uDC85(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC86\\uDC87](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDC88-\\uDC8E]|\\uDC8F(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC90|\\uDC91(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC92-\\uDCA9]|\\uDCAA(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDCAB-\\uDCFC]|\\uDCFD\\uFE0F?|[\\uDCFF-\\uDD3D]|[\\uDD49\\uDD4A]\\uFE0F?|[\\uDD4B-\\uDD4E\\uDD50-\\uDD67]|[\\uDD6F\\uDD70\\uDD73]\\uFE0F?|\\uDD74(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|\\uDD75(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD76-\\uDD79]\\uFE0F?|\\uDD7A(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD87\\uDD8A-\\uDD8D]\\uFE0F?|\\uDD90(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|[\\uDD95\\uDD96](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDA4|[\\uDDA5\\uDDA8\\uDDB1\\uDDB2\\uDDBC\\uDDC2-\\uDDC4\\uDDD1-\\uDDD3\\uDDDC-\\uDDDE\\uDDE1\\uDDE3\\uDDE8\\uDDEF\\uDDF3\\uDDFA]\\uFE0F?|[\\uDDFB-\\uDE2D]|\\uDE2E(?:\\u200D\\uD83D\\uDCA8)?|[\\uDE2F-\\uDE34]|\\uDE35(?:\\u200D\\uD83D\\uDCAB)?|\\uDE36(?:\\u200D\\uD83C\\uDF2B\\uFE0F?)?|[\\uDE37-\\uDE44]|[\\uDE45-\\uDE47](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDE48-\\uDE4A]|\\uDE4B(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDE4C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDE4D\\uDE4E](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDE4F(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDE80-\\uDEA2]|\\uDEA3(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDEA4-\\uDEB3]|[\\uDEB4-\\uDEB6](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDEB7-\\uDEBF]|\\uDEC0(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDEC1-\\uDEC5]|\\uDECB\\uFE0F?|\\uDECC(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDECD-\\uDECF]\\uFE0F?|[\\uDED0-\\uDED2\\uDED5-\\uDED7]|[\\uDEE0-\\uDEE5\\uDEE9]\\uFE0F?|[\\uDEEB\\uDEEC]|[\\uDEF0\\uDEF3]\\uFE0F?|[\\uDEF4-\\uDEFC\\uDFE0-\\uDFEB])|\\uD83E(?:\\uDD0C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD0D\\uDD0E]|\\uDD0F(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD10-\\uDD17]|[\\uDD18-\\uDD1C](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDD1D|[\\uDD1E\\uDD1F](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD20-\\uDD25]|\\uDD26(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD27-\\uDD2F]|[\\uDD30-\\uDD34](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDD35(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDD36(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD37-\\uDD39](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDD3A|\\uDD3C(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDD3D\\uDD3E](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD3F-\\uDD45\\uDD47-\\uDD76]|\\uDD77(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD78\\uDD7A-\\uDDB4]|[\\uDDB5\\uDDB6](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDB7|[\\uDDB8\\uDDB9](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDBA|\\uDDBB(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDDBC-\\uDDCB]|[\\uDDCD-\\uDDCF](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDD0|\\uDDD1(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD]))|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFC-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFE]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|[\\uDDD2\\uDDD3](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDD4(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDD5(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDDD6-\\uDDDD](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDDDE\\uDDDF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDDE0-\\uDDFF\\uDE70-\\uDE74\\uDE78-\\uDE7A\\uDE80-\\uDE86\\uDE90-\\uDEA8\\uDEB0-\\uDEB6\\uDEC0-\\uDEC2\\uDED0-\\uDED6])";

	private const string regex2 = "<sprite=([a-z0-9A-Z]+)>";

	private const string regex3 = "<\\|>";

	private const string regex4 = "<color=#[a-z0-9A-Z]+>|</color>";

	private static Regex Regex1 = new Regex("[#*0-9]\\uFE0F?\\u20E3|©\\uFE0F?|[®\\u203C\\u2049\\u2122\\u2139\\u2194-\\u2199\\u21A9\\u21AA]\\uFE0F?|[\\u231A\\u231B]|[\\u2328\\u23CF]\\uFE0F?|[\\u23E9-\\u23EC]|[\\u23ED-\\u23EF]\\uFE0F?|\\u23F0|[\\u23F1\\u23F2]\\uFE0F?|\\u23F3|[\\u23F8-\\u23FA\\u24C2\\u25AA\\u25AB\\u25B6\\u25C0\\u25FB\\u25FC]\\uFE0F?|[\\u25FD\\u25FE]|[\\u2600-\\u2604\\u260E\\u2611]\\uFE0F?|[\\u2614\\u2615]|\\u2618\\uFE0F?|\\u261D(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|[\\u2620\\u2622\\u2623\\u2626\\u262A\\u262E\\u262F\\u2638-\\u263A\\u2640\\u2642]\\uFE0F?|[\\u2648-\\u2653]|[\\u265F\\u2660\\u2663\\u2665\\u2666\\u2668\\u267B\\u267E]\\uFE0F?|\\u267F|\\u2692\\uFE0F?|\\u2693|[\\u2694-\\u2697\\u2699\\u269B\\u269C\\u26A0]\\uFE0F?|\\u26A1|\\u26A7\\uFE0F?|[\\u26AA\\u26AB]|[\\u26B0\\u26B1]\\uFE0F?|[\\u26BD\\u26BE\\u26C4\\u26C5]|\\u26C8\\uFE0F?|\\u26CE|[\\u26CF\\u26D1\\u26D3]\\uFE0F?|\\u26D4|\\u26E9\\uFE0F?|\\u26EA|[\\u26F0\\u26F1]\\uFE0F?|[\\u26F2\\u26F3]|\\u26F4\\uFE0F?|\\u26F5|[\\u26F7\\u26F8]\\uFE0F?|\\u26F9(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\u26FA\\u26FD]|\\u2702\\uFE0F?|\\u2705|[\\u2708\\u2709]\\uFE0F?|[\\u270A\\u270B](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\u270C\\u270D](?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|\\u270F\\uFE0F?|[\\u2712\\u2714\\u2716\\u271D\\u2721]\\uFE0F?|\\u2728|[\\u2733\\u2734\\u2744\\u2747]\\uFE0F?|[\\u274C\\u274E\\u2753-\\u2755\\u2757]|\\u2763\\uFE0F?|\\u2764(?:\\u200D(?:\\uD83D\\uDD25|\\uD83E\\uDE79)|\\uFE0F(?:\\u200D(?:\\uD83D\\uDD25|\\uD83E\\uDE79))?)?|[\\u2795-\\u2797]|\\u27A1\\uFE0F?|[\\u27B0\\u27BF]|[\\u2934\\u2935\\u2B05-\\u2B07]\\uFE0F?|[\\u2B1B\\u2B1C\\u2B50\\u2B55]|[\\u3030\\u303D\\u3297\\u3299]\\uFE0F?|\\uD83C(?:[\\uDC04\\uDCCF]|[\\uDD70\\uDD71\\uDD7E\\uDD7F]\\uFE0F?|[\\uDD8E\\uDD91-\\uDD9A]|\\uDDE6\\uD83C[\\uDDE8-\\uDDEC\\uDDEE\\uDDF1\\uDDF2\\uDDF4\\uDDF6-\\uDDFA\\uDDFC\\uDDFD\\uDDFF]|\\uDDE7\\uD83C[\\uDDE6\\uDDE7\\uDDE9-\\uDDEF\\uDDF1-\\uDDF4\\uDDF6-\\uDDF9\\uDDFB\\uDDFC\\uDDFE\\uDDFF]|\\uDDE8\\uD83C[\\uDDE6\\uDDE8\\uDDE9\\uDDEB-\\uDDEE\\uDDF0-\\uDDF5\\uDDF7\\uDDFA-\\uDDFF]|\\uDDE9\\uD83C[\\uDDEA\\uDDEC\\uDDEF\\uDDF0\\uDDF2\\uDDF4\\uDDFF]|\\uDDEA\\uD83C[\\uDDE6\\uDDE8\\uDDEA\\uDDEC\\uDDED\\uDDF7-\\uDDFA]|\\uDDEB\\uD83C[\\uDDEE-\\uDDF0\\uDDF2\\uDDF4\\uDDF7]|\\uDDEC\\uD83C[\\uDDE6\\uDDE7\\uDDE9-\\uDDEE\\uDDF1-\\uDDF3\\uDDF5-\\uDDFA\\uDDFC\\uDDFE]|\\uDDED\\uD83C[\\uDDF0\\uDDF2\\uDDF3\\uDDF7\\uDDF9\\uDDFA]|\\uDDEE\\uD83C[\\uDDE8-\\uDDEA\\uDDF1-\\uDDF4\\uDDF6-\\uDDF9]|\\uDDEF\\uD83C[\\uDDEA\\uDDF2\\uDDF4\\uDDF5]|\\uDDF0\\uD83C[\\uDDEA\\uDDEC-\\uDDEE\\uDDF2\\uDDF3\\uDDF5\\uDDF7\\uDDFC\\uDDFE\\uDDFF]|\\uDDF1\\uD83C[\\uDDE6-\\uDDE8\\uDDEE\\uDDF0\\uDDF7-\\uDDFB\\uDDFE]|\\uDDF2\\uD83C[\\uDDE6\\uDDE8-\\uDDED\\uDDF0-\\uDDFF]|\\uDDF3\\uD83C[\\uDDE6\\uDDE8\\uDDEA-\\uDDEC\\uDDEE\\uDDF1\\uDDF4\\uDDF5\\uDDF7\\uDDFA\\uDDFF]|\\uDDF4\\uD83C\\uDDF2|\\uDDF5\\uD83C[\\uDDE6\\uDDEA-\\uDDED\\uDDF0-\\uDDF3\\uDDF7-\\uDDF9\\uDDFC\\uDDFE]|\\uDDF6\\uD83C\\uDDE6|\\uDDF7\\uD83C[\\uDDEA\\uDDF4\\uDDF8\\uDDFA\\uDDFC]|\\uDDF8\\uD83C[\\uDDE6-\\uDDEA\\uDDEC-\\uDDF4\\uDDF7-\\uDDF9\\uDDFB\\uDDFD-\\uDDFF]|\\uDDF9\\uD83C[\\uDDE6\\uDDE8\\uDDE9\\uDDEB-\\uDDED\\uDDEF-\\uDDF4\\uDDF7\\uDDF9\\uDDFB\\uDDFC\\uDDFF]|\\uDDFA\\uD83C[\\uDDE6\\uDDEC\\uDDF2\\uDDF3\\uDDF8\\uDDFE\\uDDFF]|\\uDDFB\\uD83C[\\uDDE6\\uDDE8\\uDDEA\\uDDEC\\uDDEE\\uDDF3\\uDDFA]|\\uDDFC\\uD83C[\\uDDEB\\uDDF8]|\\uDDFD\\uD83C\\uDDF0|\\uDDFE\\uD83C[\\uDDEA\\uDDF9]|\\uDDFF\\uD83C[\\uDDE6\\uDDF2\\uDDFC]|\\uDE01|\\uDE02\\uFE0F?|[\\uDE1A\\uDE2F\\uDE32-\\uDE36]|\\uDE37\\uFE0F?|[\\uDE38-\\uDE3A\\uDE50\\uDE51\\uDF00-\\uDF20]|[\\uDF21\\uDF24-\\uDF2C]\\uFE0F?|[\\uDF2D-\\uDF35]|\\uDF36\\uFE0F?|[\\uDF37-\\uDF7C]|\\uDF7D\\uFE0F?|[\\uDF7E-\\uDF84]|\\uDF85(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDF86-\\uDF93]|[\\uDF96\\uDF97\\uDF99-\\uDF9B\\uDF9E\\uDF9F]\\uFE0F?|[\\uDFA0-\\uDFC1]|\\uDFC2(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDFC3\\uDFC4](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFC5\\uDFC6]|\\uDFC7(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDFC8\\uDFC9]|\\uDFCA(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFCB\\uDFCC](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDFCD\\uDFCE]\\uFE0F?|[\\uDFCF-\\uDFD3]|[\\uDFD4-\\uDFDF]\\uFE0F?|[\\uDFE0-\\uDFF0]|\\uDFF3(?:\\u200D(?:\\u26A7\\uFE0F?|\\uD83C\\uDF08)|\\uFE0F(?:\\u200D(?:\\u26A7\\uFE0F?|\\uD83C\\uDF08))?)?|\\uDFF4(?:\\u200D\\u2620\\uFE0F?|\\uDB40\\uDC67\\uDB40\\uDC62\\uDB40(?:\\uDC65\\uDB40\\uDC6E\\uDB40\\uDC67|\\uDC73\\uDB40\\uDC63\\uDB40\\uDC74|\\uDC77\\uDB40\\uDC6C\\uDB40\\uDC73)\\uDB40\\uDC7F)?|[\\uDFF5\\uDFF7]\\uFE0F?|[\\uDFF8-\\uDFFF])|\\uD83D(?:[\\uDC00-\\uDC07]|\\uDC08(?:\\u200D\\u2B1B)?|[\\uDC09-\\uDC14]|\\uDC15(?:\\u200D\\uD83E\\uDDBA)?|[\\uDC16-\\uDC3A]|\\uDC3B(?:\\u200D\\u2744\\uFE0F?)?|[\\uDC3C-\\uDC3E]|\\uDC3F\\uFE0F?|\\uDC40|\\uDC41(?:\\u200D\\uD83D\\uDDE8\\uFE0F?|\\uFE0F(?:\\u200D\\uD83D\\uDDE8\\uFE0F?)?)?|[\\uDC42\\uDC43](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC44\\uDC45]|[\\uDC46-\\uDC50](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC51-\\uDC65]|[\\uDC66\\uDC67](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC68(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?|[\\uDC68\\uDC69]\\u200D\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?)|[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92])|\\uD83E[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFC-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?\\uDC68\\uD83C[\\uDFFB-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D\\uDC68\\uD83C[\\uDFFB-\\uDFFE]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|\\uDC69(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:\\uDC8B\\u200D\\uD83D)?[\\uDC68\\uDC69]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?|\\uDC69\\u200D\\uD83D(?:\\uDC66(?:\\u200D\\uD83D\\uDC66)?|\\uDC67(?:\\u200D\\uD83D[\\uDC66\\uDC67])?)|[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92])|\\uD83E[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFC-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D\\uD83D(?:[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF]|\\uDC8B\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFF])|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83D[\\uDC68\\uDC69]\\uD83C[\\uDFFB-\\uDFFE]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|\\uDC6A|[\\uDC6B-\\uDC6D](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC6E(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC6F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDC70\\uDC71](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC72(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC73(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDC74-\\uDC76](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC77(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC78(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC79-\\uDC7B]|\\uDC7C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC7D-\\uDC80]|[\\uDC81\\uDC82](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDC83(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC84|\\uDC85(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC86\\uDC87](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDC88-\\uDC8E]|\\uDC8F(?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDC90|\\uDC91(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDC92-\\uDCA9]|\\uDCAA(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDCAB-\\uDCFC]|\\uDCFD\\uFE0F?|[\\uDCFF-\\uDD3D]|[\\uDD49\\uDD4A]\\uFE0F?|[\\uDD4B-\\uDD4E\\uDD50-\\uDD67]|[\\uDD6F\\uDD70\\uDD73]\\uFE0F?|\\uDD74(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|\\uDD75(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|\\uFE0F(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD76-\\uDD79]\\uFE0F?|\\uDD7A(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD87\\uDD8A-\\uDD8D]\\uFE0F?|\\uDD90(?:\\uD83C[\\uDFFB-\\uDFFF]|\\uFE0F)?|[\\uDD95\\uDD96](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDA4|[\\uDDA5\\uDDA8\\uDDB1\\uDDB2\\uDDBC\\uDDC2-\\uDDC4\\uDDD1-\\uDDD3\\uDDDC-\\uDDDE\\uDDE1\\uDDE3\\uDDE8\\uDDEF\\uDDF3\\uDDFA]\\uFE0F?|[\\uDDFB-\\uDE2D]|\\uDE2E(?:\\u200D\\uD83D\\uDCA8)?|[\\uDE2F-\\uDE34]|\\uDE35(?:\\u200D\\uD83D\\uDCAB)?|\\uDE36(?:\\u200D\\uD83C\\uDF2B\\uFE0F?)?|[\\uDE37-\\uDE44]|[\\uDE45-\\uDE47](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDE48-\\uDE4A]|\\uDE4B(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDE4C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDE4D\\uDE4E](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDE4F(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDE80-\\uDEA2]|\\uDEA3(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDEA4-\\uDEB3]|[\\uDEB4-\\uDEB6](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDEB7-\\uDEBF]|\\uDEC0(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDEC1-\\uDEC5]|\\uDECB\\uFE0F?|\\uDECC(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDECD-\\uDECF]\\uFE0F?|[\\uDED0-\\uDED2\\uDED5-\\uDED7]|[\\uDEE0-\\uDEE5\\uDEE9]\\uFE0F?|[\\uDEEB\\uDEEC]|[\\uDEF0\\uDEF3]\\uFE0F?|[\\uDEF4-\\uDEFC\\uDFE0-\\uDFEB])|\\uD83E(?:\\uDD0C(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD0D\\uDD0E]|\\uDD0F(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD10-\\uDD17]|[\\uDD18-\\uDD1C](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDD1D|[\\uDD1E\\uDD1F](?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD20-\\uDD25]|\\uDD26(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD27-\\uDD2F]|[\\uDD30-\\uDD34](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDD35(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDD36(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD37-\\uDD39](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDD3A|\\uDD3C(?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDD3D\\uDD3E](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDD3F-\\uDD45\\uDD47-\\uDD76]|\\uDD77(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDD78\\uDD7A-\\uDDB4]|[\\uDDB5\\uDDB6](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDB7|[\\uDDB8\\uDDB9](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDBA|\\uDDBB(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDDBC-\\uDDCB]|[\\uDDCD-\\uDDCF](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDD0|\\uDDD1(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD]))|\\uD83C(?:\\uDFFB(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFC-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFC(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB\\uDFFD-\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFD(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB\\uDFFC\\uDFFE\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFE(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFD\\uDFFF]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?|\\uDFFF(?:\\u200D(?:[\\u2695\\u2696\\u2708]\\uFE0F?|\\u2764\\uFE0F?\\u200D(?:\\uD83D\\uDC8B\\u200D)?\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFE]|\\uD83C[\\uDF3E\\uDF73\\uDF7C\\uDF84\\uDF93\\uDFA4\\uDFA8\\uDFEB\\uDFED]|\\uD83D[\\uDCBB\\uDCBC\\uDD27\\uDD2C\\uDE80\\uDE92]|\\uD83E(?:\\uDD1D\\u200D\\uD83E\\uDDD1\\uD83C[\\uDFFB-\\uDFFF]|[\\uDDAF-\\uDDB3\\uDDBC\\uDDBD])))?))?|[\\uDDD2\\uDDD3](?:\\uD83C[\\uDFFB-\\uDFFF])?|\\uDDD4(?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|\\uDDD5(?:\\uD83C[\\uDFFB-\\uDFFF])?|[\\uDDD6-\\uDDDD](?:\\u200D[\\u2640\\u2642]\\uFE0F?|\\uD83C[\\uDFFB-\\uDFFF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?)?|[\\uDDDE\\uDDDF](?:\\u200D[\\u2640\\u2642]\\uFE0F?)?|[\\uDDE0-\\uDDFF\\uDE70-\\uDE74\\uDE78-\\uDE7A\\uDE80-\\uDE86\\uDE90-\\uDEA8\\uDEB0-\\uDEB6\\uDEC0-\\uDEC2\\uDED0-\\uDED6])", RegexOptions.Compiled);

	private static Regex Regex2 = new Regex("<sprite=([a-z0-9A-Z]+)>", RegexOptions.Compiled);

	private static Regex Regex3 = new Regex("<\\|>", RegexOptions.Compiled);

	private static Regex Regex4 = new Regex("<color=#[a-z0-9A-Z]+>|</color>", RegexOptions.Compiled);

	private static Regex RegexWhitespace = new Regex("\\s", RegexOptions.Compiled);

	private static TextGenerator _generator = new TextGenerator();

	private List<string> emojiHolderList = new List<string>(16);

	private const string emojiPlaceHolder = "<|>";

	[SerializeField]
	private bool m_useTextWithEllipsis;

	private static Dictionary<int, EmojiFrame> emojiDic = new Dictionary<int, EmojiFrame>();

	private static readonly Dictionary<string, EmojiFrame> emojiFrames = new Dictionary<string, EmojiFrame>();

	private static readonly Dictionary<string, string> emojiFastSearch = new Dictionary<string, string>();

	private static StringBuilder sb_ = new StringBuilder(128);

	private static byte[] utf32_bytes_ = new byte[64];

	private static StringBuilder utf32_sb_ = new StringBuilder(64);

	private static string[] b2str_ = new string[10] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09" };

	public bool supportEmoji = true;

	private float m_IconScaleOfDoubleSymbole = 1f;

	private bool m_EmojiParsingRequired = true;

	private string m_EmojiText;

	private readonly UIVertex[] m_TempVerts = new UIVertex[4];

	private StringBuilder _stringBuilder = new StringBuilder(50);

	private bool _lock;

	private string _compareValue = "";

	private string[] FixedText = new string[30];

	private string[] Holder = new string[5];

	private string TextHolder;

	private int startIndex;

	private int endIndex;

	private int length;

	public override float preferredWidth => base.cachedTextGeneratorForLayout.GetPreferredWidth(emojiText, GetGenerationSettings(base.rectTransform.rect.size)) / base.pixelsPerUnit;

	public override float preferredHeight => base.cachedTextGeneratorForLayout.GetPreferredHeight(emojiText, GetGenerationSettings(base.rectTransform.rect.size)) / base.pixelsPerUnit;

	public string emojiText
	{
		get
		{
			if (!string.IsNullOrEmpty(text))
			{
				return Regex1.Replace(text, "%?");
			}
			return text;
		}
	}

	public override string text
	{
		get
		{
			return m_Text;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				if (!string.IsNullOrEmpty(m_Text))
				{
					m_Text = "";
					SetVerticesDirty();
				}
				return;
			}
			value = GetRealValue(value);
			if (IsRtl(value))
			{
				switch (base.alignment)
				{
				case TextAnchor.LowerLeft:
					base.alignment = TextAnchor.LowerRight;
					break;
				case TextAnchor.MiddleLeft:
					base.alignment = TextAnchor.MiddleRight;
					break;
				case TextAnchor.UpperLeft:
					base.alignment = TextAnchor.UpperRight;
					break;
				}
				value = ArabicFixer.FixText(value, PopulateWithErrors);
			}
			if (m_Text != value)
			{
				m_Text = GetRealValue(value);
				SetVerticesDirty();
				SetLayoutDirty();
			}
		}
	}

	private int AppendString(int lastIndex, string value, int startIndex, string appendStr)
	{
		int num = startIndex - lastIndex;
		if (lastIndex >= 0 && lastIndex < value.Length && num >= 0 && lastIndex + num < value.Length)
		{
			sb_.Append(value, lastIndex, num);
		}
		else
		{
			Log.Error("offset out of range!");
		}
		if (!string.IsNullOrEmpty(appendStr))
		{
			sb_.Append(appendStr);
		}
		return 0;
	}

	private bool IsRtl(string str)
	{
		int num = 0;
		foreach (char c in str)
		{
			if ((c >= '\u0600' && c <= 'ۿ') || (c >= 'ﹰ' && c <= '\ufeff'))
			{
				num++;
			}
		}
		return (float)num * 1f / (float)str.Length >= 0.15f;
	}

	private string GetRealValue(string value)
	{
		emojiHolderList.Clear();
		sb_.Clear();
		if (!m_useTextWithEllipsis)
		{
			return value;
		}
		value = Regex4.Replace(value, "");
		MatchCollection matchCollection = Regex1.Matches(value);
		int num = 0;
		for (int i = 0; i < matchCollection.Count; i++)
		{
			emojiHolderList.Add(matchCollection[i].Value);
			AppendString(num, value, matchCollection[i].Index, "<|>");
			num = matchCollection[i].Index + matchCollection[i].Length;
		}
		if (value.Length - num > 0)
		{
			sb_.Append(value, num, value.Length - num);
		}
		value = sb_.ToString();
		value = GetTextWithEllipsis(value);
		sb_.Clear();
		num = 0;
		MatchCollection matchCollection2 = Regex3.Matches(value);
		for (int j = 0; j < matchCollection2.Count; j++)
		{
			string emojiByIndex = GetEmojiByIndex(j);
			AppendString(num, value, matchCollection2[j].Index, emojiByIndex);
			num = matchCollection2[j].Index + matchCollection2[j].Length;
		}
		if (value.Length - num > 0)
		{
			sb_.Append(value, num, value.Length - num);
		}
		value = sb_.ToString();
		return value;
	}

	private string GetEmojiByIndex(int index)
	{
		if (emojiHolderList.Count <= index)
		{
			return "";
		}
		return emojiHolderList[index];
	}

	private string GetTextWithEllipsis(string value)
	{
		bool flag = false;
		sb_.Clear();
		TextGenerationSettings generationSettings = GetGenerationSettings(base.rectTransform.rect.size);
		_generator.Populate(value, generationSettings);
		int characterCountVisible = _generator.characterCountVisible;
		if (characterCountVisible > 0 && characterCountVisible >= value.Length)
		{
			return value;
		}
		characterCountVisible -= 3;
		characterCountVisible = ((characterCountVisible > 0) ? characterCountVisible : 0);
		string result = value;
		if (characterCountVisible > 0 && value.Length > characterCountVisible)
		{
			int num = value.Length;
			for (int i = 0; i < num && (flag || i <= characterCountVisible); i++)
			{
				if (value[i] == '<')
				{
					flag = true;
				}
				if (value[i] == '>')
				{
					flag = false;
				}
				sb_.Append(value[i]);
			}
			sb_.Append("...");
			result = sb_.ToString();
		}
		return result;
	}

	public override void SetVerticesDirty()
	{
		m_EmojiParsingRequired = supportEmoji;
		base.SetVerticesDirty();
	}

	public bool IsContainsEmoji()
	{
		return ParseText();
	}

	private string FixText(string value)
	{
		m_DisableFontTextureRebuiltCallback = true;
		string text = "";
		string text2 = "";
		_lock = false;
		_stringBuilder.Clear();
		_compareValue = "";
		for (int i = 0; i < value.Length; i++)
		{
			if (value[i] == '<')
			{
				_lock = true;
			}
			else if (value[i] == '>')
			{
				_lock = false;
			}
			else if (!_lock)
			{
				_stringBuilder.Append(value[i]);
			}
		}
		text = _stringBuilder.ToString();
		Holder = text.Split(new char[1] { '\n' });
		for (int j = 0; j < Holder.Length; j++)
		{
			int num = 0;
			text2 = ArabicFixer.Fix(Holder[j], showTashkeel: true, useHinduNumbers: false);
			base.cachedTextGenerator.PopulateWithErrors(text2, GetGenerationSettings(base.rectTransform.rect.size), base.gameObject);
			for (int k = 0; k < FixedText.Length; k++)
			{
				FixedText[k] = "";
			}
			for (int l = 0; l < base.cachedTextGenerator.lines.Count; l++)
			{
				startIndex = base.cachedTextGenerator.lines[l].startCharIdx;
				endIndex = ((l == base.cachedTextGenerator.lines.Count - 1) ? text2.Length : base.cachedTextGenerator.lines[l + 1].startCharIdx);
				length = endIndex - startIndex;
				FixedText[l] = text2.Substring(startIndex, length);
				num = l;
			}
			text2 = "";
			if (j == Holder.Length - 1)
			{
				for (int num2 = FixedText.Length - 1; num2 >= 0; num2--)
				{
					if (FixedText[num2] != "" && FixedText[num2] != "\n" && FixedText[num2] != null)
					{
						if (num == 0)
						{
							TextHolder += FixedText[num2];
						}
						else
						{
							TextHolder += FixedText[num2];
						}
					}
				}
				continue;
			}
			for (int num3 = FixedText.Length - 1; num3 >= 0; num3--)
			{
				if (FixedText[num3] != "" && FixedText[num3] != "\n" && FixedText[num3] != null)
				{
					TextHolder = TextHolder + FixedText[num3] + "\n";
				}
			}
		}
		text2 = TextHolder;
		TextHolder = "";
		m_DisableFontTextureRebuiltCallback = false;
		return text2;
	}

	private void initEmoji()
	{
		if (emojiFrames.Count > 0)
		{
			return;
		}
		try
		{
			TextAsset textAsset = Resources.Load<TextAsset>("texturepacker_EmojiData_bin");
			if (textAsset != null)
			{
				using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(textAsset.bytes)))
				{
					EmojiFrame value = default(EmojiFrame);
					while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
					{
						string key = binaryReader.ReadString();
						value.filename = binaryReader.ReadString();
						value.frame.x = binaryReader.ReadSingle();
						value.frame.y = binaryReader.ReadSingle();
						value.frame.w = binaryReader.ReadSingle();
						value.frame.h = binaryReader.ReadSingle();
						value.rotated = binaryReader.ReadBoolean();
						value.trimmed = binaryReader.ReadBoolean();
						value.spriteSourceSize.x = binaryReader.ReadSingle();
						value.spriteSourceSize.y = binaryReader.ReadSingle();
						value.spriteSourceSize.w = binaryReader.ReadSingle();
						value.spriteSourceSize.h = binaryReader.ReadSingle();
						value.sourceSize.w = binaryReader.ReadSingle();
						value.sourceSize.h = binaryReader.ReadSingle();
						value.pivot.x = binaryReader.ReadSingle();
						value.pivot.y = binaryReader.ReadSingle();
						emojiFrames.Add(key, value);
					}
					return;
				}
			}
			Debug.Log("OnPopulateMesh : asset is null!!!");
		}
		catch (Exception)
		{
			Debug.Log("OnPopulateMesh : exception!!!");
		}
	}

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		if (base.font == null)
		{
			return;
		}
		initEmoji();
		if (m_EmojiParsingRequired)
		{
			ParseText();
		}
		emojiDic.Clear();
		if (supportEmoji)
		{
			int num = 0;
			int num2 = 0;
			m_EmojiText = RegexWhitespace.Replace(m_EmojiText, "");
			MatchCollection matchCollection = Regex2.Matches(m_EmojiText);
			for (int i = 0; i < matchCollection.Count; i++)
			{
				if (matchCollection[i].Groups.Count > 1 && emojiFrames.TryGetValue(matchCollection[i].Groups[1].Value, out var value))
				{
					emojiDic.Add(matchCollection[i].Index - num2 + num, value);
					num2 += matchCollection[i].Length - 1;
					num++;
				}
			}
		}
		m_DisableFontTextureRebuiltCallback = true;
		Vector2 size = base.rectTransform.rect.size;
		TextGenerationSettings generationSettings = GetGenerationSettings(size);
		base.cachedTextGenerator.Populate(emojiText, generationSettings);
		IList<UIVertex> verts = base.cachedTextGenerator.verts;
		float num3 = 1f / base.pixelsPerUnit;
		int count = verts.Count;
		toFill.Clear();
		bool flag = false;
		for (int j = 0; j < count; j++)
		{
			int key = j / 4;
			if (emojiDic.TryGetValue(key, out var value2))
			{
				float num4 = 1.45f * (verts[j + 1].position.x - verts[j].position.x) * m_IconScaleOfDoubleSymbole;
				float num5 = verts[j + 1].position.y - verts[j + 2].position.y;
				float num6 = verts[j + 1].position.x - verts[j].position.x;
				float num7 = (num4 - num5) * 0.5f;
				float num8 = 4f + num4 * (1f - m_IconScaleOfDoubleSymbole);
				if (j > 4)
				{
					bool flag2 = Math.Abs(verts[j].position.y - verts[j - 4].position.y) > num5;
					if (flag && !flag2)
					{
						num8 -= verts[j].position.x - verts[j - 4].position.x;
					}
					else if (j + 4 < count)
					{
						flag = Math.Abs(verts[j].position.y - verts[j + 4].position.y) > num5;
					}
				}
				m_TempVerts[3] = verts[j];
				m_TempVerts[2] = verts[j + 1];
				m_TempVerts[1] = verts[j + 2];
				m_TempVerts[0] = verts[j + 3];
				m_TempVerts[0].position += new Vector3(num8, 0f - num7, 0f);
				m_TempVerts[1].position += new Vector3(num8 - num6 + num4, 0f - num7, 0f);
				m_TempVerts[2].position += new Vector3(num8 - num6 + num4, num7, 0f);
				m_TempVerts[3].position += new Vector3(num8, num7, 0f);
				m_TempVerts[0].position *= num3;
				m_TempVerts[1].position *= num3;
				m_TempVerts[2].position *= num3;
				m_TempVerts[3].position *= num3;
				float num9 = value2.frame.x / 2048f;
				float num10 = (2048f - value2.frame.y - 32f) / 2048f;
				float num11 = value2.sourceSize.w / 2048f;
				float num12 = num11 / 64f;
				m_TempVerts[0].uv1 = new Vector2(num9 + num12, num10 + num12);
				m_TempVerts[1].uv1 = new Vector2(num9 - num12 + num11, num10 + num12);
				m_TempVerts[2].uv1 = new Vector2(num9 - num12 + num11, num10 - num12 + num11);
				m_TempVerts[3].uv1 = new Vector2(num9 + num12, num10 - num12 + num11);
				toFill.AddUIVertexQuad(m_TempVerts);
				j += 7;
			}
			else
			{
				flag = false;
				int num13 = j & 3;
				m_TempVerts[num13] = verts[j];
				m_TempVerts[num13].position *= num3;
				if (num13 == 3)
				{
					toFill.AddUIVertexQuad(m_TempVerts);
				}
			}
		}
		m_DisableFontTextureRebuiltCallback = false;
	}

	private string ByteToBinStr(byte b)
	{
		if (b >= 0 && b <= 9)
		{
			return b2str_[b];
		}
		return b.ToString("x2");
	}

	private bool DoFirstParse_UTF32(string match_string, out string key)
	{
		key = "";
		int byteCount = Encoding.UTF32.GetByteCount(match_string);
		if (byteCount > utf32_bytes_.Length)
		{
			utf32_bytes_ = new byte[byteCount + 16];
		}
		int bytes = Encoding.UTF32.GetBytes(match_string, 0, match_string.Length, utf32_bytes_, 0);
		byte[] array = utf32_bytes_;
		string text = "";
		string text2 = "";
		switch (bytes)
		{
		case 4:
		{
			utf32_sb_.Clear();
			for (int j = 0; j < 4; j++)
			{
				string value2 = ByteToBinStr(array[j]);
				utf32_sb_.Insert(0, value2);
			}
			key = utf32_sb_.ToString();
			break;
		}
		case 8:
		{
			for (int k = 0; k < 4; k++)
			{
				text = ByteToBinStr(array[k]) + text;
			}
			for (int l = 4; l < 8; l++)
			{
				text2 = ByteToBinStr(array[l]) + text2;
			}
			key = text + text2;
			break;
		}
		default:
		{
			if (bytes % 4 != 0)
			{
				break;
			}
			string text3 = "";
			utf32_sb_.Clear();
			for (int i = 0; i < bytes; i++)
			{
				string value = ByteToBinStr(array[i]);
				utf32_sb_.Insert(0, value);
				if (i % 4 == 3)
				{
					text3 += utf32_sb_.ToString();
					utf32_sb_.Clear();
				}
			}
			key = text3;
			break;
		}
		}
		EmojiFrame value3;
		bool flag = emojiFrames.TryGetValue(key, out value3);
		if (!flag)
		{
			if (text == "" && text2 == "")
			{
				key += "0000fe0f";
				flag |= emojiFrames.TryGetValue(key, out value3);
			}
			else if (text == "" && text2 != "")
			{
				key = text2 + "0000fe0f";
				flag |= emojiFrames.TryGetValue(key, out value3);
			}
			else if (text2 == "0000fe0f")
			{
				key = text;
				flag |= emojiFrames.TryGetValue(key, out value3);
			}
			else if (text2 == "000020e3")
			{
				key = text + "0000fe0f" + text2;
				flag |= emojiFrames.TryGetValue(key, out value3);
			}
		}
		return flag;
	}

	private bool ParseText()
	{
		if (!supportEmoji)
		{
			m_EmojiParsingRequired = false;
			return false;
		}
		bool result = false;
		m_EmojiText = text;
		MatchCollection matchCollection = Regex1.Matches(text);
		if (matchCollection.Count == 0)
		{
			return result;
		}
		sb_.Clear();
		int num = 0;
		result = true;
		for (int i = 0; i < matchCollection.Count; i++)
		{
			if (!emojiFastSearch.TryGetValue(matchCollection[i].Value, out var value))
			{
				if (DoFirstParse_UTF32(matchCollection[i].Value, out value))
				{
					emojiFastSearch.Add(matchCollection[i].Value, value);
				}
				else
				{
					value = "00002753";
					emojiFastSearch.Add(matchCollection[i].Value, value);
				}
			}
			AppendString(num, m_EmojiText, matchCollection[i].Index, null);
			sb_.Append("<sprite=").Append(value).Append(">");
			num = matchCollection[i].Index + matchCollection[i].Length;
		}
		if (m_EmojiText.Length - num > 0)
		{
			sb_.Append(m_EmojiText, num, m_EmojiText.Length - num);
		}
		m_EmojiText = sb_.ToString();
		return result;
	}

	public UGUITextLines[] PopulateWithErrors(string finalTxt)
	{
		base.cachedTextGenerator.PopulateWithErrors(finalTxt, GetGenerationSettings(base.rectTransform.rect.size), base.gameObject);
		return GetArabicSupportLinesInfo(finalTxt);
	}

	private UGUITextLines[] GetArabicSupportLinesInfo(string input)
	{
		if (base.cachedTextGenerator.rectExtents.width < 1f || base.cachedTextGenerator.rectExtents.height < 1f)
		{
			UGUITextLines uGUITextLines = new UGUITextLines
			{
				startIndex = 0,
				endIndex = input.Length - 1
			};
			return new UGUITextLines[1] { uGUITextLines };
		}
		int count = base.cachedTextGenerator.lines.Count;
		UGUITextLines[] array = new UGUITextLines[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = new UGUITextLines();
			array[i].startIndex = base.cachedTextGenerator.lines[i].startCharIdx;
			array[i].endIndex = ((i == count - 1) ? (input.Length - 1) : Math.Max(0, base.cachedTextGenerator.lines[i + 1].startCharIdx - 1));
		}
		return array;
	}
}
