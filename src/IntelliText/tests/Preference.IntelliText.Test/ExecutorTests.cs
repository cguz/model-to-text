namespace Preference.IntelliText.Test
{
    using Moq;
    using Preference.IntelliText;
    using Preference.IntelliText.Common;
    using System.Globalization;
    using System.IO;
    using Xunit;

    public class PreferenceIntelliTextExecutorTests
    {
        private MockRepository mockRepository;

        public PreferenceIntelliTextExecutorTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            // remove the output file
            // File.WriteAllText(OutputFile, "");
        }


        [Theory]
        [InlineData("WB-DE-OneDescription-UseCase1.txt", "WB-OneDescription-UseCase1", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-OneDescription-UseCase1.xml", "TXT", "de-DE")]
        [InlineData("WB-EN-OneDescription-UseCase1.txt", "WB-OneDescription-UseCase1", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-OneDescription-UseCase1.xml", "TXT", "en-EN")]
        [InlineData("WB-ES-OneDescription-UseCase1.txt", "WB-OneDescription-UseCase1", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-OneDescription-UseCase1.xml", "TXT", "es-ES")]
        [InlineData("WB-DE-SeveralDescriptions-UseCase2.txt", "WB-SeveralDescriptions-UseCase2", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-SeveralDescriptions-UseCase2.xml", "TXT", "de-DE")]
        [InlineData("WB-EN-SeveralDescriptions-UseCase2.txt", "WB-SeveralDescriptions-UseCase2", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-SeveralDescriptions-UseCase2.xml", "TXT", "en-EN")]
        [InlineData("WB-ES-SeveralDescriptions-UseCase2.txt", "WB-SeveralDescriptions-UseCase2", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-SeveralDescriptions-UseCase2.xml", "TXT", "es-ES")]
        [InlineData("WB-DE-SeveralDescriptionsPrice-UseCase3.txt", "WB-SeveralDescriptionsPrice-UseCase3", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-SeveralDescriptionsPrice-UseCase3.xml", "TXT", "de-DE")]
        [InlineData("WB-EN-SeveralDescriptionsPrice-UseCase3.txt", "WB-SeveralDescriptionsPrice-UseCase3", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-SeveralDescriptionsPrice-UseCase3.xml", "TXT", "en-EN")]
        [InlineData("WB-ES-SeveralDescriptionsPrice-UseCase3.txt", "WB-SeveralDescriptionsPrice-UseCase3", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-SeveralDescriptionsPrice-UseCase3.xml", "TXT", "es-ES")]
        [InlineData("WB-DE-SeveralDescriptionsPriceGrouped-UseCase4.txt", "WB-SeveralDescriptionPriceGrouped-UseCase4", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-SeveralDescriptionsPriceGrouped-UseCase4.xml", "TXT", "de-DE")]
        [InlineData("WB-EN-SeveralDescriptionsPriceGrouped-UseCase4.txt", "WB-SeveralDescriptionPriceGrouped-UseCase4", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-SeveralDescriptionsPriceGrouped-UseCase4.xml", "TXT", "en-EN")]
        [InlineData("WB-ES-SeveralDescriptionsPriceGrouped-UseCase4.txt", "WB-SeveralDescriptionPriceGrouped-UseCase4", "FILE", "Consumer", "FILE", "./xml-descriptive/WB-SeveralDescriptionsPriceGrouped-UseCase4.xml", "TXT", "es-ES")]
        public void IntelliTextExecutorTest(string OutputFile, string template, string automaticTextGeneratorProvider, string audience, string dataProvider, string pathDataProvider, string render, string language)
        {
            // Arrange
            TextSettings settings = new();

            // set the template id, audience and language
            settings.Template = template;

            settings.TextGeneratorProvider = automaticTextGeneratorProvider;

            settings.PathTextGeneratorProvider = Path.GetFullPath("./text-generator-files/");

            settings.Audience = audience;

            settings.Culture = new(language, false);

            settings.PathTemplates = Path.GetFullPath("./catalogs/company/");

            settings.DataProvider = dataProvider;

            settings.PathDataProvider = Path.GetFullPath(pathDataProvider);

            settings.Render = render;

            settings.SaveTo = OutputFile;

            // IntelliText creator receives the catalog constructor, the render to use, the data provider to use
            PreferenceIntelliText intelliText = new(settings);

            // build the required information: template, render, and structure
            intelliText.Build();

            // Act
            intelliText.Execute();

            // Assert
            Assert.True(File.Exists(settings.SaveTo));
            this.mockRepository.VerifyAll();
        }
    }
}
