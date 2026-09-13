namespace RTLTMPro;

public struct TashkeelLocation
{
	public char Tashkeel { get; set; }

	public int Position { get; set; }

	public TashkeelLocation(TashkeelCharacters tashkeel, int position)
	{
		this = default(TashkeelLocation);
		Tashkeel = (char)tashkeel;
		Position = position;
	}
}
