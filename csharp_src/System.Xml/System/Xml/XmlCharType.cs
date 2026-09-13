using System.Threading;

namespace System.Xml;

internal struct XmlCharType
{
	internal const int SurHighStart = 55296;

	internal const int SurHighEnd = 56319;

	internal const int SurLowStart = 56320;

	internal const int SurLowEnd = 57343;

	internal const int SurMask = 64512;

	internal const int fWhitespace = 1;

	internal const int fLetter = 2;

	internal const int fNCStartNameSC = 4;

	internal const int fNCNameSC = 8;

	internal const int fCharData = 16;

	internal const int fNCNameXml4e = 32;

	internal const int fText = 64;

	internal const int fAttrValue = 128;

	private const string s_PublicIdBitmap = "␀\0ﾻ꿿\uffff蟿\ufffe߿";

	private const uint CharPropertiesSize = 65536u;

	internal const string s_Whitespace = "\t\n\r\r  ";

	private const string s_NCStartName = "AZ__azÀÖØöøıĴľŁňŊžƀǃǍǰǴǵǺȗɐʨʻˁΆΆΈΊΌΌΎΡΣώϐϖϚϚϜϜϞϞϠϠϢϳЁЌЎяёќўҁҐӄӇӈӋӌӐӫӮӵӸӹԱՖՙՙաֆאתװײءغفيٱڷںھۀێېۓەەۥۦअहऽऽक़ॡঅঌএঐওনপরললশহড়ঢ়য়ৡৰৱਅਊਏਐਓਨਪਰਲਲ਼ਵਸ਼ਸਹਖ਼ੜਫ਼ਫ਼ੲੴઅઋઍઍએઑઓનપરલળવહઽઽૠૠଅଌଏଐଓନପରଲଳଶହଽଽଡ଼ଢ଼ୟୡஅஊஎஐஒகஙசஜஜஞடணதநபமவஷஹఅఌఎఐఒనపళవహౠౡಅಌಎಐಒನಪಳವಹೞೞೠೡഅഌഎഐഒനപഹൠൡกฮะะาำเๅກຂຄຄງຈຊຊຍຍດທນຟມຣລລວວສຫອຮະະາຳຽຽເໄཀཇཉཀྵႠჅაჶᄀᄀᄂᄃᄅᄇᄉᄉᄋᄌᄎᄒᄼᄼᄾᄾᅀᅀᅌᅌᅎᅎᅐᅐᅔᅕᅙᅙᅟᅡᅣᅣᅥᅥᅧᅧᅩᅩᅭᅮᅲᅳᅵᅵᆞᆞᆨᆨᆫᆫᆮᆯᆷᆸᆺᆺᆼᇂᇫᇫᇰᇰᇹᇹḀẛẠỹἀἕἘἝἠὅὈὍὐὗὙὙὛὛὝὝὟώᾀᾴᾶᾼιιῂῄῆῌῐΐῖΊῠῬῲῴῶῼΩΩKÅ℮℮ↀↂ〇〇〡〩ぁゔァヺㄅㄬ一龥가힣";

