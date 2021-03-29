using System.Globalization;

namespace Preference.IntelliText
{
	public class TextSettings
	{
		public string Template { get; set; }
		public string Audience { get; internal set; }
		public CultureInfo Culture { get; set; }
        public string PathTemplates { get; internal set; }
        public string DataProvider { get; internal set; }
        public string PathDataProvider { get; internal set; }
        public string Render { get; internal set; }
        public string SaveTo { get; internal set; }
        public string TextGeneratorProvider { get; internal set; }
        public string PathTextGeneratorProvider { get; internal set; }
    }
}