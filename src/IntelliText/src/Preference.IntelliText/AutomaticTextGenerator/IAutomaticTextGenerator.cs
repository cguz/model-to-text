namespace Preference.IntelliText.AutomaticTextGenerator
{
	public interface IAutomaticTextGenerator
	{
		string GetText(ObjectId id, TextSettings settings);

		string GetText(ObjectId id);
	}
}