	private const string s_NCName = "-.09AZ__az··ÀÖØöøıĴľŁňŊžƀǃǍǰǴǵǺȗɐʨʻˁːˑ\u0300\u0345\u0360\u0361ΆΊΌΌΎΡΣώϐϖϚϚϜϜϞϞϠϠϢϳЁЌЎяёќўҁ\u0483\u0486ҐӄӇӈӋӌӐӫӮӵӸӹԱՖՙՙաֆ\u0591\u05a1\u05a3\u05b9\u05bb\u05bd\u05bf\u05bf\u05c1\u05c2\u05c4\u05c4אתװײءغـ\u0652٠٩\u0670ڷںھۀێېۓە\u06e8\u06ea\u06ed۰۹\u0901\u0903अह\u093c\u094d\u0951\u0954क़\u0963०९\u0981\u0983অঌএঐওনপরললশহ\u09bc\u09bc\u09be\u09c4\u09c7\u09c8\u09cb\u09cd\u09d7\u09d7ড়ঢ়য়\u09e3০ৱ\u0a02\u0a02ਅਊਏਐਓਨਪਰਲਲ਼ਵਸ਼ਸਹ\u0a3c\u0a3c\u0a3e\u0a42\u0a47\u0a48\u0a4b\u0a4dਖ਼ੜਫ਼ਫ਼੦ੴ\u0a81\u0a83અઋઍઍએઑઓનપરલળવહ\u0abc\u0ac5\u0ac7\u0ac9\u0acb\u0acdૠૠ૦૯\u0b01\u0b03ଅଌଏଐଓନପରଲଳଶହ\u0b3c\u0b43\u0b47\u0b48\u0b4b\u0b4d\u0b56\u0b57ଡ଼ଢ଼ୟୡ୦୯\u0b82ஃஅஊஎஐஒகஙசஜஜஞடணதநபமவஷஹ\u0bbe\u0bc2\u0bc6\u0bc8\u0bca\u0bcd\u0bd7\u0bd7௧௯\u0c01\u0c03అఌఎఐఒనపళవహ\u0c3e\u0c44\u0c46\u0c48\u0c4a\u0c4d\u0c55\u0c56ౠౡ౦౯\u0c82\u0c83ಅಌಎಐಒನಪಳವಹ\u0cbe\u0cc4\u0cc6\u0cc8\u0cca\u0ccd\u0cd5\u0cd6ೞೞೠೡ೦೯\u0d02\u0d03അഌഎഐഒനപഹ\u0d3e\u0d43\u0d46\u0d48\u0d4a\u0d4d\u0d57\u0d57ൠൡ൦൯กฮะ\u0e3aเ\u0e4e๐๙ກຂຄຄງຈຊຊຍຍດທນຟມຣລລວວສຫອຮະ\u0eb9\u0ebbຽເໄໆໆ\u0ec8\u0ecd໐໙\u0f18\u0f19༠༩\u0f35\u0f35\u0f37\u0f37\u0f39\u0f39\u0f3eཇཉཀྵ\u0f71\u0f84\u0f86ྋ\u0f90\u0f95\u0f97\u0f97\u0f99\u0fad\u0fb1\u0fb7\u0fb9\u0fb9ႠჅაჶᄀᄀᄂᄃᄅᄇᄉᄉᄋᄌᄎᄒᄼᄼᄾᄾᅀᅀᅌᅌᅎᅎᅐᅐᅔᅕᅙᅙᅟᅡᅣᅣᅥᅥᅧᅧᅩᅩᅭᅮᅲᅳᅵᅵᆞᆞᆨᆨᆫᆫᆮᆯᆷᆸᆺᆺᆼᇂᇫᇫᇰᇰᇹᇹḀẛẠỹἀἕἘἝἠὅὈὍὐὗὙὙὛὛὝὝὟώᾀᾴᾶᾼιιῂῄῆῌῐΐῖΊῠῬῲῴῶῼ\u20d0\u20dc\u20e1\u20e1ΩΩKÅ℮℮ↀↂ々々〇〇〡\u302f〱〵ぁゔ\u3099\u309aゝゞァヺーヾㄅㄬ一龥가힣";

	private const string s_CharData = "\t\n\r\r \ud7ff\ue000\ufffd";

	private const string s_PublicID = "\n\n\r\r !#%';==?Z__az";

	private const string s_Text = " %';=\\^\ud7ff\ue000\ufffd";

	private const string s_AttrValue = " !#%(;==?\ud7ff\ue000\ufffd";

	private const string s_LetterXml4e = "AZazÀÖØöøıĴľŁňŊžƀǃǍǰǴǵǺȗɐʨʻˁΆΆΈΊΌΌΎΡΣώϐϖϚϚϜϜϞϞϠϠϢϳЁЌЎяёќўҁҐӄӇӈӋӌӐӫӮӵӸӹԱՖՙՙաֆאתװײءغفيٱڷںھۀێېۓەەۥۦअहऽऽक़ॡঅঌএঐওনপরললশহড়ঢ়য়ৡৰৱਅਊਏਐਓਨਪਰਲਲ਼ਵਸ਼ਸਹਖ਼ੜਫ਼ਫ਼ੲੴઅઋઍઍએઑઓનપરલળવહઽઽૠૠଅଌଏଐଓନପରଲଳଶହଽଽଡ଼ଢ଼ୟୡஅஊஎஐஒகஙசஜஜஞடணதநபமவஷஹఅఌఎఐఒనపళవహౠౡಅಌಎಐಒನಪಳವಹೞೞೠೡഅഌഎഐഒനപഹൠൡกฮะะาำเๅກຂຄຄງຈຊຊຍຍດທນຟມຣລລວວສຫອຮະະາຳຽຽເໄཀཇཉཀྵႠჅაჶᄀᄀᄂᄃᄅᄇᄉᄉᄋᄌᄎᄒᄼᄼᄾᄾᅀᅀᅌᅌᅎᅎᅐᅐᅔᅕᅙᅙᅟᅡᅣᅣᅥᅥᅧᅧᅩᅩᅭᅮᅲᅳᅵᅵᆞᆞᆨᆨᆫᆫᆮᆯᆷᆸᆺᆺᆼᇂᇫᇫᇰᇰᇹᇹḀẛẠỹἀἕἘἝἠὅὈὍὐὗὙὙὛὛὝὝὟώᾀᾴᾶᾼιιῂῄῆῌῐΐῖΊῠῬῲῴῶῼΩΩKÅ℮℮ↀↂ〇〇〡〩ぁゔァヺㄅㄬ一龥가힣";

	private const string s_NCNameXml4e = "-.09AZ__az··ÀÖØöøıĴľŁňŊžƀǃǍǰǴǵǺȗɐʨʻˁːˑ\u0300\u0345\u0360\u0361ΆΊΌΌΎΡΣώϐϖϚϚϜϜϞϞϠϠϢϳЁЌЎяёќўҁ\u0483\u0486ҐӄӇӈӋӌӐӫӮӵӸӹԱՖՙՙաֆ\u0591\u05a1\u05a3\u05b9\u05bb\u05bd\u05bf\u05bf\u05c1\u05c2\u05c4\u05c4אתװײءغـ\u0652٠٩\u0670ڷںھۀێېۓە\u06e8\u06ea\u06ed۰۹\u0901\u0903अह\u093c\u094d\u0951\u0954क़\u0963०९\u0981\u0983অঌএঐওনপরললশহ\u09bc\u09bc\u09be\u09c4\u09c7\u09c8\u09cb\u09cd\u09d7\u09d7ড়ঢ়য়\u09e3০ৱ\u0a02\u0a02ਅਊਏਐਓਨਪਰਲਲ਼ਵਸ਼ਸਹ\u0a3c\u0a3c\u0a3e\u0a42\u0a47\u0a48\u0a4b\u0a4dਖ਼ੜਫ਼ਫ਼੦ੴ\u0a81\u0a83અઋઍઍએઑઓનપરલળવહ\u0abc\u0ac5\u0ac7\u0ac9\u0acb\u0acdૠૠ૦૯\u0b01\u0b03ଅଌଏଐଓନପରଲଳଶହ\u0b3c\u0b43\u0b47\u0b48\u0b4b\u0b4d\u0b56\u0b57ଡ଼ଢ଼ୟୡ୦୯\u0b82ஃஅஊஎஐஒகஙசஜஜஞடணதநபமவஷஹ\u0bbe\u0bc2\u0bc6\u0bc8\u0bca\u0bcd\u0bd7\u0bd7௧௯\u0c01\u0c03అఌఎఐఒనపళవహ\u0c3e\u0c44\u0c46\u0c48\u0c4a\u0c4d\u0c55\u0c56ౠౡ౦౯\u0c82\u0c83ಅಌಎಐಒನಪಳವಹ\u0cbe\u0cc4\u0cc6\u0cc8\u0cca\u0ccd\u0cd5\u0cd6ೞೞೠೡ೦೯\u0d02\u0d03അഌഎഐഒനപഹ\u0d3e\u0d43\u0d46\u0d48\u0d4a\u0d4d\u0d57\u0d57ൠൡ൦൯กฮะ\u0e3aเ\u0e4e๐๙ກຂຄຄງຈຊຊຍຍດທນຟມຣລລວວສຫອຮະ\u0eb9\u0ebbຽເໄໆໆ\u0ec8\u0ecd໐໙\u0f18\u0f19༠༩\u0f35\u0f35\u0f37\u0f37\u0f39\u0f39\u0f3eཇཉཀྵ\u0f71\u0f84\u0f86ྋ\u0f90\u0f95\u0f97\u0f97\u0f99\u0fad\u0fb1\u0fb7\u0fb9\u0fb9ႠჅაჶᄀᄀᄂᄃᄅᄇᄉᄉᄋᄌᄎᄒᄼᄼᄾᄾᅀᅀᅌᅌᅎᅎᅐᅐᅔᅕᅙᅙᅟᅡᅣᅣᅥᅥᅧᅧᅩᅩᅭᅮᅲᅳᅵᅵᆞᆞᆨᆨᆫᆫᆮᆯᆷᆸᆺᆺᆼᇂᇫᇫᇰᇰᇹᇹḀẛẠỹἀἕἘἝἠὅὈὍὐὗὙὙὛὛὝὝὟώᾀᾴᾶᾼιιῂῄῆῌῐΐῖΊῠῬῲῴῶῼ\u20d0\u20dc\u20e1\u20e1ΩΩKÅ℮℮ↀↂ々々〇〇〡\u302f〱〵ぁゔ\u3099\u309aゝゞァヺーヾㄅㄬ一龥가힣";

	private static object s_Lock;

	private static volatile byte[] s_CharProperties;

	internal byte[] charProperties;

	private static object StaticLock
	{
		get
		{
			if (s_Lock == null)
			{
				object value = new object();
				Interlocked.CompareExchange<object>(ref s_Lock, value, (object)null);
			}
			return s_Lock;
		}
	}

	public static XmlCharType Instance
	{
		get
		{
			if (s_CharProperties == null)
			{
				InitInstance();
			}
			return new XmlCharType(s_CharProperties);
		}
	}

	private static void InitInstance()
	{
		lock (StaticLock)
		{
			if (s_CharProperties == null)
			{
				byte[] chProps = new byte[65536];
				SetProperties(chProps, "\t\n\r\r  ", 1);
				SetProperties(chProps, "AZazÀÖØöøıĴľŁňŊžƀǃǍǰǴǵǺȗɐʨʻˁΆΆΈΊΌΌΎΡΣώϐϖϚϚϜϜϞϞϠϠϢϳЁЌЎяёќўҁҐӄӇӈӋӌӐӫӮӵӸӹԱՖՙՙաֆאתװײءغفيٱڷںھۀێېۓەەۥۦअहऽऽक़ॡঅঌএঐওনপরললশহড়ঢ়য়ৡৰৱਅਊਏਐਓਨਪਰਲਲ਼ਵਸ਼ਸਹਖ਼ੜਫ਼ਫ਼ੲੴઅઋઍઍએઑઓનપરલળવહઽઽૠૠଅଌଏଐଓନପରଲଳଶହଽଽଡ଼ଢ଼ୟୡஅஊஎஐஒகஙசஜஜஞடணதநபமவஷஹఅఌఎఐఒనపళవహౠౡಅಌಎಐಒನಪಳವಹೞೞೠೡഅഌഎഐഒനപഹൠൡกฮะะาำเๅກຂຄຄງຈຊຊຍຍດທນຟມຣລລວວສຫອຮະະາຳຽຽເໄཀཇཉཀྵႠჅაჶᄀᄀᄂᄃᄅᄇᄉᄉᄋᄌᄎᄒᄼᄼᄾᄾᅀᅀᅌᅌᅎᅎᅐᅐᅔᅕᅙᅙᅟᅡᅣᅣᅥᅥᅧᅧᅩᅩᅭᅮᅲᅳᅵᅵᆞᆞᆨᆨᆫᆫᆮᆯᆷᆸᆺᆺᆼᇂᇫᇫᇰᇰᇹᇹḀẛẠỹἀἕἘἝἠὅὈὍὐὗὙὙὛὛὝὝὟώᾀᾴᾶᾼιιῂῄῆῌῐΐῖΊῠῬῲῴῶῼΩΩKÅ℮℮ↀↂ〇〇〡〩ぁゔァヺㄅㄬ一龥가힣", 2);
				SetProperties(chProps, "AZ__azÀÖØöøıĴľŁňŊžƀǃǍǰǴǵǺȗɐʨʻˁΆΆΈΊΌΌΎΡΣώϐϖϚϚϜϜϞϞϠϠϢϳЁЌЎяёќўҁҐӄӇӈӋӌӐӫӮӵӸӹԱՖՙՙաֆאתװײءغفيٱڷںھۀێېۓەەۥۦअहऽऽक़ॡঅঌএঐওনপরললশহড়ঢ়য়ৡৰৱਅਊਏਐਓਨਪਰਲਲ਼ਵਸ਼ਸਹਖ਼ੜਫ਼ਫ਼ੲੴઅઋઍઍએઑઓનપરલળવહઽઽૠૠଅଌଏଐଓନପରଲଳଶହଽଽଡ଼ଢ଼ୟୡஅஊஎஐஒகஙசஜஜஞடணதநபமவஷஹఅఌఎఐఒనపళవహౠౡಅಌಎಐಒನಪಳವಹೞೞೠೡഅഌഎഐഒനപഹൠൡกฮะะาำเๅກຂຄຄງຈຊຊຍຍດທນຟມຣລລວວສຫອຮະະາຳຽຽເໄཀཇཉཀྵႠჅაჶᄀᄀᄂᄃᄅᄇᄉᄉᄋᄌᄎᄒᄼᄼᄾᄾᅀᅀᅌᅌᅎᅎᅐᅐᅔᅕᅙᅙᅟᅡᅣᅣᅥᅥᅧᅧᅩᅩᅭᅮᅲᅳᅵᅵᆞᆞᆨᆨᆫᆫᆮᆯᆷᆸᆺᆺᆼᇂᇫᇫᇰᇰᇹᇹḀẛẠỹἀἕἘἝἠὅὈὍὐὗὙὙὛὛὝὝὟώᾀᾴᾶᾼιιῂῄῆῌῐΐῖΊῠῬῲῴῶῼΩΩKÅ℮℮ↀↂ〇〇〡〩ぁゔァヺㄅㄬ一龥가힣", 4);
				SetProperties(chProps, "-.09AZ__az··ÀÖØöøıĴľŁňŊžƀǃǍǰǴǵǺȗɐʨʻˁːˑ\u0300\u0345\u0360\u0361ΆΊΌΌΎΡΣώϐϖϚϚϜϜϞϞϠϠϢϳЁЌЎяёќўҁ\u0483\u0486ҐӄӇӈӋӌӐӫӮӵӸӹԱՖՙՙաֆ\u0591\u05a1\u05a3\u05b9\u05bb\u05bd\u05bf\u05bf\u05c1\u05c2\u05c4\u05c4אתװײءغـ\u0652٠٩\u0670ڷںھۀێېۓە\u06e8\u06ea\u06ed۰۹\u0901\u0903अह\u093c\u094d\u0951\u0954क़\u0963०९\u0981\u0983অঌএঐওনপরললশহ\u09bc\u09bc\u09be\u09c4\u09c7\u09c8\u09cb\u09cd\u09d7\u09d7ড়ঢ়য়\u09e3০ৱ\u0a02\u0a02ਅਊਏਐਓਨਪਰਲਲ਼ਵਸ਼ਸਹ\u0a3c\u0a3c\u0a3e\u0a42\u0a47\u0a48\u0a4b\u0a4dਖ਼ੜਫ਼ਫ਼੦ੴ\u0a81\u0a83અઋઍઍએઑઓનપરલળવહ\u0abc\u0ac5\u0ac7\u0ac9\u0acb\u0acdૠૠ૦૯\u0b01\u0b03ଅଌଏଐଓନପରଲଳଶହ\u0b3c\u0b43\u0b47\u0b48\u0b4b\u0b4d\u0b56\u0b57ଡ଼ଢ଼ୟୡ୦୯\u0b82ஃஅஊஎஐஒகஙசஜஜஞடணதநபமவஷஹ\u0bbe\u0bc2\u0bc6\u0bc8\u0bca\u0bcd\u0bd7\u0bd7௧௯\u0c01\u0c03అఌఎఐఒనపళవహ\u0c3e\u0c44\u0c46\u0c48\u0c4a\u0c4d\u0c55\u0c56ౠౡ౦౯\u0c82\u0c83ಅಌಎಐಒನಪಳವಹ\u0cbe\u0cc4\u0cc6\u0cc8\u0cca\u0ccd\u0cd5\u0cd6ೞೞೠೡ೦೯\u0d02\u0d03അഌഎഐഒനപഹ\u0d3e\u0d43\u0d46\u0d48\u0d4a\u0d4d\u0d57\u0d57ൠൡ൦൯กฮะ\u0e3aเ\u0e4e๐๙ກຂຄຄງຈຊຊຍຍດທນຟມຣລລວວສຫອຮະ\u0eb9\u0ebbຽເໄໆໆ\u0ec8\u0ecd໐໙\u0f18\u0f19༠༩\u0f35\u0f35\u0f37\u0f37\u0f39\u0f39\u0f3eཇཉཀྵ\u0f71\u0f84\u0f86ྋ\u0f90\u0f95\u0f97\u0f97\u0f99\u0fad\u0fb1\u0fb7\u0fb9\u0fb9ႠჅაჶᄀᄀᄂᄃᄅᄇᄉᄉᄋᄌᄎᄒᄼᄼᄾᄾᅀᅀᅌᅌᅎᅎᅐᅐᅔᅕᅙᅙᅟᅡᅣᅣᅥᅥᅧᅧᅩᅩᅭᅮᅲᅳᅵᅵᆞᆞᆨᆨᆫᆫᆮᆯᆷᆸᆺᆺᆼᇂᇫᇫᇰᇰᇹᇹḀẛẠỹἀἕἘἝἠὅὈὍὐὗὙὙὛὛὝὝὟώᾀᾴᾶᾼιιῂῄῆῌῐΐῖΊῠῬῲῴῶῼ\u20d0\u20dc\u20e1\u20e1ΩΩKÅ℮℮ↀↂ々々〇〇〡\u302f〱〵ぁゔ\u3099\u309aゝゞァヺーヾㄅㄬ一龥가힣", 8);
				SetProperties(chProps, "\t\n\r\r \ud7ff\ue000\ufffd", 16);
				SetProperties(chProps, "-.09AZ__az··ÀÖØöøıĴľŁňŊžƀǃǍǰǴǵǺȗɐʨʻˁːˑ\u0300\u0345\u0360\u0361ΆΊΌΌΎΡΣώϐϖϚϚϜϜϞϞϠϠϢϳЁЌЎяёќўҁ\u0483\u0486ҐӄӇӈӋӌӐӫӮӵӸӹԱՖՙՙաֆ\u0591\u05a1\u05a3\u05b9\u05bb\u05bd\u05bf\u05bf\u05c1\u05c2\u05c4\u05c4אתװײءغـ\u0652٠٩\u0670ڷںھۀێېۓە\u06e8\u06ea\u06ed۰۹\u0901\u0903अह\u093c\u094d\u0951\u0954क़\u0963०९\u0981\u0983অঌএঐওনপরললশহ\u09bc\u09bc\u09be\u09c4\u09c7\u09c8\u09cb\u09cd\u09d7\u09d7ড়ঢ়য়\u09e3০ৱ\u0a02\u0a02ਅਊਏਐਓਨਪਰਲਲ਼ਵਸ਼ਸਹ\u0a3c\u0a3c\u0a3e\u0a42\u0a47\u0a48\u0a4b\u0a4dਖ਼ੜਫ਼ਫ਼੦ੴ\u0a81\u0a83અઋઍઍએઑઓનપરલળવહ\u0abc\u0ac5\u0ac7\u0ac9\u0acb\u0acdૠૠ૦૯\u0b01\u0b03ଅଌଏଐଓନପରଲଳଶହ\u0b3c\u0b43\u0b47\u0b48\u0b4b\u0b4d\u0b56\u0b57ଡ଼ଢ଼ୟୡ୦୯\u0b82ஃஅஊஎஐஒகஙசஜஜஞடணதநபமவஷஹ\u0bbe\u0bc2\u0bc6\u0bc8\u0bca\u0bcd\u0bd7\u0bd7௧௯\u0c01\u0c03అఌఎఐఒనపళవహ\u0c3e\u0c44\u0c46\u0c48\u0c4a\u0c4d\u0c55\u0c56ౠౡ౦౯\u0c82\u0c83ಅಌಎಐಒನಪಳವಹ\u0cbe\u0cc4\u0cc6\u0cc8\u0cca\u0ccd\u0cd5\u0cd6ೞೞೠೡ೦೯\u0d02\u0d03അഌഎഐഒനപഹ\u0d3e\u0d43\u0d46\u0d48\u0d4a\u0d4d\u0d57\u0d57ൠൡ൦൯กฮะ\u0e3aเ\u0e4e๐๙ກຂຄຄງຈຊຊຍຍດທນຟມຣລລວວສຫອຮະ\u0eb9\u0ebbຽເໄໆໆ\u0ec8\u0ecd໐໙\u0f18\u0f19༠༩\u0f35\u0f35\u0f37\u0f37\u0f39\u0f39\u0f3eཇཉཀྵ\u0f71\u0f84\u0f86ྋ\u0f90\u0f95\u0f97\u0f97\u0f99\u0fad\u0fb1\u0fb7\u0fb9\u0fb9ႠჅაჶᄀᄀᄂᄃᄅᄇᄉᄉᄋᄌᄎᄒᄼᄼᄾᄾᅀᅀᅌᅌᅎᅎᅐᅐᅔᅕᅙᅙᅟᅡᅣᅣᅥᅥᅧᅧᅩᅩᅭᅮᅲᅳᅵᅵᆞᆞᆨᆨᆫᆫᆮᆯᆷᆸᆺᆺᆼᇂᇫᇫᇰᇰᇹᇹḀẛẠỹἀἕἘἝἠὅὈὍὐὗὙὙὛὛὝὝὟώᾀᾴᾶᾼιιῂῄῆῌῐΐῖΊῠῬῲῴῶῼ\u20d0\u20dc\u20e1\u20e1ΩΩKÅ℮℮ↀↂ々々〇〇〡\u302f〱〵ぁゔ\u3099\u309aゝゞァヺーヾㄅㄬ一龥가힣", 32);
				SetProperties(chProps, " %';=\\^\ud7ff\ue000\ufffd", 64);
				SetProperties(chProps, " !#%(;==?\ud7ff\ue000\ufffd", 128);
				Thread.MemoryBarrier();
				s_CharProperties = chProps;
			}
		}
	}

	private static void SetProperties(byte[] chProps, string ranges, byte value)
	{
		for (int i = 0; i < ranges.Length; i += 2)
		{
			int j = ranges[i];
			for (int num = ranges[i + 1]; j <= num; j++)
			{
				chProps[j] |= value;
			}
		}
	}

	private XmlCharType(byte[] charProperties)
	{
		this.charProperties = charProperties;
	}

	public bool IsWhiteSpace(char ch)
	{
		return (charProperties[(uint)ch] & 1) != 0;
	}

	public bool IsExtender(char ch)
	{
		return ch == '·';
	}

	public bool IsNCNameSingleChar(char ch)
	{
		return (charProperties[(uint)ch] & 8) != 0;
	}

	public bool IsStartNCNameSingleChar(char ch)
	{
		return (charProperties[(uint)ch] & 4) != 0;
	}

	public bool IsNameSingleChar(char ch)
	{
		if (!IsNCNameSingleChar(ch))
		{
			return ch == ':';
		}
		return true;
	}

	public bool IsStartNameSingleChar(char ch)
	{
		if (!IsStartNCNameSingleChar(ch))
		{
			return ch == ':';
		}
		return true;
	}

	public bool IsCharData(char ch)
	{
		return (charProperties[(uint)ch] & 0x10) != 0;
	}

	public bool IsPubidChar(char ch)
	{
		if (ch < '\u0080')
		{
			return ("␀\0ﾻ꿿\uffff蟿\ufffe߿"[(int)ch >> 4] & (1 << (ch & 0xF))) != 0;
		}
		return false;
	}

	internal bool IsTextChar(char ch)
	{
		return (charProperties[(uint)ch] & 0x40) != 0;
	}

	internal bool IsAttributeValueChar(char ch)
	{
		return (charProperties[(uint)ch] & 0x80) != 0;
	}

	public bool IsLetter(char ch)
	{
		return (charProperties[(uint)ch] & 2) != 0;
	}

	public bool IsNCNameCharXml4e(char ch)
	{
		return (charProperties[(uint)ch] & 0x20) != 0;
	}

	public bool IsStartNCNameCharXml4e(char ch)
	{
		if (!IsLetter(ch))
		{
			return ch == '_';
		}
		return true;
	}

	public bool IsNameCharXml4e(char ch)
	{
		if (!IsNCNameCharXml4e(ch))
		{
			return ch == ':';
		}
		return true;
	}

	public bool IsStartNameCharXml4e(char ch)
	{
		if (!IsStartNCNameCharXml4e(ch))
		{
			return ch == ':';
		}
		return true;
	}

	public static bool IsDigit(char ch)
	{
		return InRange(ch, 48, 57);
	}

	public static bool IsHexDigit(char ch)
	{
		if (!InRange(ch, 48, 57) && !InRange(ch, 97, 102))
		{
			return InRange(ch, 65, 70);
		}
		return true;
	}

	internal static bool IsHighSurrogate(int ch)
	{
		return InRange(ch, 55296, 56319);
	}

	internal static bool IsLowSurrogate(int ch)
	{
		return InRange(ch, 56320, 57343);
	}

	internal static bool IsSurrogate(int ch)
	{
		return InRange(ch, 55296, 57343);
	}

	internal static int CombineSurrogateChar(int lowChar, int highChar)
	{
		return (lowChar - 56320) | ((highChar - 55296 << 10) + 65536);
	}

	internal static void SplitSurrogateChar(int combinedChar, out char lowChar, out char highChar)
	{
		int num = combinedChar - 65536;
		lowChar = (char)(56320 + num % 1024);
		highChar = (char)(55296 + num / 1024);
	}

	internal bool IsOnlyWhitespace(string str)
	{
		return IsOnlyWhitespaceWithPos(str) == -1;
	}

	internal int IsOnlyWhitespaceWithPos(string str)
	{
		if (str != null)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if ((charProperties[(uint)str[i]] & 1) == 0)
				{
					return i;
				}
			}
		}
		return -1;
	}

	internal int IsOnlyCharData(string str)
	{
		if (str != null)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if ((charProperties[(uint)str[i]] & 0x10) == 0)
				{
					if (i + 1 >= str.Length || !IsHighSurrogate(str[i]) || !IsLowSurrogate(str[i + 1]))
					{
						return i;
					}
					i++;
				}
			}
		}
		return -1;
	}

	internal static bool IsOnlyDigits(string str, int startPos, int len)
	{
		for (int i = startPos; i < startPos + len; i++)
		{
			if (!IsDigit(str[i]))
			{
				return false;
			}
		}
		return true;
	}

	internal static bool IsOnlyDigits(char[] chars, int startPos, int len)
	{
		for (int i = startPos; i < startPos + len; i++)
		{
			if (!IsDigit(chars[i]))
			{
				return false;
			}
		}
		return true;
	}

	internal int IsPublicId(string str)
	{
		if (str != null)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if (!IsPubidChar(str[i]))
				{
					return i;
				}
			}
		}
		return -1;
	}

	private static bool InRange(int value, int start, int end)
	{
		return (uint)(value - start) <= (uint)(end - start);
	}
}
